//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//
// Please direct any bugs/comments/suggestions to http://www.flipwebapps.com
// 
// The copyright owner grants to the end user a non-exclusive, worldwide, and perpetual license to this Asset
// to integrate only as incorporated and embedded components of electronic games and interactive media and 
// distribute such electronic game and interactive media. End user may modify Assets. End user may otherwise 
// not reproduce, distribute, sublicense, rent, lease or lend the Assets. It is emphasized that the end 
// user shall not be entitled to distribute or transfer in any way (including, without, limitation by way of 
// sublicense) the Assets in any other way than as integrated components of electronic games and interactive media. 

// The above copyright notice and this permission notice must not be removed from any files.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//----------------------------------------------

using System;
using FlipWebApps.GameFramework.Scripts.GameObjects.Components;
using FlipWebApps.GameFramework.Scripts.GameStructure;
using FlipWebApps.GameFramework.Scripts.Localisation;
using FlipWebApps.GameFramework.Scripts.UI.Dialogs.Components;
using UnityEngine;
using FlipWebApps.GameFramework.Scripts.Debugging;

namespace FlipWebApps.GameFramework.Scripts.FreePrize.Components
{
    [AddComponentMenu("Game Framework/FreePrize/FreePrizeManager")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class FreePrizeManager : SingletonPersistantSavedState<FreePrizeManager>
    {
        [Header("Time")]
        [Tooltip("The delay in seconds before starting the next countdown. 0 = no wait")]
        public MinMax DelayRangeToNextCountdown = new MinMax {Min= 0, Max = 0};       // wait range before starting next countdown. 0 = no wait
        [Tooltip("The time in seconds before another prize becomes available.")]
        public MinMax TimeRangeToNextPrize = new MinMax { Min = 600, Max = 1800 };    // countdown range to next prize. 10 minutes to 30 minutes
        [Tooltip("Whether to save times for the next prize becoming available across game restarts.")]
        public bool SaveAcrossRestarts;

        [Header("Prize")]
        [Tooltip("A minimum and maxximum value for the prize.")]
        public MinMax ValueRange = new MinMax { Min = 10, Max = 20 };                // value defaults

        [Header("Free Prize Dialog")]
        [Tooltip("An optional audio clip to play when the free prize window is closed.")]
        public AudioClip PrizeDialogClosedAudioClip;
        [Tooltip("An optional prefab to use for displaying custom content in the free prize window.")]
        public GameObject ContentPrefab;
        [Tooltip("An optional animation controller to animate the free prize window content.")]
        public RuntimeAnimatorController ContentAnimatorController;
        [Tooltip("DoesnWhether the content shows the dialog buttons. Setting this hides the dialog buttons so that they can be displayed at the appropriate point e.g. after an animation has played.")]
        public bool ContentShowsButtons;

        /// <summary>
        /// DateTime then the next free prize countdown should start
        /// </summary>
        public DateTime NextCountdownStart { get; set; }

        /// <summary>
        /// DateTime then the free prize will become available
        /// </summary>
        public DateTime NextFreePrizeAvailable { get; set; }

        /// <summary>
        /// The current prize amount.
        /// </summary>
        public int CurrentPrizeAmount {get; set; }

        /// <summary>
        /// Called from singletong Awake() - Load saved prize times, or setup new if first run or not saving across restarts
        /// </summary>
        protected override void GameSetup()
        {
            base.GameSetup();

            if (SaveAcrossRestarts && PlayerPrefs.HasKey("FreePrize.NextCountdownStart"))
            {
                NextCountdownStart = DateTime.Parse(PlayerPrefs.GetString("FreePrize.NextCountdownStart", DateTime.UtcNow.ToString())); // start countdown immediately if new game
                NextFreePrizeAvailable = DateTime.Parse(PlayerPrefs.GetString("FreePrize.NextPrize", NextFreePrizeAvailable.ToString()));
            }
            else
            {
                StartNewCountdown();
            }
        }

        /// <summary>
        /// Save the current state including free prize times
        /// </summary>
        public override void SaveState()
        {
            MyDebug.Log("FreePrizeManager: SaveState");

            PlayerPrefs.SetString("FreePrize.NextCountdownStart", NextCountdownStart.ToString());
            PlayerPrefs.SetString("FreePrize.NextPrize", NextFreePrizeAvailable.ToString());
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Make the free prize immediately available
        /// </summary>
        public void MakePrizeAvailable()
        {
            NextCountdownStart = DateTime.UtcNow;
            NextFreePrizeAvailable = DateTime.UtcNow;

            SaveState();
        }

        public void StartNewCountdown()
        {
            CurrentPrizeAmount = UnityEngine.Random.Range(ValueRange.Min, ValueRange.Max);

            NextCountdownStart = DateTime.UtcNow.AddSeconds(UnityEngine.Random.Range(DelayRangeToNextCountdown.Min, DelayRangeToNextCountdown.Max + 1));

            NextFreePrizeAvailable = NextCountdownStart.AddSeconds(UnityEngine.Random.Range(TimeRangeToNextPrize.Min, TimeRangeToNextPrize.Max + 1));

            SaveState();
        }

        public bool IsCountingDown()
        {
            return GetTimeToPrize().TotalSeconds > 0 && GetTimeToCountdown().TotalSeconds <= 0;
        }

        public bool IsPrizeAvailable()
        {
            return GetTimeToPrize().TotalSeconds <= 0;
        }

        TimeSpan GetTimeToCountdown()
        {
            return NextCountdownStart.Subtract(DateTime.UtcNow);
        }

        public TimeSpan GetTimeToPrize()
        {
            return NextFreePrizeAvailable.Subtract(DateTime.UtcNow);
        }

        /// <summary>
        /// Free prize dialog that giges the user coins. We default to the standard General Message window, adding any additional
        /// content as setup in the FreePrizeManager configuration.
        /// </summary>
        public void ShowFreePrizeDialog()
        {
            var dialogInstance = DialogManager.Instance.Create(null, null, ContentPrefab, null, runtimeAnimatorController: ContentAnimatorController);
            string text = LocaliseText.Format("FreePrize.Text1", CurrentPrizeAmount);

            dialogInstance.Show(title: LocaliseText.Get("FreePrize.Title"), text: text, text2Key: "FreePrize.Text2", doneCallback: ShowFreePrizeDone, dialogButtons: ContentShowsButtons ? DialogInstance.DialogButtonsType.Custom : DialogInstance.DialogButtonsType.Ok);
        }

        public virtual void ShowFreePrizeDone(DialogInstance dialogInstance)
        {
            GameManager.Instance.GetPlayer().Coins += CurrentPrizeAmount;
            if (PrizeDialogClosedAudioClip != null)
                GameManager.Instance.PlayEffect(PrizeDialogClosedAudioClip);
            GameManager.Instance.Player.UpdatePlayerPrefs();

            StartNewCountdown();
        }

        [Serializable]
        public class MinMax
        {
            public int Min;
            public int Max;
            public float Difference { get { return Max - Min; }}
        }
    }
}
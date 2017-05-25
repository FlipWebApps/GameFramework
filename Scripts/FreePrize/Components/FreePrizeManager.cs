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
using System.Globalization;
using GameFramework.GameObjects.Components;
using GameFramework.GameStructure;
using GameFramework.Localisation;
using GameFramework.UI.Dialogs.Components;
using UnityEngine;
using GameFramework.Debugging;
using GameFramework.Preferences;

namespace GameFramework.FreePrize.Components
{
    /// <summary>
    /// Manager class for handling the Free Prize status and configuration. Add this to a persistant gameobject in your scene if you are using Free Prize functionality
    /// </summary>
    [AddComponentMenu("Game Framework/FreePrize/FreePrizeManager")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class FreePrizeManager : SingletonPersistant<FreePrizeManager>
    {
        /// <summary>
        /// The delay in seconds before starting the next countdown. 0 = no wait
        /// </summary>
        [Header("Time")]
        [Tooltip("The delay in seconds before starting the next countdown. 0 = no wait")]
        public MinMax DelayRangeToNextCountdown = new MinMax {Min= 0, Max = 0};       // wait range before starting next countdown. 0 = no wait

        /// <summary>
        /// The time in seconds before another prize becomes available.
        /// </summary>
        [Tooltip("The time in seconds before another prize becomes available.")]
        public MinMax TimeRangeToNextPrize = new MinMax { Min = 600, Max = 1800 };    // countdown range to next prize. 10 minutes to 30 minutes

        /// <summary>
        /// Whether to save times for the next prize becoming available across game restarts.
        /// </summary>
        [Tooltip("Whether to save times for the next prize becoming available across game restarts.")]
        public bool SaveAcrossRestarts;


        /// <summary>
        /// A minimum and maxximum value for the prize.
        /// </summary>
        [Header("Prize")]
        [Tooltip("A minimum and maxximum value for the prize.")]
        public MinMax ValueRange = new MinMax { Min = 10, Max = 20 };                // value defaults


        /// <summary>
        /// An optional audio clip to play when the free prize window is closed.
        /// </summary>
        [Header("Free Prize Dialog")]
        [Tooltip("An optional audio clip to play when the free prize window is closed.")]
        public AudioClip PrizeDialogClosedAudioClip;

        /// <summary>
        /// An optional prefab to use for displaying custom content in the free prize window.
        /// </summary>
        [Tooltip("An optional prefab to use for displaying custom content in the free prize window.")]
        public GameObject ContentPrefab;

        /// <summary>
        /// An optional animation controller to animate the free prize window content.
        /// </summary>
        [Tooltip("An optional animation controller to animate the free prize window content.")]
        public RuntimeAnimatorController ContentAnimatorController;

        /// <summary>
        /// Whether the content shows the dialog buttons. Setting this hides the dialog buttons so that they can be displayed at the appropriate point e.g. after an animation has played.
        /// </summary>
        [Tooltip("Whether the content shows the dialog buttons. Setting this hides the dialog buttons so that they can be displayed at the appropriate point e.g. after an animation has played.")]
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
        /// True when teh Free Prize Dialog is shown otherwise false
        /// </summary>
        public bool IsShowingFreePrizeDialog { get; set; }

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

            if (SaveAcrossRestarts && PreferencesFactory.HasKey("FreePrize.NextCountdownStart"))
            {
                NextCountdownStart = DateTime.Parse(PreferencesFactory.GetString("FreePrize.NextCountdownStart", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture))); // start countdown immediately if new game
                NextFreePrizeAvailable = DateTime.Parse(PreferencesFactory.GetString("FreePrize.NextPrize", NextFreePrizeAvailable.ToString(CultureInfo.InvariantCulture)));
                SetCurrentPrizeAmount();
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

            PreferencesFactory.SetString("FreePrize.NextCountdownStart", NextCountdownStart.ToString(CultureInfo.InvariantCulture));
            PreferencesFactory.SetString("FreePrize.NextPrize", NextFreePrizeAvailable.ToString(CultureInfo.InvariantCulture));
            PreferencesFactory.Save();
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

        /// <summary>
        /// Recalculate new times for a new countdown
        /// </summary>
        public void StartNewCountdown()
        {
            SetCurrentPrizeAmount();

            NextCountdownStart = DateTime.UtcNow.AddSeconds(UnityEngine.Random.Range(DelayRangeToNextCountdown.Min, DelayRangeToNextCountdown.Max + 1));

            NextFreePrizeAvailable = NextCountdownStart.AddSeconds(UnityEngine.Random.Range(TimeRangeToNextPrize.Min, TimeRangeToNextPrize.Max + 1));

            SaveState();
        }

        /// <summary>
        /// Set the current prize amount
        /// </summary>
        void SetCurrentPrizeAmount()
        {
            CurrentPrizeAmount = UnityEngine.Random.Range(ValueRange.Min, ValueRange.Max);
        }

        /// <summary>
        /// Returns whether a countdown is taking place. If waiting for the countdown to begin or a Free Prize is available then this will return false
        /// </summary>
        /// <returns></returns>
        public bool IsCountingDown()
        {
            return GetTimeToPrize().TotalSeconds > 0 && GetTimeToCountdown().TotalSeconds <= 0;
        }

        /// <summary>
        /// Returns whether a prize is available
        /// </summary>
        /// <returns></returns>
        public bool IsPrizeAvailable()
        {
            return GetTimeToPrize().TotalSeconds <= 0;
        }

        /// <summary>
        /// Returns the time until the next countdown will start.
        /// </summary>
        /// <returns></returns>
        TimeSpan GetTimeToCountdown()
        {
            return NextCountdownStart.Subtract(DateTime.UtcNow);
        }

        /// <summary>
        /// Returns the time until the next free prize will be available.
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTimeToPrize()
        {
            return NextFreePrizeAvailable.Subtract(DateTime.UtcNow);
        }

        /// <summary>
        /// Show a free prize dialog that gives the user coins. We default to the standard General Message window, adding any additional
        /// content as setup in the FreePrizeManager configuration.
        /// </summary>
        public void ShowFreePrizeDialog()
        {
            // only allow the free prize dialog to be shown once.
            if (IsShowingFreePrizeDialog) return;

            IsShowingFreePrizeDialog = true;
            var dialogInstance = DialogManager.Instance.Create(null, null, ContentPrefab, null,
                runtimeAnimatorController: ContentAnimatorController);
            string text = GlobalLocalisation.FormatText("FreePrize.Text1", CurrentPrizeAmount);

            dialogInstance.Show(title: GlobalLocalisation.GetText("FreePrize.Title", missingReturnsKey: true), text: text, text2Key: "FreePrize.Text2",
                doneCallback: ShowFreePrizeDone,
                dialogButtons:
                    ContentShowsButtons
                        ? DialogInstance.DialogButtonsType.Custom
                        : DialogInstance.DialogButtonsType.Ok);
        }

        /// <summary>
        ///  Callback when the free prize dialog is closed. You may override this in your own subclasses, but be sure to call this base class instance.
        /// </summary>
        /// <param name="dialogInstance"></param>
        public virtual void ShowFreePrizeDone(DialogInstance dialogInstance)
        {
            GameManager.Instance.Player.Coins += CurrentPrizeAmount;
            if (PrizeDialogClosedAudioClip != null)
                GameManager.Instance.PlayEffect(PrizeDialogClosedAudioClip);
            GameManager.Instance.Player.UpdatePlayerPrefs();

            StartNewCountdown();
            IsShowingFreePrizeDialog = false;
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
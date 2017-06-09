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

using GameFramework.EditorExtras;
using GameFramework.GameStructure;
using GameFramework.GameStructure.Levels;
using GameFramework.Localisation;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameFramework.UI.Other.Components
{
    /// <summary>
    /// Provides a up / down counter based upon either a specific time target or that specified by the current Level
    /// </summary>
    [AddComponentMenu("Game Framework/UI/Other/TimeRemaining")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/ui/")]
    public class TimeRemaining : MonoBehaviour
    {
        public enum CounterModeType { Up, Down }
        public CounterModeType CounterMode;

        /// <summary>
        /// Whether the current counter limit (a value to count up to / down from) should be taken from the currently selected level's Time Target. If false you can specify a custom value.
        /// </summary>
        [Tooltip("Whether the current counter limit should be taken from the currently selected level. If false you can specify a custom value.")]
        public bool LimitFromLevelTimeTarget;

        /// <summary>
        /// A limit to count up to / down from.
        /// </summary>
        [Tooltip("A limit to count up to / down from.")]
        [ConditionalHide("LimitFromLevelTimeTarget", Inverse = true)]
        public int Limit;

        public UnityEngine.UI.Text Text;
        public string LocalisationKey;

        public UpdateSettingsType UpdateSettings;

        string _localisationString;

        bool _isUpdating;
        //int ScorePart;

        int _lastTimeRemaining;

        // Use this for initialization
        void Start()
        {
            if (!string.IsNullOrEmpty(LocalisationKey))
                _localisationString = GlobalLocalisation.GetText(LocalisationKey, missingReturnsKey: true);

            if (Text == null) Text = GetComponent<UnityEngine.UI.Text>();
            Assert.IsNotNull(Text, "Time Remaining must either be on the same gameobject as a UI Text control or have a Text component specified in it's settings.");

            if (LimitFromLevelTimeTarget)
                Limit = (int)GameManager.Instance.Levels.Selected.TimeTarget;

            UpdateDisplay();
        }

        // Update is called once per frame
        void Update()
        {
            if (!_isUpdating)
            {
                UpdateTimeRemainingStart();
            }
        }

        public void UpdateDisplay()
        {
            UpdateTimeRemaining();
        }

        void UpdateTimeRemainingStart()
        {
            int newTimeRemaining = CalculateTimeRemaining();
            if (_lastTimeRemaining != newTimeRemaining)
            {
                //ScorePart = (Player.Score - LastTotalScore) / UpdateSettings.Steps;
                _isUpdating = true;
                if (UpdateSettings.Animator == null)
                {
                    UpdateTimeRemaining();
                    UpdateCompleted();
                }
                else if (newTimeRemaining > _lastTimeRemaining)
                {
                    UpdateSettings.Animator.Play("TotalScoreIncreased");
                }
                else if (newTimeRemaining < _lastTimeRemaining)
                {
                    UpdateSettings.Animator.Play("TotalScoreDecreased");
                }
            }
        }

        public void UpdateTimeRemaining()
        {
            _lastTimeRemaining = CalculateTimeRemaining();
            Text.text = _localisationString == null ? _lastTimeRemaining.ToString() : string.Format(_localisationString, _lastTimeRemaining);
        }


        public void UpdateCompleted()
        {
            _isUpdating = false;
        }


        int CalculateTimeRemaining()
        {
            if (CounterMode == CounterModeType.Up)
            {
                return Mathf.Min((int)LevelManager.Instance.SecondsRunning, Limit);
            }
            else
            {
                return Mathf.Max(Limit - (int)LevelManager.Instance.SecondsRunning, 0);
            }
        }

        [System.Serializable]
        public class UpdateSettingsType
        {
            //public int Steps = 1;
            public Animator Animator;
        }
    }
}
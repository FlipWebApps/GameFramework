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
using System.Collections;
using GameFramework.EditorExtras;
using GameFramework.GameObjects;
using GameFramework.GameObjects.Components;
using GameFramework.GameStructure;
using GameFramework.GameStructure.Levels;
using GameFramework.Localisation;
using GameFramework.Social;
using GameFramework.UI.Other;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using GameFramework.GameStructure.Game;
using GameFramework.GameStructure.Players.Messages;
using GameFramework.Messaging;
using GameFramework.Preferences;

#if FACEBOOK_SDK
using GameFramework.Facebook.Components;
#endif

#if UNITY_ANALYTICS
using System.Collections.Generic;
using UnityEngine.Analytics;
#endif

namespace GameFramework.UI.Dialogs.Components
{
    /// <summary>
    /// Provides functionality for displaying and managing a game over dialog.
    /// </summary>
    [RequireComponent(typeof(DialogInstance))]
    [AddComponentMenu("Game Framework/UI/Dialogs/GameOver")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/ui/dialogs/")]
    public class GameOver : Singleton<GameOver>
    {
        public enum CopyType
        {
            None,
            Always,
            OnWin
        };

        public enum ResetTimeScaleType
        {
            None,
            Close,
            OnDestroy
        };

        [Header("General")]
        public string LocalisationBase = "GameOver";
        public int TimesPlayedBeforeRatingPrompt = -1;
        public bool ShowStars = true;
        public bool ShowTime = true;
        public bool ShowCoins = true;
        public bool ShowScore = true;
        public string ContinueScene = "Menu";

        [Header("Reward Handling")]
        [Tooltip("Specifies how the players overall score should be updated with the score obtained for the level.")]
        public CopyType UpdatePlayerScore = CopyType.None;
        [Tooltip("Specifies how the players overall coins should be updated with the coins obtained for the level.")]
        public CopyType UpdatePlayerCoins = CopyType.None;

        /// <summary>
        /// Whether to change the timescale when the GameOver window is shown. 
        /// </summary>
        bool PauseWhenShown
        {
            get
            {
                return _pauseWhenShown;
            }
            set
            {
                _pauseWhenShown = value;
            }
        }
        [Header("Pause")]
        [Tooltip("Whether to change the timescale when the GameOver window is shown.")]
        [SerializeField]
        bool _pauseWhenShown;

        /// <summary>
        /// The time scale that should be set when paused. Use this to stop physics and other effects.
        /// </summary>
        public float TimeScale
        {
            get
            {
                return _timeScale;
            }
            set
            {
                _timeScale = value;
            }
        }
        [Tooltip("The time scale that should be set when the game over window is shown. Use this to stop physics and other effects.")]
        [Range(0, 1)]
        [SerializeField]
        float _timeScale = 1f;

        public ResetTimeScaleType ResetTimeScale
        {
            get
            {
                return _resetTimeScale;
            }
            set
            {
                _resetTimeScale = value;
            }
        }
        [Tooltip("Whether to reset the time scale on things like when the window is closed.\nIf you don't set this and the window was set to freeze time then you will need to manually reset this yourself otherwise your game might still appear paused.")]
        [SerializeField]
        ResetTimeScaleType _resetTimeScale = ResetTimeScaleType.Close;

        /// <summary>
        /// The DialogInstance associated with the game over dialog.
        /// </summary>
        protected DialogInstance DialogInstance;

        float _oldTimeScale;

        #region Lifecycle Methods
        /// <summary>
        /// Called when this instance is created for one time initialisation.
        /// </summary>
        /// Override this in your own base class if you want to customise the game over window. Be sure to call this base instance first.
        protected override void GameSetup()
        {
            DialogInstance = GetComponent<DialogInstance>();
            Assert.IsNotNull(DialogInstance.Target, "Ensure that you have set the script execution order of dialog instance in project settings (see help for details).");
        }


        public virtual void OnEnable()
        {
            GameManager.SafeAddListener<PlayerCoinsChangedMessage>(PlayerCoinsChanged);
        }


        public virtual void OnDisable()
        {
            GameManager.SafeRemoveListener<PlayerCoinsChangedMessage>(PlayerCoinsChanged);
        }


        public virtual void OnDestroy()
        {
            if (PauseWhenShown && (ResetTimeScale == ResetTimeScaleType.OnDestroy || ResetTimeScale == ResetTimeScaleType.Close)) // Do on close also just to be sure.
                Time.timeScale = _oldTimeScale;
        }

        #endregion Lifecycle Methods

        /// <summary>
        /// Shows the game over dialog.
        /// </summary>
        /// Override this in your own base class if you want to customise the game over window. Be sure to call this base instance when done.
        public virtual void Show(bool isWon)
        {
            Assert.IsTrue(LevelManager.IsActive, "Ensure that you have a LevelManager component attached to your scene.");

            var currentLevel = GameManager.Instance.Levels.Selected;

            // update the player score if necessary
            if ((UpdatePlayerScore == CopyType.Always) || (UpdatePlayerScore == CopyType.OnWin && isWon))
            {
                GameManager.Instance.Player.AddPoints(currentLevel.Score);
            }

            // update the player coins if necessary
            if ((UpdatePlayerCoins == CopyType.Always) || (UpdatePlayerCoins == CopyType.OnWin && isWon))
            {
                GameManager.Instance.Player.AddCoins(currentLevel.Coins);
            }

            // show won / lost game objects as appropriate
            GameObjectHelper.SafeSetActive(GameObjectHelper.GetChildNamedGameObject(DialogInstance.gameObject, "Lost", true), !isWon);

            // see if the world or game is won and also if we should unlock the next world / level
            GameObjectHelper.SafeSetActive(GameObjectHelper.GetChildNamedGameObject(DialogInstance.gameObject, "GameWon", true), false);
            GameObjectHelper.SafeSetActive(GameObjectHelper.GetChildNamedGameObject(DialogInstance.gameObject, "WorldWon", true), false);
            GameObjectHelper.SafeSetActive(GameObjectHelper.GetChildNamedGameObject(DialogInstance.gameObject, "LevelWon", true), false);
            GameObjectHelper.SafeSetActive(GameObjectHelper.GetChildNamedGameObject(DialogInstance.gameObject, "Won", true), false);
            if (isWon)
            {
                //TODO: if coins unlock mode then need to check all levels are done before saying world complete - same for game...
                //TODO: perhaps in future we might want to distinguish between the first and subsequent times a user completes something?
                //// is the game won
                //if (GameHelper.IsCurrentLevelLastInGame()) {
                //    GameObjectHelper.SafeSetActive(GameObjectHelper.GetChildNamedGameObject(DialogInstance.gameObject, "GameWon", true), true);
                //}

                //// is a world won
                //else if (GameHelper.IsCurrentLevelLastInGame())
                //{
                //    GameObjectHelper.SafeSetActive(GameObjectHelper.GetChildNamedGameObject(DialogInstance.gameObject, "WorldWon", true), true);
                //}

                //// level won
                //else if (GameManager.Instance.Levels.GetNextItem() != null)
                //{
                //    GameObjectHelper.SafeSetActive(GameObjectHelper.GetChildNamedGameObject(DialogInstance.gameObject, "LevelWon", true), true);
                //}

                //// else won with some other condition
                //else
                //{
                GameObjectHelper.SafeSetActive(GameObjectHelper.GetChildNamedGameObject(DialogInstance.gameObject, "Won", true), true);
                //}

                // process and update game state - do this last so we can check some bits above.
                GameHelper.ProcessCurrentLevelComplete();
            }

            // set some text based upon the result
            UIHelper.SetTextOnChildGameObject(DialogInstance.gameObject, "AchievementText", GlobalLocalisation.FormatText(LocalisationBase + ".Achievement", currentLevel.Score, currentLevel.Name) ?? LocalisationBase + ".Achievement");

            // setup stars
            var starsGameObject = GameObjectHelper.GetChildNamedGameObject(DialogInstance.gameObject, "Stars", true);
            GameObjectHelper.SafeSetActive(starsGameObject, ShowStars);
            if (ShowStars)
            {
                Assert.IsNotNull(starsGameObject, "GameOver->ShowStars is enabled, but could not find a 'Stars' gameobject. Disable the option or fix the structure.");
                starsGameObject.SetActive(ShowStars);
                var newStarsWon = GetNewStarsWon();
                currentLevel.StarsWon |= newStarsWon;
                var star1WonGameObject = GameObjectHelper.GetChildNamedGameObject(starsGameObject, "Star1", true);
                var star2WonGameObject = GameObjectHelper.GetChildNamedGameObject(starsGameObject, "Star2", true);
                var star3WonGameObject = GameObjectHelper.GetChildNamedGameObject(starsGameObject, "Star3", true);
                StarWon(currentLevel.StarsWon, newStarsWon, star1WonGameObject, 1);
                StarWon(currentLevel.StarsWon, newStarsWon, star2WonGameObject, 2);
                StarWon(currentLevel.StarsWon, newStarsWon, star3WonGameObject, 4);
                GameObjectHelper.SafeSetActive(GameObjectHelper.GetChildNamedGameObject(starsGameObject, "StarWon", true), newStarsWon != 0);
            }

            // set time
            var difference = DateTime.Now - LevelManager.Instance.StartTime;
            var timeGameObject = GameObjectHelper.GetChildNamedGameObject(DialogInstance.gameObject, "Time", true);
            GameObjectHelper.SafeSetActive(timeGameObject, ShowTime);
            if (ShowTime)
            {
                Assert.IsNotNull(timeGameObject, "GameOver->ShowTime is enabled, but could not find a 'Time' gameobject. Disable the option or fix the structure.");

                UIHelper.SetTextOnChildGameObject(timeGameObject, "TimeResult", difference.Minutes.ToString("D2") + "." + difference.Seconds.ToString("D2"), true);
            }

            // set coins
            var coinsGameObject = GameObjectHelper.GetChildNamedGameObject(DialogInstance.gameObject, "Coins", true);
            GameObjectHelper.SafeSetActive(coinsGameObject, ShowCoins);
            if (ShowCoins)
            {
                Assert.IsNotNull(coinsGameObject, "GameOver->ShowCoins is enabled, but could not find a 'Coins' gameobject. Disable the option or fix the structure.");
                UIHelper.SetTextOnChildGameObject(coinsGameObject, "CoinsResult", currentLevel.Coins.ToString(), true);
            }

            // set score
            var scoreGameObject = GameObjectHelper.GetChildNamedGameObject(DialogInstance.gameObject, "Score", true);
            GameObjectHelper.SafeSetActive(scoreGameObject, ShowScore);
            if (ShowScore)
            {
                Assert.IsNotNull(scoreGameObject, "GameOver->ShowScore is enabled, but could not find a 'Score' gameobject. Disable the option or fix the structure.");
                var distanceText = GlobalLocalisation.FormatText(LocalisationBase + ".ScoreResult", currentLevel.Score.ToString()) ?? LocalisationBase + ".ScoreResult";
                if (currentLevel.HighScore > currentLevel.OldHighScore)
                    distanceText += "\n" + GlobalLocalisation.GetText(LocalisationBase + ".NewHighScore", missingReturnsKey: true);
                UIHelper.SetTextOnChildGameObject(scoreGameObject, "ScoreResult", distanceText, true);
            }

            UpdateNeededCoins();

            // save game state.
            GameManager.Instance.Player.UpdatePlayerPrefs();
            currentLevel.UpdatePlayerPrefs();
            PreferencesFactory.Save();

            // pause
            if (PauseWhenShown)
            {
                _oldTimeScale = Time.timeScale;
                Time.timeScale = TimeScale;
            }

            //show dialog
            DialogInstance.Show();

            //TODO bug - as we increase TimesPlayedForRatingPrompt on both game start (GameManager) and level finish we can miss this comparison.
            if (GameManager.Instance.TimesPlayedForRatingPrompt == TimesPlayedBeforeRatingPrompt)
            {
                var gameFeedback = new GameFeedback();
                gameFeedback.GameFeedbackAssumeTheyLikeOptional();
            }

#if UNITY_ANALYTICS
    // record some analytics on the level played
            var values = new Dictionary<string, object>
                {
                    { "score", currentLevel.Score },
                    { "Coins", currentLevel.Coins },
                    { "time", difference },
                    { "level", currentLevel.Number }
                };
            Analytics.CustomEvent("GameOver", values);
#endif
        }


        void StarWon(int starsWon, int newStarsWon, GameObject starGameObject, int bitMask)
        {
            // default state
            GameObjectHelper.GetChildNamedGameObject(starGameObject, "NotWon", true).SetActive((starsWon & bitMask) != bitMask);
            GameObjectHelper.GetChildNamedGameObject(starGameObject, "Won", true).SetActive((starsWon & bitMask) == bitMask);

            // if just won then animate
            if ((newStarsWon & bitMask) == bitMask)
            {
                var siblingAnimation = starGameObject.GetComponent<UnityEngine.Animation>();
                if (siblingAnimation != null)
                    siblingAnimation.Play();
            }
        }


        /// <summary>
        /// Handler for player coins changed messages.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool PlayerCoinsChanged(BaseMessage message)
        {
            UpdateNeededCoins();
            return true;
        }


        /// <summary>
        /// If LevelManager is in use then we return the difference between stars that were recorded at the start and those that are recorded now.
        /// </summary>
        /// You may also override this function if you wish to provide your own handling such as allocating stars only on completion.
        /// <returns></returns>
        public virtual int GetNewStarsWon()
        {
            if (LevelManager.IsActive && LevelManager.Instance.Level != null)
            {
                return LevelManager.Instance.Level.StarsWon - LevelManager.Instance.StartStarsWon;
            }

            return 0;
        }


        /// <summary>
        /// Update the display for needed coins to unlock a new level if that option is in use.
        /// </summary>
        public void UpdateNeededCoins()
        {
            var minimumCoins = GameManager.Instance.Levels.ExtraCoinsNeededToUnlock(GameManager.Instance.Player.Coins);
            var targetCoinsGameobject = GameObjectHelper.GetChildNamedGameObject(DialogInstance.gameObject, "TargetCoins", true);
            if (targetCoinsGameobject != null)
            {
                if (minimumCoins == 0)
                    UIHelper.SetTextOnChildGameObject(DialogInstance.gameObject, "TargetCoins",
                        GlobalLocalisation.FormatText(LocalisationBase + ".TargetCoinsGot", minimumCoins) ?? LocalisationBase + ".TargetCoinsGot", true);
                else if (minimumCoins > 0)
                    UIHelper.SetTextOnChildGameObject(DialogInstance.gameObject, "TargetCoins",
                        GlobalLocalisation.FormatText(LocalisationBase + ".TargetCoins", minimumCoins) ?? LocalisationBase + ".TargetCoins", true);
                else
                    targetCoinsGameobject.SetActive(false);
            }
        }



        /// <summary>
        /// Callback for sharing to Facebook
        /// </summary>
        public void FacebookShare()
        {
#if FACEBOOK_SDK
            Assert.IsTrue(FacebookManager.IsActive, "Please ensure that you have a FacebookManager component added to your scene.");
            FacebookManager.Instance.ShareLink();
#endif
        }


        /// <summary>
        /// Callback for continuing to the scene specified in ContinueScene
        /// </summary>
        public void Continue()
        {
            if (PauseWhenShown && ResetTimeScale == ResetTimeScaleType.Close)
                Time.timeScale = _oldTimeScale;

            GameManager.LoadSceneWithTransitions(ContinueScene);
        }


        /// <summary>
        /// Callback for retrying the current level - reloads the current scene
        /// </summary>
        public void Retry()
        {
            if (PauseWhenShown && ResetTimeScale == ResetTimeScaleType.Close)
                Time.timeScale = _oldTimeScale;

            var sceneName = !string.IsNullOrEmpty(GameManager.Instance.IdentifierBase) && SceneManager.GetActiveScene().name.StartsWith(GameManager.Instance.IdentifierBase + "-") ? SceneManager.GetActiveScene().name.Substring((GameManager.Instance.IdentifierBase + "-").Length) : SceneManager.GetActiveScene().name;
            GameManager.LoadSceneWithTransitions(sceneName);
        }
    }
}
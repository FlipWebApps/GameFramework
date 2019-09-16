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
using GameFramework.Debugging;
using GameFramework.GameObjects.Components;
using GameFramework.GameStructure.Levels.Messages;
using GameFramework.GameStructure.Levels.ObjectModel;
using GameFramework.UI.Dialogs.Components;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameFramework.GameStructure.Levels
{
    /// <summary>
    /// Manages the concept of a running level and holds state and other information relating to this
    /// </summary>
    [AddComponentMenu("Game Framework/GameStructure/Levels/LevelManager")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/game-structure/levels/")]
    public class LevelManager : Singleton<LevelManager>
    {
        /// <summary>
        /// Whether LevelStarted should be called automatically when the scene loads
        /// </summary>
        [Tooltip("Whether LevelStarted should be called automatically when the scene loads")]
        public bool AutoStart;

        /// <summary>
        /// A delay from when a game over condition is detected before showing the game over dialog
        /// </summary>
        [Header("Auto Game Over")]
        [Tooltip("A delay from when a game over condition is detected before showing the game over dialog")]
        public float ShowGameOverDialogDelay;

        /// <summary>
        /// Whether the user should automatically lose when they have no lives left
        /// </summary>
        [Header("Auto Game Over Conditions")]
        [Tooltip("Whether the user should automatically lose when they have no lives left")]
        public bool GameOverWhenLivesIsZero;

        /// <summary>
        /// Whether the user should automatically lose when their health reaches zero
        /// </summary>
        [Tooltip("Whether the user should automatically lose when their health reaches zero")]
        public bool GameOverWhenHealthIsZero;

        /// <summary>
        /// Whether the user should automatically lose when the levels specified target time is reached
        /// </summary>
        [Tooltip("Whether the user should automatically lose when the levels specified target time is reached")]
        public bool GameOverWhenTargetTimeReached;

        /// <summary>
        /// Whether the user should automatically win the game once all stars have been gotten
        /// </summary>
        [Tooltip("Whether the user should automatically win the game once all stars have been gotten")]
        public bool GameWonWhenAllStarsGot;

        /// <summary>
        /// Whether the user should automatically win the game once the levels target score is reached
        /// </summary>
        [Tooltip("Whether the user should automatically win the game once the levels target score is reached")]
        public bool GameWonWhenTargetScoreReached;

        /// <summary>
        /// Whether the user should automatically win the game once the levels target coins is reached
        /// </summary>
        [Tooltip("Whether the user should automatically win the game once the levels target coins is reached")]
        public bool GameWonWhenTargetCoinsReached;

        /// <summary>
        /// Time when the level was started.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// The number of stars that were already won when the level was started.
        /// </summary>
        public int StartStarsWon { get; set; }

        /// <summary>
        /// The number of seconds that the level has been running for
        /// </summary>
        public float SecondsRunning { get; set; }

        /// <summary>
        /// Whether the level has started
        /// </summary>
        public bool IsLevelStarted { get; set; }

        /// <summary>
        /// Whether the level is finished.
        /// </summary>
        public bool IsLevelFinished { get; set; }

        /// <summary>
        /// Whether the level is paused.
        /// </summary>
        public bool IsLevelPaused { get; private set; }

        /// <summary>
        /// Whether the level is running (i.e. started but not finished)
        /// </summary>
        public bool IsLevelRunning
        {
            get { return IsLevelStarted && !IsLevelFinished && !IsLevelPaused; }
        }

        /// <summary>
        /// Shortcut to the current level set in GameManager.
        /// </summary>
        public Level Level
        {
            get { return GameManager.Instance.Levels != null ? GameManager.Instance.Levels.Selected : null; }
        }

        float _prePauseTimeScale;

        protected override void GameSetup()
        {
            base.GameSetup();

            Reset();
        }


        /// <summary>
        /// Start the level automatically is set to do so.
        /// </summary>
        void Start()
        {
#if UNITY_EDITOR
            // some sanity checking and warnings
            if (GameOverWhenTargetTimeReached && Mathf.Approximately(Level.TimeTarget, 0))
                MyDebug.LogWarningF("You have enabled the option 'GameOverWhenTargetTimeReached' in LevelManager however the current level has a configured TimeTarget of 0 and so will end immediately. Consider using Level resource configuration files and setting this value.");
            if (GameWonWhenTargetScoreReached && Mathf.Approximately(Level.ScoreTarget, 0))
                MyDebug.LogWarningF("You have enabled the option 'GameWonWhenTargetScoreReached' in LevelManager however the current level has a configured ScoreTarget of 0 and so will end immediately. Consider using Level resource configuration files and setting this value.");
            if (GameWonWhenTargetCoinsReached && Mathf.Approximately(Level.CoinTarget, 0))
                MyDebug.LogWarningF("You have enabled the option 'GameWonWhenTargetCoinsReached' in LevelManager however the current level has a configured CoinTarget of 0 and so will end immediately. Consider using Level resource configuration files and setting this value.");
#endif

            if (AutoStart)
                StartLevel();
        }


        /// <summary>
        /// Reset the level state
        /// </summary>
        public void Reset()
        {
            IsLevelStarted = false;
            IsLevelFinished = false;

            if (Level != null)
            {
                Level.Coins = 0;
                Level.Score = 0;
                StartStarsWon = Level.StarsWon;
            }
        }


        /// <summary>
        /// State change to level started
        /// </summary>
        public void StartLevel()
        {
            StartTime = DateTime.Now;
            if (Level != null)
                StartStarsWon = Level.StarsWon;

            SecondsRunning = 0f;
            IsLevelStarted = true;
            GameManager.SafeQueueMessage(new LevelStartedMessage(Level));
        }

        [Obsolete("Call StartLevel() instead.")]
        public void LevelStarted()
        {
            StartLevel();
        }

        /// <summary>
        /// State change to level finished. Call GameOver if you want the GameOver dialog to be displayed.
        /// </summary>
        public void EndLevel(bool won = false)
        {
            IsLevelFinished = true;
            GameManager.Instance.TimesLevelsPlayed++;
            GameManager.Instance.TimesPlayedForRatingPrompt++;
            GameManager.SafeQueueMessage(new LevelEndedMessage(Level, won));
        }

        [Obsolete("Call EndLevel() instead.")]
        public void LevelFinished()
        {
            StartLevel();
        }


        /// <summary>
        /// Pause the level optionally showing a dialog and setting the timeScale.
        /// </summary>
        public void PauseLevel(bool showPauseDialog, float timeScale = 0)
        {
            if (IsLevelPaused) return;

            IsLevelPaused = true;
            _prePauseTimeScale = Time.timeScale;
            Time.timeScale = timeScale;
            GameManager.SafeQueueMessage(new LevelPausedMessage(Level));

            if (showPauseDialog)
            {
                Assert.IsTrue(PauseWindow.IsActive, "Enaure that you have added a pause window prefab to your scene.");
                PauseWindow.Instance.Show();
            }
        }


        /// <summary>
        /// Pause a level showing a dialog
        /// </summary>
        public void PauseLevel()
        {
            PauseLevel(true);
        }


        /// <summary>
        /// Resume a paused level.
        /// </summary>
        public void ResumeLevel()
        {
            if (!IsLevelPaused) return;

            IsLevelPaused = false;
            Time.timeScale = _prePauseTimeScale;
            GameManager.SafeQueueMessage(new LevelResumedMessage(Level));
        }


        /// <summary>
        /// Trigger that the game is over and show the game over dialog.
        /// </summary>
        /// <param name="isWon"></param>
        /// <param name="showDialogDelay"></param>
        public void GameOver(bool isWon, float showDialogDelay = 0)
        {
            if (!Instance.IsLevelRunning) return;

            Assert.IsTrue(UI.Dialogs.Components.GameOver.IsActive,
                "Please ensure that you have a GameOver component added to your scene, or are using one of the default GameOver prefabs.");

            EndLevel(isWon);

            //TODO: move delayed showing into dialog instance.Show()!
            StartCoroutine(ShowGameOverDialog(isWon, showDialogDelay));
        }


        /// <summary>
        /// Co routine for showing the gameover dialog after a given delay.
        /// </summary>
        /// <param name="isWon"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        IEnumerator ShowGameOverDialog(bool isWon, float delay)
        {
            yield return new WaitForSeconds(delay);
            UI.Dialogs.Components.GameOver.Instance.Show(isWon);
        }


        void Update()
        {
            if (IsLevelRunning)
            {
                SecondsRunning += Time.deltaTime;

                // check for gameover conditions.
                if ((GameOverWhenLivesIsZero && GameManager.Instance.Player.Lives == 0) ||
                    (GameOverWhenHealthIsZero && (Mathf.Approximately(GameManager.Instance.Player.Health, 0) || GameManager.Instance.Player.Health < 0)) ||
                    (GameOverWhenTargetTimeReached && SecondsRunning >= Level.TimeTarget))
                    GameOver(false, ShowGameOverDialogDelay);

                // check for gameover (win) conditions.
                if ((GameWonWhenAllStarsGot && Level.StarsTotalCount == Level.StarsWonCount && StartStarsWon != Level.StarsWon) ||
                    GameWonWhenTargetScoreReached && Level.Score >= Level.ScoreTarget ||
                    GameWonWhenTargetCoinsReached && Level.Coins >= Level.CoinTarget)
                    GameOver(true, ShowGameOverDialogDelay);
            }

        }
    }
}
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
using FlipWebApps.GameFramework.Scripts.GameObjects.Components;
using FlipWebApps.GameFramework.Scripts.GameStructure.Levels.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Levels
{
    /// <summary>
    /// Manages the concept of a running level
    /// </summary>
    public class LevelManager : Singleton<LevelManager>
    {
        /// <summary>
        /// Whether LevelStarted should be called on Start()
        /// </summary>
        public bool AutoStart;

        [Header("Auto Game Over")]
        public float ShowGameOverDialogDelay;

        [Header("Auto Game Over Conditions")]
        public bool GameOverWhenLivesIsZero;
        public bool GameOverWhenHealthIsZero;

        public DateTime StartTime { get; set; }
        public int StartStarsWon { get; set; }

        public float SecondsRunning { get; set; }

        public bool IsLevelStarted { get; set; }
        public bool IsLevelFinished { get; set; }

        public bool IsLevelRunning
        {
            get { return IsLevelStarted && !IsLevelFinished; }
        }

        public Level Level
        {
            get { return GameManager.Instance.Levels != null ? GameManager.Instance.Levels.Selected : null; }
        }


        protected override void GameSetup()
        {
            base.GameSetup();

            Reset();
        }


        void Start()
        {
            if (AutoStart)
                LevelStarted();
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
            }
        }


        /// <summary>
        /// State change to level started
        /// </summary>
        public void LevelStarted()
        {
            StartTime = DateTime.Now;
            if (Level != null)
                StartStarsWon = Level.StarsWon;

            SecondsRunning = 0f;
            IsLevelStarted = true;
        }


        /// <summary>
        /// State change to level finished.
        /// </summary>
        public void LevelFinished()
        {
            IsLevelFinished = true;
            GameManager.Instance.TimesLevelsPlayed++;
            GameManager.Instance.TimesPlayedForRatingPrompt++;
        }


        /// <summary>
        /// Trigger that the game is over and show the game over dialog.
        /// </summary>
        /// <param name="isWon"></param>
        public void GameOver(bool isWon, float showDialogDelay = 0)
        {
            if (!Instance.IsLevelRunning) return;

            Assert.IsTrue(UI.Dialogs.Components.GameOver.IsActive,
                "Please ensure that you have a GameOver component added to your scene, or are using one of the default GameOver prefabs.");

            LevelFinished();

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
                if (GameOverWhenLivesIsZero && GameManager.Instance.Player.Lives == 0)
                    GameOver(false, ShowGameOverDialogDelay);
                if (GameOverWhenHealthIsZero && Mathf.Approximately(GameManager.Instance.Player.Health, 0))
                    GameOver(false, ShowGameOverDialogDelay);

            }

        }
    }
}
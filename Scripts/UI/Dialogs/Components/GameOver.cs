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
using FlipWebApps.GameFramework.Scripts.GameObjects;
using FlipWebApps.GameFramework.Scripts.GameObjects.Components;
using FlipWebApps.GameFramework.Scripts.GameStructure;
using FlipWebApps.GameFramework.Scripts.GameStructure.Levels;
using FlipWebApps.GameFramework.Scripts.GameStructure.Levels.ObjectModel;
using FlipWebApps.GameFramework.Scripts.Localisation;
using FlipWebApps.GameFramework.Scripts.Social;
using FlipWebApps.GameFramework.Scripts.UI.Other;
using FlipWebApps.GameFramework.Scripts.UI.Other.Components;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using FlipWebApps.GameFramework.Scripts.GameStructure.Game;

#if FACEBOOK_SDK
using FlipWebApps.GameFramework.Scripts.Facebook.Components;
#endif

#if UNITY_ANALYTICS
using System.Collections.Generic;
using UnityEngine.Analytics;
#endif

namespace FlipWebApps.GameFramework.Scripts.UI.Dialogs.Components
{
    /// <summary>
    /// Base class for a game over dialog.
    /// </summary>
    [RequireComponent(typeof(DialogInstance))]
    [AddComponentMenu("Game Framework/UI/Dialogs/GameOver")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class GameOver : Singleton<GameOver>
    {
        [Header("General")]
        public string LocalisationBase = "GameOver";
        public int TimesPlayedBeforeRatingPrompt = -1;
        public bool ShowStars = true;
        public bool ShowTime = true;
        public bool ShowCoins = true;
        public bool ShowScore = true;
        public string ContinueScene = "Menu";

        [Header("Tuning")]
        public float PeriodicUpdateDelay = 1f;

        protected DialogInstance DialogInstance;

        protected override void GameSetup()
        {
            DialogInstance = GetComponent<DialogInstance>();

            Assert.IsNotNull(DialogInstance.DialogGameObject, "Ensure that you have set the script execution order of dialog instance in settings (see help for details).");
        }

        public virtual void Show(bool isWon)
        {
            Assert.IsTrue(LevelManager.IsActive, "Ensure that you have a LevelManager component attached to your scene.");

            Level currentLevel = GameManager.Instance.Levels.Selected;

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
            UIHelper.SetTextOnChildGameObject(DialogInstance.gameObject, "AchievementText", LocaliseText.Format(LocalisationBase + ".Achievement", currentLevel.Score, currentLevel.Name));

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
                GameObjectHelper.SafeSetActive(
                    GameObjectHelper.GetChildNamedGameObject(starsGameObject, "StarWon", true),
                    newStarsWon != 0);
            }

            // set time
            TimeSpan difference = DateTime.Now - LevelManager.Instance.StartTime;
            var timeGameObject = GameObjectHelper.GetChildNamedGameObject(DialogInstance.gameObject, "Time", true);
            GameObjectHelper.SafeSetActive(timeGameObject, ShowTime);
            if (ShowTime)
            {
                Assert.IsNotNull(timeGameObject,
                    "GameOver->ShowTime is enabled, but could not find a 'Time' gameobject. Disable the option or fix the structure.");
                
                UIHelper.SetTextOnChildGameObject(timeGameObject, "TimeResult",
                    difference.Minutes.ToString("D2") + "." + difference.Seconds.ToString("D2"), true);
            }

            // set coins
            var coinsGameObject = GameObjectHelper.GetChildNamedGameObject(DialogInstance.gameObject, "Coins", true);
            GameObjectHelper.SafeSetActive(coinsGameObject, ShowCoins);
            if (ShowCoins)
            {
                Assert.IsNotNull(coinsGameObject,
                    "GameOver->ShowCoins is enabled, but could not find a 'Coins' gameobject. Disable the option or fix the structure.");
                UIHelper.SetTextOnChildGameObject(coinsGameObject, "CoinsResult",
                    currentLevel.Coins.ToString(), true);
            }

            // set score
            var scoreGameObject = GameObjectHelper.GetChildNamedGameObject(DialogInstance.gameObject, "Score", true);
            GameObjectHelper.SafeSetActive(scoreGameObject, ShowScore);
            if (ShowScore)
            {
                Assert.IsNotNull(scoreGameObject, "GameOver->ShowScore is enabled, but could not find a 'Score' gameobject. Disable the option or fix the structure.");
                var distanceText = LocaliseText.Format(LocalisationBase + ".ScoreResult", currentLevel.Score.ToString());
                if (currentLevel.HighScore > currentLevel.OldHighScore)
                    distanceText += "\n" + LocaliseText.Get(LocalisationBase + ".NewHighScore");
                UIHelper.SetTextOnChildGameObject(scoreGameObject, "ScoreResult", distanceText, true);
            }

            UpdateNeededCoins();

            // save game state.
            GameManager.Instance.Player.UpdatePlayerPrefs();
            currentLevel.UpdatePlayerPrefs();
            PlayerPrefs.Save();

            //show dialog
            DialogInstance.Show();

            //TODO bug - as we increase TimesPlayedForRatingPrompt on both game start (GameManager) and level finish we can miss this comparison.
            if (GameManager.Instance.TimesPlayedForRatingPrompt == TimesPlayedBeforeRatingPrompt)
            {
                GameFeedback gameFeedback = new GameFeedback();
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

            // co routine to periodic updates of display (don't need to do this every frame)
            if (!Mathf.Approximately(PeriodicUpdateDelay, 0))
                StartCoroutine(PeriodicUpdate());
        }

        void StarWon(int starsWon, int newStarsWon, GameObject starGameObject, int bitMask)
        {
            // default state
            GameObjectHelper.GetChildNamedGameObject(starGameObject, "NotWon", true).SetActive((starsWon & bitMask) != bitMask);
            GameObjectHelper.GetChildNamedGameObject(starGameObject, "Won", true).SetActive((starsWon & bitMask) == bitMask);

            // if just won then animate
            if ((newStarsWon & bitMask) == bitMask)
            {
                var animation = starGameObject.GetComponent<UnityEngine.Animation>();
                if (animation != null)
                    animation.Play();
            }
        }

        public virtual IEnumerator PeriodicUpdate()
        {
            while (true)
            {
                UpdateNeededCoins();

                yield return new WaitForSeconds(PeriodicUpdateDelay);
            }
        }


        /// <summary>
        /// If LevelManager is in use then we return the difference between stars that were recorded at the start and those that are recorded now.
        /// 
        /// You may also override this function if you wish to provide your own handling such as allocating stars only on completion.
        /// </summary>
        /// <returns></returns>
        public virtual int GetNewStarsWon()
        {
            if (LevelManager.IsActive && LevelManager.Instance.Level != null)
            {
                return LevelManager.Instance.Level.StarsWon - LevelManager.Instance.StartStarsWon;
            }

            return 0;
        }


        public void UpdateNeededCoins()
        {
            int minimumCoins = GameManager.Instance.Levels.ExtraValueNeededToUnlock(GameManager.Instance.Player.Coins);
            if (minimumCoins == 0)
                UIHelper.SetTextOnChildGameObject(DialogInstance.gameObject, "TargetCoins", LocaliseText.Format(LocalisationBase + ".TargetCoinsGot", minimumCoins), true);
            else
                UIHelper.SetTextOnChildGameObject(DialogInstance.gameObject, "TargetCoins", LocaliseText.Format(LocalisationBase + ".TargetCoins", minimumCoins), true);
        }

        public void FacebookShare()
        {
#if FACEBOOK_SDK
            FacebookManager.Instance.PostAndLoginIfNeeded();
#endif
        }

        public void Continue()
        {
            GameManager.LoadSceneWithTransitions(ContinueScene);
        }

        public void Retry()
        {
            var sceneName = !string.IsNullOrEmpty(GameManager.Instance.IdentifierBase) && SceneManager.GetActiveScene().name.StartsWith(GameManager.Instance.IdentifierBase + "-") ?
                SceneManager.GetActiveScene().name.Substring((GameManager.Instance.IdentifierBase + "-").Length) : SceneManager.GetActiveScene().name;
            GameManager.LoadSceneWithTransitions(sceneName);
        }
    }
}
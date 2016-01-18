//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
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
    public class GameOver : Singleton<GameOver>
    {
        public string LocalisationBase = "GameOver";
        public int TimesPlayedBeforeRatingPrompt = -1;
        public float PeriodicUpdateDelay = 1f;

        DialogInstance _dialogInstance;

        protected override void GameSetup()
        {
            _dialogInstance = GetComponent<DialogInstance>();

            Assert.IsNotNull(_dialogInstance.DialogGameObject, "Ensure that you have set the script execution order of dialog instance in settings (see help for details.");
        }

        public virtual void Show(bool isWon)
        {
            Level currentLevel = GameManager.Instance.Levels.Selected;

            // show won / lost game objects as appropriate
            GameObjectHelper.SafeSetActive(GameObjectHelper.GetChildNamedGameObject(_dialogInstance.gameObject, "Won", true), isWon);
            GameObjectHelper.SafeSetActive(GameObjectHelper.GetChildNamedGameObject(_dialogInstance.gameObject, "Lost", true), !isWon);

            // set some text based upon the result
            UIHelper.SetTextOnChildGameObject(_dialogInstance.gameObject, "AchievementText", LocaliseText.Format(LocalisationBase + ".Achievement", currentLevel.Score, currentLevel.Name));

            // setup stars
            int newStarsWon = GetNewStarsWon();
            currentLevel.StarsWon |= newStarsWon;
            GameObject star1WonGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "Star1", true);
            GameObject star2WonGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "Star2", true);
            GameObject star3WonGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "Star3", true);
            StarWon(currentLevel.StarsWon, newStarsWon, star1WonGameObject, 1);
            StarWon(currentLevel.StarsWon, newStarsWon, star2WonGameObject, 2);
            StarWon(currentLevel.StarsWon, newStarsWon, star3WonGameObject, 4);
            GameObjectHelper.SafeSetActive(GameObjectHelper.GetChildNamedGameObject(gameObject, "StarWon", true), newStarsWon != 0);

            //show high score
            string distanceText = LocaliseText.Format(LocalisationBase + ".ScoreResult", currentLevel.Score.ToString());
            if (currentLevel.HighScore > currentLevel.OldHighScore)
                distanceText += "\n" + LocaliseText.Get(LocalisationBase + ".NewHighScore");
            UIHelper.SetTextOnChildGameObject(_dialogInstance.gameObject, "ScoreResult", distanceText, true);

            // set time
            TimeSpan difference = DateTime.Now - LevelManager.Instance.StartTime;
            UIHelper.SetTextOnChildGameObject(_dialogInstance.gameObject, "TimeResult", difference.Minutes.ToString("D2") + "." + difference.Seconds.ToString("D2"), true);

            // set count
            UIHelper.SetTextOnChildGameObject(_dialogInstance.gameObject, "CoinsResult", currentLevel.Coins.ToString(), true);

            UpdateNeededCoins();

            // save game state.
            GameManager.Instance.Player.UpdatePlayerPrefs();
            currentLevel.UpdatePlayerPrefs();
            PlayerPrefs.Save();

            //show dialog
            _dialogInstance.Show();

            //TODO bug - as we increase TimesPlayedForRatingPrompt on both game start (GameManager) and level finish we can miss this comparison.
            if (GameManager.Instance.TimesPlayedForRatingPrompt == TimesPlayedBeforeRatingPrompt)
            {
                GameFeedback gameFeedback = new GameFeedback();
                gameFeedback.GameFeedbackAssumeTheyLikeOptional();
            }

#if UNITY_ANALYTICS
            // record some analytics on the level played
            Analytics.CustomEvent("GameOver", new Dictionary<string, object>
                {
                    { "score", currentLevel.Score },
                    { "Coins", currentLevel.Coins },
                    { "time", difference },
                    { "level", currentLevel.Number }
                });
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
                UnityEngine.Animation animation = starGameObject.GetComponent<UnityEngine.Animation>();
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

   
        public virtual int GetNewStarsWon()
        {
            return 0;
        }


        public void UpdateNeededCoins()
        {
            int minimumCoins = GameManager.Instance.Levels.ExtraValueNeededToUnlock(GameManager.Instance.Player.Coins);
            if (minimumCoins == 0)
                UIHelper.SetTextOnChildGameObject(_dialogInstance.gameObject, "TargetCoins", LocaliseText.Format(LocalisationBase + ".TargetCoinsGot", minimumCoins), true);
            else
                UIHelper.SetTextOnChildGameObject(_dialogInstance.gameObject, "TargetCoins", LocaliseText.Format(LocalisationBase + ".TargetCoins", minimumCoins), true);
        }

        public void FacebookShare()
        {
#if FACEBOOK_SDK
            FacebookManager.Instance.PostAndLoginIfNeeded();
#endif
        }

        public void Continue()
        {
            FadeLevelManager.Instance.LoadScene(GameManager.GetIdentifierScene("Menu"));
        }

        public void Retry()
        {
            FadeLevelManager.Instance.LoadScene(GameManager.GetIdentifierScene("Game"));
        }
    }
}
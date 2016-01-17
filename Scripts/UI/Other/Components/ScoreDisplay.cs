//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameStructure;
using FlipWebApps.GameFramework.Scripts.GameStructure.Levels.ObjectModel;
using FlipWebApps.GameFramework.Scripts.GameStructure.Players.ObjectModel;
using FlipWebApps.GameFramework.Scripts.Localisation;
using UnityEngine;
using UnityEngine.Assertions;

namespace FlipWebApps.GameFramework.Scripts.UI.Other.Components
{

    public class ScoreDisplay : MonoBehaviour
    {
        public enum DisplayType { TotalScore, TotalCoins, LevelScore, LevelCoins, LevelHighScore }
        public DisplayType Display;
        public UnityEngine.UI.Text Text;
        public string LocalisationKey;

        public UpdateSettingsType UpdateSettings;


        Player _player;
        Level _level;
        string _localisationString;

        bool _isUpdating;
        //int ScorePart;

        int _lastTotalScore, _lastTotalCoins, _lastLevelScore, _lastLevelCoins, _lastLevelHighScore;

        // Use this for initialization
        void Start()
        {
            if (Text == null) Text = GetComponent<UnityEngine.UI.Text>();
            Assert.IsNotNull(Text, "You either have to specify a Text component, or attach the Score Display to a gameobject that contains one.");

            _player = GameManager.Instance.GetPlayer();
            _level = GameManager.Instance.Levels == null ? null : GameManager.Instance.Levels.Selected;
            if (!string.IsNullOrEmpty(LocalisationKey))
                _localisationString = LocaliseText.Get(LocalisationKey);

            UpdateDisplay();
        }

        // Update is called once per frame
        void Update()
        {
            if (_isUpdating) return;

            switch (Display)
            {
                case DisplayType.TotalScore:
                    UpdateTotalScoreStart();
                    break;
                case DisplayType.TotalCoins:
                    UpdateTotalCoinsStart();
                    break;
                case DisplayType.LevelScore:
                    UpdateLevelScoreStart();
                    break;
                case DisplayType.LevelCoins:
                    UpdateLevelCoinsStart();
                    break;
                case DisplayType.LevelHighScore:
                    UpdateLevelHighScoreStart();
                    break;
            }
        }

        public void UpdateDisplay()
        {
            switch (Display)
            {
                case DisplayType.TotalScore:
                    UpdateTotalScore();
                    break;
                case DisplayType.TotalCoins:
                    UpdateTotalCoins();
                    break;
                case DisplayType.LevelScore:
                    UpdateLevelScore();
                    break;
                case DisplayType.LevelCoins:
                    UpdateLevelCoins();
                    break;
                case DisplayType.LevelHighScore:
                    UpdateLevelHighScore();
                    break;
            }
        }

        void UpdateTotalScoreStart()
        {
            if (_lastTotalScore != _player.Score)
            {
                //ScorePart = (Player.Score - LastTotalScore) / UpdateSettings.Steps;
                _isUpdating = true;
                if (UpdateSettings.Animator == null)
                {
                    UpdateTotalScore();
                    UpdateCompleted();
                }
                else if (_player.Score > _lastTotalScore)
                {
                    UpdateSettings.Animator.Play("TotalScoreIncreased");
                }
                else if (_player.Score < _lastTotalScore)
                {
                    UpdateSettings.Animator.Play("TotalScoreDecreased");
                }
            }
        }

        public void UpdateTotalScore()
        {
            _lastTotalScore = _player.Score;
            Text.text = _localisationString == null ? _lastTotalScore.ToString() : string.Format(_localisationString, _lastTotalScore);
        }


        void UpdateTotalCoinsStart()
        {
            if (_lastTotalCoins != _player.Coins)
            {
                if (UpdateSettings.Animator == null)
                {
                    UpdateTotalCoins();
                    UpdateCompleted();
                }
                else if (_player.Coins > _lastTotalCoins)
                {
                    UpdateSettings.Animator.Play("TotalCoinsIncreased");
                }
                else if (_player.Coins < _lastTotalCoins)
                {
                    UpdateSettings.Animator.Play("TotalCoinsDecreased");
                }
            }
        }

        public void UpdateTotalCoins()
        {
            _lastTotalCoins = _player.Coins;
            Text.text = _localisationString == null ? _lastTotalCoins.ToString() : string.Format(_localisationString, _lastTotalCoins);
        }

        void UpdateLevelScoreStart()
        {
            Assert.IsNotNull(_level, "To display the level score you need to have an active level assigned");

            if (_lastLevelScore != _level.Score)
            {
                //ScorePart = (Player.Score - LastLevelScore) / UpdateSettings.Steps;
                _isUpdating = true;
                if (UpdateSettings.Animator == null)
                {
                    UpdateLevelScore();
                    UpdateCompleted();
                }
                else if (_level.Score > _lastLevelScore)
                {
                    UpdateSettings.Animator.Play("LevelScoreIncreased");
                }
                else if (_level.Score < _lastLevelScore)
                {
                    UpdateSettings.Animator.Play("LevelScoreDecreased");
                }
            }
        }

        public void UpdateLevelScore()
        {
            Assert.IsNotNull(_level, "To display the level score you need to have an active level assigned");

            _lastLevelScore = _level.Score;
            Text.text = _localisationString == null ? _lastLevelScore.ToString() : string.Format(_localisationString, _lastLevelScore);
        }

        void UpdateLevelHighScoreStart()
        {
            Assert.IsNotNull(_level, "To display the level score you need to have an active level assigned");

            if (_lastLevelHighScore != _level.HighScore)
            {
                //CoinsPart = (Player.Coins - LastLevelCoins) / UpdateSettings.Steps;
                _isUpdating = true;
                if (UpdateSettings.Animator == null)
                {
                    UpdateLevelHighScore();
                    UpdateCompleted();
                }
                else if (_level.HighScore > _lastLevelHighScore)
                {
                    UpdateSettings.Animator.Play("LevelScoreIncreased");
                }
                else if (_level.HighScore < _lastLevelHighScore)
                {
                    UpdateSettings.Animator.Play("LevelScoreDecreased");
                }
            }
        }

        public void UpdateLevelHighScore()
        {
            Assert.IsNotNull(_level, "To display the level score you need to have an active level assigned");

            _lastLevelHighScore = _level.HighScore;
            Text.text = _localisationString == null ? _lastLevelHighScore.ToString() : string.Format(_localisationString, _lastLevelHighScore);
        }

        void UpdateLevelCoinsStart()
        {
            Assert.IsNotNull(_level, "To display the level score you need to have an active level assigned");

            if (_lastLevelCoins != _level.Coins)
            {
                //CoinsPart = (Player.Coins - LastLevelCoins) / UpdateSettings.Steps;
                _isUpdating = true;
                if (UpdateSettings.Animator == null)
                {
                    UpdateLevelCoins();
                    UpdateCompleted();
                }
                else if (_level.Coins > _lastLevelCoins)
                {
                    UpdateSettings.Animator.Play("LevelCoinsIncreased");
                }
                else if (_level.Coins < _lastLevelCoins)
                {
                    UpdateSettings.Animator.Play("LevelCoinsDecreased");
                }
            }
        }

        public void UpdateLevelCoins()
        {
            Assert.IsNotNull(_level, "To display the level score you need to have an active level assigned");

            _lastLevelCoins = _level.Coins;
            Text.text = _localisationString == null ? _lastLevelCoins.ToString() : string.Format(_localisationString, _lastLevelCoins);
        }


        public void UpdateCompleted()
        {
            _isUpdating = false;
        }

        [System.Serializable]
        public class UpdateSettingsType
        {
            //public int Steps = 1;
            public Animator Animator;
        }
    }
}
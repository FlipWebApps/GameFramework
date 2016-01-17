//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameStructure.Levels;
using FlipWebApps.GameFramework.Scripts.Localisation;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.UI.Other.Components
{
    public class TimeRemaining : MonoBehaviour
    {
        public enum CounterModeType { Up, Down }
        public CounterModeType CounterMode;
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
                _localisationString = LocaliseText.Get(LocalisationKey);

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
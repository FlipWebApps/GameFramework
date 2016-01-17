//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using System;
using FlipWebApps.GameFramework.Scripts.GameObjects.Components;
using FlipWebApps.GameFramework.Scripts.GameStructure.Levels.ObjectModel;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Levels
{
    /// <summary>
    /// Manages the concept of a level
    /// </summary>
    public class LevelManager : Singleton<LevelManager>
    {
        /// <summary>
        /// Whether LevelStarted should be called on Start()
        /// </summary>
        public bool AutoStart;

        public DateTime StartTime { get; set; }
        public float SecondsRunning { get; set; }
        public bool IsLevelStarted { get; set; }
        public bool IsLevelFinished { get; set; }
        public bool IsLevelRunning { get { return IsLevelStarted && !IsLevelFinished; } }
        public Level Level { get { return GameManager.Instance.Levels != null ? GameManager.Instance.Levels.Selected : null; } }

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

        public void Reset()
        {
            IsLevelStarted = false;
            IsLevelFinished = false;

            if (Level != null) {
                Level.Coins = 0;
                Level.Score = 0;
            }
        }

        public void LevelStarted()
        {
            StartTime = DateTime.Now;
            SecondsRunning = 0f;
            IsLevelStarted = true;
        }

        public void LevelFinished()
        {
            IsLevelFinished = true;
            GameManager.Instance.TimesLevelsPlayed++;
            GameManager.Instance.TimesPlayedForRatingPrompt++;
        }

        void Update() {
            if (IsLevelStarted && !IsLevelFinished)
            {
                SecondsRunning += Time.deltaTime;
            }
        }
    }
}
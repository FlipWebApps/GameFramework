//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using System;
using System.Collections;
using FlipWebApps.GameFramework.Scripts.GameObjects;
using FlipWebApps.GameFramework.Scripts.GameObjects.Components;
using FlipWebApps.GameFramework.Scripts.GameStructure;
using FlipWebApps.GameFramework.Scripts.Localisation;
using FlipWebApps.GameFramework.Scripts.UI.Dialogs.Components;
using FlipWebApps.GameFramework.Scripts.UI.Other;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.FreePrize.Components
{
    public class FreePrizeManager : SingletonPersistantSavedState<FreePrizeManager>
    {
        [Header("Time")]
        public MinMax DelayRangeToNextCountdown = new MinMax() {Min= 0, Max = 0};       // default to no wait
        public MinMax TimeRangeToNextPrize = new MinMax() { Min = 600, Max = 1800 };    // 10 minutes to 30 minutes
        public float DialogShowButtonDelay;

        [Header("Prize")]
        public MinMax ValueRange = new MinMax() { Min = 10, Max = 20 };                // value detaults
        public AudioClip PrizeDialogClosedAudioClip;

        public DateTime NextCountdownStart { get; set; }
        public DateTime NextFreePrizeAvailable { get; set; }
        public int CurrentPrizeAmount {get; set; }

        protected override void GameSetup()
        {
            base.GameSetup();

            StartNewCountdown();

            NextCountdownStart = DateTime.Parse(PlayerPrefs.GetString("FreePrize.NextCountdownStart", DateTime.UtcNow.ToString())); // start countdown immediately if new game
            NextFreePrizeAvailable = DateTime.Parse(PlayerPrefs.GetString("FreePrize.NextPrize", NextFreePrizeAvailable.ToString()));
        }

        public override void SaveState()
        {
            Debug.Log("FreePrizeManager: SaveState");

            PlayerPrefs.SetString("FreePrize.NextCountdownStart", NextCountdownStart.ToString());
            PlayerPrefs.SetString("FreePrize.NextPrize", NextFreePrizeAvailable.ToString());
            PlayerPrefs.Save();
        }

        public void MakePrizeAvailable()
        {
            NextCountdownStart = DateTime.UtcNow;
            NextFreePrizeAvailable = DateTime.UtcNow;

            SaveState();
        }

        public void StartNewCountdown()
        {
            CurrentPrizeAmount = UnityEngine.Random.Range(ValueRange.Min, ValueRange.Max);

            CalculateNextCountdownStart();
            CalculateNextFreePrizeAvailable();

            SaveState();
        }

        void CalculateNextCountdownStart()
        {
            DateTime newCountdownStart = DateTime.UtcNow;
            NextCountdownStart = newCountdownStart.AddSeconds(UnityEngine.Random.Range(DelayRangeToNextCountdown.Min, DelayRangeToNextCountdown.Max + 1));
        }

        void CalculateNextFreePrizeAvailable()
        {
            DateTime newPrizeDateTime = NextCountdownStart;
            NextFreePrizeAvailable = newPrizeDateTime.AddSeconds(UnityEngine.Random.Range(TimeRangeToNextPrize.Min, TimeRangeToNextPrize.Max + 1));
        }

        public bool IsCountingDown()
        {
            return GetTimeToPrize().TotalSeconds > 0 && GetTimeToCountDown().TotalSeconds <= 0;
        }

        public bool IsPrizeAvailable()
        {
            return GetTimeToPrize().TotalSeconds <= 0;
        }

        TimeSpan GetTimeToCountDown()
        {
            return NextCountdownStart.Subtract(DateTime.UtcNow);
        }

        public TimeSpan GetTimeToPrize()
        {
            return NextFreePrizeAvailable.Subtract(DateTime.UtcNow);
        }

        //
        // FOR FREE PRIZE THAT GIVES THE PLAYER COINS
        //
        public void ShowFreePrizeDialog(string prefab = null, string contentPrefab = "FreePrizePlaceHolder")
        {
            StartCoroutine(ShowFreePrizeCoRoutine(prefab, contentPrefab));
        }

        IEnumerator ShowFreePrizeCoRoutine(string prefab, string contentPrefab)
        {
            DialogInstance dialogInstance = DialogManager.Instance.Create(prefab, contentPrefab, 2);

            string text = LocaliseText.Format("FreePrize.Text1", CurrentPrizeAmount);
            UIHelper.SetTextOnChildGameObject(dialogInstance.Content, "fpph_Text", text, true);
            UIHelper.SetTextOnChildGameObjectLocalised(dialogInstance.Content, "fpph_Text2", "FreePrize.Text2", true);

            dialogInstance.Show(title: LocaliseText.Get("FreePrize.Title"), doneCallback: ShowFreePrizeDone);

            yield return new WaitForSeconds(DialogShowButtonDelay);

            GameObjectHelper.GetChildNamedGameObject(dialogInstance.gameObject, "OkButton", true).SetActive(true);
        }

        public virtual void ShowFreePrizeDone(DialogInstance dialogInstance)
        {
            GameManager.Instance.GetPlayer().Coins += CurrentPrizeAmount;
            if (PrizeDialogClosedAudioClip != null)
                GameManager.Instance.PlayEffect(PrizeDialogClosedAudioClip);
            GameManager.Instance.Player.UpdatePlayerPrefs();

            StartNewCountdown();
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
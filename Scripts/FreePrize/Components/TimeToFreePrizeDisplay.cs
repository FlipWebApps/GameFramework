//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using System;
using FlipWebApps.GameFramework.Scripts.GameObjects.Components;
using FlipWebApps.GameFramework.Scripts.Localisation;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.FreePrize.Components
{
    /// <summary>
    /// Shows the amount of time until the free prize is available
    /// </summary>
    [RequireComponent(typeof(UnityEngine.UI.Text))]
    public class TimeToFreePrizeDisplay : RunOnState
    {
        UnityEngine.UI.Text _text;

        public new void Awake()
        {
            base.Awake();

            _text = GetComponent<UnityEngine.UI.Text>();
        }


        public override void RunMethod()
        {
            TimeSpan time = FreePrizeManager.Instance.GetTimeToPrize();
            _text.text = LocaliseText.Format("FreePrize.NewPrize", time.Hours, time.Minutes, time.Seconds);
 }
    }
}
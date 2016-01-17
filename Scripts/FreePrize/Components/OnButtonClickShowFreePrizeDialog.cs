//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameObjects.Components;
using UnityEngine.Assertions;

namespace FlipWebApps.GameFramework.Scripts.FreePrize.Components
{
    /// <summary>
    /// Show the free prize dialog when the button is clicked.
    /// 
    /// This automatically hooks up the button onClick listener
    /// </summary>
    public class OnButtonClickShowFreePrizeDialog : OnButtonClick
    {
        public override void OnClick()
        {
            Assert.IsTrue(FreePrizeManager.IsActive, "You need to add the FreePrizeManager to the scene.");

            FreePrizeManager.Instance.ShowFreePrizeDialog();
        }
    }
}
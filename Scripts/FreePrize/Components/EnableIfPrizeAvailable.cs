//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameObjects.Components;
using UnityEngine;
using UnityEngine.Assertions;

namespace FlipWebApps.GameFramework.Scripts.FreePrize.Components
{
    /// <summary>
    /// Shows an enabled or a disabled gameobject based upon whether there is a free prize available
    /// </summary>
    public class EnableIfPrizeAvailable : RunOnState
    {
        public GameObject PrizeAvailableGameObject;
        public GameObject PrizeCountdownGameObject;
        public GameObject DelayGameObject;

        public override void RunMethod()
        {
            Assert.IsTrue(FreePrizeManager.IsActive, "Please ensure that FlipWebApps.GameFramework.Scripts.FreePrize.Components.FreePrizeManager is added to Edit->ProjectSettings->ScriptExecution before 'Default Time'.\n" +
                                                "FlipWebApps.GameFramework.Scripts.FreePrize.Components.EnableIfPrizeAvailable does not necessarily need to appear in this list, but if it does ensure FreePrizeManager comes first");

            var isPrizeAvailable = FreePrizeManager.Instance.IsPrizeAvailable();
            var isCountingDown = FreePrizeManager.Instance.IsCountingDown();

            if (PrizeAvailableGameObject != null)
                PrizeAvailableGameObject.SetActive(isPrizeAvailable);

            if (PrizeCountdownGameObject != null)
                PrizeCountdownGameObject.SetActive(isCountingDown);

            if (DelayGameObject != null)
                DelayGameObject.SetActive(!isPrizeAvailable && !isCountingDown);
        }
    }

}
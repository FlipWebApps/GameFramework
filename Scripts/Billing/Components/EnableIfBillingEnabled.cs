//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameObjects.Components;

namespace FlipWebApps.GameFramework.Scripts.Billing.Components
{
    /// <summary>
    /// Enabled or a disabled a gameobject based upon whether the facebook SDK is installed
    /// </summary>
    public class EnableIfBillingEnabled : EnableDisableGameObject
    {
        public override bool IsConditionMet()
        {
#if UNITY_PURCHASING
            return true;
#else
            return false;
#endif
        }
    }
}
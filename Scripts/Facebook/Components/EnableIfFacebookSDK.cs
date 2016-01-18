//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameObjects.Components;

namespace FlipWebApps.GameFramework.Scripts.Facebook.Components
{
    /// <summary>
    /// Enabled or a disabled a gameobject based upon whether the facebook SDK is installed
    /// </summary>
    public class EnableIfFacebookSDK : EnableDisableGameObject
    {
        public override bool IsConditionMet()
        {
#if FACEBOOK_SDK
            return true;
#else
            return false;
#endif
        }
    }
}
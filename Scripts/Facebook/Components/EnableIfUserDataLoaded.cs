//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameObjects.Components;

namespace FlipWebApps.GameFramework.Scripts.Facebook.Components
{
    /// <summary>
    /// Shows an enabled or a disabled gameobject based upon whether the users data is loaded
    /// </summary>
    public class EnableIfUserDataLoaded : EnableDisableGameObject
    {
        public override bool IsConditionMet()
        {
#if FACEBOOK_SDK
            return FacebookManager.Instance.IsLoggedIn && FacebookManager.Instance.IsUserDataLoaded;
#else
            return false;
#endif
        }
    }
}
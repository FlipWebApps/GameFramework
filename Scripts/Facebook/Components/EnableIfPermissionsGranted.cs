//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameObjects.Components;

namespace FlipWebApps.GameFramework.Scripts.Facebook.Components
{
    /// <summary>
    /// Shows an enabled or a disabled gameobject based upon whether the specified facebook permissions are granted
    /// </summary>
    public class EnableIfPermissionsGranted : EnableDisableGameObject
    {
        public bool PermissionUserFriends;
        public bool PermissionPublishActions;

        public override bool IsConditionMet()
        {
#if FACEBOOK_SDK
            return !((PermissionUserFriends && !FacebookManager.Instance.HasUserFriendsPermission()) ||
                            PermissionPublishActions && !FacebookManager.Instance.HasPublishActionsPermission());
#else
            return false;
#endif
        }
    }
}
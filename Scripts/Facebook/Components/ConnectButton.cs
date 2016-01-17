//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

#if FACEBOOK_SDK
using FlipWebApps.GameFramework.Scripts.GameObjects;
using FlipWebApps.GameFramework.Scripts.Localisation;
using FlipWebApps.GameFramework.Scripts.UI.Other;
using UnityEngine;
using UnityEngine.UI;

namespace FlipWebApps.GameFramework.Scripts.Facebook.Components
{
    /// <summary>
    /// Provides a button that reacts to the current state of the facebook connection.
    /// </summary>
    public class ConnectButton : MonoBehaviour
    {
        public bool ReconnectMode = false;

        private void Start()
        {
            SetButtonText();
            SetProfileImage();
            FacebookManager.Instance.OnConnectingAction += OnConnectingAction;
            FacebookManager.Instance.OnInitLoginAndGetUserDataAction += OnInitLoginAndGetUserDataAction;
            FacebookManager.Instance.OnUserProfilePictureRequestCompleteAction += SetProfileImage;
        }

        public void OnDestroy()
        {
            FacebookManager.Instance.OnConnectingAction -= OnConnectingAction;
            FacebookManager.Instance.OnInitLoginAndGetUserDataAction -= OnInitLoginAndGetUserDataAction;
            FacebookManager.Instance.OnUserProfilePictureRequestCompleteAction -= SetProfileImage;
        }

        private void OnClick()
        {
            if (!IsConnected())
            {
                FacebookManager.Instance.InitLoginAndGetUserData();
            }
            else if (ReconnectMode)
            {
                FacebookManager.Instance.InitLoginAndGetUserData();
            }
            else
            {
                FacebookManager.Instance.Logout();
                FacebookManager.Instance.AutoConnectOnStartup = false;
                SetButtonText();
            }
        }

        private void OnConnectingAction()
        {
            SetButtonText();
        }

        private void OnInitLoginAndGetUserDataAction(FacebookManager.FacebookHelperResultType resultType)
        {
            if (resultType == FacebookManager.FacebookHelperResultType.OK)
            {
                FacebookManager.Instance.AutoConnectOnStartup = true;
                SetButtonText();
            }
        }

        private void SetButtonText()
        {
            GameObjectHelper.GetChildNamedGameObject(this.gameObject, "Image", true).SetActive(!IsConnected());
            GameObjectHelper.GetChildNamedGameObject(this.gameObject, "Connect", true).SetActive(!IsConnected());
            UIHelper.SetTextOnChildGameObject(this.gameObject, "Connect", FacebookManager.Instance.IsConnecting ? LocaliseText.Get("Facebook.Connecting") : LocaliseText.Get("Facebook.Connect"), true);
            GameObjectHelper.GetChildNamedGameObject(this.gameObject, "ProfileImage", true).SetActive(IsConnected());
            GameObjectHelper.GetChildNamedGameObject(this.gameObject, "Connected", true).SetActive(IsConnected());
            UIHelper.SetTextOnChildGameObject(this.gameObject, "Connected", ReconnectMode ? LocaliseText.Get("Facebook.Reconnect") : LocaliseText.Get("Facebook.Logout"), true);
        }

        private void SetProfileImage()
        {
            if (FacebookManager.Instance.ProfilePicture != null)
            {
                RawImage profileImage = GameObjectHelper.GetChildNamedGameObject(this.gameObject, "ProfileImage", true).GetComponent<RawImage>();
                profileImage.texture = FacebookManager.Instance.ProfilePicture;
            }
        }

        private static bool IsConnected()
        {
            return FacebookManager.Instance.AutoConnectOnStartup && FacebookManager.Instance.IsUserDataLoaded;
        }
    }
}
#endif
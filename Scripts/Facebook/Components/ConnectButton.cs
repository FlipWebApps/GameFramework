//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//
// Please direct any bugs/comments/suggestions to http://www.flipwebapps.com
// 
// The copyright owner grants to the end user a non-exclusive, worldwide, and perpetual license to this Asset
// to integrate only as incorporated and embedded components of electronic games and interactive media and 
// distribute such electronic game and interactive media. End user may modify Assets. End user may otherwise 
// not reproduce, distribute, sublicense, rent, lease or lend the Assets. It is emphasized that the end 
// user shall not be entitled to distribute or transfer in any way (including, without, limitation by way of 
// sublicense) the Assets in any other way than as integrated components of electronic games and interactive media. 

// The above copyright notice and this permission notice must not be removed from any files.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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
    [AddComponentMenu("Game Framework/Facebook/ConnectButton")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
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
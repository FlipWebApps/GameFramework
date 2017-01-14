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
using GameFramework.Facebook.Messages;
using GameFramework.GameStructure;
using GameFramework.Messaging;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.Facebook.Components
{
    /// <summary>
    /// Support for login / logout functionality.
    /// </summary>
    /// Create seperate gameobjects for the different states Connect, Connecting and Connected.
    /// 
    /// If you want to create simple login / logout functionality then create 3 button gameobjects:
    /// 1. Login - Assign to ConnectGameObject and add Login() method to click event.
    /// 2. Connecting - A disabled button or label assigned to ConnectingGameOBject.
    /// 3. Logout - A logout button assigned to ConnectedGameObject and add Logout() method to click event.
    [AddComponentMenu("Game Framework/Facebook/FacebookConnection")]
    [HelpURL("http://www.flipwebapps.com/game-framework/facebook/")]
    public class FacebookConnection : MonoBehaviour
    {
        /// <summary>
        /// A GameObject to show when not logged in or trying to connect
        /// </summary>
        [Tooltip("A GameObject to show when not logged in or trying to connect")]
        public GameObject ConnectGameObject;

        /// <summary>
        /// A GameObject to show when trying to connect
        /// </summary>
        [Tooltip("A GameObject to show when trying to connect")]
        public GameObject ConnectingGameObject;

        /// <summary>
        /// A GameObject to show when logged in
        /// </summary>
        [Tooltip("A GameObject to show when logged in")]
        public GameObject ConnectedGameObject;

        /// <summary>
        /// A RawImage placeholder for the users profile image
        /// </summary>
        [Tooltip("A RawImage placeholder for the users profile image")]
        public RawImage ProfileImage;

        /// <summary>
        /// Whether to enable AutoConnect if the user logs in.
        /// </summary>
        [Tooltip("Whether to enable AutoConnect if the user logs in.")]
        public bool EnableAutoConnect;

        void Awake()
        {
            UpdateStatus();
            SetProfileImage();
            GameManager.SafeAddListener<FacebookConnectingMessage>(OnFacebookConnectingMessage);
            GameManager.SafeAddListener<FacebookLoginMessage>(OnFacebookLoginMessage);
            GameManager.SafeAddListener<FacebookUserPictureMessage>(OnFacebookUserPictureMessage);
        }

        void OnDestroy()
        {
            GameManager.SafeRemoveListener<FacebookUserPictureMessage>(OnFacebookUserPictureMessage);
            GameManager.SafeRemoveListener<FacebookLoginMessage>(OnFacebookLoginMessage);
            GameManager.SafeRemoveListener<FacebookConnectingMessage>(OnFacebookConnectingMessage);
        }

        /// <summary>
        /// Button event handler to login.
        /// </summary>
        public void Login()
        {
            FacebookManager.Instance.Login();
        }

        /// <summary>
        /// Button event handler to logout.
        /// </summary>
        public void Logout()
        {
            FacebookManager.Instance.Logout();
            UpdateStatus();
        }


        bool OnFacebookConnectingMessage(BaseMessage message)
        {
            UpdateStatus();
            return true;
        }


        bool OnFacebookLoginMessage(BaseMessage message)
        {
            var facebookLoginMessage = message as FacebookLoginMessage;
            if (facebookLoginMessage.Result == FacebookLoginMessage.ResultType.OK)
            {
                if (EnableAutoConnect) FacebookManager.Instance.AutoConnectOnStartup = true;
                UpdateStatus();
            }
            return true;
        }


        bool OnFacebookUserPictureMessage(BaseMessage message)
        {
            SetProfileImage();
            return true;
        }


        void UpdateStatus()
        {
            if (FacebookManager.Instance.IsConnecting)
            {
                ConnectGameObject.SetActive(false);
                ConnectingGameObject.SetActive(true);
                ConnectedGameObject.SetActive(false);
            }
            else if (FacebookManager.Instance.IsUserDataLoaded)
            {
                ConnectGameObject.SetActive(false);
                ConnectingGameObject.SetActive(false);
                ConnectedGameObject.SetActive(true);
            }
            else
            {
                ConnectGameObject.SetActive(true);
                ConnectingGameObject.SetActive(false);
                ConnectedGameObject.SetActive(false);
            }
        }

        void SetProfileImage()
        {
            if (FacebookManager.Instance.ProfilePicture != null && ProfileImage != null)
            {
                ProfileImage.texture = FacebookManager.Instance.ProfilePicture;
            }
        }
    }
}
#endif

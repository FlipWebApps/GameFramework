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
using System;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;
using Facebook.Unity;
using GameFramework.Debugging;
using GameFramework.GameObjects.Components;
using GameFramework.GameStructure;
using GameFramework.Helper;
using GameFramework.Localisation;
using GameFramework.UI.Dialogs.Components;
using GameFramework.UI.Other.Components;
using UnityEngine;
using GameFramework.EditorExtras;
using GameFramework.Preferences;
using GameFramework.Facebook.Messages;
using GameFramework.Messaging;
using UnityEngine.Assertions;

namespace GameFramework.Facebook.Components
{
    /// <summary>
    /// Functionality to handle logging into facebook and interactions such as posting updates, inviting friends etc.
    /// </summary>
    [AddComponentMenu("Game Framework/Facebook/FacebookManager")]
    [HelpURL("http://www.flipwebapps.com/game-framework/facebook/")]
    public class FacebookManager : SingletonPersistant<FacebookManager>
    {
        public enum FacebookHelperResultType { ERROR, CANCELLED, OK };

        /// <summary>
        /// Enum for whether permssions are not needed, optional or required.
        /// </summary>
        public enum PermissionRequestType { NONE, OPTIONAL, REQUIRED };

        #region Inspector properties

        /// <summary>
        /// Whether to try and obtain the Email permission when connecting
        /// </summary>
        [Tooltip("Whether to try and obtain the Email permission when connecting")]
        public PermissionRequestType RequestEmailPermission = PermissionRequestType.NONE;

        /// <summary>
        /// Whether to try and obtain the UserFriends permission when connecting
        /// </summary>
        [Tooltip("Whether to try and obtain the UserFriends permission when connecting")]
        public PermissionRequestType RequestUserFriendsPermission = PermissionRequestType.NONE;

        /// <summary>
        /// Whether to try and obtain the PulishActions permission when connecting
        /// </summary>
        [Tooltip("Whether to try and obtain the PulishActions permission when connecting")]
        public PermissionRequestType RequestPublishActionsPermission = PermissionRequestType.NONE;

        /// <summary>
        /// Whether to try and load any friends that are using this app / game
        /// </summary>
        [Tooltip("Whether to try and load any friends that are using this app / game")]
        public bool LoadFriendsUsingApp = false;

        /// <summary>
        /// Whether to try and pre load any friends images
        /// </summary>
        [Tooltip("Whether to try and pre load any friends images")]
        [ConditionalHide("LoadFriendsUsingApp", hideInInspector: false)]
        public bool PreloadFriendImages = false;

        /// <summary>
        /// Whether to try and obtain the Email permission when connectiong
        /// </summary>
        [Header("ShareLink")]
        [Tooltip("(optional) A default url when calling ShareLink if nothing else is specifed.")]
        public string ShareLinkUrl;

        #endregion Inspector properties

        #region Properties - State

        /// <summary>
        /// Whether we are currently trying to connect to Facebook
        /// </summary>
        public bool IsConnecting { get; private set; }

        /// <summary>
        /// Whether Facebook is initalised
        /// </summary>
        public bool IsInitialized { get { return FB.IsInitialized; } }

        /// <summary>
        /// Whether Facebook is logged in
        /// </summary>
        public bool IsLoggedIn { get { return FB.IsLoggedIn; } }

        /// <summary>
        /// Whether we have loaded user data about the user.
        /// </summary>
        public bool IsUserDataLoaded { get; private set; }

        /// <summary>
        /// Whether the users picture is loaded.
        /// </summary>
        public bool IsUserPictureLoaded { get; private set; }

        /// <summary>
        /// Whether the users friends are loaded.
        /// </summary>
        public bool IsFriendsLoaded { get; private set; }

        /// <summary>
        /// Whether the user has invited friends.
        /// </summary>
        /// This value is saved and reloaded so persists across game restarts.
        public bool HasInvitedFriends
        {
            get
            {
                return PreferencesFactory.GetInt("Facebook.HasInvitedFriends", 0) != 0;
            }
            set
            {
                PreferencesFactory.SetInt("Facebook.HasInvitedFriends", value ? 1 : 0);
                PreferencesFactory.Save();
            }
        }

        /// <summary>
        /// The number of friends invites that the user has sent
        /// </summary>
        /// This value is saved and reloaded so persists across game restarts.
        public int NumberOfInvitesSent
        {
            get
            {
                return PreferencesFactory.GetInt("Facebook.NumberOfInvitesSent", 0);
            }
            set
            {
                PreferencesFactory.SetInt("Facebook.NumberOfInvitesSent", value);
                PreferencesFactory.Save();
            }
        }

        /// <summary>
        /// Whether to try and automatically connect to facebook on startup
        /// </summary>
        public bool AutoConnectOnStartup
        {
            get
            {
                return PlayerPrefs.GetInt("Facebook.AutoConnectOnStartup", 0) != 0;
            }
            set
            {
                PlayerPrefs.SetInt("Facebook.AutoConnectOnStartup", value ? 1 : 0);
                PlayerPrefs.Save();
            }
        }

        # endregion Properties - State
        #region Properties - Profile

        /// <summary>
        /// The users first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The users profile picture
        /// </summary>
        public Texture2D ProfilePicture { get; set; }

        /// <summary>
        /// A list of the users facebook friends
        /// </summary>
        public List<FacebookFriend> Friends = new List<FacebookFriend>();

        /// <summary>
        /// Profile images for the users friends (userid, texture)
        /// </summary>
        public Dictionary<string, Texture> ProfileImages = new Dictionary<string, Texture>();

        #endregion Properties Properties - Profile
        #region Properties - Permissions

        /// <summary>
        /// A list of the permissions that the user has granted
        /// </summary>
        public List<string> PermissionsGranted { get; set; }

        /// <summary>
        /// Whether the public_profile permission has been granted
        /// </summary>
        public bool HasPublicProfilePermission
        {
            get { return PermissionsGranted.Contains("public_profile"); }
        }

        /// <summary>
        /// Whether the email permission has been granted
        /// </summary>
        public bool HasEmailPermission
        {
            get
            { return PermissionsGranted.Contains("email"); }
        }

        /// <summary>
        /// Whether the user_friends permission has been granted
        /// </summary>
        public bool HasUserFriendsPermission
        {
            get { return PermissionsGranted.Contains("user_friends"); }
        }

        /// <summary>
        /// Whether the publish_actions permission has been granted
        /// </summary>
        public bool HasPublishActionsPermission
        {
            get { return PermissionsGranted.Contains("publish_actions"); }
        }

        # endregion Properties - Permissions

        #region Scores API (deprecated for now)
        //--------------------------------------
        //  Scores API 
        //  https://developers.facebook.com/docs/games/scores
        //------------------------------------
        //public Action<FacebookHelperResultType, FBResult> OnAppScoresRequestCompleteAction = delegate {};
        //public Action<FacebookHelperResultType, FBResult> OnSubmitScoreRequestCompleteAction = delegate {};
        //   public Action<FacebookHelperResultType, FBResult> OnDeleteScoresRequestCompleteAction = delegate { };

        //private int lastSubmitedScore = 0;
        //public List<HighScore> scores = new List<HighScore>();
        #endregion Scores API (deprecated for now)

        protected override void GameSetup()
        {
            Assert.IsTrue(GameManager.IsActive, "Please ensure that you have a GameManager added to your scene and that it is added to 'Main Menu->Edit->Project Settings->Script Execution Order' before 'Default Time'.\n" +
                                                "FacebookManager does not necessarily need to appear in this list, but if it does ensure GameManager comes first");

            base.GameSetup();

            IsConnecting = false;
            IsUserDataLoaded = false;
            IsUserPictureLoaded = false;
            IsFriendsLoaded = false;
            PermissionsGranted = new List<string>();

            if (AutoConnectOnStartup)
                Login();
        }

        #region Login / Logout

        /// <summary>
        /// Try and log the user into Facebook and gather their basic user data
        /// </summary>
        public void Login()
        {
            if (IsConnecting)
            {
                MyDebug.Log("Facebook Helper: Already Connecting. Check IsConnecting Flag before calling");
                return;
            }
            IsConnecting = true;
            GameManager.SafeQueueMessage(new FacebookConnectingMessage());

            if (!IsInitialized)
            {
                FB.Init(OnInitComplete, OnHideUnity);
            }
            else {
                FB.ActivateApp();
                OnInitComplete();
            }
        }

        /// <summary>
        /// Log the user out from Facebook
        /// </summary>
        /// Also clears the AutoConnectOnStartup flag as if we log out it doesnæt make sense to try and login!
        public void Logout()
        {
            FB.LogOut();
            IsConnecting = false;
            IsUserDataLoaded =false;
            AutoConnectOnStartup = false;
        }

        /// <summary>
        /// Login step 2. Initialisation complete - get extra permissions if needed.
        /// </summary>
        void OnInitComplete()
        {
            Debug.Log("OnInitComplete: Is user logged in? " + FB.IsLoggedIn);

            // if necessary get extra permissions before calling login callback
            if (IsLoggedIn && HasPublicProfilePermission &&
                (RequestEmailPermission != PermissionRequestType.REQUIRED || HasEmailPermission) &&
                (RequestPublishActionsPermission != PermissionRequestType.REQUIRED || HasPublishActionsPermission) &&
                (RequestUserFriendsPermission != PermissionRequestType.REQUIRED || HasUserFriendsPermission))
            {
                LoginCallback(null);
            }
            else
            {
                //GameManager.Instance.IsUserInteractionEnabled = false;
                List<string> permissions = new List<string>();
                permissions.Add("public_profile");
                if (RequestEmailPermission != PermissionRequestType.NONE) permissions.Add("email");
                if (RequestUserFriendsPermission != PermissionRequestType.NONE) permissions.Add("user_friends");
                if (RequestPublishActionsPermission != PermissionRequestType.NONE) permissions.Add("publish_actions");

                // do different login depending upon whether we need read permissions or publish permissions.
                if (RequestPublishActionsPermission != PermissionRequestType.NONE)
                {
                    permissions.Add("publish_actions");
                    FB.LogInWithPublishPermissions(permissions, LoginCallback);
                }
                else
                {
                    FB.LogInWithReadPermissions(permissions, LoginCallback);
                }

            }
        }


        /// <summary>
        /// Login step 3. Logged in - user data.
        /// </summary>
        void LoginCallback(ILoginResult result)
        {
            FBLog("LoginCallback", result != null ? result.RawResult : "");

            //GameManager.Instance.IsUserInteractionEnabled = true;

            if (IsLoggedIn)
            {
                FB.ActivateApp();

                // Get current access token's granted permissions
                foreach (string permission in AccessToken.CurrentAccessToken.Permissions)
                {
                    PermissionsGranted.Add(permission);
                }

                // and try and load user data
                LoadUserData();
            }
            else
            {
                IsConnecting = false;
                GameManager.SafeQueueMessage(new FacebookLoginMessage(FacebookLoginMessage.ResultType.CANCELLED));
            }
        }


        /// <summary>
        /// Login step 4. Get user data, picture and friends if set.
        /// </summary>

        private void LoadUserData()
        {
            // load user data
            if (!IsUserDataLoaded)
            {
                FB.API("/me", HttpMethod.GET, UserDataCallBack);
            }
            else
            {
                IsConnecting = false;
                GameManager.SafeQueueMessage(new FacebookLoginMessage(FacebookLoginMessage.ResultType.OK));
            }

            // load the picture
            if (!IsUserPictureLoaded)
            {
                LoadPictureAPI(GetPictureURL("me", (int)(Screen.height / 15.36f) * 3, (int)(Screen.height / 15.36f) * 3), MyPictureCallback);
            }
            else
            {
                GameManager.SafeQueueMessage(new FacebookUserPictureMessage(FacebookUserPictureMessage.ResultType.OK, ProfilePicture));
            }

            // load friend data
            if (LoadFriendsUsingApp)
            {
                FB.API("/v2.2/me/friends", HttpMethod.GET, FriendsUsingAppCallBack);
            }
        }

        /// <summary>
        /// Login step 5a. user data callback
        /// </summary>
        private void UserDataCallBack(IGraphResult result)
        {
            FBLog("UserDataCallBack", result.RawResult);

            if (result.Error != null)
            {
                IsConnecting = false;
                GameManager.SafeQueueMessage(new FacebookLoginMessage(FacebookLoginMessage.ResultType.ERROR));
            }
            else
            {
                // extract user data.
                var responseObject = Json.Deserialize(result.RawResult) as Dictionary<string, object>;
                object nameH;
                if (responseObject.TryGetValue("first_name", out nameH))
                {
                    FirstName = (string)nameH;
                }

                IsUserDataLoaded = true;
                IsConnecting = false;
                GameManager.SafeQueueMessage(new FacebookLoginMessage(FacebookLoginMessage.ResultType.OK));
            }
        }

        /// <summary>
        /// Login step 5b. friends list callback
        /// </summary>
        private void FriendsUsingAppCallBack(IGraphResult result)
        {
            FBLog("FriendsDataCallBack", result.RawResult);
            var resultType = FacebookFriendsUsingAppMessage.ResultType.ERROR;

            if (result != null && result.Error == null)
            {
                JSONObject jsonObject = JSONObject.Parse(result.RawResult);
                JSONValue dataArray = jsonObject.GetArray("data");

                foreach (JSONValue friend in dataArray.Array)
                {
                    string userId = friend.Obj.GetString("id");
                    string UserName = friend.Obj.GetString("name");
                    Debug.Log("NAME " + UserName);
                    if (PreloadFriendImages)
                        LoadProfileImage(userId);
                    FacebookFriend facebookFriend = new FacebookFriend();
                    facebookFriend.Id = userId;
                    facebookFriend.Name = UserName;
                    Friends.Add(facebookFriend);
                }

                IsFriendsLoaded = true;
                resultType = FacebookFriendsUsingAppMessage.ResultType.OK;
            }

            GameManager.SafeQueueMessage(new FacebookFriendsUsingAppMessage(resultType));
        }


        /// <summary>
        /// Login step 5c. profile picture callback
        /// </summary>
        void MyPictureCallback(Texture2D texture)
        {
            ProfilePicture = (Texture2D)texture;
            if (!ProfileImages.ContainsKey(AccessToken.CurrentAccessToken.UserId))
                ProfileImages.Add(AccessToken.CurrentAccessToken.UserId, ProfilePicture);

            IsUserPictureLoaded = true;

            GameManager.SafeQueueMessage(new FacebookUserPictureMessage(FacebookUserPictureMessage.ResultType.OK, ProfilePicture));
            GameManager.SafeQueueMessage(new FacebookProfilePictureMessage(AccessToken.CurrentAccessToken.UserId, ProfilePicture));
        }

        #endregion Login / Logout

        #region OnHideUnity

        void OnHideUnity(bool isGameShown)
        {
            //GameManager.Instance.IsUserInteractionEnabled = !isGameShown;
            GameManager.SafeQueueMessage(new FacebookHideUnityMessage(isGameShown));
        }

        #endregion OnHideUnity

        #region ShareLink

        /// <summary>
        /// Share a link to Facebook, automatically handling login if necessary.
        /// </summary>
        /// Many of the parameters are optional and will be set to default values if not specified (see parameter arguments).
        /// <param name="contentURL">Content url. If not specified then uses FacebookManager.Instance.PostLink</param>
        /// <param name="autoLogin">Whether to automatically login to Facebook if not already done so.</param>
        /// <returns>true if the show process was started, flase if not logged in and not auto logging in either.</returns>
        public bool ShareLink(Uri contentURL = null, bool autoLogin = true)
        {
            // if already logged in then just share the link otherwise check for auto login.
            if (IsLoggedIn)
            {
                ShareLinkInternal(contentURL);
                return true;
            }
            else { 
                // try and auto login if specified, otherwise just return false.
                if (autoLogin)
                {
                    FacebookShareLinkHandler postHandler = new FacebookShareLinkHandler() { contentURL = contentURL };
                    GameManager.SafeAddListener<FacebookLoginMessage>(postHandler.LoginHandler);
                    if (!IsUserDataLoaded)
                        Login();
                    else
                        postHandler.LoginHandler(new FacebookLoginMessage(FacebookLoginMessage.ResultType.OK));

                    return true;
                }
                else
                {
                    MyDebug.LogWarning("Authorise user before calling ShareLink.");
                    return false;
                }
            }
        }


        /// <summary>
        /// This is a seperate class so we can safely handle multiple concurrent calls with different data.
        /// </summary>
        class FacebookShareLinkHandler
        {
            public Uri contentURL = null;

            public bool LoginHandler(BaseMessage message)
            {
                var facebookLoginMessage = message as FacebookLoginMessage;
                GameManager.SafeRemoveListener<FacebookLoginMessage>(LoginHandler);
                if (facebookLoginMessage.Result == FacebookLoginMessage.ResultType.OK)
                {
                    FacebookManager.Instance.ShareLinkInternal(contentURL);
                }
                else
                {
                    DialogManager.Instance.ShowError(textKey: "Facebook.Error.Login.Description");
                }
                return true;
            }
        }


        /// <summary>
        /// Do the actual share. See the definition of ShareLink for fallback values incase of nulls being passed.
        /// </summary>
        /// <param name="contentURL"></param>
        /// Note that From Graph API 2.9? title, description and photoURL are no longer used.
        void ShareLinkInternal(Uri contentURL)
        {
            if (contentURL == null)
            {
                Assert.IsFalse(string.IsNullOrEmpty(FacebookManager.Instance.ShareLinkUrl), "FacebookManager.ShareLinkInternal: FacebookManager.Instance.PostLink is null or empty and no contentURL passed");
                contentURL = new Uri(FacebookManager.Instance.ShareLinkUrl);
            }

            FB.ShareLink(contentURL, callback: ShareLinkCallback);
        }


        /// <summary>
        /// Callback for when sharing is done
        /// </summary>
        /// <param name="result"></param>
        void ShareLinkCallback(IShareResult result)
        {
            FBLog("ShareLinkCallback", result.RawResult);
            FacebookShareLinkMessage.ResultType resultType = FacebookShareLinkMessage.ResultType.ERROR;

            if (result != null)
            {
                if (result.Error == null)
                {
                    var responseObject = Json.Deserialize(result.RawResult) as Dictionary<string, object>;
                    object obj = 0;
                    if (responseObject.TryGetValue("cancelled", out obj))
                    {
                        resultType = FacebookShareLinkMessage.ResultType.CANCELLED;
                    }
                    else if (responseObject.TryGetValue("id", out obj))
                    {
                        resultType = FacebookShareLinkMessage.ResultType.OK; 
                    }
                }
            }
            GameManager.SafeQueueMessage(new FacebookShareLinkMessage(resultType, result.RawResult));
        }

        #endregion ShareLink

        #region AppRequest

        /// <summary>
        /// Show an AppRequest dialog, automatically handling login if necessary.
        /// </summary>
        /// <param name="message">Message to show. If not specified then uses localisation key Facebook.Invite.Caption with the game name as a parameter</param>
        /// <param name="to"></param>
        /// <param name="filters"></param>
        /// <param name="excludeIds"></param>
        /// <param name="maxRecipients"></param>
        /// <param name="data"></param>
        /// <param name="title">Title to show. If not specified then uses localisation key Facebook.Invite.Description with the game name as a parameter</param>
        /// <param name="autoLogin">Whether to automatically login to Facebook if not already done so.</param>
        /// <returns>true if the show process was started, flase if not logged in and not auto logging in either.</returns>
        public bool AppRequest(string message = null, string[] to = null, List<object> filters = null, string[] excludeIds = null,
            int? maxRecipients = null, string data = "", string title = "", bool autoLogin = true)

        {
            // if already logged in then just share the link otherwise check for auto login.
            if (IsLoggedIn)
            {
                AppRequestInternal(message, to, filters, excludeIds, maxRecipients, data, title);
                return true;
            }
            else
            {
                // try and auto login if specified, otherwise just return false.
                if (autoLogin)
                {
                    FacebookShareHandler postHandler = new FacebookShareHandler() { Message = message, Title = title };
                    GameManager.SafeAddListener<FacebookLoginMessage>(postHandler.LoginHandler);
                    if (!IsUserDataLoaded)
                        Login();
                    else
                        postHandler.LoginHandler(new FacebookLoginMessage(FacebookLoginMessage.ResultType.OK));

                    return true;
                }
                else
                {
                    MyDebug.LogWarning("Authorise user before calling AppRequest.");
                    return false;
                }
            }
        }

        /// <summary>
        /// This is a seperate class so we can safely handle multiple concurrent calls with different data.
        /// </summary>
        class FacebookShareHandler
        {
            public string Message;
            public string Title;

            public bool LoginHandler(BaseMessage message)
            {
                var facebookLoginMessage = message as FacebookLoginMessage;
                GameManager.SafeRemoveListener<FacebookLoginMessage>(LoginHandler);
                if (facebookLoginMessage.Result == FacebookLoginMessage.ResultType.OK)
                {
                    FacebookManager.Instance.AppRequestInternal(message: Message, title: Title);
                }
                else
                {
                    DialogManager.Instance.ShowError(textKey: "Facebook.Error.Login.Description");
                }
                return true;
            }
        }


        /// <summary>
        /// Do the actual app request
        /// </summary>
        /// <param name="message"></param>
        /// <param name="to"></param>
        /// <param name="filters"></param>
        /// <param name="excludeIds"></param>
        /// <param name="maxRecipients"></param>
        /// <param name="data"></param>
        /// <param name="title"></param>
        void AppRequestInternal(string message = null, string[] to = null, List<object> filters = null, string[] excludeIds = null,
            int? maxRecipients = null, string data = "", string title = "")
        {
            FB.AppRequest(
                message != null ? message : LocaliseText.Format("Facebook.Invite.Caption", GameManager.Instance.GameName), 
                to, 
                filters, 
                excludeIds, 
                maxRecipients, 
                data, 
                title != null ? title : LocaliseText.Format("Facebook.Invite.Description", GameManager.Instance.GameName), 
                AppRequestCallBack);
        }

        /// <summary>
        /// Callback for when app request is done
        /// </summary>
        /// <param name="result"></param>
        void AppRequestCallBack(IAppRequestResult result)
        {
            FBLog("AppRequestCallBack", result.RawResult);
            FacebookAppRequestMessage.ResultType resultType = FacebookAppRequestMessage.ResultType.ERROR;
            List<string> InvitedFriends = new List<string>();

            if (result != null && result.Error == null)
            {
                JSONObject jsonObject = JSONObject.Parse(result.RawResult);
                if (jsonObject.ContainsKey("cancelled"))
                {
                    resultType = FacebookAppRequestMessage.ResultType.CANCELLED;
                }
                else if (jsonObject.ContainsKey("request"))
                {
                    //{   "request": "420211088059698",
                    //    "to": [
                    //        "100002669403922",
                    //        "100000048490273"
                    //    ]}
                    resultType = FacebookAppRequestMessage.ResultType.OK;
                    HasInvitedFriends = true;

                    JSONValue to = jsonObject.GetValue("to");
                    if (to != null && to.Type == JSONValueType.Array)
                    {
                        NumberOfInvitesSent += to.Array.Length;
                        foreach (JSONValue value in to.Array)
                        {
                            InvitedFriends.Add(value.Str);
                            Debug.Log("Value: " + value.Str);
                        }
                    }else if (to != null && to.Type == JSONValueType.String)
                    {
                        NumberOfInvitesSent++;
                        InvitedFriends.Add(to.Str);
                    }
                }
            }
            GameManager.SafeQueueMessage(new FacebookAppRequestMessage(resultType, InvitedFriends, result.RawResult));
        }
        #endregion AppRequest

        #region Scores - Deprecated
        ////--------------------------------------
        ////  Scores API 
        ////  https://developers.facebook.com/docs/games/scores
        ////------------------------------------

        ////Read scores for players and friends
        //public void LoadAppScores() {
        //       MyDebug.Log("LoadAppScores");
        //       FB.API("/" + FB.AppId + "/scores", HttpMethod.GET, OnAppScoresComplete);  
        //}

        ////Create or update a score
        //public void SubmitScore(int score) {
        //	lastSubmitedScore = score;
        //	FB.API("/" + AccessToken.CurrentAccessToken.UserId + "/scores?score=" + score, HttpMethod.POST, OnScoreSubmited);  
        //}

        ////Delete scores for a player
        //   public void DeletePlayerScores()
        //   {
        //       FB.API("/" + AccessToken.CurrentAccessToken.UserId + "/scores", HttpMethod.DELETE, OnScoreDeleted);
        //   }

        //   private void OnScoreDeleted(FBResult result)
        //   {
        //       FBLog("OnScoreDeleted", result);
        //       FacebookHelperResultType resultType = FacebookHelperResultType.ERROR;

        //       if (result != null && result.Error == null)
        //       {
        //           JSONObject jsonObject = JSONObject.Parse(result.Text);
        //           if (jsonObject.GetBoolean("success"))
        //           {
        //               resultType = FacebookHelperResultType.OK;
        //               HighScore score = GetOrAddAppScore(AccessToken.CurrentAccessToken.UserId);
        //               score.Score = 0;
        //           }
        //           OnDeleteScoresRequestCompleteAction(resultType, result);
        //       }
        //   }

        //private void OnScoreSubmited(FBResult result) {
        //       FBLog("OnScoreSubmited", result);
        //       FacebookHelperResultType resultType = FacebookHelperResultType.ERROR;

        //	if (result != null && result.Error == null) {
        //           JSONObject jsonObject = JSONObject.Parse(result.Text);
        //           if (jsonObject.GetBoolean("success")) {
        //			resultType = FacebookHelperResultType.OK;
        //               HighScore score = GetOrAddAppScore(AccessToken.CurrentAccessToken.UserId);
        //               score.UserName = FirstName;
        //			score.Score = lastSubmitedScore;
        //               Debug.Log(score);
        //		} 
        //		OnSubmitScoreRequestCompleteAction (resultType, result);
        //	}
        //}

        //private void OnAppScoresComplete(FBResult result) {
        //       FBLog("OnAppScoresComplete", result);
        //       FacebookHelperResultType resultType = FacebookHelperResultType.ERROR;

        //	if (result != null && result.Error == null) {
        //           JSONObject jsonObject = JSONObject.Parse(result.Text);
        //           JSONValue dataArray = jsonObject.GetArray("data");

        //           foreach (JSONValue data in dataArray.Array)
        //           {
        //               JSONObject user = data.Obj.GetObject("user");
        //               string userId = user.GetString("id");
        //               HighScore score = GetOrAddAppScore(userId);
        //               score.UserName = user.GetString("name");
        //               score.Score = (int)data.Obj.GetNumber("score");
        //               LoadProfileImage(userId);
        //		}

        //           // for testing.
        //           //FacebookManager.Instance.scores.Add(new HighScore(AccessToken.CurrentAccessToken.UserId, "Test2", 2));
        //           //FacebookManager.Instance.scores.Add(new HighScore(AccessToken.CurrentAccessToken.UserId, "Test3", 5));

        //           scores.Sort(delegate(HighScore firstObj,
        //                                HighScore secondObj)
        //           {
        //               return -firstObj.Score.CompareTo(secondObj.Score);
        //           }
        //           );

        //           resultType = FacebookHelperResultType.OK;
        //	}
        //       OnAppScoresRequestCompleteAction(resultType, result);
        //}


        //private HighScore GetOrAddAppScore(string userId) {
        //       foreach (HighScore score in scores)
        //       {
        //           if (score.UserId == userId)
        //               return score;
        //       }
        //       HighScore newScore = new HighScore();
        //       newScore.UserId = userId;
        //       scores.Add(newScore);
        //       return newScore;
        //}
        #endregion Scores - Deprecated


        #region Helper functions
        /// <summary>
        /// Load the profile picture for the specified userId.
        /// </summary>
        public void LoadProfileImage(string userId)
        {

            if (!ProfileImages.ContainsKey(userId))
            {
                // We don't have this players image yet, request it now
                LoadPictureAPI(GetPictureURL(userId, 128, 128), pictureTexture =>
                {
                    if (pictureTexture != null && !ProfileImages.ContainsKey(userId))   // check again as by time callback is called it could have been added elsewhere.
                {
                        ProfileImages.Add(userId, pictureTexture);
                        GameManager.SafeQueueMessage(new FacebookProfilePictureMessage(userId, pictureTexture));
                    }
                });
            }
        }

        /// <summary>
        /// Generate a url for downloading a a users picture of the specified dimensions and type
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetPictureURL(string userId, int? width = null, int? height = null, string type = null)
        {
            string url = string.Format("/{0}/picture", userId);
            string query = width != null ? "&width=" + width.ToString() : "";
            query += height != null ? "&height=" + height.ToString() : "";
            query += type != null ? "&type=" + type : "";
            query += "&redirect=false";
            if (query != "") url += ("?g" + query);
            return url;
        }

        delegate void LoadPictureCallback(Texture2D texture);
        IEnumerator LoadPictureEnumerator(string url, LoadPictureCallback callback)
        {
            WWW www = new WWW(url);
            yield return www;
            callback(www.texture);
        }
        void LoadPictureAPI(string url, LoadPictureCallback callback)
        {
            FB.API(url, HttpMethod.GET, result =>
            {
                if (result.Error != null)
                {
                    MyDebug.LogError(result.Error);
                    return;
                }

                var imageUrl = DeserializePictureURLString(result.RawResult);

                StartCoroutine(LoadPictureEnumerator(imageUrl, callback));
            });
        }
        void LoadPictureURL(string url, LoadPictureCallback callback)
        {
            StartCoroutine(LoadPictureEnumerator(url, callback));

        }

        public static string DeserializePictureURLString(string response)
        {
            return DeserializePictureURLObject(Json.Deserialize(response));
        }

        public static string DeserializePictureURLObject(object pictureObj)
        {
            var picture = (Dictionary<string, object>)(((Dictionary<string, object>)pictureObj)["data"]);
            object urlH = null;
            if (picture.TryGetValue("url", out urlH))
            {
                return (string)urlH;
            }

            return null;
        }

        void FBLog(string method, string result)
        {
            //if (result == null)
            Debug.Log(method + ": " + result);  // null FBResult");
                                                //else
                                                //    Debug.Log(method + "\nresult.Error:" + (result.Error == null ? "null" : result.Error) + "\nresult.Text:" + (result.Text == null ? "null" : result.Text));
        }
        #endregion Helper functions

    }
}
#endif

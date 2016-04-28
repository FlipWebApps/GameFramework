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
using FlipWebApps.GameFramework.Scripts.Debugging;
using FlipWebApps.GameFramework.Scripts.GameObjects.Components;
using FlipWebApps.GameFramework.Scripts.GameStructure;
using FlipWebApps.GameFramework.Scripts.Helper;
using FlipWebApps.GameFramework.Scripts.Localisation;
using FlipWebApps.GameFramework.Scripts.UI.Dialogs.Components;
using FlipWebApps.GameFramework.Scripts.UI.Other.Components;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Facebook.Components
{
    /// <summary>
    /// Functionality to handle logging into facebook and interactions such as posting updates, inviting friends etc.
    /// </summary>
    [AddComponentMenu("Game Framework/Facebook/FacebookManager")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class FacebookManager : SingletonPersistant<FacebookManager>
    {
        public enum FacebookHelperResultType { ERROR, CANCELLED, OK };
        public enum PermissionRequestType { NONE, OPTIONAL, REQUIRED };

        public PermissionRequestType RequestEmailPermission = PermissionRequestType.NONE;
        public PermissionRequestType RequestUserFriendsPermission = PermissionRequestType.NONE;
        public PermissionRequestType RequestPublishActionsPermission = PermissionRequestType.NONE;

        public bool LoadFriendsUsingApp = false;
        public bool PreloadFriendImages = true;
        public string PostLink;
        public string PostPicture;

        public bool IsConnecting { get; private set; }
        public bool IsInitialized { get { return FB.IsInitialized; } }
        public bool IsLoggedIn { get { return FB.IsLoggedIn; } }
        public bool IsUserDataLoaded { get; private set; }
        public bool IsUserPictureLoaded { get; private set; }
        public bool IsFriendsLoaded { get; private set; }
        public bool HasInvitedFriends
        {
            get
            {
                return PlayerPrefs.GetInt("Facebook.HasInvitedFriends", 0) != 0;
            }
            set
            {
                PlayerPrefs.SetInt("Facebook.HasInvitedFriends", value ? 1 : 0);
                PlayerPrefs.Save();
            }
        }
        public int NumberOfInvitesSent
        {
            get
            {
                return PlayerPrefs.GetInt("Facebook.NumberOfInvitesSent", 0);
            }
            set
            {
                PlayerPrefs.SetInt("Facebook.NumberOfInvitesSent", value);
                PlayerPrefs.Save();
            }
        }

        public List<string> PermissionsGranted { get; set; }
        public string FirstName { get; set; }
        public Texture2D ProfilePicture { get; set; }
        public List<FacebookFriend> Friends = new List<FacebookFriend>();
        public Dictionary<string, Texture> ProfileImages = new Dictionary<string, Texture>();

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

        public Action OnConnectingAction = delegate { };
        public Action<FacebookHelperResultType> OnInitLoginAndGetUserDataAction = delegate { };
        public Action<FacebookHelperResultType> OnFriendsUsingAppAction = delegate { };
        public Action<bool> OnFocusChanged = delegate { };
        public Action OnUserProfilePictureRequestCompleteAction = delegate { };
        public Action<string, Texture> OnProfilePictureRequestCompleteAction = delegate { };
        public Action<FacebookHelperResultType, IShareResult> OnPostingCompleteAction = delegate { };
        public Action<FacebookHelperResultType, List<string>, IAppRequestResult> OnAppRequestCompleteAction = delegate { };

        //--------------------------------------
        //  Scores API 
        //  https://developers.facebook.com/docs/games/scores
        //------------------------------------
        //public Action<FacebookHelperResultType, FBResult> OnAppScoresRequestCompleteAction = delegate {};
        //public Action<FacebookHelperResultType, FBResult> OnSubmitScoreRequestCompleteAction = delegate {};
        //   public Action<FacebookHelperResultType, FBResult> OnDeleteScoresRequestCompleteAction = delegate { };

        //private int lastSubmitedScore = 0;
        //public List<HighScore> scores = new List<HighScore>();

        protected override void GameSetup()
        {
            base.GameSetup();

            IsConnecting = false;
            IsUserDataLoaded = false;
            IsUserPictureLoaded = false;
            IsFriendsLoaded = false;
            PermissionsGranted = new List<string>();

            if (AutoConnectOnStartup)
                InitLoginAndGetUserData();
        }

        public void InitLoginAndGetUserData()
        {
            if (IsConnecting)
            {
                Debug.Log("Facebook Helper: Already Connecting. Check IsConnecting Flag before calling");
                return;
            }
            IsConnecting = true;
            OnConnectingAction();

            if (!IsInitialized)
            {
                FB.Init(OnInitComplete, OnHideUnity);
            }
            else {
                OnInitComplete();
            }
        }

        public void Logout()
        {
            FB.LogOut();
        }

        private void OnInitComplete()
        {
            Debug.Log("OnInitComplete: Is user logged in? " + FB.IsLoggedIn);

            if (IsLoggedIn && HasPublicProfilePermission() &&
                (RequestEmailPermission != PermissionRequestType.REQUIRED || HasEmailPermission()) &&
                (RequestPublishActionsPermission != PermissionRequestType.REQUIRED || HasPublishActionsPermission()) &&
                (RequestUserFriendsPermission != PermissionRequestType.REQUIRED || HasUserFriendsPermission()))
            {
                LoginCallback(null);
            }
            else
            {
                GameManager.Instance.IsUserInteractionEnabled = false;
                List<string> permissions = new List<string>();
                permissions.Add("public_profile");
                if (RequestEmailPermission != PermissionRequestType.NONE) permissions.Add("email");
                if (RequestPublishActionsPermission != PermissionRequestType.NONE) permissions.Add("publish_actions");
                if (RequestUserFriendsPermission != PermissionRequestType.NONE) permissions.Add("user_friends");

                FB.LogInWithReadPermissions(permissions, LoginCallback);
            }
        }


        private void OnHideUnity(bool isGameShown)
        {
            MyDebug.Log("OnHideUnity: " + isGameShown);

            GameManager.Instance.IsUserInteractionEnabled = !isGameShown;

            OnFocusChanged(isGameShown);
        }


        private void LoginCallback(ILoginResult result)
        {
            FBLog("LoginCallback", result != null ? result.RawResult : "");

            GameManager.Instance.IsUserInteractionEnabled = true;

            if (IsLoggedIn)
            {
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
                OnInitLoginAndGetUserDataAction(FacebookHelperResultType.ERROR);
            }
        }


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
                OnInitLoginAndGetUserDataAction(FacebookHelperResultType.OK);
            }

            // load the picture
            if (!IsUserPictureLoaded)
            {
                LoadPictureAPI(GetPictureURL("me", (int)(Screen.height / 15.36f) * 3, (int)(Screen.height / 15.36f) * 3), MyPictureCallback);
            }
            else
            {
                OnUserProfilePictureRequestCompleteAction();
            }

            // load friend data
            if (LoadFriendsUsingApp)
            {
                FB.API("/v2.2/me/friends", HttpMethod.GET, FriendsUsingAppCallBack);
            }
        }

        private void UserDataCallBack(IGraphResult result)
        {
            FBLog("UserDataCallBack", result.RawResult);

            if (result.Error != null)
            {
                IsConnecting = false;
                OnInitLoginAndGetUserDataAction(FacebookHelperResultType.ERROR);
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
                OnInitLoginAndGetUserDataAction(FacebookHelperResultType.OK);
            }
        }

        private void FriendsUsingAppCallBack(IGraphResult result)
        {
            FBLog("FriendsDataCallBack", result.RawResult);
            FacebookHelperResultType resultType = FacebookHelperResultType.ERROR;

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
                resultType = FacebookHelperResultType.OK;
            }

            OnFriendsUsingAppAction(resultType);
        }


        void MyPictureCallback(Texture2D texture)
        {
            ProfilePicture = (Texture2D)texture;
            if (!ProfileImages.ContainsKey(AccessToken.CurrentAccessToken.UserId))
                ProfileImages.Add(AccessToken.CurrentAccessToken.UserId, ProfilePicture);

            IsUserPictureLoaded = true;

            OnUserProfilePictureRequestCompleteAction();
            OnProfilePictureRequestCompleteAction(AccessToken.CurrentAccessToken.UserId, ProfilePicture);
        }

        public bool HasPublicProfilePermission()
        {
            return PermissionsGranted.Contains("public_profile");
        }

        public bool HasEmailPermission()
        {
            return PermissionsGranted.Contains("email");
        }

        public bool HasUserFriendsPermission()
        {
            return PermissionsGranted.Contains("user_friends");
        }

        public bool HasPublishActionsPermission()
        {
            return PermissionsGranted.Contains("publish_actions");
        }

        //--------------------------------------
        //  Posts
        //------------------------------------



        public void PostAndLoginIfNeeded()
        {
            FacebookPostHandler postHandler = new FacebookPostHandler();
            FacebookManager.Instance.OnInitLoginAndGetUserDataAction += postHandler.OnInitLoginAndGetUserDataAction;
            if (!FacebookManager.Instance.IsUserDataLoaded)
            {
                FacebookManager.Instance.InitLoginAndGetUserData();
            }
            else
            {
                postHandler.OnInitLoginAndGetUserDataAction(FacebookManager.FacebookHelperResultType.OK);
            }
        }


        // seperate class so we can safely remove the reference if there are concurrent calls.
        class FacebookPostHandler
        {
            public void OnInitLoginAndGetUserDataAction(FacebookManager.FacebookHelperResultType result)
            {
                FacebookManager.Instance.OnInitLoginAndGetUserDataAction -= OnInitLoginAndGetUserDataAction;
                if (result == FacebookManager.FacebookHelperResultType.OK)
                {
                    FacebookManager.Instance.ShareLink(
                        contentURL: new Uri(FacebookManager.Instance.PostLink),
                        contentTitle: LocaliseText.Format("Facebook.Share.Caption", GameManager.Instance.GameName),
                        contentDescription: LocaliseText.Format("Facebook.Share.Description", GameManager.Instance.GameName),
                        photoURL: new Uri(FacebookManager.Instance.PostPicture)
                        );
                }
                else
                {
                    DialogManager.Instance.ShowError(textKey: "Facebook.Error.Login.Description");
                }
            }
        }


        public void ShareLink(Uri contentURL, string contentTitle = "",
                        string contentDescription = "", Uri photoURL = null)
        {
            if (!IsLoggedIn)
            {
                MyDebug.LogWarning("Authorise user before posting, fail event generated");

                IShareResult res = new ShareResult("User isn't authorised");
                OnPostingCompleteAction(FacebookHelperResultType.ERROR, res);
                return;
            }

            FB.ShareLink(
                contentURL: contentURL,
                contentTitle: contentTitle,
                contentDescription: contentDescription,
                photoURL: photoURL,
                callback: PostCallBack
                );
        }


        private void PostCallBack(IShareResult result)
        {
            FBLog("PostCallBack", result.RawResult);
            FacebookHelperResultType resultType = FacebookHelperResultType.ERROR;

            if (result != null)
            {
                if (result.Error == null)
                {
                    var responseObject = Json.Deserialize(result.RawResult) as Dictionary<string, object>;
                    object obj = 0;
                    if (responseObject.TryGetValue("cancelled", out obj))
                    {
                        resultType = FacebookHelperResultType.CANCELLED;
                    }
                    else if (responseObject.TryGetValue("id", out obj))
                    {
                        resultType = FacebookHelperResultType.OK;
                    }
                }
            }
            OnPostingCompleteAction(resultType, result);
        }

        //--------------------------------------
        //  App Requests / Invites
        //------------------------------------


        public void Share()
        {
            FacebookShareHandler postHandler = new FacebookShareHandler();
            FacebookManager.Instance.OnInitLoginAndGetUserDataAction += postHandler.OnInitLoginAndGetUserDataAction;
            if (!FacebookManager.Instance.IsUserDataLoaded)
            {
                FacebookManager.Instance.InitLoginAndGetUserData();
            }
            else
            {
                postHandler.OnInitLoginAndGetUserDataAction(FacebookManager.FacebookHelperResultType.OK);
            }
        }

        // seperate class so we can safely remove the reference if there are concurrent calls.
        class FacebookShareHandler
        {
            public void OnInitLoginAndGetUserDataAction(FacebookManager.FacebookHelperResultType result)
            {
                FacebookManager.Instance.OnInitLoginAndGetUserDataAction -= OnInitLoginAndGetUserDataAction;
                if (result == FacebookManager.FacebookHelperResultType.OK)
                {
                    FacebookManager.Instance.AppRequest(message: LocaliseText.Format("Facebook.Invite.Caption", GameManager.Instance.GameName), title: LocaliseText.Format("Facebook.Invite.Description", GameManager.Instance.GameName));
                }
                else
                {
                    DialogManager.Instance.ShowError(textKey: "Facebook.Error.Login.Description");
                }
            }
        }


        public void AppRequest(
            string message,
            string[] to = null,
            List<object> filters = null,
            string[] excludeIds = null,
            int? maxRecipients = null,
            string data = "",
            string title = "")
        {
            if (!IsLoggedIn)
            {
                Debug.LogWarning("Auth user before AppRequest, fail event generated");
                IAppRequestResult res = new AppRequestResult("User isn't authed");
                OnAppRequestCompleteAction(FacebookHelperResultType.ERROR, null, res);
                return;
            }

            FB.AppRequest(message, to, filters, excludeIds, maxRecipients, data, title, AppRequestCallBack);
        }

        private void AppRequestCallBack(IAppRequestResult result)
        {
            FBLog("AppRequestCallBack", result.RawResult);
            FacebookHelperResultType resultType = FacebookHelperResultType.ERROR;
            List<string> InvitedFriends = new List<string>();

            if (result != null && result.Error == null)
            {
                JSONObject jsonObject = JSONObject.Parse(result.RawResult);
                if (jsonObject.ContainsKey("cancelled"))
                {
                    resultType = FacebookHelperResultType.CANCELLED;
                }
                else if (jsonObject.ContainsKey("request"))
                {
                    //{   "request": "420211088059698",
                    //    "to": [
                    //        "100002669403922",
                    //        "100000048490273"
                    //    ]}
                    resultType = FacebookHelperResultType.OK;
                    HasInvitedFriends = true;

                    JSONValue to = jsonObject.GetArray("to");
                    if (to != null || to.Type == JSONValueType.Array)
                    {
                        NumberOfInvitesSent += to.Array.Length;
                        foreach (JSONValue value in to.Array)
                        {
                            InvitedFriends.Add(value.Str);
                            Debug.Log("Value: " + value.Str);
                        }
                    }
                }
            }
            OnAppRequestCompleteAction(resultType, InvitedFriends, result);
        }

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


        /// <summary>
        /// Gets the picture UR.
        /// </summary>
        /// 
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
                        OnProfilePictureRequestCompleteAction(userId, pictureTexture);
                    }
                });
            }
        }

        public static string GetPictureURL(string facebookID, int? width = null, int? height = null, string type = null)
        {
            string url = string.Format("/{0}/picture", facebookID);
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
    }
}
#endif
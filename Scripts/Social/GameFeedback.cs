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

using GameFramework.Preferences;
using GameFramework.GameStructure;
using GameFramework.Localisation;
using GameFramework.UI.Dialogs.Components;
using UnityEngine;

namespace GameFramework.Social
{
    /// <summary>
    /// Functionality for getting game feedback from the user.
    /// </summary>
    /// You can configure any displayed text through the localisation file.
    public class GameFeedback
    {
        /// <summary>
        /// Indicates whether the user has rated the game.
        /// </summary>
        public bool HasRated {
            get { return _hasRated; }
            set
            {
                _hasRated = value;
                PreferencesFactory.SetInt("HasRated", _hasRated ? 1 : 0);
                PreferencesFactory.Save();
            }
        }
        bool _hasRated;

        /// <summary>
        /// Indicates whether we have asked the user to rate the game.
        /// </summary>
        public bool HasAskedToRate
        {
            get { return _hasAskedToRate; }
            set
            {
                _hasAskedToRate = value;
                PreferencesFactory.SetInt("HasAskedToRate", _hasAskedToRate ? 1 : 0);
                PreferencesFactory.Save();
            }
        }
        bool _hasAskedToRate;


        public GameFeedback()
        {
            HasRated = PreferencesFactory.GetInt("HasRated") == 1;
            HasAskedToRate = PreferencesFactory.GetInt("HasAskedToRate") == 1;
        }


        /// <summary>
        /// Reset all rated flags.
        /// </summary>
        public void Reset()
        {
            HasRated = false;
            HasAskedToRate = false;
        }


        /// <summary>
        /// Should be called as a direct result of the user clicking on a rate button when you are uncertain if they 
        /// like the game.  e.g. a static rate button
        /// </summary>
        /// Gives a yes/no dialog to check they like. Yes takes them to the rate page, no takes them to a feedback dialog.
        public void GameFeedbackUnsureIfTheyLike()
        {
            DialogManager.Instance.Show("GameFeedbackDialog",
                titleKey: "GameFeedback.FeedbackTitle",
                text2: GlobalLocalisation.FormatText("GameFeedback.AssumeLike", GameManager.Instance.GameName),
                doneCallback: LikeCallback,
                dialogButtons: DialogInstance.DialogButtonsType.YesNo);

            HasAskedToRate = true;

            //#if UNITY_EDITOR
            //            Application.OpenURL(GameManager.Instance.PlayWebUrl);
            //#elif UNITY_ANDROID
            //		AndroidDialog dialog = AndroidDialog.Create(LocaliseText.Get("GameFeedback.FeedbackTitle"), String.Format(LocaliseText.Get("GameFeedback.Unsure"), GameManager.Instance.GameName));
            //		dialog.ActionComplete += OnAndroidUnsureIfTheyLikeDialogClose;
            //#elif UNITY_IPHONE
            //		IOSDialog dialog = IOSDialog.Create(LocaliseText.Get("GameFeedback.FeedbackTitle"), String.Format(LocaliseText.Get("GameFeedback.Unsure"), GameManager.Instance.GameName));
            //		dialog.OnComplete += OnIOSUnsureIfTheyLikeDialogClose;
            //#else
            //#endif
        }


        /// <summary>
        /// Should be called when invoked as a direct result of their actions and  if you assume the user likes 
        /// the game. e.g. a rate button shown only after a certain number of plays. 
        /// </summary>
        /// This displays a popup with just a message and ok button before taking them to the rate page.
        public void GameFeedbackAssumeTheyLike()
        {
            DialogManager.Instance.Show(titleKey: "GameFeedback.RateTitle",
                text2: GlobalLocalisation.FormatText("GameFeedback.AssumeLike", GameManager.Instance.GameName),
                doneCallback: RateCallback);

            HasAskedToRate = true;

            //#if UNITY_ANDROID
            //		AndroidMessage msg = AndroidMessage.Create(DialogTitle, String.Format(LocaliseText.Get("GameFeedback.AssumeLikeAndroid"), GameManager.Instance.GameName));
            //        msg.ActionComplete += OnAndroidRateMessageClose;
            //#elif UNITY_IPHONE
            //		IOSMessage msg = IOSMessage.Create(DialogTitle, String.Format(LocaliseText.Get("GameFeedback.AssumeLikeiOS"), GameManager.Instance.GameName));
            //        msg.OnComplete += OnIOSRateMessageClose;
            //#else
            //#endif
        }


        /// <summary>
        /// Should be called if you assume the user likes the game and when not invoked as a direct result of their 
        /// actions. e.g. shown automatically after a set number of plays. 
        /// </summary>
        /// Gives rate, later and remind options.
        public void GameFeedbackAssumeTheyLikeOptional()
        {
            // open the text based upon the platform
            string text2Key;
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    text2Key = "GameFeedback.AssumeLikeOptionalAndroid";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    text2Key = "GameFeedback.AssumeLikeOptionaliOS";
                    break;
                default:
                    text2Key = "GameFeedback.AssumeLikeOptionalAndroid";
                    break;
            }

            DialogManager.Instance.Show("GameFeedbackDialog",
                titleKey: "GameFeedback.RateTitle",
                text2: GlobalLocalisation.FormatText(text2Key, GameManager.Instance.GameName),
                doneCallback: RateCallback,
                dialogButtons: DialogInstance.DialogButtonsType.Custom);

            HasAskedToRate = true;

            //#if UNITY_EDITOR
            //            Application.OpenURL(GameManager.Instance.PlayWebUrl);
            //#elif UNITY_ANDROID
            //		AndroidRateUsPopUp rate = AndroidRateUsPopUp.Create(DialogTitle, String.Format(LocaliseText.Get("GameFeedback.AssumeLikeOptionalAndroid"), GameManager.Instance.GameName), GameManager.Instance.PlayMarketUrl);
            //        rate.ActionComplete += OnAndroidRatePopUpClose;
            //#elif UNITY_IPHONE
            //		IOSRateUsPopUp rate = IOSRateUsPopUp.Create(DialogTitle, String.Format(LocaliseText.Get("GameFeedback.AssumeLikeOptionaliOS"), GameManager.Instance.GameName));
            //        rate.OnComplete += OnIOSRatePopUpClose;
            //#else
            //#endif
        }


        /// <summary>
        /// Callback from feedback window where we are unsure if they like.
        /// </summary>
        /// <param name="instance"></param>
        void LikeCallback(DialogInstance instance)
        {
            // and based upon the dialog action
            switch (instance.DialogResult)
            {
                case DialogInstance.DialogResultType.Yes:
                    OpenRatingPage();
                    break;
                case DialogInstance.DialogResultType.No:
                    DialogManager.Instance.Show(titleKey: "GameFeedback.FeedbackTitle", text2Key: "GameFeedback.Unsure.No");
                    break;
            }
        }


        /// <summary>
        /// Callback from assusme they like feedback windows.
        /// </summary>
        /// <param name="instance"></param>
        void RateCallback(DialogInstance instance)
        {
            // and based upon the dialog action
            switch (instance.DialogResult)
            {
                case DialogInstance.DialogResultType.Ok:
                case DialogInstance.DialogResultType.Yes:
                    OpenRatingPage();
                    break;
                case DialogInstance.DialogResultType.Custom:
                    GameManager.Instance.TimesPlayedForRatingPrompt = 0;
                    break;
                case DialogInstance.DialogResultType.No:
                    break;
            }
        }


        /// <summary>
        /// Open the rating page for the current platform
        /// </summary>
        public void OpenRatingPage()
        {
            // open the rate url based upon the platform
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    Application.OpenURL(GameManager.Instance.PlayMarketUrl);
                    break;
                case RuntimePlatform.IPhonePlayer:
                    Application.OpenURL(GameManager.Instance.iOSWebUrl);
                    break;
                default:
                    Application.OpenURL(GameManager.Instance.PlayMarketUrl);
                    return;
            }

            HasRated = true;
        }

        #region Android Rating - not used
        /*
        //private void OnAndroidUnsureIfTheyLikeDialogClose(AndroidDialogResult result)
        //{
        //    //parsing result
        //    switch (result)
        //    {
        //        case AndroidDialogResult.YES:
        //            OnAndroidRateMessageClose(new AndroidDialogResult());
        //            break;
        //        case AndroidDialogResult.NO:
        //            AndroidMessage.Create(LocaliseText.Get("GameFeedback.FeedbackTitle"), LocaliseText.Get("GameFeedback.UnsureDoesNotLike"));
        //            break;
        //    }
        //}

        //private void OnAndroidRateMessageClose(AndroidDialogResult result)
        //{
        //    AndroidNativeUtility.OpenAppRatingPage(GameManager.Instance.PlayMarketUrl);
        //    AndroidToast.ShowToastNotification(LocaliseText.Get("GameFeedback.RateThanks"), 6);
        //    SetHasRated(true);
        //}

        //private void OnAndroidRatePopUpClose(AndroidDialogResult result)
        //{
        //    if (result == AndroidDialogResult.RATED)
        //    {
        //        AndroidToast.ShowToastNotification(LocaliseText.Get("GameFeedback.RateThanks"), 6);
        //        SetHasRated(true);
        //    }
        //    else if (result == AndroidDialogResult.REMIND)
        //    {
        //        GameManager.Instance.TimesPlayedForRatingPrompt = 0;
        //    }
        //}
        */
        #endregion Android Rating
        #region iOS Native Rating - not used
        /*

                private void OnIOSUnsureIfTheyLikeDialogClose(IOSDialogResult result)
                {
                    switch (result)
                    {
                        case IOSDialogResult.YES:
                            OnIOSRateMessageClose();
                            break;
                        case IOSDialogResult.NO:
                            IOSMessage.Create(LocaliseText.Get("GameFeedback.FeedbackTitle"), LocaliseText.Get("GameFeedback.UnsureDoesNotLike"));
                            break;
                    }
                }

                private void OnIOSRateMessageClose()
                {
                    IOSNativeUtility.RedirectToAppStoreRatingPage();
                    SetHasRated(true);
                }

                private void OnIOSRatePopUpClose(IOSDialogResult result)
                {
                    if (result == IOSDialogResult.RATED)
                    {
                        SetHasRated(true);
                    }
                    else if (result == IOSDialogResult.REMIND)
                    {
                        GameManager.Instance.TimesPlayedForRatingPrompt = 0;
                    }
                }
            */

        #endregion iOS Rating
    }
}
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

using UnityEngine;
#if GOOGLE_ADS
using GameFramework.Display.Other;
using GoogleMobileAds.Api;
#endif

/// <summary>
/// Support and extensions for advertising including AdMob and UnityAds
/// </summary>
/// For further information please see: http://www.flipwebapps.com/unity-assets/game-framework/advertising/
namespace GameFramework.Advertising
{
    // For doxygen documentation purposes only 
}

/// <summary>
/// Support classes and components to help with admob usage.
/// 
/// For additional information see http://www.flipwebapps.com/unity-assets/game-framework/advertising/
/// </summary>
namespace GameFramework.Advertising.AdMob
{
    /// <summary>
    /// Helper class for using AdMob that provides properties and methods for managing the adverts.
    /// </summary>
    /// Currently this is limited to showing a smart banner at the bottom of the screen or a full screen 'interstitial' advert.
    /// 
    /// NOTE: If you want to use Admob then be sure to enable through the integrations window or define 
    /// GOOGLE_ADS in the player settings.
    [System.Serializable]
    public class AdMob
    {
        /// <summary>
        /// Whether ads should be tagged for child directed treetment
        /// </summary>
        /// See the AdMob documentation for further details
        public bool TagForChildDirectedTreatment = true;

        /// <summary>
        /// A list of keywords to use for targeting adverts
        /// </summary>
        /// See the AdMob documentation for further details
        public bool IsDesignedForFamilies = false;

        /// <summary>
        /// A list of keywords to use for targeting adverts
        /// </summary>
        /// See the AdMob documentation for further details
        public string[] KeyWords = { };

        /// <summary>
        /// A list of test devices
        /// </summary>
        /// See the AdMob documentation for further details
        public string[] TestDevices = { };

        /// <summary>
        /// A birthday to use for targeting adverts
        /// </summary>
        /// See the AdMob documentation for further details
        public System.DateTime Birthday = new System.DateTime(1985, 1, 1);

        /// <summary>
        /// A background color to use when showing adverts
        /// </summary>
        /// See the AdMob documentation for further details
        public Color BackgroundColor = Color.white;

#if GOOGLE_ADS
        readonly BannerView _bannerView;
        readonly InterstitialAd _interstitialAd;

        public AdMob(string admobUnitIdAndroid, string admobUnitIdIos)
        {
#if UNITY_EDITOR
            string adUnitId = "unused";
#elif UNITY_ANDROID
            string adUnitId = admobUnitIdAndroid;
#elif UNITY_IPHONE
            string adUnitId = admobUnitIdIos;
#else
            string adUnitId = "unexpected_platform";
#endif

            // Create a banner at the bottom of the screen.
            _bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
            _interstitialAd = new InterstitialAd(adUnitId);

            // Register for ad events.
            //_bannerView.AdLoaded += HandleAdLoaded;
            //_bannerView.AdFailedToLoad += HandleAdFailedToLoad;
            //_bannerView.AdOpened += HandleAdOpened;
            //_bannerView.AdClosing += HandleAdClosing;
            //_bannerView.AdClosed += HandleAdClosed;
            //_bannerView.AdLeftApplication += HandleAdLeftApplication;
        }

        /// <summary>
        /// Request a banner ad targeted according to the class attributes 
        /// </summary>
        public void RequestBanner()
        {
            var adRequest = SetupNewAdRequest();
            _bannerView.LoadAd(adRequest);
        }

        /// <summary>
        /// Request an Interstitial ad targeted according to the class attributes 
        /// </summary>
        public void RequestInterstitial()
        {
            var adRequest = SetupNewAdRequest();
            _interstitialAd.LoadAd(adRequest);
        }

        AdRequest SetupNewAdRequest() {
            var request = new AdRequest.Builder();
            foreach (var testDevice in TestDevices)
                request.AddTestDevice(testDevice);
            foreach (var keyword in KeyWords)
                request.AddKeyword(keyword);
            request.TagForChildDirectedTreatment(TagForChildDirectedTreatment);
            request.AddExtra("color_bg", ColorHelper.HexString((Color)BackgroundColor));
            request.SetBirthday(Birthday);
            return request.Build();
        }

        /// <summary>
        /// Show a banner ad
        /// </summary>
        public void ShowBanner()
        {
            _bannerView.Show();
        }

        /// <summary>
        /// Hide a banner ad
        /// </summary>
        public void HideBanner()
        {
            _bannerView.Hide();
        }

        /// <summary>
        /// Show an interstitial ad
        /// </summary>
        public void ShowInterstitialBanner()
        {
            if (_interstitialAd.IsLoaded())
            {
                _interstitialAd.Show();
            }
        }

        ///// <summary>
        ///// Show an interstitial ad
        ///// </summary>
        //public void HideInterstitialBanner()
        //{
        //    _interstitialAd.Hide();
        //}

        #region Banner callback handlers
        /*
        public void HandleAdLoaded(object sender, EventArgs args)
        {
            Debug.Log("HandleAdLoaded event received.");
        }

        public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            Debug.Log("HandleFailedToReceiveAd event received with message: " + args.Message);
        }

        public void HandleAdOpened(object sender, EventArgs args)
        {
            Debug.Log("HandleAdOpened event received");
        }

        void HandleAdClosing(object sender, EventArgs args)
        {
            Debug.Log("HandleAdClosing event received");
        }

        public void HandleAdClosed(object sender, EventArgs args)
        {
            Debug.Log("HandleAdClosed event received");
        }

        public void HandleAdLeftApplication(object sender, EventArgs args)
        {
            Debug.Log("HandleAdLeftApplication event received");
        }
        */
        #endregion
#endif
    }
}
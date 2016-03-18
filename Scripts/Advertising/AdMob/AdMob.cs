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
#if GOOGLE_ADS
using GoogleMobileAds.Api;
#endif
namespace FlipWebApps.GameFramework.Scripts.Advertising.AdMob
{
    /// <summary>
    /// Helper class for using AdMob.
    /// If you want to use Admob then be sure to define GOOGLE_ADS in the player settings.
    /// 
    /// NOTE: This class is beta and subject to changebreaking change without warning.
    /// </summary>
    public class AdMob
    {
        public bool TagForChildDirectedTreatment = true;
        public bool IsDesignedForFamilies = true;
#if GOOGLE_ADS
        readonly BannerView _bannerView;
        readonly InterstitialAd _interstitialAd;

        public AdMob()
        {
#if UNITY_EDITOR
            string adUnitId = "unused";
#elif UNITY_ANDROID
            string adUnitId = GameManager.Instance.AdmobUnitIdAndroid;
#elif UNITY_IPHONE
            string adUnitId = GameManager.Instance.AdmobUnitIdIos;
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

        public void RequestBanner()
        {
            //TODO add suppport for Designed for Families setting https://developers.google.com/admob/android/targeting#designed_for_families_setting
            //TODO move test devices to class parameters
            // Request a banner ad, with optional custom ad targeting.
            AdRequest request = new AdRequest.Builder()
                //    .AddTestDevice(AdRequest.TestDeviceSimulator)
                .AddTestDevice("A431B6F2AB563BE62EAC8CBB5ECCD43F")      // Mark S2
                .AddTestDevice("6e2d8b66674d9ef3225b90cb584e5975")
                .AddTestDevice("C359AF1E66C7AFFCC94663B0EEF0D544")      // Galaxy Alpha
                .AddKeyword("game")
                .SetGender(Gender.Male)         //TODO - give the user some way of setting these (need to mention in privacy policy if so).
                .SetBirthday(new System.DateTime(1985, 1, 1))
                .TagForChildDirectedTreatment(TagForChildDirectedTreatment)
                .AddExtra("color_bg", "9B30FF")
                .Build();
            _bannerView.LoadAd(request);
        }

        public void RequestInterstitial()
        {
            // Request a banner ad, with optional custom ad targeting.
            AdRequest request = new AdRequest.Builder()
                //    .AddTestDevice(AdRequest.TestDeviceSimulator)
                .AddTestDevice("A431B6F2AB563BE62EAC8CBB5ECCD43F")      // Mark S2
                .AddTestDevice("6e2d8b66674d9ef3225b90cb584e5975")
                .AddTestDevice("C359AF1E66C7AFFCC94663B0EEF0D544")      // Galaxy Alpha
                .AddKeyword("game")
                .SetGender(Gender.Male)
                .SetBirthday(new System.DateTime(1985, 1, 1))
                .TagForChildDirectedTreatment(TagForChildDirectedTreatment)
                .AddExtra("color_bg", "9B30FF")
                .Build();
            _interstitialAd.LoadAd(request);
        }

        public void ShowBanner()
        {
            _bannerView.Show();
        }

        public void HideBanner()
        {
            _bannerView.Hide();
        }

        public void ShowInterstitialBanner()
        {
            if (_interstitialAd.IsLoaded())
            {
                _interstitialAd.Show();
            }
        }

        //public void HideInterstitialBanner()
        //{
        //    _interstitialAd.Hide();
        //}

        #region Banner callback handlers

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

        #endregion
#endif
    }
}
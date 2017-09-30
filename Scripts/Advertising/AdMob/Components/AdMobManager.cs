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

using GameFramework.GameObjects.Components;
using GameFramework.GameStructure;
using UnityEngine;

namespace GameFramework.Advertising.AdMob.Components
{
    /// <summary>
    /// Manager class for setting up and accessing AdMob functionality.
    /// </summary>
    /// 
    /// Add this component to your scene to automatically setup for AdMob functionality. You can then use members on this
    /// component, or some of the other available components to show and hide adverts.
    /// 
    /// In addition to the methods provided by this class, you can also access the main Admob helper class 
    /// which provides properties and methods for managing the adverts through AdMobManager.Instance.Adverts
    /// 
    /// This class is a persistant singleton and is recommended placed onto a gameobject called _GameScope along
    /// with other such persistant classes.
    /// 
    /// NOTE: If you want to use Admob then be sure to enable through the integrations window or define 
    /// GOOGLE_ADS in the player settings.
    /// 
    /// For additional information see http://www.flipwebapps.com/unity-assets/game-framework/advertising/
    [AddComponentMenu("Game Framework/Advertising/AdMob/AdMobManager")]
    [HelpURL("http://www.flipwebapps.com/game-framework/advertising/")]
    public class AdMobManager : SingletonPersistant<AdMobManager>
    {
        [Header("Advertising")]
        /// <summary>
        /// UnitID for use when this game is running on Android
        /// </summary>
        [Tooltip("UnitID for use when this game is running on Android")]
        public string AdmobUnitIdAndroid = "";

        /// <summary>
        /// UnitID for use when this game is running on iOS
        /// </summary>
        [Tooltip("UnitID for use when this game is running on iOS")]
        public string AdmobUnitIdIos = "";

        /// <summary>
        /// Admob helper class - you may access this directly to call setup and support functions 
        /// </summary>
        public AdMob Adverts;

        /// <summary>
        /// Called on Awake after the singleton has been setup.
        /// </summary>
        protected override void GameSetup()
        {
#if GOOGLE_ADS
            if (! GameManager.Instance.IsUnlocked && !string.IsNullOrEmpty(AdmobUnitIdAndroid) && !string.IsNullOrEmpty(AdmobUnitIdIos))
            {
                Adverts = new AdMob(AdmobUnitIdAndroid, AdmobUnitIdIos);
                Adverts.HideBanner();
                Adverts.RequestBanner();
            }
#endif
        }

        /// <summary>
        /// Show a banner ad
        /// </summary>
        public static void ShowBanner()
        {
#if GOOGLE_ADS
            if (IsActive)
                Instance.Adverts.ShowBanner();
#endif
        }

        /// <summary>
        /// Hide a banner ad
        /// </summary>
        public static void HideBanner()
        {
#if GOOGLE_ADS
            if (IsActive)
                Instance.Adverts.HideBanner();
#endif
        }

        /// <summary>
        /// Show an interstitial ad
        /// </summary>
        public static void ShowInterstitialBanner()
        {
#if GOOGLE_ADS
            if (IsActive)
                Instance.Adverts.ShowInterstitialBanner();
#endif
        }
    }
}
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

using GameFramework.UI.Dialogs.Components;

#if UNITY_ADS
using GameFramework.GameStructure;
using GameFramework.Preferences;
using GameFramework.Localisation;
using GameFramework.UI.Other.Components;
using UnityEngine;
using UnityEngine.Advertisements;
#endif

/// <summary>
/// Support classes and components to help with Unity Ads usage.
/// 
/// For additional information see http://www.flipwebapps.com/unity-assets/game-framework/advertising/
/// </summary>
namespace GameFramework.Advertising.UnityAds
{
    /// <summary>
    /// Helper functions for Unity Adverts
    /// </summary>
    /// 
    /// For additional information see http://www.flipwebapps.com/unity-assets/game-framework/advertising/
    public class UnityAds
    {
        /// <summary>
        /// Show an advert and award the player coins if they complete watching it.
        /// </summary>
        /// <param name="coins"></param>
        public static void ShowWatchAdvertForCoins(int coins)
        {
#if UNITY_ADS
            //TODO only show advert button if actually ready to avoid errors.
            if (Advertisement.IsReady())
            {
                Advertisement.Show(null, new ShowOptions
                {
                    //pause = true,
                    resultCallback = result =>
                    {
                        switch (result)
                        {
                            case (ShowResult.Finished):
                                GameManager.Instance.Player.Coins += coins;
                                GameManager.Instance.Player.UpdatePlayerPrefs();
                                PreferencesFactory.Save();
                                DialogManager.Instance.Show(title: "Coins", text: LocaliseText.Format("Advertising.UnityAds.WatchForCoins.Finished", coins));
                                break;
                            case (ShowResult.Skipped):
                                DialogManager.Instance.Show(title: "Coins", text: LocaliseText.Get("Advertising.UnityAds.WatchForCoins.Skipped"));
                                break;
                            case (ShowResult.Failed):
                                DialogManager.Instance.Show(title: "Coins", text: LocaliseText.Get("Advertising.UnityAds.WatchForCoins.UnableToShow"));
                                break;

                        }
                        Debug.Log(result.ToString());
                    }
                });
            }
            else
            {
                DialogManager.Instance.Show(title: "Error", text: LocaliseText.Get("Advertising.UnityAds.UnableToShow"));
            }
#else
            DialogManager.Instance.ShowInfo("This functionality requires that you enable the standard Unity Ads service.\n\nPlease check our website if you need further help.");
#endif
        }

        /// <summary>
        /// Show an advert.
        /// </summary>
        public static void ShowAdvert()
        {
#if UNITY_ADS
            //TODO only show advert button if actually ready to avoid errors.
            if (Advertisement.IsReady())
            {
                Advertisement.Show();
            }
#else
            DialogManager.Instance.ShowInfo("This functionality requires that you enable the standard Unity Ads service.\n\nPlease check our website if you need further help.");
#endif
        }
    }
}
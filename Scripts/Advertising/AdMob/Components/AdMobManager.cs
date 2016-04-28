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

using FlipWebApps.GameFramework.Scripts.GameObjects.Components;
using FlipWebApps.GameFramework.Scripts.GameStructure;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Advertising.AdMob.Components
{
    /// <summary>
    /// manager class for setting up and accessing AdMob functionality.
    /// 
    /// NOTE: This class is beta and subject to changebreaking change without warning.
    /// </summary>
    [AddComponentMenu("Game Framework/Advertising/AdMob/AdMobManager")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class AdMobManager : SingletonPersistant<GameManager>
    {
        [Header("Advertising")]
        public string AdmobUnitIdAndroid = "";
        public string AdmobUnitIdIos = "";

        /// <summary>
        /// Advertising related properties
        /// </summary>
        public AdMob Adverts;

        protected override void GameSetup()
        {
            Debug.Log("AdMobManager: GameSetup");

            // advertising
#if GOOGLE_ADS
            if (!GameManager.Instance.IsUnlocked && !string.IsNullOrEmpty(AdmobUnitIdAndroid) && !string.IsNullOrEmpty(AdmobUnitIdIos))
            {
                Adverts = new AdMob();
                Adverts.HideBanner();
                Adverts.RequestBanner();
            }
#endif
        }
    }
}
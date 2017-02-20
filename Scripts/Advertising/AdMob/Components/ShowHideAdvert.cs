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

namespace GameFramework.Advertising.AdMob.Components
{
    /// <summary>
    /// Automatically show or hide an advert. 
    /// </summary>
    /// 
    /// The advert will be shown when this components Start method is called.
    /// 
    /// For additional information see http://www.flipwebapps.com/unity-assets/game-framework/advertising/
    [AddComponentMenu("Game Framework/Advertising/AdMob/ShowAdvert")]
    [HelpURL("http://www.flipwebapps.com/game-framework/advertising/")]
    public class ShowHideAdvert : MonoBehaviour
    {
        /// <summary>
        /// Whether to try showing an advert when this component is Started (The scene starts)
        /// </summary>
        [Tooltip("Whether to try showing an advert when this component is Started (i.e. scene starts)")]
        public bool ShowOnStart = true;

        /// <summary>
        /// Whether to try hiding any advert when this component is Destroyed (The scene ends)
        /// </summary>
        [Tooltip("Whether to try hiding any advert when this component is Destroyed (i.e the scene ends)")]
        public bool HideOnDestroy = true;

        void Start()
        {
            if (ShowOnStart)
                AdMobManager.ShowBanner();
        }

        void OnDestroy()
        {
            if (HideOnDestroy)
                AdMobManager.HideBanner();
        }
    }
}
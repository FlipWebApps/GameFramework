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

namespace GameFramework.GameFramework
{
    /// <summary>
    /// General GameFramework helper functions
    /// </summary>
    public class GameFrameworkHelper
    {
        public static void ShowHomepage()
        {
            Application.OpenURL("http://www.flipwebapps.com/game-framework/");
        }

        public static void ShowOnlineTutorials()
        {
            Application.OpenURL("http://www.flipwebapps.com/game-framework/tutorials/");
        }

        public static void ShowDocumentation()
        {
            Application.OpenURL("http://www.flipwebapps.com/game-framework/");
        }

        public static void ShowAPIDocumentation()
        {
            Application.OpenURL("http://www.flipwebapps.com/documentation/api/game-framework/index.html");
        }

        public static void ShowSupportForum()
        {
            Application.OpenURL("http://www.flipwebapps.com/forum/");
        }

        public static void ShowContact()
        {
            Application.OpenURL("http://www.flipwebapps.com/contact/");
        }

        public static void ShowAssetStorePage()
        {
#if UNITY_EDITOR
            if (UnityEditor.AssetDatabase.IsValidFolder(@"Assets\FlipWebApps\GameFrameworkExtras") || UnityEditor.AssetDatabase.IsValidFolder(@"Assets\FlipWebApps\GameFrameworkTutorials"))
                ShowAssetStorePageExtrasBundle();
            else
                ShowAssetStorePageFree();
#else
                ShowAssetStorePageFree();
#endif
        }

        public static void ShowAssetStorePageFree()
        {
            Application.OpenURL("https://www.assetstore.unity3d.com/#!/content/55334?aid=1011lGnE");
        }

        public static void ShowAssetStorePageExtrasBundle()
        {
            Application.OpenURL("https://www.assetstore.unity3d.com/#!/content/50893?aid=1011lGnE");
        }

        public static void ShowAssetStorePagePrefsEditor()
        {
            Application.OpenURL("https://www.assetstore.unity3d.com/#!/content/61908?aid=1011lGnE");
        }
    }
}
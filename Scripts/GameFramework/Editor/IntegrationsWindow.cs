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

using GameFramework.EditorExtras.Editor;
using UnityEditor;
using UnityEngine;

namespace GameFramework.GameFramework.Editor {

    /// <summary>
    /// Window allowing to enable integrations with various 3rd party assets.
    /// </summary>
    public class IntegrationsWindow : EditorWindow
    {
        Texture2D _beautifulTransitionsIcon;
        Texture2D _prefsEditorIcon;
        Texture2D _proPoolingIcon;
        Texture2D _playMakerActionsForGameFrameworkIcon;
        Texture2D _adMobIcon;
        Texture2D _facebookIcon;
        Texture2D _playMakerIcon;
        Vector2 _messageLogScrollPosition;

        // Add menu item
        [MenuItem("Window/Game Framework/Integrations Window", priority=1)]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            //var window = 
            GetWindow<IntegrationsWindow>(true, "Integrations", true);

        }

        void OnEnable()
        {
            _beautifulTransitionsIcon = AssetDatabase.LoadAssetAtPath(@"Assets\FlipWebApps\GameFramework\Sprites\Integrations\BeautifulTransitions.png", typeof(Texture2D)) as Texture2D;
            _prefsEditorIcon = AssetDatabase.LoadAssetAtPath(@"Assets\FlipWebApps\GameFramework\Sprites\Integrations\PrefsEditor.png", typeof(Texture2D)) as Texture2D;
            _proPoolingIcon = AssetDatabase.LoadAssetAtPath(@"Assets\FlipWebApps\GameFramework\Sprites\Integrations\ProPooling.png", typeof(Texture2D)) as Texture2D;
            _playMakerActionsForGameFrameworkIcon = AssetDatabase.LoadAssetAtPath(@"Assets\FlipWebApps\GameFramework\Sprites\Integrations\PlayMakerActionsForGameFramework.png", typeof(Texture2D)) as Texture2D;
            _adMobIcon = AssetDatabase.LoadAssetAtPath(@"Assets\FlipWebApps\GameFramework\Sprites\Integrations\AdMob.png", typeof(Texture2D)) as Texture2D;
            _facebookIcon = AssetDatabase.LoadAssetAtPath(@"Assets\FlipWebApps\GameFramework\Sprites\Integrations\Facebook.png", typeof(Texture2D)) as Texture2D;
            _playMakerIcon = AssetDatabase.LoadAssetAtPath(@"Assets\FlipWebApps\GameFramework\Sprites\Integrations\PlayMaker.png", typeof(Texture2D)) as Texture2D;
        }

        void OnGUI()
        {
            _messageLogScrollPosition = GUILayout.BeginScrollView(_messageLogScrollPosition);

            EditorGUILayout.HelpBox("The below assets all add extra features to Game Framework. If you have an asset installed you can enable it to make use of additional features.", MessageType.None);
            EditorGUILayout.Space();

            GUILayout.Label("First Party", EditorStyles.boldLabel);
            ShowAsset("Beautiful Transitions", "BEAUTIFUL_TRANSITIONS", "https://www.assetstore.unity3d.com/#!/content/56442?aid=1011lGnE", "Simply and easily create beautiful transitions for your UI elements, gameobjects , cameras or scenes. Included as part of the Game Framework - Extras bundle along with several other assets.\n\nEnable for automatic use of scene and gameobject transitions.", "Asset Store", @"Assets\FlipWebApps\BeautifulTransitions", _beautifulTransitionsIcon, true);
            ShowAsset("Prefs Editor (Secured)", "PREFS_EDITOR", "https://www.assetstore.unity3d.com/#!/content/61908?aid=1011lGnE", "Full editor for viewing, editing and managing your PlayerPrefs. Included as part of the Game Framework - Extras bundle along with several other assets.\n\nEnable for optional use of secured player preferences to prevent cheating.", "Asset Store", @"Assets\FlipWebApps\PrefsEditor", _prefsEditorIcon, true);
            ShowAsset("Pro Pooling", "PRO_POOLING", "https://www.assetstore.unity3d.com/#!/content/59286?aid=1011lGnE", "Powerful gameobject and prefab pooling. Included as part of the Game Framework - Extras bundle along with several other assets.\n\nAdds Gameobject pooling and performance improvements", "Asset Store", @"Assets\FlipWebApps\ProPooling", _proPoolingIcon, true);
            ShowAsset("PlayMaker Actions for Game Framework", null, "https://www.assetstore.unity3d.com/#!/content/88418?aid=1011lGnE", "Combine the power of Game Framework and PlayMaker using this set of custom actions.\n\nPlaymaker integration requires the third party PlayMaker asset and the Game Framework PlayMaker Extensions that is available either as a seperate download or included in the Game Framework Extras Bundle.\n\nWith these assets installed, enable PlayMaker integration below to use these actions.", "Asset Store", @"Assets\FlipWebApps\GameFrameworkPlayMakerExtensions", _playMakerActionsForGameFrameworkIcon, true);

            GUILayout.Label("Third Party", EditorStyles.boldLabel);
            ShowAsset("Facebook", "FACEBOOK_SDK", "https://developers.facebook.com/docs/unity/", "Facebook support for WebGL, Android and iOS.\n\nEnables Facebook support components and functionality in GameFramework", "Download", @"Assets\FacebookSDK", _facebookIcon);
            ShowAsset("Google Mobile Ads (AdMob)", "GOOGLE_ADS", "https://github.com/googleads/googleads-mobile-unity", "Serve Google Mobile Ads on Android and iOS apps.\n\nAdds google mobile ads support to GameFramework", "Download", @"Assets\GoogleMobileAds", _adMobIcon);
            ShowAsset("PlayMaker", "PLAYMAKER", "https://www.assetstore.unity3d.com/#!/content/368?aid=1011lGnE", "Combine the power of Game Framework and PlayMaker.\n\nPlaymaker integration requires the third party PlayMaker asset and the Game Framework PlayMaker Extensions that is available either as a seperate download or included in the Game Framework Extras Bundle.", "Asset Store (Play Maker)", @"Assets\PlayMaker", _playMakerIcon, true);

            GUILayout.Label("Asset developers - contact us to get your asset listed here!");

            GUILayout.EndScrollView();
        }

        void ShowAsset(string assetName, string defineName, string url, string description, string urlName, string folder = null, Texture2D icon = null, bool isInBundle = false)
        {
            EditorGUILayout.BeginVertical("Box");

            GUILayout.BeginHorizontal();
            GUI.enabled = folder == null || AssetDatabase.IsValidFolder(folder);
            if (defineName != null)
            {
                if (GUILayout.Toggle(PlayerSettingsHelper.IsScriptingDefineSet(defineName), ""))
                {
                    PlayerSettingsHelper.AddScriptingDefineAllTargets(defineName);
                }
                else
                {
                    PlayerSettingsHelper.RemoveScriptingDefineAllTargets(defineName);
                }
            }
            GUI.enabled = true;
            GUILayout.Label(assetName, new GUIStyle(EditorStyles.boldLabel) { padding = new RectOffset(5, 5, 5, 5), margin = new RectOffset(0, 0, 0, 0) });
            GUILayout.FlexibleSpace();
            if (EditorHelper.LinkButton(urlName, true))
                Application.OpenURL(url);
            if (isInBundle)
            {
                GUILayout.Label(" | ");
                if (EditorHelper.LinkButton("Extras Bundle", true))
                    GameFrameworkHelper.ShowAssetStorePageExtrasBundle();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (icon != null)
                GUILayout.Label(icon, GUILayout.Width(64), GUILayout.Height(64));
            EditorGUILayout.HelpBox(description, MessageType.None);
            GUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }
    }
}
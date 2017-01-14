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

using GameFramework.Billing;
using GameFramework.EditorExtras;
using GameFramework.FreePrize.Components;
using GameFramework.GameStructure;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GameFramework.EditorExtras.Editor;
using UnityEditor;
using UnityEngine;

namespace GameFramework.Debugging.Components.Editor {

    /// <summary>
    /// Component for allowing various cheat functions to be called such as increasing score, resetting prefs etc..
    /// </summary>
    public class CheatFunctionsWindow : EditorWindow
    {
        readonly string[] _tabNames = {"General", "Player", "World", "Level", "Free Prize"};
        int _tabSelected;
        string _productId;

        // Add menu item
        [MenuItem("Window/Game Framework/Cheat Functions Window", priority=1)]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            var window = GetWindow(typeof(CheatFunctionsWindow));
            window.titleContent.text = "Cheat Functions";
        }

        void OnGUI()
        {
            DrawTabs();
            switch (_tabSelected)
            {
                case 0:
                    PreferencesMenuOptions();
                    IAPMenuOptions();
                    break;
                case 1:
                    PlayerMenuOptions();
                    break;
                case 2:
                    WorldMenuOptions();
                    break;
                case 3:
                    LevelMenuOptions();
                    break;
                case 4:
                    FreePrizeMenuOptions();
                    break;
            }
        }

        void DrawTabs()
        {
            _tabSelected = GUILayout.Toolbar(_tabSelected, _tabNames);
        }

        void PreferencesMenuOptions()
        {
            // preferences
            GUILayout.Label("Preferences", new GUIStyle() { fontStyle = FontStyle.Bold, padding = new RectOffset(5, 5, 5, 5) });
            EditorGUILayout.HelpBox("Manage preference (PlayerPrefs)", MessageType.None);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset", GUILayout.Width(100)))
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
                Debug.Log("Player prefs deleted. Note: Some gameobjects might hold values and write these out after this call!");
            }
            if (GUILayout.Button("List Preferences to Console", GUILayout.Width(200)))
            {
                WriteWindowsPreferences();
            }
            GUILayout.EndHorizontal();
        }

        void IAPMenuOptions()
        {
            // preferences
            GUILayout.Label("In App Purchase Simulation", new GUIStyle() { fontStyle = FontStyle.Bold, padding = new RectOffset(5, 5, 5, 5) });
            EditorGUILayout.HelpBox("Simulate a purchase by entering a product id, either your own or a built in one (unlockgame, unlock.world.xx, unlock.level.xx, unlock.characher.xx)", MessageType.None);
            GUILayout.BeginHorizontal();
            _productId = EditorGUILayout.TextField("Product Id: ", _productId, GUILayout.Width(300));
            if (GUILayout.Button("Simulate Purchase", GUILayout.Width(150)))
            {
                if (!string.IsNullOrEmpty(_productId))
                    Payment.ProcessPurchase(_productId);
            }

            GUILayout.EndHorizontal();
        }

        private void PlayerMenuOptions()
        {
            // player
            GUILayout.Label("Player", new GUIStyle() { fontStyle = FontStyle.Bold, padding = new RectOffset(5, 5, 5, 5) });
            // lives
            GUILayout.BeginHorizontal();
            var playerLives = GameManager.IsActive ? " (" + GameManager.Instance.Player.Lives + ")" : "";
            GUILayout.Label("Lives" + playerLives, GUILayout.Width(100));
            if (GUILayout.Button("-1", GUILayout.Width(50)))
            {
                if (Application.isPlaying && GameManager.IsActive)
                {
                    GameManager.Instance.Player.Lives -= 1;
                }
                else
                {
                    Debug.LogWarning("This only works in play mode. You also need to add a GameManager.");
                }
            }
            if (GUILayout.Button("+1", GUILayout.Width(50)))
            {
                if (Application.isPlaying && GameManager.IsActive)
                {
                    GameManager.Instance.Player.Lives += 1;
                }
                else
                {
                    Debug.LogWarning("This only works in play mode. You also need to add a GameManager.");
                }
            }
            GUILayout.EndHorizontal();
            // health
            GUILayout.BeginHorizontal();
            var playerHealth = GameManager.IsActive ? " (" + GameManager.Instance.Player.Health + ")" : "";
            GUILayout.Label("Health" + playerHealth, GUILayout.Width(100));
            if (GUILayout.Button("-0.1", GUILayout.Width(50)))
            {
                if (Application.isPlaying && GameManager.IsActive)
                {
                    // only assign once!
                    GameManager.Instance.Player.Health = GameManager.Instance.Player.Health - 0.1f < 0 ? 0 : GameManager.Instance.Player.Health - 0.1f;
                }
                else
                {
                    Debug.LogWarning("This only works in play mode. You also need to add a GameManager.");
                }
            }
            if (GUILayout.Button("+0.1", GUILayout.Width(50)))
            {
                if (Application.isPlaying && GameManager.IsActive)
                {
                    GameManager.Instance.Player.Health += 0.1f;
                    GameManager.Instance.Player.Health = Mathf.Min(GameManager.Instance.Player.Health, 1);
                }
                else
                {
                    Debug.LogWarning("This only works in play mode. You also need to add a GameManager.");
                }
            }
            GUILayout.EndHorizontal();
            // player score
            GUILayout.BeginHorizontal();
            string playerScore = GameManager.IsActive ? " (" + GameManager.Instance.Player.Score + ")" : "";
            GUILayout.Label("Score" + playerScore, GUILayout.Width(100));
            if (GUILayout.Button("-100", GUILayout.Width(50)))
            {
                UpdatePlayerScore(-100);
            }
            if (GUILayout.Button("-10", GUILayout.Width(50)))
            {
                UpdatePlayerScore(-10);
            }
            if (GUILayout.Button("0", GUILayout.Width(50)))
            {
                UpdatePlayerScore(int.MinValue);
            }
            if (GUILayout.Button("+10", GUILayout.Width(50)))
            {
                UpdatePlayerScore(10);
            }
            if (GUILayout.Button("+100", GUILayout.Width(50)))
            {
                UpdatePlayerScore(100);
            }
            GUILayout.EndHorizontal();

            // player coins
            GUILayout.BeginHorizontal();
            string playerCoins = GameManager.IsActive ? " (" + GameManager.Instance.Player.Coins + ")" : "";
            GUILayout.Label("Coins" + playerCoins, GUILayout.Width(100));
            if (GUILayout.Button("-100", GUILayout.Width(50)))
            {
                UpdatePlayerCoins(-100);
            }
            if (GUILayout.Button("-10", GUILayout.Width(50)))
            {
                UpdatePlayerCoins(-10);
            }
            if (GUILayout.Button("0", GUILayout.Width(50)))
            {
                UpdatePlayerCoins(int.MinValue);
            }
            if (GUILayout.Button("+10", GUILayout.Width(50)))
            {
                UpdatePlayerCoins(10);
            }
            if (GUILayout.Button("+100", GUILayout.Width(50)))
            {
                UpdatePlayerCoins(100);
            }
            GUILayout.EndHorizontal();
        }

        private void WorldMenuOptions()
        {
            GUILayout.Label("World", new GUIStyle() { fontStyle = FontStyle.Bold, padding = new RectOffset(5, 5, 5, 5) });
            // general
            GUILayout.BeginHorizontal();
            GUILayout.Label("Current Worlds", GUILayout.Width(100));
            if (GUILayout.Button("Unlock All", GUILayout.Width(100)))
            {
                if (Application.isPlaying && GameManager.IsActive && GameManager.Instance.Worlds != null)
                {
                    foreach (var world in GameManager.Instance.Worlds.Items)
                    {
                        world.IsUnlocked = true;
                        world.IsUnlockedAnimationShown = true;
                        world.UpdatePlayerPrefs();
                    }
                    PlayerPrefs.Save();
                }
                else
                {
                    Debug.LogWarning("This only works in play mode. You also need to add a GameManager and have worlds setup.");
                }
            }
            if (GUILayout.Button("Lock All", GUILayout.Width(100)))
            {
                if (Application.isPlaying && GameManager.IsActive && GameManager.Instance.Worlds != null)
                {
                    foreach (var world in GameManager.Instance.Worlds.Items)
                    {
                        world.IsUnlocked = false;
                        world.IsUnlockedAnimationShown = false;
                        world.UpdatePlayerPrefs();
                    }
                    PlayerPrefs.Save();
                }
                else
                {
                    Debug.LogWarning("This only works in play mode. You also need to add a GameManager and have worlds setup.");
                }
            }
            GUILayout.EndHorizontal();
        }

        private void LevelMenuOptions()
        {
            if (!Application.isPlaying)
            {
                GUILayout.Label("These functions are only available in play mode.", EditorHelper.ItalicLabelStyle(TextAnchor.MiddleCenter), GUILayout.ExpandWidth(true));
                return;
            }
            if (!GameManager.IsActive || GameManager.Instance.Levels == null)
            {
                GUILayout.Label("These functions are only available when a GameManager is active and a level selected.", EditorHelper.ItalicLabelStyle(TextAnchor.MiddleCenter), GUILayout.ExpandWidth(true));
                return;
            }

            GUILayout.Label("Level", new GUIStyle() { fontStyle = FontStyle.Bold, padding = new RectOffset(5, 5, 5, 5) });
            // general
            GUILayout.BeginHorizontal();
            GUILayout.Label("Current Levels", GUILayout.Width(100));
            if (GUILayout.Button("Unlock All", GUILayout.Width(100)))
            {
                foreach (var level in GameManager.Instance.Levels.Items)
                {
                    level.IsUnlocked = true;
                    level.IsUnlockedAnimationShown = true;
                    level.UpdatePlayerPrefs();
                }
                PlayerPrefs.Save();
            }
            if (GUILayout.Button("Lock All", GUILayout.Width(100)))
            {
                if (Application.isPlaying && GameManager.IsActive && GameManager.Instance.Levels != null)
                {
                    foreach (var level in GameManager.Instance.Levels.Items)
                    {
                        level.IsUnlocked = false;
                        level.IsUnlockedAnimationShown = false;
                        level.UpdatePlayerPrefs();
                    }
                    PlayerPrefs.Save();
                }
                else
                {
                    Debug.LogWarning("This only works in play mode. You also need to add a GameManager and have levels setup.");
                }
            }
            GUILayout.EndHorizontal();

            // level score
            GUILayout.BeginHorizontal();
            string levelScore = (GameManager.IsActive && GameManager.Instance.Levels != null) ? " (" + GameManager.Instance.Levels.Selected.Score + ")" : "";
            GUILayout.Label("Score" + levelScore, GUILayout.Width(100));
            if (GUILayout.Button("-100", GUILayout.Width(50)))
            {
                UpdateLevelScore(-100);
            }
            if (GUILayout.Button("-10", GUILayout.Width(50)))
            {
                UpdateLevelScore(-10);
            }
            if (GUILayout.Button("0", GUILayout.Width(50)))
            {
                UpdateLevelScore(int.MinValue);
            }
            if (GUILayout.Button("+10", GUILayout.Width(50)))
            {
                UpdateLevelScore(10);
            }
            if (GUILayout.Button("+100", GUILayout.Width(50)))
            {
                UpdateLevelScore(100);
            }
            GUILayout.EndHorizontal();

            // level coins
            GUILayout.BeginHorizontal();
            string levelCoins = (GameManager.IsActive && GameManager.Instance.Levels != null) ? " (" + GameManager.Instance.Levels.Selected.Coins + ")" : "";
            GUILayout.Label("Coins" + levelCoins, GUILayout.Width(100));
            if (GUILayout.Button("-100", GUILayout.Width(50)))
            {
                UpdateLevelCoins(-100);
            }
            if (GUILayout.Button("-10", GUILayout.Width(50)))
            {
                UpdateLevelCoins(-10);
            }
            if (GUILayout.Button("0", GUILayout.Width(50)))
            {
                UpdateLevelCoins(int.MinValue);
            }
            if (GUILayout.Button("+10", GUILayout.Width(50)))
            {
                UpdateLevelCoins(10);
            }
            if (GUILayout.Button("+100", GUILayout.Width(50)))
            {
                UpdateLevelCoins(100);
            }
            GUILayout.EndHorizontal();

            // level stars
            GUILayout.BeginHorizontal();
            GUILayout.Label("Stars", GUILayout.Width(100));
            GameManager.Instance.Levels.Selected.StarWon(1,
                GUILayout.Toggle(GameManager.Instance.Levels.Selected.IsStarWon(1), "1", GUILayout.Width(50)));
            GameManager.Instance.Levels.Selected.StarWon(2,
                GUILayout.Toggle(GameManager.Instance.Levels.Selected.IsStarWon(2), "2", GUILayout.Width(50)));
            GameManager.Instance.Levels.Selected.StarWon(3,
                GUILayout.Toggle(GameManager.Instance.Levels.Selected.IsStarWon(3), "3", GUILayout.Width(50)));
            GameManager.Instance.Levels.Selected.StarWon(4,
                GUILayout.Toggle(GameManager.Instance.Levels.Selected.IsStarWon(4), "4", GUILayout.Width(50)));
            GUILayout.EndHorizontal();
        }

        void FreePrizeMenuOptions()
        {
            GUI.enabled = Application.isPlaying && FreePrizeManager.IsActive;

            GUILayout.BeginHorizontal();
            if (Application.isPlaying && FreePrizeManager.IsActive)
                GUILayout.Label("Free Prize (prize in " + FreePrizeManager.Instance.GetTimeToPrize() + ")",
                    EditorStyles.boldLabel);
            else if (Application.isPlaying && !FreePrizeManager.IsActive)
                GUILayout.Label("Free Prize (no FreePrizeMaanger detected)", EditorStyles.boldLabel);
            else
                GUILayout.Label("Free Prize", EditorStyles.boldLabel);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Make Prize Available", GUILayout.Width(150)))
            {
                FreePrizeManager.Instance.MakePrizeAvailable();
            }
            if (GUILayout.Button("Reset Counter", GUILayout.Width(150)))
            {
                FreePrizeManager.Instance.StartNewCountdown();
            }
            GUILayout.EndHorizontal();
        }

        private void UpdatePlayerScore(int amount)
        {
            if (Application.isPlaying)
            {
                if (GameManager.IsActive)
                {
                    if (amount > 0)
                        GameManager.Instance.Player.AddPoints(amount);
                    else
                        GameManager.Instance.Player.RemovePoints(-amount);
                }
                else
                {
                    Debug.LogWarning("You need to add a GameManager to your scene to use this function.");
                }
            }
            else
            {
                Debug.LogWarning("THis only works in play mode.");
            }
        }

        void UpdatePlayerCoins(int amount)
        {
            if (Application.isPlaying)
            {
                if (GameManager.IsActive)
                {
                    if (amount > 0)
                        GameManager.Instance.Player.AddCoins(amount);
                    else
                        GameManager.Instance.Player.RemoveCoins(-amount);
                }
                else
                {
                    Debug.LogWarning("You need to add a GameManager to your scene to use this function.");
                }
            }
            else
            {
                Debug.LogWarning("THis only works in play mode.");
            }
        }

        private void UpdateLevelScore(int amount)
        {
            if (Application.isPlaying)
            {
                if (GameManager.IsActive)
                {
                    if (GameManager.Instance.Levels != null)
                    {
                        if (amount > 0)
                            GameManager.Instance.Levels.Selected.AddPoints(amount);
                        else
                            GameManager.Instance.Levels.Selected.RemovePoints(-amount);
                    }
                    else
                    {
                        Debug.LogWarning("You need to have levels setup to use this function.");
                    }
                }
                else
                {
                    Debug.LogWarning("You need to add a GameManager to your scene to use this function.");
                }
            }
            else
            {
                Debug.LogWarning("THis only works in play mode.");
            }
        }

        private void UpdateLevelCoins(int amount)
        {
            if (Application.isPlaying)
            {
                if (GameManager.IsActive)
                {
                    if (GameManager.Instance.Levels != null)
                    {
                        if (amount > 0)
                            GameManager.Instance.Levels.Selected.AddCoins(amount);
                        else
                            GameManager.Instance.Levels.Selected.RemoveCoins(-amount);
                    }
                    else
                    {
                        Debug.LogWarning("You need to have levels setup to use this function.");
                    }
                }
                else
                {
                    Debug.LogWarning("You need to add a GameManager to your scene to use this function.");
                }
            }
            else
            {
                Debug.LogWarning("This only works in play mode.");
            }
        }


        /// <summary>
        /// Call the appropriate function for writing out the preferences.
        /// </summary>
        void WritePreferences()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                WriteWindowsPreferences();
            }
            else if (Application.platform == RuntimePlatform.OSXEditor)
            {
                WriteOSXPreferences();
            }
            else
            {
                Debug.Log("This currently only works on Windows and Mac.");
            }
        }


        /// <summary>
        /// On Windows, PlayerPrefs are stored in the registry under HKCU\Software\[company name]\[product name] key, where 
        /// company and product names are the names set up in Project Settings. (http://docs.unity3d.com/ScriptReference/PlayerPrefs.html)
        /// </summary>
        void WriteWindowsPreferences()
        {
            var prefsKeyStore = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\" + PlayerSettings.companyName + "\\" + PlayerSettings.productName);
            var prefsKeyNames = prefsKeyStore.GetValueNames();
            prefsKeyNames.ToList().Sort();
            var output = new StringBuilder();
            foreach (var prefsKey in prefsKeyNames)
            {
                OutputPrefsValue(output, prefsKey);
            }
            Debug.Log(output.ToString());
        }


        /// <summary>
        /// On Mac OS X PlayerPrefs are stored in ~/Library/Preferences folder, in a file named unity.[company name].[product name].plist, 
        /// where company and product names are the names set up in Project Settings. The same .plist file is used for both Projects run 
        /// in the Editor and standalone players. (http://docs.unity3d.com/ScriptReference/PlayerPrefs.html)
        /// </summary>
        void WriteOSXPreferences()
        {
            var prefsPath = "~/Library/Preferences/unity." + PlayerSettings.companyName + "." + PlayerSettings.productName + ".plist";

            if (File.Exists(prefsPath))
            {
                var prefsPlist = (Dictionary<string, object>)Plist.readPlist(prefsPath);
                var output = new StringBuilder();
                foreach (var prefsKey in prefsPlist.Keys)
                {
                    OutputPrefsValue(output, prefsKey);
                }
                Debug.Log(output.ToString());
            }
            else
            {
                Debug.Log("OSX Prefs file not found '" + prefsPath + "'");
            }
        }

        private static void OutputPrefsValue(StringBuilder output, string prefsKey)
        {
            var keyName = prefsKey.Substring(0, prefsKey.LastIndexOf("_"));
            var stringValue = PlayerPrefs.GetString(keyName, "DUMMY STRING");
            if (stringValue != "DUMMY STRING")
                output.AppendFormat("{0} (string): {1}\n", keyName, stringValue);
            else
            {
                var intValue = PlayerPrefs.GetInt(keyName, int.MinValue + 10);
                if (intValue != int.MinValue + 10)
                    output.AppendFormat("{0} (int): {1}\n", keyName, intValue);
                else
                {
                    var floatValue = PlayerPrefs.GetFloat(keyName, float.MinValue + 10);
                    if (floatValue != float.MinValue + 10)
                        output.AppendFormat("{0} (float): {1}\n", keyName, floatValue);
                }
            }
        }
    }
}
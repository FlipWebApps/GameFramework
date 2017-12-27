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
using GameFramework.FreePrize.Components;
using GameFramework.GameStructure;
using GameFramework.EditorExtras.Editor;
using UnityEditor;
using UnityEngine;
using GameFramework.GameStructure.Game.ObjectModel;
using System.Collections.Generic;
using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.Localisation;

namespace GameFramework.Debugging.Components.Editor {

    /// <summary>
    /// Component for allowing various cheat functions to be called such as increasing score, resetting prefs etc..
    /// </summary>
    public class CheatFunctionsWindow : EditorWindow
    {
        readonly string[] _tabNames = {"General", "Player", "World", "Level", "Free Prize", "Localisation"};
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
                case 5:
                    LocalisationMenuOptions();
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
            GUILayout.Label("Preferences", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Manage preference (PlayerPrefs). For more control and information enable Prefs Editor from the integrations window (Window Menu | Game Framework).", MessageType.None);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset", GUILayout.Width(100)))
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
                Debug.Log("Player prefs deleted. Note: Some gameobjects might hold values and write these out after this call!");
            }

            if (GUILayout.Button("Show Prefs Editor", GUILayout.Width(150)))
            {
#if PREFS_EDITOR
                PrefsEditor.Editor.PrefsEditorWindow.ShowWindow();
#else
                if (EditorUtility.DisplayDialog("Prefs Editor", "Prefs Editor is a seperate asset for displaying and managing preferences with optional encryption.\n\nPrefs Editor is available for purchase seperately or included free as a part of the Game Framework Extras Bundle.\n\nIf you already have this installed then enable Prefs Editor integration in the Window -> GameFramework -> Integrations window.", "More Information...", "Cancel"))
                    GameFramework.GameFrameworkHelper.ShowAssetStorePagePrefsEditor();
#endif
            }
            GUI.enabled = true;
            GUILayout.EndHorizontal();
        }

        void IAPMenuOptions()
        {
            // preferences
            GUILayout.Label("In App Purchase Simulation", EditorStyles.boldLabel);
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
            if (!Application.isPlaying)
            {
                GUILayout.Label("These functions are only available in play mode.", EditorHelper.ItalicLabelStyle(TextAnchor.MiddleCenter), GUILayout.ExpandWidth(true));
                return;
            }
            if (!GameManager.IsActive || GameManager.Instance.Players == null || GameManager.Instance.Players.Selected == null)
            {
                GUILayout.Label("These functions are only available when a GameManager is active and a player selected.", EditorHelper.ItalicLabelStyle(TextAnchor.MiddleCenter), GUILayout.ExpandWidth(true));
                return;
            }

            // player
            GUILayout.Label("Player", EditorStyles.boldLabel);
            // lives
            GUILayout.BeginHorizontal();
            GUILayout.Label("Lives (" + GameManager.Instance.Player.Lives + ")", GUILayout.Width(100));
            if (GUILayout.Button("-1", GUILayout.Width(50)))
            {
                GameManager.Instance.Player.Lives -= 1;
            }
            if (GUILayout.Button("+1", GUILayout.Width(50)))
            {
                GameManager.Instance.Player.Lives += 1;
            }
            GUILayout.EndHorizontal();
            // health
            GUILayout.BeginHorizontal();
            GUILayout.Label("Health (" + GameManager.Instance.Player.Health + ")", GUILayout.Width(100));
            if (GUILayout.Button("-0.1", GUILayout.Width(50)))
            {
                // only assign once!
                GameManager.Instance.Player.Health = GameManager.Instance.Player.Health - 0.1f < 0 ? 0 : GameManager.Instance.Player.Health - 0.1f;
            }
            if (GUILayout.Button("+0.1", GUILayout.Width(50)))
            {
                GameManager.Instance.Player.Health += 0.1f;
                GameManager.Instance.Player.Health = Mathf.Min(GameManager.Instance.Player.Health, 1);
            }
            GUILayout.EndHorizontal();

            ShowCounters(GameConfiguration.Instance.PlayerCounterConfiguration, GameManager.Instance.Players.Selected);
        }

        private void WorldMenuOptions()
        {
            if (!Application.isPlaying)
            {
                GUILayout.Label("These functions are only available in play mode.", EditorHelper.ItalicLabelStyle(TextAnchor.MiddleCenter), GUILayout.ExpandWidth(true));
                return;
            }
            if (!GameManager.IsActive || GameManager.Instance.Worlds == null || GameManager.Instance.Worlds .Selected == null)
            {
                GUILayout.Label("These functions are only available when a GameManager is active and a world selected.", EditorHelper.ItalicLabelStyle(TextAnchor.MiddleCenter), GUILayout.ExpandWidth(true));
                return;
            }

            GUILayout.Label("World", EditorStyles.boldLabel);
            // general
            GUILayout.BeginHorizontal();
            GUILayout.Label("Current Worlds", GUILayout.Width(100));
            if (GUILayout.Button("Unlock All", GUILayout.Width(100)))
            {
                foreach (var world in GameManager.Instance.Worlds.Items)
                {
                    world.IsUnlocked = true;
                    world.IsUnlockedAnimationShown = true;
                    world.UpdatePlayerPrefs();
                }
                PlayerPrefs.Save();
            }
            if (GUILayout.Button("Lock All", GUILayout.Width(100)))
            {
                foreach (var world in GameManager.Instance.Worlds.Items)
                {
                    world.IsUnlocked = false;
                    world.IsUnlockedAnimationShown = false;
                    world.UpdatePlayerPrefs();
                }
                PlayerPrefs.Save();
            }
            GUILayout.EndHorizontal();

            ShowCounters(GameConfiguration.Instance.WorldCounterConfiguration, GameManager.Instance.Worlds.Selected);
        }

        private void LevelMenuOptions()
        {
            if (!Application.isPlaying)
            {
                GUILayout.Label("These functions are only available in play mode.", EditorHelper.ItalicLabelStyle(TextAnchor.MiddleCenter), GUILayout.ExpandWidth(true));
                return;
            }
            if (!GameManager.IsActive || GameManager.Instance.Levels == null || GameManager.Instance.Levels.Selected == null)
            {
                GUILayout.Label("These functions are only available when a GameManager is active and a level selected.", EditorHelper.ItalicLabelStyle(TextAnchor.MiddleCenter), GUILayout.ExpandWidth(true));
                return;
            }

            GUILayout.Label("Level", EditorStyles.boldLabel);
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
                foreach (var level in GameManager.Instance.Levels.Items)
                {
                    level.IsUnlocked = false;
                    level.IsUnlockedAnimationShown = false;
                    level.UpdatePlayerPrefs();
                }
                PlayerPrefs.Save();
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

            ShowCounters(GameConfiguration.Instance.LevelCounterConfiguration, GameManager.Instance.Levels.Selected);
        }

        // show counters - int then float to keep the interface nice due to different buttons.
        static void ShowCounters(List<CounterConfiguration> counterConfigurationList, GameItem gameItem)
        {
            foreach (var counterConfiguration in counterConfigurationList)
            {
                GUILayout.BeginHorizontal();
                var counter = gameItem.GetCounter(counterConfiguration.Name);
                if (counter.Configuration.CounterType == CounterConfiguration.CounterTypeEnum.Int)
                {
                    GUILayout.Label(string.Format("{0} ({1}, {2})", counterConfiguration.Name, counter.IntAmount, counter.IntAmountBest), GUILayout.Width(100));
                    if (GUILayout.Button("-100", GUILayout.Width(50)))
                        counter.Decrease(100);
                    if (GUILayout.Button("-10", GUILayout.Width(50)))
                        counter.Decrease(10);
                    if (GUILayout.Button("-1", GUILayout.Width(50)))
                        counter.Decrease(1);
                    if (GUILayout.Button("0", GUILayout.Width(50)))
                        counter.Set(0);
                    if (GUILayout.Button("+1", GUILayout.Width(50)))
                        counter.Increase(1);
                    if (GUILayout.Button("+10", GUILayout.Width(50)))
                        counter.Increase(10);
                    if (GUILayout.Button("+100", GUILayout.Width(50)))
                        counter.Increase(100);
                }
                GUILayout.EndHorizontal();
            }
            foreach (var counterConfiguration in counterConfigurationList)
            {
                GUILayout.BeginHorizontal();
                var counter = gameItem.GetCounter(counterConfiguration.Name);
                if (counter.Configuration.CounterType == CounterConfiguration.CounterTypeEnum.Float)
                {
                    GUILayout.Label(string.Format("{0} ({1}, {2})", counterConfiguration.Name, counter.FloatAmount, counter.FloatAmountBest), GUILayout.Width(100));
                    if (GUILayout.Button("-100", GUILayout.Width(50)))
                        counter.Decrease(100f);
                    if (GUILayout.Button("-10", GUILayout.Width(50)))
                        counter.Decrease(10f);
                    if (GUILayout.Button("-1", GUILayout.Width(50)))
                        counter.Decrease(1f);
                    if (GUILayout.Button("-0.1", GUILayout.Width(50)))
                        counter.Decrease(0.1f);
                    if (GUILayout.Button("0", GUILayout.Width(50)))
                        counter.Set(0f);
                    if (GUILayout.Button("+0.1", GUILayout.Width(50)))
                        counter.Increase(0.1f);
                    if (GUILayout.Button("+1", GUILayout.Width(50)))
                        counter.Increase(1f);
                    if (GUILayout.Button("+10", GUILayout.Width(50)))
                        counter.Increase(10f);
                    if (GUILayout.Button("+100", GUILayout.Width(50)))
                        counter.Increase(100f);
                }
                GUILayout.EndHorizontal();
            }
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


        void LocalisationMenuOptions()
        {
            GUILayout.Label("Current language: " + GlobalLocalisation.Language, EditorStyles.boldLabel);
            foreach (var language in GlobalLocalisation.SupportedLanguages)
            {
                if (GUILayout.Button(language, GUILayout.Width(150)))
                {
                    if (!GlobalLocalisation.TrySetAllowedLanguage(language))
                        Debug.LogWarningFormat("Unable to set language {0}", language);
                    //FreePrizeManager.Instance.MakePrizeAvailable();
                }
            }
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

    }
}
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
using System.Collections.Generic;
using System.IO;

namespace GameFramework.GameFramework.Editor
{

    /// <summary>
    /// Window displayed on startup to help users get up and running.
    /// </summary>
    [InitializeOnLoad]
    public class StartupWindow : EditorWindow
    {
        Texture2D _startWindowIcon;

        static bool _autoShow;
        static string _gameFrameworkRootFolder;

        const string AutoShowPrefsKey = "GameFramework.StartupWindow.AutoShow";
        const int WindowWidth = 300;
        const int WindowHeight = 340;

        // Add menu item
        [MenuItem("Window/Game Framework/Startup Window", priority = 1)]
        public static void ShowWindow()
        {
            var window = GetWindow<StartupWindow>(true, "Startup Window", true);
            window.minSize = window.maxSize = new Vector2(WindowWidth, WindowHeight);

            _autoShow = EditorPrefs.GetBool(AutoShowPrefsKey, true);
        }

        /// <summary>
        /// Static constructor - register update so we can show window
        /// </summary>
        static StartupWindow()
        {
            EditorApplication.update += Update;
        }

        /// <summary>
        /// Perform startup check to show window 1 time only.
        /// </summary>
        static void Update()
        {
            // Show startup window if do not show hasn't been set
            if (EditorPrefs.GetBool(AutoShowPrefsKey, true))
            {
                ShowWindow();
            }

            // Add GAME_FRAMEWORK define if not already set.
            if (!PlayerSettingsHelper.IsScriptingDefineSet("GAME_FRAMEWORK"))
                PlayerSettingsHelper.AddScriptingDefineAllTargets("GAME_FRAMEWORK");

            EditorApplication.update -= Update;
        }

        void OnEnable()
        {
            string basePathToStartWindowIcon = FindGameFrameworkRootFolder();
            string relativePathToStartWindowIcon = @"\Sprites\StartWindow\StartWindow.png";
            string pathTostartWindowIcon = string.Concat(basePathToStartWindowIcon, relativePathToStartWindowIcon);

            _startWindowIcon = AssetDatabase.LoadAssetAtPath(pathTostartWindowIcon, typeof(Texture2D)) as Texture2D;
        }

        void OnGUI()
        {
            GUI.DrawTexture(new Rect(0, 0, 400, 100), _startWindowIcon);

            var buttonsRect = new Rect(10, 110, WindowWidth - 20, 20);
            if (GUI.Button(buttonsRect, "Homepage"))
                GameFrameworkHelper.ShowHomepage();

            buttonsRect.y += 30;
            if (GUI.Button(buttonsRect, "Documentation"))
                GameFrameworkHelper.ShowDocumentation();

            buttonsRect.y += 30;
            if (GUI.Button(buttonsRect, "Getting Started Tutorials"))
                GameFrameworkHelper.ShowOnlineTutorials();

            buttonsRect.y += 30;
            if (GUI.Button(buttonsRect, "Support Forum"))
                GameFrameworkHelper.ShowSupportForum();

            buttonsRect.y += 30;
            if (GUI.Button(buttonsRect, "Integrations"))
                IntegrationsWindow.ShowWindow();

            buttonsRect.y += 30;
            if (GUI.Button(buttonsRect, "Contact"))
                GameFrameworkHelper.ShowContact();

            buttonsRect.y += 30;
            if (GUI.Button(buttonsRect, "Like it? Rate Us"))
                GameFrameworkHelper.ShowAssetStorePage();

            var newAutoShow = GUI.Toggle(new Rect(10, WindowHeight - 20, WindowWidth - 20, 20), _autoShow, "Show on startup");
            if (newAutoShow != _autoShow)
            {
                _autoShow = newAutoShow;
                EditorPrefs.SetBool(AutoShowPrefsKey, _autoShow);
            }
        }

        string FindGameFrameworkRootFolder()
        {
            //perform BFS to find GameFramework folder
            //source: https://stackoverflow.com/a/7296966/3183423 but heavily modified

            string currentPath = @"Assets";
            Queue<string> queue = new Queue<string>();
            queue.Enqueue(currentPath);
            bool found = false;

            while (!found && queue.Count > 0)
            {
                currentPath = queue.Dequeue();

                if (currentPath.EndsWith(@"GameFramework"))
                {
                    found = true;
                    break;
                }

                string[] subPaths = Directory.GetDirectories(currentPath);

                foreach (string subPath in subPaths)
                {
                    queue.Enqueue(subPath);
                }
            }

            return currentPath;
        }
    }
}
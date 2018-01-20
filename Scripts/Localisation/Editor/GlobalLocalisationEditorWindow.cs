//----------------------------------------------
// Flip Web Apps: Prefs Editor
// Copyright © 2016-2017 Flip Web Apps / Mark Hewitt
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

using UnityEditor;
using UnityEngine;
using System.Linq;
using GameFramework.EditorExtras.Editor;

namespace GameFramework.Localisation.Editor
{
    /// <summary>
    /// Editor window for viewing global localisation.
    /// </summary>
    public class GlobalLocalisationEditorWindow : EditorWindow
    {
        public int SelectedKeyIndex;

        //Texture2D _newIcon;
        //Texture2D _saveIcon;
        Texture2D _refreshIcon;
        //Texture2D _deleteIcon;
        //Texture2D _redTexture;

        Vector2 _scrollPosition = Vector2.zero;
        Vector2 _scrollPositionKeys = Vector2.zero;
        [SerializeField]
        float _leftPanelWidth;
        bool _isResizing;

        [SerializeField]
        bool _showNew;
        string _newKey;

        // Add menu item for showing the window
        //[MenuItem("Window/Game Framework/Global Localisation Viewer", priority = 1)]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            //var prefsEditorWindow = 
            GetWindow<GlobalLocalisationEditorWindow>("Global Localisations", true);
        }


        public static void ShowWindowNew(string key = "")
        {
            //Show existing window instance. If one doesn't exist, make one.
            var localisationEditorWindow =
            GetWindow<GlobalLocalisationEditorWindow>("Global Localisations", true);
            localisationEditorWindow._showNew = true;
        }


        void OnEnable()
        {
            //_newIcon = AssetDatabase.LoadAssetAtPath(@"Assets\FlipWebApps\PrefsEditor\Sprites\New.png", typeof(Texture2D)) as Texture2D;
            //_saveIcon = AssetDatabase.LoadAssetAtPath(@"Assets\FlipWebApps\PrefsEditor\Sprites\Save.png", typeof(Texture2D)) as Texture2D;
            _refreshIcon = AssetDatabase.LoadAssetAtPath(@"Assets\FlipWebApps\PrefsEditor\Sprites\Refresh.png", typeof(Texture2D)) as Texture2D;
            //_deleteIcon = AssetDatabase.LoadAssetAtPath(@"Assets\FlipWebApps\PrefsEditor\Sprites\Delete.png", typeof(Texture2D)) as Texture2D;
            //_redTexture = MakeColoredTexture(1, 1, new Color(1.0f, 0.0f, 0.0f, 0.1f));
            //RefreshPlayerPrefs();

#if UNITY_2017_2_OR_NEWER
            EditorApplication.playModeStateChanged += OnPlaymodeStateChanged;
#else
            EditorApplication.playmodeStateChanged += OnPlaymodeStateChanged;
#endif
        }



        void OnDisable()
        {
#if UNITY_2017_2_OR_NEWER
            EditorApplication.playModeStateChanged -= OnPlaymodeStateChanged;
#else
            EditorApplication.playmodeStateChanged -= OnPlaymodeStateChanged;
#endif
        }


        /// <summary>
        /// When the playmode changes, clear the log if necessary
        /// </summary>
        void OnPlaymodeStateChanged()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
                Debug.LogWarning("TODO: Check for changes first then prompt here. For now, close the localisation window to stop showing this message.");
            //EditorUtility.DisplayDialog("Save", "TODO: Check for changes first then prompt here. For now, close the localisation window to stop showing this message.", "Yes",
            //    "No");
        }


#if UNITY_2017_2_OR_NEWER
        /// <summary>
        /// When the playmode changes, clear the log if necessary (2017.2+ version)
        /// </summary>
        void OnPlaymodeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
                Debug.LogWarning("TODO: Check for changes first then prompt here. For now, close the localisation window to stop showing this message.");
        }
#endif


        /// <summary>
        /// Draw the GUI
        /// </summary>
        void OnGUI()
        {
            GlobalLocalisation.Load(); // ensure loaded

            DrawToolbar();
            if (_showNew) DrawNew();
            GUILayout.Space(5);
            DrawKeyEntries();
        }


        /// <summary>
        /// Draws the toolbar.
        /// </summary>
        void DrawToolbar()
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Global Localisation", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("For now you must add / edit manually in the Localisation.csv file for now (see online help).");
            EditorGUILayout.LabelField("Please show your support and rate Game Framework on the asset store");
            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true));
            //if (ButtonTrimmed("New...", _newIcon, EditorStyles.toolbarButton, "Add a new item"))
            //{
            //    _newKey = "";
            //    _showNew = true;
            //    ClearFocus();
            //}

            //GUI.enabled = false;
            //if (ButtonTrimmed("Save All", _saveIcon, EditorStyles.toolbarButton, "Save modified entries"))
            //{
            //    Save();
            //    Reload();
            //}

            //if (ButtonTrimmed("Delete All...", null, EditorStyles.toolbarButton, "Delete all prefs entries"))
            //{
            //    if (EditorUtility.DisplayDialog("Delete All Player Prefs",
            //        "Are you sure you want to delete all Player Prefs?", "Yes", "No"))              
            //        DeleteAll();
            //}
            GUILayout.FlexibleSpace();
            GUI.enabled = true;

            if (ButtonTrimmed("Refresh", _refreshIcon, EditorStyles.toolbarButton, "Reload prefs to reflect any changes"))
            {
                Debug.LogWarning("Localisation Window TODO: Prompt if changes will be lost.");
                Reload();
            }

            EditorGUILayout.EndHorizontal();
        }




        /// <summary>
        /// Draws the new item.
        /// </summary>
        void DrawNew()
        {
            EditorGUILayout.BeginVertical("box");
            _newKey = EditorGUILayout.TextField(new GUIContent("Key", "A unique key for this localisation item"), _newKey);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (ButtonTrimmed("Add", null, EditorStyles.miniButtonRight, "Create a new prefs item with the values entered above."))
            {
                EditorUtility.DisplayDialog("Message", "Doesn't do anything yet - add manually to the localisation.csv text file!!", "Ok");

                //if (!string.IsNullOrEmpty(_newItemKey))
                //{
                //    PlayerPrefsEntry newPlayerPrefsEntry = null;
                //    switch (_newItemType)
                //    {
                //        case SecurePlayerPrefs.ItemType.Int:
                //            newPlayerPrefsEntry = new PlayerPrefsEntry(_newItemKey, _newItemValueInt, _newItemEncrypted);
                //            break;
                //        case SecurePlayerPrefs.ItemType.Float:
                //            newPlayerPrefsEntry = new PlayerPrefsEntry(_newItemKey, _newItemValueFloat, _newItemEncrypted);
                //            break;
                //        case SecurePlayerPrefs.ItemType.String:
                //            newPlayerPrefsEntry = new PlayerPrefsEntry(_newItemKey, _newItemValueString, _newItemEncrypted);
                //            break;
                //    }
                //    newPlayerPrefsEntry.Save();
                //    _playerPrefsEntries.Add(newPlayerPrefsEntry);
                //}
                ClearFocus();
                _showNew = false;
            }

            if (ButtonTrimmed("Cancel", null, EditorStyles.miniButtonRight, "Close this popup without adding a new localisation item"))
            {
                _showNew = false;
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }


        /// <summary>
        /// Draw the player prefs entries
        /// </summary>
        private void DrawKeyEntries()
        {
            var normalbackgroundColor = GUI.backgroundColor;

            EditorGUILayout.BeginHorizontal();

            _scrollPositionKeys = EditorGUILayout.BeginScrollView(_scrollPositionKeys, GUILayout.Width(_leftPanelWidth), GUILayout.MinWidth(100));
            var keys = GlobalLocalisation.LocalisationData.EntriesDictionary.Keys.ToList();
            keys.Sort();
            for (var i = 0; i < keys.Count; i++)
            {
                var k = keys[i];
                var s = new GUIStyle();
                s.normal.background = MakeColoredTexture(1, 1, new Color(1.0f, 1.0f, 1.0f, 0.1f));
                GUI.backgroundColor = SelectedKeyIndex == i ? Color.blue : normalbackgroundColor;
                GUILayout.BeginHorizontal(s);
                GUILayout.Label(k, GUILayout.ExpandWidth(true));
                GUILayout.EndHorizontal();
                if (Event.current.button == 0 && Event.current.type == EventType.MouseUp)
                {
                    if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                    {
                        SelectedKeyIndex = i;

                        ClearFocus();
                        Repaint();
                    }
                    // Handle events here
                }
            }
            GUI.backgroundColor = normalbackgroundColor;
            EditorGUILayout.EndScrollView();

            var lastrect = GUILayoutUtility.GetLastRect();
            lastrect.x = lastrect.xMax + 5;
            lastrect.width = 3;
            GUILayout.Space(5);

            HandleResize(lastrect);
            GUILayout.Space(5);

            //var drawnLines = 0;
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(position.width - _leftPanelWidth));

            LocaliseText.LoadDictionary();

            //var boldGUIStyle = new GUIStyle(EditorStyles.numberField);
            //boldGUIStyle.fontStyle = FontStyle.Bold;
            for (var i = 0; i < LocaliseText.Languages.Length; i++)
            {
                var languageEntry = GlobalLocalisation.LocalisationData.Languages[i];

                string stringValue = GlobalLocalisation.LocalisationData.GetText(keys[SelectedKeyIndex], i);
                //float num = EditorStyles.textArea.CalcHeight(new GUIContent(stringValue), EditorGUIUtility.currentViewWidth);
                //int num2 = Mathf.CeilToInt(num / 13f);
                //num2 = Mathf.Clamp(num2, 1, int.MaxValue);
                //var height = 32f + (float)((num2 - 1) * 13);
                //Debug.Log(num2 + ", " + height);
                EditorGUILayout.LabelField(languageEntry.Name, EditorStyles.boldLabel);
                EditorGUILayout.TextArea(stringValue, GuiStyles.WordWrapStyle, GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth));

                //EditorGUI.BeginChangeCheck();
                //stringValue = EditorGUILayout.TextArea(stringValue, EditorStyles.textArea, GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth), GUILayout.MinHeight(height), GUILayout.MaxHeight(height));
                //if (EditorGUI.EndChangeCheck())
                //{
                //    //LocaliseText.Localisations[key][i] = stringValue;
                //}
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.EndHorizontal();
        }


        #region ToolbarOptions

        /// <summary>
        /// save all changes
        /// </summary>
        void Save()
        {
        }


        /// <summary>
        /// Delete all entries
        /// </summary>
        void DeleteAll()
        {
        }

        #endregion ToolbarOptions


        #region Load 
        /// <summary>
        /// Reload the localisation files
        /// </summary>
        void Reload()
        {
            LocaliseText.ReloadDictionary();
        }

        #endregion Load



        /// <summary>
        /// Handle resize of the split messages window
        /// </summary>
        void HandleResize(Rect dragRect, Color? resizeAreaColour = null)
        {
            resizeAreaColour = resizeAreaColour ?? GUI.backgroundColor * 0.5f;

            GUI.DrawTexture(dragRect, MakeColoredTexture(1, 1, resizeAreaColour.Value));
            EditorGUIUtility.AddCursorRect(dragRect, MouseCursor.ResizeHorizontal);

            if (Event.current.type == EventType.MouseDown && dragRect.Contains(Event.current.mousePosition))
            {
                _isResizing = true;
            }

            if (_isResizing)
            {
                _leftPanelWidth = Mathf.Clamp(Event.current.mousePosition.x, 100, position.width - 100);
                Repaint();
            }

            if (Event.current.type == EventType.MouseUp)
                _isResizing = false;

        }


        #region Editor Helper Functions

        /// <summary>
        /// Show a button trimmed to the length of the text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public static bool ButtonTrimmed(string text, GUIStyle style)
        {
            return GUILayout.Button(text, style, GUILayout.MaxWidth(style.CalcSize(new GUIContent(text)).x));
        }


        /// <summary>
        /// Show a button trimmed to the length of the text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="texture"></param>
        /// <param name="style"></param>
        /// <param name="tooltip"></param>
        /// <returns></returns>
        public static bool ButtonTrimmed(string text, Texture2D texture, GUIStyle style, string tooltip = null)
        {
            if (texture != null)
                return GUILayout.Button(new GUIContent(text, texture, tooltip), style, GUILayout.MaxWidth(style.CalcSize(new GUIContent(text)).x + texture.width));
            else
                return ButtonTrimmed(text, style);
        }


        /// <summary>
        /// Make a texture of the given size and color
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private Texture2D MakeColoredTexture(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }

        /// <summary>
        /// Clear focus from the current item
        /// </summary>
        public void ClearFocus()
        {
            GUIUtility.keyboardControl = 0;
        }

        #endregion Editor Helper Functions

    }
}

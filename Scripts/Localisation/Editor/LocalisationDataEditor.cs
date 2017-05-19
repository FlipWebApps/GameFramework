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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework.EditorExtras.Editor;
using GameFramework.Localisation.ObjectModel;
using UnityEditor;
using UnityEngine;

namespace GameFramework.Localisation.Editor
{
    [CustomEditor(typeof(LocalisationData))]
    [Serializable]
    public class LocalisationDataEditor : UnityEditor.Editor
    {
        LocalisationData _targetLocalisationData;
        SerializedProperty _languagesProperty;

        Rect _mainHelpRect;
        int _currentTab;

        // Entries tab variables
        Rect _entriesHelpRect;
        [SerializeField]
        string _filter;
        [SerializeField]
        Vector2 _entriesScrollPosition;
        float _entriesScrollHeight;
        int _entriesDefaultRowHeight = 16;
        string _newEntry;
        [SerializeField]
        public List<EntryData> _entryDataList;

        // Languages tab variables
        Rect _languagesHelpRect;
        Vector2 _languagesScrollPosition;
        string _newLanguage;

        // Tools tab variables
        Rect _importExportHelpRect;
        string _importExportFilename;

        protected virtual void OnEnable()
        {
            _targetLocalisationData = target as LocalisationData;

            // setup for entries tab
            // set scroll height to initial value (full screen height) - we can later set to the correct size in Repaint event.
            if (_entriesScrollHeight <= 0) _entriesScrollHeight = Screen.height;

            // setup for languages tab
            _languagesProperty = serializedObject.FindProperty("_languages");

            // setup for tools tab
            _importExportFilename = Application.dataPath;
        }

        public override void OnInspectorGUI()
        {
            _mainHelpRect = EditorHelper.ShowHideableHelpBox("GameFramework.LocalisationEditorWindow.Main", "This localisation file is where you can define localised text in different languages.\n\nIf you have previously used .csv files then you can import these under the tools tab.", _mainHelpRect);

            _currentTab = GUILayout.Toolbar(_currentTab, new string[] { "Entries", "Languages", "Tools" });
            switch (_currentTab)
            {
                case 0:
                    DrawEntries();
                    break;
                case 1:
                    DrawLanguages();
                    break;
                case 2:
                    DrawTools();
                    break;
            }
        }


        protected void DrawEntries() {
            _entriesHelpRect = EditorHelper.ShowHideableHelpBox("GameFramework.LocalisationEditorWindow.Entries", "Entries contain a set of unique tags that identify the text that you want to localise. You can further associate different translations with these tags for the different languages that you have setup.", _entriesHelpRect);
            SyncEntries();
            Debug.Log(_entryDataList.Count);
            Undo.RecordObject(target, "Localisation Entry Changed");

            // filter
            EditorGUILayout.BeginHorizontal();
            GUI.changed = false;
            EditorGUI.BeginChangeCheck();
            _filter = EditorGUILayout.TextField(_filter, GuiStyles.ToolbarSearchField, GUILayout.ExpandWidth(true));
            if (EditorGUI.EndChangeCheck())
            {
                foreach (var entry in _entryDataList)
                    entry.MatchesFilter = entry.LocalisationEntry.Key.IndexOf(_filter, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            if (GUILayout.Button("", string.IsNullOrEmpty(_filter) ? GuiStyles.ToolbarSearchFieldCancelEmpty : GuiStyles.ToolbarSearchFieldCancel, GUILayout.ExpandWidth(false)))
            {
                _filter = "";
                GUIUtility.keyboardControl = 0;
                foreach (var entry in _entryDataList)
                    entry.MatchesFilter = true;
            }
            EditorGUILayout.EndHorizontal();

            _entriesScrollPosition = EditorGUILayout.BeginScrollView(_entriesScrollPosition, false, false, GUILayout.ExpandHeight(false));

            // here we avoid using properties due to them being very slow!
            var indexForDeleting = -1;
            var accumulativeHeightDrawn = 0;
            var accumulativeSpace = 0;
            for (var i = 0; i < _entryDataList.Count; i++)
            {
                var entryData = _entryDataList[i];
                if (entryData.MatchesFilter)
                {
                    // if top is below viewport or bottom is above then this item is not visible. For repaint events we need to use the 
                    // previously recorded display state to avoid changing the number of controls drawn.
                    //if ((Event.current.type == EventType.Repaint && entryData.IsShown == false) ||
                    //    (Event.current.type != EventType.Repaint &&
                    if ((Event.current.type != EventType.Layout && entryData.IsShown == false) ||
                        (Event.current.type == EventType.Layout &&
                         (accumulativeHeightDrawn > _entriesScrollPosition.y + _entriesScrollHeight ||
                          accumulativeHeightDrawn + entryData.Height < _entriesScrollPosition.y)))
                    {
                        accumulativeHeightDrawn += entryData.Height;
                        accumulativeSpace += entryData.Height;
                        entryData.IsShown = false;
                    }
                    else
                    {
                        // fill space from skipped.
                        if (accumulativeSpace > 0) GUILayout.Space(accumulativeSpace);
                        accumulativeSpace = 0;

                        // draw the displayed item
                        entryData.IsShown = true;
                        var localisationEntry = _entryDataList[i].LocalisationEntry;
                        EditorGUILayout.BeginHorizontal();
                        EditorGUI.indentLevel++;
                        entryData.IsExpanded = EditorGUILayout.Foldout(entryData.IsExpanded, localisationEntry.Key);
                        EditorGUI.indentLevel--;
                        if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(GuiStyles.RemoveButtonWidth)))
                        {
                            indexForDeleting = i;
                            break;
                        }
                        EditorGUILayout.EndHorizontal();
                        if (Event.current.type == EventType.Repaint)
                            //entryData.Height = (int)GUILayoutUtility.GetLastRect().height; // only works on repaint.
                            entryData.Height = _entriesDefaultRowHeight;

                        // handle expended status
                        if (entryData.IsExpanded)
                        {
                            EditorGUILayout.BeginVertical();
                            EditorGUI.indentLevel++;
                            for (var li = 0; li < localisationEntry.Languages.Length; li++)
                            {
                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.LabelField(_targetLocalisationData.Languages[li].Name,
                                    GUILayout.Width(100));
                                EditorGUI.BeginChangeCheck();
                                var lang = EditorGUILayout.TextArea(localisationEntry.Languages[li], GuiStyles.WordWrapStyle, GUILayout.Width(Screen.width - 148 - 60 - 2));
                                if (EditorGUI.EndChangeCheck())
                                {
                                    localisationEntry.Languages[li] = lang;
                                }

                                if (li > 0 && GUILayout.Button("Translate", EditorStyles.miniButton, GUILayout.Width(60)))
                                {
                                    var sourceCode = _targetLocalisationData.Languages[0].Code;
                                    if (!string.IsNullOrEmpty(sourceCode))
                                    {
                                        var targetCode = _targetLocalisationData.Languages[li].Code;
                                        if (!string.IsNullOrEmpty(targetCode))
                                        {
                                            var sourceText = localisationEntry.Languages[0];
                                            string url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl="
                                                         + sourceCode + "&tl=" + targetCode + "&dt=t&q=" + WWW.EscapeURL(sourceText);

                                            WWW www = new WWW(url);
                                            while (!www.isDone) ;
                                            if (www.error != null)
                                            {
                                                Debug.LogError(www.error);
                                            }
                                            else
                                            {
                                                Debug.Log(www.text);
                                            }
                                        }
                                        else
                                        {
                                            EditorUtility.DisplayDialog("Localisation Import", "There is no code specified for the language '" + _targetLocalisationData.Languages[li].Name + "'.\n\nPlease enter under the languages tab.", "Ok");
                                        }
                                    }
                                    else
                                    {
                                        EditorUtility.DisplayDialog("Localisation Translate", "There is no code specified for the language '" + _targetLocalisationData.Languages[0].Name + "'.\n\nPlease enter under the languages tab.", "Ok");
                                    }
                                }


                                EditorGUILayout.EndHorizontal();
                            }
                            EditorGUI.indentLevel--;
                            EditorGUILayout.EndVertical();

                            // repaint events will give a new correct size for the last drawn rect so record here.
                            if (Event.current.type == EventType.Repaint)
                                entryData.Height += (int)GUILayoutUtility.GetLastRect().height; // only works on repaint.
                        }

                        accumulativeHeightDrawn += entryData.Height;
                       // Debug.Log(entryData.LocalisationEntry.Key + ", " + accumulativeHeightDrawn);
                    }
                }
            }
            if (accumulativeSpace > 0) GUILayout.Space(accumulativeSpace);

            EditorGUILayout.EndScrollView();

            if (Event.current.type == EventType.Repaint)
                _entriesScrollHeight = GUILayoutUtility.GetLastRect().height; // only works on repaint.

            // add functionality
            EditorGUILayout.BeginHorizontal();
            bool entryAddPressed = false;
            if (GUI.GetNameOfFocusedControl() == "EntryAdd" && Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
            {
                entryAddPressed = true;
                Event.current.Use();
            }
            GUI.SetNextControlName("EntryAdd");
            _newEntry = EditorGUILayout.TextField("", _newEntry, GUILayout.ExpandWidth(true));
            var isValidEntry = !string.IsNullOrEmpty(_newEntry) && !_targetLocalisationData.ContainsEntry(_newEntry);
            GUI.enabled = isValidEntry;
            if (entryAddPressed || GUILayout.Button(new GUIContent("Add", "Add the specified entry to the list"), EditorStyles.miniButton, GUILayout.Width(100)))
            {
                if (isValidEntry)
                {
                    var newLocalisationEntry = _targetLocalisationData.AddEntry(_newEntry);
                    serializedObject.Update();
                    var insertIndex = 0;
                    var scrollOffset = 0;
                    for (; insertIndex < _entryDataList.Count; insertIndex++)
                    {
                        if (_newEntry.CompareTo(_entryDataList[insertIndex].LocalisationEntry.Key) < 0)
                            break;
                        else
                            scrollOffset += _entryDataList[insertIndex].Height;
                    }
                    _entryDataList.Insert(insertIndex, new EntryData()
                    {
                        LocalisationEntry = newLocalisationEntry,
                        MatchesFilter = _newEntry.IndexOf(_filter + "", StringComparison.OrdinalIgnoreCase) >= 0
                    });
                    _entriesScrollPosition.y = scrollOffset;

                    var lastDot = _newEntry.LastIndexOf(".");
                    if (lastDot == -1)
                        _newEntry = "";
                    else
                        _newEntry = _newEntry.Substring(0, lastDot + 1);
                }
                GUI.FocusControl("EntryAdd");
            }
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();

            // delay deleting to avoid editor issues.
            if (indexForDeleting != -1 &&
                EditorUtility.DisplayDialog("Delete Entry?", "Are you sure you want to delete this entry?", "Yes", "No"))
            {
                _targetLocalisationData.RemoveEntry(_entryDataList[indexForDeleting].LocalisationEntry.Key);
                _entryDataList.RemoveAt(indexForDeleting);
                serializedObject.Update();
            }
        }

        private void SyncEntries()
        {
            if (_entryDataList == null) _entryDataList = new List<EntryData>();
            if (_entryDataList.Count != _targetLocalisationData.Entries.Count)
            {
                // find a match in target or add.
                foreach (var entry in _targetLocalisationData.Entries)
                {
                    var match = false;
                    foreach (var entryData in _entryDataList)
                        if (entry.Equals(entryData.LocalisationEntry))
                            match = true;
                    if (!match) _entryDataList.Add(new EntryData() { Height = _entriesDefaultRowHeight, LocalisationEntry = entry });
                }
                // find a match in target or remove.
                for (int i = _entryDataList.Count - 1; i >= 0; i--)
                {
                    var entryData = _entryDataList[i];
                    var match = false;
                    foreach (var entry in _targetLocalisationData.Entries)
                        if (entry.Equals(entryData.LocalisationEntry))
                            match = true;
                    if (!match) _entryDataList.RemoveAt(i);
                }
                _entryDataList.Sort((x, y) => x.LocalisationEntry.Key.CompareTo(y.LocalisationEntry.Key));
            }
            var internalStateError = _targetLocalisationData.InternalVerifyState();
            if (internalStateError != null)
                Debug.Log("Internal state error - please report including the actions you just took Thanks!" + _targetLocalisationData.InternalVerifyState());
        }


        [Serializable]
        public class EntryData
        {
            public bool IsExpanded;
            public bool IsShown;
            public bool MatchesFilter = true;
            public int Height;
            public LocalisationEntry LocalisationEntry;
        }


        #region Languages

        protected void DrawLanguages()
        {
            serializedObject.Update();

            _languagesHelpRect = EditorHelper.ShowHideableHelpBox("GameFramework.LocalisationEditorWindow.Languages", "Here you can specify the languages for which you will provide localised values.\n\nYou should enter the language name and also an optional ISO-639-1 code for use with google translate if you want to perform automatic translations. For convenience Unity supported languages are available from the dropdown button at the bottom right.", _languagesHelpRect);
            EditorGUILayout.BeginVertical("Box");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Name", GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("Code", GUILayout.Width(60 + GuiStyles.RemoveButtonWidth + 6));
            //EditorGUILayout.LabelField("", GUILayout.Width(GuiStyles.RemoveButtonWidth));
            EditorGUILayout.EndHorizontal();

            string languageForDeleting = null;
            for (var i = 0; i < _languagesProperty.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();

                var languageProperty = _languagesProperty.GetArrayElementAtIndex(i);
                var nameProperty = languageProperty.FindPropertyRelative("Name");
                EditorGUILayout.PropertyField(nameProperty, GUIContent.none, GUILayout.ExpandWidth(true));

                var codeProperty = languageProperty.FindPropertyRelative("Code");
                EditorGUILayout.PropertyField(codeProperty, GUIContent.none, GUILayout.Width(60));

                if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(GuiStyles.RemoveButtonWidth)))
                {
                    languageForDeleting = nameProperty.stringValue;
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            // add functionality
            EditorGUILayout.BeginHorizontal();
            _newLanguage = EditorGUILayout.TextField("", _newLanguage, GUILayout.ExpandWidth(true));
            if (string.IsNullOrEmpty(_newLanguage) || _targetLocalisationData.ContainsLanguage(_newLanguage))
                GUI.enabled = false;
            if (GUILayout.Button(new GUIContent("Add", "Add the specified language to the list"), EditorStyles.miniButton, GUILayout.Width(100)))
            {
                serializedObject.ApplyModifiedProperties();
                _targetLocalisationData.AddLanguage(_newLanguage);
                serializedObject.Update();
                _newLanguage = "";
            }
            GUI.enabled = true;

            //if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Plus More", "Add to list"), GUILayout.Width(25)))
            if (GUILayout.Button(new GUIContent("+", "Add a new language to the list"), EditorStyles.miniButton, GUILayout.Width(20)))
            {
                serializedObject.ApplyModifiedProperties();
                var menu = new GenericMenu();
                for (var i = 0; i < Languages.LanguageDefinitions.Length; i++)
                {
                    if (!_targetLocalisationData.ContainsLanguage(Languages.LanguageDefinitions[i].Name))
                        menu.AddItem(new GUIContent(Languages.LanguageDefinitions[i].Name + " (" + Languages.LanguageDefinitions[i].Code + ")"), false, AddLanguage, Languages.LanguageDefinitions[i].Name);
                }
                menu.ShowAsContext();
            }
            EditorGUILayout.EndHorizontal();

            // delay deleting to avoid editor issues.
            if (languageForDeleting != null && 
                EditorUtility.DisplayDialog("Delete Language?", "Are you sure you want to delete this language?", "Yes", "No"))
            {
                //TODO: Show a warning first!
                serializedObject.ApplyModifiedProperties();
                _targetLocalisationData.RemoveLanguage(languageForDeleting);
                serializedObject.Update();
            }

            serializedObject.ApplyModifiedProperties();

        }

        void AddLanguage(object languageObject)
        {
            var language = languageObject as string;
            _targetLocalisationData.AddLanguage(language, Languages.LanguageDefinitionsDictionary[language].Code);
            serializedObject.Update();
        }

        #endregion Languages

        #region Tools

        protected void DrawTools()
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField(new GUIContent("Import / Export", ""), EditorStyles.boldLabel);
            _importExportHelpRect = EditorHelper.ShowHideableHelpBox("GameFramework.LocalisationEditorWindow.ImportExport", "If you would like to edit the data outside Unity then you can import from and export to .csv (text) files. Entries from any imported file will be merged with existing entries, replacing any keys that already exist with a similar name.\n\nIf you have previously used .csv files for localisation then you should use the import button to import old files into the new localisation system", _importExportHelpRect);
            if (GUILayout.Button("Import csv", EditorStyles.miniButton))
            {
                var newFileName = EditorUtility.OpenFilePanel("Select a .csv localisation file", _importExportFilename, "csv");
                if (!string.IsNullOrEmpty(newFileName))
                {
                    _importExportFilename = newFileName;
                    if (_targetLocalisationData.LoadCsv(_importExportFilename))
                        EditorUtility.DisplayDialog("Localisation Import", "Import complete!", "Ok");
                    else
                        EditorUtility.DisplayDialog("Localisation Import", "Import failed!\n\nSee the console window for further details.", "Ok");
                }
            }
            if (GUILayout.Button("Export csv", EditorStyles.miniButton))
            {
                var newFileName = EditorUtility.SaveFilePanel("Select a .csv localisation file", _importExportFilename, "localisation", "csv");
                if (!string.IsNullOrEmpty(newFileName))
                {
                    _importExportFilename = newFileName;
                    if (_targetLocalisationData.WriteCsv(_importExportFilename))
                        EditorUtility.DisplayDialog("Localisation Export", "Export complete!", "Ok");
                    else
                        EditorUtility.DisplayDialog("Localisation Export", "Export failed!\n\nSee the console window for further details.", "Ok");
                }
            }
            EditorGUILayout.EndVertical();
        }

        #endregion Tools
    }
}

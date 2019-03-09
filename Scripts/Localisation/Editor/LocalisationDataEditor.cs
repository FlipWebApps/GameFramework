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
using GameFramework.EditorExtras.Editor;
using GameFramework.Localisation.ObjectModel;
using UnityEditor;
using UnityEngine;

namespace GameFramework.Localisation.Editor
{
    [CustomEditor(typeof(LocalisationData))]
    public class LocalisationDataEditor : UnityEditor.Editor
    {
        LocalisationData _targetLocalisationData;
        SerializedProperty _languagesProperty;

        Rect _mainHelpRect;
        int _currentTab;
        bool _targetChanged;

        // Entries tab variables
        Rect _entriesHelpRect;
        string _filter;
        Vector2 _entriesScrollPosition;
        float _entriesScrollHeight;
        int _entriesDefaultRowHeight = 16;
        string _newKey;
        public List<EntryReference> _entryReferenceList;
        int _indexForFocus; // for focus on an item after adding.

        // Languages tab variables
        Rect _languagesHelpRect;
        Vector2 _languagesScrollPosition;
        string _newLanguage;
        int _languagesCount;

        // Tools tab variables
        Rect _importExportHelpRect;
        string _importExportFilename;

        protected virtual void OnEnable()
        {
            _targetLocalisationData = target as LocalisationData;

            // setup for entries tab
            // set scroll height to initial value (full screen height) - we can later set to the correct size in Repaint event.
            _entryReferenceList = new List<EntryReference>();
            if (_entriesScrollHeight <= 0) _entriesScrollHeight = Screen.height;

            // setup for languages tab
            _languagesProperty = serializedObject.FindProperty("_languages");
            _languagesCount = -1;

            // setup for tools tab
            _importExportFilename = Application.dataPath;
        }

        public override void OnInspectorGUI()
        {
            _mainHelpRect = EditorHelper.ShowHideableHelpBox("GameFramework.LocalisationEditorWindow.Main", "Welcome to the new Game Framework localisation system!\n\nThese localisation files are where you can define localised text in different languages.\n\nIf you have previously used .csv files then you can import these under the tools tab.\n\nIf you experience any problems, can help with new translations, or have improvement suggestions then please get in contact. Your support is appreciated.", _mainHelpRect);
            _targetChanged = false;

            // Additional handling for detecting undo / redo and set current tab.
            // for language changes just set focus to correct tab - we don't maintain any internal state so nothing more needed
            if (_targetLocalisationData.Languages.Count != _languagesCount)
            {
                _currentTab = 1;
                _languagesCount = _targetLocalisationData.Languages.Count;
                _targetChanged = true;
            }
            // for entry changes we need to also update our internal state. 
            // this catches also import csv changes etc. that don't update internal editor state directly.
            // Undo could include both entry and language changes so therefor 2 if checks rather then if / else. Due unity undoing 
            // by deserialising out dictionary get setup automatically through the OnAfterDeserialize call
            if (_entryReferenceList.Count != _targetLocalisationData.Entries.Count)
            {
                _currentTab = 0;
                SyncEntries();
                _targetChanged = true;
            }

            // Main tabs and display
            _currentTab = GUILayout.Toolbar(_currentTab, new[] { "Entries", "Languages", "Tools" });
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

            if (_targetChanged)
            {
                EditorUtility.SetDirty(_targetLocalisationData);
                GlobalLocalisation.Reload();
            }
        }

        #region Entries

        protected void DrawEntries()
        {
            _entriesHelpRect = EditorHelper.ShowHideableHelpBox("GameFramework.LocalisationEditorWindow.Entries", "Entries contain a set of unique tags that identify the text that you want to localise. You can further associate different translations with these tags for the different languages that you have setup.", _entriesHelpRect);

            // filter
            EditorGUILayout.BeginHorizontal();
            GUI.changed = false;
            EditorGUI.BeginChangeCheck();
            _filter = EditorGUILayout.TextField(_filter, GuiStyles.ToolbarSearchField, GUILayout.ExpandWidth(true));
            if (EditorGUI.EndChangeCheck())
            {
                foreach (var entryReference in _entryReferenceList)
                    entryReference.MatchesFilter = entryReference.Key.IndexOf(_filter, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            if (GUILayout.Button("", string.IsNullOrEmpty(_filter) ? GuiStyles.ToolbarSearchFieldCancelEmpty : GuiStyles.ToolbarSearchFieldCancel, GUILayout.ExpandWidth(false)))
            {
                _filter = "";
                GUIUtility.keyboardControl = 0;
                foreach (var entryReference in _entryReferenceList)
                    entryReference.MatchesFilter = true;
            }
            EditorGUILayout.EndHorizontal();

            if (_entryReferenceList.Count == 0)
            {
                GUILayout.Space(20);
                EditorGUILayout.LabelField("Create new localisation entries below.", GuiStyles.CenteredLabelStyle, GUILayout.ExpandWidth(true));
                GUILayout.Space(20);
            }
            else
            {
                _entriesScrollPosition = EditorGUILayout.BeginScrollView(_entriesScrollPosition, false, false, GUILayout.ExpandHeight(false));
                //Debug.Log(Event.current.type + ": " +_entriesScrollHeight + ", " + _entriesScrollPosition);

                // here we avoid using properties due to them being very slow!
                var indexForDeleting = -1;
                //var accumulativeHeightDrawn = 0;
                //var accumulativeSpace = 0;
                for (var i = 0; i < _entryReferenceList.Count; i++)
                {
                    var entryReference = _entryReferenceList[i];
                    if (entryReference.MatchesFilter)
                    {
                        //// if top is below viewport or bottom is above then this item is not visible. For repaint events we need to use the 
                        //// previously recorded display state to avoid changing the number of controls drawn.
                        ////if ((Event.current.type == EventType.Repaint && entryData.IsShown == false) ||
                        ////    (Event.current.type != EventType.Repaint &&
                        //if (((Event.current.type != EventType.Layout && entryReference.IsShown == false) ||
                        //    (Event.current.type == EventType.Layout &&
                        //     (accumulativeHeightDrawn > _entriesScrollPosition.y + _entriesScrollHeight ||
                        //      accumulativeHeightDrawn + entryReference.Height < _entriesScrollPosition.y))) && !entryReference.IsExpanded)
                        //{
                        //    accumulativeHeightDrawn += entryReference.Height;
                        //    accumulativeSpace += entryReference.Height;
                        //    entryReference.IsShown = false;
                        //}
                        //else
                        //{
                        //    // fill space from skipped.
                        //    if (accumulativeSpace > 0) GUILayout.Space(accumulativeSpace);
                        //    accumulativeSpace = 0;

                        // draw the displayed item
                        entryReference.IsShown = true;
                        var localisationEntry = _targetLocalisationData.GetEntry(_entryReferenceList[i].Key);
                        EditorGUILayout.BeginHorizontal();
                        EditorGUI.indentLevel++;
                        //var oldExpanded = entryReference.IsExpanded;
                        entryReference.IsExpanded = EditorGUILayout.Foldout(entryReference.IsExpanded, localisationEntry.Key);
                        // if we are not using a static default height (see below) then we need to flag changes to the expanded state, and in OnInspectorGUI
                        // call repaint within a 'if (Event.current.type == EventType.Repaint && _updatePostRepaint)' check. This so that if an entry 
                        // decreases in size we correctly draw items that might previously have been hidden.
                        //if (entryReference.IsExpanded != oldExpanded) // force repaint on changes incase new items become visable due to collapsing
                        //    _updatePostRepaint = true;
                        EditorGUI.indentLevel--;
                        if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(GuiStyles.RemoveButtonWidth)))
                        {
                            indexForDeleting = i;
                            break;
                        }
                        EditorGUILayout.EndHorizontal();
                        //if (Event.current.type == EventType.Repaint)
                        //entryData.Height = (int)GUILayoutUtility.GetLastRect().height; // only works on repaint.
                        entryReference.Height = _entriesDefaultRowHeight;

                        // handle expended status
                        if (entryReference.IsExpanded)
                        {
                            EditorGUILayout.BeginVertical();
                            EditorGUI.indentLevel++;
                            for (var li = 0; li < localisationEntry.Languages.Length; li++)
                            {
                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.LabelField(_targetLocalisationData.Languages[li].Name,
                                    GUILayout.Width(100));
                                EditorGUI.BeginChangeCheck();

                                // Set the internal name of the textfield so we can focus
                                if (_indexForFocus == i)
                                    GUI.SetNextControlName("FocusTextField");

                                var lang = EditorGUILayout.TextArea(localisationEntry.Languages[li], GuiStyles.WordWrapStyle, GUILayout.Width(Screen.width - 100 - 60 - 50));
                                if (EditorGUI.EndChangeCheck())
                                {
                                    Undo.RecordObject(_targetLocalisationData, "Edit Localisation Entry");
                                    localisationEntry.Languages[li] = lang;
                                    _targetChanged = true;
                                }

                                // And focus if needs be
                                if (_indexForFocus == i)
                                {
                                    EditorGUI.FocusTextInControl("FocusTextField");
                                    _indexForFocus = -1;
                                }

                                // TODO: Move to a callback so we don't block the UI
                                if (li > 0 && GUILayout.Button("Translate", EditorStyles.miniButton, GUILayout.Width(60)))
                                {
                                    var sourceCode = _targetLocalisationData.Languages[0].Code;
                                    if (!string.IsNullOrEmpty(sourceCode))
                                    {
                                        var targetCode = _targetLocalisationData.Languages[li].Code;
                                        if (!string.IsNullOrEmpty(targetCode))
                                        {
                                            var sourceText = localisationEntry.Languages[0];
#if UNITY_2017_3_OR_NEWER
                                            var escapedSourceText = UnityEngine.Networking.UnityWebRequest.EscapeURL(sourceText);
#else
                                            var escapedSourceText = WWW.EscapeURL(sourceText);
#endif
                                            string url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl="
                                                         + sourceCode + "&tl=" + targetCode + "&dt=t&q=" + escapedSourceText;
                                            var wwwForm = new WWWForm();
                                            wwwForm.AddField("username", "");
                                            //var headers = new Dictionary<string, string>();
                                            wwwForm.headers.Add("content-type", "application/json");
#if UNITY_2017_2_OR_NEWER
                                            var www = UnityEngine.Networking.UnityWebRequest.Post(url, wwwForm);
                                            www.SendWebRequest();
#else
                                            var www = new WWW(url, wwwForm);
#endif

                                            while (!www.isDone) ;
#if UNITY_2017_2_OR_NEWER
                                            if (www.isNetworkError || www.isHttpError)
#else
                                            if (www.error != null)
#endif
                                            {
                                                Debug.LogError(www.error);
                                            }
                                            else
                                            {
#if UNITY_2017_2_OR_NEWER
                                                var text = www.downloadHandler.text;
#else
                                                var text = www.text;
#endif
                                                Debug.Log("Google Translate Response:" + text);
                                                var json = ObjectModel.Internal.SimpleJSON.JSONNode.Parse(text);
                                                if (json != null)
                                                {
                                                    var translation = "";
                                                    for (var lines = 0; lines < json[0].Count; lines++)
                                                    {
                                                        // Dig through and take apart the text to get to the good stuff.
                                                        var translatedText = json[0][lines][0].ToString();
                                                        if (translatedText.Length > 2)
                                                            translatedText = translatedText.Substring(1,
                                                                translatedText.Length - 2);
                                                        if (translation.Length > 0)
                                                            translation += "\n";
                                                        translation +=
                                                            translatedText.Replace("\\n", "").Replace("\\\"", "\"");
                                                    }
                                                    Undo.RecordObject(_targetLocalisationData, "Edit Localisation Entry");
                                                    localisationEntry.Languages[li] = translation;
                                                    _targetChanged = true;
                                                }
                                                else
                                                    Debug.LogError("Unable to parse json response");
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
                            //if (Event.current.type == EventType.Repaint)
                            //    entryReference.Height += (int)GUILayoutUtility.GetLastRect().height; // only works on repaint.
                            //}

                            //accumulativeHeightDrawn += entryReference.Height;
                            // Debug.Log(entryData.LocalisationEntry.Key + ", " + accumulativeHeightDrawn);
                        }
                    }
                }
                //if (accumulativeSpace > 0) GUILayout.Space(accumulativeSpace);
                //Debug.Log(accumulativeHeightDrawn);
                EditorGUILayout.EndScrollView();

                //if (Event.current.type == EventType.Repaint)
                //    _entriesScrollHeight = GUILayoutUtility.GetLastRect().height; // only works on repaint.

                // delay deleting to avoid editor issues.
                if (indexForDeleting != -1)
                {
                    var keyToDelete = _entryReferenceList[indexForDeleting].Key;
                    if (EditorUtility.DisplayDialog("Delete Entry?", String.Format("Are you sure you want to delete the entry '{0}?'", keyToDelete), "Yes", "No"))
                    {
                        Undo.RecordObject(_targetLocalisationData, "Delete Localisation Entry");
                        _targetLocalisationData.RemoveEntry(keyToDelete);
                        _entryReferenceList.RemoveAt(indexForDeleting);
                        _targetChanged = true;
                    }
                }
            }

            // add functionality
            EditorGUILayout.BeginHorizontal();
            bool entryAddPressed = false;
            if (GUI.GetNameOfFocusedControl() == "EntryAdd" && Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
            {
                entryAddPressed = true;
                Event.current.Use();
            }
            GUI.SetNextControlName("EntryAdd");
            _newKey = EditorGUILayout.TextField("", _newKey, GUILayout.ExpandWidth(true));
            var isValidEntry = !string.IsNullOrEmpty(_newKey) && !_targetLocalisationData.ContainsEntry(_newKey);
            GUI.enabled = isValidEntry;
            if (entryAddPressed || GUILayout.Button(new GUIContent("Add", "Add the specified entry to the list"), EditorStyles.miniButton, GUILayout.Width(100)))
            {
                if (isValidEntry)
                {
                    Undo.RecordObject(_targetLocalisationData, "Add Localisation Entry");
                    _targetLocalisationData.AddEntry(_newKey);
                    _targetChanged = true;

                    _languagesCount = _targetLocalisationData.Languages.Count; // set incase a first language was autocreated.

                    var insertIndex = 0;
                    var scrollOffset = 0;
                    for (; insertIndex < _entryReferenceList.Count; insertIndex++)
                    {
                        if (_newKey.CompareTo(_entryReferenceList[insertIndex].Key) < 0)
                            break;
                        else
                            scrollOffset += _entryReferenceList[insertIndex].Height;
                    }
                    _entryReferenceList.Insert(insertIndex, new EntryReference()
                    {
                        Key = _newKey,
                        MatchesFilter = _newKey.IndexOf(_filter + "", StringComparison.OrdinalIgnoreCase) >= 0,
                        IsExpanded = true
                    });
                    _entriesScrollPosition.y = scrollOffset;

                    var lastDot = _newKey.LastIndexOf(".", StringComparison.Ordinal);
                    if (lastDot == -1)
                        _newKey = "";
                    else
                        _newKey = _newKey.Substring(0, lastDot + 1);
                }
                GUI.FocusControl("EntryAdd");
            }
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUI.enabled = _entryReferenceList.Count > 0;
            if (GUILayout.Button(new GUIContent("Delete All", "Delete All entries"), EditorStyles.miniButton, GUILayout.Width(100)))
            {
                Undo.RecordObject(_targetLocalisationData, "Delete Localisation Entry");
                _targetLocalisationData.ClearEntries();
                _entryReferenceList.Clear();
                _targetChanged = true;
            }
            GUI.enabled = true;
        }

        private void SyncEntries()
        {
            if (_entryReferenceList.Count != _targetLocalisationData.Entries.Count)
            {
                // find a match in target or add.
                foreach (var entry in _targetLocalisationData.Entries)
                {
                    var match = false;
                    foreach (var entryReference in _entryReferenceList)
                        if (entry.Key.Equals(entryReference.Key))
                            match = true;
                    if (!match) _entryReferenceList.Add(new EntryReference() { Height = _entriesDefaultRowHeight, Key = entry.Key });
                }
                // find a match in target or remove.
                for (int i = _entryReferenceList.Count - 1; i >= 0; i--)
                {
                    var entryReference = _entryReferenceList[i];
                    var match = false;
                    foreach (var entry in _targetLocalisationData.Entries)
                        if (entry.Key.Equals(entryReference.Key))
                            match = true;
                    if (!match) _entryReferenceList.RemoveAt(i);
                }
                _entryReferenceList.Sort((x, y) => x.Key.CompareTo(y.Key));
            }
            var internalStateError = _targetLocalisationData.InternalVerifyState(); // should not happen!
            if (internalStateError != null)
                Debug.Log("Internal state error - please report including the actions you just took Thanks!" + _targetLocalisationData.InternalVerifyState());
        }


        [Serializable]
        public class EntryReference
        {
            public bool IsExpanded;
            public bool IsShown;
            public bool MatchesFilter = true;
            public int Height;
            public string Key; // a reference that can be used for looking up items from the original collection.
        }

#endregion Entries

#region Languages

        protected void DrawLanguages()
        {
            _languagesHelpRect = EditorHelper.ShowHideableHelpBox("GameFramework.LocalisationEditorWindow.Languages", "Here you can specify the languages for which you will provide localised values.\n\nYou should enter the language name and also an optional ISO-639-1 code for use with google translate if you want to perform automatic translations. For convenience Unity supported languages are available from the dropdown button at the bottom right.", _languagesHelpRect);
            EditorGUILayout.BeginVertical("Box");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Name", GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("Code", GUILayout.Width(60 + GuiStyles.RemoveButtonWidth + 6));
            //EditorGUILayout.LabelField("", GUILayout.Width(GuiStyles.RemoveButtonWidth));
            EditorGUILayout.EndHorizontal();

            serializedObject.Update();

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

            serializedObject.ApplyModifiedProperties();

            // add functionality
            EditorGUILayout.BeginHorizontal();
            _newLanguage = EditorGUILayout.TextField("", _newLanguage, GUILayout.ExpandWidth(true));
            if (string.IsNullOrEmpty(_newLanguage) || _targetLocalisationData.ContainsLanguage(_newLanguage))
                GUI.enabled = false;
            if (GUILayout.Button(new GUIContent("Add", "Add the specified language to the list"), EditorStyles.miniButton, GUILayout.Width(100)))
            {
                Undo.RecordObject(_targetLocalisationData, "Add Language");
                _targetLocalisationData.AddLanguage(_newLanguage);
                _targetChanged = true;
                _newLanguage = "";
            }
            GUI.enabled = true;

            //if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Plus More", "Add to list"), GUILayout.Width(25)))
            if (GUILayout.Button(new GUIContent("+", "Add a new language to the list"), EditorStyles.miniButton, GUILayout.Width(20)))
            {
                var menu = new GenericMenu();
                foreach (var languageDefinition in Languages.LanguageDefinitions)
                {
                    if (!_targetLocalisationData.ContainsLanguage(languageDefinition.Name))
                        menu.AddItem(new GUIContent(languageDefinition.Name + " (" + languageDefinition.Code + ")"), false, AddLanguage, languageDefinition.Name);
                }
                menu.ShowAsContext();
            }
            EditorGUILayout.EndHorizontal();

            // delay deleting to avoid editor issues.
            if (languageForDeleting != null &&
                EditorUtility.DisplayDialog("Delete Language?", string.Format("Are you sure you want to delete the language '{0}'?\n\nDeleting this language will also delete all translations for this language from the list of entries.", languageForDeleting), "Yes", "No"))
            {
                Undo.RecordObject(_targetLocalisationData, "Delete Language");
                _targetLocalisationData.RemoveLanguage(languageForDeleting);
                _targetChanged = true;
            }
        }

        void AddLanguage(object languageObject)
        {
            var language = (string)languageObject;
            Undo.RecordObject(_targetLocalisationData, "Add Language");
            _targetLocalisationData.AddLanguage(language, Languages.LanguageDefinitionsDictionary[language].Code);
            _targetChanged = true;
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
                    var importedLocalisationData = LocalisationData.LoadCsv(_importExportFilename);
                    if (importedLocalisationData != null)
                    {
                        Undo.RecordObject(_targetLocalisationData, "Import Localisation Csv");
                        _targetLocalisationData.Merge(importedLocalisationData);
                        _targetChanged = true;
                        EditorUtility.DisplayDialog("Localisation Import", string.Format("Import Complete!\n\nImported {0} languages and {1} entries.", importedLocalisationData.Languages.Count, importedLocalisationData.Entries.Count), "Ok");
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Localisation Import", "Import failed!\n\nSee the console window for further details.", "Ok");
                    }
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

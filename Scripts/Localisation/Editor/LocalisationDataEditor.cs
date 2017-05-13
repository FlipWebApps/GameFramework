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
using System.Text;
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

        SerializedProperty _entriesProperty;
        SerializedProperty _languagesProperty;

        Rect _mainHelpRect;
        int _currentTab;

        Rect _entriesHelpRect;
        string _newEntry;

        Rect _languagesHelpRect;
        string _newLanguage;

        protected virtual void OnEnable()
        {
            _targetLocalisationData = target as LocalisationData;

            _entriesProperty = serializedObject.FindProperty("_localisationEntries");
            _languagesProperty = serializedObject.FindProperty("_languages");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawGUI();
            serializedObject.ApplyModifiedProperties();
        }

        protected void DrawGUI()
        {
            _mainHelpRect = EditorHelper.ShowHideableHelpBox("GameFramework.LocalisationEditorWindow.Main", "This localisation file is where you can define localised text in different languages.", _mainHelpRect);

            _currentTab = GUILayout.Toolbar(_currentTab, new string[] { "Entries", "Languages" });
            switch (_currentTab)
            {
                case 0:
                    DrawEntries();
                    break;
                case 1:
                    DrawLanguages();
                    break;
            }
        }

        protected void DrawEntries() {
            _entriesHelpRect = EditorHelper.ShowHideableHelpBox("GameFramework.LocalisationEditorWindow.Entries", "Entries contain a set of unique tags that identify the text that you want to localise. You can further associate different translations with these tags for the different languages that you have setup.", _entriesHelpRect);
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Basic Properties", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_entriesProperty, true);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("Box");
            string entryForDeleting = null;
            for (var i = 0; i < _entriesProperty.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                var entryProperty = _entriesProperty.GetArrayElementAtIndex(i);
                var keyProperty = entryProperty.FindPropertyRelative("Key");
                EditorGUILayout.PropertyField(keyProperty, GUIContent.none, GUILayout.ExpandWidth(true));
                if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(GuiStyles.RemoveButtonWidth)))
                {
                    entryForDeleting = keyProperty.stringValue;
                    break;
                }
                EditorGUILayout.EndHorizontal();

                var languagesProperty = entryProperty.FindPropertyRelative("Languages");
                for (var li = 0; li < languagesProperty.arraySize; li++)
                {
                    var languageProperty = languagesProperty.GetArrayElementAtIndex(li);
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(_targetLocalisationData.GetLanguages()[li].Name, GUILayout.Width(100));
                    EditorGUILayout.PropertyField(languageProperty, GUIContent.none, GUILayout.ExpandWidth(true));
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();


            // add functionality
            EditorGUILayout.BeginHorizontal();
            _newEntry = EditorGUILayout.TextField("", _newEntry, GUILayout.ExpandWidth(true));
            if (string.IsNullOrEmpty(_newEntry) || _targetLocalisationData.ContainsEntry(_newEntry))
                GUI.enabled = false;
            if (GUILayout.Button(new GUIContent("Add", "Add the specified entry to the list"), EditorStyles.miniButton, GUILayout.Width(100)))
            {
                serializedObject.ApplyModifiedProperties();
                _targetLocalisationData.AddEntry(_newEntry);
                serializedObject.Update();
                var lastDot = _newEntry.LastIndexOf(".");
                if (lastDot == -1)
                    _newEntry = ""; 
                else _newEntry = _newEntry.Substring(0, lastDot + 1);
            }
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();

            // delay deleting to avoid editor issues.
            if (entryForDeleting != null)
            {
                //TODO: Show a warning first!
                serializedObject.ApplyModifiedProperties();
                _targetLocalisationData.RemoveEntry(entryForDeleting);
                serializedObject.Update();
            }

        }

        protected void DrawLanguages()
        {
            _languagesHelpRect = EditorHelper.ShowHideableHelpBox("GameFramework.LocalisationEditorWindow.Languages", "Here you can specify the languages for which you will provide localised values.", _languagesHelpRect);
            EditorGUILayout.BeginVertical("Box");

            string languageForDeleting = null;
            for (var i = 0; i < _languagesProperty.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();

                var languageProperty = _languagesProperty.GetArrayElementAtIndex(i);
                var nameProperty = languageProperty.FindPropertyRelative("Name");
                EditorGUILayout.PropertyField(nameProperty, GUIContent.none, GUILayout.ExpandWidth(true));
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

            if (GUILayout.Button(new GUIContent("+", "Add a new language to the list"), EditorStyles.miniButton, GUILayout.Width(20)))
            {
                serializedObject.ApplyModifiedProperties();
                var menu = new GenericMenu();
                var systemLanguages = Enum.GetNames(typeof(SystemLanguage));
                for (var i = 0; i < systemLanguages.Length; i++)
                {
                    if (!_targetLocalisationData.ContainsLanguage(systemLanguages[i]))
                        menu.AddItem(new GUIContent(systemLanguages[i]), false, AddLanguage, systemLanguages[i]);
                }
                menu.ShowAsContext();
            }
            EditorGUILayout.EndHorizontal();

            // delay deleting to avoid editor issues.
            if (languageForDeleting != null)
            {
                //TODO: Show a warning first!
                serializedObject.ApplyModifiedProperties();
                _targetLocalisationData.RemoveLanguage(languageForDeleting);
                serializedObject.Update();
            }
        }

        void AddLanguage(object languageObject)
        {
            var language = languageObject as string;
            _targetLocalisationData.AddLanguage(language);
            serializedObject.Update();
        }
    }
}

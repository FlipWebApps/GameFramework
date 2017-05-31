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
using GameFramework.Localisation.Components;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace GameFramework.Localisation.Editor
{
    [CustomEditor(typeof(LocalisationConfiguration))]
    public class LocalisationConfigurationEditor : UnityEditor.Editor
    {
        ReorderableList _localisationDataList;
        ReorderableList _supportedLanguagesList;

        SerializedProperty _setupModeProperty;
        SerializedProperty _specifiedLocalisationDataProperty;
        SerializedProperty _supportedLanguagesProperty;

        Rect _mainHelpRect;

        void OnEnable()
        {
            _setupModeProperty = serializedObject.FindProperty("_setupMode");
            _specifiedLocalisationDataProperty = serializedObject.FindProperty("_specifiedLocalisationData");
            _supportedLanguagesProperty = serializedObject.FindProperty("_supportedLanguages");

            _localisationDataList = new ReorderableList(serializedObject, _specifiedLocalisationDataProperty, true, true, true, true);
            _localisationDataList.drawHeaderCallback = (Rect rect) => {
                EditorGUI.LabelField(rect, "Localisation Files");
            };
            _localisationDataList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) => {
                    var element = _localisationDataList.serializedProperty.GetArrayElementAtIndex(index);
                    rect.y += 2;
                    EditorGUI.PropertyField(
                        new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                        element, GUIContent.none);
                };

            _supportedLanguagesList = new ReorderableList(serializedObject, _supportedLanguagesProperty, true, true, true, true);
            _supportedLanguagesList.drawHeaderCallback = (Rect rect) => {
                EditorGUI.LabelField(rect, "Supported Languages");
            };
            _supportedLanguagesList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) => {
                    var element = _supportedLanguagesList.serializedProperty.GetArrayElementAtIndex(index);
                    rect.y += 2;
                    EditorGUI.PropertyField(
                        new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                        element, GUIContent.none);
                };

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawLocalisation();

            serializedObject.ApplyModifiedProperties();


        }


        void DrawLocalisation()
        {
            _mainHelpRect = EditorHelper.ShowHideableHelpBox("GameFramework.LocalisationEditorWindow.Configuration", "Welcome to the new Game Framework localisation system!\n\nYou can add a Localisation Configuration to your Resources folder (named LocalisationConfiguration) to control the default setup of the localisation system.\n\nIf you experience any problems, can help with new translations, or have improvement suggestions then please get in contact. Your support is appreciated.", _mainHelpRect);

            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.PropertyField(_setupModeProperty);
            if (_setupModeProperty.enumValueIndex == 1)
            {
                _localisationDataList.DoLayoutList();
            }
            EditorGUILayout.EndVertical();

            _supportedLanguagesList.DoLayoutList();
        }
    }
}
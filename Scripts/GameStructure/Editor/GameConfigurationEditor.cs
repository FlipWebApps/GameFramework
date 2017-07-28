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
using GameFramework.GameStructure.Game.ObjectModel;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace GameFramework.GameStructure.Editor
{
    [CustomEditor(typeof(GameConfiguration))]
    public class GameConfigurationEditor : UnityEditor.Editor
    {
        SerializedProperty _characterCounterConfigurationEntriesProperty;

        SerializedProperty _levelCounterConfigurationEntriesProperty;

        SerializedProperty _playerCounterConfigurationEntriesProperty;

        SerializedProperty _worldCounterConfigurationEntriesProperty;

        Rect _mainHelpRect;
        int _currentTab;

        Rect _countersHelpRect;

        void OnEnable()
        {
            _characterCounterConfigurationEntriesProperty = serializedObject.FindProperty("_characterCounterConfigurationEntries");

            _levelCounterConfigurationEntriesProperty = serializedObject.FindProperty("_levelCounterConfigurationEntries");

            _playerCounterConfigurationEntriesProperty = serializedObject.FindProperty("_playerCounterConfigurationEntries");

            _worldCounterConfigurationEntriesProperty = serializedObject.FindProperty("_worldCounterConfigurationEntries");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            _mainHelpRect = EditorHelper.ShowHideableHelpBox("GameFramework.GameStructure.GameConfigurationEditorWindow", "Add a Game Configuration to your Resources folder (named GameConfiguration) to control fixed aspects of your game.\n\nNOTE: These are seperate from the GameManager component so that they can be loaded and used independently.\n\nIf you experience any problems or have improvement suggestions then please get in contact. Your support is appreciated.", _mainHelpRect);

            // Main tabs and display
            _currentTab = GUILayout.Toolbar(_currentTab, new string[] { "Characters", "Levels", "Players", "Worlds" });
            switch (_currentTab)
            {
                //case 0:
                //    DrawGeneral();
                //    break;
                case 0:
                    DrawCharacters();
                    break;
                case 1:
                    DrawLevels();
                    break;
                case 2:
                    DrawPlayers();
                    break;
                case 3:
                    DrawWorlds();
                    break;
            }

            serializedObject.ApplyModifiedProperties();

            // reload singleton so changes are available to other components.
            if (GUI.changed)
            {
                GameConfiguration.ReloadSingletonGameConfiguration();
            }
        }


        //void DrawGeneral() 
        //{
        //    _countersHelpRect = EditorHelper.ShowHideableHelpBox("GameFramework.GameStructure.GameConfigurationEditorWindow.Global", "By default GameItems such as Player, Level, etc. have support for scores and coins.\n\nYou can add additional 'Counter' counters here that you might need in your game e.g. Gems, ... These will then be available for use in all GameItems from code or within the components that reference a counter such as 'ShowCounter'.", _countersHelpRect);

        //    EditorGUILayout.BeginVertical("Box");
        //    EditorGUI.indentLevel++;
        //    EditorGUILayout.PropertyField(_globalCounterConfigurationEntriesProperty, new GUIContent("Custom Counters"), true);
        //    EditorGUI.indentLevel--;
        //    EditorGUILayout.EndVertical();
        //}


        void DrawCharacters()
        {
            DrawCounters(_characterCounterConfigurationEntriesProperty);
        }


        void DrawLevels()
        {
            DrawCounters(_levelCounterConfigurationEntriesProperty);
        }


        void DrawPlayers()
        {
            DrawCounters(_playerCounterConfigurationEntriesProperty);
        }


        void DrawWorlds()
        {
            DrawCounters(_worldCounterConfigurationEntriesProperty);
        }


        private void DrawCounters(SerializedProperty arrayProperty)
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField(new GUIContent("Counters", "By default GameItems such as Player, Level, etc. have support for scores and coins.\n\nYou can add additional 'Counter' counters here that you might need in your game e.g. Gems, ... These will then be available for use in all GameItems from code or within the components that reference a counter such as 'ShowCounter'."), EditorStyles.boldLabel);
            _countersHelpRect = EditorHelper.ShowHideableHelpBox("GameFramework.GameStructure.GameConfigurationEditorWindow.Counter", "By default GameItems such as Player, Level, etc. have support for scores and coins.\n\nYou can add additional 'Counter' counters here that you might need in your game e.g. Gems, ... These will then be available for use in all GameItems from code or within the components that reference a counter such as 'ShowCounter'.", _countersHelpRect);

            if (arrayProperty.arraySize > 0)
            {
                for (var i = 0; i < arrayProperty.arraySize; i++)
                {
                    var elementProperty = arrayProperty.GetArrayElementAtIndex(i);
                    var keyProperty = elementProperty.FindPropertyRelative("_key");
                    var minimumProperty = elementProperty.FindPropertyRelative("_minimum");
                    var persistChangesProperty = elementProperty.FindPropertyRelative("_persistChanges");
                    var deleted = false;
                    var isSystemEntry = keyProperty.stringValue.Equals("Score") || keyProperty.stringValue.Equals("Coins");
                    EditorGUILayout.BeginHorizontal(GuiStyles.BoxLightStyle);
                    GUILayout.Space(15f);
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.BeginHorizontal();
                    var name = string.IsNullOrEmpty(keyProperty.stringValue) ? "<missing name>" :
                        keyProperty.stringValue + (isSystemEntry ? " (Built in)" : "");
                    elementProperty.isExpanded = EditorGUILayout.Foldout(elementProperty.isExpanded, name);
                    if (!isSystemEntry)
                        if (GUILayout.Button("X", GuiStyles.BorderlessButtonStyle, GUILayout.Width(12), GUILayout.Height(12)) &&
                            EditorUtility.DisplayDialog("Remove Entry?", "Are you sure you want to remove this entry?", "Yes",
                                "No"))
                        {
                            arrayProperty.DeleteArrayElementAtIndex(i);
                            deleted = true;
                        }
                    EditorGUILayout.EndHorizontal();

                    if (!deleted && elementProperty.isExpanded)
                    {
                        EditorGUILayout.PropertyField(keyProperty);

                        GUILayout.BeginHorizontal();
                        var minimumToggle = EditorGUILayout.Toggle(new GUIContent(minimumProperty.displayName, minimumProperty.tooltip), minimumProperty.intValue != int.MinValue);
                        if (minimumToggle && minimumProperty.intValue == int.MinValue)
                            minimumProperty.intValue = 0;
                        else if (!minimumToggle && minimumProperty.intValue != int.MinValue)
                            minimumProperty.intValue = int.MinValue;
                        if (minimumToggle)
                            EditorGUILayout.PropertyField(minimumProperty, GUIContent.none);
                        GUILayout.EndHorizontal();

                        //GUILayout.BeginHorizontal();
                        //var maximumToggle = EditorGUILayout.Toggle(new GUIContent(minimumProperty.displayName, minimumProperty.tooltip), minimumProperty.intValue != int.MinValue);
                        //if (minimumToggle && minimumProperty.intValue == int.MinValue)
                        //    minimumProperty.intValue = 0;
                        //else if (!minimumToggle && minimumProperty.intValue != int.MinValue)
                        //    minimumProperty.intValue = int.MinValue;
                        //if (minimumToggle)
                        //    EditorGUILayout.PropertyField(minimumProperty, GUIContent.none);
                        //GUILayout.EndHorizontal();

                        EditorGUILayout.PropertyField(persistChangesProperty);
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();

                    GUILayout.Space(2f);
                }
            }

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent("Add Counter"), GUILayout.ExpandWidth(false)))
            {
                AddNewCounter(arrayProperty);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(2f);
            EditorGUILayout.EndVertical();
        }


        void AddNewCounter(SerializedProperty arrayProperty)
        {
            arrayProperty.arraySize++;
            var newElement =
                arrayProperty.GetArrayElementAtIndex(arrayProperty.arraySize - 1);
            newElement.isExpanded = true;
            var keyProperty = newElement.FindPropertyRelative("_key");
            keyProperty.stringValue = null;
            var minimumProperty = newElement.FindPropertyRelative("_minimum");
            minimumProperty.intValue = 0;
            var persistChangesProperty = newElement.FindPropertyRelative("_persistChanges");
            persistChangesProperty.boolValue = false;
        }
    }
}
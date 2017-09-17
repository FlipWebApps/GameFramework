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
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GameFramework.GameStructure.Game.Editor
{
    [CustomEditor(typeof(GameConfiguration))]
    public class GameConfigurationEditor : UnityEditor.Editor
    {
        SerializedProperty _characterCounterConfigurationProperty;

        SerializedProperty _levelCounterConfigurationProperty;

        SerializedProperty _playerCounterConfigurationProperty;

        SerializedProperty _worldCounterConfigurationProperty;

        Rect _mainHelpRect;
        int _currentTab;

        Rect _countersHelpRect;
        readonly string[] _counterTypeNames = new string[] { "Integer", "Float" };

        void OnEnable()
        {
            _characterCounterConfigurationProperty = serializedObject.FindProperty("_characterCounterConfiguration");

            _levelCounterConfigurationProperty = serializedObject.FindProperty("_levelCounterConfiguration");

            _playerCounterConfigurationProperty = serializedObject.FindProperty("_playerCounterConfiguration");

            _worldCounterConfigurationProperty = serializedObject.FindProperty("_worldCounterConfiguration");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            _mainHelpRect = EditorHelper.ShowHideableHelpBox("GameFramework.GameStructure.GameConfigurationEditorWindow", "Add a Game Configuration to your Resources folder (named GameConfiguration) to control fixed aspects of your game.\n\nNOTE: These are seperate from the GameManager component so that they can be loaded and used independently.\n\nIf you experience any problems or have improvement suggestions then please get in contact. Your support is appreciated.", _mainHelpRect);

            // Main tabs and display
            _currentTab = GUILayout.Toolbar(_currentTab, new[] { "Characters", "Levels", "Players", "Worlds" });
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
        //    EditorGUILayout.PropertyField(_globalCounterConfigurationProperty, new GUIContent("Custom Counters"), true);
        //    EditorGUI.indentLevel--;
        //    EditorGUILayout.EndVertical();
        //}


        void DrawCharacters()
        {
            DrawCounters(_characterCounterConfigurationProperty, new[] { "Score", "Coins" });
        }


        void DrawLevels()
        {
            DrawCounters(_levelCounterConfigurationProperty, new[] { "Score", "Coins", "Progress" });
        }


        void DrawPlayers()
        {
            DrawCounters(_playerCounterConfigurationProperty, new[] { "Score", "Coins", "Lives", "Health" });
        }


        void DrawWorlds()
        {
            DrawCounters(_worldCounterConfigurationProperty, new[] { "Score", "Coins" });
        }


        private void DrawCounters(SerializedProperty arrayProperty, string[] systemCounters)
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField(new GUIContent("Counters", "By default GameItems such as Player, Level, etc. have support for scores and coins.\n\nYou can add additional 'Counter' counters here that you might need in your game e.g. Gems, ... These will then be available for use in all GameItems from code or within the components that reference a counter such as 'ShowCounter'."), EditorStyles.boldLabel);
            _countersHelpRect = EditorHelper.ShowHideableHelpBox("GameFramework.GameStructure.GameConfigurationEditorWindow.Counter", "By default GameItems such as Player, Level, etc. have support for scores and coins.\n\nYou can add additional 'Counter' counters here that you might need in your game e.g. Gems, ... These will then be available for use in all GameItems from code or within the components that reference a counter such as 'ShowCounter'.", _countersHelpRect);

            if (arrayProperty.arraySize > 0)
            {
                for (var i = 0; i < arrayProperty.arraySize; i++)
                {
                    var elementProperty = arrayProperty.GetArrayElementAtIndex(i);
                    var nameProperty = elementProperty.FindPropertyRelative("_name");
                    var counterTypeProperty = elementProperty.FindPropertyRelative("_counterType");
                    var saveProperty = elementProperty.FindPropertyRelative("_save");
                    var saveBestProperty = elementProperty.FindPropertyRelative("_saveBest");
                    var deleted = false;
                    var isSystemEntry = systemCounters.Contains(nameProperty.stringValue);
                    EditorGUILayout.BeginHorizontal(GuiStyles.BoxLightStyle);
                    GUILayout.Space(15f);
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.BeginHorizontal();
                    var title = string.IsNullOrEmpty(nameProperty.stringValue) ? "<missing name>" :
                        nameProperty.stringValue + (isSystemEntry ? " (Built in)" : "");
                    elementProperty.isExpanded = EditorGUILayout.Foldout(elementProperty.isExpanded, title);
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
                        EditorGUILayout.PropertyField(nameProperty);

                        counterTypeProperty.enumValueIndex = EditorGUILayout.Popup("Type", counterTypeProperty.enumValueIndex, _counterTypeNames);

                        // display appropriate minimum / maximum field using type min / max as bounds if we are not restricting the limits
                        if (counterTypeProperty.enumValueIndex == 0)
                        {
                            var defaultProperty = elementProperty.FindPropertyRelative("_intDefault");
                            EditorGUILayout.PropertyField(defaultProperty, new GUIContent("Default", defaultProperty.tooltip));

                            var minimumProperty = elementProperty.FindPropertyRelative("_intMinimum");
                            GUILayout.BeginHorizontal();
                            var minimumToggle = EditorGUILayout.Toggle(new GUIContent("Minimum", "The lowest value that this counter can take."), minimumProperty.intValue != int.MinValue);
                            var isMinimumValue = minimumProperty.intValue == int.MinValue;
                            if (minimumToggle && isMinimumValue)
                                minimumProperty.intValue = 0;
                            else if (!minimumToggle && !isMinimumValue)
                                minimumProperty.intValue = int.MinValue;
                            if (minimumToggle)
                                EditorGUILayout.PropertyField(minimumProperty, GUIContent.none);
                            GUILayout.EndHorizontal();

                            var maximumProperty = elementProperty.FindPropertyRelative("_intMaximum");
                            GUILayout.BeginHorizontal();
                            var maximumToggle = EditorGUILayout.Toggle(new GUIContent("Maximum", "The highest value that this counter can take."), maximumProperty.intValue != int.MaxValue);
                            var isMaximumValue = maximumProperty.intValue == int.MaxValue;
                            if (maximumToggle && isMaximumValue)
                                maximumProperty.intValue = 0;
                            else if (!maximumToggle && !isMaximumValue)
                                maximumProperty.intValue = int.MaxValue;
                            if (maximumToggle)
                                EditorGUILayout.PropertyField(maximumProperty, GUIContent.none);
                            GUILayout.EndHorizontal();
                        }
                        else if (counterTypeProperty.enumValueIndex == 1)
                        {
                            var defaultProperty = elementProperty.FindPropertyRelative("_floatDefault");
                            EditorGUILayout.PropertyField(defaultProperty, new GUIContent("Default", defaultProperty.tooltip));

                            var minimumProperty = elementProperty.FindPropertyRelative("_floatMinimum");
                            GUILayout.BeginHorizontal();
                            var minimumToggle = EditorGUILayout.Toggle(new GUIContent("Minimum", "The lowest value that this counter can take."), !Mathf.Approximately(minimumProperty.floatValue, float.MinValue));
                            var isMinimumValue = Mathf.Approximately(minimumProperty.floatValue, float.MinValue);
                            if (minimumToggle && isMinimumValue)
                                minimumProperty.floatValue = 0;
                            else if (!minimumToggle && !isMinimumValue)
                                minimumProperty.floatValue = float.MinValue;
                            if (minimumToggle)
                                EditorGUILayout.PropertyField(minimumProperty, GUIContent.none);
                            GUILayout.EndHorizontal();

                            var maximumProperty = elementProperty.FindPropertyRelative("_floatMaximum");
                            GUILayout.BeginHorizontal();
                            var maximumToggle = EditorGUILayout.Toggle(new GUIContent("Maximum", "The highest value that this counter can take."), !Mathf.Approximately(maximumProperty.floatValue, float.MaxValue));
                            var isMaximumValue = Mathf.Approximately(maximumProperty.floatValue, float.MaxValue);
                            if (maximumToggle && isMaximumValue)
                                maximumProperty.floatValue = 0;
                            else if (!maximumToggle && !isMaximumValue)
                                maximumProperty.floatValue = float.MaxValue;
                            if (maximumToggle)
                                EditorGUILayout.PropertyField(maximumProperty, GUIContent.none);
                            GUILayout.EndHorizontal();
                        }

                        EditorGUILayout.PropertyField(saveProperty);
                        EditorGUILayout.PropertyField(saveBestProperty);
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
            newElement.FindPropertyRelative("_name").stringValue = null;
            newElement.FindPropertyRelative("_counterType").enumValueIndex = 0;
            newElement.FindPropertyRelative("_intMinimum").intValue = 0;
            newElement.FindPropertyRelative("_floatMinimum").floatValue = 0;
            newElement.FindPropertyRelative("_intMaximum").intValue = int.MaxValue;
            newElement.FindPropertyRelative("_floatMaximum").floatValue = float.MaxValue;
            newElement.FindPropertyRelative("_save").enumValueIndex = 0;
            newElement.FindPropertyRelative("_saveBest").enumValueIndex = 1;
        }
    }
}
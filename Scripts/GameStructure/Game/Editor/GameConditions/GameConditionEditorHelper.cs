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
using GameFramework.GameStructure.Game.Editor.GameConditions.Common;
using GameFramework.GameStructure.Game.ObjectModel;
using GameFramework.GameStructure.Game.ObjectModel.Abstract;
using GameFramework.Helper;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameFramework.GameStructure.Game
{
    /// <summary>
    /// Helper class for GameAction Editors
    /// </summary>
    public class GameConditionEditorHelper
    {
        const float RemoveButtonWidth = 30f;

        /// <summary>
        /// Get class details for all GameActions
        /// </summary>
        /// <returns></returns>
        internal static List<ClassDetailsAttribute> FindTypesClassDetails()
        {
            return EditorHelper.FindTypesClassDetails(typeof(GameCondition));
        }

        #region Display

        internal static void DrawConditions(SerializedObject serializedObject, SerializedProperty conditionsProperty, GameConditionReference[] conditionReferences, 
            ref GameConditionEditor[] conditionEditors, List<ClassDetailsAttribute> classDetails, string heading = null, string tooltip = null)
        {
            if (heading != null)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField(new GUIContent(heading, tooltip), EditorStyles.boldLabel);
            }

            EditorGUILayout.Space();

            conditionEditors = GameConditionEditorHelper.CheckAndCreateSubEditors(conditionEditors, conditionReferences, serializedObject, conditionsProperty);

            if (conditionsProperty.arraySize == 0)
            {
                EditorGUILayout.LabelField("No conditions specified.", GuiStyles.CenteredLabelStyle, GUILayout.ExpandWidth(true));
            }
            else
            {
                // Display all items - use a for loop rather than a foreach loop in case of deletion.
                for (var i = 0; i < conditionsProperty.arraySize; i++)
                {
                    EditorGUILayout.BeginVertical(GUI.skin.box);

                    if (conditionEditors[i] == null)
                    {
                        var conditionReference = conditionsProperty.GetArrayElementAtIndex(i);
                        var conditionClassNameProperty = conditionReference.FindPropertyRelative("_className");
                        EditorGUILayout.LabelField("Error loading " + conditionClassNameProperty.stringValue);
                    }
                    else
                    {
                        EditorGUILayout.BeginHorizontal();
                        var currentConditionClass = classDetails.Find(x => conditionReferences[i].ScriptableObject.GetType() == x.ClassType);
                        var conditionReference = conditionsProperty.GetArrayElementAtIndex(i);
                        EditorGUI.indentLevel++;
                        conditionReference.isExpanded = EditorGUILayout.Foldout(conditionReference.isExpanded, new GUIContent(currentConditionClass.Name, currentConditionClass.Tooltip));
                        EditorGUI.indentLevel--;
                        //EditorGUILayout.LabelField(new GUIContent(currentConditionClass.Name, currentConditionClass.Tooltip));

                        if (GUILayout.Button("-", GUILayout.Width(RemoveButtonWidth)))
                        {
                            conditionsProperty.DeleteArrayElementAtIndex(i);
                            break;
                        }
                        EditorGUILayout.EndHorizontal();

                        if (conditionReference.isExpanded)
                        {
                            EditorGUILayout.BeginVertical();
                            conditionEditors[i].OnInspectorGUI();
                            EditorGUILayout.EndVertical();
                        }
                    }

                    EditorGUILayout.EndVertical();
                }
            }

            if (GUILayout.Button(new GUIContent("Add Condition", "Add a new condition to the list"), EditorStyles.miniButton))
            {
                var menu = new GenericMenu();
                foreach (var classDetailsAttribute in classDetails)
                {
                    var conditionType = classDetailsAttribute.ClassType;
                    menu.AddItem(new GUIContent(classDetailsAttribute.Path), false, () => {
                        AddCondition(conditionType, conditionsProperty, serializedObject);
                    });
                }
                menu.ShowAsContext();
            }
        }

        internal static void AddCondition(object userData, SerializedProperty arrayProperty, SerializedObject serializedObject)
        {
            arrayProperty.arraySize++;
            var newElement = arrayProperty.GetArrayElementAtIndex(arrayProperty.arraySize - 1);
            var propClassName = newElement.FindPropertyRelative("_className");
            var actionType = (System.Type)userData;
            propClassName.stringValue = actionType.Name;
            newElement.isExpanded = true;
            newElement.FindPropertyRelative("_data").stringValue = null;
            newElement.FindPropertyRelative("_isReference").boolValue = false;
            newElement.FindPropertyRelative("_objectReferences").arraySize = 0;
            serializedObject.ApplyModifiedProperties();
        }

        #endregion Display

        #region SubEditors

        /// <summary>
        /// In case of changes create sub editors
        /// </summary>
        internal static GameConditionEditor[] CheckAndCreateSubEditors(GameConditionEditor[] subEditors, GameConditionReference[] conditionReferences,
            SerializedObject container, SerializedProperty _scriptableReferenceArrayProperty)
        {
            // If there are the correct number of subEditors then do nothing.
            if (subEditors != null && subEditors.Length == conditionReferences.Length)
                return subEditors;

            // Otherwise get rid of any old editors.
            EditorHelper.CleanupSubEditors(subEditors);

            // Create an array of the subEditor type that is the right length for the targets.
            subEditors = new GameConditionEditor[conditionReferences.Length];

            // Populate the array and setup each Editor.
            for (var i = 0; i < subEditors.Length; i++)
            {
                subEditors[i] =
                    UnityEditor.Editor.CreateEditor(conditionReferences[i].ScriptableObject) as GameConditionEditor;
                if (subEditors[i] != null)
                {
                    subEditors[i].Container = conditionReferences[i];
                    var scriptableObjectContainer = _scriptableReferenceArrayProperty.GetArrayElementAtIndex(i);
                    subEditors[i].ContainerSerializedObject = container;
                    subEditors[i].DataProperty =
                        scriptableObjectContainer.FindPropertyRelative("_data");
                    subEditors[i].ObjectReferencesProperty =
                        scriptableObjectContainer.FindPropertyRelative("_objectReferences"); 
                }
            }
            return subEditors;
        }

        #endregion SubEditors
    }
}

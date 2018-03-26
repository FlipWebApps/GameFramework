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
using GameFramework.GameStructure.Game.Editor.GameActions;
using GameFramework.GameStructure.Game.ObjectModel;
using GameFramework.GameStructure.Game.ObjectModel.Abstract;
using GameFramework.Helper;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameFramework.GameStructure.Game.Editor.GameActions
{
    /// <summary>
    /// Helper class for GameAction Editors
    /// </summary>
    public class GameActionEditorHelper
    {
        // names for possible targets.
        public static readonly string[] TargetNames = { "This GameObject", "Specified" };

        // names for possible targets with ColliderOptions set
        public static readonly string[] TargetNamesCollider = { "This GameObject", "Specified", "Colliding GameObject" };

        // enum to control different game action editor options.
        [System.Flags]
        public enum GameActionEditorOption {
            None = 0,                   // No special setup - use defaults.
            ColliderOptions = 1         // Show options for using collider
        }

        const float RemoveButtonWidth = 30f;

        /// <summary>
        /// Get class details for all GameActions
        /// </summary>
        /// <returns></returns>
        internal static List<ClassDetailsAttribute> GameActionClassDetails
        {
            get {
                if (_gameActionClassDetails == null)
                    _gameActionClassDetails = EditorHelper.FindTypesClassDetails(typeof(GameAction));
                return _gameActionClassDetails;
            }
        }
        static List<ClassDetailsAttribute> _gameActionClassDetails;

        /// <summary>
        /// Get class details for all GameActions
        /// </summary>
        /// <returns></returns>
        internal static List<ClassDetailsAttribute> FindTypesClassDetails()
        {
            return EditorHelper.FindTypesClassDetails(typeof(GameAction));
        }


        #region Display

        internal static void DrawActions(SerializedObject serializedObject, SerializedProperty actionsProperty, GameActionReference[] actionReferences, 
            ref GameActionEditor[] actionEditors, List<ClassDetailsAttribute> classDetails, SerializedProperty callbackProperty, GameActionEditorOption options = GameActionEditorOption.None, string heading = null, string tooltip = null)
        {
            if (heading != null)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField(new GUIContent(heading, tooltip), EditorStyles.boldLabel);
                EditorGUILayout.Space();
            }

            actionEditors = GameActionEditorHelper.CheckAndCreateSubEditors(actionEditors, actionReferences, serializedObject, actionsProperty, options);

            if (actionsProperty.arraySize == 0)
            {
                EditorGUILayout.LabelField("No actions specified.", GuiStyles.CenteredLabelStyle, GUILayout.ExpandWidth(true));
            }
            else
            {
                // Display all items - use a for loop rather than a foreach loop in case of deletion.
                for (var i = 0; i < actionsProperty.arraySize; i++)
                {
                    EditorGUILayout.BeginVertical(GUI.skin.box);

                    if (actionEditors[i] == null)
                    {
                        EditorGUILayout.BeginHorizontal();
                        var actionReference = actionsProperty.GetArrayElementAtIndex(i);
                        var actionClassNameProperty = actionReference.FindPropertyRelative("_className");
                        EditorGUILayout.LabelField("Error loading " + actionClassNameProperty.stringValue);
                        if (GUILayout.Button("-", GUILayout.Width(RemoveButtonWidth)))
                        {
                            actionsProperty.DeleteArrayElementAtIndex(i);
                            break;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    else
                    {
                        EditorGUILayout.BeginHorizontal();
                        var currentActionClass = classDetails.Find(x => actionReferences[i].ScriptableObject.GetType() == x.ClassType);
                        var actionReference = actionsProperty.GetArrayElementAtIndex(i);
                        EditorGUI.indentLevel++;
                        actionReference.isExpanded = EditorGUILayout.Foldout(actionReference.isExpanded, new GUIContent(currentActionClass.Name, currentActionClass.Tooltip));
                        EditorGUI.indentLevel--;

                        //EditorGUILayout.LabelField(new GUIContent(currentActionClass.Name, currentActionClass.Tooltip)); // actionEditors[i].GetLabel(), 
                        if (GUILayout.Button("-", GUILayout.Width(RemoveButtonWidth)))
                        {
                            actionsProperty.DeleteArrayElementAtIndex(i);
                            break;
                        }
                        EditorGUILayout.EndHorizontal();

                        if (actionReference.isExpanded)
                        {
                            EditorGUILayout.BeginVertical();
                            actionEditors[i].OnInspectorGUI();
                            EditorGUILayout.EndVertical();
                        }
                    }

                    EditorGUILayout.EndVertical();
                }
            }

            if (GUILayout.Button(new GUIContent("Add Action", "Add a new action to the list"), EditorStyles.miniButton))
            {
                var menu = new GenericMenu();
                foreach (var classDetailsAttribute in classDetails)
                {
                    if (!classDetailsAttribute.ExcludeFromMenu)
                    {
                        var actionType = classDetailsAttribute.ClassType;
                        menu.AddItem(new GUIContent(classDetailsAttribute.Path), false, () =>
                        {
                            AddAction(actionType, actionsProperty, serializedObject);
                        });
                    }
                }
                menu.ShowAsContext();
            }

            if (callbackProperty != null)
            {
                EditorGUILayout.Space();
                EditorGUI.indentLevel++;
                callbackProperty.isExpanded = EditorGUILayout.Foldout(callbackProperty.isExpanded, "Custom Callbacks");
                if (callbackProperty.isExpanded)
                    EditorGUILayout.PropertyField(callbackProperty, new GUIContent("Callbacks", callbackProperty.tooltip), true);
                EditorGUI.indentLevel--;
            }
        }

        internal static void AddAction(object userData, SerializedProperty arrayProperty, SerializedObject serializedObject)
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
        internal static GameActionEditor[] CheckAndCreateSubEditors(GameActionEditor[] subEditors, GameActionReference[] actionReferences,
            SerializedObject container, SerializedProperty _scriptableReferenceArrayProperty, GameActionEditorOption options = GameActionEditorOption.None)
        {
            // If there are the correct number of subEditors then do nothing.
            if (subEditors != null && subEditors.Length == actionReferences.Length)
                return subEditors;

            // Otherwise get rid of any old editors.
            EditorHelper.CleanupSubEditors(subEditors);

            // Create an array of the subEditor type that is the right length for the targets.
            subEditors = new GameActionEditor[actionReferences.Length];

            // Populate the array and setup each Editor.
            for (var i = 0; i < subEditors.Length; i++)
            {
                subEditors[i] =
                    UnityEditor.Editor.CreateEditor(actionReferences[i].ScriptableObject) as GameActionEditor;
                if (subEditors[i] != null)
                {
                    subEditors[i].Options = options;
                    subEditors[i].Container = actionReferences[i];
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

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
using GameFramework.GameStructure.Game.Components;
using GameFramework.GameStructure.Game.Editor.GameActions.Abstract;
using GameFramework.GameStructure.Game.ObjectModel;
using GameFramework.GameStructure.Game.ObjectModel.Abstract;
using GameFramework.Helper;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameFramework.GameStructure.Game.Editor
{
    [CustomEditor(typeof(GameCollider))]
    public class GameColliderEditor : UnityEditor.Editor
    {
        //GameItem _gameItem;
        SerializedProperty _collidingTagProperty;
        SerializedProperty _intervalProperty;
        SerializedProperty _disableAfterUseProperty;
        SerializedProperty _onlyWhenLevelRunningProperty;

        SerializedProperty _enterProperty;
        SerializedProperty _processWithinProperty;
        SerializedProperty _runIntervalProperty;
        SerializedProperty _withinProperty;
        SerializedProperty _exitProperty;

        float RemoveButtonWidth = 30f;
        GameCollider gameCollider;

        //List<Type> _actionTypes;
        List<ClassDetailsAttribute> _actionClassDetails;
        //List<string> _actionTypeNames;

        GameActionEditor[] actionEditorsEnter;
        GameActionEditor[] actionEditorsWithin;
        GameActionEditor[] actionEditorsExit;

        protected virtual void OnEnable()
        {
            gameCollider = (GameCollider)target;

            // get serialized objects so we can use attached property drawers (e.g. tooltips, ...)
            _collidingTagProperty = serializedObject.FindProperty("_collidingTag");
            _intervalProperty = serializedObject.FindProperty("_interval");
            _disableAfterUseProperty = serializedObject.FindProperty("_disableAfterUse");
            _onlyWhenLevelRunningProperty = serializedObject.FindProperty("_onlyWhenLevelRunning");
            _enterProperty = serializedObject.FindProperty("_enter");
            _processWithinProperty = serializedObject.FindProperty("_processWithin");
            _runIntervalProperty = serializedObject.FindProperty("_runInterval");
            _withinProperty = serializedObject.FindProperty("_within");
            _exitProperty = serializedObject.FindProperty("_exit");

            // setup actions types
            var actionTypes = EditorHelper.FindTypes(typeof(GameAction));
            _actionClassDetails = EditorHelper.TypeListClassDetails(actionTypes);
            //_actionTypes.Sort((type1, type2) => type1.Name.CompareTo(type2.Name));
            //_actionTypeNames = EditorHelper.TypeListToNames(_actionTypes);
            //for (int i = 0; i < _actionTypes.Count; i++)
            //    if (_actionClassDetails[i] != null && !string.IsNullOrEmpty(_actionClassDetails[i].Name))
            //        _actionTypeNames[i] = _actionClassDetails[i].Name;

            //foreach (var type in _actionTypes)
            //    foreach (var attr in type.GetCustomAttributes(typeof(ClassDetailsAttribute), true))
            //        Debug.Log(((ClassDetailsAttribute)attr).Name + ", " + ((ClassDetailsAttribute)attr).Path);
        }

        protected void OnDisable()
        {
            GameActionEditorHelper.CleanupSubEditors(actionEditorsEnter);
            GameActionEditorHelper.CleanupSubEditors(actionEditorsWithin);
            GameActionEditorHelper.CleanupSubEditors(actionEditorsExit);
            actionEditorsEnter = null;
            actionEditorsWithin = null;
            actionEditorsExit = null;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawGUI();
            serializedObject.ApplyModifiedProperties();
        }


        protected void DrawGUI()
        {
#if !PRO_POOLING
            EditorGUILayout.HelpBox("This component features enhancements when combined with the ProPooling asset (also included in the extras bundle) allowing you to add gameobjects to the scene from a pre-allocated pool. Adding from a pool gives performance gains over instantiating prefabs on the fly. For more details see: Main Menu | Window | Game Framework | Integrations Window", MessageType.Info);
#endif
            EditorGUILayout.PropertyField(_collidingTagProperty);
            EditorGUILayout.PropertyField(_intervalProperty);
            EditorGUILayout.PropertyField(_disableAfterUseProperty);
            EditorGUILayout.PropertyField(_onlyWhenLevelRunningProperty);
            DrawTriggerData(_enterProperty, gameCollider.Enter.ActionReferences, ref actionEditorsEnter, "When Entering a Trigger", "The actions that should happen when a GameObject with a matching tag enters a trigger.");

            EditorGUILayout.Space();
            EditorGUILayout.LabelField(new GUIContent("When Within a Trigger", "The actions that should happen when a GameObject with a matching tag has entered and remains within a trigger."), EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_processWithinProperty);
            if (_processWithinProperty.boolValue)
            {
                EditorGUILayout.PropertyField(_runIntervalProperty);
                DrawTriggerData(_withinProperty, gameCollider.Within.ActionReferences, ref actionEditorsWithin);
            }
            DrawTriggerData(_exitProperty, gameCollider.Exit.ActionReferences, ref actionEditorsExit, "When Exiting a Trigger", "The actions that should happen when a GameObject with a matching tag exits a trigger.");
        }

        
        void DrawTriggerData(SerializedProperty triggerDataProperty, GameActionReference[] actionReferences, ref GameActionEditor[] actionEditors, string heading = null, string tooltip = null)
        {
            if (heading != null)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField(new GUIContent(heading, tooltip), EditorStyles.boldLabel);
            }

            EditorGUILayout.Space();

            var actionsProperty = triggerDataProperty.FindPropertyRelative("_actionReferences");
            //EditorGUILayout.PropertyField(actionsProperty, true);
            actionEditors = GameActionEditorHelper.CheckAndCreateSubEditors(actionEditors, actionReferences, serializedObject, actionsProperty);

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
                        var actionReference = actionsProperty.GetArrayElementAtIndex(i);
                        var actionClassNameProperty = actionReference.FindPropertyRelative("_className");
                        EditorGUILayout.LabelField("Error loading " + actionClassNameProperty.stringValue);
                    }
                    else
                    {
                        EditorGUILayout.BeginHorizontal();
                        var currentActionClass = _actionClassDetails.Find(x => actionReferences[i].ScriptableObject.GetType() == x.ClassType);//  actionReferences[i].ScriptableObject.GetType() find match from _actionClassDetailsList
                        EditorGUILayout.LabelField(new GUIContent(currentActionClass.Name, currentActionClass.Tooltip)); // actionEditors[i].GetLabel(), 
                        if (GUILayout.Button("-", GUILayout.Width(RemoveButtonWidth)))
                        {
                            actionsProperty.DeleteArrayElementAtIndex(i);
                            break;
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginVertical();
                        actionEditors[i].OnInspectorGUI();
                        EditorGUILayout.EndVertical();
                    }

                    EditorGUILayout.EndVertical();
                }
            }

            if (GUILayout.Button(new GUIContent("Add Action", "Add a new action to the list"), EditorStyles.miniButton))
            {
                var menu = new GenericMenu();
                for (var i = 0; i < _actionClassDetails.Count; i++)
                {
                    var actionType = _actionClassDetails[i].ClassType;
                    menu.AddItem(new GUIContent(_actionClassDetails[i].Path), false, () => {
                        AddAction(actionType, actionsProperty);
                    });
                }
                menu.ShowAsContext();
            }

            EditorGUILayout.Space();
            var property = triggerDataProperty.FindPropertyRelative("_callback");
            EditorGUILayout.PropertyField(property, true);
        }

        void AddAction(object userData, SerializedProperty arrayProperty)
        {
            arrayProperty.arraySize++;
            var newElement = arrayProperty.GetArrayElementAtIndex(arrayProperty.arraySize - 1);
            var propClassName = newElement.FindPropertyRelative("_className");
            var actionType = (Type)userData;
            propClassName.stringValue = actionType.Name;
            newElement.FindPropertyRelative("_data").stringValue = null;
            newElement.FindPropertyRelative("_isReference").boolValue = false;
            newElement.FindPropertyRelative("_objectReferences").arraySize = 0;
            serializedObject.ApplyModifiedProperties();
        }
    }
}

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
using GameFramework.GameStructure.Game.Editor.GameActions;
using GameFramework.GameStructure.Game.ObjectModel;
using GameFramework.Helper;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameFramework.GameStructure.Game.Editor
{
    [CustomEditor(typeof(CollisionHandler))]
    public class CollisionHandlerEditor : UnityEditor.Editor
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
        SerializedProperty _processExitProperty;
        SerializedProperty _exitProperty;

        CollisionHandler gameCollider;

        List<ClassDetailsAttribute> _gameActionClassDetails;
        Rect _mainHelpRect;

        GameActionEditor[] actionEditorsEnter;
        GameActionEditor[] actionEditorsWithin;
        GameActionEditor[] actionEditorsExit;

        protected virtual void OnEnable()
        {
            gameCollider = (CollisionHandler)target;

            // get serialized objects so we can use attached property drawers (e.g. tooltips, ...)
            _collidingTagProperty = serializedObject.FindProperty("_collidingTag");
            _intervalProperty = serializedObject.FindProperty("_interval");
            _disableAfterUseProperty = serializedObject.FindProperty("_disableAfterUse");
            _onlyWhenLevelRunningProperty = serializedObject.FindProperty("_onlyWhenLevelRunning");
            _enterProperty = serializedObject.FindProperty("_enter");
            _processWithinProperty = serializedObject.FindProperty("_processWithin");
            _runIntervalProperty = serializedObject.FindProperty("_runInterval");
            _withinProperty = serializedObject.FindProperty("_within");
            _processExitProperty = serializedObject.FindProperty("_processExit");
            _exitProperty = serializedObject.FindProperty("_exit");

            // setup actions types
            _gameActionClassDetails = GameActionEditorHelper.FindTypesClassDetails();
        }


        protected void OnDisable()
        {
            EditorHelper.CleanupSubEditors(actionEditorsEnter);
            EditorHelper.CleanupSubEditors(actionEditorsWithin);
            EditorHelper.CleanupSubEditors(actionEditorsExit);
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
            _mainHelpRect = EditorHelper.ShowHideableHelpBox("GameFramework.GameColliderEditor", "With this component you can configure many different actions that can occur when a physics collision happens.\nMore actions will come over time - if there is something you are missing then let us know. Alternatively easily create your own custom actions or add a callback .", _mainHelpRect);

            EditorGUILayout.PropertyField(_collidingTagProperty);
            EditorGUILayout.PropertyField(_intervalProperty);
            EditorGUILayout.PropertyField(_disableAfterUseProperty);
            EditorGUILayout.PropertyField(_onlyWhenLevelRunningProperty);
            DrawTriggerData(_enterProperty, gameCollider.Enter.ActionReferences, ref actionEditorsEnter, "When Entering a Collision", "The actions that should happen when a GameObject with a matching tag enters a trigger.");

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_processWithinProperty, new GUIContent(), GUILayout.MaxWidth(15));
            EditorGUILayout.LabelField(new GUIContent("When Within a Collision", "The actions that should happen when a GameObject with a matching tag has entered and remains within a trigger."), EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            if (_processWithinProperty.boolValue)
            {
                EditorGUILayout.PropertyField(_runIntervalProperty);
                DrawTriggerData(_withinProperty, gameCollider.Within.ActionReferences, ref actionEditorsWithin);
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_processExitProperty, new GUIContent(), GUILayout.MaxWidth(15));
            EditorGUILayout.LabelField(new GUIContent("When Exiting a Collision", "The actions that should happen when a GameObject with a matching tag exits a trigger."), EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            if (_processExitProperty.boolValue)
            {
                DrawTriggerData(_exitProperty, gameCollider.Exit.ActionReferences, ref actionEditorsExit);
            }
        }

        
        void DrawTriggerData(SerializedProperty triggerDataProperty, GameActionReference[] actionReferences, ref GameActionEditor[] actionEditors, string heading = null, string tooltip = null)
        {
            var actionsProperty = triggerDataProperty.FindPropertyRelative("_actionReferences");
            var callbackProperty = triggerDataProperty.FindPropertyRelative("_callback");
            GameActionEditorHelper.DrawActions(serializedObject, actionsProperty, actionReferences,
                ref actionEditors, _gameActionClassDetails, callbackProperty, GameActionEditorHelper.GameActionEditorOption.ColliderOptions, heading, tooltip);
        }
    }
}

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
using GameFramework.EditorExtras.Editor;
using UnityEditor;
using UnityEngine;

namespace GameFramework.GameStructure.Colliders.Editor.AbstractClasses
{
    public abstract class ColliderEditor : UnityEditor.Editor
    {
        //GameItem _gameItem;
        SerializedProperty _collidingTagProperty;
        SerializedProperty _intervalProperty;
        SerializedProperty _disableAfterUseProperty;
        SerializedProperty _onlyWhenLevelRunningProperty;

        SerializedProperty _enterProperty;
        SerializedProperty _actionsProperty;

        SerializedProperty _processWithinProperty;
        SerializedProperty _runIntervalProperty;
        SerializedProperty _withinProperty;
        SerializedProperty _exitProperty;

        public const float RemoveButtonWidth = 30f;

        readonly string[] _conditionNames = Enum.GetNames(typeof(GenericCollider.ActionTypes));
        readonly int[] _conditionValues = (int[])Enum.GetValues(typeof(GenericCollider.ActionTypes));
        readonly string[] _conditionTooltips = {
            "Whether this GameItem can be unlocked by coins",
            "Whether this GameItem can be unlocked by completion",
            "Whether this GameItem can be unlocked by payment",
            "Compare the GameItems coins with a specified value",
            "Whether the current player can unlock this GameItem (combine if needed with Unlocked condition)",
            "Compare the GameItems score with a specified value",
            "Whether this GameItem is selected",
            "Whether this GameItem is unlocked",
            "A custom Condition that you should add a reference to",
            "Whether an unlocked animation has been show for this GameItem",
        };


        protected virtual void OnEnable()
        {
            // get serialized objects so we can use attached property drawers (e.g. tooltips, ...)
            _collidingTagProperty = serializedObject.FindProperty("_collidingTag");
            _intervalProperty = serializedObject.FindProperty("_interval");
            _disableAfterUseProperty = serializedObject.FindProperty("_disableAfterUse");
            _onlyWhenLevelRunningProperty = serializedObject.FindProperty("_onlyWhenLevelRunning");
            _enterProperty = serializedObject.FindProperty("_enter");
            _actionsProperty = serializedObject.FindProperty("ActionReferences");
            _processWithinProperty = serializedObject.FindProperty("_processWithin");
            _runIntervalProperty = serializedObject.FindProperty("_runInterval");
            _withinProperty = serializedObject.FindProperty("_within");
            _exitProperty = serializedObject.FindProperty("_exit");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawGUI();
            serializedObject.ApplyModifiedProperties();
        }


        protected void DrawGUI()
        {
            DrawBasicProperties();
            DrawCustomProperties();
        }


        protected void DrawBasicProperties()
        {
#if !PRO_POOLING
            EditorGUILayout.HelpBox("This component features enhancements when combined with the ProPooling asset (also included in the extras bundle) allowing you to add gameobjects to the scene from a pre-allocated pool. Adding from a pool gives performance gains over instantiating prefabs on the fly. For more details see: Main Menu | Window | Game Framework | Integrations Window", MessageType.Info);
#endif
            EditorGUILayout.PropertyField(_collidingTagProperty);
            EditorGUILayout.PropertyField(_intervalProperty);
            EditorGUILayout.PropertyField(_disableAfterUseProperty);
            EditorGUILayout.PropertyField(_onlyWhenLevelRunningProperty);
            DrawTriggerData(_enterProperty, "When Entering a Trigger", "The actions that should happen when a GameObject with a matching tag enters a trigger.");


            // Display all items - use a for loop rather than a foreach loop in case of deletion.
            //EditorGUILayout.PropertyField(_conditionsProperty, true);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(new GUIContent("Conditions", "The different conditions that this component should react to."), EditorStyles.boldLabel);
            for (var i = 0; i < _actionsProperty.arraySize; i++)
            {
                // draw built in editor 
                EditorGUILayout.BeginHorizontal(GUI.skin.box);

                var actionReference = _actionsProperty.GetArrayElementAtIndex(i);
                var actionType = (GenericCollider.ActionTypes)((GenericCollider)target).ActionReferences[i].Identifier;
                EditorGUILayout.PrefixLabel(new GUIContent(EditorHelper.PrettyPrintCamelCase(_conditionNames[(int)actionType]), _conditionTooltips[(int)actionType]));
                switch (actionType)
                {
                    case GenericCollider.ActionTypes.InstantiatePrefab:
                        var scriptableObjectProperty = actionReference.FindPropertyRelative("_scriptableObject");
                        var prefabProperty = scriptableObjectProperty.FindPropertyRelative("_prefab");
                        //EditorGUILayout.PropertyField(prefabProperty, GUIContent.none, GUILayout.ExpandWidth(true));
                        break;
                    case GenericCollider.ActionTypes.Custom:
                        scriptableObjectProperty = actionReference.FindPropertyRelative("_scriptableObject");
                        EditorGUILayout.PropertyField(scriptableObjectProperty, GUIContent.none, GUILayout.ExpandWidth(true));
                        break;
                    default:
                        Debug.LogError("Unknown built in type : " + ((GenericCollider)target).ActionReferences[i].Identifier);
                        break;
                }
                if (GUILayout.Button("-", GUILayout.Width(RemoveButtonWidth)))
                {
                    _actionsProperty.DeleteArrayElementAtIndex(i);
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }

            //            if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Plus More", "Add to list"), EditorStyles.miniButton))
            if (GUILayout.Button(new GUIContent("Add Action", "Add a new action to the list"), EditorStyles.miniButton))
            {
                var menu = new GenericMenu();
                for (var i = 0; i < _conditionNames.Length; i++)
                {
                    menu.AddItem(new GUIContent(_conditionNames[i]), false, AddAction,
                        _conditionValues[i]);
                }
                menu.ShowAsContext();
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField(new GUIContent("When Within a Trigger", "The actions that should happen when a GameObject with a matching tag has entered and remains within a trigger."), EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_processWithinProperty);
            if (_processWithinProperty.boolValue)
            {
                EditorGUILayout.PropertyField(_runIntervalProperty);
                DrawTriggerData(_withinProperty);
            }
            DrawTriggerData(_exitProperty, "When Exiting a Trigger", "The actions that should happen when a GameObject with a matching tag exits a trigger.");
        }


        void DrawTriggerData(SerializedProperty triggerDataProperty, string heading = null, string tooltip = null)
        {
            if (heading != null)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField(new GUIContent(heading, tooltip), EditorStyles.boldLabel);
            }

            var property = triggerDataProperty.FindPropertyRelative("_instantiatePrefab");
            EditorGUILayout.PropertyField(property);
            property = triggerDataProperty.FindPropertyRelative("_addPooledItem");
            EditorGUILayout.PropertyField(property);
            property = triggerDataProperty.FindPropertyRelative("_audioClip");
            EditorGUILayout.PropertyField(property);
            property = triggerDataProperty.FindPropertyRelative("_enableGameObjects");
            EditorGUILayout.PropertyField(property, true);
            property = triggerDataProperty.FindPropertyRelative("_disableGameObjects");
            EditorGUILayout.PropertyField(property, true);
            property = triggerDataProperty.FindPropertyRelative("_callback");
            EditorGUILayout.PropertyField(property, true);
        }


        protected abstract void DrawCustomProperties();


        void AddAction(object conditionValue)
        {
            var conditionType = (GenericCollider.ActionTypes)conditionValue;

            _actionsProperty.arraySize++;
            var newElement = _actionsProperty.GetArrayElementAtIndex(_actionsProperty.arraySize - 1);
            var propName = newElement.FindPropertyRelative("_identifier");
            propName.intValue = (int)conditionType;
            var propUseScriptableObject = newElement.FindPropertyRelative("_useScriptableObject");
            if (conditionType == GenericCollider.ActionTypes.Custom)
            {
                propUseScriptableObject.boolValue = true;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}

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
        SerializedProperty _processWithinProperty;
        SerializedProperty _runIntervalProperty;
        SerializedProperty _withinProperty;
        SerializedProperty _exitProperty;

        protected virtual void OnEnable()
        {
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
            EditorGUILayout.HelpBox("All old colliders are replaced in favour of the new, more powerful, Game Collider (Add Component | Game Framework | Game Structure | Game Collider) and will in the future be removed. Please convert your game to use this new component.", MessageType.Error);

            EditorGUILayout.PropertyField(_collidingTagProperty);
            EditorGUILayout.PropertyField(_intervalProperty);
            EditorGUILayout.PropertyField(_disableAfterUseProperty);
            EditorGUILayout.PropertyField(_onlyWhenLevelRunningProperty);
            DrawTriggerData(_enterProperty, "When Entering a Trigger", "The actions that should happen when a GameObject with a matching tag enters a trigger.");

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
    }
}

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
using GameFramework.GameStructure.Game.ObjectModel.Abstract;
using GameFramework.Helper;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameFramework.GameStructure.Game.Editor.GameActions
{
    [CustomEditor(typeof(GameAction), true)]
    public class GameActionEditor : UnityEditor.Editor
    {
        public GameActionEditorHelper.GameActionEditorOption Options;
        public IScriptableObjectContainerReferences Container;
        public SerializedObject ContainerSerializedObject;
        public SerializedProperty DataProperty; // Needed to ensure configuration string gets updated.
        public SerializedProperty ObjectReferencesProperty;

        protected SerializedProperty DelayProperty;

        // shortcut to common optional properties
        protected SerializedProperty TargetTypeProperty;
        protected SerializedProperty TargetProperty;

        protected Rect HideableHelpRect;    // incase anybase classed need to show help.

        GameAction _action;
        string _name;

        void OnEnable()
        {
            _action = (GameAction)target;
            DelayProperty = serializedObject.FindProperty("_delay");

            // below are optional but commonly used so we add here even though they may return null!
            TargetTypeProperty = serializedObject.FindProperty("_targetType");
            TargetProperty = serializedObject.FindProperty("_target");

            // Call an initialisation method for inheriting classes.
            Initialise();
        }


        /// <summary>
        /// Override this method if you need to do any specific initialisation for the ActionEditor implementation.
        /// </summary>
        protected virtual void Initialise() { }


        /// <summary>
        /// Draws the GUI. Override DrawGUI if you want additional control on just the contents part (excluding the frame part)
        /// </summary>
        public override void OnInspectorGUI()
        {
            // Update the object from the Data and then pull into the serializedObject for editing.
            JsonUtility.FromJsonOverwrite(DataProperty.stringValue, target);
            _action.SetReferencesFromContainer(Container.ObjectReferences);
            serializedObject.Update();

            // Draw custom GUI
            DrawGUI();

            // push data back from the serializedObject to the target.
            serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
            {
                DataProperty.stringValue = JsonUtility.ToJson(_action);
                var objectReferences = _action.GetReferencesForContainer();
                if (objectReferences != null)
                {
                    if (ObjectReferencesProperty.arraySize != objectReferences.Length)
                        ObjectReferencesProperty.arraySize = objectReferences.Length;
                    for (int i = 0; i < objectReferences.Length; i++)
                    {
                        var prop = ObjectReferencesProperty.GetArrayElementAtIndex(i);
                        prop.objectReferenceValue = objectReferences[i];
                    }
                }
            }
        }


        /// <summary>
        /// This function can overridden by inheriting classes, but if it isn't, draw the default for it's properties.
        /// </summary>
        protected virtual void DrawGUI()
        {
            EditorHelper.DrawDefaultInspector(serializedObject, new List<string>() { "m_Script" });
        }


        #region helper methods
        /// <summary>
        /// Returns whether this editor should show collider options.
        /// </summary>
        /// <returns></returns>
        protected bool IsColliderEditor()
        {
            return (Options & GameActionEditorHelper.GameActionEditorOption.ColliderOptions) != 0;
        }


        /// <summary>
        /// Helper method to show a target property based upon GameActionHelper.TargetType
        /// </summary>
        /// <param name="typeProperty"></param>
        /// <param name="targetProperty"></param>
        /// <param name="label"></param>
        protected void ShowTargetTypeProperty(SerializedProperty typeProperty, SerializedProperty targetProperty, string label)
        {
            typeProperty.enumValueIndex = EditorGUILayout.Popup(label, typeProperty.enumValueIndex,
                IsColliderEditor() ? GameActionEditorHelper.TargetNamesCollider : GameActionEditorHelper.TargetNames);
            if (typeProperty.enumValueIndex == 1)
                EditorGUILayout.PropertyField(targetProperty, new GUIContent(" ", targetProperty.tooltip));
        }



        /// <summary>
        /// Helper method to show a target property based upon GameActionHelper.TargetType
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="targetTypeProperty"></param>
        /// <param name="targetProperty"></param>
        /// <param name="label"></param>
        protected void ShowTargetTypeProperty(SerializedObject obj, string targetTypePropertyName, string targetPropertyName, string label)
        {
            var targetTypeProperty = serializedObject.FindProperty(targetTypePropertyName);
            var targetProperty = serializedObject.FindProperty(targetPropertyName);
            ShowTargetTypeProperty(targetTypeProperty, targetProperty, label);
        }

        #endregion helper methods
    }
}

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
using GameFramework.GameStructure.GameItems.ObjectModel.Conditions;
using UnityEditor;
using UnityEngine;

namespace GameFramework.GameStructure.GameItems.Editor.Conditions
{
    public abstract class ConditionEditor : UnityEditor.Editor
    {
        // Represents the SerializedProperty of the array the target for this editor belongs to (use for e.g. deleting this item).
        public SerializedProperty ParentCollectionProperty;
        public SerializedProperty DataProperty;
        //public SerializedObject ParentTarget;
        public int index;

        Condition _condition;
        protected const float RemoveButtonWidth = 30f;

        void OnEnable()
        {
            // Cache the target reference.
            _condition = (Condition)target;

            // Call an initialisation method for inheriting classes.
            Init();
        }


        /// <summary>
        /// This function should be overridden by inheriting classes that need initialisation. 
        /// </summary>
        protected virtual void Init() { }


        /// <summary>
        /// Draws the GUI. Override DrawGUI if you want additional control on just the contents part (excluding the frame part)
        /// </summary>
        public override void OnInspectorGUI()
        {
            // Pull data from the target into the serializedObject.
            serializedObject.Update();

            EditorGUILayout.BeginHorizontal(GUI.skin.box);

            // Draw custom GUI
            DrawGUI();

            if (GUILayout.Button("-", GUILayout.Width(RemoveButtonWidth)))
            {
                ParentCollectionProperty.serializedObject.Update();
                ParentCollectionProperty.DeleteArrayElementAtIndex(index);
                ParentCollectionProperty.serializedObject.ApplyModifiedProperties();
            }
            else
            {
                // not deleted so Push data back from the serializedObject to the target.
                serializedObject.ApplyModifiedProperties();

                // and update the data property.
                DataProperty.stringValue = JsonUtility.ToJson(_condition);
                //ParentTarget.ApplyModifiedProperties();
            }
            EditorGUILayout.EndHorizontal();
        }


        /// <summary>
        /// This function can overridden by inheriting classes, but if it isn't, draw the default for it's properties.
        /// </summary>
        protected virtual void DrawGUI()
        {
            DrawDefaultInspector();
        }


        /// <summary>
        /// Override this to provide a custom label for the editor
        /// </summary>
        /// <returns></returns>
        protected virtual string GetLabel()
        {
            var name = _condition.GetType().Name;
            if (name.EndsWith("Condition"))
                name = name.Remove(name.Length - 9);
            return EditorHelper.PrettyPrintCamelCase(name);
        }


        /// <summary>
        /// Override this to provide a custom tooltip for the editor
        /// </summary>
        /// <returns></returns>
        protected virtual string GetTooltip()
        {
            return "";
        }
    }
}

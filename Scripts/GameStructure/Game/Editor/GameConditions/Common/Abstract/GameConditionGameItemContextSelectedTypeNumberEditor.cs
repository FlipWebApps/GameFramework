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

namespace GameFramework.GameStructure.Game.Editor.GameConditions.Common.Abstract
{
    public class GameConditionGameItemContextSelectedTypeNumberEditor : GameConditionEditor
    {
        SerializedProperty _gameItemTypeProperty;
        SerializedProperty _contextProperty;
        SerializedProperty _comparisonProperty;
        SerializedProperty _valueProperty;

        /// <summary>
        /// Override this method if you need to do any specific initialisation for the ActionEditor implementation.
        /// </summary>
        protected override void Initialise() {
            _gameItemTypeProperty = serializedObject.FindProperty("_gameItemType");
            _contextProperty = serializedObject.FindProperty("_context");
            _comparisonProperty = serializedObject.FindProperty("_comparison");
            _valueProperty = serializedObject.FindProperty("_value");
        }


        /// <summary>
        /// This function can be overridden by inheriting classes, but if it isn't, draw the default for it's properties.
        /// </summary>
        protected override void DrawGUI()
        {
            EditorGUILayout.PropertyField(_gameItemTypeProperty);
            EditorGUILayout.PropertyField(_contextProperty);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent("Value", "The value to compare against"));
            EditorGUILayout.PropertyField(_comparisonProperty, GUIContent.none, GUILayout.ExpandWidth(true));
            EditorGUILayout.PropertyField(_valueProperty, GUIContent.none, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();
        }
    }
}

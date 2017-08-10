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

using GameFramework.GameStructure.GameItems.ObjectModel;
using UnityEditor;
using UnityEngine;

namespace GameFramework.GameStructure.GameItems.Editor
{
    [CustomPropertyDrawer(typeof(GameItemContext))]
    public class GameItemContextDrawer : PropertyDrawer
    {
        readonly float _propertyRowHeight = EditorGUIUtility.singleLineHeight + 2;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var contextModeProperty = property.FindPropertyRelative("_contextMode");

            var rowPosition = new Rect(position) { height = EditorGUIUtility.singleLineHeight };
            EditorGUI.PropertyField(rowPosition, contextModeProperty, new GUIContent("Game Item Context", "The context that we are working within for determining what GameItem to use."));
            //if (contextModeProperty.enumValueIndex == 0)
            //{
            //    EditorGUI.indentLevel++;
            //    rowPosition.y += _propertyRowHeight;
            //    var reactToChangesProperty = property.FindPropertyRelative("_reactToChanges");
            //    EditorGUI.PropertyField(rowPosition, reactToChangesProperty);
            //    EditorGUI.indentLevel--;
            //}
            //else 
            if (contextModeProperty.enumValueIndex == 1)
            {
                EditorGUI.indentLevel++;
                rowPosition.y += _propertyRowHeight;
                var numberProperty = property.FindPropertyRelative("_number");
                EditorGUI.PropertyField(rowPosition, numberProperty);
                EditorGUI.indentLevel--;
            }
            else if (contextModeProperty.enumValueIndex == 3)
            {
                EditorGUI.indentLevel++;
                rowPosition.y += _propertyRowHeight;
                var referencedProperty = property.FindPropertyRelative("_referencedGameItemContextBase");
                EditorGUI.PropertyField(rowPosition, referencedProperty, new GUIContent("Referenced Context"));
                EditorGUI.indentLevel--;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = _propertyRowHeight;
            var contextModeProperty = property.FindPropertyRelative("_contextMode");
//            if (contextModeProperty.enumValueIndex == 0 || contextModeProperty.enumValueIndex == 1 || contextModeProperty.enumValueIndex == 3)
            if (contextModeProperty.enumValueIndex == 1 || contextModeProperty.enumValueIndex == 3)
            {
                    height += _propertyRowHeight;
            }

            return height;
        }
    }
}
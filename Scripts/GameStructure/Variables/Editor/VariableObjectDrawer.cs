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

using GameFramework.GameStructure.Variables.ObjectModel;
using UnityEditor;
using UnityEngine;

namespace GameFramework.GameStructure.Variables.Editor
{
    
    [CustomPropertyDrawer(typeof(StringVariable))]
    public class VariableObjectDrawer : PropertyDrawer
    {
        readonly float _propertyRowHeight = EditorGUIUtility.singleLineHeight + 2;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var tagProperty = property.FindPropertyRelative("_tag");
            var nameProperty = property.FindPropertyRelative("_name");
            var categoryProperty = property.FindPropertyRelative("_category");
            var defaultValueProperty = property.FindPropertyRelative("_defaultValue");
            //var valueProperty = property.FindPropertyRelative("Value");
            var persistChangesProperty = property.FindPropertyRelative("_persistChanges");

            var rowPosition = new Rect(position) { height = EditorGUIUtility.singleLineHeight };

            EditorGUI.indentLevel += 1;
            property.isExpanded = EditorGUI.Foldout(rowPosition, property.isExpanded, tagProperty.stringValue);
            rowPosition.y += _propertyRowHeight;
            if (property.isExpanded)
            {
                EditorGUI.PropertyField(rowPosition, tagProperty);
                rowPosition.y += _propertyRowHeight;
                EditorGUI.PropertyField(rowPosition, nameProperty);
                rowPosition.y += _propertyRowHeight;
                EditorGUI.PropertyField(rowPosition, categoryProperty);
                rowPosition.y += _propertyRowHeight;
                EditorGUI.PropertyField(rowPosition, defaultValueProperty);
                //rowPosition.y += _propertyRowHeight;
                //EditorGUI.PropertyField(rowPosition, valueProperty);
                rowPosition.y += _propertyRowHeight;
                EditorGUI.PropertyField(rowPosition, persistChangesProperty);
                rowPosition.y += _propertyRowHeight;

                //var addLocalisationButtonGuiContent = new GUIContent("Add Localisation Override");
                //var buttonSize = EditorStyles.miniButton.CalcSize(addLocalisationButtonGuiContent);
                //var buttonContentRect = new Rect(rowPosition)
                //{
                //    width = buttonSize.x + 10,
                //    x = rowPosition.center.x - ((buttonSize.x + 10) / 2)
                //};

            }
            EditorGUI.indentLevel -= 1;

            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = _propertyRowHeight;
            if (property.isExpanded)
            {
                height += _propertyRowHeight*5;
                //    var localisedItemsProperty = property.FindPropertyRelative("_localisedObjects");
                //    height += localisedItemsProperty.arraySize *_propertyRowHeight + _propertyRowHeight;
            }

            return height;
        }
    }
}
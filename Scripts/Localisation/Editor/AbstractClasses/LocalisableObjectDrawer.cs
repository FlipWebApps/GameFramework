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
using UnityEngine;
using UnityEditor;

namespace GameFramework.Localisation.Editor.AbstractClasses
{
    public abstract class LocalisableObjectDrawer : PropertyDrawer
    {
        readonly float _propertyRowHeight = EditorGUIUtility.singleLineHeight + 2;

        internal abstract string DefaultName { get; }
        internal abstract string DefaultTooltip { get; }
        internal abstract Type LocalisableType { get; }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var defaultProperty = property.FindPropertyRelative("_default");
            var localisedItemsProperty = property.FindPropertyRelative("_localisedObjects");

            var rowPosition = new Rect(position) { height = EditorGUIUtility.singleLineHeight };

            EditorGUI.BeginProperty(position, label, property);

            defaultProperty.objectReferenceValue = EditorGUI.ObjectField(rowPosition, new GUIContent(DefaultName, DefaultTooltip),  defaultProperty.objectReferenceValue, LocalisableType, false);
            //EditorGUI.PropertyField(rowPosition, defaultProperty, label);
            rowPosition.y += _propertyRowHeight;

            EditorGUI.indentLevel += 1;
            property.isExpanded = EditorGUI.Foldout(rowPosition, property.isExpanded, "Localisation Overrides");
            rowPosition.y += _propertyRowHeight;
            if (property.isExpanded)
            {
                if (localisedItemsProperty.arraySize > 0)
                {
                    for (var i = 0; i < localisedItemsProperty.arraySize; i++)
                    {
                        var arrayProperty = localisedItemsProperty.GetArrayElementAtIndex(i);
                        var languageProperty = arrayProperty.FindPropertyRelative("_language");
                        var itemProperty = arrayProperty.FindPropertyRelative("_object");
                        var contentRect = new Rect(rowPosition) {width = rowPosition.width/2};
                        EditorGUI.PropertyField(contentRect, languageProperty, GUIContent.none);
                        contentRect.x += contentRect.width + 2;
                        itemProperty.objectReferenceValue = EditorGUI.ObjectField(contentRect, itemProperty.objectReferenceValue, LocalisableType, false);
                        //EditorGUI.PropertyField(contentRect, itemProperty, GUIContent.none);
                        rowPosition.y += EditorGUIUtility.singleLineHeight + 2;
                    }
                }

                var addLocalisationButtonGuiContent = new GUIContent("Add Localisation Override");
                var buttonSize = EditorStyles.miniButton.CalcSize(addLocalisationButtonGuiContent);
                var buttonContentRect = new Rect(rowPosition)
                {
                    width = buttonSize.x + 10,
                    x = rowPosition.center.x - ((buttonSize.x + 10) / 2)
                };
                if (GUI.Button(buttonContentRect, addLocalisationButtonGuiContent, EditorStyles.miniButton))
                {
                    localisedItemsProperty.arraySize++;
                    var newElement =
                        localisedItemsProperty.GetArrayElementAtIndex(localisedItemsProperty.arraySize - 1);
                    var languageOverrideProperty = newElement.FindPropertyRelative("_language");
                    languageOverrideProperty.enumValueIndex = (int)SystemLanguage.Unknown;
                    var prefabOverrideProperty = newElement.FindPropertyRelative("_object");
                    prefabOverrideProperty.objectReferenceValue = null;
                }
            }
            EditorGUI.indentLevel -= 1;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = _propertyRowHeight*2;
            if (property.isExpanded)
            {
                var localisedItemsProperty = property.FindPropertyRelative("_localisedObjects");
                height += localisedItemsProperty.arraySize *_propertyRowHeight + _propertyRowHeight;
            }

            return height;
        }
    }
}
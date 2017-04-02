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

using GameFramework.Animation.ObjectModel;
using UnityEditor;
using UnityEngine;

namespace GameFramework.Animation.Editor
{
    [CustomPropertyDrawer(typeof(GameObjectToGameObjectAnimation))]
    public class GameObjectToGameObjectAnimationDrawer : PropertyDrawer
    {
#if BEAUTIFUL_TRANSITIONS
        readonly float _propertyRowHeight = EditorGUIUtility.singleLineHeight + 2;
#else
        const string RequiresBeautifulTransitionsText =
            "If you have the Beautiful Transitions asset (also included in the extras bundle) then you can automatically animate gameobject changes. For more details see: Main Menu | Window | Game Framework | Integrations Window";
#endif

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
#if BEAUTIFUL_TRANSITIONS
            var contextModeProperty = property.FindPropertyRelative("_animationMode");

            var rowPosition = new Rect(position) { height = EditorGUIUtility.singleLineHeight };
            EditorGUI.PropertyField(rowPosition, contextModeProperty);

            if (contextModeProperty.enumValueIndex == 1) // Beautiful Transitions
            {
                EditorGUI.indentLevel++;
                rowPosition.y += _propertyRowHeight;
                var numberProperty = property.FindPropertyRelative("_transitionOrder");
                EditorGUI.PropertyField(rowPosition, numberProperty);
                EditorGUI.indentLevel--;
            }
#else
            EditorGUI.HelpBox(position, RequiresBeautifulTransitionsText, MessageType.Info);
#endif
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
#if BEAUTIFUL_TRANSITIONS
            var height = _propertyRowHeight;
            var contextModeProperty = property.FindPropertyRelative("_animationMode");
            if (contextModeProperty.enumValueIndex == 1) // Beautiful Transitions
            {
                height += _propertyRowHeight;
            }
            return height;
#else
            return EditorStyles.helpBox.CalcHeight(new GUIContent(RequiresBeautifulTransitionsText), EditorGUIUtility.currentViewWidth);
#endif
        }
    }
}
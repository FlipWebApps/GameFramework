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
using GameFramework.GameStructure.Game.Editor.GameConditions.Common;
using GameFramework.GameStructure.Game.GameConditions.GameItem;

namespace GameFramework.GameStructure.Game.Editor.GameConditions.GameItem
{
    [CustomEditor(typeof(GameConditionGameItemMatchesGameItem), true)]
    public class GameItemMatchesGameItemGameConditionEditor : GameConditionEditor
    {
        SerializedProperty _gameItemTypeProperty;
        SerializedProperty _contextProperty;
        SerializedProperty _secondContextProperty;

        /// <summary>
        /// Override this method if you need to do any specific initialisation for the ActionEditor implementation.
        /// </summary>
        protected override void Initialise()
        {
            _gameItemTypeProperty = serializedObject.FindProperty("_gameItemType");
            _contextProperty = serializedObject.FindProperty("_context");
            _secondContextProperty = serializedObject.FindProperty("_secondContext");

        }


        /// <summary>
        /// This function can be overridden by inheriting classes, but if it isn't, draw the default for it's properties.
        /// </summary>
        protected override void DrawGUI()
        {
            EditorGUILayout.PropertyField(_gameItemTypeProperty);
            EditorGUILayout.LabelField(new GUIContent("First GameItem", "The first GameItem to compare"), EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_contextProperty);
            EditorGUILayout.LabelField(new GUIContent("Second GameItem", "The second GameItem to compare"), EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_secondContextProperty, new GUIContent("Second GameItem", "The second GameItem to compare"));
        }
    }
}

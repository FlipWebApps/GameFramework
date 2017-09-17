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

using GameFramework.GameStructure.Game.ObjectModel;
using GameFramework.GameStructure.Game.ObjectModel.Abstract;
using UnityEditor;
using UnityEngine;

namespace GameFramework.GameStructure.Game.Editor.GameActions
{
    [CustomEditor(typeof(GameActionGameItemContextSelectableTypeCounter), true)]
    public class GameActionGameItemContextSelectedTypeCounterEditor : GameActionEditor
    {
        SerializedProperty _gameItemTypeProperty;
        SerializedProperty _contextProperty;
        SerializedProperty _counterProperty;
        SerializedProperty _intAmountProperty;
        SerializedProperty _floatAmountProperty;

        int _counterIndex;
        protected CounterConfiguration CounterConfiguration;

        /// <summary>
        /// Override this method if you need to do any specific initialisation for the ActionEditor implementation.
        /// </summary>
        protected override void Initialise()
        {
            _gameItemTypeProperty = serializedObject.FindProperty("_gameItemType");
            _contextProperty = serializedObject.FindProperty("_context");
            _counterProperty = serializedObject.FindProperty("_counter");
            _intAmountProperty = serializedObject.FindProperty("_intAmount");
            _floatAmountProperty = serializedObject.FindProperty("_floatAmount");
        }


        /// <summary>
        /// This function can be overridden by inheriting classes, but if it isn't, draw the default for it's properties.
        /// </summary>
        protected override void DrawGUI()
        {
            EditorGUILayout.PropertyField(DelayProperty);
            EditorGUILayout.PropertyField(_gameItemTypeProperty);
            EditorGUILayout.PropertyField(_contextProperty);

            var counterConfiguration = GameConfiguration.Instance.GetCounterConfiguration((GameConfiguration.GameItemType)_gameItemTypeProperty.enumValueIndex);
            var counters = new string[counterConfiguration.Count];
            _counterIndex = 0; // initialise incase gameitem type changed
            for (int i = 0; i < counterConfiguration.Count; i++)
            {
                counters[i] = counterConfiguration[i].Name;
                if (counters[i] == _counterProperty.stringValue)
                {
                    _counterIndex = i;
                }
            }

            int newIndex = EditorGUILayout.Popup("Counter", _counterIndex, counters);
            if (newIndex != _counterIndex || string.IsNullOrEmpty(_counterProperty.stringValue))
            {
                _counterProperty.stringValue = counters[newIndex];
                _counterIndex = newIndex;
            }
            CounterConfiguration = counterConfiguration[_counterIndex];

            EditorGUILayout.BeginHorizontal();
            if (CounterConfiguration.CounterType == CounterConfiguration.CounterTypeEnum.Int)
                EditorGUILayout.PropertyField(_intAmountProperty, new GUIContent("Amount", DelayProperty.tooltip), GUILayout.ExpandWidth(true));
            else
                EditorGUILayout.PropertyField(_floatAmountProperty, new GUIContent("Amount", DelayProperty.tooltip),  GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();
        }
    }
}

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
using GameFramework.GameStructure.Game.ObjectModel;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameFramework.GameStructure.GameItems.Editor.AbstractClasses
{
    public abstract class ShowGameItemCounterEditor : UnityEditor.Editor
    {
        SerializedProperty _contextProperty;
        SerializedProperty _textProperty;
        SerializedProperty _counterProperty;

        string[] _counters;
        int _counterIndex;
        Rect _helpRect;


        public void OnEnable()
        {
            _contextProperty = serializedObject.FindProperty("_context");
            _textProperty = serializedObject.FindProperty("_text");
            _counterProperty = serializedObject.FindProperty("_counter");

            var counterConfiguration = GetCounterConfiguration();
            _counters = new string[counterConfiguration.Count];
            for (int i = 0; i < counterConfiguration.Count; i++)
            {
                _counters[i] = counterConfiguration[i].Name;
                if (_counters[i] == _counterProperty.stringValue)
                    _counterIndex = i;
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            _helpRect = EditorHelper.ShowHideableHelpBox("GameFramework.GameStructure.ShowGameItemCounterEditor", "Use this component to display values from a built, in our your own custom counter for the given GameItem context.\nYou can setup your own counters by adding a Game Configuration to your Resources folder (named GameConfiguration).\nSee the Text tooltip for help on text format parameters.", _helpRect);

            EditorGUILayout.PropertyField(_contextProperty);

            int newIndex = EditorGUILayout.Popup("Counter", _counterIndex, _counters);
            if (newIndex != _counterIndex || string.IsNullOrEmpty(_counterProperty.stringValue))
            {
                _counterProperty.stringValue = _counters[newIndex];
                _counterIndex = newIndex;
            }

            EditorGUILayout.PropertyField(_textProperty);

            serializedObject.ApplyModifiedProperties();
        }


        /// <summary>
        /// Override in subclasses to return a list of custom counter configuration entries that should be used
        /// </summary>
        /// <returns></returns>
        public virtual List<CounterConfiguration> GetCounterConfiguration()
        {
            return GameConfiguration.Instance.DefaultGameItemCounterConfiguration;
        }
    }
}

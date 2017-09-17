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
using GameFramework.GameStructure.GameItems.Components;
using UnityEditor;
using UnityEngine;

namespace GameFramework.GameStructure.GameItems.Editor.AbstractClasses
{
    [CustomEditor(typeof(ChangeCounterOverTime))]
    public class ChangeCounterOverTimeEditor : GameItemContextBaseRunnableCounterEditor
    {
        SerializedProperty _onlyWhenLevelRunningtProperty;
        SerializedProperty _intAmountProperty;
        SerializedProperty _floatAmountProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            _onlyWhenLevelRunningtProperty = serializedObject.FindProperty("_onlyWhenLevelRunning");
            _intAmountProperty = serializedObject.FindProperty("_intAmount");
            _floatAmountProperty = serializedObject.FindProperty("_floatAmount");
        }

        /// <summary>
        /// Show GUI elements before context / counter.
        /// </summary>
        /// <returns></returns>
        protected override void ShowHeaderGUI()
        {
            HelpRect = EditorHelper.ShowHideableHelpBox("GameFramework.GameStructure.ChangeCounterOverTimeEditor", "Use this component to constantly change a built in or custom counter over time.\nYou can setup your own counters by adding a Game Configuration to your Resources folder (named GameConfiguration).\nSee the Text tooltip for help on text format parameters.", HelpRect);
        }


        /// <summary>
        /// Show GUI elements after context / counter.
        /// </summary>
        /// <returns></returns>
        protected override void ShowFooterGUI()
        {
            EditorGUILayout.PropertyField(_onlyWhenLevelRunningtProperty);
            if (CounterConfiguration.CounterType == Game.ObjectModel.CounterConfiguration.CounterTypeEnum.Int)
                EditorGUILayout.PropertyField(_intAmountProperty, new GUIContent("Amount", _intAmountProperty.tooltip));
            else
                EditorGUILayout.PropertyField(_floatAmountProperty, new GUIContent("Amount", _floatAmountProperty.tooltip));
        }
    }
}

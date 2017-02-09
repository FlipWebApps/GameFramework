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

using GameFramework.GameStructure.Levels.ObjectModel;
using GameFramework.GameStructure.GameItems.Editor;
using UnityEditor;

namespace GameFramework.GameStructure.Levels.Editor
{
    [CustomEditor(typeof(Level))]
    public class LevelEditor : GameItemEditor
    {
        //Level _level;
        SerializedProperty _starTotalCountProperty;
        SerializedProperty _star1TargetProperty;
        SerializedProperty _star2TargetProperty;
        SerializedProperty _star3TargetProperty;
        SerializedProperty _star4TargetProperty;
        SerializedProperty _timeTargetProperty;
        SerializedProperty _scoreTargetProperty;
        SerializedProperty _coinTargetProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            //_level = (Level)target;
            // get serialized objects so we can use attached property drawers (e.g. tooltips, ...)
            _starTotalCountProperty = serializedObject.FindProperty("_starTotalCount");
            _star1TargetProperty = serializedObject.FindProperty("_star1Target");
            _star2TargetProperty = serializedObject.FindProperty("_star2Target");
            _star3TargetProperty = serializedObject.FindProperty("_star3Target");
            _star4TargetProperty = serializedObject.FindProperty("_star4Target");
            _timeTargetProperty = serializedObject.FindProperty("_timeTarget");
            _scoreTargetProperty = serializedObject.FindProperty("_scoreTarget");
            _coinTargetProperty = serializedObject.FindProperty("_coinTarget");
        }


        protected override void DrawGUI()
        {
            EditorGUILayout.LabelField("Level", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Use these settings to provide customisation for Levels.\n\nFor automatic loading instances should be in a folder 'Resources\\Level' and named 'Level_<number>'\n\nYou can create your own Level derived classes to hold custom properties and / or code", MessageType.Info);
            DrawProperties();
            DrawLevelProperties();
        }


        protected void DrawLevelProperties()
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Level Properties", EditorStyles.boldLabel);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(_starTotalCountProperty);
            EditorGUI.indentLevel += 1;
            if (_starTotalCountProperty.intValue >= 1)
                EditorGUILayout.PropertyField(_star1TargetProperty);
            if (_starTotalCountProperty.intValue >= 2)
                EditorGUILayout.PropertyField(_star2TargetProperty);
            if (_starTotalCountProperty.intValue >= 3)
                EditorGUILayout.PropertyField(_star3TargetProperty);
            if (_starTotalCountProperty.intValue >= 4)
                EditorGUILayout.PropertyField(_star4TargetProperty);
            EditorGUI.indentLevel -= 1;
            EditorGUILayout.PropertyField(_timeTargetProperty);
            EditorGUILayout.PropertyField(_scoreTargetProperty);
            EditorGUILayout.PropertyField(_coinTargetProperty);
            EditorGUI.indentLevel -= 1;
            EditorGUILayout.EndVertical();
        }

    }
}

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

using FlipWebApps.GameFramework.Scripts.GameStructure.Levels.ObjectModel;
using UnityEditor;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Editor
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
        }


        protected override void DrawGUI()
        {
            EditorGUILayout.LabelField("Level Extension", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Use these settings to provide customisation for Levels.\n\nFor automatic loading instances should be in a folder 'Resources\\Level' and named 'Level_<number>'\n\nYou can create your own Level derived classes to hold custom properties and / or code", MessageType.Info);
            EditorGUILayout.BeginVertical("Box");
            DrawBasicProperties();
            EditorGUILayout.Space();
            DrawLevelProperties();
            EditorGUILayout.EndVertical();
        }


        protected void DrawLevelProperties()
        {
            EditorGUILayout.LabelField("Level Properties", EditorStyles.boldLabel);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(_starTotalCountProperty);
            EditorGUILayout.PropertyField(_star1TargetProperty);
            EditorGUILayout.PropertyField(_star2TargetProperty);
            EditorGUILayout.PropertyField(_star3TargetProperty);
            EditorGUILayout.PropertyField(_star4TargetProperty);
            EditorGUILayout.PropertyField(_timeTargetProperty);
            EditorGUILayout.PropertyField(_scoreTargetProperty);
            EditorGUI.indentLevel -= 1;
        }

    }
}

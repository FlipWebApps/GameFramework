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

using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;
using UnityEditor;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Editor
{
    [CustomEditor(typeof(GameItemExtension))]
    public class GameItemExtensionInspector : UnityEditor.Editor
    {
        GameItemExtension _gameItemExtension;
        SerializedProperty _giNameProperty;
        SerializedProperty _giDescriptionProperty;
        SerializedProperty _giValueToUnlockProperty;

        protected virtual void OnEnable()
        {
            _gameItemExtension = (GameItemExtension)target;
            // get serialized objects so we can use attached property drawers (e.g. tooltips, ...)
            _giNameProperty = serializedObject.FindProperty("_name");
            _giDescriptionProperty = serializedObject.FindProperty("_description");
            _giValueToUnlockProperty = serializedObject.FindProperty("_valueToUnlock");
        }

        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();
            serializedObject.Update();

            DrawGUI();
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DrawGUI()
        {
            EditorGUILayout.LabelField("Game Item Extension", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Use these settings to provide additional customisation for GameItems.\n\nThere are also Extensions for specific GameItems such as Level and Player. In addition you can create your own derived classes to hold custom properties and / or code", MessageType.Info);
            // Game Item setup
            EditorGUILayout.BeginVertical("Box");
            DrawBasicProperties();
            EditorGUILayout.EndVertical();
        }

        protected void DrawBasicProperties()
        {
            EditorGUILayout.LabelField("Basic Properties", EditorStyles.boldLabel);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(_giNameProperty);
            EditorGUILayout.PropertyField(_giDescriptionProperty);
            _gameItemExtension.OverrideValueToUnlock = EditorGUILayout.BeginToggleGroup("Override ValueToUnlock", _gameItemExtension.OverrideValueToUnlock);
            // TODO: Have a int field that allows an empty value that signifies don't override.
            //var valueToUnlock = EditorGUILayout.TextField("ValueToUnlock", _gameItemExtension.OverrideValueToUnlock ? _gameItemExtension.ValueToUnlock.ToString() : "");
            //valueToUnlock.S
            EditorGUILayout.PropertyField(_giValueToUnlockProperty);
            EditorGUILayout.EndToggleGroup();
            EditorGUI.indentLevel -= 1;
        }
    }
}

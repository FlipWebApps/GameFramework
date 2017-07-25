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
using GameFramework.GameStructure.GameItems.ObjectModel;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace GameFramework.GameStructure.Editor
{
    [CustomEditor(typeof(GameConfiguration))]
    public class GameConfigurationEditor : UnityEditor.Editor
    {
        SerializedProperty _scoreConfigurationEntryProperty;

        Rect _mainHelpRect;
        int _currentTab;

        Rect _scoresHelpRect;

        void OnEnable()
        {
            _scoreConfigurationEntryProperty = serializedObject.FindProperty("_scoreConfigurationEntry");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            _mainHelpRect = EditorHelper.ShowHideableHelpBox("GameFramework.GameStructure.GameConfigurationEditorWindow", "Add a Game Configuration to your Resources folder (named GameConfiguration) to control fixed aspects of your game.\n\nNOTE: These are seperate from the GameManager component so that they can be loaded and used independently.\n\nIf you experience any problems or have improvement suggestions then please get in contact. Your support is appreciated.", _mainHelpRect);

            // Main tabs and display
            //_currentTab = GUILayout.Toolbar(_currentTab, new string[] { "Scores" });
            //switch (_currentTab)
            //{
            //    case 0:
            DrawScores();
            //        break;
            //}

            serializedObject.ApplyModifiedProperties();
        }


        void DrawScores() 
        {
            _scoresHelpRect = EditorHelper.ShowHideableHelpBox("GameFramework.GameStructure.GameConfigurationEditorWindow.Scores", "By default GameItems such as Player, Level, etc. have support for a standard Score counter and Coins.\n\nYou can add additional 'Score' counters here that you might need in your game e.g. Gems, ... These will then be available for use in all GameItems from code or within the components that reference a score such as 'ShowScore'.", _scoresHelpRect);

            EditorGUILayout.BeginVertical("Box");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_scoreConfigurationEntryProperty, true);
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
    }
}
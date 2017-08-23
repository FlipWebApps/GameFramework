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

using GameFramework.GameStructure.Game.Editor.GameActions.Abstract;
using GameFramework.GameStructure.Game.ObjectModel;
using UnityEditor;
using UnityEngine;

namespace GameFramework.GameStructure.Game
{
    /// <summary>
    /// Helper class for GameAction Editors
    /// </summary>
    public class GameActionEditorHelper
    {
        #region SubEditors

        /// <summary>
        /// In case of changes create sub editors
        /// </summary>
        internal static GameActionEditor[] CheckAndCreateSubEditors(GameActionEditor[] subEditors, GameActionReference[] actionReferences,
            SerializedObject container, SerializedProperty _scriptableReferenceArrayProperty)
        {
            // If there are the correct number of subEditors then do nothing.
            if (subEditors != null && subEditors.Length == actionReferences.Length)
                return subEditors;

            // Otherwise get rid of any old editors.
            CleanupSubEditors(subEditors);

            // Create an array of the subEditor type that is the right length for the targets.
            subEditors = new GameActionEditor[actionReferences.Length];

            // Populate the array and setup each Editor.
            for (var i = 0; i < subEditors.Length; i++)
            {
                subEditors[i] =
                    UnityEditor.Editor.CreateEditor(actionReferences[i].ScriptableObject) as GameActionEditor;
                if (subEditors[i] != null)
                {
                    subEditors[i].Container = actionReferences[i];
                    var scriptableObjectContainer = _scriptableReferenceArrayProperty.GetArrayElementAtIndex(i);
                    subEditors[i].ContainerSerializedObject = container;
                    subEditors[i].DataProperty =
                        scriptableObjectContainer.FindPropertyRelative("_data");
                    subEditors[i].ObjectReferencesProperty =
                        scriptableObjectContainer.FindPropertyRelative("_objectReferences"); ;
                }
            }
            return subEditors;
        }

        /// <summary>
        /// Destroy all subeditors
        /// </summary>
        internal static void CleanupSubEditors(GameActionEditor[] subEditors)
        {
            if (subEditors == null) return;
            for (var i = 0; i < subEditors.Length; i++)
            {
                if (subEditors[i] != null)
                {
                    Object.DestroyImmediate(subEditors[i]);
                    subEditors[i] = null;
                }
            }
        }

        #endregion SubEditors
    }
}

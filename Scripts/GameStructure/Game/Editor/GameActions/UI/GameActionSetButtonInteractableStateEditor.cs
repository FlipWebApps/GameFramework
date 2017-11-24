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
using GameFramework.GameStructure.Game.GameActions.UI;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameFramework.GameStructure.Game.Editor.GameActions.UI
{
    [CustomEditor(typeof(GameActionSetButtonInteractableState))]
    public class GameActionSetButtonInteractableStateEditor : GameActionEditor
    {
        /// <summary>
        /// Draw the Editor GUI
        /// </summary>
        protected override void DrawGUI()
        {
#if BEAUTIFUL_TRANSITIONS
            HideableHelpRect = EditorHelper.ShowHideableHelpBox("GameFramework.GameStructure.SetButtonInteractableStateGameActionEditor", "To animate state changes you need to have an animation added to your button's GameObject that uses the Beautiful Transitions DisplayItem Animation Controller as a base. See the Beautiful Transitions Display Item demo and online help for further details.", HideableHelpRect);
#else
            EditorGUILayout.HelpBox("Make your game more professional by adding the Beautiful Transitions asset to animate Button Interactable State changes. See the Menu | Window | Game Framework | Integrations Window for more information.", MessageType.Warning);
#endif

            EditorHelper.DrawProperties(serializedObject, new List<string>() { "_delay" });
            ShowTargetTypeProperty(TargetTypeProperty, TargetProperty, "Target");
            EditorHelper.DrawProperties(serializedObject, new List<string>() { "_interactable" });

#if !BEAUTIFUL_TRANSITIONS
            GUI.enabled = false;
#endif
            EditorHelper.DrawProperties(serializedObject, new List<string>() { "_animateChanges" });
            GUI.enabled = true;
        }
    }
}

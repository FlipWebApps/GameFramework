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

using GameFramework.GameObjects;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameFramework.UI.Dialogs.Components
{
    /// <summary>
    /// Call back that will show the specified dialog buttons. This might typically be triggered from an 
    /// animation to only show buttons dialog after an animation is shown. This can be used to stop the 
    /// user clicking and exiting a dialog before we have shown what we want to show.
    /// </summary>
    [AddComponentMenu("Game Framework/UI/Dialogs/DialogCallbackShowButtons")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/ui/dialogs/")]
    public class DialogCallbackShowButtons : MonoBehaviour
    {
        public DialogInstance.DialogButtonsType Buttons = DialogInstance.DialogButtonsType.Ok;

        /// <summary>
        /// Method that you should invoke to display the dialog buttons.
        /// </summary>
        public void ShowDialogButtons()
        {
            Assert.AreEqual(DialogInstance.DialogButtonsType.Ok, Buttons, "Currently only Ok button is supported.");

            var panelGameobject = GameObjectHelper.GetParentNamedGameObject(gameObject, "Panel");
            Assert.IsNotNull(panelGameobject, "A Dialog must follow certain naming conventions. Missing Panel.");
            GameObjectHelper.GetChildNamedGameObject(panelGameobject, "OkButton", true).SetActive(true);
        }
    }
}
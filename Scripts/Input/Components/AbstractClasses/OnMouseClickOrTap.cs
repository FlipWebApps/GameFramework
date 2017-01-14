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

using System.Collections.Generic;
using System.Linq;
using GameFramework.UI.Dialogs.Components;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFramework.Input.Components.AbstractClasses
{
    /// <summary>
    /// Abstract class that calls a method when a mouse button is pressed or the screen is tapped anywhere on the screen
    /// </summary>
    /// By setting a list of UI game objects or UI gameobjects you can set areas that will block the input.
    public abstract class OnMouseClickOrTap : MonoBehaviour
    {
        /// <summary>
        /// A list of gameobjects or UI gameobjects that will block input to this component.
        /// </summary>
        [Tooltip("A list of gameobjects or UI gameobjects that will block input to this component.")]
        public List<GameObject> BlockingGameObjects;

        void Update()
        {
            if (!UnityEngine.Input.GetMouseButtonDown(0)) return;                       // don't allow if click / tap not active
            if (DialogManager.IsActive && DialogManager.Instance.Count > 0) return;     // don't allow if popup dialog shown.

            // check agains blocking UI? game objects
            if (BlockingGameObjects != null && EventSystem.current != null)
            {
                var pe = new PointerEventData(EventSystem.current)
                {
                    position = UnityEngine.Input.mousePosition
                };

                var hits = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pe, hits);

                if (hits.Any(hit => BlockingGameObjects.Contains(hit.gameObject)))
                {
                    return;
                }
            }

            // if we got here then run the custom method
            RunMethod();
        }

        /// <summary>
        /// Run method that you should implement in your own derived classes that will be called when a valid click is detected
        /// </summary>
        public abstract void RunMethod();
    }
}
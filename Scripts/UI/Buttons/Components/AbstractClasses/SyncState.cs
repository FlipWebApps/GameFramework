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

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameFramework.UI.Buttons.Components.AbstractClasses
{
    /// <summary>
    /// Abstract base class that when added to a gameobject with a UI Button will monitor for state changes and
    /// call the implementing classes StateChanged method so it can update the display.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public abstract class SyncState : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IPointerDownHandler, IPointerUpHandler
    {
        Button _button;

        /// <summary>
        /// Indicates if the button is interactable
        /// </summary>
        protected bool IsInteractable;

        /// <summary>
        /// Indicates if the pointer is over the button
        /// </summary>
        protected bool IsPointerOver;

        /// <summary>
        /// Indicates if the pointer is pressed
        /// </summary>
        protected bool IsPointerDown;

        /// <summary>
        /// Indicates if the button is selected
        /// </summary>
        protected bool IsSelected;

        public void Awake()
        {
            _button = GetComponent<Button>();
            IsInteractable = _button.interactable;

            StateChanged();
        }

        /// <summary>
        /// Check for changes to the buttons interactable state.
        /// </summary>
        void Update()
        {
            if (_button.interactable == IsInteractable) return;

            IsInteractable = _button.interactable;
            StateChanged();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            IsPointerOver = true;
            StateChanged();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsPointerOver = false;
            StateChanged();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            IsPointerDown = true;
            StateChanged();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsPointerDown = false;
            StateChanged();
        }

        public void OnSelect(BaseEventData eventData)
        {
            IsSelected = true;
            StateChanged();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            IsSelected = false;
            StateChanged();
        }

        /// <summary>
        /// Implement this in derived classes to be notified when the button state changes so you can 
        /// react accordingly.
        /// </summary>
        public abstract void StateChanged();
    }
}
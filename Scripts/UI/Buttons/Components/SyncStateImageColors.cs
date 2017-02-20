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

using GameFramework.UI.Buttons.Components.AbstractClasses;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace GameFramework.UI.Buttons.Components
{
    /// <summary>
    /// Syncronises UI Image colors agains changing button states. This can be used where you want button 
    /// state changes to be reflected across multiple images such as when you have more complex buttons 
    /// composed of multiple or child images.
    /// </summary>
    [AddComponentMenu("Game Framework/UI/Buttons/SyncStateImageColors")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/ui/")]
    public class SyncStateImageColors : SyncState
    {
        /// <summary>
        /// A color to set any images to when the button is disabled.
        /// </summary>
        [Tooltip("A color to set any images to when the button is disabled.")]
        public Color DisabledColor;

        /// <summary>
        /// A color to set any images to when the button is highlighted.
        /// </summary>
        [Tooltip("A color to set any images to when the button is highlighted.")]
        public Color HighlightedColor;

        /// <summary>
        /// A color to set any images to when the button is pressed.
        /// </summary>
        [Tooltip("A color to set any images to when the button is pressed.")]
        public Color PressedColor;

        /// <summary>
        /// An array of UI Image components to synchronise with the button.
        /// </summary>
        [Tooltip("An array of images to synchronise with the button.")]
        public Image[] Images;

        Color _normalColor;

        new void Awake()
        {
            Assert.AreNotEqual(0, Images.Length, "Please specify the images that you would like to sync the button state with.");

            _normalColor = Images[0].color;

            base.Awake();
        }

        /// <summary>
        /// Handle state changes and update image colours accordingly
        /// </summary>
        public override void StateChanged()
        {
            Color imageColor;

            if (IsInteractable)
            {
                if (IsPointerOver || IsSelected)
                {
                    if (IsPointerOver && IsPointerDown)
                    {
                        imageColor = PressedColor;
                    }
                    else
                    {
                        imageColor = HighlightedColor;
                    }
                }
                else
                {
                    imageColor = _normalColor;
                }
            }
            else
            {
                imageColor = DisabledColor;
            }

            // update colours
            foreach (var image in Images)
            {
                image.color = imageColor;
            }
        }
    }
}
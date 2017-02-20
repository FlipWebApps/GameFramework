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
    /// Syncronises UI text, shadows and outline colors agains changing button states. This can be used 
    /// where you want button state changes to be reflected across multiple Text components such as 
    /// when you have more complex buttons composed of multiple or child Text components.
    /// </summary>
    [RequireComponent(typeof(Button))]
    [AddComponentMenu("Game Framework/UI/Buttons/SyncStateTextColors")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/ui/")]
    public class SyncStateTextColors : SyncState
    {
        /// <summary>
        /// A color to set any texts to when the button is disabled.
        /// </summary>
        [Header("Text Color")]
        [Tooltip("A color to set any texts to when the button is disabled.")]
        public Color DisabledTextColor;

        /// <summary>
        /// A color to set any texts to when the button is highlighted.
        /// </summary>
        [Tooltip("A color to set any texts to when the button is disabled.")]
        public Color HighlightedTextColor;

        /// <summary>
        /// A color to set any texts to when the button is pressed.
        /// </summary>
        [Tooltip("A color to set any texts to when the button is disabled.")]
        public Color PressedTextColor;

        /// <summary>
        /// A color to set any shadows to when the button is disabled.
        /// </summary>
        [Header("Shadow Color")]
        [Tooltip("A color to set any shadows to when the button is disabled.")]
        public Color DisabledShadowColor;

        /// <summary>
        /// A color to set any shadows to when the button is highlighted.
        /// </summary>
        [Tooltip("A color to set any shadows to when the button is disabled.")]
        public Color HighlightedShadowColor;

        /// <summary>
        /// A color to set any shadows to when the button is pressed.
        /// </summary>
        [Tooltip("A color to set any shadows to when the button is disabled.")]
        public Color PressedShadowColor;

        /// <summary>
        /// A color to set any outlines to when the button is disabled.
        /// </summary>
        [Header("Outline Color")]
        [Tooltip("A color to set any outlines to when the button is disabled.")]
        public Color DisabledOutlineColor;

        /// <summary>
        /// A color to set any outlines to when the button is highlighted.
        /// </summary>
        [Tooltip("A color to set any outlines to when the button is highlighted.")]
        public Color HighlightedOutlineColor;

        /// <summary>
        /// A color to set any outlines to when the button is pressed.
        /// </summary>
        [Tooltip("A color to set any outlines to when the button is pressed.")]
        public Color PressedOutlineColor;


        /// <summary>
        /// An array of UI Text components to synchronise with the button.
        /// </summary>
        [Tooltip("An array of UI Text components to synchronise with the button.")]
        Text[] _texts;

        /// <summary>
        /// An array of UI Shadow components to synchronise with the button.
        /// </summary>
        [Tooltip("An array of UI Shadow components to synchronise with the button.")]
        Shadow[] _shadows;

        /// <summary>
        /// An array of UI Outline components to synchronise with the button.
        /// </summary>
        [Tooltip("An array of UI Outline components to synchronise with the button.")]
        Outline[] _outlines;

        Color _normalTextColor;
        Color _normalShadowColor;
        Color _normalOutlineColor;

        new void Awake()
        {
            _texts = GetComponentsInChildren<Text>(true);
            Assert.AreNotEqual(0, _texts.Length, "No child Text components found.");
            _normalTextColor = _texts[0].color;

            _shadows = GetComponentsInChildren<Shadow>(true);
            if (_shadows.Length > 0)
                _normalShadowColor = _shadows[0].effectColor;

            _outlines = GetComponentsInChildren<Outline>(true);
            if (_shadows.Length > 0)
                _normalOutlineColor = _shadows[0].effectColor;

            base.Awake();
        }


        /// <summary>
        /// Handle state changes and update text, shadows and outline colours accordingly
        /// </summary>
        public override void StateChanged()
        {
            // determin colour based upon state.
            Color textColor;
            Color shadowColor;
            Color outlineColor;
            if (IsInteractable)
            {
                if (IsPointerOver || IsSelected)
                {
                    if (IsPointerOver &&IsPointerDown)
                    {
                        textColor = PressedTextColor;
                        shadowColor = PressedShadowColor;
                        outlineColor = PressedOutlineColor;
                    }
                    else
                    {
                        textColor = HighlightedTextColor;
                        shadowColor = HighlightedShadowColor;
                        outlineColor = HighlightedOutlineColor;
                    }
                }
                else
                {
                    textColor = _normalTextColor;
                    shadowColor = _normalShadowColor;
                    outlineColor = _normalOutlineColor;
                }
            }
            else
            {
                textColor = DisabledTextColor;
                shadowColor = DisabledShadowColor;
                outlineColor = DisabledOutlineColor;
            }

            // update colours
            foreach (var text in _texts)
            {
                text.color = textColor;
            }
            foreach (var shadow in _shadows)
            {
                shadow.effectColor = shadowColor;
            }
            foreach (var outline in _outlines)
            {
                outline.effectColor = outlineColor;
            }
        }
    }
}
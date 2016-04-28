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
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FlipWebApps.GameFramework.Scripts.UI.Buttons.Components
{
    [RequireComponent(typeof(Button))]
    [AddComponentMenu("Game Framework/UI/Buttons/SyncStateTextColors")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class SyncStateTextColors : SyncState
    {
        [Header("Text Color")]
        public Color DisabledTextColor;
        public Color HighlightedTextColor;
        public Color PressedTextColor;

        [Header("Shadow Color")]
        public Color DisabledShadowColor;
        public Color HighlightedShadowColor;
        public Color PressedShadowColor;

        [Header("Outline Color")]
        public Color DisabledOutlineColor;
        public Color HighlightedOutlineColor;
        public Color PressedOutlineColor;

        Color _normalTextColor;
        Color _normalShadowColor;
        Color _normalOutlineColor;

        Text[] _texts;
        Shadow[] _shadows;
        Outline[] _outlines;

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
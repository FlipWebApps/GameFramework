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

using GameFramework.GameStructure.GameItems.Components.AbstractClasses;
using UnityEngine;

namespace GameFramework.GameStructure.GameItems.Components
{
    /// <summary>
    /// Set Sprite color from a counter from the specified GameItem
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    [AddComponentMenu("Game Framework/GameStructure/Common/Set SpriteRenderer Color From Counter")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/game-structure/")]
    public class SetSpriteRendererColorFromCounter : SetFromCounterNormalisedAmount<SpriteRenderer>
    {
        /// <summary>
        /// A gradient to use to set the SpriteRenderer color based upon the specified counter amount relative to min / max.
        /// </summary>
        public Gradient Gradient
        {
            get { return _gradient; }
            set { _gradient = value; }
        }
        [Tooltip("A gradient to use to set the SpriteRenderer color based upon the specified counter amount relative to min / max.")]
        [SerializeField]
        Gradient _gradient;

        /// <summary>
        /// Assigns the amount to the target component.
        /// </summary>
        /// <returns></returns>
        protected override void AssignAmount(SpriteRenderer component, float normalisedAmount)
        {
            component.color = Gradient.Evaluate(normalisedAmount);
        }
    }
}
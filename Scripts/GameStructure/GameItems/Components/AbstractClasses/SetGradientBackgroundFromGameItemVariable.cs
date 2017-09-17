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

using GameFramework.Display.Other.Components;
using GameFramework.GameStructure.GameItems.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameFramework.GameStructure.GameItems.Components.AbstractClasses
{
    /// <summary>
    /// Set gradient background colors from GameItem variables
    /// </summary>
    [RequireComponent(typeof(GradientBackground))]
    public abstract class SetGradientBackgroundFromGameItemVariable<T> : GameItemContextBaseRunnable<T> where T : GameItem
    {

        /// <summary>
        /// The tag of a color variable to set the top color from.
        /// </summary>
        public string TagTopColor
        {
            get { return _tagTopColor; }
            set { _tagTopColor = value; }
        }
        [Tooltip("The tag of a color variable to set the top color from.")]
        [SerializeField]
        string _tagTopColor;

        /// <summary>
        /// The tag of a color variable to set the bottom color from.
        /// </summary>
        public string TagBottomColor
        {
            get { return _tagBottomColor; }
            set { _tagBottomColor = value; }
        }
        [Tooltip("The tag of a color variable to set the bottom color from.")]
        [SerializeField]
        string _tagBottomColor;

        GradientBackground _gradientBackground;

        protected override void Awake()
        {
            _gradientBackground = GetComponent<GradientBackground>();
            base.Awake();
        }

        /// <summary>
        /// Called by the base class from start and optionally if the selection chages.
        /// </summary>
        /// <param name="isStart"></param>
        public override void RunMethod(bool isStart = true)
        {
            var topColorVariable = GameItem.Variables.GetColor(TagTopColor);
            var bottomColorVariable = GameItem.Variables.GetColor(TagBottomColor);
            Assert.IsNotNull(topColorVariable, string.Format("A color variable with tag '{0}' was not found. Please check that it exists.", topColorVariable));
            Assert.IsNotNull(bottomColorVariable, string.Format("A color variable with tag '{0}' was not found. Please check that it exists.", bottomColorVariable));
            _gradientBackground.TopColor = topColorVariable.Value;
            _gradientBackground.BottomColor = bottomColorVariable.Value;
            _gradientBackground.DisplayGradient();
        }
    }
}
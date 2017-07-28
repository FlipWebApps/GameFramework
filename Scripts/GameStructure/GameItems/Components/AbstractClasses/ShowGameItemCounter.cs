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

using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.Localisation.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace GameFramework.GameStructure.GameItems.Components.AbstractClasses
{
    /// <summary>
    /// Show a counter from the specified GameItem
    /// </summary>
    [RequireComponent(typeof(Text))]
    public abstract class ShowGameItemCounter<T> : GameItemContextBaseRunnable<T> where T : GameItem
    {
        /// <summary>
        /// A localisation key or text string to use to display the counter. Use the placeholder {0} to identify where in teh string the counter should be placed.
        /// </summary>
        [Tooltip("A localisation key or text string to use to display the counter. Use the placeholder {0} to identify where in teh string the counter should be placed.")]
        public LocalisableText Text;

        /// <summary>
        /// The counter that we want to display.
        /// </summary>
        [Tooltip("The counter that we want to display.")]
        public string Counter;

        Text _textComponent;
        int _counterIndex;

        protected override void Awake()
        {
            base.Awake();
            _textComponent = GetComponent<Text>();
            _counterIndex = GameItem.GetCounterIndex(Counter);

            Assert.AreNotEqual(-1, _counterIndex, string.Format("The specified Counter '{0}' was not found. Check that is exists in the game configuration.", Counter));
        }

        /// <summary>
        /// You should implement this method which is called from start and optionally if the selection chages.
        /// </summary>
        /// <param name="isStart"></param>
        public override void RunMethod(bool isStart = true)
        {
            if (GameItem != null)
            {
                _textComponent.text = Text.FormatValue(GameItem.GetCounterInt(_counterIndex));
            }
        }
    }
}
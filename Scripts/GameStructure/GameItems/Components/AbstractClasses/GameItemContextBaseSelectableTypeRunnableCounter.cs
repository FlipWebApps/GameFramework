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

using GameFramework.GameStructure.GameItems.Messages;
using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.Messaging;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameFramework.GameStructure.GameItems.Components.AbstractClasses
{
    /// <summary>
    /// Abstract base class to show a counter from the specified GameItem context and user selected type.
    /// </summary>
    public abstract class GameItemContextBaseSelectableTypeRunnableCounter : GameItemContextBaseSelectableTypeRunnable
    {
        /// <summary>
        /// The counter that we want to use.
        /// </summary>
        public string Counter
        {
            get { return _counter; }
            set { _counter = value; }
        }
        [Tooltip("The counter that we want to use.")]
        [SerializeField]
        string _counter;

        protected Counter CounterReference;

        /// <summary>
        /// Awake - if you override then call through to the base class 
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            CounterReference = GameItem.GetCounter(Counter);
            Assert.IsNotNull(CounterReference, string.Format("The specified Counter '{0}' was not found. Check that is exists in the game configuration.", Counter));
        }

        /// <summary>
        /// OnEnable - if you override then call through to the base class 
        /// </summary>
        protected virtual void OnEnable()
        {
            Assert.IsTrue(GameManager.IsActive, string.Format("Please ensure that you have a GameManager added to your scene to use the {0} component.", GetType().Name));

            if (CounterReference.Configuration.CounterType == Game.ObjectModel.CounterConfiguration.CounterTypeEnum.Int)
            {
                GameManager.SafeAddListener<CounterIntAmountChangedMessage>(CounterIntChangedHandler);
            }
            else
            {
                GameManager.SafeAddListener<CounterFloatAmountChangedMessage>(CounterFloatChangedHandler);
            }
        }

        /// <summary>
        /// OnDisable - if you override then call through to the base class 
        /// </summary>
        protected virtual void OnDisable()
        {
            if (CounterReference.Configuration.CounterType == Game.ObjectModel.CounterConfiguration.CounterTypeEnum.Int)
            {
                GameManager.SafeRemoveListener<CounterIntAmountChangedMessage>(CounterIntChangedHandler);
            }
            else
            {
                GameManager.SafeRemoveListener<CounterFloatAmountChangedMessage>(CounterFloatChangedHandler);
            }
        }


        /// <summary>
        /// Called when a counter is changed - for now we just use the counters latest value.
        /// </summary>
        /// Verifies that this refers to the referenced counter on the specified GameItem
        /// <param name="message"></param>
        /// <returns></returns>
        bool CounterIntChangedHandler(BaseMessage message)
        {
            var counterIntAmountChangedMessage = message as CounterIntAmountChangedMessage;
            if (counterIntAmountChangedMessage.GameItem == GameItem && 
                counterIntAmountChangedMessage.Counter.Identifier == CounterReference.Identifier)
                RunMethod(false);
            return true;
        }


        /// <summary>
        /// Called when a counter is changed.
        /// </summary>
        /// Verifies that this refers to the referenced counter on the specified GameItem
        /// <param name="message"></param>
        /// <returns></returns>
        bool CounterFloatChangedHandler(BaseMessage message)
        {
            var counterIntAmountChangedMessage = message as CounterFloatAmountChangedMessage;
            if (counterIntAmountChangedMessage.GameItem == GameItem &&
                counterIntAmountChangedMessage.Counter.Identifier == CounterReference.Identifier)
                RunMethod(false);
            return true;
        }
    }
}
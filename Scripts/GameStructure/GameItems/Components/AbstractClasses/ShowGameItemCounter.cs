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
using GameFramework.Localisation.Messages;
using GameFramework.Localisation.ObjectModel;
using GameFramework.Messaging;
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
        public LocalisableText Text
        {
            get { return _text; }
            set { _text = value;}
        }
        [Tooltip("A localisation key or text string to use to display the counter. Use the following optional placeholders in the string:\n {0} - The current amount\n {1} - The best amount\n {2} - The last saved amount\n {3} - The last saved best amount\ne.g. \"Score {0}, Best {1}\"\nGoogle \".net string format\" for further options.")]
        [SerializeField]
        LocalisableText _text;

        /// <summary>
        /// The counter that we want to display.
        /// </summary>
        public string Counter
        {
            get { return _counter; }
            set { _counter = value; }
        }
        [Tooltip("The counter that we want to display.")]
        [SerializeField]
        string _counter;

        Text _textComponent;
        Counter _counterReference;
        string _cachedText;

        protected override void Awake()
        {
            base.Awake();
            _textComponent = GetComponent<Text>();
            _counterReference = GameItem.GetCounter(Counter);
            CacheTextValue();

            Assert.IsNotNull(_counterReference, string.Format("The specified Counter '{0}' was not found. Check that is exists in the game configuration.", Counter));
        }

        private void OnEnable()
        {
            Assert.IsTrue(GameManager.IsActive, "Please ensure that you have a GameManager added to your scene to use the ShowXxxCounter components.");

            if (_counterReference.Configuration.CounterType == Game.ObjectModel.CounterConfiguration.CounterTypeEnum.Int)
            {
                GameManager.SafeAddListener<CounterIntAmountChangedMessage>(CounterChangedHandler);
            }
            else
            {
                GameManager.SafeAddListener<CounterFloatAmountChangedMessage>(CounterChangedHandler);
            }
            GameManager.SafeAddListener<LocalisationChangedMessage>(LocalisationChangedHandler);
        }

        private void OnDisable()
        {
            GameManager.SafeRemoveListener<LocalisationChangedMessage>(LocalisationChangedHandler);
            if (_counterReference.Configuration.CounterType == Game.ObjectModel.CounterConfiguration.CounterTypeEnum.Int)
            {
                GameManager.SafeRemoveListener<CounterIntAmountChangedMessage>(CounterChangedHandler);
            }
            else
            {
                GameManager.SafeRemoveListener<CounterFloatAmountChangedMessage>(CounterChangedHandler);
            }
        }


        /// <summary>
        /// Called when a counter is changed - for now we just use the counters latest value.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool CounterChangedHandler(BaseMessage message)
        {
            RunMethod(false);
            return true;
        }


        /// <summary>
        /// Called when the localisation changes and updates the cached text string to display.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool LocalisationChangedHandler(BaseMessage message)
        {
            CacheTextValue();
            RunMethod(false);
            return true;
        }

        private void CacheTextValue()
        {
            _cachedText = Text.GetValue();
            if (_cachedText != null && _cachedText.Length == 0) _cachedText = null;
        }

        /// <summary>
        /// You should implement this method which is called from start and optionally if the selection chages.
        /// </summary>
        /// <param name="isStart"></param>
        public override void RunMethod(bool isStart = true)
        {
            if (GameItem != null)
            {
                if (_counterReference.Configuration.CounterType == Game.ObjectModel.CounterConfiguration.CounterTypeEnum.Int)
                {
                    _textComponent.text = _cachedText == null ? _counterReference.IntAmount.ToString() :
                        string.Format(_cachedText, _counterReference.IntAmount, _counterReference.IntAmountBest, _counterReference.IntAmountSaved, _counterReference.IntAmountBestSaved);
                }
                else
                {
                    _textComponent.text = _cachedText == null ? _counterReference.FloatAmount.ToString("n2") :
                        string.Format(_cachedText, _counterReference.FloatAmount, _counterReference.FloatAmountBest, _counterReference.FloatAmountSaved, _counterReference.FloatAmountBestSaved);
                }
            }
        }
    }
}
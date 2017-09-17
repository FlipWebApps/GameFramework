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

using GameFramework.Localisation.Messages;
using GameFramework.Localisation.ObjectModel;
using GameFramework.Messaging;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.GameStructure.GameItems.Components.AbstractClasses
{
    /// <summary>
    /// Show a counter from the specified GameItem
    /// </summary>
    /// Includes performance enhancements to cache localised text and update on localisation changes
    [RequireComponent(typeof(Text))]
    public abstract class ShowGameItemCounter : GameItemContextBaseSelectableTypeRunnableCounter
    {
        /// <summary>
        /// A localisation key or text string to use to display the counter. Use the following optional placeholders in the string:\n {0} - The current amount\n {1} - The best amount\n {2} - The last saved amount\n {3} - The last saved best amount\n {4} - The minimum allowed amount\n {5} - The maximum allowed amount\n {6} - Amount as a percentage between min and max\ne.g. \"Score {0}, Best {1}\"\nGoogle \".net string format\" for further options.
        /// </summary>
        public LocalisableText Text
        {
            get { return _text; }
            set { _text = value;}
        }
        [Tooltip("A localisation key or text string to use to display the counter. Use the following optional placeholders in the string:\n {0} - The current amount\n {1} - The best amount\n {2} - The last saved amount\n {3} - The last saved best amount\n {4} - The minimum allowed amount\n {5} - The maximum allowed amount\n {6} - Amount as a percentage between min and max\ne.g. \"Score {0}, Best {1}\"\nGoogle \".net string format\" for further options.")]
        [SerializeField]
        LocalisableText _text;

        Text _textComponent;
        string _cachedText;

        // set defaults from constructor.
        public ShowGameItemCounter()
        {
            _text = LocalisableText.CreateNonLocalised("{0}");
        }

        protected override void Awake()
        {
            base.Awake();
            _textComponent = GetComponent<Text>();
            CacheTextValue();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            GameManager.SafeAddListener<LocalisationChangedMessage>(LocalisationChangedHandler);
        }

        protected override void OnDisable()
        {
            GameManager.SafeRemoveListener<LocalisationChangedMessage>(LocalisationChangedHandler);
            base.OnDisable();
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
                if (CounterReference.Configuration.CounterType == Game.ObjectModel.CounterConfiguration.CounterTypeEnum.Int)
                {
                    _textComponent.text = _cachedText == null ? CounterReference.IntAmount.ToString() :
                        string.Format(_cachedText, CounterReference.IntAmount, CounterReference.IntAmountBest, 
                        CounterReference.IntAmountSaved, CounterReference.IntAmountBestSaved,
                        CounterReference.Configuration.IntMinimum, CounterReference.Configuration.IntMaximum,
                        CounterReference.GetAsPercent());
                }
                else
                {
                    _textComponent.text = _cachedText == null ? CounterReference.FloatAmount.ToString("n2") :
                        string.Format(_cachedText, CounterReference.FloatAmount, CounterReference.FloatAmountBest, 
                        CounterReference.FloatAmountSaved, CounterReference.FloatAmountBestSaved,
                        CounterReference.Configuration.FloatMinimum, CounterReference.Configuration.FloatMaximum,
                        CounterReference.GetAsPercent());
                }
            }
        }
    }
}
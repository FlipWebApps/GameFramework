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

using GameFramework.Localisation;
using System;
using System.Collections.Generic;
using GameFramework.Messaging;
using GameFramework.Messaging.Components.AbstractClasses;
using UnityEngine;
using UnityEngine.Assertions;
using GameFramework.GameStructure;
using GameFramework.Localisation.Messages;

namespace GameFramework.UI.Other.Components.AbstractClasses
{
    /// <summary>
    /// An abstract class that runs updates a value in an optional animated fashion.
    /// 
    /// Override and implement the condition as you best see fit.
    /// </summary>
    public abstract class ShowValueAnimatedMessaging<T, TM> : RunOnMessage<TM> where T : IComparable<T> where TM : BaseMessage
    {

        [Tooltip("A text component to update. Not needed if on the same gameobject as this component.")]
        public UnityEngine.UI.Text Text;
        [Tooltip("Optional localisation key. If not specified then the value is just displayed directly.\n\nIn the text, {0} is substituted with the actual value")]
        public string LocalisationKey;

        [Header("Optional Animation Settings")]
        [Tooltip("An animator with Increased and Decreased triggers to set when the value changes. See GameFramework\\Animations\\UI\\ShowValueAnimated for an example.")]
        public Animator Animator;
        [Tooltip("How value changes that occur while an animation is already running are handled.")]
        public UpdateModeType UpdateMode = UpdateModeType.Aggregated;

        string _localisationString;
        bool _localisationHandlerAdded;
        bool _isAnimationRunning;

        Queue<T> _valuesPendingDisplay;
        T _lastQueuedItem;
        T _currentlyDisplayedValue;


        /// <summary>
        /// Custom initialisation.
        /// </summary>
        public override void Start()
        {
            // if text not specified then get from current gameobject
            if (Text == null) Text = GetComponent<UnityEngine.UI.Text>();
            Assert.IsNotNull(Text, "You either have to specify a Text component, or attach the Show Lives component to a gameobject that contains one.");

            GetLocalisationString();

            // initialise queue. we set aside capacity 2 as we assume most cases won't exceed this.
            _valuesPendingDisplay = new Queue<T>(2);

            // set the current value and display it
            _valuesPendingDisplay.Enqueue(GetValueFromMessage(null));
            UpdateDisplay();

            GameManager.SafeAddListener<LocalisationChangedMessage>(LocalisationChangedHandler);
            _localisationHandlerAdded = true;
            base.Start();
        }

        public override void OnDestroy()
        {
            if (_localisationHandlerAdded)
                GameManager.SafeRemoveListener<LocalisationChangedMessage>(LocalisationChangedHandler);
            base.OnDestroy();
        }

        /// <summary>
        /// Called when localisation changes, updates the localisation string and the display
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool LocalisationChangedHandler(BaseMessage message)
        {
            GetLocalisationString();
            RefreshDisplayedValue();
            return true;
        }

        private void GetLocalisationString()
        {
            // if localisation key specified then get and cache string.
            if (!string.IsNullOrEmpty(LocalisationKey))
                _localisationString = GlobalLocalisation.GetText(LocalisationKey, missingReturnsKey: true);
        }

        /// <summary>
        /// This is the method that you should implement that will be called when 
        /// a message is received. Call base.Update() if you override this!
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool RunMethod(TM message)
        {
            var latestValue = GetValueFromMessage(message);

            // if no animator then we just update directly.
            if (Animator == null)
            {
                if (latestValue.CompareTo(_currentlyDisplayedValue) != 0)
                {
                    _valuesPendingDisplay.Enqueue(latestValue);
                    UpdateDisplay();
                }
            }

            // otherwise there is an animator so we need to conider how we do updates in respect to any updates that are already running.
            else
            {
                // add all messages for display unless the latest value is the same as the one pending display or we have the same value as
                // the previous queued message.
                if ( UpdateMode == UpdateModeType.Queued)
                {
                    // if there are either no other queued values or this value is different from the last queued value then we add. 
                    if (_valuesPendingDisplay.Count == 0 || latestValue.CompareTo(_lastQueuedItem) != 0)
                    {
                        _valuesPendingDisplay.Enqueue(latestValue);
                        _lastQueuedItem = latestValue;  // so we can easily reference this next time.
                    }
                }

                // only add the latest value
                else if (UpdateMode == UpdateModeType.Immediate || UpdateMode == UpdateModeType.Aggregated)
                {
                    _valuesPendingDisplay.Clear();
                    _valuesPendingDisplay.Enqueue(latestValue);
                }

                // process queued messages for display.
                ProcessQueuedMessages();
            }
            return true;
        }


        /// <summary>
        /// Check if we are ready to display new messages from the queue and if so then display them as needed.
        /// </summary>
        public void ProcessQueuedMessages()
        {
            if (_valuesPendingDisplay.Count == 0) return;

            // if we are ready to update then do so, otherwise we will be triggered by animation completion or a new item arriving.
            if (UpdateMode == UpdateModeType.Immediate ||
                ((UpdateMode == UpdateModeType.Aggregated || UpdateMode == UpdateModeType.Queued) && !_isAnimationRunning))
            {
                var nextValue = _valuesPendingDisplay.Peek();
                if (nextValue.CompareTo(_currentlyDisplayedValue) > 0)
                {
                    Animator.SetTrigger("Increased");
                    _isAnimationRunning = true;
                }
                else if (nextValue.CompareTo(_currentlyDisplayedValue) < 0)
                {
                    Animator.SetTrigger("Decreased");
                    _isAnimationRunning = true;
                }
            }
        }


        /// <summary>
        /// Update the display with the next value that is in the queue
        /// </summary>
        public void UpdateDisplay()
        {
            if (_valuesPendingDisplay.Count <= 0) return;
            // TODO Add a new mode - aggregate early - get new updated value incase changed since triggered.

            _currentlyDisplayedValue = _valuesPendingDisplay.Dequeue();
            RefreshDisplayedValue();
        }


        /// <summary>
        /// refresh the display with teh currently set _currentlyDisplayedValue
        /// </summary>
        private void RefreshDisplayedValue()
        {
            Text.text = _localisationString == null ? _currentlyDisplayedValue.ToString() : string.Format(_localisationString, _currentlyDisplayedValue);
        }


        /// <summary>
        /// Flag that the update is complete and try processing any other pending items.
        /// </summary>
        public void UpdateCompleted()
        {
            _isAnimationRunning = false;
            ProcessQueuedMessages();
        }


        /// <summary>
        /// Implement this to return the latest value of the item that you are trying to display.
        /// Note message can be null if this is during initial setup in which case you should return a default of initial value.
        /// </summary>
        /// <returns></returns>
        public abstract T GetValueFromMessage(TM message);
    }
}
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
using UnityEngine;
using UnityEngine.Assertions;

namespace GameFramework.UI.Other.Components.AbstractClasses
{
    public enum UpdateModeType { Immediate, Aggregated, Queued };

    /// <summary>
    /// An abstract class that runs updates a value in an optional animated fashion.
    /// 
    /// Override and implement the condition as you best see fit.
    /// </summary>
    public abstract class ShowValueAnimated<T> : MonoBehaviour where T : IComparable<T>
    {

        [Tooltip("A text component to update. Not needed if on the same gameobject as this component.")]
        public UnityEngine.UI.Text Text;
        [Tooltip("Optional localisation key. If not specified then the value is just displayed directly.")]
        public string LocalisationKey;

        [Header("Optional Animation Settings")]
        [Tooltip("An animator with Increased and Decreased triggers to set when the value changes. See GameFramework\\Animations\\UI\\ShowValueAnimated for an example.")]
        public Animator Animator;
        [Tooltip("How value changes that occur while an animation is already running are handled.")]
        public UpdateModeType UpdateMode = UpdateModeType.Aggregated;

        string _localisationString;
        bool _isAnimationRunning;

        Queue<T> _valuesPendingDisplay;
        T _currentlyDisplayedValue;
        T _nextValue;


        /// <summary>
        /// Start method. Call base.Start() if you override this!
        /// </summary>
        public virtual void Start()
        {
            // if text not specified then get from current gameobject
            if (Text == null) Text = GetComponent<UnityEngine.UI.Text>();
            Assert.IsNotNull(Text, "You either have to specify a Text component, or attach the Show Lives component to a gameobject that contains one.");

            // if localisation key specified then get and cache string.
            if (!string.IsNullOrEmpty(LocalisationKey))
                _localisationString = GlobalLocalisation.GetText(LocalisationKey, missingReturnsKey: true);

            // initialise queue. we set aside capacity 2 as we assume most cases won't exceed this.
            _valuesPendingDisplay = new Queue<T>(2);

            // set the current value and display it
            _nextValue = GetLatestValue();
            UpdateDisplay();
        }


        /// <summary>
        /// Update method. Call base.Update() if you override this!
        /// </summary>
        public virtual void Update()
        {
            // process further if value has changed or queued items are waiting for processing (will only happen in queued mode).
            T latestValue = GetLatestValue();
            if (latestValue.CompareTo(_currentlyDisplayedValue) != 0 || _valuesPendingDisplay.Count != 0)
            {
                // if no animator then we just update directly.
                if (Animator == null)
                {
                    _nextValue = latestValue;
                    UpdateDisplay();
                    UpdateCompleted();
                }
                else
                {
                    bool trigger = false;
                    // update immediately, but only if value has changed so we don't repeatedly trigger on same value (ref. condition above against _currentlyDisplayedValue).
                    if (UpdateMode == UpdateModeType.Immediate)
                    {
                        if (_nextValue.CompareTo(latestValue) != 0)
                        {
                            _nextValue = latestValue;
                            trigger = true;
                        }
                    }
                    // only update if an animation is not running
                    else if (UpdateMode == UpdateModeType.Aggregated)
                    {
                        if (!_isAnimationRunning)
                        {
                            _nextValue = latestValue;
                            trigger = true;
                        }
                    }
                    // if the value has changed save to the queue. if no animation running then pull from queue and update
                    else if (UpdateMode == UpdateModeType.Queued)
                    {
                        if (latestValue.CompareTo(_currentlyDisplayedValue) != 0)
                            _valuesPendingDisplay.Enqueue(latestValue);

                        if (!_isAnimationRunning)
                        {
                            _nextValue = _valuesPendingDisplay.Dequeue();
                            trigger = true;
                        }
                    }

                    // trigger the update if the value has changed.
                    if (trigger)
                    {
                        if (_nextValue.CompareTo(_currentlyDisplayedValue) > 0)
                        {
                            Animator.SetTrigger("Increased");
                            _isAnimationRunning = true;
                        }
                        else if (_nextValue.CompareTo(_currentlyDisplayedValue) < 0)
                        {
                            Animator.SetTrigger("Decreased");
                            _isAnimationRunning = true;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Update the display with the next value that is in the queue
        /// </summary>
        public void UpdateDisplay() 
        {
            // TODO Add a new mode - aggregate early - get new updated value incase changed since triggered.

            Text.text = _localisationString == null ? _nextValue.ToString() : string.Format(_localisationString, _nextValue);
            _currentlyDisplayedValue = _nextValue;
        }


        /// <summary>
        /// Notify that the update is complete
        /// </summary>
        public void UpdateCompleted()
        {
            _isAnimationRunning = false;
        }


        /// <summary>
        /// Implement this to return the latest value of the item that you are trying to display.
        /// </summary>
        /// <returns></returns>
        public abstract T GetLatestValue();
    }
}
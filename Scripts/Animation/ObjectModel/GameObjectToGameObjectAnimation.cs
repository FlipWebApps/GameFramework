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

using System.Collections;
using UnityEngine;

#if BEAUTIFUL_TRANSITIONS
using BeautifulTransitions.Scripts.Transitions;
#endif

namespace GameFramework.Animation.ObjectModel
{
    /// <summary>
    /// Used for animating between two different gameobjects.
    /// </summary>
    [System.Serializable]
    public class GameObjectToGameObjectAnimation
    {
        #region Enums

        /// <summary>
        /// The type of animation to run
        /// </summary>
        public enum AnimationModeType
        {
            Immediate,
            BeautifulTransitions
        } // , Animation }

        /// <summary>
        /// The order in which to run transitions
        /// </summary>
        /// OutIn - Run a transition out then when complete a transition in.
        /// Parallel - Run a out and at the same time a transition in (use a delay to offset).

        public enum TransitionOrderType
        {
            OutIn,
            Parallel
        }

        #endregion Enums

        #region Editor Parameters

        /// <summary>
        /// The animation mode for swapping between gameobjects.
        /// </summary>
        public AnimationModeType AnimationMode
        {
            get
            {
                return _animationMode;
            }
            set
            {
                _animationMode = value;
            }
        }
        [Tooltip("The animation mode for swapping between gameobjects.")]
        [SerializeField]
        AnimationModeType _animationMode;

        /// <summary>
        /// The order in which to run transitions
        /// </summary>
        public TransitionOrderType TransitionOrder
        {
            get
            {
                return _transitionOrder;
            }
            set
            {
                _transitionOrder = value;
            }
        }
        [Tooltip("The order in which to run transitions")]
        [SerializeField]
        TransitionOrderType _transitionOrder;

        ///// <summary>
        ///// An optional transition that should be run to transition in / out. If not specified and TransitionChanges is set then the first attached transition will be used.
        ///// </summary>
        ///// You might want to use different transitions from when this item is first shown and when changes occur.
        //public TransitionBase Transition
        //{
        //    get { return _transition; }
        //    set { _transition = value; }
        //}
        //[Tooltip("An optional transition that should be run to transition in / out. If not specified and TransitionChanges is set then the first attached transition will be used.")]
        //[SerializeField]
        //TransitionBase _transition;

        #endregion Editor Parameters

        public void AnimatedSwap(MonoBehaviour controller, GameObject fromGameObject, GameObject toGameObject, bool isStart = false)
        {
            // Option to not animate onStart
#if BEAUTIFUL_TRANSITIONS
            if (AnimationMode == AnimationModeType.BeautifulTransitions)
            {
                if (TransitionOrder == TransitionOrderType.OutIn)
                    controller.StartCoroutine(TransitionOutIn(fromGameObject, toGameObject));
                else
                    controller.StartCoroutine(TransitionParallel(fromGameObject, toGameObject));
                return;
            }
            //else if (AnimationType == GameItemContextConditionallyEnable.AnimationType.Animation) {
            //    if (!isStart) {
            //        activateImmediately = false;
            //    }
            //}
#endif

            // default to swap immediately
            SwapImmediately(fromGameObject, toGameObject);
        }


        public void SwapImmediately(GameObject fromGameObject, GameObject toGameObject)
        {
            if (fromGameObject != null)
                fromGameObject.SetActive(false);
            if (toGameObject != null)
                toGameObject.SetActive(true);
        }


#if BEAUTIFUL_TRANSITIONS
        IEnumerator TransitionOutIn(GameObject fromGameObject, GameObject toGameObject)
        {
            if (fromGameObject != null)
            {
                // is an out transition then run and wait for completion
                if (TransitionHelper.ContainsTransition(fromGameObject))
                {
                    var transitions = TransitionHelper.TransitionOut(fromGameObject);
                    var transitionOutTime = TransitionHelper.GetTransitionOutTime(transitions);
                    yield return new WaitForSeconds(transitionOutTime);
                }
                fromGameObject.SetActive(false);
            }
            if (toGameObject != null)
            {
                toGameObject.SetActive(true);
                if (TransitionHelper.ContainsTransition(toGameObject))
                {
                    TransitionHelper.TransitionIn(toGameObject);
                }
            }
        }
#endif

#if BEAUTIFUL_TRANSITIONS
        IEnumerator TransitionParallel(GameObject fromGameObject, GameObject toGameObject)
        {
            float transitionOutTime = 0;
            if (fromGameObject != null)
            {
                if (TransitionHelper.ContainsTransition(fromGameObject))
                {
                    var transitions = TransitionHelper.TransitionOut(fromGameObject);
                    transitionOutTime = TransitionHelper.GetTransitionOutTime(transitions);
                }
            }
            if (toGameObject != null)
            {
                toGameObject.SetActive(true);
                if (TransitionHelper.ContainsTransition(toGameObject))
                {
                    TransitionHelper.TransitionIn(toGameObject);
                }
            }

            // wait for transition out to complete before we disable.
            if (!Mathf.Approximately(0, transitionOutTime))
                yield return new WaitForSeconds(transitionOutTime);

            if (fromGameObject != null)
                fromGameObject.SetActive(false);
        }
#endif
    }
}
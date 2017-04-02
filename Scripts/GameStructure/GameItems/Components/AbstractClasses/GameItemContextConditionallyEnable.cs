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

using GameFramework.Animation.ObjectModel;
using GameFramework.GameStructure.GameItems.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace GameFramework.GameStructure.GameItems.Components.AbstractClasses
{
    public class GameItemContextConditionallyEnable
    {
        /// <summary>
        /// The type of object that we are enabling
        /// </summary>
        public enum EnableModeType
        {
            GameObject,
            Button
        }
    }

    /// <summary>
    /// Enabled or a disabled a gameobject based upon a condition.
    /// </summary>
    public abstract class GameItemContextConditionallyEnable<T> : GameItemContextBaseRunnable<T> where T : GameItem
    {
        /// <summary>
        /// The enable mode. Either changing interactivity of an attached button or swapping between gameobjects 
        /// </summary>
        [Header("Enable")]
        [Tooltip("The enable mode. Either changing interactivity of an attached button or swapping between gameobjects")]
        public GameItemContextConditionallyEnable.EnableModeType EnableMode;

        /// <summary>
        /// GameObject to show if the specified GameItem is selected
        /// </summary>
        [Tooltip("GameObject to show if the specified GameItem is selected")]
        public GameObject ConditionMetGameObject;

        /// <summary>
        /// GameObject to show if the specified GameItem is not selected
        /// </summary>
        [Tooltip("GameObject to show if the specified GameItem is not selected")]
        public GameObject ConditionNotMetGameObject;

        /// <summary>
        /// Settings for how to animate changes
        /// </summary>
        [Tooltip("Settings for hwow to animate changes")]
        public GameObjectToGameObjectAnimation GameObjectToGameObjectAnimation;

        Button _button;
        bool _isConditionMet;

        /// <summary>
        /// Setup
        /// </summary>
        protected override void Awake()
        {
            _button = GetComponent<Button>();
            base.Awake();
        }

        /// <summary>
        /// Called by the base class from start and optionally if a condition might have changed.
        /// </summary>
        /// <param name="isStart"></param>
        public override void RunMethod(bool isStart = true)
        {
            var newIsConditionMet = IsConditionMet(GetGameItem<T>());

            if (isStart || newIsConditionMet != _isConditionMet)
            {
                _isConditionMet = newIsConditionMet;

                if (EnableMode == GameItemContextConditionallyEnable.EnableModeType.Button)
                {
                    Assert.IsNotNull(_button,
                        "If you have an enable mode of button then ensure that the component is added to a GameObject that has a UI button.");
                    _button.interactable = _isConditionMet;
                }
                else
                {
                    var fromGameObject = _isConditionMet ? ConditionNotMetGameObject : ConditionMetGameObject;
                    var toGameObject = _isConditionMet ? ConditionMetGameObject : ConditionNotMetGameObject;

                    if (isStart)
                        GameObjectToGameObjectAnimation.SwapImmediately(fromGameObject, toGameObject);
                    else
                        GameObjectToGameObjectAnimation.AnimatedSwap(this, fromGameObject, toGameObject);
                }
            }
        }


        /// <summary>
        /// Implement this to return whether to show the condition met gameobject (true) or the condition not met one (false)
        /// </summary>
        /// <returns></returns>
        public abstract bool IsConditionMet(T gameItem);
    }
}
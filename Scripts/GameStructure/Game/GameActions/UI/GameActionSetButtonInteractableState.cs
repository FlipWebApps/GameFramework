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

using GameFramework.GameStructure.Game.ObjectModel.Abstract;
using GameFramework.Helper;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace GameFramework.GameStructure.Game.GameActions.UI
{
    /// <summary>
    /// Set the specified Buttons interactable state.
    /// </summary>
    [System.Serializable]
    [ClassDetails("Set Button Interactable State", "UI/Set Button Interactable State", "Set the specified Button's interactable state.")]
    public class GameActionSetButtonInteractableState : GameAction
    {
        /// <summary>
        /// What Button to target.
        /// </summary>
        public GameActionHelper.TargetType TargetType
        {
            get
            {
                return _targetType;
            }
            set
            {
                _targetType = value;
            }
        }
        [Tooltip("What Button to target.")]
        [SerializeField]
        GameActionHelper.TargetType _targetType = GameActionHelper.TargetType.ThisGameObject;


        /// <summary>
        /// The target Button
        /// </summary>
        public Button Target
        {
            get
            {
                return _target;
            }
            set
            {
                _target = value;
            }
        }
        [Tooltip("The target Button")]
        [SerializeField]
        Button _target;

        /// <summary>
        /// The interactable state for the button
        /// </summary>
        public bool Interactable
        {
            get
            {
                return _interactable;
            }
            set
            {
                _interactable = value;
            }
        }
        [Tooltip("The interactable state for the button")]
        [SerializeField]
        bool _interactable;

        /// <summary>
        /// Whether to animate state changes using Beautiful Transitions DsiaplyItem animation controller.
        /// </summary>
        public bool AnimateChanges
        {
            get
            {
                return _animateChanges;
            }
            set
            {
                _animateChanges = value;
            }
        }
        [Tooltip("Whether to animate state changes using Beautiful Transitions DsiaplyItem animation controller.")]
        [SerializeField]
        bool _animateChanges;


        Button _cachedFinalTarget;


        /// <summary>
        /// Perform the action
        /// </summary>
        /// <returns></returns>
        protected override void Execute(bool isStart)
        {
            // use cached version unless target could be dynamic (TargetType.CollidingGameObject)
            var targetFinal = _cachedFinalTarget;
            if (targetFinal == null)
            {
                targetFinal = GameActionHelper.ResolveTargetComponent<Button>(TargetType, this, Target);
                if (TargetType != GameActionHelper.TargetType.CollidingGameObject)
                    _cachedFinalTarget = targetFinal;
            }

            Assert.IsNotNull(targetFinal,
                "Ensure that you specify a Target button when using the 'Set Button Interactable' action.");

            targetFinal.interactable = Interactable;
            if (AnimateChanges)
            {
                Debug.LogWarning("Animation of Button Interactable State changes is only supported if using the Beautiful Transitions asset. See the Menu | Window | Game Framework | Integrations Window for more information.");
#if BEAUTIFUL_TRANSITIONS
                BeautifulTransitions.Scripts.DisplayItem.DisplayItemHelper.SetActiveAnimated(Owner, targetFinal.gameObject, Interactable);
#else
#endif
            }
        }

        #region IScriptableObjectContainerSyncReferences

        /// <summary>
        /// Workaround for ObjectReference issues with ScriptableObjects (See ScriptableObjectContainer for details)
        /// </summary>
        /// <param name="objectReferences"></param>
        public override void SetReferencesFromContainer(UnityEngine.Object[] objectReferences)
        {
            if (objectReferences != null && objectReferences.Length == 1)
                Target = objectReferences[0] as Button;
        }

        /// <summary>
        /// Workaround for ObjectReference issues with ScriptableObjects (See ScriptableObjectContainer for details)
        /// </summary>
        public override UnityEngine.Object[] GetReferencesForContainer()
        {
            var objectReferences = new Object[1];
            objectReferences[0] = Target;
            return objectReferences;
        }

        #endregion IScriptableObjectContainerSyncReferences
    }
}

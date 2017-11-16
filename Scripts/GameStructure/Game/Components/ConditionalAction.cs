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

using GameFramework.Messaging;
using UnityEngine;
using GameFramework.GameStructure.Game.ObjectModel;
using GameFramework.Helper.UnityEvents;

namespace GameFramework.GameStructure.Game.Components
{
    /// <summary>
    /// Generic component for taking user defined actions on specified conditions.
    /// </summary>
    [AddComponentMenu("Game Framework/GameStructure/Conditional Action")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/game-structure/")]
    public class ConditionalAction : MonoBehaviour
    {
        /// <summary>
        /// A list of conditions to evaluate.
        /// </summary>
        public GameConditionReference[] ConditionReferences { get { return _conditionReferences; } set { _conditionReferences = value; } }
        [Tooltip("A list of conditions to evaluate.")]
        [SerializeField]
        GameConditionReference[] _conditionReferences = new GameConditionReference[0];

        /// <summary>
        /// A list of actions that should be run when the condition is met.
        /// </summary>
        public GameActionReference[] ActionReferencesConditionMet { get { return _actionReferencesConditionMet; } set { _actionReferencesConditionNotMet = value; } }
        [Tooltip("A list of actions that should be run when the condition is met.")]
        [SerializeField]
        GameActionReference[] _actionReferencesConditionMet = new GameActionReference[0];

        /// <summary>
        /// Methods that should be called when the condition is met.
        /// </summary>
        public UnityGameObjectEvent ActionReferencesConditionMetCallback
        {
            get
            {
                return _actionReferencesConditionMetCallback;
            }
            set
            {
                _actionReferencesConditionMetCallback = value;
            }
        }
        [Tooltip("Methods that should be called when the condition is met.")]
        [SerializeField]
        UnityGameObjectEvent _actionReferencesConditionMetCallback;

        /// <summary>
        /// A list of actions that should be run when the condition is NOT met.
        /// </summary>
        public GameActionReference[] ActionReferencesConditionNotMet { get { return _actionReferencesConditionNotMet; } set { _actionReferencesConditionNotMet = value; } }
        [Tooltip("A list of actions that should be run when the condition is not met.")]
        [SerializeField]
        GameActionReference[] _actionReferencesConditionNotMet = new GameActionReference[0];

        /// <summary>
        /// Methods that should be called when the condition is not met.
        /// </summary>
        public UnityGameObjectEvent ActionReferencesConditionNotMetCallback
        {
            get
            {
                return _actionReferencesConditionNotMetCallback;
            }
            set
            {
                _actionReferencesConditionNotMetCallback = value;
            }
        }
        [Tooltip("Methods that should be called when the condition is met.")]
        [SerializeField]
        UnityGameObjectEvent _actionReferencesConditionNotMetCallback;

        bool _isConditionMet;

        /// <summary>
        /// Setup
        /// </summary>
        protected void Start()
        {
            GameConditionHelper.InitialiseGameConditions(_conditionReferences, this);
            GameActionHelper.InitialiseGameActions(_actionReferencesConditionMet, this);
            GameActionHelper.InitialiseGameActions(_actionReferencesConditionNotMet, this);

            GameConditionHelper.AddListeners(_conditionReferences, EvaluateConditionChanges);

            RunMethod(true);
        }


        /// <summary>
        /// Destroy
        /// </summary>
        protected void OnDestroy()
        {
            GameConditionHelper.RemoveListeners(_conditionReferences, EvaluateConditionChanges);
        }


        /// <summary>
        /// Called when a message is received that indicates a possible condition change.
        /// </summary>
        /// <param name="message"></param>
        bool EvaluateConditionChanges(BaseMessage message)
        {
            RunMethod(false);
            return true;
        }


        /// <summary>
        /// Called by the base class from start and optionally if a condition might have changed.
        /// </summary>
        /// <param name="isStart"></param>
        public void RunMethod(bool isStart = true)
        {
            var newIsConditionMet = GameConditionHelper.AllConditionsMet(_conditionReferences, this);

            if (isStart || newIsConditionMet != _isConditionMet)
            {
                _isConditionMet = newIsConditionMet;

                if (_isConditionMet)
                {
                    GameActionHelper.ExecuteGameActions(_actionReferencesConditionMet, isStart);
                }
                else
                {
                    GameActionHelper.ExecuteGameActions(_actionReferencesConditionNotMet, isStart);
                }
            }
        }
    }
}
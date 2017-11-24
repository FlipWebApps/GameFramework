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
    /// Generic component for running a list of GameActions.
    /// </summary>
    [AddComponentMenu("Game Framework/GameStructure/Run Game Actions")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/game-structure/")]
    public class RunGameActions : MonoBehaviour
    {
        // enum to determine when to run the Game Actions.
        public enum AutomaticallyRunEnum
        {
            Never,                    // Don't automatically run.
            OnStart,                   // Run on Start.
            OnEnable,                  // Run on Enable.
        }


        /// <summary>
        /// When to automatically run the actions
        /// </summary>
        public AutomaticallyRunEnum AutomaticallyRun { get { return _automaticallyRun; } set { _automaticallyRun = value; } }
        [Tooltip("When to automatically run the actions.")]
        [SerializeField]
        AutomaticallyRunEnum _automaticallyRun = AutomaticallyRunEnum.Never;


        /// <summary>
        /// A list of actions that should be run.
        /// </summary>
        public GameActionReference[] ActionReferences { get { return _actionReferences; } set { _actionReferences = value; } }
        [Tooltip("A list of actions that should be run when the condition is met.")]
        [SerializeField]
        GameActionReference[] _actionReferences = new GameActionReference[0];


        /// <summary>
        /// Setup
        /// </summary>
        protected void Start()
        {
            GameActionHelper.InitialiseGameActions(_actionReferences, this);

            if (AutomaticallyRun == AutomaticallyRunEnum.OnStart)
                ExecuteActions();
        }


        /// <summary>
        /// Setup
        /// </summary>
        protected void OnEnable()
        {
            if (AutomaticallyRun == AutomaticallyRunEnum.OnEnable)
                ExecuteActions();
        }


        /// <summary>
        /// Run all of the attached Game Actions.
        /// </summary>
        /// <param name="isStart"></param>
        public void ExecuteActions()
        {
            GameActionHelper.ExecuteGameActions(_actionReferences, false);
        }
    }
}
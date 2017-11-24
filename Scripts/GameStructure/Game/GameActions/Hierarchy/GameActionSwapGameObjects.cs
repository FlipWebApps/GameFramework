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
using GameFramework.GameStructure.Game.ObjectModel.Abstract;
using GameFramework.Helper;
using UnityEngine;

namespace GameFramework.GameStructure.Game.GameActions.Hierarchy
{
    /// <summary>
    /// Enable the specified GameObject
    /// </summary>
    [System.Serializable]
    [ClassDetails("Swap GameObjects", "Hierarchy/Swap GameObjects", "Switch between different GameObjects with optional animation.")]
    public class GameActionSwapGameObjects : GameAction
    {
        /// <summary>
        /// The GameObject to switch from.
        /// </summary>
        public GameActionHelper.TargetType SwitchFromTargetType
        {
            get
            {
                return _switchFromTargetType;
            }
            set
            {
                _switchFromTargetType = value;
            }
        }
        [Tooltip("The GameObject to switch from.")]
        [SerializeField]
        GameActionHelper.TargetType _switchFromTargetType = GameActionHelper.TargetType.ThisGameObject;


        /// <summary>
        /// The GameObject to switch from
        /// </summary>
        public GameObject SwitchFrom
        {
            get
            {
                return _switchFrom;
            }
            set
            {
                _switchFrom = value;
            }
        }
        [Tooltip("The GameObject to switch from")]
        [SerializeField]
        GameObject _switchFrom;

        /// <summary>
        /// The GameObject to switch to
        /// </summary>
        public GameObject SwitchTo
        {
            get
            {
                return _switchTo;
            }
            set
            {
                _switchTo = value;
            }
        }
        [Tooltip("The GameObject to switch to")]
        [SerializeField]
        GameObject _switchTo;

        /// <summary>
        /// Settings for how to animate changes
        /// </summary>
        [Tooltip("Settings for hwow to animate changes")]
        public GameObjectToGameObjectAnimation GameObjectToGameObjectAnimation;

        /// <summary>
        /// Perform the action
        /// </summary>
        /// <returns></returns>
        protected override void Execute(bool isStart)
        {
            var switchFromFinal = GameActionHelper.ResolveTarget(SwitchFromTargetType, this, SwitchFrom);
            if (switchFromFinal == null) Debug.LogWarningFormat("No Target is specified for the action {0} on {1}", GetType().Name, Owner.gameObject.name);
            if (switchFromFinal != null)
            {
                if (isStart)
                    GameObjectToGameObjectAnimation.SwapImmediately(switchFromFinal, SwitchTo);
                else
                    GameObjectToGameObjectAnimation.AnimatedSwap(Owner, switchFromFinal, SwitchTo);
            }
        }

        #region IScriptableObjectContainerSyncReferences

        /// <summary>
        /// Workaround for ObjectReference issues with ScriptableObjects (See ScriptableObjectContainer for details)
        /// </summary>
        /// <param name="objectReferences"></param>
        public override void SetReferencesFromContainer(UnityEngine.Object[] objectReferences)
        {
            if (objectReferences != null && objectReferences.Length == 2)
            {
                SwitchFrom = objectReferences[0] as GameObject;
                SwitchTo = objectReferences[1] as GameObject;
            }
        }

        /// <summary>
        /// Workaround for ObjectReference issues with ScriptableObjects (See ScriptableObjectContainer for details)
        /// </summary>
        public override UnityEngine.Object[] GetReferencesForContainer()
        {
            var objectReferences = new Object[2];
            objectReferences[0] = SwitchFrom;
            objectReferences[1] = SwitchTo;
            return objectReferences;
        }

        #endregion IScriptableObjectContainerSyncReferences

    }
}

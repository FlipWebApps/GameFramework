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

namespace GameFramework.GameStructure.Game.ObjectModel.Abstract
{
    /// <summary>
    /// Base GameAction class that that allows for specifying a target GameObject
    /// </summary>
    [System.Serializable]
    public abstract class GameActionTarget : GameAction
    {
        /// <summary>
        /// What GameObject to target.
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
        [Tooltip("What GameObject to target.")]
        [SerializeField]
        GameActionHelper.TargetType _targetType = GameActionHelper.TargetType.ThisGameObject;


        /// <summary>
        /// The target GameObject to act upon if using a target type mode of Specified.
        /// </summary>
        public GameObject Target
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
        [Tooltip("The target GameObject to act upon if using a target type mode of Specified.")]
        [SerializeField]
        GameObject _target;

        #region IScriptableObjectContainerSyncReferences

        /// <summary>
        /// Workaround for ObjectReference issues with ScriptableObjects (See ScriptableObjectContainer for details)
        /// </summary>
        /// <param name="objectReferences"></param>
        public override void SetReferencesFromContainer(UnityEngine.Object[] objectReferences)
        {
            if (objectReferences != null && objectReferences.Length == 1)
                Target = objectReferences[0] as GameObject;
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

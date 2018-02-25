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

namespace GameFramework.GameStructure.Game.GameActions.ProPooling
{
    /// <summary>
    /// GameAction class to add an item from a Pro Pooling PoolManager pool with the given name (requires Pro Pooling)
    /// </summary>
    [System.Serializable]
    [ClassDetails("Add Pooled Item", "Pro Pooling/Add Pooled Item", "Add an item from the named Pro Pooling PoolManager pool (requires Pro Pooling).")]
    public class GameActionAddPooledItem : GameAction
    {
        /// <summary>
        /// Name of the Pro Pooling PoolManager pool to get an item from (requires Pro Pooling)
        /// </summary>
        public string PoolName
        {
            get
            {
                return _poolName;
            }
            set
            {
                _poolName = value;
            }
        }
        [Tooltip("Name of the Pro Pooling PoolManager pool to get an item from (requires Pro Pooling)")]
        [SerializeField]
        string _poolName;


        /// <summary>
        /// What target to use for the location.
        /// </summary>
        public GameActionHelper.TargetType LocationTargetType
        {
            get
            {
                return _locationTargetType;
            }
            set
            {
                _locationTargetType = value;
            }
        }
        [Tooltip("What target to use for the location.")]
        [SerializeField]
        GameActionHelper.TargetType _locationTargetType = GameActionHelper.TargetType.ThisGameObject;


        /// <summary>
        /// A Transform that defines where to instantiate the prefab. If blank uses the containing GameObjects transform.
        /// </summary>
        public Transform Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }
        [Tooltip("A Transform that defines where to instantiate the prefab. If blank uses the containing GameObjects transform.")]
        [SerializeField]
        Transform _location;

        /// <summary>
        /// Perform the action
        /// </summary>
        /// <returns></returns>
        protected override void Execute(bool isStart)
        {

#if PRO_POOLING
            if (!string.IsNullOrEmpty(PoolName))
            {
                // use cached version unless target could be dynamic (TargetType.CollidingGameObject)
                var transformFinal = GameActionHelper.ResolveTargetComponent<Transform>(LocationTargetType, this, Location);
                if (transformFinal == null) Debug.LogWarningFormat("No Target Location is specified for the action {0} on {1}", GetType().Name, Owner.gameObject.name);
                if (transformFinal != null)
                {
                    global::ProPooling.Components.GlobalPools.Instance.Spawn(PoolName,
                        transformFinal.position,
                        transformFinal.rotation);
                }
            }
#endif
        }
    }
}

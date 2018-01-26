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

namespace GameFramework.GameStructure.Game.GameActions.Hierarchy
{
    /// <summary>
    /// Enable the specified GameObject
    /// </summary>
    [System.Serializable]
    [ClassDetails("Set Rigidbody Type", "Physics 2D/Set Rigidbody Type", "Set the Rigidbody type.")]
    public class GameActionPhysics2DRigidbodyType : GameActionTarget
    {
#if UNITY_5_6_OR_NEWER
        /// <summary>
        /// What RigidbodyType2D to set.
        /// </summary>
        public RigidbodyType2D BodyType
        {
            get
            {
                return _bodyType;
            }
            set
            {
                _bodyType = value;
            }
        }
        [Tooltip("What RigidbodyType2D to set.")]
        [SerializeField]
        RigidbodyType2D _bodyType;

        /// <summary>
        /// Perform the action
        /// </summary>
        /// <returns></returns>
        protected override void Execute(bool isStart)
        {
            var targetFinal = GameActionHelper.ResolveTarget(TargetType, this, Target);
            if (targetFinal == null) Debug.LogWarningFormat("No Target is specified for the action {0} on {1}", GetType().Name, Owner.gameObject.name);
            if (targetFinal != null)
            {
                var rigidBody2D = targetFinal.GetComponent<Rigidbody2D>();
                if (rigidBody2D != null)
                {
                    rigidBody2D.bodyType = BodyType;
                }
                else
                {
                    Debug.LogWarningFormat("No rigidbody found for the action {0} on {1}", GetType().Name, targetFinal.gameObject.name);
                }
            }
        }
#else
        // do nothing
        protected override void Execute(bool isStart) { }
#endif
    }
}

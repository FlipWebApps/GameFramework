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

using UnityEngine;

namespace GameFramework.GameStructure.Colliders
{
    /// <summary>
    /// Collider for increasing or decreasing a players / levels points when a tagged gameobject touches the attached collider or trigger.
    /// </summary>
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/game-structure/colliders/")]
    public class PointsCollider : GenericCollider
    {
        /// <summary>
        /// An amount that specifies how much the health should change by. Put a minus value to decrease.
        /// </summary>
        public int PointsChange
        {
            get
            {
                return _pointsChange;
            }
            set
            {
                _pointsChange = value;
            }
        }
        [Header("Points Specific Settings")]
        [Tooltip("An amount that specifies how much the number of pointss should change by. Put a minus value to decrease.")]
        [SerializeField]
        int _pointsChange = 1;

        /// <summary>
        /// Whether to affect the Players points count.
        /// </summary>
        public bool ChangePlayerPoints
        {
            get
            {
                return _changePlayerPoints;
            }
            set
            {
                _changePlayerPoints = value;
            }
        }
        [Tooltip("Whether to change the Players points")]
        [SerializeField]
        bool _changePlayerPoints = true;

        /// <summary>
        /// Whether to affect the Levels points count.
        /// </summary>
        public bool ChangeLevelPoints
        {
            get
            {
                return _changeLevelPoints;
            }
            set
            {
                _changeLevelPoints = value;
            }
        }
        [Tooltip("Whether to change the Levels points")]
        [SerializeField]
        bool _changeLevelPoints = true;


        /// <summary>
        /// Called when we have detected and processed a valid trigger / collider enter based upon other settings
        /// </summary>
        /// Override this in you custom base classes that you want to hook into the trigger system.
        /// <param name="collidingGameObject">The GameObject that we collided with</param>
        public override void EnterOccurred(GameObject collidingGameObject)
        {
            AdjustPoints();
        }


        /// <summary>
        /// Called when we have detected and processed a valid trigger / collider stay based upon other settings
        /// </summary>
        /// Override this in you custom base classes that you want to hook into the trigger system.
        /// <param name="collidingGameObject">The GameObject that we collided with</param>
        public override void StayOccurred(GameObject collidingGameObject)
        {
            AdjustPoints();
        }


        /// <summary>
        /// Adjust the number of pointss
        /// </summary>
        void AdjustPoints()
        {
            if (ChangePlayerPoints)
                GameManager.Instance.Player.AddPoints(PointsChange);
            if (ChangeLevelPoints)
                GameManager.Instance.Levels.Selected.AddPoints(PointsChange);
        }
    }
}
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

using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Levels.ObjectModel
{
    /// <summary>
    /// A subclass of GameItemExtension for overriding and customising Levels
    /// </summary>
    /// You can also subclass from this to add your own custom data and functionality.
    [CreateAssetMenu(fileName = "Level_x", menuName="Game Framework/Level Extension")]
    public class LevelExtension : GameItemExtension
    {
        /// <summary>
        /// Whether to override the default star total count.
        /// </summary>
        /// See Level for more information.
        public bool OverrideStarTotalCount
        {
            get
            {
                return _overrideStarTotalCount;
            }
            set
            {
                _overrideStarTotalCount = value;
            }
        }
        [Tooltip("Whether to override the default star total count.")]
        [SerializeField]
        bool _overrideStarTotalCount;


        /// <summary>
        /// An override for the default star total count.
        /// </summary>
        /// See Level for more information.
        public int StarTotalCount
        {
            get
            {
                return _starTotalCount;
            }
            set
            {
                _starTotalCount = value;
            }
        }
        [Tooltip("An override for the default star total count.")]
        [SerializeField]
        int _starTotalCount;

        
        /// <summary>
        /// Whether to override the target for getting the first star.
        /// </summary>
        /// See Level for more information.
        public bool OverrideStar1Target
        {
            get
            {
                return _overrideStar1Target;
            }
            set
            {
                _overrideStar1Target = value;
            }
        }
        [Tooltip("Whether to override the target for getting the first star.")]
        [SerializeField]
        bool _overrideStar1Target;


        /// <summary>
        /// The target for getting the first star or -1 if no target
        /// </summary>
        public float Star1Target
        {
            get
            {
                return _star1Target;
            }
            set
            {
                _star1Target = value;
            }
        }
        [Tooltip("The target for getting 1 star or -1 if no target")]
        [SerializeField]
        float _star1Target;


        /// <summary>
        /// Whether to override the target for getting the second star.
        /// </summary>
        /// See Level for more information.
        public bool OverrideStar2Target
        {
            get
            {
                return _overrideStar2Target;
            }
            set
            {
                _overrideStar2Target = value;
            }
        }
        [Tooltip("Whether to override the target for getting the second star.")]
        [SerializeField]
        bool _overrideStar2Target;


        /// <summary>
        /// The target for getting the second star or -1 if no target
        /// </summary>
        public float Star2Target
        {
            get
            {
                return _star2Target;
            }
            set
            {
                _star2Target = value;
            }
        }
        [Tooltip("The target for getting 2 stars or -1 if no target")]
        [SerializeField]
        float _star2Target;


        /// <summary>
        /// Whether to override the target for getting the third star.
        /// </summary>
        /// See Level for more information.
        public bool OverrideStar3Target
        {
            get
            {
                return _overrideStar3Target;
            }
            set
            {
                _overrideStar3Target = value;
            }
        }
        [Tooltip("Whether to override the target for getting the third star.")]
        [SerializeField]
        bool _overrideStar3Target;


        /// <summary>
        /// The target for getting the third star or -3 if no target
        /// </summary>
        public float Star3Target
        {
            get
            {
                return _star3Target;
            }
            set
            {
                _star3Target = value;
            }
        }
        [Tooltip("The target for getting 3 stars or -1 if no target")]
        [SerializeField]
        float _star3Target;


        /// <summary>
        /// Whether to override the target for getting the fourth star.
        /// </summary>
        /// See Level for more information.
        public bool OverrideStar4Target
        {
            get
            {
                return _overrideStar4Target;
            }
            set
            {
                _overrideStar4Target = value;
            }
        }
        [Tooltip("Whether to override the target for getting the fourth star.")]
        [SerializeField]
        bool _overrideStar4Target;


        /// <summary>
        /// The target for getting the fourth star or -4 if no target
        /// </summary>
        public float Star4Target
        {
            get
            {
                return _star4Target;
            }
            set
            {
                _star4Target = value;
            }
        }
        [Tooltip("The target for getting 4 stars or -1 if no target")]
        [SerializeField]
        float _star4Target;


        /// <summary>
        /// Whether to override the time target.
        /// </summary>
        /// See Level for more information.
        public bool OverrideTimeTarget
        {
            get
            {
                return _overrideTimeTarget;
            }
            set
            {
                _overrideTimeTarget = value;
            }
        }
        [Tooltip("Whether to override the time target.")]
        [SerializeField]
        bool _overrideTimeTarget;


        /// <summary>
        /// The time target for completing the level
        /// </summary>
        public float TimeTarget
        {
            get
            {
                return _TimeTarget;
            }
            set
            {
                _TimeTarget = value;
            }
        }
        [Tooltip("The time target for completing the level.")]
        [SerializeField]
        float _TimeTarget;
    }

}
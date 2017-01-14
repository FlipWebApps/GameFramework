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
using UnityEngine.Assertions;

/// <summary>
/// Support components for working with animations.
/// 
/// For additional information see http://www.flipwebapps.com/unity-assets/game-framework/animations/
/// </summary>
namespace GameFramework.Animation
{
    // For doxygen documentation purposes only 
}

namespace GameFramework.Animation.Components
{
    /// <summary>
    /// Set a list of start values on an Animator. 
    /// </summary>
    /// This can be used for dynamically providing configuration to an Animator.
    /// 
    /// Must be attached to a gameobject that contains an Animator component.
    [RequireComponent(typeof(Animator))]
    [AddComponentMenu("Game Framework/Animation/SetAnimatorStartValues")]
    [HelpURL("http://www.flipwebapps.com/game-framework/animation/")]
    public class SetAnimatorStartValues : MonoBehaviour
    {
        /// <summary>
        /// A list of integar names to set
        /// </summary>
        [Tooltip("A list of integar names to set")]
        public string[] IntValueNames;

        /// <summary>
        /// A list of integar values to set for each of the names specified
        /// </summary>
        [Tooltip("A list of integar values to set for each of the names specified")]
        public int[] IntValueValues;

        void Awake()
        {
            Assert.AreEqual(IntValueNames.Length, IntValueValues.Length, "You must provide the same number of names as values");

            Animator animator = GetComponent<Animator>();
            animator.SetInteger(IntValueNames[0], IntValueValues[0]);
        }
    }
}
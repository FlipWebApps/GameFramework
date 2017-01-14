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

using GameFramework.GameObjects.Components.AbstractClasses;
using UnityEngine;

namespace GameFramework.Animation.Components
{
    /// <summary>
    /// Set an animation trigger one time only for the entire lifetime of your game, optionally only after another 
    /// RunOnceGameObject derived component (including this one) has been run.
    /// </summary>
    /// 
    /// You can use this functionality to trigger things that you only want to show one time such as instructions
    /// or some sort of introduction information. Use the enable after functionality to chain such actions.
    /// 
    /// See RunOnceGameObject for more details 
    [AddComponentMenu("Game Framework/Animation/SetTriggerOnce")]
    [HelpURL("http://www.flipwebapps.com/game-framework/animation/")]
    public class SetTriggerOnce : RunOnce
    {
        /// <summary>
        /// The Animator that should have the trigger set
        /// </summary>
        [Tooltip("The Animator that should have the trigger set")]
        public Animator Animator;

        /// <summary>
        /// The name of the trigger to set
        /// </summary>
        [Tooltip("The name of a boolean parameter to set")]
        public string Trigger;

        public override void RunOnceMethod()
        {
            Animator.SetTrigger(Trigger);
        }
    }
}
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

using System.Collections;
using GameFramework.GameStructure.Players.Messages;
using GameFramework.Messaging.Components.AbstractClasses;
using UnityEngine;

namespace GameFramework.GameStructure.Players.Components
{
    /// <summary>
    /// Enable a specified gameobject when a life is lost.
    /// </summary>
    [AddComponentMenu("Game Framework/GameStructure/Players/OnLifeLostEnableGameobject")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/game-structure/players/")]
    public class OnLifeLostEnableGameobject : RunOnMessage<LivesChangedMessage>
    {
        /// <summary>
        /// The target gameobject that should be enabled.
        /// </summary>
        [Tooltip("The target gameobject that should be enabled.")]
        public GameObject Target;

        /// <summary>
        /// The number of seconds after which to disable the gameobject again. 0 = never disable.
        /// </summary>
        [Tooltip("The number of seconds after which to disable the gameobject again. 0 = never disable.")]
        public float DisableDelay;

        /// <summary>
        /// Set the specified gameobject to active.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool RunMethod(LivesChangedMessage message)
        {
            Target.SetActive(true);
            if (DisableDelay > 0)
                StartCoroutine(DelayedDisable(Target, DisableDelay));
            return true;
        }

        /// <summary>
        /// Delay a gameobject after the given number of seconds.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        IEnumerator DelayedDisable(GameObject target, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (target != null)
                target.SetActive(false);
        }
    }
}

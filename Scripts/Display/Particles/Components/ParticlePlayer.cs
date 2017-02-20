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

namespace GameFramework.Display.Particles.Components
{
    /// <summary>
    /// Provides a callback for creating a new particle system that you can hook up to event handlers.
    /// </summary>
    [AddComponentMenu("Game Framework/Display/Particles/ParticlePlayer")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class ParticlePlayer : MonoBehaviour
    {
        public ParticleSystem ParticleSystem;

        public void CreateParticleSystem()
        {
            var newParticleSystem = Instantiate(
                ParticleSystem,
                transform.position,
                Quaternion.identity
                ) as ParticleSystem;

            // Make sure it will be destroyed
            if (newParticleSystem != null)
            {
#if UNITY_5_5_OR_NEWER
                Destroy(
                    newParticleSystem.gameObject,
                    newParticleSystem.main.startLifetime.constant
                    );
#else
                Destroy(
                    newParticleSystem.gameObject,
                    newParticleSystem.startLifetime
                    );
#endif
            }
        }
    }
}
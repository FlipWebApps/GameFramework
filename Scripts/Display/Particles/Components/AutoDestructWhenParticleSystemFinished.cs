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
using UnityEngine;

/// <summary>
/// Contains components to help with using particles
/// </summary>
/// For further information please see: http://www.flipwebapps.com/unity-assets/game-framework/display/
namespace GameFramework.Display.Particles.Components
{
    /// <summary>
    /// Destroy or disable the specified gameobject once the particle system is finished playing.
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    [AddComponentMenu("Game Framework/Display/Particles/AutoDestructWhenParticleSystemFinished")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/display/")]
    public class AutoDestructWhenParticleSystemFinished : MonoBehaviour
    {
        /// <summary>
        /// Whether this gameobject should just be deactivated rather than destroyed.
        /// </summary>
        [Tooltip("Whether this gameobject should just be deactivated rather than destroyed.")]
        public bool OnlyDeactivate;

        /// <summary>
        /// A gameobject to destroy / disable. If not specified the current gameobject will be used.
        /// </summary>
        [Tooltip("A gameobject to destroy / disable. If not specified the current gameobject will be used.")]
        public GameObject GameObjectToDestruct;

        ParticleSystem _particleSystem;

        void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        void OnEnable()
        {
            if (GameObjectToDestruct == null) GameObjectToDestruct = gameObject;
            StartCoroutine("CheckIfAlive");
        }

        IEnumerator CheckIfAlive()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.5f);
                if (!_particleSystem.IsAlive(true))
                {
                    if (OnlyDeactivate)
                        GameObjectToDestruct.SetActive(false);
                    else
                        Destroy(GameObjectToDestruct);
                    break;
                }
            }
        }
    }
}
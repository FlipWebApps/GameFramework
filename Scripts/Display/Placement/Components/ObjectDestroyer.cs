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

namespace GameFramework.Display.Placement.Components
{
    /// <summary>
    /// Automatically destroys gameobjects that collide with this one.
    /// </summary>
    /// This uses the physics system so ensure that colliders and rigid bodies are setup correctly.
    [AddComponentMenu("Game Framework/Display/Placement/ObjectDestroyer")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/display/")]
    public class ObjectDestroyer : MonoBehaviour
    {
        public enum DestroyModeType { DestroyCollidingGameObject, DestroyCollidingParentGameObject }
        /// <summary>
        /// Whether to destroy the colliding gameobject or it's parent.
        /// </summary>
        [Tooltip("Whether to destroy the colliding gameobject or it's parent.")]
        public DestroyModeType DestroyMode;

        /// <summary>
        /// The tag of colliding gameobjects to destroy. Leave blank to destroy all colliding gameobjects
        /// </summary>
        [Tooltip("The tag of colliding gameobjects to destroy. Leave blank to destroy all colliding gameobjects")]
        public string DestroyTag = "Obstacle";

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision == null)
                return;

            // Is this an obsticle?
            if (string.IsNullOrEmpty(DestroyTag) || collision.gameObject.CompareTag(DestroyTag))
            {
                DestroyCollidingGameObject(collision.gameObject);
            }
        }

        void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (otherCollider == null)
                return;

            // Is this an obsticle?
            if (string.IsNullOrEmpty(DestroyTag) || otherCollider.gameObject.CompareTag(DestroyTag))
            {
                DestroyCollidingGameObject(otherCollider.gameObject);
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision == null)
                return;

            // Is this an obsticle?
            if (string.IsNullOrEmpty(DestroyTag) || collision.gameObject.CompareTag(DestroyTag))
            {
                DestroyCollidingGameObject(collision.gameObject);
            }
        }

        void OnTriggerEnter(Collider otherCollider)
        {
            if (otherCollider == null)
                return;

            // Is this an obsticle?
            if (string.IsNullOrEmpty(DestroyTag) || otherCollider.gameObject.CompareTag(DestroyTag))
            {
                DestroyCollidingGameObject(otherCollider.gameObject);
            }
        }

        void DestroyCollidingGameObject(GameObject collidingGameObject)
        {
            switch (DestroyMode)
            {
                case DestroyModeType.DestroyCollidingGameObject:
                    Destroy(collidingGameObject);
                    break;
                case DestroyModeType.DestroyCollidingParentGameObject:
                    Destroy(collidingGameObject.transform.parent.gameObject);
                    break;
            }
        }
    }
}
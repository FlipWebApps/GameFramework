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

namespace GameFramework.Display.Placement
{
    /// <summary>
    /// Provides methods to help with positioning and moving items.
    /// </summary>
    public class PositionHelper : MonoBehaviour
    {
        public static Plane XzPlane = new Plane(Vector3.up, Vector3.zero);
        public static Plane XyPlane = new Plane(Vector3.forward, Vector3.zero);

        /// <summary>
        /// Return the position of a given Vector3 on the X / Z plane where the y position is 0
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetPositionOnXzPlane(Vector3 position)
        {
            return GetPositionOnPlane(position, XzPlane);
        }

        /// <summary>
        /// Return the position of a given Vector3 on the X / Z plane for a given y position
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetPositionOnXzPlane(Vector3 position, float y)
        {
            return GetPositionOnPlane(position, new Plane(Vector3.up, new Vector3(0, y, 0)));
        }

        /// <summary>
        /// Return the position of a Vector3 on a specified plane
        /// </summary>
        /// <param name="position"></param>
        /// <param name="plane"></param>
        /// <returns></returns>
        public static Vector3 GetPositionOnPlane(Vector3 position, Plane plane)
        {
            float distance;
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (plane.Raycast(ray, out distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                //Just double check to ensure the y position is exactly zero
                //hitPoint.y = 0;
                return hitPoint;
            }
            return Vector3.zero;
        }


        /// <summary>
        /// Rotate a point around a specified pivot by the specified angle
        /// </summary>
        /// <param name="point"></param>
        /// <param name="pivot"></param>
        /// <param name="angles"></param>
        /// <returns></returns>
        public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
        {
            return RotatePointAroundPivot(point, pivot, Quaternion.Euler(angles));
        }

        /// <summary>
        /// Rotate a point around a specified pivot by the specified quaternion
        /// </summary>
        /// <param name="point"></param>
        /// <param name="pivot"></param>
        /// <param name="quaternion"></param>
        /// <returns></returns>
        public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion quaternion)
        {
            Vector3 dir = point - pivot; // get point direction relative to pivot
            dir = quaternion * dir; // rotate it
            point = dir + pivot; // calculate rotated point
            return point; // return it
        }

        /// <summary>
        /// Gets the bounds for this and all children based upon their colliders.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public Bounds GetBoundsFromColliders(GameObject gameObject)
        {
            var totalBounds = new Bounds(Vector3.zero, Vector3.zero);
            foreach (var collider in gameObject.GetComponentsInChildren<Collider>())
            {
                if (totalBounds.extents == Vector3.zero)
                    totalBounds = collider.bounds;
                else
                    totalBounds.Encapsulate(collider.bounds);
            }

            return totalBounds;
        }
    }
}
//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Display.Placement
{

    public class PositionHelper : MonoBehaviour
    {
        public static Plane XzPlane = new Plane(Vector3.up, Vector3.zero);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetPositionOnXzPlane(Vector3 position)
        {
            return GetPositionOnPlane(position, XzPlane);
        }

        public static Vector3 GetPositionOnXzPlane(Vector3 position, float y)
        {
            return GetPositionOnPlane(position, new Plane(Vector3.up, new Vector3(0, y, 0)));
        }

        /// <summary>
        /// 
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



        public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
        {
            return RotatePointAroundPivot(point, pivot, Quaternion.Euler(angles));
        }

        public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion quaternion)
        {
            Vector3 dir = point - pivot; // get point direction relative to pivot
            dir = quaternion * dir; // rotate it
            point = dir + pivot; // calculate rotated point
            return point; // return it
        }

    }
}
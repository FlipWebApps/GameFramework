//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Display.Placement.Components
{
    /// <summary>
    /// Maintain a fixed distance from the specified transform
    /// </summary>
    public class MoveWithTransform : MonoBehaviour
    {
        public Transform Target;            // The position that that camera will be following.
        public float Smoothing = 5f;        // The Speed with which the camera will be following.

        Vector3 _offset;                     // The initial offset from the target.


        void Start ()
        {
            // Calculate the initial offset.
            _offset = transform.position - Target.position;
        }


        void FixedUpdate ()
        {
            // Create a postion the camera is aiming for based on the offset from the target.
            Vector3 targetCamPos = Target.position + _offset;

            // Smoothly interpolate between the camera's current position and it's target position.
            transform.position = Vector3.Lerp (transform.position, targetCamPos, Smoothing * Time.deltaTime);
        }
    }
}
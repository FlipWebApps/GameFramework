//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Display.Placement.Components
{
    /// <summary>
    /// Rotate so that the gameobject looks towards the specified transform
    /// </summary>
    public class LookAtTransform : MonoBehaviour
    {
        public Transform LookAtTansform;

        void LateUpdate()
        {
            //Billboard sprite
            Vector3 lookAtDir = new Vector3(LookAtTansform.position.x - transform.position.x, 0, LookAtTansform.position.z - transform.position.z);
            transform.rotation = Quaternion.LookRotation(lookAtDir.normalized, Vector3.up);
        }
    }
}
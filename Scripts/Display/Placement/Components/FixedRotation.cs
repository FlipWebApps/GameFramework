//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Display.Placement.Components
{
    /// <summary>
    /// Rotate this gameobject at a given rate.
    /// </summary>
    public class FixedRotation : MonoBehaviour
    {
        public Space Space;
        public float XAngle;
        public float YAngle;
        public float ZAngle;

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(XAngle * Time.deltaTime, YAngle * Time.deltaTime, ZAngle * Time.deltaTime, Space);
        }
    }
}
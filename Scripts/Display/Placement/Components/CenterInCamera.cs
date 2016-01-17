//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Display.Placement.Components
{
    /// <summary>
    /// Center this gameobject within the main camera.
    /// </summary>
    public class CenterInCamera : MonoBehaviour
    {
        void Update()
        {
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
        }
    }
}
//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameStructure;
using FlipWebApps.GameFramework.Scripts.GameStructure.Levels;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Display.Placement.Components
{
    /// <summary>
    /// Move this gameobject at a given rate.
    /// </summary>
    public class FixedMovement : MonoBehaviour
    {
        /// <summary>
        /// Scrolling Speed
        /// </summary>
        public Vector3 Speed = new Vector3(0, 0, 1);

        void Update()
        {
#pragma warning disable 618
            if (!LevelManager.Instance.IsLevelRunning || GameManager.Instance.IsPaused)
#pragma warning restore 618
                return;

            transform.Translate(Speed * Time.deltaTime);
        }
    }
}
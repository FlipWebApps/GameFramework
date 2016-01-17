//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Input.Components
{
    /// <summary>
    /// Quit the application when the escape key or android back button is pressed
    /// </summary>
    public class OnEscapeQuit : MonoBehaviour
    {
        void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

        }
    }
}
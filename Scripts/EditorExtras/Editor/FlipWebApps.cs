//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.EditorExtras.Editor {
    /// <summary>
    /// Flip Web Apps links and documentation
    /// </summary>

    public class FlipWebApps : MonoBehaviour {

        [MenuItem("Window/Flip Web Apps/Homepage", false, 1100)]
        static void ShowHomepage()
        { 
            Process.Start(@"http://www.flipwebapps.com/");
        }

        [MenuItem("Window/Flip Web Apps/Documentation", false, 1101)]
        static void ShowDocumentation()
        {
            Process.Start(@"http://www.flipwebapps.com/game-framework/");
        }

        [MenuItem("Window/Flip Web Apps/Contact", false, 1101)]
        static void ShowContact()
        {
            Process.Start(@"http://www.flipwebapps.com/contact/");
        }
    }
}
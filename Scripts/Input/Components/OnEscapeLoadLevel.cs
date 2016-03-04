//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameStructure;
using FlipWebApps.GameFramework.Scripts.UI.Other.Components;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Input.Components
{
    /// <summary>
    /// Loads the specified scene when the escape key or android back button is pressed
    /// </summary>
    public class OnEscapeLoadLevel : MonoBehaviour
    {
        public string SceneName;

        void Update()
        {
            if (!UnityEngine.Input.GetKeyDown(KeyCode.Escape)) return;

            GameManager.LoadSceneWithTransitions(SceneName);
        }
    }
}
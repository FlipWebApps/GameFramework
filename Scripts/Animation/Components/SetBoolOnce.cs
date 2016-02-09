//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameObjects.Components;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Animation.Components
{
    /// <summary>
    /// Set an animation bool only one time and optionally after another animation has already been triggered
    /// </summary>
    public class SetBoolOnce : RunOnceGameObject
    {
        public Animator Animator;
        public string Name;
        public bool Value;

        public override void RunOnce()
        {
            Animator.SetBool(Name, Value);
        }
    }
}
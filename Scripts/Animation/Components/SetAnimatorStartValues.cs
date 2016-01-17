//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Animation.Components
{
    /// <summary>
    /// Set the specified start values on the animator
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class SetAnimatorStartValues : MonoBehaviour
    {
        public string[] IntValueNames;
        public int[] IntValueValues;

        void Awake()
        {
            Animator animator = GetComponent<Animator>();
            animator.SetInteger(IntValueNames[0], IntValueValues[0]);
        }
    }
}
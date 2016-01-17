//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Animation.Components
{
    /// <summary>
    /// Set an animation trigger only one time and optionally after another animation has already been triggered
    /// </summary>
    public class SetTriggerOnce : MonoBehaviour
    {
        public Animator Animator;
        public string Trigger;
        public string Key;
        public string EnableAfterKey;

        void Start()
        {
            // show hint panel first time only
            if (string.IsNullOrEmpty(EnableAfterKey) || PlayerPrefs.GetInt("AnimationTriggerOnce." + EnableAfterKey, 0) == 1)
            {
                if (PlayerPrefs.GetInt("AnimationTriggerOnce." + Key, 0) == 0)
                {
                    PlayerPrefs.SetInt("AnimationTriggerOnce." + Key, 1);
                    PlayerPrefs.Save();

                    Animator.SetTrigger(Trigger);
                }
            }
        }
    }
}
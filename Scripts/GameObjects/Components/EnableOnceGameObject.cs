//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.GameObjects.Components
{
    /// <summary>
    /// An abstract class to show run something one time only.
    /// 
    /// Override and implement the condition as you best see fit.
    /// </summary>
    public abstract class RunOnceGameObject : RunOnState
    {
        public string Key;
        public string EnableAfterKey;

        public override void RunMethod()
        {
            // show hint panel first time only
            if (string.IsNullOrEmpty(EnableAfterKey) || PlayerPrefs.GetInt("AnimationTriggerOnce." + EnableAfterKey, 0) == 1)
            {
                if (PlayerPrefs.GetInt("AnimationTriggerOnce." + Key, 0) == 0)
                {
                    PlayerPrefs.SetInt("AnimationTriggerOnce." + Key, 1);
                    PlayerPrefs.Save();

                    RunOnce();
                }
            }
        }

        public abstract void RunOnce();
    }
}
//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.GameObjects.Components
{
    /// <summary>
    /// An abstract class to show an enabled or a disabled gameobject based upon the given condition.
    /// 
    /// Override and implement the condition as you best see fit.
    /// </summary>
    public abstract class EnableDisableGameObject : RunOnState
    {
        public GameObject ConditionMetGameObject;
        public GameObject ConditionNotMetGameObject;

        public override void RunMethod()
        {
            var isConditionMet = IsConditionMet();
            if (ConditionMetGameObject != null)
                ConditionMetGameObject.SetActive(isConditionMet);
            if (ConditionNotMetGameObject != null)
                ConditionNotMetGameObject.SetActive(!isConditionMet);
        }

        public abstract bool IsConditionMet();
    }
}
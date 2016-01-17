//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.GameObjects.Components
{
    /// <summary>
    /// Enables a component one time only. This can be useful for e.g. showing an animation the first time accesses a level.
    /// </summary>
    public class EnableComponentOnce : MonoBehaviour
    {
        public Behaviour Component;
        public string Key;
        public string EnableAfterKey;

        void Awake()
        {
            // show hint panel first time only
            var shouldEnable = false;
            if (string.IsNullOrEmpty(EnableAfterKey) || PlayerPrefs.GetInt("EnableComponentOnce." + EnableAfterKey, 0) == 1)
            {
                if (PlayerPrefs.GetInt("EnableComponentOnce." + Key, 0) == 0)
                {
                    PlayerPrefs.SetInt("EnableComponentOnce." + Key, 1);
                    PlayerPrefs.Save();

                    shouldEnable = true;
                }
            }

            Component.enabled = shouldEnable;
        }
    }
}
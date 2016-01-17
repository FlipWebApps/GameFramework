//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.GameObjects.Components
{
    /// <summary>
    /// Automatically destroys the GameObject when there are no children left.
    /// </summary>
    public class AutoDestructWhenNoChildren : MonoBehaviour
    {
        public bool OnlyDeactivate;

        void Update()
        {
            if (transform.childCount != 0) return;

            if (OnlyDeactivate)
                gameObject.SetActive(false);
            else
                Destroy(gameObject);
        }
    }
}
//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.GameObjects.Components
{
    /// <summary>
    ///  A persistant singleton implementation pattern that additionally allows for saving state upon exit or application pause events
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingletonPersistantSavedState<T> : SingletonSavedState<T> where T : Component
    {
        protected override void GameSetup()
        {
            DontDestroyOnLoad(gameObject);
            base.GameSetup();
        }
    }
}
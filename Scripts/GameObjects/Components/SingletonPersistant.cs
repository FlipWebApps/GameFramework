//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.GameObjects.Components
{
    /// <summary>
    ///  A singleton implementation pattern that is persistant across scene changes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonPersistant<T> : Singleton<T> where T : Component
    {
        protected override void GameSetup()
        {
            DontDestroyOnLoad(gameObject);
            base.GameSetup();
        }
    }
}
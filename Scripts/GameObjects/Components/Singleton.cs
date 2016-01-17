//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.Debugging;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.GameObjects.Components
{
    /// <summary>
    ///  A singleton implementation pattern
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        // Static singleton property
        public static T Instance { get; private set; }
        public string TypeName { get; private set; }

        public static bool IsActive
        {
            get
            {
                return Instance != null;
            }
        }

        void Awake()
        {
            TypeName = typeof(T).FullName;
            MyDebug.Log(TypeName + ": Awake");

            // First we check if there are any other instances conflicting then destroy this and return
            if (Instance != null)
            {
                if (Instance != this)
                    Destroy(gameObject);
                return;             // return is my addition so that the inspector in unity still updates
            }

            // Here we save our singleton instance
            Instance = this as T;

            // setup specifics.
            GameSetup();
        }

        void OnDestroy()
        {
            MyDebug.Log(TypeName + ": OnDestroy");

            if (Instance == this)
                GameDestroy();
        }

        protected virtual void GameSetup()
        {
        }

        protected virtual void GameDestroy()
        {
        }
    }
}
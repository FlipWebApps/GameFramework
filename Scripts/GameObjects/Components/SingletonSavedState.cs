//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.Debugging;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.GameObjects.Components
{
    /// <summary>
    ///  A singleton implementation pattern that additionally allows for saving state upon exit or application pause events
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingletonSavedState<T> : Singleton<T> where T : Component
    {
        protected override void GameDestroy()
        {
            MyDebug.Log(TypeName + "(PersistantSingletonSavedState): GameDestroy");
            SaveState();
        }

        public abstract void SaveState();

        //Note that iOS applications are usually suspended and do not quit. You should tick "Exit on Suspend" in Player settings for iOS builds to cause the game to quit and not suspend, otherwise you may not see this call. If "Exit on Suspend" is not ticked then you will see calls to OnApplicationPause instead.
        protected virtual void OnApplicationQuit()
        {
            MyDebug.Log(TypeName + "(PersistantSingletonSavedState): OnApplicationQuit");

            SaveState();
        }

        protected virtual void OnApplicationPause(bool pauseStatus)
        {
            MyDebug.Log(TypeName + "(PersistantSingletonSavedState): OnApplicationPause");

            SaveState();
        }

        //protected virtual void Reset()
        //{
        //    MyDebug.Log(TypeName + "(PersistantSingletonSavedState): Reset");

        //    SaveState();
        //}
    }
}
//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameObjects.Components;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Debugging.Components
{
    /// <summary>
    /// Used for logging exception information to a file in debug build mode.
    /// </summary>
    public class LogToDisk : SingletonPersistant<LogToDisk>
    {
        //Filename to assign log
        public string LogFileName = "log.txt";

        //Internal reference to stream writer object
        System.IO.StreamWriter _sw;

        protected override void GameSetup()
        {
            if (!Debug.isDebugBuild) return;

            base.GameSetup();

            //Create string writer object
            _sw = new System.IO.StreamWriter(Application.persistentDataPath + "/" + LogFileName);
            MyDebug.Log("ExceptionLogger to :" + Application.persistentDataPath + "/" + LogFileName);
        }

        protected override void GameDestroy()
        {
            //Close file
            if (_sw != null)
            {
                _sw.Close();
            }
            base.GameDestroy();
        }

        /// <summary>
        /// Register for exception listening, and log exceptions
        /// </summary>
        void OnEnable()
        {
            if (Debug.isDebugBuild)
            {
                Application.logMessageReceived += HandleLogMessage;
            }
        }

        /// <summary>
        /// Unregister for exception listening
        /// </summary>
        void OnDisable()
        {
            if (Debug.isDebugBuild)
            {
                Application.logMessageReceived -= HandleLogMessage;
            }
        }

        /// <summary>
        /// Log exception to a text file
        /// </summary>
        /// <param name="logString"></param>
        /// <param name="stackTrace"></param>
        /// <param name="type"></param>
        void HandleLogMessage(string logString, string stackTrace, LogType type)
        {
            //If an exception or error, then log to file
            if (type == LogType.Exception || type == LogType.Error)
            {
                _sw.WriteLine("Logged at: " + System.DateTime.Now + " - Log Desc: " + logString + " - Trace: " + stackTrace + " - Type: " + type);
            }
        }
    }
}
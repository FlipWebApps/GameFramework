using System.Collections;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Debugging.Components
{
    /// <summary>
    /// Displays the current log onto the screen for debugging purposes
    /// </summary>
    public class LogToScreen : MonoBehaviour
    {
        static string _myLog;
        static readonly Queue LogMessagesQueue = new Queue();
        public string Output = "";
        public string Stack = "";
        bool _hidden = true;
        public int MaxLines = 30;

        void OnEnable()
        {
            if (MyDebug.IsDebugBuildOrEditor)
            {
                Application.logMessageReceived += HandleLogMessage;
            }
        }

        void OnDisable()
        {
            if (MyDebug.IsDebugBuildOrEditor)
            {
                Application.logMessageReceived -= HandleLogMessage;
            }
        }

        void HandleLogMessage(string logString, string stackTrace, LogType type)
        {
            Output = logString;
            Stack = stackTrace;
            string newString = "\n [" + type + "] : " + Output;
            LogMessagesQueue.Enqueue(newString);
            if (type == LogType.Exception)
            {
                newString = "\n" + stackTrace;
                LogMessagesQueue.Enqueue(newString);
            }

            while (LogMessagesQueue.Count > MaxLines)
            {
                LogMessagesQueue.Dequeue();
            }

            _myLog = string.Empty;
            foreach (string s in LogMessagesQueue)
            {
                _myLog += s;
            }
        }

        void OnGUI()
        {
            if (MyDebug.IsDebugBuildOrEditor)
            {
                if (_hidden)
                {
                    if (GUI.Button(new Rect(Screen.width - 100, 10, 80, 20), "Show"))
                    {
                        Hide(false);
                    }
                }
                else
                {
                    GUI.TextArea(new Rect(0, 0, (float)Screen.width / 3, Screen.height), _myLog);
                    if (GUI.Button(new Rect(Screen.width - 100, 10, 80, 20), "Hide"))
                    {
                        Hide(true);
                    }
                }
            }
        }

        public void Hide(bool shouldHide)
        {
            _hidden = shouldHide;
        }
    }
}
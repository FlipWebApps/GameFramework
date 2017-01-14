//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//
// Please direct any bugs/comments/suggestions to http://www.flipwebapps.com
// 
// The copyright owner grants to the end user a non-exclusive, worldwide, and perpetual license to this Asset
// to integrate only as incorporated and embedded components of electronic games and interactive media and 
// distribute such electronic game and interactive media. End user may modify Assets. End user may otherwise 
// not reproduce, distribute, sublicense, rent, lease or lend the Assets. It is emphasized that the end 
// user shall not be entitled to distribute or transfer in any way (including, without, limitation by way of 
// sublicense) the Assets in any other way than as integrated components of electronic games and interactive media. 

// The above copyright notice and this permission notice must not be removed from any files.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//----------------------------------------------

// Windows Phone 8 .net profile doesn't include all features so set define to skip some code
#if UNITY_WP_8 || UNITY_WP_8_1
#define SKIP_CODE
#endif

using GameFramework.GameObjects.Components;
using UnityEngine;

namespace GameFramework.Debugging.Components
{
    /// <summary>
    /// Used for storing logging information to a file in debug build mode.
    /// </summary>
    [AddComponentMenu("Game Framework/Debugging/LogToDisk")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class LogToDisk : SingletonPersistant<LogToDisk>
    {
        /// <summary>
        /// Filename to save to
        /// </summary>
        [Tooltip("Filename to save to")]
        public string LogFileName = "log.txt";

        /// <summary>
        /// Whether this should only be active an debug builds.
        /// </summary>
        [Tooltip("Whether this should only be active an debug builds")]
        public bool DebugBuildsOnly = true;

        //Internal reference to stream writer object
        System.IO.StreamWriter _sw;

        protected override void GameSetup()
        {
            if (!Debug.isDebugBuild && DebugBuildsOnly) return;

            base.GameSetup();

#if !SKIP_CODE
            //Create string writer object
            _sw = new System.IO.StreamWriter(Application.persistentDataPath + "/" + LogFileName);
            MyDebug.Log("ExceptionLogger to :" + Application.persistentDataPath + "/" + LogFileName);
#endif
        }

        protected override void GameDestroy()
        {
#if !SKIP_CODE
            //Close file
            if (_sw != null)
            {
                _sw.Close();
            }
#endif
            base.GameDestroy();
        }

        /// <summary>
        /// Register for exception listening, and log exceptions
        /// </summary>
        void OnEnable()
        {
#if !SKIP_CODE
            if (Debug.isDebugBuild || !DebugBuildsOnly)
            {
                Application.logMessageReceived += HandleLogMessage;
            }
#endif
        }

        /// <summary>
        /// Unregister for exception listening
        /// </summary>
        void OnDisable()
        {
#if !SKIP_CODE
            if (Debug.isDebugBuild || !DebugBuildsOnly)
            {
                Application.logMessageReceived -= HandleLogMessage;
            }
#endif
        }

        /// <summary>
        /// Log exception to a text file
        /// </summary>
        /// <param name="logString"></param>
        /// <param name="stackTrace"></param>
        /// <param name="type"></param>
        void HandleLogMessage(string logString, string stackTrace, LogType type)
        {
#if !SKIP_CODE
            //If an exception or error, then log to file
            if (type == LogType.Exception || type == LogType.Error)
            {
                _sw.WriteLine("Logged at: " + System.DateTime.Now + " - Log Desc: " + logString + " - Trace: " + stackTrace + " - Type: " + type);
            }
#endif
        }
    }
}
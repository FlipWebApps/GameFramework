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

using System.Collections.Generic;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Messaging
{
    public enum LogEntryType { AddListener, RemoveListener, Send }

    /// <summary>
    /// Represents the message log.
    /// </summary>
    [System.Serializable]
    public class MessageLog : UnityEngine.ScriptableObject
    {
        public List<MessageLogEntry> LogEntries = new List<MessageLogEntry>();
        public bool ClearOnPlay = true;
        public System.Action LogEntryAdded;

        static public MessageLog Create()
        {
            var messageLog = ScriptableObject.FindObjectOfType<MessageLog>();

            if (messageLog == null)
            {
                messageLog = ScriptableObject.CreateInstance<MessageLog>();
            }

            return messageLog;
        }

        void OnEnable()
        {
            hideFlags = HideFlags.HideAndDontSave;
        }


        public void AddLogEntry(MessageLogEntry messageLogEntry)
        {
            LogEntries.Add(messageLogEntry);
            if (LogEntryAdded != null) LogEntryAdded();
        }

        public void Clear()
        {
            LogEntries.Clear();
        }
    }

    /// <summary>
    /// An instance within the message log
    /// </summary>
    [System.Serializable]
    public class MessageLogEntry
    {
        public LogEntryType LogEntryType;
        public System.DateTime Time;
        public string MessageType;
        public string Contents;
        public string Message;

        public MessageLogEntry(LogEntryType logEntryType, string messageType, string contents = null, string message = null)
        {
            LogEntryType = logEntryType;
            Time = System.DateTime.Now;
            MessageType = messageType;
            Contents = contents;
            Message = message;
        }
    }

    public static class MessageLogHandler {

#if UNITY_EDITOR
        /// <summary>
        /// Message Log.
        /// </summary>
        //public static List<MessageLogEntry> _messageLog = new List<MessageLogEntry>();
        public static MessageLog MessageLog { get; set; }
#endif

        /// <summary>
        /// Add a message to the log.
        /// </summary>
        /// <param name="logEntryType"></param>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void AddLogEntry(LogEntryType logEntryType, string messageType, string contents = null, string message = null)
        {
            if (MessageLog != null)
            {
                MessageLog.AddLogEntry(new MessageLogEntry(logEntryType, messageType, contents, message));
            }

        }
    }
}
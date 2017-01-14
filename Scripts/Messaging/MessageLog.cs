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
using System.Diagnostics;
using UnityEngine;

namespace GameFramework.Messaging
{
    public enum LogEntryType { AddListener, RemoveListener, Send }

    /// <summary>
    /// Represents the message log.
    /// </summary>
    [System.Serializable]
    public class MessageLog : ScriptableObject
    {
        public List<MessageLogEntry> LogEntries = new List<MessageLogEntry>();
        public bool ClearOnPlay = true;
        public System.Action LogEntryAdded;

        public Dictionary<string, MessageStatistics> Statistics = new Dictionary<string, MessageStatistics>();

        public static MessageLog Create()
        {
            var messageLog = FindObjectOfType<MessageLog>() ?? CreateInstance<MessageLog>();

            return messageLog;
        }

        void OnEnable()
        {
            hideFlags = HideFlags.HideAndDontSave;
            foreach (var messageLogEntry in LogEntries)
                UpdateStatistics(messageLogEntry);
        }


        public void AddLogEntry(MessageLogEntry messageLogEntry)
        {
            LogEntries.Add(messageLogEntry);
            UpdateStatistics(messageLogEntry);
            if (LogEntryAdded != null) LogEntryAdded();
        }


        void UpdateStatistics(MessageLogEntry messageLogEntry)
        {
            MessageStatistics count;
            var isExisting = Statistics.TryGetValue(messageLogEntry.MessageType, out count);
            if (!isExisting)
                count = new MessageStatistics();

            if (messageLogEntry.LogEntryType == LogEntryType.Send)
                count.MessageCount++;
            else if (messageLogEntry.LogEntryType == LogEntryType.AddListener)
                count.HandlerCount++;
            else if (messageLogEntry.LogEntryType == LogEntryType.RemoveListener)
                count.HandlerCount--;

            if (isExisting)
                Statistics[messageLogEntry.MessageType] = count;
            else
                Statistics.Add(messageLogEntry.MessageType, count);
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
        public StackTrace StackTrace;

        public MessageLogEntry() { }

        public MessageLogEntry(LogEntryType logEntryType, string messageType, string contents = null, string message = null, StackTrace stackTrace = null)
        {
            LogEntryType = logEntryType;
            Time = System.DateTime.Now;
            MessageType = messageType;
            Contents = contents;
            Message = message;
            StackTrace = stackTrace;
        }
    }

    // Windows Phone 8 .net profile doesn't include StackTrace so for that platform we substitute a dummy class
#if NETFX_CORE
    public class StackTrace
    {
        public StackTrace(bool dummy)
        {
        }

        public string[] GetFrames()
        {
            return null;
        }
    }
#endif

    /// <summary>
    /// An instance within the statistics
    /// </summary>
    public class MessageStatistics
    {
        public int MessageCount;
        public int HandlerCount;
    }


    /// <summary>
    /// Class for handling the message log and the go between to the editor window
    /// </summary>
    public static class MessageLogHandler {

        /// <summary>
        /// Message Log.
        /// </summary>
        public static MessageLog MessageLog { get; set; }

        /// <summary>
        /// Add a message to the log.
        /// </summary>
        /// <param name="logEntryType"></param>
        /// <param name="messageType"></param>
        /// <param name="contents"></param>
        /// <param name="message"></param>
        [Conditional("UNITY_EDITOR")]
        public static void AddLogEntry(LogEntryType logEntryType, string messageType, string contents = null, string message = null)
        {
            if (MessageLog != null)
            {
                MessageLog.AddLogEntry(new MessageLogEntry(logEntryType, messageType, contents, message, new StackTrace(true)));
            }
        }
    }
}
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

using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace GameFramework.Messaging
{
    /// <summary>
    /// A messaging system to allow for simple decoupling of components using either queued (QueueMessage) 
    /// or immediate execution (TriggerMessage). 
    /// 
    /// Exposing delegates from the individual classes directly would be slightly faster, but this is
    /// cleaner, gives us more control and allows for additional features such as prioritisation, logging 
    /// filtering etc. Delegates might still be used when we need a response or other functionality not
    /// covered by the Messenger class.
    /// </summary>
    public class Messenger
    {
        /// <summary>
        /// Our delegate type that listeners will need to implement.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public delegate bool MessageListenerDelegate(BaseMessage message);

        /// <summary>
        /// delegates list for the different message types. We maintain this as a list to allow for future
        /// possibilities in setting priority and call order etc.
        /// </summary>
        readonly Dictionary<string, List<MessageListenerDelegate>> _listeners = new Dictionary<string, List<MessageListenerDelegate>>();

        /// <summary>
        /// Messages waiting to be processed.
        /// </summary>
        readonly Queue<BaseMessage> _messageQueue = new Queue<BaseMessage>();

        #region Queue Processing

        /// <summary>
        /// Process the queue of messages.
        /// 
        /// Note: We should be careful to limit the time (without messaging we often would just 
        /// call these methods anyway), and also be aware that the system might not process low
        /// time resolutions
        /// low ranges
        /// </summary>
        public void ProcessQueue()
        {
            while (_messageQueue.Count > 0)
            {
                BaseMessage msg = _messageQueue.Dequeue();
                TriggerMessage(msg);
            }
        }

        #endregion Queue Processing

        #region Listener Registration

        /// <summary>
        /// Add a listener for the specified message type. Be sure to call RemoveListener when you are done.
        /// </summary>
        /// <param name="messageType">Type of the message to add a listener for</param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public void AddListener(Type messageType, MessageListenerDelegate handler)
        {
            var messageName = messageType.Name;
            if (!_listeners.ContainsKey(messageName))
            {
                _listeners.Add(messageName, new List<MessageListenerDelegate>());
            }

            List<MessageListenerDelegate> listenerList = _listeners[messageName];
            Assert.IsFalse(listenerList.Contains(handler), "You should not add the same listener multiple times for " + messageName);
            listenerList.Add(handler);

            MessageLogHandler.AddLogEntry(LogEntryType.AddListener, messageName);
        }


        /// <summary>
        /// Remove the listener from the specified message type.
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public void RemoveListener(Type messageType, MessageListenerDelegate handler)
        {
            var messageName = messageType.Name;
            Assert.IsTrue(_listeners.ContainsKey(messageName), "You are trying to remove a handler that a message type that isn't registered for " + messageName);

            List<MessageListenerDelegate> listenerList = _listeners[messageName];
            Assert.IsTrue(listenerList.Contains(handler), "You are trying to remove a handler that isn't registered for " + messageName);
            listenerList.Remove(handler);

            MessageLogHandler.AddLogEntry(LogEntryType.RemoveListener, messageName);
        }


        /// <summary>
        /// Add a listener for the specified message type. Be sure to call RemoveListener when you are done.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        /// <returns></returns>
        public void AddListener<T>(MessageListenerDelegate handler) where T : BaseMessage
        {
            var messageType = typeof(T);
            AddListener(messageType, handler);
        }


        /// <summary>
        /// Remove the listener from the specified message type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        /// <returns></returns>
        public void RemoveListener<T>(MessageListenerDelegate handler) where T : BaseMessage
        {
            var messageType = typeof(T);
            RemoveListener(messageType, handler);
        }

        #endregion Listener Registration

        #region Adding Messages and Sending

        /// <summary>
        /// Add the specified message to the processing queue for sending.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool QueueMessage(BaseMessage msg)
        {
            // if no listeners then just return.
            if (!_listeners.ContainsKey(msg.Name))
            {
                AddSendLogEntry(msg, "No listeners are setup. Discarding message!");
                return false;
            }

            _messageQueue.Enqueue(msg);
            return true;
        }

        /// <summary>
        /// Immediately send the specified message to all listeners.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool TriggerMessage(BaseMessage msg)
        {
            if (!_listeners.ContainsKey(msg.Name))
            {
                AddSendLogEntry(msg, "No listeners are setup. Discarding message!");
                return false;
            }

            List<MessageListenerDelegate> listenerList = _listeners[msg.Name];
            for (int i = 0; i < listenerList.Count; ++i)
            {
                var sent = listenerList[i](msg);

                if (msg.SendMode == BaseMessage.SendModeType.SendToFirst && sent)
                {
                    AddSendLogEntry(msg, "Sent to first listener.");
                    return true;
                }
            }
            AddSendLogEntry(msg, "Sent to " + listenerList.Count + " listeners.");
            return true;
        }

        static void AddSendLogEntry(BaseMessage msg, string message)
        {
#if UNITY_EDITOR
            if (!msg.DontShowInEditorLogInternal)
                MessageLogHandler.AddLogEntry(LogEntryType.Send, msg.Name, msg.ToString(), message);
#endif
        }

        #endregion Adding Messages and Sending
    }
}
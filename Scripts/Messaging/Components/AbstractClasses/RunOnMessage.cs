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

using GameFramework.GameStructure;
using UnityEngine;

#if NETFX_CORE
using System.Reflection;
#endif

namespace GameFramework.Messaging.Components.AbstractClasses
{
    /// <summary>
    /// An simple message listener abstract class that subscribes and unsubscribes to messages and 
    /// calls a method upon receipt.
    /// 
    /// Override and implement the handler as you best see fit.
    /// </summary>
    public abstract class RunOnMessage<T> : MonoBehaviour where T : BaseMessage
    {
        protected RunOnMessageAttribute.SubscribeTypeOptions SubscribeType = RunOnMessageAttribute.SubscribeTypeOptions.EnableDisable;
        bool _isListenerAdded = false;

        /// <summary>
        /// Get and record and attribute options.
        /// </summary>
        protected RunOnMessage()
        {
#if NETFX_CORE          
            var runOnMessageAttribute = typeof(RunOnMessage<T>).GetTypeInfo().GetCustomAttribute<RunOnMessageAttribute>();
#else
            var runOnMessageAttribute = (RunOnMessageAttribute)System.Attribute.GetCustomAttribute(typeof(RunOnMessage<T>), typeof(RunOnMessageAttribute));
#endif
            if (runOnMessageAttribute != null)
                SubscribeType = runOnMessageAttribute.SubscribeType;
        }

        /// <summary>
        ///  Register the listener if RunOnMessage attribute has SubscribeType of AwakeDestroy.
        ///  
        /// If you override this method then be sure to call the base function
        /// </summary>
        public virtual void Awake()
        {
            if (SubscribeType == RunOnMessageAttribute.SubscribeTypeOptions.AwakeDestroy)
            {
                _isListenerAdded = GameManager.SafeAddListener<T>(MessageListener);
            }
        }

        /// <summary>
        ///  Register the listener if RunOnMessage attribute has SubscribeType of StartDestroy.
        ///  
        /// If you override this method then be sure to call the base function
        /// </summary>
        public virtual void Start()
        {
            if (SubscribeType == RunOnMessageAttribute.SubscribeTypeOptions.StartDestroy)
            {
                _isListenerAdded = GameManager.SafeAddListener<T>(MessageListener);
            }
        }

        /// <summary>
        ///  Register the listener if RunOnMessage attribute has SubscribeType of EnableDisable or is not present.
        ///  
        /// If you override this method then be sure to call the base function
        /// </summary>
        public virtual void OnEnable()
        {
            if (SubscribeType == RunOnMessageAttribute.SubscribeTypeOptions.EnableDisable)
            {
                _isListenerAdded = GameManager.SafeAddListener<T>(MessageListener);
            }
        }

        /// <summary>
        /// Remove the listener if RunOnMessage attribute has SubscribeType of EnableDisable or is not present.
        ///  
        /// If you override this method then be sure to call the base function
        /// </summary>
        public virtual void OnDisable()
        {
            if (SubscribeType == RunOnMessageAttribute.SubscribeTypeOptions.EnableDisable && _isListenerAdded)
            {
                GameManager.SafeRemoveListener<T>(MessageListener);
            }
        }


        /// <summary>
        ///  Remove the listener if RunOnMessage attribute has SubscribeType of AwakeDestroy.
        ///  
        /// If you override this method then be sure to call the base function
        /// </summary>
        public virtual void OnDestroy()
        {
            if ((SubscribeType == RunOnMessageAttribute.SubscribeTypeOptions.AwakeDestroy ||
                SubscribeType == RunOnMessageAttribute.SubscribeTypeOptions.StartDestroy) && _isListenerAdded)
            {
                GameManager.SafeRemoveListener<T>(MessageListener);
            }
        }

        /// <summary>
        /// Receives a message and casts it to the correct type.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool MessageListener(BaseMessage message)
        {
            return RunMethod(message as T);
        }

        /// <summary>
        /// This is the method that you should implement that will be called when 
        /// a message is received.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public abstract bool RunMethod(T message);
    }

    /// <summary>
    /// An simple message listener abstract class that subscribes and unsubscribes to multiple messages and 
    /// calls a method upon receipt.
    /// 
    /// Override and implement the handler as you best see fit.
    /// </summary>
    public abstract class RunOnMessage<T1, T2> : MonoBehaviour where T1 : BaseMessage where T2 : BaseMessage
    {
        protected RunOnMessageAttribute.SubscribeTypeOptions SubscribeType = RunOnMessageAttribute.SubscribeTypeOptions.EnableDisable;
        bool _isListener1Added = false;
        bool _isListener2Added = false;

        /// <summary>
        /// Get and record and attribute options.
        /// </summary>
        protected RunOnMessage()
        {
#if NETFX_CORE          
            var runOnMessageAttribute = typeof(RunOnMessage<T1, T2>).GetTypeInfo().GetCustomAttribute<RunOnMessageAttribute>();
#else
            var runOnMessageAttribute = (RunOnMessageAttribute)System.Attribute.GetCustomAttribute(typeof(RunOnMessage<T1, T2>), typeof(RunOnMessageAttribute));
#endif
            if (runOnMessageAttribute != null)
                SubscribeType = runOnMessageAttribute.SubscribeType;
        }

        /// <summary>
        ///  Register the listener if RunOnMessage attribute has SubscribeType of AwakeDestroy.
        ///  
        /// If you override this method then be sure to call the base function
        /// </summary>
        public virtual void Awake()
        {
            if (SubscribeType == RunOnMessageAttribute.SubscribeTypeOptions.AwakeDestroy)
            {
                _isListener1Added = GameManager.SafeAddListener<T1>(MessageListener1);
                _isListener2Added = GameManager.SafeAddListener<T2>(MessageListener2);
            }
        }

        /// <summary>
        ///  Register the listener if RunOnMessage attribute has SubscribeType of StartDestroy.
        ///  
        /// If you override this method then be sure to call the base function
        /// </summary>
        public virtual void Start()
        {
            if (SubscribeType == RunOnMessageAttribute.SubscribeTypeOptions.StartDestroy)
            {
                _isListener1Added = GameManager.SafeAddListener<T1>(MessageListener1);
                _isListener2Added = GameManager.SafeAddListener<T2>(MessageListener2);
            }
        }

        /// <summary>
        ///  Register the listener if RunOnMessage attribute has SubscribeType of EnableDisable or is not present.
        ///  
        /// If you override this method then be sure to call the base function
        /// </summary>
        public virtual void OnEnable()
        {
            if (SubscribeType == RunOnMessageAttribute.SubscribeTypeOptions.EnableDisable)
            {
                _isListener1Added = GameManager.SafeAddListener<T1>(MessageListener1);
                _isListener2Added = GameManager.SafeAddListener<T2>(MessageListener2);
            }
        }

        /// <summary>
        /// Remove the listener if RunOnMessage attribute has SubscribeType of EnableDisable or is not present.
        ///  
        /// If you override this method then be sure to call the base function
        /// </summary>
        public virtual void OnDisable()
        {
            if (SubscribeType == RunOnMessageAttribute.SubscribeTypeOptions.EnableDisable && _isListener1Added)
            {
                GameManager.SafeRemoveListener<T1>(MessageListener1);
            }
            if (SubscribeType == RunOnMessageAttribute.SubscribeTypeOptions.EnableDisable && _isListener2Added)
            {
                GameManager.SafeRemoveListener<T1>(MessageListener2);
            }
        }


        /// <summary>
        ///  Remove the listener if RunOnMessage attribute has SubscribeType of AwakeDestroy.
        ///  
        /// If you override this method then be sure to call the base function
        /// </summary>
        public virtual void OnDestroy()
        {
            if ((SubscribeType == RunOnMessageAttribute.SubscribeTypeOptions.AwakeDestroy ||
                SubscribeType == RunOnMessageAttribute.SubscribeTypeOptions.StartDestroy) && _isListener1Added)
            {
                GameManager.SafeRemoveListener<T1>(MessageListener1);
            }
            if ((SubscribeType == RunOnMessageAttribute.SubscribeTypeOptions.AwakeDestroy ||
                SubscribeType == RunOnMessageAttribute.SubscribeTypeOptions.StartDestroy) && _isListener2Added)
            {
                GameManager.SafeRemoveListener<T1>(MessageListener2);
            }
        }

        /// <summary>
        /// Receives a message and casts it to the correct type.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool MessageListener1(BaseMessage message)
        {
            return RunMethod(message as T1);
        }

        /// <summary>
        /// This is the method that you should implement that will be called when 
        /// a message of type T1 is received.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public abstract bool RunMethod(T1 message);

        /// <summary>
        /// Receives a message and casts it to the correct type.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool MessageListener2(BaseMessage message)
        {
            return RunMethod(message as T2);
        }

        /// <summary>
        /// This is the method that you should implement that will be called when 
        /// a message of type T1 is received.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public abstract bool RunMethod(T2 message);
    }
}
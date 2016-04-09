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

using FlipWebApps.GameFramework.Scripts.GameStructure;
using UnityEngine;


namespace FlipWebApps.GameFramework.Scripts.Messaging.Components.AbstractClasses
{
    /// <summary>
    /// An simple message listener abstract class subscribes and unsubscribes to messages and 
    /// calls a method upon receipt.
    /// 
    /// Override and implement the handler as you best see fit.
    /// </summary>
    public abstract class RunOnMessage<T> : MonoBehaviour where T : BaseMessage
    {
        /// <summary>
        ///  Register the listener.
        /// </summary>
        void Start()
        {
            GameManager.Messenger.AddListener<T>(MessageListener);
            CustomStart();
        }

        /// <summary>
        ///  Register the listener.
        /// </summary>
        void OnDestroy()
        {
            if (GameManager.IsActive)
                GameManager.Messenger.RemoveListener<T>(MessageListener);
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
        /// Called during the Start() phase for your own custom initialisation.
        /// </summary>
        public virtual void CustomStart()
        {          
        }

        /// <summary>
        /// This is the method that you should implement that will be called when 
        /// a message is received.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public abstract bool RunMethod(T message);
    }
}
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

using GameFramework.Helper;
using UnityEngine;

namespace GameFramework.GameStructure.Game.ObjectModel.Abstract
{
    /// <summary>
    /// Base GameAction class that should be inherited by all other actions
    /// </summary>
    [System.Serializable]
    public abstract class GameAction : ScriptableObject, IScriptableObjectContainerSyncReferences
    {
        /// <summary>
        /// A delay in seconds before the action should be performed.
        /// </summary>
        public float Delay
        {
            get
            {
                return _delay;
            }
        }
        [Tooltip("A delay in seconds before the action should be performed.")]
        [SerializeField]
        float _delay;

        /// <summary>
        /// The runtime MonoBehaviour that contains this GameAction.
        /// </summary>
        public MonoBehaviour Owner { get; set; }

        /// <summary>
        /// An optional runtime reference to 'another' Object, either a GameObject or Component.
        /// </summary>
        public GameActionInvocationContext InvocationContext { get; set; }

        WaitForSeconds _waitForSeconds;

        /// <summary>
        /// Perform any internal initialisation for all GameActions before invoking Initialise
        /// </summary>
        /// <returns></returns>
        public void InitialiseInternal(MonoBehaviour owner)
        {
            Owner = owner;

            if (!Mathf.Approximately(0, Delay))
                _waitForSeconds = new WaitForSeconds(Delay);

            Initialise();
        }

        /// <summary>
        /// Override this method if you need to do any specific initialisation for the GameAction implementation.
        /// </summary>
        /// <returns></returns>
        protected virtual void Initialise() { }


        /// <summary>
        /// Perform all internal things for the GameAction before invoking PerformAction
        /// </summary>
        /// <returns></returns>
        public void ExecuteInternal(bool isStart)
        {
            if (_waitForSeconds != null)
                Owner.StartCoroutine(ExecuteCoroutine(isStart));
            else
                Execute(isStart);
        }


        /// <summary>
        /// Coroutine to wait a specified number of seconds before callind PerformAction()
        /// </summary>
        /// <returns></returns>
        protected System.Collections.IEnumerator ExecuteCoroutine(bool isStart)
        {
            yield return _waitForSeconds;
            Execute(isStart);
        }


        /// <summary>
        /// Implement this method to perform the actual action.
        /// </summary>
        /// <returns></returns>
        protected abstract void Execute(bool isStart);


        /// <summary>
        /// Override this method if you want to return a custom name for use in the editor
        /// </summary>
        /// <returns></returns>
        public string AutoName()
        {
            return null;
        }


        #region IScriptableObjectContainerSyncReferences

        /// <summary>
        /// Workaround for ObjectReference issues with ScriptableObjects (See ScriptableObjectContainer for details)
        /// </summary>
        /// <param name="objectReferences"></param>
        public virtual void SetReferencesFromContainer(UnityEngine.Object[] objectReferences)
        {
        }

        /// <summary>
        /// Workaround for ObjectReference issues with ScriptableObjects (See ScriptableObjectContainer for details)
        /// </summary>
        public virtual UnityEngine.Object[] GetReferencesForContainer()
        {
            return null;
        }

        #endregion IScriptableObjectContainerSyncReferences

        public class GameActionInvocationContext
        {
            /// <summary>
            /// An optional runtime reference to 'another' Object, either a GameObject or Component.
            /// </summary>
            public GameObject OtherGameObject { get; set; }
        }
    }
}

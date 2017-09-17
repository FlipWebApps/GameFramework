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

using GameFramework.GameStructure.GameItems.ObjectModel;
using UnityEngine;

namespace GameFramework.GameStructure.Game.ObjectModel.Abstract
{
    /// <summary>
    /// Base GameAction class that that allows for specifying the GameItem context
    /// </summary>
    /// NOTE: FromLoop mode we need to do in awake so ensure this is setup so we don't support that mode here
    /// jsut add a GameItemContext component and reference that if so needed.
    [System.Serializable]
    public abstract class GameActionGameItemContext : GameAction
    {
        /// <summary>
        /// GameItem Context to operate within.
        /// </summary>
        public GameItemContext Context
        {
            get { return _context; }
            set { _context = value; }
        }
        [Tooltip("The context that we are working within for determining what GameItem to use.")]
        [SerializeField]
        GameItemContext _context = new GameItemContext();


        /// <summary>
        /// Returns a reference to the GameItem that this context represents.
        /// </summary>
        public GameItem GameItem
        {
            get
            {
                // refresh if needed
                if (Context.ContextMode == GameItemContext.ContextModeType.Selected || Context.ContextMode == GameItemContext.ContextModeType.Reference || _gameItem == null)
                    GameItem = GameItemContext.GetGameItemFromContextReference(Context, GetIBaseGameItemManager(), GetType().Name);
                return _gameItem;
            }
            private set
            {
                _gameItem = value;
            }
        }
        GameItem _gameItem;


        /// <summary>
        /// Implement this method to return an IBaseGameItemManager that contains the GameItems that this works upon.
        /// </summary>
        /// <returns></returns>
        protected abstract IBaseGameItemManager GetIBaseGameItemManager();

        #region IScriptableObjectContainerSyncReferences


        /// <summary>
        /// Workaround for ObjectReference issues with ScriptableObjects (See ScriptableObjectContainer for details)
        /// </summary>
        /// <param name="objectReferences"></param>
        public override void SetReferencesFromContainer(UnityEngine.Object[] objectReferences)
        {
            if (objectReferences != null && objectReferences.Length >= 1)
                Context.ReferencedGameItemContextBase = objectReferences[0] as GameItems.Components.AbstractClasses.GameItemContextBase;
        }

        /// <summary>
        /// Workaround for ObjectReference issues with ScriptableObjects (See ScriptableObjectContainer for details)
        /// </summary>
        public override UnityEngine.Object[] GetReferencesForContainer()
        {
            var objectReferences = new Object[1];
            objectReferences[0] = Context.ReferencedGameItemContextBase;
            return objectReferences;
        }
        #endregion IScriptableObjectContainerSyncReferences

    }
}

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
using UnityEngine.Assertions;

namespace GameFramework.GameStructure.GameItems.Components.AbstractClasses
{
    /// <summary>
    /// abstract base for setting a shared context that other GameItemContext's can reference.
    /// </summary>
    /// This uses non generic types so that we can add references to instances of this through the Unity editor
    /// <typeparam name="T">The type of the GameItem that we are creating a button for</typeparam>
    public abstract class GameItemContextBase : MonoBehaviour
    {
        /// <summary>
        /// Shared context that other GameItemContext's can reference
        /// </summary>
        public GameItemContext Context
        {
            get { return _context; }
            set { _context = value; }
        }
        [Tooltip("The context that we are working within for determining what GameItem to use or for reference from other items.")]
        [SerializeField]
        GameItemContext _context = new GameItemContext();

        /// <summary>
        /// Returns a reference to the GameItem that this context represents.
        /// </summary>
        public GameItem GameItem
        {
            get
            {
                if (Context.ContextMode == GameItemContext.ContextModeType.Selected)
                {
                    // always get new reference incase selection has changed.
                    var gameItemManager = GetIBaseGameItemManager();
                    Assert.IsNotNull(gameItemManager, "GameItemManager not found. Verify that you have one setup and that the execution order is correct. Also with a mode of Selected you should only access GameItem in Start or later if no script execution order is setup.\nGameobject: " + gameObject.name);
                    GameItem = gameItemManager.BaseSelected;
                }
                else if (Context.ContextMode == GameItemContext.ContextModeType.ByNumber && _gameItem == null)
                {
                    var gameItemManager = GetIBaseGameItemManager();
                    Assert.IsNotNull(gameItemManager, "GameItemManager not found. Verify that you have one setup and that the execution order is correct. Also with a mode of ByNumber GetGameItem() should only be called in Start or later if no script execution order is setup.\nGameobject: " + gameObject.name);
                    GameItem = gameItemManager.BaseGetItem(Context.Number);
                    Assert.IsNotNull(_gameItem, "Could not find a GameItem with number " + Context.Number + " on gameobject: " + gameObject.name);
                }
                else if (Context.ContextMode == GameItemContext.ContextModeType.FromLoop && _gameItem == null)
                {
                    var gameItemManager = GetIBaseGameItemManager();
                    Assert.IsNotNull(gameItemManager, "GameItemManager not found. When using a GameItemContext reference mode of FromLoop ensure that it is placed within a looping scope.\nGameobject: " + gameObject.name);
                    GameItem = GetIBaseGameItemManager().BaseEnumeratorCurrent;
                    Assert.IsNotNull(_gameItem, "When using a GameItemContext reference mode of FromLoop ensure that it is placed within a looping scope.\nGameobject: " + gameObject.name);
                }
                else if (Context.ContextMode == GameItemContext.ContextModeType.Reference && _gameItem == null)
                {
                    Assert.IsNotNull(Context.ReferencedGameItemContextBase, "When using a GameItemContext reference mode of Reference ensure that you specify a valid reference.\nGameobject: " + gameObject.name);
                    GameItem = Context.ReferencedGameItemContextBase.GameItem;
                    Assert.IsNotNull(_gameItem, "When using a GameItemContext reference mode of Reference ensure that you are referencing a context of the same GameItem type (e.g. a Level can't reference a Character.\nGameobject: " + gameObject.name);
                }
                return _gameItem;
            }
            private set
            {
                _gameItem = value;
            }
        }
        GameItem _gameItem;


        /// <summary>
        /// Setup
        /// </summary>
        /// FromLoop mode we need to do in awake so ensure this is setup here, others types we can defer to Start or first use to try and 
        /// ensure other components such as GameItemManagers are setup.
        /// If you override Awake be sure to call this base method.
        protected virtual void Awake()
        {
            if (Context.ContextMode == GameItemContext.ContextModeType.FromLoop && GameItem == null)
            {
                var gameItemManager = GetIBaseGameItemManager();
                Assert.IsNotNull(gameItemManager, "GameItemManager not found. When using a GameItemContext reference mode of FromLoop ensure that it is placed within a looping scope.\nGameobject: " + gameObject.name);
                Assert.IsNotNull(gameItemManager.BaseEnumeratorCurrent, "When using a GameItemContext reference mode of FromLoop ensure that it is placed within a looping scope.\nGameobject: " + gameObject.name);
                GameItem = GetIBaseGameItemManager().BaseEnumeratorCurrent;
            }
        }


        /// <summary>
        /// Implement this method to return an IBaseGameItemManager that contains the GameItems that this works upon.
        /// </summary>
        /// <returns></returns>
        protected abstract IBaseGameItemManager GetIBaseGameItemManager();


        /// <summary>
        /// Returns a reference to the GameItem that this Context represents cast to type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetGameItem<T>() where T : GameItem
        {
            Assert.IsNotNull(GameItem as T, "You are trying to get a GameItem of type " + typeof(T) + " however the GameItem is of a different type. Ensure that you are referencing the correct types.");
            return GameItem as T;
        }
    }
}
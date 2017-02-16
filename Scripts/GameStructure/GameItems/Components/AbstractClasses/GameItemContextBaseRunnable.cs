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
    /// abstract base for showing a prefab from the selected item including updating to a new prefab when the selection changes.
    /// </summary>
    /// <typeparam name="T">The type of the GameItem that we are creating a button for</typeparam>
    public abstract class GameItemContextBaseRunnable<T> : GameItemContextBase where T : GameItem
    {
        ///// <summary>
        ///// What GameItem we should use for showing prefabs from.
        ///// </summary>
        //public ObjectModel.GameItemContext Context
        //{
        //    get { return _context; }
        //    set { _context = value; }
        //}
        //[Tooltip("Context that other GameItemContext's can reference.")]
        //[SerializeField]
        //ObjectModel.GameItemContext _context = new ObjectModel.GameItemContext();


        //protected GameItem GameItem
        //{
        //    get
        //    {
        //        if (Context.ContextMode == ObjectModel.GameItemContext.ContextModeType.Selected)
        //        {
        //            // always get new reference incase selection has changed.
        //            var gameItemManager = GetGameItemManager();
        //            Assert.IsNotNull(gameItemManager, "GameItemManager not found. Verify that you have one setup and that the execution order is correct. Also with a mode of Selected you should only access GameItem in Start or later if no script execution order is setup.\nGameobject: " + gameObject.name);
        //            GameItem = gameItemManager.Selected;
        //        }
        //        else if (Context.ContextMode == ObjectModel.GameItemContext.ContextModeType.ByNumber && _gameItem == null)
        //        {
        //            var gameItemManager = GetGameItemManager();
        //            Assert.IsNotNull(gameItemManager, "GameItemManager not found. Verify that you have one setup and that the execution order is correct. Also with a mode of ByNumber GetGameItem() should only be called in Start or later if no script execution order is setup.\nGameobject: " + gameObject.name);
        //            GameItem = gameItemManager.GetItem(Context.Number);
        //            Assert.IsNotNull(_gameItem, "Could not find a GameItem with number " + Context.Number + " on gameobject: " + gameObject.name);
        //        }
        //        else if (Context.ContextMode == ObjectModel.GameItemContext.ContextModeType.FromLoop && _gameItem == null)
        //        {
        //            var gameItemManager = GetGameItemManager();
        //            Assert.IsNotNull(gameItemManager, "GameItemManager not found. When using a GameItemContext reference mode of FromLoop ensure that it is placed within a looping scope.\nGameobject: " + gameObject.name);
        //            GameItem = GetGameItemManager().EnumeratorCurrent;
        //            Assert.IsNotNull(_gameItem, "When using a GameItemContext reference mode of FromLoop ensure that it is placed within a looping scope.\nGameobject: " + gameObject.name);
        //        }
        //        else if (Context.ContextMode == ObjectModel.GameItemContext.ContextModeType.Reference && _gameItem == null)
        //        {
        //            Assert.IsNotNull(Context.ReferencedGameItemContext, "When using a GameItemContext reference mode of Reference ensure that you specify a valid reference.");
        //            GameItem = Context.ReferencedGameItemContext.GameItem as T;
        //            Assert.IsNotNull(_gameItem, "When using a GameItemContext reference mode of Reference ensure that you are referencing a context of the same GameItem type (e.g. a Level can't reference a Character.");
        //        }
        //        return _gameItem;
        //    }
        //    set
        //    {
        //        _gameItem = value;
        //    }
        //}
        //GameItem _gameItem;


        //protected virtual void Awake()
        //{
        //    // FromLoop we need to do in awake so ensure this is setup here, others types we can defer to Start or first use to try and ensure other components
        //    // such as GameItemManagers are setup.
        //    if (Context.ContextMode == ObjectModel.GameItemContext.ContextModeType.FromLoop && GameItem == null)
        //    {
        //        var gameItemManager = GetGameItemManager();
        //        Assert.IsNotNull(gameItemManager, "GameItemManager not found. When using a GameItemContext reference mode of FromLoop ensure that it is placed within a looping scope.\nGameobject: " + gameObject.name);
        //        Assert.IsNotNull(gameItemManager.EnumeratorCurrent, "When using a GameItemContext reference mode of FromLoop ensure that it is placed within a looping scope.\nGameobject: " + gameObject.name);
        //        GameItem = GetGameItemManager().EnumeratorCurrent;
        //    }
        //}


        /// <summary>
        /// Setup
        /// </summary>
        protected virtual void Start()
        {
            if (Context.GetReferencedContextMode() == ObjectModel.GameItemContext.ContextModeType.Selected)
                GetGameItemManager().SelectedChanged += SelectedChanged;
            RunMethod(GameItem as T);
        }


        /// <summary>
        /// Destroy
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (Context.GetReferencedContextMode() == ObjectModel.GameItemContext.ContextModeType.Selected)
                GetGameItemManager().SelectedChanged -= SelectedChanged;
        }


        /// <summary>
        /// Returns the current GameItemManager as type IBaseGameItemManager
        /// </summary>
        /// <returns></returns>
        protected override IBaseGameItemManager GetIBaseGameItemManager()
        {
            return GetGameItemManager();
        }


        /// <summary>
        /// Implement this method to return a GameItemManager that contains the GameItems that this works upon.
        /// </summary>
        /// <returns></returns>
        protected abstract GameItemManager<T, GameItem> GetGameItemManager();


        /// <summary>
        /// Called if we are reacting to selection changes and the selection changes.
        /// </summary>
        /// <param name="oldItem"></param>
        /// <param name="item"></param>
        protected virtual void SelectedChanged(T oldItem, T item)
        {
            RunMethod(item, false);
        }


        /// <summary>
        /// You should implement this method which is called from start and optionally if the selection chages.
        /// </summary>
        /// <param name="gameItem"></param>
        /// <param name="isStart"></param>
        public abstract void RunMethod(T gameItem, bool isStart = true);
    }
}
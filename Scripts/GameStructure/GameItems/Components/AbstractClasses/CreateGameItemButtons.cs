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
using GameFramework.Messaging;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.GameStructure.GameItems.Components.AbstractClasses
{
    /// <summary>
    /// Creates instances of the passed prefab for all game items provided by the implementation class
    /// </summary>
    /// <typeparam name="TGameItemButton">The type of the Game Item button</typeparam>
    /// <typeparam name="TGameItem">The type of the Game Item</typeparam>
    public abstract class CreateGameItemButtons<TGameItemButton, TGameItem> : MonoBehaviour where TGameItemButton: GameItemButton<TGameItem> where TGameItem: GameItem, new()
    {
        /// <summary>
        /// A prefab that will be created for all GameItems.
        /// </summary>
        [Tooltip("A prefab that will be created for all GameItems.")]
        public GameObject Prefab;

        /// <summary>
        /// Override for the GameItemButtons selection mode
        /// </summary>
        /// How this is handled depends a bit on the exact implementation, however typically ClickThrough = go to next scene, Select = select item and remain
        [Tooltip("Override for the GameItemButtons selection mode.")]
        [Header("GameItemButton Defaults")]
        public GameItemButton.SelectionModeType SelectionMode;

        /// <summary>
        /// Override for the GameItemButtons name of the scene that should be loaded when this button is clicked.
        /// </summary>
        /// You can add the format parameter{0} to substitute in the current gameitems number to allow for different scenes for each gameitem.
        [Tooltip("Override for the GameItemButtons  name of the scene that should be loaded when this button is clicked.\n\nYou can add the format parameter{0} to substitute in the current gameitems number to allow for different scenes for each gameitem.")]
        public string ClickUnlockedSceneToLoad;

        /// <summary>
        /// Whether the user can unlock buttons directly by clicking if the GameItem has coin unlock enabled and they have enough coins.
        /// </summary>
        public bool ClickToUnlock
        {
            get { return _clickToUnlock; }
            set { _clickToUnlock = value; }
        }
        [Tooltip("Whether the user can unlock buttons directly by clicking if the GameItem has coin unlock enabled and they have enough coins.")]
        [SerializeField]
        bool _clickToUnlock;

        Stack<GameObject> _buttonGameObjects = new Stack<GameObject>();


        /// <summary>
        /// Create and add all buttons
        /// </summary>
        protected virtual void Awake()
        {
            CreateButtons();
        }


        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnDestroy()
        {
        }


        /// <summary>
        /// A listener that can be used to update the values when the display needs updating
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected bool OnMessageCreateButtons(BaseMessage message)
        {
            CreateButtons();
            return true;
        }


        void CreateButtons()
        {
            // first delete any old buttons
            while (_buttonGameObjects.Count != 0)
            {
                var buttonGameObject = _buttonGameObjects.Pop();
                Destroy(buttonGameObject);
            }

            var button = Prefab.GetComponent<TGameItemButton>();
#if UNITY_EDITOR
            // prefab values will get overwritten if running in editor mode so save and restore.
            var oldContext = button.Context.ContextMode;
            var oldSelectionMode = button.SelectionMode;
            var oldScene = button.ClickUnlockedSceneToLoad;
            var oldClickToUnlock = button.ClickToUnlock;
#endif
            button.Context.ContextMode = GameItemContext.ContextModeType.FromLoop;
            button.SelectionMode = SelectionMode;
            button.ClickUnlockedSceneToLoad = ClickUnlockedSceneToLoad;
            button.ClickToUnlock = ClickToUnlock;

#pragma warning disable 0168
            foreach (var gameItem in GetGameItemManager())
#pragma warning restore 0168
            {
                var newObject = Instantiate(Prefab);
                newObject.transform.SetParent(transform, false);
                _buttonGameObjects.Push(newObject);
            }

#if UNITY_EDITOR
            // prefab values will get overwritten if running in editor mode so save and restore.
            button.Context.ContextMode = oldContext;
            button.SelectionMode = oldSelectionMode;
            button.ClickUnlockedSceneToLoad = oldScene;
            button.ClickToUnlock = oldClickToUnlock;
#endif
        }


        /// <summary>
        /// Implement this method to return a GameItemManager that contains the GameItems that this works upon.
        /// </summary>
        /// <returns></returns>
        protected abstract GameItemManager<TGameItem, GameItem> GetGameItemManager();
    }
}
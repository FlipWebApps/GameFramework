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
using System.Linq;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;
using FlipWebApps.GameFramework.Scripts.Debugging;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.GameItems
{
    /// <summary>
    /// For managing an array of game items inlcuding selection, unlocking
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TParent"></typeparam>
    public class GameItemsManager<T, TParent> where T: GameItem, new() where TParent: GameItem
    {
        /// <summary>
        /// The unlock mode that will be used
        /// </summary>
        public GameItem.UnlockModeType UnlockMode;

        /// <summary>
        /// The full name of the type (T) that this GameItemsManager represents
        /// </summary>
        public string TypeNameFull = typeof(T).FullName;

        /// <summary>
        /// The type (T) name that this GameItemsManager represents
        /// </summary>
        public string TypeName = typeof(T).Name;

        /// <summary>
        /// A list of items of type T
        /// </summary>
        public T[] Items { get; set; }

        /// <summary>
        /// An optional Parent item
        /// </summary>
        public TParent Parent { get; set; }

        /// <summary>
        /// An action called when this GameItem is Unlocked. 
        /// </summary>
        /// Note: This may be replaced by global messaging in the future.
        public Action<T> Unlocked;

        /// <summary>
        /// An action called when the selection changes, passing the old and newly selected items
        /// </summary>
        /// Note: This may be replaced by global messaging in the future.
        public Action<T, T> SelectedChanged;

        readonly string _baseKey;

        /// <summary>
        /// The currently selected item
        /// </summary>
        /// The selected item number is persisted and loaded next time this GameItemsManager is created.
        public T Selected
        {
            get { return _selected; }
            set
            {
                T oldItem = Selected;
                _selected = value;
                if (SelectedChanged != null)
                    SelectedChanged(oldItem, Selected);
                GameManager.Instance.Player.SetSetting(_baseKey + "Selected" + TypeName, Selected.Number);
            }
        }
        T _selected;

        public GameItemsManager() : this(null) { }

        public GameItemsManager(TParent parent)
        {
            MyDebug.Log(TypeNameFull + ": Constructor");
            Parent = parent;

            // get the base key to use for any general settings for this item. If parent object we place it on that to avoid conflict if we have multiple instances of this e.g. worlds->levels
            _baseKey = parent == null ? "" : Parent.FullKey("");
        }

        /// <summary>
        /// Load method that will call the LoadItems() method before standard selection and unlock setup.
        /// </summary>
        public void Load()
        {
            LoadItems();
            Assert.AreNotEqual(Items.Length, 0, "You need to create 1 or more items in GameItemsManager.Load()");

            SetupSelectedItem();
            SetupUnlockedItems();
        }

        /// <summary>
        /// A method that you can override to manually setup the Items collection.
        /// </summary>
        /// If you want detault selection and unlock setting up then call Load() instead of this directly.
        protected virtual void LoadItems()
        {

        }

        /// <summary>
        /// Load method that will setup the Items collection using common defaults before standard selection and unlock setup.
        /// </summary>
        public void LoadDefaultItems(int startNumber, int lastNumber, int valueToUnlock = -1, bool loadFromResources = false)
        { 
            int count = (lastNumber + 1) - startNumber;     // e.g. if start == 1 and last == 1 then we still want to create item number 1
            Items = new T[count];

            for (var i = 0; i < count; i++)
            {
                Items[i] = new T();
                Items[i].Initialise(startNumber + i, valueToUnlock: valueToUnlock, loadFromResources : loadFromResources);
            }
            Assert.AreNotEqual(Items.Length, 0, "You need to create 1 or more items in GameItemsManager.Load()");

            SetupSelectedItem();
            SetupUnlockedItems();
        }

        /// <summary>
        /// Set the selected item from prefs if found, if not then the first item.
        /// </summary>
        void SetupSelectedItem()
        {
            // get the last selected item or default to the first
            int selectedNumber = GameManager.Instance.Player.GetSettingInt(_baseKey + "Selected" + TypeName, -1);
            foreach (T item in Items)
            {
                if (item.Number == selectedNumber)
                    Selected = item;
            }
            if (Selected == null)
                Selected = Items[0];
        }


        /// <summary>
        /// Make sure that any initial items are unlocked. 
        /// </summary>
        /// This includes the currently selected item and any items that have ValueTounlock set to 0.
        void SetupUnlockedItems()
        {
            Assert.IsNotNull(Selected, "Ensure you have a selected item (or default selected item before calling UnlockInitialItems");
            foreach (T item in Items)
            {
                // If this is the first run, the the selected item and other items with valuetoUnlock==0 might not be unlocked, 
                // so make sure they are
                if (item.Number == Selected.Number || (item.ValueToUnlock == 0 && !item.IsUnlocked))
                {
                    item.IsUnlocked = item.IsUnlockedAnimationShown = true;
                    item.UpdatePlayerPrefs();
                }
            }
        }

        /// <summary>
        /// Get the item with the specified number
        /// </summary>
        /// <param name="number"></param>
        /// <returns>A GameItem or null</returns>
        public T GetItem(int number)
        {
            return Items.FirstOrDefault(gameItem => gameItem.Number == number);
        }


        /// <summary>
        /// Get the item that comes after the currently selected item
        /// </summary>
        /// <returns>A GameItem or null</returns>
        public T GetNextItem()
        {
            return GetNextItem(Selected);
        }


        /// <summary>
        /// Get the item that comes after the item with the specified number
        /// </summary>
        /// <param name="number"></param>
        /// <returns>A GameItem or null</returns>
        public T GetNextItem(int number)
        {
            return GetItem(number + 1);
        }


        /// <summary>
        /// Get the item that comes after the specified item
        /// </summary>
        /// <param name="number"></param>
        /// <returns>A GameItem or null</returns>
        public T GetNextItem(T item)
        {
            return GetNextItem(item.Number);
        }

        /// <summary>
        /// Return a list of unlockable items whose value to unlock is less than the specified value
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="lockedOnly">Whether to return all matching unlockable items (default) or only currently locked ones</param>
        /// <returns></returns>
        public T[] UnlockableItems(int currentValue, bool lockedOnly = false)
        {
            return Items.Where(gameItem => (!lockedOnly || (lockedOnly && !gameItem.IsUnlocked)) && gameItem.ValueToUnlock > 0 && gameItem.ValueToUnlock <= currentValue).ToArray();
        }

        /// <summary>
        /// Returns the minimum value needed to unlock the item with the lowest ValueToUnlock.
        /// </summary>
        /// <param name="currentValue"></param>
        /// <returns></returns>
        public int MinimumValueToUnlock(int currentValue)
        {
            // Ssetup how many Coins to win to push them to get more.
            int minimumCoins = -1;
            foreach (T gameItem in Items)
            {
                if (!gameItem.IsUnlocked && (minimumCoins == -1 || gameItem.ValueToUnlock < minimumCoins))
                {
                    minimumCoins = gameItem.ValueToUnlock;
                }
            }
            return minimumCoins;
        }

        /// <summary>
        /// How much extra is needed to unlock the item with the lowest ValueToUnlock.
        /// </summary>
        /// <param name="currentValue"></param>
        /// <returns></returns>
        public int ExtraValueNeededToUnlock(int currentValue)
        {
            int minimumCoins = MinimumValueToUnlock(currentValue);
            if (minimumCoins == -1) return -1;
            minimumCoins -= currentValue; // deduct the Coins we already have.
            if (minimumCoins < 0) minimumCoins = 0;
            return minimumCoins;
        }
    }
}
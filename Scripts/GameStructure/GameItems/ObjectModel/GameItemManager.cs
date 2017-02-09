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
using UnityEngine;
using UnityEngine.Assertions;
using GameFramework.Debugging;
using GameFramework.GameStructure.Players.ObjectModel;
using GameFramework.Localisation.ObjectModel;
using GameFramework.Preferences;
using Random = UnityEngine.Random;

namespace GameFramework.GameStructure.GameItems.ObjectModel
{
    public class GameItemManager
    {
        /// <summary>
        /// The different modes for unlocking items.
        /// </summary>
        /// Placed in non generic class to avoid display issues in Unity Editor
        public enum UnlockModeType
        {
            RandomAll,
            RandomLocked,
            NextLocked
        }
    }

    /// <summary>
    /// For managing an array of game items inlcuding selection, unlocking
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TParent"></typeparam>
    public class GameItemManager<T, TParent> where T: GameItem where TParent: GameItem
    {
        /// <summary>
        /// The unlock mode that will be used
        /// </summary>
        public GameItem.UnlockModeType UnlockMode;

        /// <summary>
        /// The full name of the type (T) that this GameItemManager represents
        /// </summary>
        public string TypeNameFull = typeof(T).FullName;

        /// <summary>
        /// The type (T) name that this GameItemManager represents
        /// </summary>
        public string TypeName = typeof(T).Name;

        /// <summary>
        /// An array of items of type T
        /// </summary>
        public T[] Items { get; set; }

        /// <summary>
        /// The currently selected item
        /// </summary>
        /// The selected item number is persisted and loaded next time this GameItemManager is created.
        public T Selected
        {
            get { return _selected; }
            set
            {
                T oldItem = Selected;
                _selected = value;
                if (_isLoaded && oldItem.Number != _selected.Number)
                {
                    if (SelectedChanged != null)
                        SelectedChanged(oldItem, Selected);
                    OnSelectedChanged(Selected, oldItem);

                    if (_holdsPlayers)
                        PreferencesFactory.SetInt("Selected" + TypeName, Selected.Number);
                    else
                        GameManager.Instance.Player.SetSetting(_baseKey + "Selected" + TypeName, Selected.Number);
                }
            }
        }
        T _selected;

        /// <summary>
        /// An optional Parent item
        /// </summary>
        public TParent Parent { get; set; }

        #region Callbacks

        /// <summary>
        /// An action called when this GameItem is Unlocked. 
        /// </summary>
        public Action<T> Unlocked;

        /// <summary>
        /// An action called when the selection changes, passing the old and newly selected items
        /// </summary>
        public Action<T, T> SelectedChanged;

        #endregion Callbacks

        readonly string _baseKey;
        readonly bool _holdsPlayers;
        bool _isLoaded;


        public GameItemManager() : this(null) { }

        public GameItemManager(TParent parent)
        {
            MyDebug.Log(TypeNameFull + ": Constructor");
            Parent = parent;

            // get the base key to use for any general settings for this item. If parent object we place it on that to avoid conflict if we have multiple instances of this
            _baseKey = parent == null ? "" : Parent.FullKey("");

            // determine if holding players - if so we need to handle setting selected at global level.
            _holdsPlayers = TypeNameFull == typeof (Player).FullName;

        }

        
        /// <summary>
        /// Load method that will setup the Items collection using common defaults before standard selection and unlock setup. If loadFromResources
        /// is specified then this will try and load the GameItem from the resources folder
        /// </summary>
        public virtual void Load(int startNumber, int lastNumber, int valueToUnlock = -1, bool loadFromResources = false)
        {
            bool didLoadFromResources = false;
            var count = (lastNumber + 1) - startNumber;     // e.g. if start == 1 and last == 1 then we still want to create item number 1
            Items = new T[count];

            for (var i = 0; i < count; i++)
            {
                // preference is to load from resources.
                var loadedItem = GameItem.LoadFromResources<T>(TypeName, startNumber + i);
                if (loadedItem != null)
                {
                    Items[i] = loadedItem;
                    didLoadFromResources = true;
                    Items[i].InitialiseNonScriptableObjectValues(loadFromResources: loadFromResources);
                }
                else { 
                    MyDebug.LogWarning("Unable to find " + TypeName + " GameItem in resources folder " + TypeName + "\\" + TypeName + "_" + (startNumber + i) + " so using defaults. To get the most from Game Framework if is recommended to create a new item in this folder (right click the folder | Create | Game Framework).");
                    Items[i] = ScriptableObject.CreateInstance<T>();
                    Items[i].Initialise(startNumber + i, LocalisableText.CreateLocalised(), LocalisableText.CreateLocalised(), valueToUnlock: valueToUnlock, loadFromResources: loadFromResources);
                }
            }
            Assert.AreNotEqual(Items.Length, 0, "You need to create 1 or more items in GameItemManager.Load()");

            SetupSelectedItem();

            // if we didn't get any configuration files then unlock the selected item if it isn't already 
            // (otherwise unlock info comes directly from the config files).
            if (!didLoadFromResources && !Selected.IsUnlocked)
            {
                Selected.StartUnlocked = Selected.IsUnlocked = Selected.IsUnlockedAnimationShown = true;
                Selected.UpdatePlayerPrefs();
            }

            _isLoaded = true;
        }


        /// <summary>
        /// Set the selected item from prefs if found, if not then the first item.
        /// </summary>
        void SetupSelectedItem()
        {
            // get the previously selected item or default to the first - for player type we need to get setting at global level.
            var selectedNumber = _holdsPlayers ? 
                PreferencesFactory.GetInt("Selected" + TypeName, -1) : 
                GameManager.Instance.Player.GetSettingInt(_baseKey + "Selected" + TypeName, -1);

            foreach (T item in Items)
            {
                if (item.Number == selectedNumber)
                    Selected = item;
            }
            if (Selected == null)
                Selected = Items[0];
        }


        /// <summary>
        /// Called when the current selection changes. Override this in any base class to provide further handling such as sending out messaging.
        /// </summary>
        /// <param name="newSelection"></param>
        /// <param name="oldSelection"></param>
        /// You may want to override this in your derived classes to send custom messages.
        public virtual void OnSelectedChanged(T newSelection, T oldSelection)
        {
        }


        #region Get / Set Items

        /// <summary>
        /// Get the index of the item with the specified number
        /// </summary>
        /// <param name="number"></param>
        /// <returns>index or -1 if not found</returns>
        int GetItemIndex(int number)
        {
            for (var i = 0; i < Items.Length; i++)
                if (Items[i].Number == number)
                    return i;
            return -1;
        }

        /// <summary>
        /// Get the item with the specified number
        /// </summary>
        /// <param name="number"></param>
        /// <returns>A GameItem or null</returns>
        public T GetItem(int number)
        {
            var i = GetItemIndex(number);
            return i == -1 ? null : Items[i];
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
        /// Get the item that comes after the specified item
        /// </summary>
        /// <param name="item"></param>
        /// <returns>A GameItem or null</returns>
        public T GetNextItem(T item)
        {
            return GetNextItem(item.Number);
        }


        /// <summary>
        /// Get the item that comes after the item with the specified number
        /// </summary>
        /// <param name="number"></param>
        /// <returns>A GameItem or null</returns>
        public T GetNextItem(int number)
        {
            var i = GetItemIndex(number);
            if (i == -1) return null;
            return i + 1 < Items.Length ? Items[i + 1] : null;
        }


        /// <summary>
        /// Get the item that comes before the currently selected item
        /// </summary>
        /// <returns>A GameItem or null</returns>
        public T GetPreviousItem()
        {
            return GetPreviousItem(Selected);
        }


        /// <summary>
        /// Get the item that comes before the specified item
        /// </summary>
        /// <param name="item"></param>
        /// <returns>A GameItem or null</returns>
        public T GetPreviousItem(T item)
        {
            return GetPreviousItem(item.Number);
        }


        /// <summary>
        /// Get the item that comes before the item with the specified number
        /// </summary>
        /// <param name="number"></param>
        /// <returns>A GameItem or null</returns>
        public T GetPreviousItem(int number)
        {
            var i = GetItemIndex(number);
            if (i == -1) return null;
            return i > 0 ? Items[i - 1] : null;
        }


        /// <summary>
        /// Set the selected item to be the one with the specified number
        /// </summary>
        /// <param name="number"></param>
        public void SetSelected(int number)
        {
            Selected = GetItem(number);
        }


        /// <summary>
        /// Set the selected item to the specified item
        /// </summary>
        /// <param name="item"></param>
        public void SetSelected(T item)
        {
            Selected = item;
        }

        /// <summary>
        /// Set the selected item to the next item if one exists
        /// </summary>
        /// <param name="item"></param>
        public void SelectNext()
        {
            var nextItem = GetNextItem(Selected);
            if (nextItem != null)
                Selected = nextItem;
        }

        /// <summary>
        /// Set the selected item to the previous item if one exists
        /// </summary>
        /// <param name="item"></param>
        public void SelectPrevious()
        {
            var previousItem = GetPreviousItem(Selected);
            if (previousItem != null)
                Selected = previousItem;
        }


        #endregion get / set items

        #region Unlocking

        /// <summary>
        /// Return a list of coin unlockable items whose value to unlock is less than the specified value
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="lockedOnly">Whether to return all matching unlockable items (default) or only currently locked ones</param>
        /// <returns></returns>
        public T[] CoinUnlockableItems(int currentValue, bool lockedOnly = false)
        {
            return Items.Where(gameItem => !gameItem.StartUnlocked && gameItem.UnlockWithCoins && (!lockedOnly || !gameItem.IsUnlocked) && gameItem.ValueToUnlock <= currentValue).ToArray();
        }

        /// <summary>
        /// Return the first coin unlockable item whose value to unlock is less than the specified value
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="lockedOnly">Whether to return all matching unlockable items (default) or only currently locked ones</param>
        /// <returns></returns>
        public T CoinUnlockableItem(int currentValue = int.MaxValue)
        {
            foreach (var item in Items)
            {
                if (!item.IsUnlocked && item.UnlockWithCoins && item.ValueToUnlock <= currentValue)
                    return item;
            }
            return null;
        }

        [Obsolete("Use CoinUnlockableItems instead. This method will be removed in a future version.")]
        public T[] UnlockableItems(int currentValue, bool lockedOnly = false)
        {
            return CoinUnlockableItems(currentValue, lockedOnly);
        }

        /// <summary>
        /// Returns the minimum coins needed to unlock the item with the lowest ValueToUnlock.
        /// </summary>
        /// <returns></returns>
        public int MinimumCoinsToUnlock()
        {
            // Setup how many Coins to win to push them to get more.
            var minimumCoins = -1;
            var coinUnlockableItems = CoinUnlockableItems(int.MaxValue, true);
            foreach (var gameItem in coinUnlockableItems)
            {
                if (minimumCoins == -1 || gameItem.ValueToUnlock < minimumCoins)
                {
                    minimumCoins = gameItem.ValueToUnlock;
                }
            }
            return minimumCoins;
        }

        [Obsolete("Use MinimumValueToUnlock instead. This method will be removed in a future version.")]
        public int MinimumValueToUnlock() {
            return MinimumCoinsToUnlock();
        }

        /// <summary>
        /// How much extra is needed to unlock the item with the lowest ValueToUnlock.
        /// </summary>
        /// <param name="currentValue"></param>
        /// <returns></returns>
        public int ExtraCoinsNeededToUnlock(int currentValue)
        {
            var minimumCoins = MinimumCoinsToUnlock();
            if (minimumCoins == -1) return -1;
            minimumCoins -= currentValue; // deduct the Coins we already have.
            if (minimumCoins < 0) minimumCoins = 0;
            return minimumCoins;
        }

        [Obsolete("Use ExtraCoinsNeededToUnlock instead. This method will be removed in a future version.")]
        public int ExtraValueNeededToUnlock(int currentValue)
        {
            return ExtraCoinsNeededToUnlock(currentValue);
        }


        /// <summary>
        /// Returns whether 
        /// </summary>
        /// <param name="unlockMode"></param>
        /// <param name="coins"></param>
        /// <returns></returns>
        public bool CanCoinUnlockNewItem(GameItemManager.UnlockModeType unlockMode, int coins)
        {
            var canUnlock = false;
            if (unlockMode == GameItemManager.UnlockModeType.RandomAll || unlockMode == GameItemManager.UnlockModeType.RandomLocked)
            {
                var extraCoinsNeeded = ExtraCoinsNeededToUnlock(coins);
                canUnlock = extraCoinsNeeded != -1 && extraCoinsNeeded == 0;
            }
            else if (unlockMode == GameItemManager.UnlockModeType.NextLocked)
            {
                var gameItem = CoinUnlockableItem(coins);
                if (gameItem != null)
                    canUnlock = coins >= gameItem.ValueToUnlock;
            }

            return canUnlock;
        }


        /// <summary>
        /// Get an item that we can try unlocking based upon the specified coin unlock mode
        /// </summary>
        /// <param name="unlockMode"></param>
        /// <param name="coins"></param>
        /// <param name="failedUnlockAttempts"></param>
        /// <param name="maxFailedUnlocks"></param>
        /// <returns></returns>
        public T GetItemToCoinUnlock(GameItemManager.UnlockModeType unlockMode, int coins, int failedUnlockAttempts = 0, int maxFailedUnlocks = 0)
        {
            T gameItem = null;
            if (unlockMode == GameItemManager.UnlockModeType.RandomAll)
            {
                // If failed unlock attempts is greater then max then unlock one of the locked items so they don't get fed up.
                var gameItems = failedUnlockAttempts >= maxFailedUnlocks
                    ? CoinUnlockableItems(coins, true)
                    : CoinUnlockableItems(coins);
                if (gameItems.Length >= 0)
                    gameItem = gameItems[Random.Range(0, gameItems.Length)];
            }
            else if (unlockMode == GameItemManager.UnlockModeType.RandomLocked)
            {
                var gameItems = CoinUnlockableItems(coins, true);
                if (gameItems.Length >= 0)
                    gameItem = gameItems[Random.Range(0, gameItems.Length)];
            }
            else if (unlockMode == GameItemManager.UnlockModeType.NextLocked)
            {
                gameItem = CoinUnlockableItem(coins);
            }

            return gameItem;
        }
        #endregion Unlocking

    }
}
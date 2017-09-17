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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using GameFramework.Debugging;
using GameFramework.GameStructure.Players.ObjectModel;
using GameFramework.Localisation.ObjectModel;
using GameFramework.Preferences;
using Random = UnityEngine.Random;
using GameFramework.GameStructure.Game.ObjectModel;

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
            NextLocked,
            GameItem
        }
    }


    /// <summary>
    /// This interface is needed as Unity (5.3 to ...) doesn't properly support covariant generic interfaces. This interface will be deprecated when 
    /// Unity supports this so use the other methods if at all possible.
    /// </summary>
    public interface IBaseGameItemManager
    {
        /// <summary>
        /// Action that is called when teh selection changes.
        /// </summary>
        Action<GameItem, GameItem> BaseSelectedChanged { get; set; }

        /// <summary>
        /// The currently selected item
        /// </summary>
        /// The selected item number is persisted and loaded next time this GameItemManager is created.
        GameItem BaseSelected { get; set; }

        /// <summary>
        /// The last gameitem returned by the enumerator.
        /// </summary>
        GameItem BaseEnumeratorCurrent { get; }

        /// <summary>
        /// Get the item with the specified number
        /// </summary>
        /// <param name="number"></param>
        /// <returns>A GameItem or null</returns>
        GameItem BaseGetItem(int number);
    }

    /// <summary>
    /// For managing an array of game items inlcuding selection, unlocking
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TParent"></typeparam>
    public class GameItemManager<T, TParent> : IEnumerable<T>, IBaseGameItemManager where T: GameItem where TParent: GameItem
    {
        public T GetEnumeratorCurrent() {  return null; }

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
                    if (BaseSelectedChanged != null)
                        BaseSelectedChanged(oldItem, Selected);
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
        /// The last gameitem returned by the enumerator.
        /// </summary>
        public T EnumeratorCurrent { get; protected set; }

        /// <summary>
        /// An optional Parent item
        /// </summary>
        public TParent Parent { get; set; }

        #region Callbacks

        /// <summary>
        /// An action called when a GameItem is Unlocked. 
        /// </summary>
        public Action<T> Unlocked { get; set; }

        /// <summary>
        /// An action called when the selection changes, passing the old and newly selected items
        /// </summary>
        public Action<T, T> SelectedChanged { get; set; }

        #endregion Callbacks

        readonly string _baseKey;
        readonly bool _holdsPlayers;
        bool _isLoaded;


        public GameItemManager() : this(null) { }

        public GameItemManager(TParent parent)
        {
            MyDebug.Log(TypeNameFull + ": Constructor");
            Parent = parent;
            Items = new T[0];

            // get the base key to use for any general settings for this item. If parent object we place it on that to avoid conflict if we have multiple instances of this
            _baseKey = parent == null ? "" : Parent.FullKey("");

            // determine if holding players - if so we need to handle setting selected at global level.
            _holdsPlayers = TypeNameFull == typeof (Player).FullName;

        }


        /// <summary>
        /// Load method that will setup the Items collection using GameItem configuration files from the resources folder
        /// </summary>
        /// If any items are not able to be loaded then we will create a default item and show a warning.
        public void Load(int startNumber, int lastNumber, int valueToUnlock = -1, bool loadFromResources = false)
        {
            if (loadFromResources) Debug.LogWarning("Obsolete in v4.4: The Load() loadFromResources parameter no longer does anything. Either subclass the GameItems if you need custom data or call LoadData() on the GameItem manually.");
            if (valueToUnlock != -1) Debug.LogWarning("Obsolete in v4.4: The Load() valueToUnlock parameter no longer does anything as the value is loaded from the GameItem configuration files. Either call LoadAutomatic() or specify in configuration files instead.");

            var count = (lastNumber + 1) - startNumber;     // e.g. if start == 1 and last == 1 then we still want to create item number 1
            Items = new T[count];

#if UNITY_EDITOR
            // aggregate messages up to avoid spamming to log
            var containsCreatedGameItemsCount = 0;
            var containsCreatedGameItemsMessage = "";
#endif
            for (var i = 0; i < count; i++)
            {
                var loadedItem = GameItem.LoadFromResources<T>(TypeName, startNumber + i);
                if (loadedItem != null)
                {
                    Items[i] = loadedItem;
                    Items[i].InitialiseNonScriptableObjectValues(GameConfiguration.Instance, GameManager.Instance.Player, GameManager.Messenger);
                }
                else
                {
                    // otherwise create a default item on the fly - but show a warning.
#if UNITY_EDITOR
                    containsCreatedGameItemsCount++;
                    containsCreatedGameItemsMessage += TypeName + "\\" + TypeName + "_" + (startNumber + i) + "\n";
#endif
                    Items[i] = ScriptableObject.CreateInstance<T>();
                    Items[i].Initialise(GameConfiguration.Instance, GameManager.Instance.Player, GameManager.Messenger,
                        startNumber + i, LocalisableText.CreateLocalised(), LocalisableText.CreateLocalised(), valueToUnlock: 10);
                    Items[i].UnlockWithCoins = true;
                }
            }
            Assert.AreNotEqual(Items.Length, 0, "You need to create 1 or more items in GameItemManager.Load()");

#if UNITY_EDITOR
            if (containsCreatedGameItemsMessage.Length > 0)
                MyDebug.LogWarningF("GameItem Configuration resources files not found for {0} of {1} {2} GameItems so falling back to a default setup. Either use Automatic setup mode or to get the most out of Game Framework please create GameItem configuration files for the following (see the getting started tutorial for more details):\n{3}", containsCreatedGameItemsCount, count, TypeName, containsCreatedGameItemsMessage);
#endif

            SetupSelectedItem();
            _isLoaded = true;
        }


        /// <summary>
        /// Load method that will setup the GameItems using common defaults before standard selection and unlock setup. 
        /// </summary>
        public void LoadAutomatic(int startNumber, int lastNumber, int valueToUnlock = -1, bool unlockWithCompletion = false, bool unlockWithCoins = false)
        {
            var count = (lastNumber + 1) - startNumber;     // e.g. if start == 1 and last == 1 then we still want to create item number 1
            Assert.AreNotEqual(count, 0, "You need to create 1 or more items in GameItemManager.LoadAutomatic()");

            Items = new T[count];
            for (var i = 0; i < count; i++)
            {
                Items[i] = ScriptableObject.CreateInstance<T>();
                Items[i].Initialise(GameConfiguration.Instance, GameManager.Instance.Player, GameManager.Messenger,
                    startNumber + i, LocalisableText.CreateLocalised(), LocalisableText.CreateLocalised(), valueToUnlock: valueToUnlock);
                Items[i].UnlockWithCompletion = unlockWithCompletion;
                Items[i].UnlockWithCoins = unlockWithCoins;
            }

            SetupSelectedItem();

            // Ensure the first item is always unlocked.
            if (!Items[0].IsUnlocked)
            {
                Items[0].StartUnlocked = Items[0].IsUnlocked = Items[0].IsUnlockedAnimationShown = true;
                Items[0].UpdatePlayerPrefs();
            }

            _isLoaded = true;
        }


        /// <summary>
        /// Load method that will setup the GameItems using a master with overrides for specific levels. 
        /// </summary>
        public void LoadMasterWithOverrides(int startNumber, int lastNumber, GameItem master, NumberedGameItemReference<T>[] overrides)
        {
            var count = (lastNumber + 1) - startNumber;     // e.g. if start == 1 and last == 1 then we still want to create item number 1
            Assert.AreNotEqual(count, 0, "You need to create 1 or more items in GameItemManager.LoadMasterWithOverrides()");
            Assert.IsNotNull(master, "You must specify a master GameItem of type " + TypeName);

            Items = new T[count];
            for (var i = 1; i <= count; i++)
            {
                // check for any override
                var instance = master;
                foreach (var gameItemOverride in overrides)
                {
                    if (gameItemOverride.Number == i)
                    {
                        instance = gameItemOverride.GameItemReference;
                        break;
                    }
                }
                var gameItem = UnityEngine.Object.Instantiate(instance) as T; // create a copy so we don't overwrite values.
                Assert.IsNotNull(gameItem, "The gameItem for item " + i + " is not of type " + TypeName);
                gameItem.Number = i;
                gameItem.InitialiseNonScriptableObjectValues(GameConfiguration.Instance, GameManager.Instance.Player, GameManager.Messenger);
                Items[i-1] = gameItem;
            }

            SetupSelectedItem();

            // Ensure the first item is always unlocked but only if no override already in place for the first level.
            if (!Items[0].IsUnlocked && overrides.Length > 0 && overrides[0].Number != 1)
            {
                Items[0].StartUnlocked = Items[0].IsUnlocked = Items[0].IsUnlockedAnimationShown = true;
                Items[0].UpdatePlayerPrefs();
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
        public void SelectNext()
        {
            var nextItem = GetNextItem(Selected);
            if (nextItem != null)
                Selected = nextItem;
        }

        /// <summary>
        /// Set the selected item to the previous item if one exists
        /// </summary>
        public void SelectPrevious()
        {
            var previousItem = GetPreviousItem(Selected);
            if (previousItem != null)
                Selected = previousItem;
        }


        #endregion get / set items

        #region Unlocking

        /// <summary>
        /// Return a list of GameItems that can be unlocked with coins and whose value to unlock is less than the specified value
        /// </summary>
        /// <param name="currentValue">An optional value where only items with a value to unlock below this should be considered.</param>
        /// <param name="lockedOnly">Whether to return all matching unlockable items (default) or only currently locked ones</param>
        /// <returns></returns>
        public T[] CoinUnlockableItems(int currentValue = int.MaxValue, bool lockedOnly = true)
        {
            return Items.Where(gameItem => !gameItem.StartUnlocked && gameItem.UnlockWithCoins && (!lockedOnly || !gameItem.IsUnlocked) && gameItem.ValueToUnlock <= currentValue).ToArray();
        }

        /// <summary>
        /// Return the first coin unlockable item that is not already unlocked
        /// </summary>
        /// <param name="currentValue">An optional value where only items with a value to unlock below this should be considered.</param>
        /// <param name="lockedOnly">Whether to return all matching unlockable items (default) or only currently locked ones</param>
        /// <returns></returns>
        public T FirstCoinUnlockableItem(int currentValue = int.MaxValue, bool lockedOnly = true)
        {
            foreach (var gameItem in Items)
            {
                if (!gameItem.StartUnlocked && gameItem.UnlockWithCoins && (!lockedOnly || !gameItem.IsUnlocked) && gameItem.ValueToUnlock <= currentValue)
                    return gameItem;
            }
            return null;
        }

        /// <summary>
        /// Return whether there are more items that are locked that are coin unlockable.
        /// </summary>
        /// <returns></returns>
        public bool HasMoreLockedCoinUnlockableItems()
        {
            return FirstCoinUnlockableItem() != null;
        }

        [Obsolete("Use CoinUnlockableItems instead. This method will be removed in a future version.")]
        public T[] UnlockableItems(int currentValue, bool lockedOnly = false)
        {
            return CoinUnlockableItems(currentValue, lockedOnly);
        }

        /// <summary>
        /// Returns the minimum coins needed to unlock the locked, coin unlockable item with the lowest ValueToUnlock
        /// </summary>
        /// <returns></returns>
        public int MinimumCoinsNeededToUnlock()
        {
            // Setup how many Coins to win to push them to get more.
            var minimumCoins = -1;
            var coinUnlockableItems = CoinUnlockableItems();
            foreach (var gameItem in coinUnlockableItems)
            {
                if (minimumCoins == -1 || gameItem.ValueToUnlock < minimumCoins)
                {
                    minimumCoins = gameItem.ValueToUnlock;
                }
            }
            return minimumCoins;
        }

        [Obsolete("Use MinimumCoinsNeededToUnlock instead. This method will be removed in a future version.")]
        public int MinimumValueToUnlock() {
            return MinimumCoinsNeededToUnlock();
        }

        /// <summary>
        /// How much extra is needed to unlock the locked, coin unlockable item with the lowest ValueToUnlock.
        /// </summary>
        /// <param name="currentValue"></param>
        /// <returns></returns>
        public int ExtraCoinsNeededToUnlock(int currentValue)
        {
            var minimumCoins = MinimumCoinsNeededToUnlock();
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
        /// Returns whether we can try unlocking a new item. 
        /// </summary>
        /// Note that with certain modes such as RandomAll when we try unlocking we might not actually unlock an item.
        /// <param name="unlockMode"></param>
        /// <param name="coins"></param>
        /// <returns></returns>
        public bool CanTryCoinUnlocking(GameItemManager.UnlockModeType unlockMode, int coins)
        {
            var canUnlock = false;
            switch (unlockMode)
            {
                case GameItemManager.UnlockModeType.RandomAll:
                case GameItemManager.UnlockModeType.RandomLocked:
                    // can try unlocking if we have aenough coins to unlock a locked item
                    var extraCoinsNeeded = ExtraCoinsNeededToUnlock(coins);
                    canUnlock = extraCoinsNeeded != -1 && extraCoinsNeeded == 0;
                    break;
                case GameItemManager.UnlockModeType.NextLocked:
                    // can try unlocking if we have enough coins to unlock the next item
                    var gameItem = FirstCoinUnlockableItem();
                    if (gameItem != null)
                        canUnlock = coins >= gameItem.ValueToUnlock;
                    break;
                default:
                    MyDebug.LogWarning("Unhandled unlock mode in CanCoinUnlockNewItem.");
                    break;
            }

            return canUnlock;
        }


        /// <summary>
        /// Get an item that we can try unlocking based upon the specified coin unlock mode returning null if there is no item
        /// that we can unlock
        /// </summary>
        /// <param name="unlockMode"></param>
        /// <param name="coins"></param>
        /// <param name="failedUnlockAttempts"></param>
        /// <param name="maxFailedUnlocks">If using RandomAll mode then the maximum number of failed unlock attempts before they will be definately unlock an item - used to stop them waiting too long before they unlock.</param>
        /// <returns></returns>
        public T GetItemToTryCoinUnlocking(GameItemManager.UnlockModeType unlockMode, int coins, int failedUnlockAttempts = 0, int maxFailedUnlocks = 0)
        {
            T gameItem = null;
            switch (unlockMode) {
                case GameItemManager.UnlockModeType.RandomAll:
                case GameItemManager.UnlockModeType.RandomLocked:
                    // If RandomAll mode and not reached max failed unlocks then include items already unlocked.
                    var gameItems = CoinUnlockableItems(coins,
                        unlockMode == GameItemManager.UnlockModeType.RandomLocked ||
                         failedUnlockAttempts >= maxFailedUnlocks);
                    if (gameItems.Length > 0)
                        gameItem = gameItems[Random.Range(0, gameItems.Length - 1)];
                    break;
                case GameItemManager.UnlockModeType.NextLocked:
                    gameItem = FirstCoinUnlockableItem();
                    if (gameItem != null && gameItem.ValueToUnlock > coins)
                        gameItem = null;
                    break;
                default:
                    MyDebug.LogWarning("Unhandled unlock mode in CanCoinUnlockNewItem.");
                    break;
            }

            return gameItem;
        }
        #endregion Unlocking

        #region IEnumerable

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T item in Items)
            {
                // Sanity check
                if (item == null)
                {
                    break;
                }

                // Return the current element and then on next function call 
                // resume from next element rather than starting all over again;
                EnumeratorCurrent = item;
                yield return item;
            }
            EnumeratorCurrent = null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion IEnumerable

        #region IBaseGameItemManager

        public GameItem BaseSelected {
            get { return Selected;  }
            set { Selected = (T)value; }
        }

        public GameItem BaseEnumeratorCurrent
        {
            get { return EnumeratorCurrent; }
        }

        public GameItem BaseGetItem(int number)
        {
            return GetItem(number);
        }

        public Action<GameItem, GameItem> BaseSelectedChanged { get; set; }

        #endregion IBaseGameItemManager

    }
}
//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.GameItems
{
    /// <summary>
    /// For managing an array of game items inlcuding selection, unlocking
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TParent"></typeparam>
    public abstract class GameItemsManager<T, TParent> where T: GameItem where TParent: GameItem
    {
        public string TypeNameFull = typeof(T).FullName;
        public string TypeName = typeof(T).Name;

        public T[] Items { get; set; }
        public TParent Parent { get; set; }

        // some standard actions that might be needed
        public Action<T> Unlocked;
        public Action<T, T> SelectedChanged;

        readonly string _baseKey;

        T _selected;
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

        protected GameItemsManager() : this(null) { }

        protected GameItemsManager(TParent parent)
        {
            Debug.Log(TypeNameFull + ": Constructor");
            Parent = parent;

            // get the base key to use for any general settings for this item. If parent object we place it on that to avoid conflict if we have multiple instances of this e.g. worlds->levels
            _baseKey = parent == null ? "" : Parent.FullKey("");


        }

        public void Load()
        {
            LoadItems();

            Assert.AreNotEqual(Items.Length, 0, "You need to create 1 or more items in GameItemsManager.Load()");

            // get the last selected item or default to the first
            int selectedNumber = GameManager.Instance.Player.GetSettingInt(_baseKey + "Selected" + TypeName, -1);
            if (selectedNumber == -1) selectedNumber = Items[0].Number;
            foreach (T item in Items)
            {
                if (item.Number == selectedNumber)
                    Selected = item;

                // If this is the first run, the the selected item and other items with valuetoUnlock==0 might not be unlocked, 
                // so make sure they are
                if (item.Number == selectedNumber || (item.ValueToUnlock == 0 && !item.IsUnlocked))
                {
                    item.IsUnlocked = item.IsUnlockedAnimationShown = true;
                    item.UpdatePlayerPrefs();
                }
            }
        }

        protected abstract void LoadItems();

        public T GetItem(int number)
        {
            return Items.FirstOrDefault(gameItem => gameItem.Number == number);
        }

        public T[] UnlockableItems(int currentValue, bool lockedOnly = false)
        {
            return Items.Where(gameItem => (!lockedOnly || (lockedOnly && !gameItem.IsUnlocked)) && gameItem.ValueToUnlock > 0 && gameItem.ValueToUnlock <= currentValue).ToArray();
        }

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
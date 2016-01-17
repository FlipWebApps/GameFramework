//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.Components
{
    /// <summary>
    /// Creates instances of the passed prefab for all game items provided by the implementation class
    /// </summary>
    /// <typeparam name="TGameItemButton">The type of the Game Item button</typeparam>
    /// <typeparam name="TGameItem">The type of the Game Item</typeparam>
    public abstract class CreateGameItemButtons<TGameItemButton, TGameItem> : MonoBehaviour where TGameItemButton: GameItemButton<TGameItem> where TGameItem: GameItem
    {
        public GameObject Prefab;

        public void Awake()
        {
            foreach (GameItem gameItem in GetGameItems())
            {
                TGameItemButton button = Prefab.GetComponent<TGameItemButton>();
                button.Number = gameItem.Number;

                GameObject newObject = Instantiate(Prefab);
                newObject.transform.SetParent(transform, false);
            }
        }

        protected abstract GameItem[] GetGameItems();
    }
}
//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameStructure.Characters.ObjectModel;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.Components;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Characters.Components
{
    /// <summary>
    /// Unlock GameItem button for Characters 
    /// </summary>
    public class UnlockCharacterButton : UnlockGameItemButton<Character>
    {
        protected override GameItemsManager<Character, GameItem> GetGameItemsManager()
        {
            return GameManager.Instance.Characters;
        }
    }
}
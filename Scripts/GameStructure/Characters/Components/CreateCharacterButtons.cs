//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameStructure.Characters.ObjectModel;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.Components;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Characters.Components
{
    /// <summary>
    /// Creates instances of all Character Game Items
    /// </summary>
    public class CreateCharacterButtons : CreateGameItemButtons<CharacterButton, Character>
    {
        protected override GameItem[] GetGameItems()
        {
            return GameManager.Instance.Characters.Items;
        }
    }
}
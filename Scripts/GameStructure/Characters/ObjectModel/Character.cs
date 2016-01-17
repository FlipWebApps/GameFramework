//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;
using FlipWebApps.GameFramework.Scripts.GameStructure.Players.ObjectModel;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Characters.ObjectModel
{
    /// <summary>
    /// Character Game Item
    /// </summary>
    public class Character : GameItem
    {
        public Character(int levelNumber, string name = null, bool localiseName = true, string description = null, bool localiseDescription = true, Sprite sprite = null, int valueToUnlock = -1, Player player = null) //, GameItem parent = null)
            : base(levelNumber, name: name, localiseName: localiseName, description: description, localiseDescription: localiseDescription, sprite: sprite, valueToUnlock: valueToUnlock, player: player, identifierBase: "Character", identifierBasePrefs: "C") //parent: parent, 
        {
        }
    }
}

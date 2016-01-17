//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;
using FlipWebApps.GameFramework.Scripts.GameStructure.Players.ObjectModel;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Levels.ObjectModel
{
    /// <summary>
    /// Level Game Item
    /// </summary>
    public class Level : GameItem
    {
        public static string IdBase = "Level";

        public Level(int levelNumber, string name = null, bool localiseName = true, string description = null, bool localiseDescription = true, Sprite sprite = null, int valueToUnlock = -1, Player player = null, bool loadFromResources = false) //, GameItem parent = null)
            : base(levelNumber, name: name, localiseName: localiseName, description: description, localiseDescription: localiseDescription, sprite: sprite, valueToUnlock: valueToUnlock, player: player, identifierBase: "Level", identifierBasePrefs: "L", loadFromResources : loadFromResources) //parent: parent, 
        {
        }
    }
}

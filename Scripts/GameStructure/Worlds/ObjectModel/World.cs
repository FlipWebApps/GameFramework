//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;
using FlipWebApps.GameFramework.Scripts.GameStructure.Players.ObjectModel;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Worlds.ObjectModel
{
    /// <summary>
    /// World Game Item
    /// </summary>
    public class World : GameItem
    {
        public int SelectedLevel;

        public World(int number, string name = null, bool localiseName = true, string description = null, bool localiseDescription = true, Sprite sprite = null, int valueToUnlock = -1, Player player = null) //, GameItem parent = null)
            : base(number, name: name, localiseName: localiseName, description: description, localiseDescription: localiseDescription, sprite: sprite, valueToUnlock: valueToUnlock, player: player, identifierBase: "World", identifierBasePrefs: "W") //parent: parent, 
        {
            //SelectedLevel = GameManager.Instance.Player.GetSettingInt(FullKey("SelectedLevel"), -1);
        }

        //public override void UpdatePlayerPrefs()
        //{
        //    GameManager.Instance.Player.SetSetting(FullKey("SelectedLevel"), SelectedLevel);

        //    base.UpdatePlayerPrefs();
        //}
    }
}

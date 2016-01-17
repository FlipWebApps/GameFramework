//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Players.ObjectModel
{
    /// <summary>
    /// Player Game Item
    /// </summary>

    public class Player : GameItem
    {
        public int MaximumWorld;
        public int MaximumLevel;
        public int SelectedWorld;
        public int SelectedLevel;   // only use when not using worlds, other use World.SelectedLevel for world specific level.

        public Player(int playerNumber)
            : base(playerNumber, identifierBase: "Player", identifierBasePrefs: "P", localiseDescription: false)
        {
            Reset();

            Name = GetSettingString("Name", Name);

            Score = GetSettingInt("TotalScore", 0);
            Coins = GetSettingInt("TotalCoins", 0);

            MaximumWorld = GetSettingInt("MaximumWorld", MaximumWorld);
            MaximumLevel = GetSettingInt("MaximumLevel", MaximumLevel);
            SelectedWorld = GetSettingInt("SelectedWorld", SelectedWorld);
            SelectedLevel = GetSettingInt("SelectedLevel", SelectedLevel);
        }

        public virtual void Reset()
        {
            MaximumWorld = 0;
            MaximumLevel = 0;
            SelectedWorld = 0;
            SelectedLevel = 0;

            Score = 0;
            Coins = 0;
        }

        public override void UpdatePlayerPrefs()
        {
            SetSetting("Name", Name);

            SetSetting("TotalScore", Score);
            SetSetting("TotalCoins", Coins);

            SetSetting("MaximumWorld", MaximumWorld);
            SetSetting("MaxLevel", MaximumLevel);
            SetSetting("SelectedWorld", SelectedWorld);
            SetSetting("SelectedLevel", SelectedLevel);

            base.UpdatePlayerPrefs();
        }
    }
}
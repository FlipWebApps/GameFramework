//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.Components;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;
using FlipWebApps.GameFramework.Scripts.GameStructure.Levels.ObjectModel;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Levels.Components
{
    public class UnlockLevelButton : UnlockGameItemButton<Level>
    {
        protected override GameItemsManager<Level, GameItem> GetGameItemsManager()
        {
            return GameManager.Instance.Levels;
        }
    }
}
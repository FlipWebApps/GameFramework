//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.Components;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;
using FlipWebApps.GameFramework.Scripts.GameStructure.Levels.ObjectModel;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Levels.Components
{
    /// <summary>
    /// Creates instances of all Level GameItems
    /// </summary>
    public class CreateLevelButtons : CreateGameItemButtons<LevelButton, Level>
    {
        protected override GameItem[] GetGameItems()
        {
            return GameManager.Instance.Levels.Items;
        }
    }
}
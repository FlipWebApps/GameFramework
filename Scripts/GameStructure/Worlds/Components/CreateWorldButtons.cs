//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.Components;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;
using FlipWebApps.GameFramework.Scripts.GameStructure.Worlds.ObjectModel;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Worlds.Components
{
    /// <summary>
    /// Creates instances of all World GameItems
    /// </summary>
    public class CreateWorldButtons : CreateGameItemButtons<WorldButton, World>
    {
        protected override GameItem[] GetGameItems()
        {
            return GameManager.Instance.Worlds.Items;
        }
    }
}
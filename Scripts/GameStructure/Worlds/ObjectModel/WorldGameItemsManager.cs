//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Worlds.ObjectModel
{
    /// <summary>
    /// A simple implementation of a World GameItemsManager for setting up Worlds using the standard object model class.
    /// 
    /// Name and Description are collected from the localisation file.
    /// </summary>
    public class WorldGameItemsManager : GameItemsManager<World, GameItem>
    {
        readonly int _numberOfWorlds;

        public WorldGameItemsManager(int numberOfWorlds)
        {
            _numberOfWorlds = numberOfWorlds;
        }

        protected override void LoadItems()
        {
            Items = new World[_numberOfWorlds];

            for (var count = 0; count < _numberOfWorlds; count++ )
            {
                Items[count] = new World(count + 1);
            }
        }
    }
}
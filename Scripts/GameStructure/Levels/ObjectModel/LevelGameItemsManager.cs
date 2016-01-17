using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Levels.ObjectModel
{
    /// <summary>
    /// A simple implementation of a Level GameItemsManager for setting up Levels using the standard object model class.
    /// 
    /// Name and Description are collected from the localisation file.
    /// </summary>
    public class LevelGameItemsManager : GameItemsManager<Level, GameItem>
    {
        public enum LevelUnlockModeType { Custom, Completion, Coins }

        readonly int _numberOfLevels;
        readonly LevelUnlockModeType _levelUnlockModeType;
        readonly int _valueToUnlock;

        public LevelGameItemsManager(int numberOfLevels, LevelUnlockModeType levelUnlockModeType, int valueToUnlock)
        {
            _numberOfLevels = numberOfLevels;
            _levelUnlockModeType = levelUnlockModeType;
            _valueToUnlock = valueToUnlock;
        }

        protected override void LoadItems()
        {
            Items = new Level[_numberOfLevels];

            for (var count = 0; count < _numberOfLevels; count++ )
            {
                if (_levelUnlockModeType == LevelUnlockModeType.Completion)
                {
                    Items[count] = new Level(count + 1);
                }
                else
                {
                    Items[count] = new Level(count + 1, valueToUnlock: _valueToUnlock);
                }
            };
        }
    }
}
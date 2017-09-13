//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//
// Please direct any bugs/comments/suggestions to http://www.flipwebapps.com
// 
// The copyright owner grants to the end user a non-exclusive, worldwide, and perpetual license to this Asset
// to integrate only as incorporated and embedded components of electronic games and interactive media and 
// distribute such electronic game and interactive media. End user may modify Assets. End user may otherwise 
// not reproduce, distribute, sublicense, rent, lease or lend the Assets. It is emphasized that the end 
// user shall not be entitled to distribute or transfer in any way (including, without, limitation by way of 
// sublicense) the Assets in any other way than as integrated components of electronic games and interactive media. 

// The above copyright notice and this permission notice must not be removed from any files.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

namespace GameFramework.GameStructure.Game.ObjectModel
{
    /// <summary>
    /// Allows for setting of global game configuration. This is seperate from GameManager and a Scriptable Object so that it
    /// can be used from unit testing and loaded by editor components etc.
    /// </summary>
    [CreateAssetMenu(fileName = "GameConfiguration", menuName = "Game Framework/Game Configuration", order = 30)]
    [System.Serializable]
    public class GameConfiguration : ScriptableObject
    {
        public enum GameItemType { Character, Level, Player, World }

        #region Static Singleton Reference
        static GameConfiguration _instance;
        public static GameConfiguration Instance
        {
            get
            {
                if (_instance == null)
                    _instance = LoadSingletonGameConfiguration();
                if (_instance == null)
                    _instance = CreateInstance<GameConfiguration>(); // fallback to default
                return _instance;
            }
        }

        /// <summary>
        /// Load the default GameConfiguration
        /// </summary>
        static GameConfiguration LoadSingletonGameConfiguration()
        {
            return GameManager.LoadResource<GameConfiguration>("GameConfiguration");
        }

        /// <summary>
        /// To allow for reloading - e.g. so the static instance gets updated if we make changes in the editor
        /// </summary>
        public static void ReloadSingletonGameConfiguration()
        {
            var config = LoadSingletonGameConfiguration();
            if (config != null)
                _instance = config;
        }
        #endregion Static Singleton Reference

        #region Default Settings
        /// <summary>
        /// List of counter configurations.
        /// </summary>
        public List<CounterConfiguration> DefaultGameItemCounterConfiguration
        {
            get
            {
                return _defaultGameItemCounterConfiguration;
            }
        }
        [SerializeField]
        [Tooltip("A default list of custom counters to be used unless otherwise overridden.")]
        List<CounterConfiguration> _defaultGameItemCounterConfiguration = new List<CounterConfiguration>();
        #endregion Default Settings

        #region Character Settings
        /// <summary>
        /// List of counter configurations.
        /// </summary>
        public List<CounterConfiguration> CharacterCounterConfiguration
        {
            get
            {
                return _characterCounterConfiguration;
            }
        }
        [SerializeField]
        [Tooltip("A list of custom counters to be used by all characters.")]
        List<CounterConfiguration> _characterCounterConfiguration = new List<CounterConfiguration>();
        #endregion Character Settings

        #region Level Settings
        /// <summary>
        /// List of counter configurations.
        /// </summary>
        public List<CounterConfiguration> LevelCounterConfiguration
        {
            get
            {
                return _levelCounterConfiguration;
            }
        }
        [SerializeField]
        [Tooltip("A list of custom counters to be used by all levels.")]
        List<CounterConfiguration> _levelCounterConfiguration = new List<CounterConfiguration>();
        #endregion Level Settings

        #region Player Settings
        /// <summary>
        /// List of counter configurations.
        /// </summary>
        public List<CounterConfiguration> PlayerCounterConfiguration
        {
            get
            {
                return _playerCounterConfiguration;
            }
        }
        [SerializeField]
        [Tooltip("A list of custom counters to be used by all players.")]
        List<CounterConfiguration> _playerCounterConfiguration = new List<CounterConfiguration>();
        #endregion Player Settings

        #region World Settings
        /// <summary>
        /// List of counter configurations.
        /// </summary>
        public List<CounterConfiguration> WorldCounterConfiguration
        {
            get
            {
                return _worldCounterConfiguration;
            }
        }
        [SerializeField]
        [Tooltip("A list of custom counters to be used by all worlds.")]
        List<CounterConfiguration> _worldCounterConfiguration = new List<CounterConfiguration>();
        #endregion World Settings

        public GameConfiguration()
        {
            DefaultGameItemCounterConfiguration.Add(new CounterConfiguration() { Name = "Coins" });
            DefaultGameItemCounterConfiguration.Add(new CounterConfiguration() { Name = "Score" });
            CharacterCounterConfiguration.Add(new CounterConfiguration() { Name = "Coins" });
            CharacterCounterConfiguration.Add(new CounterConfiguration() { Name = "Score" });
            LevelCounterConfiguration.Add(new CounterConfiguration() { Name = "Coins" });
            LevelCounterConfiguration.Add(new CounterConfiguration() { Name = "Score" });
            LevelCounterConfiguration.Add(new CounterConfiguration() { Name = "Progress", CounterType = CounterConfiguration.CounterTypeEnum.Float, FloatMaximum = 1 });
            PlayerCounterConfiguration.Add(new CounterConfiguration() { Name = "Coins", Save = CounterConfiguration.SaveType.Always });
            PlayerCounterConfiguration.Add(new CounterConfiguration() { Name = "Score", Save = CounterConfiguration.SaveType.Always });
            PlayerCounterConfiguration.Add(new CounterConfiguration() { Name = "Lives", Save = CounterConfiguration.SaveType.Always, IntDefault = 3 });
            PlayerCounterConfiguration.Add(new CounterConfiguration() { Name = "Health", Save = CounterConfiguration.SaveType.Always, CounterType = CounterConfiguration.CounterTypeEnum.Float, FloatMaximum = 1, FloatDefault = 1 });
            WorldCounterConfiguration.Add(new CounterConfiguration() { Name = "Coins" });
            WorldCounterConfiguration.Add(new CounterConfiguration() { Name = "Score" });
        }

        /// <summary>
        /// Returns the counter configuration for the specified GameItemType.
        /// </summary>
        /// <param name="gameItemType"></param>
        /// <returns></returns>
        public List<CounterConfiguration> GetCounterConfiguration(GameItemType gameItemType) {
            switch (gameItemType)
            {
                case GameItemType.Character:
                    return CharacterCounterConfiguration;
                case GameItemType.Level:
                    return LevelCounterConfiguration;
                case GameItemType.Player:
                    return PlayerCounterConfiguration;
                case GameItemType.World:
                    return WorldCounterConfiguration;
                default:
                    return null;
            }
        }
    }
}
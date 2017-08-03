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
            PlayerCounterConfiguration.Add(new CounterConfiguration() { Name = "Coins", Save = CounterConfiguration.SaveType.Always });
            PlayerCounterConfiguration.Add(new CounterConfiguration() { Name = "Score", Save = CounterConfiguration.SaveType.Always });
            WorldCounterConfiguration.Add(new CounterConfiguration() { Name = "Coins" });
            WorldCounterConfiguration.Add(new CounterConfiguration() { Name = "Score" });
        }
    }


    /// <summary>
    /// Configuration information about a single counter entry including the key that identifies it.
    /// </summary>
    [System.Serializable]
    public class CounterConfiguration
    {
        public enum CounterTypeEnum { Int, Float }

        public enum SaveType { None, Always }

        #region Configuration Properties
        /// <summary>
        /// A unique key that identifies this counter.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        [SerializeField]
        [Tooltip("A unique name that identifies this counter.")]
        string _name;

        /// <summary>
        /// The type that this counter represents.
        /// </summary>
        public CounterTypeEnum CounterType
        {
            get
            {
                return _counterType;
            }
            set
            {
                _counterType = value;
            }
        }
        [SerializeField]
        [Tooltip("The type that this counter represents.")]
        CounterTypeEnum _counterType;

        /// <summary>
        /// The lowest value that this counter can take (if it is an int type).
        /// </summary>
        public int IntMinimum
        {
            get
            {
                return _intMinimum;
            }
            set
            {
                _intMinimum = value;
            }
        }
        [SerializeField]
        [Tooltip("The lowest value that this counter can take.")]
        int _intMinimum;

        /// <summary>
        /// The lowest value that this counter can take (if it is an float type).
        /// </summary>
        public float FloatMinimum
        {
            get
            {
                return _floatMinimum;
            }
            set
            {
                _floatMinimum = value;
            }
        }
        [SerializeField]
        [Tooltip("The lowest value that this counter can take.")]
        float _floatMinimum;

        /// <summary>
        /// The lowest value that this counter can take (if it is an int type).
        /// </summary>
        public int IntMaximum
        {
            get
            {
                return _intMaximum;
            }
            set
            {
                _intMaximum = value;
            }
        }
        [SerializeField]
        [Tooltip("The highest value that this counter can take.")]
        int _intMaximum = int.MaxValue;

        /// <summary>
        /// The lowest value that this counter can take (if it is an float type).
        /// </summary>
        public float FloatMaximum
        {
            get
            {
                return _floatMaximum;
            }
            set
            {
                _floatMaximum = value;
            }
        }
        [SerializeField]
        [Tooltip("The highest value that this counter can take.")]
        float _floatMaximum = float.MaxValue;

        /// <summary>
        /// If and when the counter should be saved for use across game sessions.
        /// </summary>
        public SaveType Save
        {
            get
            {
                return _save;
            }
            set
            {
                _save = value;
            }
        }
        [Tooltip("If and when the counter should be saved for use across game sessions.")]
        [SerializeField]
        SaveType _save;

        /// <summary>
        /// If and when the best value for the counter should be saved for use across game sessions.
        /// </summary>
        public SaveType SaveBest
        {
            get
            {
                return _saveBest;
            }
            set
            {
                _saveBest = value;
            }
        }
        [Tooltip("If and when the counter should be saved for use across game sessions.")]
        [SerializeField]
        SaveType _saveBest = SaveType.Always;

        #endregion Configuration Properties
    }
}
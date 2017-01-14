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

using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.GameStructure.Players.Messages;
using UnityEngine;

namespace GameFramework.GameStructure.Players.ObjectModel
{
    /// <summary>
    /// Player Game Item
    /// </summary>
    [CreateAssetMenu(fileName = "Player_x", menuName = "Game Framework/Player")]
    public class Player : GameItem
    {
        /// <summary>
        /// A unique identifier for this type of GameItem
        /// </summary>
        public override string IdentifierBase { get { return "Player"; } }

        /// <summary>
        /// A unique shortened version of IdentifierBase to save memory.
        /// </summary>
        public override string IdentifierBasePrefs { get { return "P"; } }


        /// <summary>
        /// A custom name that you can assign to the player. 
        /// CustomNameChangedMessage is sent whenever this value changes outside of initialisation.
        /// </summary>
        /// This can be used for a user specified name that is automatically saved between sessions. 
        /// This differs from Name which is a standard label that is typically fetched from the localisation file.
        public string CustomName {
            get { return _customName; }
            set
            {
                var oldValue = CustomName;
                _customName = value;
                if (IsInitialised && oldValue != CustomName)
                    GameManager.SafeQueueMessage(new CustomNameChangedMessage(CustomName, oldValue));
            }
        }
        string _customName;


        /// <summary>
        /// The number of lives that the current player has. 
        /// LivesChangedMessage is sent whenever this value changes outside of initialisation.
        /// </summary>
        public int Lives
        {
            get { return _lives; }
            set
            {
                var oldValue = Lives;
                _lives = value;
                if (IsInitialised && oldValue != Lives)
                    GameManager.SafeQueueMessage(new LivesChangedMessage(Lives, oldValue));
            }
        }
        int _lives;


        /// <summary>
        /// The health that the current player as in the range 0-1. 
        /// HealthChangedMessage is sent whenever this value changes outside of initialisation.
        /// </summary>
        public float Health
        {
            get { return _health; }
            set
            {
                var oldValue = Health;
                _health = value;
                if (IsInitialised && !Mathf.Approximately(oldValue, Health))
                    GameManager.SafeQueueMessage(new HealthChangedMessage(Health, oldValue));
            }
        }
        float _health;


        /// <summary>
        /// Whether the current player has won the whole game.
        /// GameWonMessage is sent whenever this value is set to true outside of initialisation.
        /// </summary>
        public bool IsGameWon
        {
            get { return _isGameWon; }
            set
            {
                var oldValue = IsGameWon;
                _isGameWon = value;
                if (IsInitialised && oldValue != IsGameWon && IsGameWon)
                    GameManager.SafeTriggerMessage(new GameWonMessage());
            }
        }
        bool _isGameWon;


        public int MaximumWorld { get; set; }
        public int MaximumLevel { get; set; }
        public int SelectedWorld { get; set; }
        public int SelectedLevel { get; set; }   // only use when not using worlds, other use World.SelectedLevel for world specific level.

        /// <summary>
        /// Provides a simple method that you can overload to do custom initialisation in your own classes.
        /// This is called after ParseLevelFileData (if loading from resources) so you can use values setup by that method. 
        /// 
        /// If overriding from a base class be sure to call base.CustomInitialisation()
        /// </summary>
        public override void CustomInitialisation()
        {
            Reset();
            
            //Name = GetSettingString("CustomName", CustomName);

            Score = GetSettingInt("TotalScore", Score);
            Coins = GetSettingInt("TotalCoins", Coins);
            Lives = GetSettingInt("Lives", Lives);
            Health = GetSettingFloat("Health", Health);
            IsGameWon = GetSettingBool("IsGameWon", IsGameWon);

            MaximumWorld = GetSettingInt("MaximumWorld", MaximumWorld);
            MaximumLevel = GetSettingInt("MaximumLevel", MaximumLevel);
            SelectedWorld = GetSettingInt("SelectedWorld", SelectedWorld);
            SelectedLevel = GetSettingInt("SelectedLevel", SelectedLevel);
        }


        /// <summary>
        /// Reset the player to some default values.
        /// </summary>
        public virtual void Reset()
        {
            MaximumWorld = 0;
            MaximumLevel = 0;
            SelectedWorld = 0;
            SelectedLevel = 0;
            IsGameWon = false;

            Score = 0;
            Coins = 0;
            Lives = 0;
            Health = 1;

            if (GameManager.IsActive)
                Lives = GameManager.Instance.DefaultLives;
        }


        /// <summary>
        /// Update PlayerPrefs with setting or preferences for this item.
        /// Note: This does not call PlayerPrefs.Save()
        /// 
        /// If overriding from a base class be sure to call base.ParseGameData()
        /// </summary>
        public override void UpdatePlayerPrefs()
        {
            SetSetting("CustomName", CustomName);

            SetSetting("TotalScore", Score);
            SetSetting("TotalCoins", Coins);
            SetSetting("Lives", Lives);
            SetSettingFloat("Health", Health);

            SetSetting("IsGameWon", IsGameWon);

            SetSetting("MaximumWorld", MaximumWorld);
            SetSetting("MaxLevel", MaximumLevel);
            SetSetting("SelectedWorld", SelectedWorld);
            SetSetting("SelectedLevel", SelectedLevel);

            base.UpdatePlayerPrefs();
        }

        #region Score and Coin Messaging Overrides
        /// <summary>
        /// Sends a PlayerScoreChangedMessage whenever the players score changes.
        /// </summary>
        /// <param name="newScore"></param>
        /// <param name="oldScore"></param>
        public override void SendScoreChangedMessage(int newScore, int oldScore)
        {
            GameManager.Messenger.QueueMessage(new PlayerScoreChangedMessage(this, newScore, oldScore));
        }


        /// <summary>
        /// Sends a PlayerHighScoreChangedMessage whenever the players high score changes.
        /// </summary>
        /// <param name="newHighScore"></param>
        /// <param name="oldHighScore"></param>
        public override void SendHighScoreChangedMessage(int newHighScore, int oldHighScore)
        {
            GameManager.Messenger.QueueMessage(new PlayerHighScoreChangedMessage(this, newHighScore, oldHighScore));
        }


        /// <summary>
        /// Sends a PlayerCoinsChangedMessage whenever the players coin count changes.
        /// </summary>
        /// <param name="newCoins"></param>
        /// <param name="oldCoins"></param>
        public override void SendCoinsChangedMessage(int newCoins, int oldCoins)
        {
            GameManager.Messenger.QueueMessage(new PlayerCoinsChangedMessage(this, newCoins, oldCoins));
        }

        #endregion Score and Coin Messaging Overrides

    }
}
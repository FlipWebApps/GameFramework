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

using FlipWebApps.GameFramework.Scripts.Debugging;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.Messages;
using FlipWebApps.GameFramework.Scripts.GameStructure.Players.ObjectModel;
using FlipWebApps.GameFramework.Scripts.Helper;
using FlipWebApps.GameFramework.Scripts.Localisation;
using UnityEngine;
using UnityEngine.Assertions;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel
{
    /// <summary>
    /// Game Item class. This represents most in game items that have features such as a name and description, 
    /// the ability to unlock, a score or value. 
    /// 
    /// This might include players, worlds, levels and characters...
    /// </summary>
    public class GameItem
    {
        public enum UnlockModeType { Custom, Completion, Coins }

        public virtual string IdentifierBase { get; protected set; }
        public virtual string IdentifierBasePrefs { get; protected set; }
        public bool IsInitialised { get; private set; }

        public Player Player;
        //public GameItem Parent;

        /// <summary>
        /// Global settings
        /// </summary>
        public int Number { get; protected set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Sprite Sprite {
            // delayed load from resources.
            get
            {
                if (_sprite == null && !_spriteTriedLoading)
                {
                    _sprite = GameManager.LoadResource<Sprite>(IdentifierBase + "\\" + IdentifierBase + "_" + Number);
                    _spriteTriedLoading = true;
                }
                return _sprite;
            }
            set { _sprite = value; }
        }

        Sprite _sprite;
        bool _spriteTriedLoading;

        /// <summary>
        /// Whether the current item is purchased. Saved at the root level for all players.
        /// </summary>
        public bool IsBought { get; set; }

        public int ValueToUnlock { get; set; }
        public int HighScoreLocalPlayers { get; set; }              // global high score for all local players
        public int HighScoreLocalPlayersPlayerNumber { get; set; }  // number of the player that has overall high score
        public int OldHighScoreLocalPlayers { get; set; }           // high score before this turn

        #region User Specific Settings

        /// <summary>
        /// The number of coins that are currently assigned to the current GameItem. 
        /// CoinsChangedMessage or some other varient is usually sent whenever this value changes outside of initialisation.
        /// </summary>
        public int Coins
        {
            get { return _coins; }
            set
            {
                var oldValue = Coins;
                _coins = value;
                if (IsInitialised && oldValue != Coins)
                    SendCoinsChangedMessage(Coins, oldValue);
            }
        }
        int _coins;


        /// <summary>
        /// The score associated with the current GameItem. 
        /// ScoreChangedMessage or some other varient is usually sent whenever this value changes outside of initialisation.
        /// </summary>
        public int Score
        {
            get { return _score; }
            set
            {
                var oldValue = Score;
                _score = value;
                if (IsInitialised && oldValue != Score)
                    SendScoreChangedMessage(Score, oldValue);
            }
        }
        int _score;


        /// <summary>
        /// The high score associated with the current GameItem. 
        /// HighScoreChangedMessage or some other varient is usually sent whenever this value changes outside of initialisation.
        /// </summary>
        public int HighScore
        {
            get { return _highScore; }
            set
            {
                var oldValue = _highScore;
                _highScore = value;
                if (IsInitialised && oldValue != HighScore)
                    SendHighScoreChangedMessage(HighScore, oldValue);
            }
        }
        int _highScore;

        /// <summary>
        /// The initial high score before this turn / game...
        /// </summary>
        public int OldHighScore { get; set; }

        /// <summary>
        /// Whether the current item is unlocked. Saved per player.
        /// </summary>
        public bool IsUnlocked { get; set; } 

        /// <summary>
        /// Whether the unlocked animation has been shown. Used if a level is unlocked when not being displayed
        /// so that we can still show a unlock animation to the user (e.g. when completing a level unlocks the next level)
        /// Saved per player.
        /// </summary>
        public bool IsUnlockedAnimationShown { get; set; }

        #endregion User Specific Settings

        /// <summary>
        /// Stored json data from disk. Consider creating a subclass to save this instead.
        /// </summary>
        public JSONObject JsonConfigurationData { get; set; }

        bool _isPlayer;

        #region Initialisation


        /// <summary>
        /// Setup and initialise this gameitem. This will invoke CustomInitialisation() which you can override if you 
        /// want to provide your own custom initialisation.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="name"></param>
        /// <param name="localiseName"></param>
        /// <param name="description"></param>
        /// <param name="localiseDescription"></param>
        /// <param name="sprite"></param>
        /// <param name="valueToUnlock"></param>
        /// <param name="player"></param>
        /// <param name="identifierBase"></param>
        /// <param name="identifierBasePrefs"></param>
        /// <param name="loadFromResources"></param>
        public void Initialise(int number, string name = null, bool localiseName = true, string description = null, bool localiseDescription = true, Sprite sprite = null, int valueToUnlock = -1, Player player = null, string identifierBase = "", string identifierBasePrefs = "", bool loadFromResources = false) //GameItem parent = null, 
        {
            IdentifierBase = identifierBase;
            IdentifierBasePrefs = identifierBasePrefs;
            _isPlayer = IdentifierBase == "Player";

            Number = number;
            // if not already set and not a player game item then set Player to the current player so that we can have per player scores etc.
            Player = player ?? (_isPlayer ? null : GameManager.Instance.Player);
            //Parent = parent;
            Name = localiseName ? LocaliseText.Get(FullKey(name ?? "Name")) : name;
            Description = localiseDescription ? LocaliseText.Get(FullKey(description ?? "Desc")) : description;
            Sprite = sprite;
            ValueToUnlock = valueToUnlock;

            HighScoreLocalPlayers = PlayerPrefs.GetInt(FullKey("HSLP"), 0);	                // saved at global level rather than pre player.
            HighScoreLocalPlayersPlayerNumber = PlayerPrefs.GetInt(FullKey("HSLPN"), -1);	// saved at global level rather than pre player.
            OldHighScoreLocalPlayers = HighScoreLocalPlayers;

            Coins = 0;
            Score = 0;
            HighScore = GetSettingInt("HS", 0);
            OldHighScore = HighScore;
            IsBought = PlayerPrefs.GetInt(FullKey("IsB"), 0) == 1;                          // saved at global level rather than pre player.
            IsUnlocked = IsBought || GetSettingInt("IsU", 0) == 1;
            IsUnlockedAnimationShown = GetSettingInt("IsUAS", 0) == 1;

            if (loadFromResources)
                LoadLevelData();

            // allow for any custom game item specific initialisation
            CustomInitialisation();

            IsInitialised = true;
        }


        /// <summary>
        /// Provides a simple method that you can overload to do custom initialisation in your own classes.
        /// This is called after ParseLevelFileData (if loading from resources) so you can use values setup by that method. 
        /// 
        /// If overriding from a base class be sure to call base.CustomInitialisation()
        /// </summary>
        public virtual void CustomInitialisation()
        {
        }

        #endregion Initialisation


        #region Load JSON Data
        /// <summary>
        /// Load simple meta data associated with this game item.
        /// </summary>
        public void LoadLevelData()
        {
            if (JsonConfigurationData == null)
                JsonConfigurationData = LoadJSONDataFile();
            Assert.IsNotNull(JsonConfigurationData, "Unable to load json data. CHeck the file exists : " + IdentifierBase + "\\" + IdentifierBase + "_" + Number);
            ParseLevelFileData(JsonConfigurationData);
        }


        /// <summary>
        /// Parse the loaded level file data. 
        /// 
        /// If overriding from a base class be sure to call base.ParseLevelFileData()
        /// </summary>
        /// <param name="jsonObject"></param>
        public virtual void ParseLevelFileData(JSONObject jsonObject)
        {
            if (jsonObject.ContainsKey("name"))
                Name = jsonObject.GetString("name");
            if (jsonObject.ContainsKey("description"))
                Description = jsonObject.GetString("description");
            if (jsonObject.ContainsKey("valuetounlock"))
                ValueToUnlock = (int)jsonObject.GetNumber("valuetounlock");
        }


        /// <summary>
        /// Load larger data that takes up more space or needs additional parsing
        /// </summary>
        public void LoadGameData()
        {
            if (JsonConfigurationData == null)
                JsonConfigurationData = LoadJSONDataFile();
            Assert.IsNotNull(JsonConfigurationData, "Unable to load json data. Check the file exists : " + IdentifierBase + "\\" + IdentifierBase + "_" + Number);
            ParseGameData(JsonConfigurationData);
        }


        /// <summary>
        /// Parse the loaded game data. 
        /// 
        /// If overriding from a base class be sure to call base.ParseGameData()
        /// </summary>
        /// <param name="jsonObject"></param>
        public virtual void ParseGameData(JSONObject jsonObject)
        {
        }


        /// <summary>
        /// Load the json file that corresponds to this item.
        /// </summary>
        /// <param name="jsonObject"></param>
        JSONObject LoadJSONDataFile()
        {
            TextAsset jsonTextAsset = GameManager.LoadResource<TextAsset>(IdentifierBase + "\\" + IdentifierBase + "_" + Number);
            if (jsonTextAsset == null) return null;

            MyDebug.Log(jsonTextAsset.text);
            JSONObject jsonObject = JSONObject.Parse(jsonTextAsset.text);
            return jsonObject;
        }

        #endregion Load JSON Data

        /// <summary>
        /// This is seperate so that we can save the bought status (e.g. from in app purchase) without affecting any of the other 
        /// settings. This way we can setup a gameitem and save this Value from IAP code without worrying about it being used 
        /// elsewhere.
        /// </summary>
        public void MarkAsBought()
        {
            IsBought = true;
            IsUnlocked = true;
            PlayerPrefs.SetInt(FullKey("IsB"), 1);	                                    // saved at global level rather than per player.
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Update PlayerPrefs with setting or preferences for this item.
        /// Note: This does not call PlayerPrefs.Save()
        /// 
        /// If overriding from a base class be sure to call base.ParseGameData()
        /// </summary>
        public virtual void UpdatePlayerPrefs()
        {
            SetSetting("IsU", IsUnlocked ? 1 : 0);
            SetSetting("IsUAS", IsUnlockedAnimationShown ? 1 : 0);
            SetSetting("HS", HighScore);

            if (IsBought)
                PlayerPrefs.SetInt(FullKey("IsB"), 1);                                  // saved at global level rather than per player.
            if (HighScoreLocalPlayers != 0)
                PlayerPrefs.SetInt(FullKey("HSLP"), HighScoreLocalPlayers);	            // saved at global level rather than per player.
            if (HighScoreLocalPlayersPlayerNumber != -1)
                PlayerPrefs.SetInt(FullKey("HSLPN"), HighScoreLocalPlayersPlayerNumber);	// saved at global level rather than per player.
        }

        #region Score Related

        public void AddPoint()
        {
            AddPoints(1);
        }

        public void AddPoints(int points, int playerNumber = 0)
        {
            Score += points;
            if (Score > HighScore)
            {
                HighScore = Score;
                if (HighScore > HighScoreLocalPlayers)
                {
                    HighScoreLocalPlayers = HighScore;
                    HighScoreLocalPlayersPlayerNumber = playerNumber;
                }
            }
        }

        public void RemovePoint()
        {
            RemovePoints(1);
        }

        public void RemovePoints(int points)
        {
            var tempScore = Score - points; // batch updates to avoid sending multiple messages.
            Score = Mathf.Max(tempScore, 0);
        }

        public virtual void SendScoreChangedMessage(int newScore, int oldScore)
        {
            GameManager.Messenger.QueueMessage(new ScoreChangedMessage(this, newScore, oldScore));
        }

        public virtual void SendHighScoreChangedMessage(int newHighScore, int oldHighScore)
        {
            GameManager.Messenger.QueueMessage(new HighScoreChangedMessage(this, newHighScore, oldHighScore));
        }
        #endregion

        #region Coins Related
        public void AddCoin()
        {
            AddCoins(1);
        }

        public void AddCoins(int coins)
        {
            Coins += coins;
        }

        public void RemoveCoin()
        {
            RemoveCoins(1);
        }

        public void RemoveCoins(int coins)
        {
            var tempCoins = Coins - coins; // batch updates to avoid sending multiple messages.
            Coins = Mathf.Max(tempCoins, 0);
        }

        public virtual void SendCoinsChangedMessage(int newCoins, int oldCoins)
        {
            GameManager.Messenger.QueueMessage(new CoinsChangedMessage(this, newCoins, oldCoins));
        }

        #endregion

        #region For setting gameitem instance specific settings
        public void SetSetting(string key, string value, string defaultValue = null)
        {
            if (_isPlayer)
            {
                // only set or keep values that aren't the default
                if (value != defaultValue)
                {
                    PlayerPrefs.SetString(FullKey(key), value);
                }
                else
                {
                    PlayerPrefs.DeleteKey(FullKey(key));
                }
            }
            //else if (Parent != null)
            //    Parent.SetSetting(FullKey(key), Value);
            else
                Player.SetSetting(FullKey(key), value, defaultValue);
        }

        public void SetSetting(string key, bool value, bool defaultValue = false)
        {
            if (_isPlayer)
            {
                // only set or keep values that aren't the default
                if (value != defaultValue)
                {
                    PlayerPrefs.SetInt(FullKey(key), value ? 1 : 0);
                }
                else
                {
                    PlayerPrefs.DeleteKey(FullKey(key));
                }
            }
            //else if (Parent != null)
            //    Parent.SetSetting(FullKey(key), Value);
            else
                Player.SetSetting(FullKey(key), value, defaultValue);
        }

        public void SetSetting(string key, int value, int defaultValue = 0)
        {
            if (_isPlayer) {
                // only set or keep values that aren't the default
                if (value != defaultValue)
                {
                    PlayerPrefs.SetInt(FullKey(key), value);
                }
                else
                {
                    PlayerPrefs.DeleteKey(FullKey(key));
                }
            }
            //else if (Parent != null)
            //    Parent.SetSetting(FullKey(key), Value);
            else
                Player.SetSetting(FullKey(key), value, defaultValue);
        }


        public void SetSettingFloat(string key, float value, float defaultValue = 0)
        {
            if (_isPlayer)
            {
                // only set or keep values that aren't the default
                if (!Mathf.Approximately(value, defaultValue))
                {
                    PlayerPrefs.SetFloat(FullKey(key), value);
                }
                else
                {
                    PlayerPrefs.DeleteKey(FullKey(key));
                }
            }
            //else if (Parent != null)
            //    Parent.SetSetting(FullKey(key), Value);
            else
                Player.SetSettingFloat(FullKey(key), value, defaultValue);
        }


        public string GetSettingString(string key, string defaultValue)
        {
            if (_isPlayer)
               return PlayerPrefs.GetString(FullKey(key), defaultValue);
            //else if (Parent != null)
            //    return Parent.GetSettingString(FullKey(key), defaultValue);
            else
                return Player.GetSettingString(FullKey(key), defaultValue);
        }


        public int GetSettingInt(string key, int defaultValue)
        {
            if (_isPlayer)
                return PlayerPrefs.GetInt(FullKey(key), defaultValue);
            //else if (Parent != null)
            //    return Parent.GetSettingInt(FullKey(key), defaultValue);
            else
                return Player.GetSettingInt(FullKey(key), defaultValue);
        }


        public bool GetSettingBool(string key, bool defaultValue)
        {
            if (_isPlayer)
                return PlayerPrefs.GetInt(FullKey(key), defaultValue ? 1 : 0) == 1;
            //else if (Parent != null)
            //    return Parent.GetSettingInt(FullKey(key), defaultValue);
            else
                return Player.GetSettingBool(FullKey(key), defaultValue);
        }


        public float GetSettingFloat(string key, float defaultValue)
        {
            if (_isPlayer)
                return PlayerPrefs.GetFloat(FullKey(key), defaultValue);
            //else if (Parent != null)
            //    return Parent.GetSettingInt(FullKey(key), defaultValue);
            else
                return Player.GetSettingFloat(FullKey(key), defaultValue);
        }

        public string FullKey(string key)
        {
            return IdentifierBasePrefs + Number + "." + key;
        }
        #endregion
    }

}
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

using FlipWebApps.GameFramework.Scripts.Preferences;
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
    /// Base representation for many in game items such as players, worlds, levels and characters...
    /// </summary>
    /// This provides many of the common features that game items need such as a name and description, 
    /// localisation support, the ability to unlock, a score or value etc.
    public class GameItem
    {
        //TODO Hanging here to be closer to top, move it where you need
        /// <summary>
        /// Stored GameItemExtension data. 
        /// </summary>
        /// You can provide a GameItemExtension configuration object that contains custom values to replace default GameItem values.
        public GameItemExtension gameItemExtensionData { get; set; }

        #region Load ScriptableObject Data
        /// <summary>
        /// Load the GameItemExtension that corresponds to this item.
        /// </summary>
        GameItemExtension LoadGameItemExtension()
        {
            GameItemExtension giExtension = GameManager.LoadResource<GameItemExtension>(IdentifierBase + "\\" + IdentifierBase + "_" + Number);
            if (giExtension == null) return null;
            return giExtension;
        }

        /// <summary>
        /// Parse the loaded GameItemExtension object and extract certain default values
        /// </summary>
        /// GameExtension properties 'Name', 'Description' and 'ValueToUnlock' will be used to automatically set the corresponding GameItem
        /// properties. You can also override this method to parse and extract your own custom values.
        /// 
        /// If overriding from a base class be sure to call base.ParseLevelFileData()
        /// <param name="gameItemExtension"></param>
        public virtual void ParseLevelFileData(GameItemExtension gameItemExtension)
        {
            if (gameItemExtension.Name != null)
                Name = gameItemExtension.Name;
            if (gameItemExtension.Description != null)
                Description = gameItemExtension.Description;
            if (gameItemExtension.ValueToUnlock != null)
                ValueToUnlock = gameItemExtension.ValueToUnlock;
        }

		/// <summary>
        /// Return GameItemExtension object, caasted to type <T>
        /// </summary>
        public T GetExtension<T>() where T : class
        {
            Assert.IsNotNull(gameItemExtensionData as T, "Unable to cast GameItemExtension to type specified : " + typeof(T).FullName);
            return gameItemExtensionData as T;
        }
        #endregion

        /// <summary>
        /// Ways in which a GameItem can be unlocked
        /// </summary>
        public enum UnlockModeType { Custom, Completion, Coins }

        /// <summary>
        /// A unique identifier for this type of GameItem (override in your derived classes)
        /// </summary>
        /// You should override this in your derived classes to return a string identifier that is unique to your type of
        /// GameItem e.g. "Level", "World".
        public virtual string IdentifierBase { get; protected set; }

        /// <summary>
        /// A unique shortened version of IdentifierBase to save memory.
        /// </summary>
        /// You should override this in your derived classes to return a string identifier that is a unique shortened version
        /// of IdentifierBase e.g. "L", "W".
        public virtual string IdentifierBasePrefs { get; protected set; }

        /// <summary>
        /// Returns whether this GameItem is setup and initialised.
        /// </summary>
        public bool IsInitialised { get; private set; }

        /// <summary>
        /// A reference to the current Player
        /// </summary>
        /// Many game items will hold unique values depending upon the player e.g. high score. This field is used to identify 
        /// the current player that the GameItem represents.
        public Player Player;
        //public GameItem Parent;

        /// <summary>
        /// A number that represents this game item that is unique for this class of GameItem
        /// </summary>
        public int Number { get; protected set; }

        /// <summary>
        /// The name of this gameitem. Through the constructor you can specify whether this is part of a localisation key, or a fixed value
        /// </summary>
        public string Name {
            get
            {
                return _localiseName? LocaliseText.Get(FullKey(_name ?? "Name")) : _name;
            }
            set
            {
                _name = value;
            }
        }
        string _name;

        /// <summary>
        /// A description of this gameitem. Through the constructor you can specify whether this is part of a localisation key, or a fixed value
        /// </summary>
        public string Description
        {
            get
            {
                return _localiseDescription ? LocaliseText.Get(FullKey(_description ?? "Desc")) : _description;
            }
            set
            {
                _description = value;
            }
        }
        string _description;

        /// <summary>
        /// A sprite that is associated with this gameitem. 
        /// </summary>
        /// The sprite is loaded from resources on first access.
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
        /// Whether the current item has been is purchased. Saved at the root level for all players.
        /// </summary>
        public bool IsBought { get; set; }

        /// <summary>
        /// A value that is needed to unlock this item.
        /// </summary>
        /// Typically this will be the number of coins that you need to collect before being able to unlock this item. A value of
        /// -1 means that you can not unlock this item in this way.
        public int ValueToUnlock { get; set; }

        /// <summary>
        /// A global high score for this GameItem for all local players
        /// </summary>
        public int HighScoreLocalPlayers { get; set; }

        /// <summary>
        /// The number of the player that has the overall high score for this GameItem
        /// </summary>
        public int HighScoreLocalPlayersPlayerNumber { get; set; }

        /// <summary>
        /// Local player high score for this GameItem before this turn
        /// </summary>
        public int OldHighScoreLocalPlayers { get; set; }

        #region User Specific Settings

        /// <summary>
        /// The number of coins that are currently assigned to the current GameItem. 
        /// </summary>
        /// CoinsChangedMessage or some other GameItem specific varient is usually sent whenever this value changes outside of initialisation.
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
        /// </summary>
        /// ScoreChangedMessage or some other GameItem specific varient is usually sent whenever this value changes outside of initialisation.
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
        /// HighScoreChangedMessage or some other GameItem specific varient is usually sent whenever this value changes outside of initialisation.
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
        /// Whether the current item is unlocked. 
        /// </summary>
        /// Saved per player.
        public bool IsUnlocked { get; set; }

        /// <summary>
        /// Whether an unlocked animation has been shown. 
        /// </summary>
        /// Used if a GameItem is unlocked when not being displayed so that we can still show a unlock animation to the user 
        /// (e.g. when completing a level unlocks the next level, we want to show an animation when they go back to the
        /// level select screen).
        /// 
        /// Saved per player.
        public bool IsUnlockedAnimationShown { get; set; }

        #endregion User Specific Settings

        /// <summary>
        /// Stored json data from disk. 
        /// </summary>
        /// You can provide a json configuration file that contains both standard and custom values for setting up 
        /// this game item and also holding other configuration.
        /// 
        /// You can access the Json data directly however it may be cleaner to creating a new subclass to save this instead.
        public JSONObject JsonConfigurationData { get; set; }

        bool _isPlayer;
        bool _localiseName;
        bool _localiseDescription;

        #region Initialisation


        /// <summary>
        /// Setup and initialise this gameitem. This will invoke CustomInitialisation() which you can override if you 
        /// want to provide your own custom initialisation.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="name"></param>
        /// <param name="localiseName">Whether name represents a localisation key (true) or a fixed value (false). Default is a localisation key</param>
        /// <param name="description"></param>
        /// <param name="localiseDescription">Whether description represents a localisation key (true) or a fixed value (false). Default is a localisation key</param>
        /// <param name="sprite"></param>
        /// <param name="valueToUnlock"></param>
        /// <param name="player"></param>
        /// <param name="identifierBase"></param>
        /// <param name="identifierBasePrefs"></param>
        /// <param name="loadFromResources">Whether a resources file is present with additional information</param>
        public void Initialise(int number, string name = null, bool localiseName = true, string description = null, bool localiseDescription = true, Sprite sprite = null, int valueToUnlock = -1, Player player = null, string identifierBase = "", string identifierBasePrefs = "", bool loadFromResources = false) //GameItem parent = null, 
        {
            IdentifierBase = identifierBase;
            IdentifierBasePrefs = identifierBasePrefs;
            _isPlayer = IdentifierBase == "Player";

            Number = number;
            // if not already set and not a player game item then set Player to the current player so that we can have per player scores etc.
            Player = player ?? (_isPlayer ? null : GameManager.Instance.Player);
            //Parent = parent;
            _localiseName = localiseName;
            Name = name;
            _localiseDescription = localiseDescription;
            Description = description;
            Sprite = sprite;
            ValueToUnlock = valueToUnlock;

            HighScoreLocalPlayers = PreferencesFactory.GetInt(FullKey("HSLP"), 0);	                // saved at global level rather than pre player.
            HighScoreLocalPlayersPlayerNumber = PreferencesFactory.GetInt(FullKey("HSLPN"), -1);	// saved at global level rather than pre player.
            OldHighScoreLocalPlayers = HighScoreLocalPlayers;

            Coins = 0;
            Score = 0;
            HighScore = GetSettingInt("HS", 0);
            OldHighScore = HighScore;
            IsBought = PreferencesFactory.GetInt(FullKey("IsB"), 0) == 1;                          // saved at global level rather than pre player.
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
        /// </summary>
        /// This is called after ParseLevelFileData (if loading from resources) so you can use values setup by that method. 
        /// 
        /// If overriding from a base class be sure to call base.CustomInitialisation()
        public virtual void CustomInitialisation()
        {
        }

        #endregion Initialisation

        #region Load JSON Data
        /// <summary>
        /// Load simple meta data associated with this game item.
        /// </summary>
        /// The file loaded must be placed in the resources folder under [IdentifierBase]\[IdentifierBase]_[Number].json
        public void LoadLevelData()
        {
            if (JsonConfigurationData == null)
                JsonConfigurationData = LoadJSONDataFile();
            //Assert.IsNotNull(JsonConfigurationData, "Unable to load json data. Check the file exists : " + IdentifierBase + "\\" + IdentifierBase + "_" + Number);
            if (gameItemExtensionData == null)
                gameItemExtensionData = LoadGameItemExtension();
            //Assert.IsNotNull(gameItemExtensionData, "Unable to load scriptable object data. CHeck the Game Item Extension exists for : " + IdentifierBase + "\\" + IdentifierBase + "_" + Number);
            Assert.IsFalse(JsonConfigurationData == null && gameItemExtensionData == null, "When loading game item from resources, corresponding JSON or GameItemExtension should be present. Check the file exists : " + IdentifierBase + "\\" + IdentifierBase + "_" + Number);
            Assert.IsFalse(JsonConfigurationData != null && gameItemExtensionData != null, "When loading game item from resources, only JSON or GameItemExtension should be present, but not both. Check the file exists : " + IdentifierBase + "\\" + IdentifierBase + "_" + Number);

            if (JsonConfigurationData!=null)
                ParseLevelFileData(JsonConfigurationData);
            else if (gameItemExtensionData != null)
                ParseLevelFileData(gameItemExtensionData);
        }


        /// <summary>
        /// Parse the loaded json file data and extract certain default values
        /// </summary>
        /// Json entries 'name', 'description' and 'valuetounlock' will be used to automatically set the corresponding GameItem
        /// properties. You can also override this method to parse and extract your own custom values.
        /// 
        /// If overriding from a base class be sure to call base.ParseLevelFileData()
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
        /// You may not want to load and hold game data for all GameItemss, especially if it takes up a lot of memory. You can
        /// use this method to selectively load such data.
        public void LoadGameData()
        {
            if (JsonConfigurationData == null)
                JsonConfigurationData = LoadJSONDataFile();
            Assert.IsNotNull(JsonConfigurationData, "Unable to load json data. Check the file exists : " + IdentifierBase + "\\" + IdentifierBase + "_" + Number);
            ParseGameData(JsonConfigurationData);
        }


        /// <summary>
        /// Parse the loaded game data. 
        /// </summary>
        /// If overriding from a base class be sure to call base.ParseGameData()
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
        /// Mark an item as bought and save.
        /// </summary>
        /// This is seperate from IsBought so that we can save the bought status (e.g. from in app purchase) without affecting any of the other 
        /// settings. This way we can temporarily setup a gameitem and save this Value from IAP code without worrying about it being used 
        /// elsewhere.
        public void MarkAsBought()
        {
            IsBought = true;
            IsUnlocked = true;
            PreferencesFactory.SetInt(FullKey("IsB"), 1);	                                    // saved at global level rather than per player.
            PreferencesFactory.Save();
        }

        /// <summary>
        /// Update PlayerPrefs with setting or preferences for this item.
        /// </summary>
        /// Note: This does not call PlayerPrefs.Save()
        /// 
        /// If overriding from a base class be sure to call base.ParseGameData()
        public virtual void UpdatePlayerPrefs()
        {
            SetSetting("IsU", IsUnlocked ? 1 : 0);
            SetSetting("IsUAS", IsUnlockedAnimationShown ? 1 : 0);
            SetSetting("HS", HighScore);

            if (IsBought)
                PreferencesFactory.SetInt(FullKey("IsB"), 1);                                  // saved at global level rather than per player.
            if (HighScoreLocalPlayers != 0)
                PreferencesFactory.SetInt(FullKey("HSLP"), HighScoreLocalPlayers);	            // saved at global level rather than per player.
            if (HighScoreLocalPlayersPlayerNumber != -1)
                PreferencesFactory.SetInt(FullKey("HSLPN"), HighScoreLocalPlayersPlayerNumber);	// saved at global level rather than per player.
        }

        #region Score Related

        /// <summary>
        /// Add a point onto this GameItems Score.
        /// </summary>
        public void AddPoint()
        {
            AddPoints(1);
        }

        /// <summary>
        /// Add the specified number of points onto this GameItems Score.
        /// </summary>
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

        /// <summary>
        /// Subtract a point from this GameItems Score.
        /// </summary>

        public void RemovePoint()
        {
            RemovePoints(1);
        }

        /// <summary>
        /// Subtract the specified number of point from this GameItems Score.
        /// </summary>

        public void RemovePoints(int points)
        {
            var tempScore = Score - points; // batch updates to avoid sending multiple messages.
            Score = Mathf.Max(tempScore, 0);
        }

        /// <summary>
        /// Send a ScoreChangedMessage when the score changes.
        /// </summary>
        /// <param name="newScore"></param>
        /// <param name="oldScore"></param>
        /// You may want to override this in your derived classes to send custom messages.
        public virtual void SendScoreChangedMessage(int newScore, int oldScore)
        {
            GameManager.Messenger.QueueMessage(new ScoreChangedMessage(this, newScore, oldScore));
        }

        /// <summary>
        /// Send a HidhScoreChangedMessage when the high score changes.
        /// </summary>
        /// <param name="newScore"></param>
        /// <param name="oldScore"></param>
        /// You may want to override this in your derived classes to send custom messages.
        public virtual void SendHighScoreChangedMessage(int newHighScore, int oldHighScore)
        {
            GameManager.Messenger.QueueMessage(new HighScoreChangedMessage(this, newHighScore, oldHighScore));
        }
        #endregion

        #region Coins Related
        /// <summary>
        /// Add a coin onto this GameItems coin count.
        /// </summary>

        public void AddCoin()
        {
            AddCoins(1);
        }

        /// <summary>
        /// Add the specified number of coins onto this GameItems coin count.
        /// </summary>
        public void AddCoins(int coins)
        {
            Coins += coins;
        }


        /// <summary>
        /// Subtract a coin from this GameItems coin count.
        /// </summary>
        public void RemoveCoin()
        {
            RemoveCoins(1);
        }


        /// <summary>
        /// Subtract the specified number of coins from this GameItems coin count.
        /// </summary>
        public void RemoveCoins(int coins)
        {
            var tempCoins = Coins - coins; // batch updates to avoid sending multiple messages.
            Coins = Mathf.Max(tempCoins, 0);
        }

        /// <summary>
        /// Send a CoinsChangedMessage when the coin coint changes.
        /// </summary>
        /// <param name="newCoins"></param>
        /// <param name="oldCoins"></param>
        /// You may want to override this in your derived classes to send custom messages.

        public virtual void SendCoinsChangedMessage(int newCoins, int oldCoins)
        {
            GameManager.Messenger.QueueMessage(new CoinsChangedMessage(this, newCoins, oldCoins));
        }

        #endregion

        #region For setting gameitem instance specific settings
        /// <summary>
        /// Set a string preferences setting that is unique to this GameItem instance
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        public void SetSetting(string key, string value, string defaultValue = null)
        {
            if (_isPlayer)
            {
                // only set or keep values that aren't the default
                if (value != defaultValue)
                {
                    PreferencesFactory.SetString(FullKey(key), value);
                }
                else
                {
                    PreferencesFactory.DeleteKey(FullKey(key));
                }
            }
            //else if (Parent != null)
            //    Parent.SetSetting(FullKey(key), Value);
            else
                Player.SetSetting(FullKey(key), value, defaultValue);
        }

        /// <summary>
        /// Set a bool preferences setting that is unique to this GameItem instance
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        public void SetSetting(string key, bool value, bool defaultValue = false)
        {
            if (_isPlayer)
            {
                // only set or keep values that aren't the default
                if (value != defaultValue)
                {
                    PreferencesFactory.SetInt(FullKey(key), value ? 1 : 0);
                }
                else
                {
                    PreferencesFactory.DeleteKey(FullKey(key));
                }
            }
            //else if (Parent != null)
            //    Parent.SetSetting(FullKey(key), Value);
            else
                Player.SetSetting(FullKey(key), value, defaultValue);
        }

        /// <summary>
        /// Set an int preferences setting that is unique to this GameItem instance
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        public void SetSetting(string key, int value, int defaultValue = 0)
        {
            if (_isPlayer) {
                // only set or keep values that aren't the default
                if (value != defaultValue)
                {
                    PreferencesFactory.SetInt(FullKey(key), value);
                }
                else
                {
                    PreferencesFactory.DeleteKey(FullKey(key));
                }
            }
            //else if (Parent != null)
            //    Parent.SetSetting(FullKey(key), Value);
            else
                Player.SetSetting(FullKey(key), value, defaultValue);
        }


        /// <summary>
        /// Set a float preferences setting that is unique to this GameItem instance
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        public void SetSettingFloat(string key, float value, float defaultValue = 0)
        {
            if (_isPlayer)
            {
                // only set or keep values that aren't the default
                if (!Mathf.Approximately(value, defaultValue))
                {
                    PreferencesFactory.SetFloat(FullKey(key), value);
                }
                else
                {
                    PreferencesFactory.DeleteKey(FullKey(key));
                }
            }
            //else if (Parent != null)
            //    Parent.SetSetting(FullKey(key), Value);
            else
                Player.SetSettingFloat(FullKey(key), value, defaultValue);
        }


        /// <summary>
        /// Get a string preferences setting that is unique to this GameItem instance
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        public string GetSettingString(string key, string defaultValue)
        {
            if (_isPlayer)
               return PreferencesFactory.GetString(FullKey(key), defaultValue);
            //else if (Parent != null)
            //    return Parent.GetSettingString(FullKey(key), defaultValue);
            else
                return Player.GetSettingString(FullKey(key), defaultValue);
        }


        /// <summary>
        /// Get an int preferences setting that is unique to this GameItem instance
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        public int GetSettingInt(string key, int defaultValue)
        {
            if (_isPlayer)
                return PreferencesFactory.GetInt(FullKey(key), defaultValue);
            //else if (Parent != null)
            //    return Parent.GetSettingInt(FullKey(key), defaultValue);
            else
                return Player.GetSettingInt(FullKey(key), defaultValue);
        }


        /// <summary>
        /// Get a bool preferences setting that is unique to this GameItem instance
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        public bool GetSettingBool(string key, bool defaultValue)
        {
            if (_isPlayer)
                return PreferencesFactory.GetInt(FullKey(key), defaultValue ? 1 : 0) == 1;
            //else if (Parent != null)
            //    return Parent.GetSettingInt(FullKey(key), defaultValue);
            else
                return Player.GetSettingBool(FullKey(key), defaultValue);
        }


        /// <summary>
        /// Get a float preferences setting that is unique to this GameItem instance
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        public float GetSettingFloat(string key, float defaultValue)
        {
            if (_isPlayer)
                return PreferencesFactory.GetFloat(FullKey(key), defaultValue);
            //else if (Parent != null)
            //    return Parent.GetSettingInt(FullKey(key), defaultValue);
            else
                return Player.GetSettingFloat(FullKey(key), defaultValue);
        }

        /// <summary>
        /// Return the full preferences key that will be used for this GameItem
        /// </summary>
        /// The full key is derived from: IdentifierBasePrefs + Number + "." + key
        /// <param name="key"></param>
        /// <returns></returns>
        public string FullKey(string key)
        {
            return IdentifierBasePrefs + Number + "." + key;
        }
        #endregion
    }

}
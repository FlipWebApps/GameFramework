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
using FlipWebApps.GameFramework.Scripts.GameStructure.Players.ObjectModel;
using FlipWebApps.GameFramework.Scripts.Helper;
using FlipWebApps.GameFramework.Scripts.Localisation;
using System;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel
{
    /// <summary>
    /// Game Item class. This represents most in static game items that have features such as a name and description, 
    /// the ability to unlock, a score or Value. 
    /// 
    /// This might include players, worlds, levels and characters.
    /// </summary>
    public class GameItem
    {
        public Player Player;
        //public GameItem Parent;

        /// <summary>
        /// Global settings
        /// </summary>
        public int Number { get; protected set; }
        public string IdentifierBase { get; protected set; }
        public string IdentifierBasePrefs { get; protected set; }
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

        public int ValueToUnlock { get; set; }
        public int HighScoreLocalPlayers { get; set; }              // global high score for all local players
        public int HighScoreLocalPlayersPlayerNumber { get; set; }  // number of the player that has overall high score
        public int OldHighScoreLocalPlayers { get; set; }           // high score before this turn

        /// <summary>
        /// User specific settings
        /// </summary>
        public int Coins { get; set; }                              // current Coins - per turn
        public int Score { get; set; }                              // current score - per turn.
        public int HighScore { get; set; }                          // high score for this level for current player
        public int OldHighScore { get; set; }                       // high score before this turn
        public bool IsBought { get; set; }                          // Is bought - saved at root level for all players
        public bool IsUnlocked { get; set; }                        // is unlocked - per player
        public bool IsUnlockedAnimationShown { get; set; }
        public int StarsWon { get; set; }

        readonly bool _isPlayer;

        public GameItem(int number, string name = null, bool localiseName = true, string description = null, bool localiseDescription = true, Sprite sprite = null, int valueToUnlock = -1, Player player = null, string identifierBase = "", string identifierBasePrefs = "", bool loadFromResources = false) //GameItem parent = null, 
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
            StarsWon = GetSettingInt("SW", 0);

            if (loadFromResources)
                LoadLevelData();
        }


        /// <summary>
        /// Load simple meta data associated with this game item.
        /// </summary>
        public void LoadLevelData()
        {
            ParseLevelFileData(LoadJSONDataFile());
        }


        public virtual void ParseLevelFileData(JSONObject jsonObject)
        {
            // get level info
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
            ParseGameData(LoadJSONDataFile());
        }

        public virtual void ParseGameData(JSONObject jsonObject)
        {
        }


        JSONObject LoadJSONDataFile()
        {
            TextAsset jsonTextAsset = GameManager.LoadResource<TextAsset>(IdentifierBase + "\\" + IdentifierBase + "_" + Number);
            MyDebug.Log(jsonTextAsset.text);
            JSONObject jsonObject = JSONObject.Parse(jsonTextAsset.text);
            return jsonObject;
        }


        /// <summary>
        /// This is seperate so that we can save the bought status (e.g. from in app purchase) without affecting any of the other 
        /// settings. This way we can setup a gameitem and save this Value from IAP code without worrying about it being used 
        /// elsewhere.
        /// </summary>
        public void MarkAsBought()
        {
            IsBought = true;
            IsUnlocked = true;
            PlayerPrefs.SetInt(FullKey("IsB"), 1);	                                    // saved at global level rather than pre player.
            PlayerPrefs.Save();
        }

        // uses optimised names as we store these for all objects so there can be quite a few!
        public virtual void UpdatePlayerPrefs()
        {
            SetSetting("IsU", IsUnlocked ? 1 : 0);
            SetSetting("IsUAS", IsUnlockedAnimationShown ? 1 : 0);
            SetSetting("SW", StarsWon);
            SetSetting("HS", HighScore);

            if (IsBought)
                PlayerPrefs.SetInt(FullKey("IsB"), 1);                                  // saved at global level rather than pre player.
            if (HighScoreLocalPlayers != 0)
                PlayerPrefs.SetInt(FullKey("HSLP"), HighScoreLocalPlayers);	            // saved at global level rather than pre player.
            if (HighScoreLocalPlayersPlayerNumber != -1)
            PlayerPrefs.SetInt(FullKey("HSLPN"), HighScoreLocalPlayersPlayerNumber);	// saved at global level rather than pre player.
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
            Score -= points;
            Score = Mathf.Max(Score, 0);
        }

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
            Coins -= coins;
            Coins = Mathf.Max(Coins, 0);
        }
        #endregion

        #region For setting player specific settings
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

        public void SetSetting(string key, int value, int defaultValue = 0)
        {
            //TODO if key exists and default Value then remove key. Only write if not default Value as many items won't use these values so doesn't make sense to write them.

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

        public string FullKey(string key)
        {
            return IdentifierBasePrefs + Number + "." + key;
        }
        #endregion
    }

}
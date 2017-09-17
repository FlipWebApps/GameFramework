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

using System;
using System.Collections.Generic;
using GameFramework.Preferences;
using GameFramework.Debugging;
using GameFramework.GameStructure.GameItems.Messages;
using GameFramework.GameStructure.Players.ObjectModel;
using GameFramework.Helper;
using GameFramework.Localisation;
using GameFramework.Localisation.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;
using GameFramework.GameStructure.Game.ObjectModel;
using GameFramework.Messaging;

namespace GameFramework.GameStructure.GameItems.ObjectModel
{
    /// <summary>
    /// Base representation for many in game items such as players, worlds, levels and characters...
    /// </summary>
    /// This provides many of the common features that game items need such as a name and description, 
    /// localisation support, the ability to unlock, a score or value etc.
    public class GameItem : ScriptableObject, ICounterChangedCallback
    {
        #region Enums
        /// <summary>
        /// Ways in which a GameItem can be unlocked.
        /// </summary>
        /// Custom - You handle all unlocking
        /// Completion - Unlocking of an item is based upon the previous item being completed (mainly applies to things like levels / worlds).
        /// Coins - Allows coin unlocking and IAP if enabled. The default (first) selected item is automatically unlocked.
        public enum UnlockModeType { Custom, Completion, Coins }

        /// <summary>
        /// The different types of prefab we have
        /// </summary>
        public enum LocalisablePrefabType { Custom, SelectionMenu, InGame, UnlockWindow, SelectionPreview }

        /// <summary>
        /// The different types of sprites we have
        /// </summary>
        public enum LocalisableSpriteType { Custom, SelectionMenu, InGame, UnlockWindow }
        #endregion Enums

        #region Editor Parameters

        /// <summary>
        /// (private) Localisable name for this GameItem. See also Name for easier access.
        /// </summary>
        LocalisableText LocalisableName
        {
            get
            {
                return _localisableName;
            }
            set
            {
                _localisableName = value;
            }
        }
        [Tooltip("The name for this item - either localised or fixed.")]
        [SerializeField]
        LocalisableText _localisableName;


        /// <summary>
        /// (private) Localisable description for this GameItem. See also Description for easier access.
        /// </summary>
        LocalisableText LocalisableDescription
        {
            get
            {
                return _localisableDescription;
            }
            set
            {
                _localisableDescription = value;
            }
        }
        [Tooltip("The description for this item - either localised or fixed.")]
        [SerializeField]
        LocalisableText _localisableDescription;


        /// <summary>
        /// Default unlocked status for this item. 
        /// </summary>
        /// Saved per player.
        public bool StartUnlocked { 
            get
            {
                return _startUnlocked;
            }
            set
            {
                _startUnlocked = value;
            }
        }
        [Tooltip("Default unlocked status for this item.")]
        [SerializeField]
        bool _startUnlocked;


        /// <summary>
        /// Whether this item can be unlocked with coins. 
        /// </summary>
        /// Saved per player.
        public bool UnlockWithCoins
        {
            get
            {
                return _unlockWithCoins;
            }
            set
            {
                _unlockWithCoins = value;
            }
        }
        [Tooltip("Whether this item can be unlocked with coins.")]
        [SerializeField]
        bool _unlockWithCoins = true;


        /// <summary>
        /// Whether this item can be unlocked with payment. (Requires Unity IAP Service Enabling)
        /// </summary>
        /// Saved per player.
        public bool UnlockWithPayment
        {
            get
            {
                return _unlockWithPayment;
            }
            set
            {
                _unlockWithPayment = value;
            }
        }
        [Tooltip("Whether this item can be unlocked with payment through e.g. IAP. (Requires Unity IAP Service Enabling)")]
        [SerializeField]
        bool _unlockWithPayment;


        /// <summary>
        /// Whether this item can be unlocked based upon the previous item being completed (mainly applies to things like levels / worlds)
        /// </summary>
        /// Saved per player.
        public bool UnlockWithCompletion
        {
            get
            {
                return _unlockWithCompletion;
            }
            set
            {
                _unlockWithCompletion = value;
            }
        }
        [Tooltip("Whether this item can be unlocked based upon the previous item being completed (mainly applies to things like levels / worlds)")]
        [SerializeField]
        bool _unlockWithCompletion;


        /// <summary>
        /// A value that is needed to unlock this item.
        /// </summary>
        /// Typically this will be the number of coins that you need to collect before being able to unlock this item. A value of
        /// -1 means that you can not unlock this item in this way.
        public int ValueToUnlock
        {
            get
            {
                return _valueToUnlock;
            }
            set
            {
                _valueToUnlock = value;
            }
        }
        [Tooltip("Value needed to unlock this item.")]
        [SerializeField]
        int _valueToUnlock;

        [SerializeField]
        List<LocalisablePrefabEntry> _localisablePrefabs = new List<LocalisablePrefabEntry>();

        [SerializeField]
        List<LocalisableSpriteEntry> _localisableSprites = new List<LocalisableSpriteEntry>();

        /// <summary>
        /// A list of custom variables for this game item.
        /// </summary>
        public Variables.ObjectModel.Variables Variables
        {
            get
            {
                return _variables;
            }
            set
            {
                _variables = value;
            }
        }
        [Tooltip("A list of custom variables for this game item.")]
        [SerializeField]
        Variables.ObjectModel.Variables _variables = new Variables.ObjectModel.Variables();


        #endregion Editor Parameters

        #region General Variables
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
        /// Currently active GameConfiguration to use
        /// </summary>
        protected GameConfiguration GameConfiguration { get; set; }

        /// <summary>
        /// Currently active Messenger to use for sending messages
        /// </summary>
        protected Messenger Messenger { get; set; }

        /// <summary>
        /// A reference to the current Player
        /// </summary>
        /// Many game items will hold unique values depending upon the player e.g. high score. This field is used to identify 
        /// the current player that the GameItem represents.
        public Player Player { get; private set; }
        //public GameItem Parent;

        /// <summary>
        /// A number that represents this game item that is unique for this class of GameItem
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// The name of this gameitem (localised if so configured). 
        /// </summary>
        /// Through the initialise function you can specify whether this is part of a localisation key, or a fixed value
        /// If this is a localisation key, or using default setup then a localised value will be attempted loaded using a
        /// key of format "[IdentifierBasePrefs][Number].Name" e.g. L1.Name. If this is not found then this will fall back 
        /// to the fixed text "[IdentifierBase] [Number]".
        public string Name {
            get
            {
                if (LocalisableName.IsLocalisedWithNoKey())
                {
                    var value = GlobalLocalisation.GetText(FullKey("Name"));
                    if (value == null) return IdentifierBase + " " + Number;
                    return value;
                }
                return LocalisableName.GetValue();
            }
        }

        /// <summary>
        /// A description of this gameitem. 
        /// </summary>
        /// Through the the initialise function you can specify whether this is part of a localisation key, or a fixed value
        public string Description
        {
            get
            {
                return LocalisableDescription.IsLocalisedWithNoKey() ? GlobalLocalisation.GetText(FullKey("Desc"), missingReturnsKey: true) : LocalisableDescription.GetValue();
            }
        }

        /// <summary>
        /// A sprite that is associated with this gameitem loaded automatically from resources. 
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
        /// Whether the current item has been purchased (if so then it is also unlocked). 
        /// Saved at the root level for all players.
        /// </summary>
        public bool IsBought
        {
            get { return _isBought; }
            set
            {
                if (value) IsUnlocked = true;
                _isBought = value;
            }
        }
        bool _isBought;

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

        #endregion General Variables

        #region Per User Settings

        /// <summary>
        /// The score associated with the current GameItem. 
        /// </summary>
        /// ScoreChangedMessage or some other GameItem specific varient is usually sent whenever this value changes outside of initialisation.
        /// Score is contained within this items Counters collection however exposed through this property for convenience.
        public int Score
        {
            get { return _scoreCounter.IntAmount; }
            set { _scoreCounter.IntAmount = value; }
        }
        Counter _scoreCounter;


        /// <summary>
        /// The high score associated with the current GameItem. 
        /// HighScoreChangedMessage or some other GameItem specific varient is usually sent whenever this value changes outside of initialisation.
        /// </summary>
        /// HighScore is contained within this items Counters collection however exposed through this property for convenience.
        public int HighScore
        {
            get { return _scoreCounter.IntAmountBest; }
        }

        /// <summary>
        /// The initial high score before this turn / game...
        /// </summary>
        /// OldHighScore is contained within this items Counters collection however exposed through this property for convenience.
        public int OldHighScore {
            get { return _scoreCounter.IntAmountBestSaved; }
        }

        /// <summary>
        /// Collection of counters including built in counters.
        /// </summary>
        Counter[] _counterEntries { get; set; }

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

        #endregion Per User Settings

        #region Extension Data
        /// <summary>
        /// Stored json data from disk. 
        /// </summary>
        /// You can provide a json configuration file that contains both standard and custom values for setting up 
        /// this game item and also holding other configuration.
        /// 
        /// You can access the Json data directly however it may be cleaner to creating a new subclass to save this instead.
        public JSONObject JsonData { get; set; }

        /// <summary>
        /// Stored json game data from disk. 
        /// </summary>
        /// You can provide a json configuration file that contains both standard and custom values for setting up 
        /// this game item and also holding other configuration.
        /// 
        /// You can access the Json data directly however it may be cleaner to creating a new subclass to save this instead.
        public JSONObject JsonGameData { get; set; }

        [Obsolete("Convert to use GameItem instances or reference JsonData instead. This data is no longer automatically loaded!")]
        public JSONObject JsonConfigurationData { get; set; }

        /// <summary>
        /// Stored GameItemExtension data. 
        /// </summary>
        /// You can provide a GameItemExtension configuration object that contains custom values to replace default GameItem values.
        public GameItemExtension GameItemExtensionData { get; set; }

        /// <summary>
        /// Stored GameItemExtension game data. 
        /// </summary>
        /// You can provide a GameItemExtension configuration object that contains custom values to replace default GameItem values.
        public GameItemExtension GameItemExtensionGameData { get; set; }

        #endregion Extension Data

        // whether this represents a player GameItem
        bool _isPlayer;

        // A prefix that will be used for preferences entries for this item that can be shared across all players. [IdentifierBasePrefs][Number].
        string _prefsPrefix { get; set; }

        // A prefix that will be used for preferences entries for this item on a per player basis. P[PlayerNumber].[IdentifierBasePrefs][Number].
        string _prefsPrefixPlayer { get; set; }

        #region Initialisation

        /// <summary>
        /// Setup and initialise this gameitem.
        /// </summary>
        /// <param name="gameConfiguration"></param>
        /// <param name="player"></param>
        /// <param name="messenger">Messenger for sending notifications of changes etc.</param>
        /// <param name="number"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="sprite"></param>
        /// <param name="valueToUnlock"></param>
        /// <param name="identifierBase"></param>
        /// <param name="identifierBasePrefs"></param>
        /// You should only call this method if you are manually creating the GameItem.
        /// Loaded GameItems will have these values set and configured via the editor.
        /// This will invoke InitialiseNonScriptableObjectValues() via that CustomInitialisation() 
        /// which you can override if you want to provide your own custom initialisation.
        public void Initialise(GameConfiguration gameConfiguration, Player player, Messenger messenger, int number, LocalisableText name = null, LocalisableText description = null, Sprite sprite = null, int valueToUnlock = -1, string identifierBase = "", string identifierBasePrefs = "")
        {
            IdentifierBase = identifierBase;
            IdentifierBasePrefs = identifierBasePrefs;
            Number = number;
            LocalisableName = name ?? LocalisableText.CreateNonLocalised();
            LocalisableDescription = description ?? LocalisableText.CreateNonLocalised();
            Sprite = sprite;
            ValueToUnlock = valueToUnlock;

            InitialiseNonScriptableObjectValues(gameConfiguration, player, messenger);
        }


        /// <summary>
        /// Setup and initialise this gameitem excluding serialisable properties that are set through a
        /// call to Initialise or within the Unity editor. 
        /// </summary>
        /// <param name="gameConfiguration"></param>
        /// <param name="player"></param>
        /// <param name="messenger">Messenger for sending notifications of changes etc.</param>
        /// This method will invoke CustomInitialisation() which you can override if you 
        /// want to provide your own custom initialisation.
        public void InitialiseNonScriptableObjectValues(GameConfiguration gameConfiguration, Player player, Messenger messenger)
        {
            GameConfiguration = gameConfiguration;
            Player = player;
            Messenger = messenger;

            _isPlayer = IdentifierBase == "Player";
            if (!_isPlayer)
                Assert.IsNotNull(Player, "Currently non Player GameItems have to have a valid Player specified.");

            _prefsPrefix = IdentifierBasePrefs + Number + ".";
            _prefsPrefixPlayer = _isPlayer ? _prefsPrefix : Player.FullKey(_prefsPrefix);

            HighScoreLocalPlayers = PreferencesFactory.GetInt(FullKey("HSLP"), 0);	                // saved at global level rather than per player.
            HighScoreLocalPlayersPlayerNumber = PreferencesFactory.GetInt(FullKey("HSLPN"), -1);	// saved at global level rather than per player.
            OldHighScoreLocalPlayers = HighScoreLocalPlayers;

            //TODO: Load!!
            //HighScore = GetSettingInt("HS", 0);
            //OldHighScore = HighScore;

            // Setup counters.
            var counterConfigurationEntries = GetCounterConfiguration();
            var numberOfCounterEntries = counterConfigurationEntries == null ? 0 : counterConfigurationEntries.Count;
            _counterEntries = new Counter[numberOfCounterEntries];
            for (var counterEntryCount = 0; counterEntryCount < numberOfCounterEntries; counterEntryCount++)
            {
                var counterEntry = new Counter(counterConfigurationEntries[counterEntryCount], _prefsPrefixPlayer, counterEntryCount, this);
                counterEntry.LoadFromPrefs();
                _counterEntries[counterEntryCount] = counterEntry;
            }
            _coinsCounter = GetCounter("Coins");
            _scoreCounter = GetCounter("Score");
            Assert.IsNotNull(_coinsCounter, "All GameItems must have a counter defined with the Key 'Coins'");
            Assert.IsNotNull(_scoreCounter, "All GamItems must have a counter defined with the Key 'Score'");

            // If the default state is unlocked then default to animation shown also, otherwise we check for bought / unlocked in prefs.
            if (StartUnlocked) {
                IsUnlocked = IsUnlockedAnimationShown = true;
            }
            else {
                // saved at global level rather than per player.
                IsBought = PreferencesFactory.GetInt(FullKey("IsB"), 0) == 1;
                if (IsBought || GetSettingInt("IsU", 0) == 1)
                    IsUnlocked = true;
                IsUnlockedAnimationShown = GetSettingInt("IsUAS", 0) == 1;
            }

            Variables.Load(_prefsPrefixPlayer);

            // allow for any custom game item specific initialisation
            CustomInitialisation();

            IsInitialised = true;
        }


        /// <summary>
        /// Provides a simple method that you can overload to do custom initialisation in your own classes.
        /// </summary>
        /// If overriding from a base class be sure to call base.CustomInitialisation()
        public virtual void CustomInitialisation()
        {
        }


        /// <summary>
        /// Load a gameitem of the specified type and number from resources
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="number"></param>
        public static T LoadFromResources<T>(string typeName, int number) where T: GameItem
        {
            var gameItem = GameManager.LoadResource<T>(typeName + "\\" + typeName + "_" + number);
            if (gameItem != null)
            {
                gameItem = Instantiate(gameItem);  // create a copy so we don't overwrite values.
                gameItem.Number = number;
            }
            return gameItem;
        }

        /// <summary>
        /// Mark an item as bought and save.
        /// </summary>
        /// This is seperate from IsBought so that we can save the bought status (e.g. from in app purchase) without affecting any of the other 
        /// settings. This way we can temporarily setup a gameitem and save this Value from IAP code without worrying about it being used 
        /// elsewhere.
        public void MarkAndSaveAsBought()
        {
            IsBought = true;
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
            //SetSetting("HS", HighScore);

            if (IsBought)
                PreferencesFactory.SetInt(FullKey("IsB"), 1);                                  // saved at global level rather than per player.
            if (HighScoreLocalPlayers != 0)
                PreferencesFactory.SetInt(FullKey("HSLP"), HighScoreLocalPlayers);	            // saved at global level rather than per player.
            if (HighScoreLocalPlayersPlayerNumber != -1)
                PreferencesFactory.SetInt(FullKey("HSLPN"), HighScoreLocalPlayersPlayerNumber); // saved at global level rather than per player.

            Variables.UpdatePlayerPrefs(_prefsPrefixPlayer);

            foreach (var counterEntry in _counterEntries)
                counterEntry.UpdatePlayerPrefs();
        }


        #endregion Initialisation

        #region Extension Data

        /// <summary>
        /// Load simple meta data associated with this game item. Called when this GameItem is initialised.
        /// </summary>
        /// The file loaded must be placed in the resources folder as a json file under [IdentifierBase]\[IdentifierBase]_[Number].json
        /// or as a GameItemExtension derived ScriptableObject also from the resources folder under [IdentifierBase]\[IdentifierBase]_[Number]
        public void LoadData()
        {
            if (JsonData == null)
                LoadJsonData();
            if (GameItemExtensionData == null)
                LoadGameItemExtension();

            if (JsonData == null && GameItemExtensionData == null)
                MyDebug.LogWarning("When loading game item from resources, corresponding JSON or GameItemExtension should be present. Either disable the Load From Resources option or check the file exists : " + IdentifierBase + "\\" + IdentifierBase + "_" + Number + "[_Extension]");
        }

        /// <summary>
        /// Load simple meta data associated with this game item.
        /// </summary>
        /// The file loaded must be placed in the resources folder as a json file under [IdentifierBase]\[IdentifierBase]_[Number].json
        public void LoadJsonData()
        {
            var path = string.Format("{0}\\{0}_{1}", IdentifierBase, Number);
            if (JsonData == null)
                JsonData = LoadJsonDataFile(path);
            if (JsonData != null)
                ParseJsonData(JsonData);
        }


        /// <summary>
        /// Parse the loaded json file data and extract certain default values
        /// </summary>
        /// If overriding from a base class be sure to call base.ParseLevelFileData()
        /// <param name="jsonObject"></param>
        public virtual void ParseJsonData(JSONObject jsonObject)
        {
        }


        /// <summary>
        /// Clear larger data that takes up more space or needs additional parsing
        /// </summary>
        /// Use this method to manually clear any loaded data to save memory
        public void ClearJsonData()
        {
            JsonData = null;
        }


        /// <summary>
        /// Load larger data that takes up more space or needs additional parsing
        /// </summary>
        /// You may not want to load and hold game data for all GameItems, especially if it takes up a lot of memory. You can
        /// use this method to selectively load such data.
        public JSONObject LoadJsonGameData()
        {
            var path = string.Format("{0}\\{0}_GameData_{1}", IdentifierBase, Number);
            if (JsonGameData == null)
                JsonGameData = LoadJsonDataFile(path);
            if (JsonGameData != null)
                ParseJsonGameData(JsonGameData);
            return JsonGameData;
        }


        /// <summary>
        /// Parse the loaded game data. 
        /// </summary>
        /// If overriding from a base class be sure to call base.ParseGameData()
        /// <param name="jsonObject"></param>
        public virtual void ParseJsonGameData(JSONObject jsonObject)
        {
        }


        /// <summary>
        /// Clear larger data that takes up more space or needs additional parsing
        /// </summary>
        /// Use this method to manually clear any loaded game data to save memory
        public void ClearJsonGameData()
        {
            JsonGameData = null;
        }


        /// <summary>
        /// Load a json file
        /// </summary>
        /// <param name="path"></param>
        JSONObject LoadJsonDataFile(string path)
        {
            var jsonTextAsset = GameManager.LoadResource<TextAsset>(path);
            if (jsonTextAsset == null) return null;
            //MyDebug.Log(jsonTextAsset.text);
            var jsonObject = JSONObject.Parse(jsonTextAsset.text);
            return jsonObject;
        }


        /// <summary>
        /// Load the GameItemExtension that corresponds to this item.
        /// </summary>
        public void LoadGameItemExtension()
        {
            GameItemExtensionData = GameManager.LoadResource<GameItemExtension>(IdentifierBase + "\\" + IdentifierBase + "_" + Number + "_Extension");
        }


        /// <summary>
        /// Clear larger data that takes up more space or needs additional parsing
        /// </summary>
        /// Use this method to manually clear any loaded game data to save memory
        public void ClearGameItemExtension()
        {
            GameItemExtensionData = null;
        }

        /// <summary>
        /// Return GameItemExtension object, caasted to type T
        /// </summary>
        public T GetExtension<T>() where T : class
        {
            Assert.IsNotNull(GameItemExtensionData as T, "Unable to cast GameItemExtension to type specified : " + typeof(T).FullName);
            return GameItemExtensionData as T;
        }
        #endregion Extension Data

        #region Get Prefab Related

        /// <summary>
        /// Get a prefab with the given name that corresponds to the currently set language
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameObject GetPrefab(string name)
        {
            var localisablePrefab = GetLocalisablePrefab(name);
            return localisablePrefab == null ? null : localisablePrefab.GetPrefab();
        }


        /// <summary>
        /// Get a prefab with the given name that correspondsto the specified language
        /// </summary>
        /// <param name="name"></param>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault"></param>
        /// <returns></returns>
        public GameObject GetPrefab(string name, SystemLanguage language, bool fallbackToDefault = true)
        {
            var localisablePrefab = GetLocalisablePrefab(name);
            return localisablePrefab == null ? null : localisablePrefab.GetPrefab(language, fallbackToDefault);
        }


        /// <summary>
        /// Get a prefab with the given name that corresponds to the specified language
        /// </summary>
        /// <param name="name"></param>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault"></param>
        /// <returns></returns>
        public GameObject GetPrefab(string name, string language, bool fallbackToDefault = true)
        {
            var localisablePrefab = GetLocalisablePrefab(name);
            return localisablePrefab == null ? null : localisablePrefab.GetPrefab(language, fallbackToDefault);
        }


        /// <summary>
        /// Get a selection menu prefab that corresponds to the currently set language
        /// </summary>
        /// <returns></returns>
        public GameObject GetPrefabSelectionMenu()
        {
            return GetPrefab(LocalisablePrefabType.SelectionMenu);
        }


        /// <summary>
        /// Get a selection menu prefab that correspondsto the specified language
        /// </summary>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault"></param>
        /// <returns></returns>
        public GameObject GetPrefabSelectionMenu(SystemLanguage language, bool fallbackToDefault = true)
        {
            return GetPrefab(LocalisablePrefabType.SelectionMenu, language, fallbackToDefault);
        }


        /// <summary>
        /// Get a selection menu prefab that corresponds to the specified language
        /// </summary>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault"></param>
        /// <returns></returns>
        public GameObject GetPrefabSelectionMenu(string language, bool fallbackToDefault = true)
        {
            return GetPrefab(LocalisablePrefabType.SelectionMenu, language, fallbackToDefault);
        }


        /// <summary>
        /// Get an in game prefab that corresponds to the currently set language
        /// </summary>
        /// <returns></returns>
        public GameObject GetPrefabInGame()
        {
            return GetPrefab(LocalisablePrefabType.InGame);
        }


        /// <summary>
        /// Get an in game prefab that correspondsto the specified language
        /// </summary>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault"></param>
        /// <returns></returns>
        public GameObject GetPrefabInGame(SystemLanguage language, bool fallbackToDefault = true)
        {
            return GetPrefab(LocalisablePrefabType.InGame, language, fallbackToDefault);
        }


        /// <summary>
        /// Get an in game prefab that corresponds to the specified language
        /// </summary>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault"></param>
        /// <returns></returns>
        public GameObject GetPrefabInGame(string language, bool fallbackToDefault = true)
        {
            return GetPrefab(LocalisablePrefabType.InGame, language, fallbackToDefault);
        }


        /// <summary>
        /// Get a prefab with the given type that corresponds to the currently set language
        /// </summary>
        /// <returns></returns>
        GameObject GetPrefab(LocalisablePrefabType prefabType)
        {
            var localisablePrefab = GetLocalisablePrefab(prefabType);
            return localisablePrefab == null ? null : localisablePrefab.GetPrefab();
        }


        /// <summary>
        /// Get a prefab with the given type that correspondsto the specified language
        /// </summary>
        /// <param name="prefabType"></param>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault"></param>
        /// <returns></returns>
        GameObject GetPrefab(LocalisablePrefabType prefabType, SystemLanguage language, bool fallbackToDefault = true)
        {
            var localisablePrefab = GetLocalisablePrefab(prefabType);
            return localisablePrefab == null ? null : localisablePrefab.GetPrefab(language, fallbackToDefault);
        }


        /// <summary>
        /// Get a prefab with the given type that corresponds to the specified language
        /// </summary>
        /// <param name="prefabType"></param>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault"></param>
        /// <returns></returns>
        GameObject GetPrefab(LocalisablePrefabType prefabType, string language, bool fallbackToDefault = true)
        {
            var localisablePrefab = GetLocalisablePrefab(prefabType);
            return localisablePrefab == null ? null : localisablePrefab.GetPrefab(language, fallbackToDefault);
        }


        /// <summary>
        /// Get a localised prefab entry with the given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        LocalisablePrefab GetLocalisablePrefab(string name)
        {
            foreach (var prefabEntry in _localisablePrefabs)
            {
                if (prefabEntry.Name == name) return prefabEntry.LocalisablePrefab;
            }
            return null;
        }


        /// <summary>
        /// Get a localised prefab entry with the given type
        /// </summary>
        /// <param name="localisablePrefabTypeEnum"></param>
        /// <returns></returns>
        LocalisablePrefab GetLocalisablePrefab(LocalisablePrefabType localisablePrefabTypeEnum)
        {
            foreach (var prefabEntry in _localisablePrefabs)
            {
                if (prefabEntry.LocalisablePrefabType == localisablePrefabTypeEnum) return prefabEntry.LocalisablePrefab;
            }
            return null;
        }
        #endregion Get Prefab Related

        #region Instantiate Prefab Related

        /// <summary>
        /// Instantiate a selection menu prefab that corresponds to the currently set language, optionally parented to the specified transform
        /// </summary>
        /// <param name="parent"></param>
        /// <param name = "worldPositionStays" > If true, the parent-relative position, scale and rotation is modified such that the object keeps the same world space position, rotation and scale as before.</param>
        /// <returns></returns>
        [Obsolete("Use the InstantiatePrefab method and pass a type of LocalisablePrefabType.SelectionMenu")]
        public GameObject InstantiatePrefabSelectionMenu(Transform parent = null, bool worldPositionStays = true)
        {
            return InstantiatePrefab(LocalisablePrefabType.SelectionMenu, parent, worldPositionStays);
        }


        /// <summary>
        /// Instantiate an in game prefab that corresponds to the currently set language, optionally parented to the specified transform
        /// </summary>
        /// <param name="parent"></param>
        /// <param name = "worldPositionStays" > If true, the parent-relative position, scale and rotation is modified such that the object keeps the same world space position, rotation and scale as before.</param>
        /// <returns></returns>
        [Obsolete("Use the InstantiatePrefab method and pass a type of LocalisablePrefabType.InGame")]
        public GameObject InstantiatePrefabInGame(Transform parent = null, bool worldPositionStays = true)
        {
            return InstantiatePrefab(LocalisablePrefabType.InGame, parent, worldPositionStays);
        }


        /// <summary>
        /// Instantiate an instance of the specified prefab type.
        /// </summary>
        /// <param name="localisablePrefabType"></param>
        /// <param name="customName"></param>
        /// <param name="parent"></param>
        /// <param name = "worldPositionStays" >If true, the parent-relative position, scale and rotation is modified such that the object keeps the same world space position, rotation and scale as before.</param>
        /// <returns></returns>
        public GameObject InstantiatePrefab(LocalisablePrefabType localisablePrefabType, string customName = null, Transform parent = null, bool worldPositionStays = true)
        {
            return localisablePrefabType == GameItem.LocalisablePrefabType.Custom ? 
                InstantiatePrefab(customName, parent, worldPositionStays) : InstantiatePrefab(localisablePrefabType, parent, worldPositionStays);
        }


        /// <summary>
        /// Instantiate the custom named prefab that corresponds to the currently set language, optionally parented to the specified transform
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        /// <param name = "worldPositionStays" > If true, the parent-relative position, scale and rotation is modified such that the object keeps the same world space position, rotation and scale as before.</param>
        /// <returns></returns>
        public GameObject InstantiatePrefab(string name, Transform parent = null, bool worldPositionStays = true)
        {
            var localisablePrefab = GetPrefab(name);
            return localisablePrefab == null ? null : InstantiatePrefab(localisablePrefab, parent, worldPositionStays);
        }


        /// <summary>
        /// Instantiate an instance of the specified prefab type.
        /// </summary>
        /// <param name="localisablePrefabType"></param>
        /// <param name="parent"></param>
        /// <param name = "worldPositionStays" >If true, the parent-relative position, scale and rotation is modified such that the object keeps the same world space position, rotation and scale as before.</param>
        /// <returns></returns>
        GameObject InstantiatePrefab(LocalisablePrefabType localisablePrefabType, Transform parent = null, bool worldPositionStays = true)
        {
            var localisablePrefab = GetPrefab(localisablePrefabType);
            return localisablePrefab == null ? null : InstantiatePrefab(localisablePrefab, parent, worldPositionStays);
        }


        /// <summary>
        /// Instantiate teh passed prefab
        /// </summary>
        /// <param name="localisablePrefab"></param>
        /// <param name="parent"></param>
        /// <param name="worldPositionStays"></param>
        /// <returns></returns>
        static GameObject InstantiatePrefab(GameObject localisablePrefab, Transform parent, bool worldPositionStays = true)
        {
            var newInstance = Instantiate(localisablePrefab);
            if (parent != null)
                newInstance.transform.SetParent(parent, worldPositionStays);
            return newInstance;
        }
        #endregion Instantiate Prefab Related

        #region Sprite Obsolete

        /// <summary>
        /// Get a selection menu sprite that corresponds to the currently set language
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use the GetSprite method and pass a type of LocalisableSpriteType.SelectionMenu")]
        public Sprite GetSpriteSelectionMenu()
        {
            return GetSpriteByType(LocalisableSpriteType.SelectionMenu);
        }


        /// <summary>
        /// Get a selection menu sprite that correspondsto the specified language
        /// </summary>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault"></param>
        /// <returns></returns>
        [Obsolete("Use the GetSprite method and pass a type of LocalisableSpriteType.SelectionMenu")]
        public Sprite GetSpriteSelectionMenu(SystemLanguage language, bool fallbackToDefault = true)
        {
            return GetSpriteByType(LocalisableSpriteType.SelectionMenu, language, fallbackToDefault);
        }


        /// <summary>
        /// Get a selection menu sprite that corresponds to the specified language
        /// </summary>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault"></param>
        /// <returns></returns>
        [Obsolete("Use the GetSprite method and pass a type of LocalisableSpriteType.SelectionMenu")]
        public Sprite GetSpriteSelectionMenu(string language, bool fallbackToDefault = true)
        {
            return GetSpriteByType(LocalisableSpriteType.SelectionMenu, language, fallbackToDefault);
        }


        /// <summary>
        /// Get an in game sprite that corresponds to the currently set language
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use the GetSprite method and pass a type of LocalisableSpriteType.InGame")]
        public Sprite GetSpriteInGame()
        {
            return GetSpriteByType(LocalisableSpriteType.InGame);
        }


        /// <summary>
        /// Get an in game sprite that correspondsto the specified language
        /// </summary>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault"></param>
        /// <returns></returns>
        [Obsolete("Use the GetSprite method and pass a type of LocalisableSpriteType.InGame")]
        public Sprite GetSpriteInGame(SystemLanguage language, bool fallbackToDefault = true)
        {
            return GetSpriteByType(LocalisableSpriteType.InGame, language, fallbackToDefault);
        }


        /// <summary>
        /// Get an in game sprite that corresponds to the specified language
        /// </summary>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault"></param>
        /// <returns></returns>
        [Obsolete("Use the GetSprite method and pass a type of LocalisableSpriteType.InGame")]
        public Sprite GetSpriteInGame(string language, bool fallbackToDefault = true)
        {
            return GetSpriteByType(LocalisableSpriteType.InGame, language, fallbackToDefault);
        }


        /// <summary>
        /// Get an unlock window sprite that corresponds to the currently set language
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use the GetSprite method and pass a type of LocalisableSpriteType.UnlockWindow")]
        public Sprite GetSpriteUnlockWindow()
        {
            return GetSpriteByType(LocalisableSpriteType.UnlockWindow);
        }


        /// <summary>
        /// Get an unlock window sprite that correspondsto the specified language
        /// </summary>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault"></param>
        /// <returns></returns>
        [Obsolete("Use the GetSprite method and pass a type of LocalisableSpriteType.UnlockWindow")]
        public Sprite GetSpriteUnlockWindow(SystemLanguage language, bool fallbackToDefault = true)
        {
            return GetSpriteByType(LocalisableSpriteType.UnlockWindow, language, fallbackToDefault);
        }


        /// <summary>
        /// Get an unlock window sprite that corresponds to the specified language
        /// </summary>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault"></param>
        /// <returns></returns>
        [Obsolete("Use the GetSprite method and pass a type of LocalisableSpriteType.UnlockWindow")]
        public Sprite GetSpriteUnlockWindow(string language, bool fallbackToDefault = true)
        {
            return GetSpriteByType(LocalisableSpriteType.UnlockWindow, language, fallbackToDefault);
        }
        #endregion Sprite Obsolete

        #region Sprite Related

        /// <summary>
        /// Get a sprite with the given name that corresponds to the currently set language
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Sprite GetSpriteByName(string name)
        {
            var localisableSprite = GetLocalisableSprite(name);
            return localisableSprite == null ? null : localisableSprite.GetSprite();
        }


        /// <summary>
        /// Get a sprite with the given name that correspondsto the specified language
        /// </summary>
        /// <param name="name"></param>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault"></param>
        /// <returns></returns>
        public Sprite GetSpriteByName(string name, SystemLanguage language, bool fallbackToDefault = true)
        {
            var localisableSprite = GetLocalisableSprite(name);
            return localisableSprite == null ? null : localisableSprite.GetSprite(language, fallbackToDefault);
        }


        /// <summary>
        /// Get a sprite with the given name that corresponds to the specified language
        /// </summary>
        /// <param name="name"></param>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault"></param>
        /// <returns></returns>
        public Sprite GetSpriteByName(string name, string language, bool fallbackToDefault = true)
        {
            var localisableSprite = GetLocalisableSprite(name);
            return localisableSprite == null ? null : localisableSprite.GetSprite(language, fallbackToDefault);
        }


        /// <summary>
        /// Get a sprite with the given type that corresponds to the currently set language
        /// </summary>
        /// <returns></returns>
        public Sprite GetSpriteByType(LocalisableSpriteType spriteType)
        {
            var localisableSprite = GetLocalisableSprite(spriteType);
            return localisableSprite == null ? null : localisableSprite.GetSprite();
        }


        /// <summary>
        /// Get a sprite with the given type that correspondsto the specified language
        /// </summary>
        /// <param name="spriteType"></param>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault"></param>
        /// <returns></returns>
        public Sprite GetSpriteByType(LocalisableSpriteType spriteType, SystemLanguage language, bool fallbackToDefault = true)
        {
            var localisableSprite = GetLocalisableSprite(spriteType);
            return localisableSprite == null ? null : localisableSprite.GetSprite(language, fallbackToDefault);
        }


        /// <summary>
        /// Get a sprite with the given type that corresponds to the specified language
        /// </summary>
        /// <param name="spriteType"></param>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault"></param>
        /// <returns></returns>
        public Sprite GetSpriteByType(LocalisableSpriteType spriteType, string language, bool fallbackToDefault = true)
        {
            var localisableSprite = GetLocalisableSprite(spriteType);
            return localisableSprite == null ? null : localisableSprite.GetSprite(language, fallbackToDefault);
        }


        /// <summary>
        /// Instantiate an instance of the specified prefab type.
        /// </summary>
        /// <param name="spriteType"></param>
        /// <param name="customName"></param>
        /// <returns></returns>
        public Sprite GetSprite(LocalisableSpriteType spriteType, string customName)
        {
            return spriteType == GameItem.LocalisableSpriteType.Custom ? GetSpriteByName(customName) : GetSpriteByType(spriteType);
        }


        /// <summary>
        /// Instantiate an instance of the specified prefab type.
        /// </summary>
        /// <param name="spriteType"></param>
        /// <param name="customName"></param>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault"></param>
        /// <returns></returns>
        public Sprite GetSprite(LocalisableSpriteType spriteType, string customName, SystemLanguage language, bool fallbackToDefault = true)
        {
            return spriteType == GameItem.LocalisableSpriteType.Custom
                ? GetSpriteByName(customName, language, fallbackToDefault)
                : GetSpriteByType(spriteType, language, fallbackToDefault);
        }


        /// <summary>
        /// Instantiate an instance of the specified prefab type.
        /// </summary>
        /// <param name="spriteType"></param>
        /// <param name="customName"></param>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault"></param>
        /// <returns></returns>
        public Sprite GetSprite(LocalisableSpriteType spriteType, string customName, string language, bool fallbackToDefault = true)
        {
            return spriteType == GameItem.LocalisableSpriteType.Custom
                ? GetSpriteByName(customName, language, fallbackToDefault)
                : GetSpriteByType(spriteType, language, fallbackToDefault);
        }


        /// <summary>
        /// Get a localised sprite entry with the given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        LocalisableSprite GetLocalisableSprite(string name)
        {
            foreach (var spriteEntry in _localisableSprites)
            {
                if (spriteEntry.Name == name) return spriteEntry.LocalisableSprite;
            }
            return null;
        }


        /// <summary>
        /// Get a localised sprite entry with the given type
        /// </summary>
        /// <param name="localisableSpriteTypeEnum"></param>
        /// <returns></returns>
        LocalisableSprite GetLocalisableSprite(LocalisableSpriteType localisableSpriteTypeEnum)
        {
            foreach (var spriteEntry in _localisableSprites)
            {
                if (spriteEntry.LocalisableSpriteType == localisableSpriteTypeEnum) return spriteEntry.LocalisableSprite;
            }
            return null;
        }

        #endregion Sprite Related

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
        public void AddPoints(int points)
        {
            Score += points;
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
            Score -= points;
        }

        /// <summary>
        /// Send a ScoreChangedMessage when the score changes.
        /// </summary>
        /// <param name="newScore"></param>
        /// <param name="oldScore"></param>
        /// You may want to override this in your derived classes to send custom messages.
        public virtual void SendScoreChangedMessage(int newScore, int oldScore)
        {
            Messenger.QueueMessage(new ScoreChangedMessage(this, newScore, oldScore));
        }

        /// <summary>
        /// Send a HidhScoreChangedMessage when the high score changes.
        /// </summary>
        /// <param name="newHighScore"></param>
        /// <param name="oldHighScore"></param>
        /// You may want to override this in your derived classes to send custom messages.
        public virtual void SendHighScoreChangedMessage(int newHighScore, int oldHighScore)
        {
            Messenger.QueueMessage(new HighScoreChangedMessage(this, newHighScore, oldHighScore));
        }
        #endregion

        #region Coins Related

        /// <summary>
        /// The number of coins that are currently assigned to the current GameItem. 
        /// </summary>
        /// CoinsChangedMessage and CounterChangedMessage or some other GameItem specific varients are usually sent whenever 
        /// this value changes outside of initialisation.
        /// Coins is contained within this items Counters collection however exposed through this property for convenience.
        public int Coins
        {
            get { return _coinsCounter.IntAmount; }
            set { _coinsCounter.IntAmount = value; }
        }
        Counter _coinsCounter;

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
            Coins -= coins;
        }

        /// <summary>
        /// Send a CoinsChangedMessage when the coin coint changes.
        /// </summary>
        /// <param name="newCoins"></param>
        /// <param name="oldCoins"></param>
        /// You may want to override this in your derived classes to send custom messages.

        public virtual void SendCoinsChangedMessage(int newCoins, int oldCoins)
        {
            Messenger.QueueMessage(new CoinsChangedMessage(this, newCoins, oldCoins));
        }

        #endregion

        #region Counter Related

        /// <summary>
        /// Override in subclasses to return a list of custom counter configuration entries that should also
        /// be added to this GameItem.
        /// </summary>
        /// <returns></returns>
        public virtual List<CounterConfiguration> GetCounterConfiguration()
        {
            return GameConfiguration.DefaultGameItemCounterConfiguration;
        }


        /// <summary>
        /// Get a reference to the specified counter by index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Counter GetCounter(int index)
        {
            return _counterEntries[index];
        }


        /// <summary>
        /// Get a reference to the specified counter by index.
        /// </summary>
        /// <returns></returns>
        public Counter GetCounter(string name)
        {
            Assert.AreNotEqual(-1, GetCounterIndex(name), string.Format("A counter with the name {0} could not be found", name));
            return GetCounter(GetCounterIndex(name));
        }

        /// <summary>
        /// Get the index of a counter so this can be passed for performance later on rather than looking up
        /// by name each time.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Index of the counter for later use when increasing / decreasing / getting the value or -1 if a counter with name is not defined.</returns>
        public int GetCounterIndex(string name)
        {
            for (var i = 0; i < _counterEntries.Length; i++)
                if (_counterEntries[i].Configuration.Name.Equals(name)) return i;
            return -1;
        }


        /// <summary>
        /// Counter IntAmount changed handler that generates a CounterIntAmountChangedMessage
        /// </summary>
        /// <param name="counter"></param>
        /// <param name="oldAmount"></param>
        /// Messages are only sent after the GameItem is initialised to avoid changes during startup.
        /// Overrides are in place for coin and score counters to send specific messages in adition
        public virtual void CounterIntAmountChanged(Counter counter, int oldAmount)
        {
            if (IsInitialised)
            {
                if (counter.Identifier == _coinsCounter.Identifier)
                    SendCoinsChangedMessage(counter.IntAmount, oldAmount);
                else if (counter.Identifier == _scoreCounter.Identifier)
                    SendScoreChangedMessage(counter.IntAmount, oldAmount);
                Messenger.QueueMessage(new CounterIntAmountChangedMessage(this, counter, oldAmount));
            }
        }


        /// <summary>
        /// Counter IntAmountBest changed handler that generates a CounterIntAmountBestChangedMessage
        /// </summary>
        /// <param name="counter"></param>
        /// <param name="oldAmount"></param>
        /// Messages are only sent after the GameItem is initialised to avoid changes during startup.
        /// Overrides are in place for score counters to send specific messages in adition
        public virtual void CounterIntAmountBestChanged(Counter counter, int oldAmount)
        {
            if (IsInitialised)
            {
                if (counter.Identifier == _scoreCounter.Identifier)
                    SendHighScoreChangedMessage(counter.IntAmountBest, oldAmount);
                Messenger.QueueMessage(new CounterIntAmountBestChangedMessage(this, counter, oldAmount));
            }
        }


        /// <summary>
        /// Counter FloatAmount changed handler that generates a CounterFloatAmountChangedMessage
        /// </summary>
        /// <param name="counter"></param>
        /// <param name="oldAmount"></param>
        /// Messages are only sent after the GameItem is initialised to avoid changes during startup.
        public virtual void CounterFloatAmountChanged(Counter counter, float oldAmount)
        {
            if (IsInitialised)
                Messenger.QueueMessage(new CounterFloatAmountChangedMessage(this, counter, oldAmount));
        }


        /// <summary>
        /// Counter FloatAmountBest changed handler that generates a CounterFloatAmountBestChangedMessage
        /// </summary>
        /// <param name="counter"></param>
        /// <param name="oldAmount"></param>
        /// Messages are only sent after the GameItem is initialised to avoid changes during startup.
        public virtual void CounterFloatAmountBestChanged(Counter counter, float oldAmount)
        {
            if (IsInitialised)
                Messenger.QueueMessage(new CounterFloatAmountBestChangedMessage(this, counter, oldAmount));
        }
        #endregion Counter Related

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
            return _prefsPrefix + key;
        }

        #endregion

        #region extra classes for configuration
        [Serializable]
        public class LocalisablePrefabEntry
        {
            [Tooltip("The type that this prefab represents, either a standard type or a custom one for your own use.")]
            public LocalisablePrefabType LocalisablePrefabType;
            [Tooltip("A unique name that identifies this prefab that you can later use for accessing it.")]
            public string Name;
            [Tooltip("The prefab that will be used for this type unless overridden for a particular language.")]
            public LocalisablePrefab LocalisablePrefab;
        }

        [Serializable]
        public class LocalisableSpriteEntry
        {
            [Tooltip("The type that this sprite represents, either a standard type or a custom one for your own use.")]
            public LocalisableSpriteType LocalisableSpriteType;
            [Tooltip("A unique name that identifies this prefab that you can later use for accessing it.")]
            public string Name;
            [Tooltip("The sprite that will be used for this type unless overridden for a particular language.")]
            public LocalisableSprite LocalisableSprite;
        }

        #endregion extra classes for configuration
    }




}
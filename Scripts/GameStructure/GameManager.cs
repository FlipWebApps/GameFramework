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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameFramework.Debugging;
using GameFramework.Display.Placement;
using GameFramework.GameObjects.Components;
using GameFramework.GameStructure.Characters.ObjectModel;
using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.GameStructure.Levels.ObjectModel;
using GameFramework.GameStructure.Players.ObjectModel;
using GameFramework.GameStructure.Worlds.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using GameFramework.GameObjects;
using GameFramework.Messaging;
using GameFramework.GameStructure.Game.Messages;
using GameFramework.Preferences;
using GameFramework.Audio.Messages;
using GameFramework.GameStructure.GameItems.Messages;
using GameFramework.GameStructure.Game.ObjectModel;
#pragma warning disable 618

#if BEAUTIFUL_TRANSITIONS
using BeautifulTransitions.Scripts.Transitions.Components;
#endif

namespace GameFramework.GameStructure
{
    /// <summary>
    /// A core component that holds and manages information about the game.
    /// </summary>
    /// GameManager is where you can setup the structure of your game and holdes other key information and functionality relating to Preferences,
    /// GameStructure, Display, Localisation, Audio, Messaging and more. Please see the online help for full information.
    [AddComponentMenu("Game Framework/GameStructure/Game Manager")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class GameManager : SingletonPersistant<GameManager>
    {
        /// <summary>
        ///  different ways that we can load the different GameItems
        /// </summary>
        public enum GameItemSetupMode { None, Automatic, FromResources, Specified, MasterWithOverrides }

        const string VariablesPrefix = "GV.";

        // Inspector Values
        #region General 

        /// <summary>
        /// The name of this game
        /// </summary>
        [Tooltip("The name of this game")]
        public string GameName = "";

        /// <summary>
        /// If targeting Andriod, a Google Play url for the game
        /// </summary>
        [Tooltip("If targeting Andriod, a Google Play url for the game")]
        public string PlayWebUrl = "";

        /// <summary>
        /// If targeting Andriod, a Google Play direct link for the game
        /// </summary>
        [Tooltip("If targeting Andriod, a Google Play direct link for the game")]
        public string PlayMarketUrl = "";

        /// <summary>
        /// If targeting iOS, a link to the App Store page game
        /// </summary>
        [Tooltip("If targeting iOS, a link to the App Store page game")]
        public string iOSWebUrl = "";

        /// <summary>
        /// Set the base identifier to allow for multiple games in a single project
        /// </summary>
        /// This property is used by the LoadResource and GetIdentifierScene methods to determine a unique name based upon the base identifier
        /// this will not be necessary in most cases, however is used in the extras bundle to allow for multiple games and demos within a 
        /// single project
        [Tooltip("Set the base identifier to allow for multiple games in a single project")]
        public string IdentifierBase;

        /// <summary>
        /// The amount of debug logging that should be shown (only applies in editor mode and debug builds.
        /// </summary>
        [Tooltip("The amount of debug logging that should be shown (only applies in editor mode and debug builds.")]
        public MyDebug.DebugLevelType DebugLevel = MyDebug.DebugLevelType.Warning;

        #endregion General Inspector Values
        #region Preferences related

        /// <summary>
        /// Whether secured preferences should be used.
        /// </summary>
        [Tooltip("Whether secured preferences should be used.")]
        public bool SecurePreferences;

        /// <summary>
        /// If using secure prefs then a pass phase to use when securing preferences. 
        /// </summary>
        /// This should ideally be a random string of 10 or more characters.
        /// Note that advanced hackers may be able to find this pass phrase in you executable. For optimum security some sort of online lookup would be needed so no pass phrase is stored locally.
        [Tooltip("A pass phase to use when securing preferences. This should ideally be a random string of 10 or more characters.\n\nNote that advanced hackers may be able to find this pass phrase in you executable. For optimum security some sort of online lookup would be needed so no pass phrase is stored locally.")]
        public string PreferencesPassPhrase;

        /// <summary>
        /// If using secure prefs then whether to migrate old unsecure values.
        /// </summary>
        /// For existing games you should set this to true if you want to keep their current progress.
        /// For new games set to false for performance as there will be nothing to convert.
        [Tooltip("Whether to migrate old unsecure values.\n\nFor existing games you should set this to true if you want to keep their current progress.\n\nFor new games set to false for performance as there will be nothing to convert.")]
        public bool AutoConvertUnsecurePrefs;

        #endregion Preferences related
        #region Display related

        /// <summary>
        /// Physical size the game is designed against. Can be used later with PhysicalScreenHeightMultiplier for screen scaling
        /// </summary>
        [Header("Display")]
        [Tooltip("Physical size the game is designed against. Can be used later with PhysicalScreenHeightMultiplier for screen scaling")]
        public float ReferencePhysicalScreenHeightInInches = 4f;

        /// <summary>
        /// How often to check for orientation and resolution changes
        /// </summary>
        [Tooltip("How often to check for orientation and resolution changes")]
        public float DisplayChangeCheckDelay = 0.5f;                      // How long to wait until we check orientation & resolution changes.

        #endregion Display related
        #region Game Structure setup

        /// <summary>
        /// How we want players to be setup.
        /// </summary>
        /// None - don't setup players.
        /// Automatic - Setup automatically.
        /// Resources - Load configuration files from resources.
        /// Specified - Specify configuration files to use directly.
        [Tooltip("How we want players to be setup.\n\nNone - don't setup players.\nAutomatic - Setup automatically.\nResources - Load configuration files from resources.\nSpecified - Specify configuration files to use directly.")]
        public GameItemSetupMode PlayerSetupMode = GameItemSetupMode.Automatic;

        /// <summary>
        /// The default number of lives players will have (optional if not using lives).
        /// </summary>
        public int DefaultLives {
            get { return Player.GetCounter("Lives").Configuration.IntDefault; }
        }

        /// <summary>
        /// The number of local players to setup
        /// </summary>
        [Tooltip("The number of local players to setup")]
        public int PlayerCount = 1;


        /// <summary>
        /// How we want worlds to be setup.
        /// </summary>
        /// None - don't setup worlds.
        /// Automatic - Setup automatically.
        /// Resources - Load configuration files from resources.
        /// Specified - Specify configuration files to use directly.
        [Tooltip("How we want worlds to be setup.\n\nNone - don't setup worlds.\nAutomatic - Setup automatically.\nResources - Load configuration files from resources.\nSpecified - Specify configuration files to use directly.")]
        public GameItemSetupMode WorldSetupMode = GameItemSetupMode.None;

        /// <summary>
        /// Whether to automatically setup wolds using default values. (Obsolete)
        /// </summary>
        [Tooltip("Whether to automatically setup wolds using default values.")]
        [Obsolete("Use WorldSetupMode instead")]
        public bool AutoCreateWorlds = false;

        /// <summary>
        /// The number of standard worlds that should be automatically created by the framework.
        /// </summary>
        [Tooltip("The number of standard worlds that should be automatically created by the framework.")]
        public int NumberOfAutoCreatedWorlds = 0;

        /// <summary>
        /// Whether to try and load data from a resources file. (Obsolete)
        /// </summary>
        [Tooltip("Whether to try and load data from a resources file.")]
        [Obsolete("Either subclass the GameItem if you need custom data or call LoadData() on the GameItem manually")]
        public bool LoadWorldDatafromResources;

        /// <summary>
        /// How we plan on letting users unlock worlds if using automatic setup.
        /// </summary>
        [Tooltip("How we plan on letting users unlock worlds if using automatic setup.")]
        public GameItem.UnlockModeType WorldUnlockMode;

        /// <summary>
        /// The default number of coins to unlock worlds (can be overriden by json configuration files or code) if using automatic setup.
        /// </summary>
        [Tooltip("The default number of coins to unlock worlds (can be overriden by json configuration files or code) if using automatic setup.")]
        public int CoinsToUnlockWorlds = 10;

        /// <summary>
        /// What levels numbers are assigned to each world.
        /// </summary>
        /// These are used for internal lookup and references for e.g. in-app purchase.
        /// Enter a range but keep the numbers unique. You might also want to leave room to expand these later. e.g. (1-10), (101,110)
        [Tooltip("What levels numbers are assigned to each world.\nThese are used for internal lookup and references for e.g. in-app purchase.\n\nEnter a range but keep the numbers unique. You might also want to leave room to expand these later. e.g. (1-10), (101,110)")]
        public MinMax[] WorldLevelNumbers;


        /// <summary>
        /// How we want levels to be setup.
        /// </summary>
        /// None - don't setup levels.
        /// Automatic - Setup automatically.
        /// Resources - Load configuration files from resources.
        /// Specified - Specify configuration files to use directly.
        [Tooltip("How we want levels to be setup.\n\nNone - don't setup levels.\nAutomatic - Setup automatically.\nResources - Load configuration files from resources.\nSpecified - Specify configuration files to use directly.")]
        public GameItemSetupMode LevelSetupMode = GameItemSetupMode.None;

        /// <summary>
        /// Whether to automatically setup levels using default values. (Obsolete)
        /// </summary>
        [Tooltip("Whether to automatically setup levels using default values.\n\nThis option is hidden if automatically creating worlds as we then need per world configuration.")]
        [Obsolete("Use LevelSetupMode instead")]
        public bool AutoCreateLevels = false;

        /// <summary>
        /// The number of standard levels that should be automatically created by the framework.
        /// </summary>
        [Tooltip("The number of standard levels that should be automatically created by the framework.")]
        [FormerlySerializedAs("NumberOfStandardLevels")]
        public int NumberOfAutoCreatedLevels = 10;

        /// <summary>
        /// Whether to try and load data from a resources file (Obsolete).
        /// </summary>
        [Tooltip("Whether to try and load data from a resources file.")]
        [Obsolete("Either subclass the GameItem if you need custom data or call LoadData() on the GameItem manually")]
        public bool LoadLevelDatafromResources = false;

        /// <summary>
        /// How we plan on letting users unlock levels if using automatic setup.
        /// </summary>
        [Tooltip("How we plan on letting users unlock levels if using automatic setup.")]
        public GameItem.UnlockModeType LevelUnlockMode;

        /// <summary>
        /// The default number of coins to unlock levels (can be overriden by json configuration files or code) if using automatic setup.
        /// </summary>
        [Tooltip("The default number of coins to unlock levels (can be overriden by json configuration files or code) if using automatic setup.")]
        public int CoinsToUnlockLevels = 10;

        /// <summary>
        /// A master level configuration file that is used as the basis for setting up levels.
        /// </summary>
        [Tooltip("A master level configuration file that is used as the basis for setting up levels.")]
        public Level LevelMaster;

        /// <summary>
        /// A master level configuration file that is used as the basis for setting up levels.
        /// </summary>
        [Tooltip("A master level configuration file that is used as the basis for setting up levels.")]
        public List<NumberedLevelReference> NumberedLevelReferences;

        /// <summary>
        /// How we want characters to be setup.
        /// </summary>
        /// None - don't setup characters.
        /// Automatic - Setup automatically.
        /// Resources - Load configuration files from resources.
        /// Specified - Specify configuration files to use directly.
        [Tooltip("How we want characters to be setup.\n\nNone - don't setup characters.\nAutomatic - Setup automatically.\nResources - Load configuration files from resources.\nSpecified - Specify configuration files to use directly.")]
        public GameItemSetupMode CharacterSetupMode = GameItemSetupMode.None;

        /// <summary>
        /// Whether to automatically setup characters using default values. (Obsolete)
        /// </summary>
        [Tooltip("Whether to automatically setup characters using default values.")]
        [Obsolete("Use CharacterSetupMode instead")]
        public bool AutoCreateCharacters = false;

        /// <summary>
        /// The number of standard characters that should be automatically created by the framework.
        /// </summary>
        [Tooltip("The number of standard characters that should be automatically created by the framework.")]
        public int NumberOfAutoCreatedCharacters = 10;

        /// <summary>
        /// Whether to try and load data from a resources file (Obsolete).
        /// </summary>
        [Tooltip("Whether to try and load data from a resources file.")]
        [Obsolete("Either subclass the GameItem if you need custom data or call LoadData() on the GameItem manually")]
        public bool LoadCharacterDatafromResources;

        /// <summary>
        /// How we plan on letting users unlock characters if using automatic setup.
        /// </summary>
        [Tooltip("How we plan on letting users unlock characters if using automatic setup.")]
        public GameItem.UnlockModeType CharacterUnlockMode;

        /// <summary>
        /// The default number of coins to unlock characters (can be overriden by json configuration files or code) if using automatic setup.
        /// </summary>
        [Tooltip("The default number of coins to unlock characters (can be overriden by json configuration files or code) if using automatic setup.")]
        public int CoinsToUnlockCharacters = 10;

        #endregion Game Structure setup
        #region Localisation Inspector Values
        /// <summary>
        /// A list of localisation languages that we support
        /// </summary>
        [Obsolete("v4.4 Obsolete - Set and access through GlobalLocalisation instead.")]
        public string[] SupportedLanguages;

        #endregion Localisation Inspector Values
        #region Global Variables

        /// <summary>
        /// A list of custom variables for this game item.
        /// </summary>
        [Tooltip("A list of custom variables for this game item.")]
        public Variables.ObjectModel.Variables Variables;

        #endregion Global Variables

        // Various properties
        #region GamePlay properties

        /// <summary>
        /// Whether Game Manager is setup and initialised
        /// </summary>
        /// THis is set to true once the GameSetup function is complete.
        public bool IsInitialised { get; set; }

        /// <summary>
        /// Whether the game is unlocked. 
        /// </summary>
        /// Some games support the concept of unlocking the game to remove ads or enabling extra features. This field can be 
        /// used control this behaviour such as only enabling certain features and can be linked to in app purchase.
        public bool IsUnlocked
        {
            get { return _isUnlocked; }
            set
            {
                var oldValue = IsUnlocked;
                _isUnlocked = value;
                if (IsInitialised && oldValue != IsUnlocked)
                    SafeQueueMessage(new GameUnlockedMessage(IsUnlocked));

                // save
                PreferencesFactory.SetInt("IsUnlocked", IsUnlocked ? 1 : 0);
                PreferencesFactory.Save();
            }
        }
        bool _isUnlocked;

        [Obsolete("Use functions in LevelManager instead?")]
        public bool IsUserInteractionEnabled { get; set; }

        /// <summary>
        /// General purpose property you can use for controlling the state of your game.
        /// </summary>
        public bool IsSplashScreenShown { get; set; }

        /// <summary>
        /// The number of times that the game has been played
        /// </summary>
        /// AUtomatically updated by GameManager,
        public int TimesGamePlayed { get; set; }

        /// <summary>
        /// The total number of times that levels have been played
        /// </summary>
        /// AUtomatically updated by LevelManager,
        public int TimesLevelsPlayed { get; set; }

        /// <summary>
        /// The number of times that levels have been played for use with a rating prompt
        /// </summary>
        /// AUtomatically updated by LevelManager, this is similar to TImesLevelsPlayed but 
        /// if they click 'remind me later' then this will be reset to 0.
        public int TimesPlayedForRatingPrompt { get; set; }

        #endregion GamePlay properties
        #region Audio properties

        /// <summary>
        /// An AudioSource for playing background music.
        /// </summary>
        /// This is automatically set as the first AudioSource on the same game object as GameManager is present
        public AudioSource BackGroundAudioSource { get; set; }

        /// <summary>
        /// AudioSources for playing sound effects.
        /// </summary>
        /// Thse are automatically set as the secend and subsequent AudioSources on the same game object as GameManager is present
        public AudioSource[] EffectAudioSources { get; set; }

        /// <summary>
        /// The background audio volume
        /// </summary>
        /// Automatically adjusts the BackGroundAudioSource if present
        public float BackGroundAudioVolume
        {
            get { return _backGroundAudioVolume; }
            set
            {
                var oldValue = _backGroundAudioVolume;
                _backGroundAudioVolume = value;
                if (BackGroundAudioSource != null) BackGroundAudioSource.volume = value;
                // notify others if not initial setup and value has changed
                if (IsInitialised && !Mathf.Approximately(oldValue, _backGroundAudioVolume))
                    Messenger.QueueMessage(new BackgroundVolumeChangedMessage(oldValue, _backGroundAudioVolume));
            }
        }
        float _backGroundAudioVolume;

        /// <summary>
        /// The effect audio volume
        /// </summary>
        /// Automatically adjusts all EffectAudioSources if present
        public float EffectAudioVolume {
            get { return _effectAudioVolume; }
            set {
                var oldValue = _effectAudioVolume;
                _effectAudioVolume = value;
                if (EffectAudioSources != null)
                {
                    foreach (var audioSource in EffectAudioSources)
                        audioSource.volume = value;
                }
                // notify others if not initial setup and value has changed
                if (IsInitialised && !Mathf.Approximately(oldValue, _effectAudioVolume))
                    Messenger.QueueMessage(new EffectVolumeChangedMessage(oldValue, _effectAudioVolume));

            }
        }
        float _effectAudioVolume;

        #endregion Audio properties
        #region Display properties

        Vector2 _resolution;                                                 // Current Resolution - for detecting changes only
        DeviceOrientation _orientation;                                      // Current Device Orientation - for detecting changes only

        /// <summary>
        /// May be deprecated in the future. Listen for OnResolutionChangeMessage instead
        /// </summary>
        public event Action<Vector2> OnResolutionChange;                    // raised on resolution change (event as only we can invoke this)

        /// <summary>
        /// May be deprecated in the future. Listen for OnOrientationChangeMessage instead
        /// </summary>
        public event Action<DeviceOrientation> OnOrientationChange;         // raised on orientation change (event as only we can invoke this)

        /// <summary>
        /// The world bottom left position as seen from the main camera (correct always for othograpthic camera, otherwise correct at camera z position for perspective camera)
        /// </summary>
        public Vector3 WorldBottomLeftPosition { get; private set; }

        /// <summary>
        /// The world bottom top right position as seen from the main camera (correct always for othograpthic camera, otherwise correct at camera z position for perspective camera)
        /// </summary>
        public Vector3 WorldTopRightPosition { get; private set; }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// The world bottom left position on the xy plane (z=0) as seen from the main camera (correct always and same as WorldBottomLeftPosition for othograpthic camera)
        /// </summary>
        public Vector3 WorldBottomLeftPositionXYPlane { get; private set; }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// The world bottom top right position on the xy plane (z=0) as seen from the main camera (correct always and same as WorldBottomLeftPosition for othograpthic camera)
        /// </summary>
        public Vector3 WorldTopRightPositionXYPlane { get; private set; }

        /// <summary>
        /// The visible world size as seen from the main camera
        /// </summary>
        public Vector3 WorldSize { get; private set; }

        /// <summary>
        /// A multiplier for the current aspect ratio based on a 4:3 default
        /// </summary>
        public float AspectRatioMultiplier { get; private set; }            // Ratio above and beyong 4:3 ratio

        /// <summary>
        /// A multiplier for the screen height based upon ReferencePhysicalScreenHeightInInches 
        /// </summary>
        public float PhysicalScreenHeightMultiplier { get; private set; }   // Ratio above and beyong 4:3 ratio

        #endregion Display properties

        #region Game Structure Properties

        /// <summary>
        /// GameItemManager containing the current Characters
        /// </summary>
        public CharacterGameItemManager Characters { get; set; }

        /// <summary>
        /// GameItemManager containing the current Worlds
        /// </summary>
        public WorldGameItemManager Worlds { get; set; }

        /// <summary>
        /// GameItemManager containing the current Levels
        /// </summary>
        public LevelGameItemManager Levels { get; set; }

        /// <summary>
        /// GameItemManager containing the current Players
        /// </summary>
        public PlayerGameItemManager Players { get; set; }

        /// <summary>
        /// The current Player. 
        /// </summary>
        /// PlayerChangedMessage is sent whenever this value changes outside of initialisation.
        public Player Player
        {
            get
            {
                Assert.IsNotNull(Players, "Players are not setup. Check that in the script execution order GameManager is setup first.");
                return Players.Selected;
            }
            set
            {
                Players.SetSelected(value);
            }
        }


        /// <summary>
        /// Returns the total number of stars won in all Levels for all Worlds
        /// </summary>
        public int StarsWon
        {
            get
            {
                if (Instance.Worlds != null)
                    return Instance.Worlds.StarsWon;
                else if (Instance.Levels != null)
                    return Instance.Levels.StarsWon;
                else
                    return 0;
            }
        }

        /// <summary>
        /// Returns the total number of stars possible in all Levels for all Worlds
        /// </summary>
        public int StarsTotal
        {
            get
            {
                if (Instance.Worlds != null)
                    return Instance.Worlds.StarsTotal;
                else if (Instance.Levels != null)
                    return Instance.Levels.StarsTotal;
                else
                    return 0;
            }
        }


        /// <summary>
        /// Get an IBaseGameItemManager for the specified GameItemType
        /// </summary>
        /// <param name="gameItemType"></param>
        /// <returns></returns>
        public IBaseGameItemManager GetIBaseGameItemManager(GameConfiguration.GameItemType gameItemType)
        {
            switch (gameItemType)
            {
                case GameConfiguration.GameItemType.Character:
                    Assert.IsNotNull(Instance.Characters, "Characters are not setup but are being referenced.");
                    return Instance.Characters;
                case GameConfiguration.GameItemType.Level:
                    Assert.IsNotNull(Instance.Levels, "Levels are not setup but are being referenced.");
                    return Instance.Levels;
                case GameConfiguration.GameItemType.Player:
                    Assert.IsNotNull(Instance.Players, "Players are not setup but are being referenced.");
                    return Instance.Players;
                case GameConfiguration.GameItemType.World:
                    Assert.IsNotNull(Instance.Worlds, "Worlds are not setup but are being referenced.");
                    return Instance.Worlds;
                default:
                    return null;
            }
        }
        #endregion Game Structure Properties

        #region Messaging properties 

        /// <summary>
        /// A reference to the global Messaging system
        /// </summary>
        readonly Messenger _messenger = new Messenger();
        public static Messenger Messenger {
            get {
                return IsActive ? Instance._messenger : null;
            }
        }

        #endregion Messaging properties 

        #region Setup

        /// <summary>
        /// Main setup routine
        /// </summary>
        protected override void GameSetup()
        {
            base.GameSetup();

            var sb = new System.Text.StringBuilder();

            MyDebug.DebugLevel = DebugLevel;

            sb.Append("GameManager: GameSetup()");
            sb.Append("\nApplication.systemLanguage: ").Append(Application.systemLanguage);

            // secure preferences
            PreferencesFactory.UseSecurePrefs = SecurePreferences;
            if (SecurePreferences)
            {
                if (string.IsNullOrEmpty(PreferencesPassPhrase))
                    Debug.LogWarning("You have not set a custom pass phrase in GameManager | Player Preferences. Please correct for improved security.");
                else
                    PreferencesFactory.PassPhrase = PreferencesPassPhrase;
                PreferencesFactory.AutoConvertUnsecurePrefs = AutoConvertUnsecurePrefs;
            }

            // Gameplay related properties
            IsUnlocked = PreferencesFactory.GetInt("IsUnlocked", 0) != 0;
            IsUserInteractionEnabled = true;
            IsSplashScreenShown = false;
            TimesGamePlayed = PreferencesFactory.GetInt("TimesGamePlayed", 0);
            TimesGamePlayed++;
            TimesLevelsPlayed = PreferencesFactory.GetInt("TimesLevelsPlayed", 0);
            TimesPlayedForRatingPrompt = PreferencesFactory.GetInt("TimesPlayedForRatingPrompt", 0);
            TimesPlayedForRatingPrompt++;
            sb.Append("\nTimesGamePlayed: ").Append(TimesGamePlayed);
            sb.Append("\nTimesLevelsPlayed: ").Append(TimesLevelsPlayed);
            sb.Append("\nTimesPlayedForRatingPrompt: ").Append(TimesPlayedForRatingPrompt);
            sb.Append("\nApplication.PersistantDataPath: ").Append(Application.persistentDataPath);

            MyDebug.Log(sb.ToString());

            // Variables
            Variables.Load(VariablesPrefix, SecurePreferences);

            // audio related properties
            BackGroundAudioVolume = 1;              // default if nothing else is set.
            EffectAudioVolume = 1;                  // default if nothing else is set.
            var audioSources = GetComponents<AudioSource>();
            if (audioSources.Length > 0)
            {
                BackGroundAudioSource = audioSources[0];
                BackGroundAudioVolume = BackGroundAudioSource.volume;
            }
            if (audioSources.Length > 1)
            {
                EffectAudioSources = new AudioSource[audioSources.Length - 1];
                Array.Copy(audioSources, 1, EffectAudioSources, 0, audioSources.Length - 1);
                EffectAudioVolume = EffectAudioSources[0].volume;
            }

            BackGroundAudioVolume = PreferencesFactory.GetFloat("BackGroundAudioVolume", BackGroundAudioVolume, false);
            EffectAudioVolume = PreferencesFactory.GetFloat("EffectAudioVolume", EffectAudioVolume, false);

			Assert.IsNotNull(Camera.main, "You need a main camera in your scene!");
            // display related properties
            SetDisplayProperties();

            // setup players
            Players = new PlayerGameItemManager();
            if (PlayerSetupMode == GameItemSetupMode.Automatic)
                Players.LoadAutomatic(0, PlayerCount - 1);
            else if (PlayerSetupMode == GameItemSetupMode.FromResources)
                Players.Load(0, PlayerCount - 1);
            else if (PlayerSetupMode == GameItemSetupMode.Specified)
                Debug.LogError("World Specified setup mode is not currently implemented. Use one of the other modes for now.");


            // setup of worlds and levels
            Worlds = new WorldGameItemManager();
            Levels = new LevelGameItemManager();
            Characters = new CharacterGameItemManager();

            #region Workaround / warnings for upgraded values.
            if (AutoCreateWorlds) {
                Debug.LogWarning("GameManager World creation is improved and the AutoCreateWorlds property is replaced. In the GameManager component change the World Setup to 'Automatic' or 'From Resources' (if using GameItem configuration files) to carry forward the existing behaviour (simulating this change for now).");
                WorldSetupMode = GameItemSetupMode.FromResources;
            }
            if (AutoCreateLevels)
            {
                Debug.LogWarning("GameManager Level creation is improved and the AutoCreateLevel property is replaced. In the GameManager component change the Level Setup to 'Automatic' or 'From Resources' (if using GameItem configuration files) to carry forward the existing behaviour (simulating this change for now).");
                LevelSetupMode = GameItemSetupMode.FromResources;
            }
            if (AutoCreateCharacters) {
                Debug.LogWarning("GameManager Character creation is improved and the AutoCreateCharacters property is replaced. In the GameManager component change the Character Setup to 'Automatic' or 'From Resources' (if using GameItem configuration files) to carry forward the existing behaviour (simulating this change for now).");
                CharacterSetupMode = GameItemSetupMode.FromResources;
            }
            #endregion Workaround / warnings for upgraded values.

            if (WorldSetupMode != GameItemSetupMode.None)
            {
                if (WorldSetupMode == GameItemSetupMode.Automatic)
                    Worlds.LoadAutomatic(1, NumberOfAutoCreatedWorlds, CoinsToUnlockWorlds, WorldUnlockMode == GameItem.UnlockModeType.Completion, WorldUnlockMode == GameItem.UnlockModeType.Coins);
                else if (WorldSetupMode == GameItemSetupMode.FromResources)
                    Worlds.Load(1, NumberOfAutoCreatedWorlds);
                else if (WorldSetupMode == GameItemSetupMode.Specified)
                    Debug.LogError("World Specified setup mode is not currently implemented. Use one of the other modes for now.");

                // if we have worlds then autocreate levels for each world.
                if (LevelSetupMode == GameItemSetupMode.Automatic)
                {
                    for (var i = 0; i < NumberOfAutoCreatedWorlds; i++)
                    {
                        Worlds.Items[i].Levels = new LevelGameItemManager();
                        Worlds.Items[i].Levels.LoadAutomatic(
                            WorldLevelNumbers[i].Min, WorldLevelNumbers[i].Max,
                            CoinsToUnlockLevels,
                            LevelUnlockMode == GameItem.UnlockModeType.Completion,
                            LevelUnlockMode == GameItem.UnlockModeType.Coins);
                    }
                    // and assign the selected set of levels
                    Levels = Worlds.Selected.Levels;
                }
                else if (LevelSetupMode == GameItemSetupMode.FromResources)
                {
                    for (var i = 0; i < NumberOfAutoCreatedWorlds; i++)
                    {
                        Worlds.Items[i].Levels = new LevelGameItemManager();
                        Worlds.Items[i].Levels.Load(WorldLevelNumbers[i].Min, WorldLevelNumbers[i].Max);
                    }
                    // and assign the selected set of levels
                    Levels = Worlds.Selected.Levels;
                }
                else if (LevelSetupMode == GameItemSetupMode.Specified)
                    Debug.LogError(
                        "Level 'Specified' setup mode is not currently implemented. Use one of the other modes for now.");
            }
            else
            {
                // otherwise not automatically setting up worlds so setup any levels at root level.
                if (LevelSetupMode == GameItemSetupMode.Automatic)
                    Levels.LoadAutomatic(1, NumberOfAutoCreatedLevels, CoinsToUnlockLevels,
                        LevelUnlockMode == GameItem.UnlockModeType.Completion,
                        LevelUnlockMode == GameItem.UnlockModeType.Coins);
                else if (LevelSetupMode == GameItemSetupMode.FromResources)
                    Levels.Load(1, NumberOfAutoCreatedLevels);
                else if (LevelSetupMode == GameItemSetupMode.Specified)
                    Debug.LogError("Level 'Specified' setup mode is not currently implemented. Use one of the other modes for now.");
                else if (LevelSetupMode == GameItemSetupMode.MasterWithOverrides)
                    Levels.LoadMasterWithOverrides(1, NumberOfAutoCreatedLevels, LevelMaster, NumberedLevelReferences.ToArray());
            }


            // setup of characters
            if (CharacterSetupMode == GameItemSetupMode.Automatic)
                Characters.LoadAutomatic(1, NumberOfAutoCreatedCharacters, CoinsToUnlockCharacters,
                    CharacterUnlockMode == GameItem.UnlockModeType.Completion,
                    CharacterUnlockMode == GameItem.UnlockModeType.Coins);
            else if (CharacterSetupMode == GameItemSetupMode.FromResources)
                Characters.Load(1, NumberOfAutoCreatedCharacters);
            else if (CharacterSetupMode == GameItemSetupMode.Specified)
                Debug.LogError("Character 'Specified' setup mode is not currently implemented. Use one of the other modes for now.");


            // coroutine to check for display changes (don't need to do this every frame)
            if (!Mathf.Approximately(DisplayChangeCheckDelay, 0))
                StartCoroutine(CheckForDisplayChanges());

            // flag as initialised
            IsInitialised = true;
        }

        #endregion Setup

        #region Update Loop

        /// <summary>
        /// Update loop
        /// </summary>
        void Update()
        {
            // queue update message and then process all messages.
            _messenger.QueueMessage(new UpdateMessage());
            _messenger.ProcessQueue();
        }

        #endregion Update Loop

        #region Display

        /// <summary>
        /// Setup properties about the current state of the display
        /// </summary>
        void SetDisplayProperties()
        {
            WorldBottomLeftPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
            WorldTopRightPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            WorldBottomLeftPositionXYPlane = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0 - Camera.main.transform.position.z));
            WorldTopRightPositionXYPlane = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0 - Camera.main.transform.position.z)); WorldSize = WorldTopRightPosition - WorldBottomLeftPosition;
            AspectRatioMultiplier = (1 / 1.3333333333f) * Camera.main.aspect;      // assume designed for a 4:3 screen
            PhysicalScreenHeightMultiplier = ReferencePhysicalScreenHeightInInches / DisplayMetrics.GetPhysicalHeight();

            var sb = new System.Text.StringBuilder();
            sb.Append("GameManager: SetDisplayProperties()");
            sb.Append("\nWorldBottomLeftPosition: ").Append(WorldBottomLeftPosition);
            sb.Append("\nWorldTopRightPosition: ").Append(WorldTopRightPosition);
            sb.Append("\nWorldSize: ").Append(WorldSize);
            sb.Append("\nAspectRatioMultiplier: ").Append(AspectRatioMultiplier);
            sb.Append("\nScreen size is: ").Append(WorldBottomLeftPosition);
            sb.Append("\nPhysicalScreenHeightMultiplier: ").Append(DisplayMetrics.GetPhysicalWidth()).Append("x").Append(DisplayMetrics.GetPhysicalHeight()).Append("\"");
            MyDebug.Log(sb.ToString());
        }


        /// <summary>
        /// Co routine that periodically check for display changes
        /// </summary>
        /// <returns></returns>
        IEnumerator CheckForDisplayChanges()
        {
            _resolution = new Vector2(Screen.width, Screen.height);
            _orientation = UnityEngine.Input.deviceOrientation;

            while (true)
            {

                // Check for a Resolution Change
                if (!Mathf.Approximately(_resolution.x, Screen.width) || !Mathf.Approximately(_resolution.y, Screen.height))
                {
                    var oldResolution = _resolution;
                    _resolution = new Vector2(Screen.width, Screen.height);
                    SetDisplayProperties();
                    SafeQueueMessage(new ResolutionChangedMessage(oldResolution, _resolution));
                    if (OnResolutionChange != null) OnResolutionChange(_resolution);
                }

                // Check for an Orientation Change
                switch (UnityEngine.Input.deviceOrientation)
                {
                    case DeviceOrientation.Unknown:            // Ignore
                    case DeviceOrientation.FaceUp:            // Ignore
                    case DeviceOrientation.FaceDown:        // Ignore
                        break;
                    default:
                        if (_orientation != UnityEngine.Input.deviceOrientation)
                        {
                            var oldOrientation = _orientation;
                            _orientation = UnityEngine.Input.deviceOrientation;
                            SetDisplayProperties();
                            SafeQueueMessage(new DeviceOrientationChangedMessage(oldOrientation, _orientation));
                            if (OnOrientationChange != null) OnOrientationChange(_orientation);
                        }
                        break;
                }

                yield return new WaitForSeconds(DisplayChangeCheckDelay);
            }
        }

        #endregion Display

        #region Save State
        /// <summary>
        /// Save GameManager state
        /// </summary>
        public override void SaveState()
        {
            MyDebug.Log("GameManager: SaveState");

            PreferencesFactory.SetInt("TimesPlayedForRatingPrompt", TimesPlayedForRatingPrompt);
            PreferencesFactory.SetInt("TimesGamePlayed", TimesGamePlayed);
            PreferencesFactory.SetInt("TimesLevelsPlayed", TimesLevelsPlayed);

            PreferencesFactory.SetFloat("BackGroundAudioVolume", BackGroundAudioVolume, false);
            PreferencesFactory.SetFloat("EffectAudioVolume", EffectAudioVolume, false);

            Variables.UpdatePlayerPrefs(VariablesPrefix, SecurePreferences);

            PreferencesFactory.Save();
        }
        #endregion Save State

        #region Audio

        /// <summary>
        /// Plays an audio clip using the first available effect audio source
        /// </summary>
        /// If no available audio sources are available then a new one will be created.
        /// <param name="clip"></param>
        /// <param name="pitchLow"></param>
        /// <param name="pitchHigh"></param>
        public void PlayEffect(AudioClip clip, float pitchLow = 1, float pitchHigh = 1)
        {
            
            Assert.IsNotNull(EffectAudioSources, "To make use of the Game Manager audio functions you should add 2 AudioSource components to the same gameobject as the GameManager. The first for background audio, sebsequent ones for effects.");
            Assert.AreNotEqual(0, EffectAudioSources.Length, "To make use of the Game Manager audio functions you should add 2 AudioSource components to the same gameobject as the GameManager. The first for background audio, sebsequent ones for effects.");

            var newPitch = UnityEngine.Random.Range(pitchLow, pitchHigh);

            // try and find a free or similar audio source
            //AudioSource similarAudioSource = null;
            foreach (var audioSource in EffectAudioSources)
            {
                if (audioSource.isPlaying == false)
                {
                    audioSource.clip = clip;
                    audioSource.pitch = newPitch;
                    audioSource.Play();
                    return;
                }

                //if (Mathf.Approximately(audioSource.pitch, pitchHigh))
                //{
                //    similarAudioSource = audioSource;
                //}
            }

            // no free so play one shot if we have a similar match.
            //if (similarAudioSource != null)
            //{
            //    MyDebug.LogWarningF("Not enough free effect AudioSources for playing {0}, ({1}). Using a similar one - consider adding more AudioSources to the GameManager gameobject for performance.", clip.name, newPitch);
            //    similarAudioSource.PlayOneShot(clip);
            //    return;
            //}

            // otherwise we create and add a new one
            MyDebug.LogF("Not enough free effect AudioSources for playing {0}, ({1}). Adding a new one - consider adding more AudioSources to the GameManager gameobject for performance.", clip.name, newPitch);
            var newAudioSource = gameObject.AddComponent<AudioSource>();
            newAudioSource.playOnAwake = false;
            newAudioSource.volume = EffectAudioVolume;
            newAudioSource.pitch = newPitch;
            newAudioSource.clip = clip;
            newAudioSource.Play();
            EffectAudioSources = EffectAudioSources.Concat(Enumerable.Repeat(newAudioSource, 1)).ToArray();
        }
        #endregion Audio

        #region BaseIdentifier related

        /// <summary>
        /// Safely get the identifier that is defined (see IdentifierBase for further details)
        /// </summary>
        /// <returns></returns>
        public static string GetIdentifierBase()
        {
            if (IsActive)
                return string.IsNullOrEmpty(Instance.IdentifierBase) ? null : Instance.IdentifierBase;

            return null;
        }

        /// <summary>
        /// Load the specified resource taking into account path priorities.
        /// </summary>
        /// When loading a resource it will be first tried loaded from a folder prefixed with IdentifierBase (if set),
        /// otherwise it will fall back to the named resource, and if that is not found to the named resource from a 
        /// folder named Default
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public static T LoadResource<T>(string name, bool fallback = true) where T : UnityEngine.Object
        {
            T resource = null;
            if (GetIdentifierBase() != null)
                resource = Resources.Load<T>(GetIdentifierBase() + "/" + name);
            if (resource == null && fallback)
                resource = Resources.Load<T>(name);
            if (resource == null && fallback)
                resource = Resources.Load<T>("Default/" + name);
            return resource;
        }

        /// <summary>
        /// Determin a scene name based upon whether IdentifierBase is set.
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public static string GetIdentifierScene(string sceneName)
        {
            string newSceneName = string.IsNullOrEmpty(GetIdentifierBase())
                    ? sceneName
                    : GetIdentifierBase() + "-" + sceneName;
            return newSceneName;
        }

        #endregion BaseIdentifier related

        #region Messaging

        /// <summary>
        /// Shortcut and safe method for adding a listener without needing to test whether a gamemanager is setup.
        /// </summary>
        /// As GameManager can be destroyed before other components when you shut down your game, it is important to 
        /// <param name="handler"></param>
        /// <returns></returns>
        public static bool SafeAddListener<T>(Messenger.MessageListenerDelegate handler) where T : BaseMessage
        {
            if (!IsActive || Messenger == null) return false;
            Messenger.AddListener<T>(handler);
            return true;
        }

        /// <summary>
        /// Shortcut and safe method for removing a listener without needing to test whether a gamemanager is setup.
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static bool SafeRemoveListener<T>(Messenger.MessageListenerDelegate handler) where T : BaseMessage
        {
            if (!IsActive || Messenger == null) return false;
            Messenger.RemoveListener<T>(handler);
            return true;
        }

        /// <summary>
        /// Shortcut and safe method for adding a listener without needing to test whether a gamemanager is setup.
        /// </summary>
        /// As GameManager can be destroyed before other components when you shut down your game, it is important to 
        /// <param name="messageType"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static bool SafeAddListener(Type messageType, Messenger.MessageListenerDelegate handler)
        {
            if (!IsActive || Messenger == null) return false;
            Messenger.AddListener(messageType, handler);
            return true;
        }

        /// <summary>
        /// Shortcut and safe method for removing a listener without needing to test whether a gamemanager is setup.
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static bool SafeRemoveListener(Type messageType, Messenger.MessageListenerDelegate handler)
        {
            if (!IsActive || Messenger == null) return false;
            Messenger.RemoveListener(messageType, handler);
            return true;
        }

        /// <summary>
        /// Shortcut and safe method for queueing messages without needing to test whether a gamemanager is setup.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool SafeQueueMessage(BaseMessage msg)
        {
            if (!IsActive || Messenger == null) return false;
            return Messenger.QueueMessage(msg);
        }

        /// <summary>
        /// Shortcut and safe method for triggering messages without needing to test whether a gamemanager is setup.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool SafeTriggerMessage(BaseMessage msg)
        {
            if (!IsActive || Messenger == null) return false;
            return Messenger.TriggerMessage(msg);
        }

        #endregion Messaging

        #region Scene Transitions

        /// <summary>
        /// Load the specified scene, applying any transitions if the Beautiful Transitions asset is installed.
        /// </summary>
        /// <param name="sceneName"></param>
        public static void LoadSceneWithTransitions(string sceneName)
        {
            sceneName = GetIdentifierScene(sceneName);
#if BEAUTIFUL_TRANSITIONS
            if (TransitionManager.IsActive)
                TransitionManager.Instance.TransitionOutAndLoadScene(sceneName);
            else
#endif
            SceneManager.LoadScene(sceneName);
        }

        #endregion Scene Transitions
        
        #region Player related code

        /// <summary>
        /// Get the current player - Obsolete
        /// </summary>
        /// <returns></returns>
        [Obsolete("GetPlayer() is deprecated - use Player")]
        public Player GetPlayer()
        {
            return Player;
        }

        /// <summary>
        /// Get the specified player - Obsolete
        /// </summary>
        /// <param name="playerNumber">player number 0 being the first player</param>
        /// <returns></returns>
        [Obsolete("GetPlayer(number) is deprecated - use Players.GetItem(number)")]
        public Player GetPlayer(int playerNumber)
        {
            return Players.GetItem(playerNumber);
        }

        /// <summary>
        /// Set the current player by player number - Obsolete
        /// </summary>
        /// <param name="playerNumber"></param>
        [Obsolete("SetPlayerByNumber(number) is deprecated - use Players.SetSelected(playerNumber);")]
        public void SetPlayerByNumber(int playerNumber)
        {
            Players.SetSelected(playerNumber);
        }
        #endregion Player Related
    }
}
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
using System.Linq;
using FlipWebApps.GameFramework.Scripts.Debugging;
using FlipWebApps.GameFramework.Scripts.Display.Placement;
using FlipWebApps.GameFramework.Scripts.GameObjects.Components;
using FlipWebApps.GameFramework.Scripts.GameStructure.Characters.ObjectModel;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;
using FlipWebApps.GameFramework.Scripts.GameStructure.Levels.ObjectModel;
using FlipWebApps.GameFramework.Scripts.GameStructure.Players.ObjectModel;
using FlipWebApps.GameFramework.Scripts.GameStructure.Worlds.ObjectModel;
using FlipWebApps.GameFramework.Scripts.Localisation;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using FlipWebApps.GameFramework.Scripts.GameObjects;
using FlipWebApps.GameFramework.Scripts.Messaging;
using FlipWebApps.GameFramework.Scripts.GameStructure.Game.Messages;
using FlipWebApps.GameFramework.Scripts.Preferences;
using FlipWebApps.GameFramework.Scripts.GameStructure.Players.Messages;
using FlipWebApps.GameFramework.Scripts.Audio.Messages;

#if BEAUTIFUL_TRANSITIONS
using FlipWebApps.BeautifulTransitions.Scripts.Transitions;
#endif

namespace FlipWebApps.GameFramework.Scripts.GameStructure
{
    /// <summary>
    /// Contains details about the game and is the base class for managing other aspects such as levels etc.
    /// </summary>
    [AddComponentMenu("Game Framework/GameStructure/GameManager")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class GameManager : SingletonPersistant<GameManager>
    {
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
        /// Game Structure setup
        /// </summary>
        [Header("Players")]
        [Tooltip("The default number of lives players will have (optional if not using lives).")]
        public int DefaultLives = 3;

        /// <summary>
        /// The number of local players to setup
        /// </summary>
        [Tooltip("The number of local players to setup")]
        public int PlayerCount = 1;


        /// <summary>
        /// Whether to automatically setup wolds using default values.
        /// </summary>
        [Header("Worlds")]
        [Tooltip("Whether to automatically setup wolds using default values.")]
        public bool AutoCreateWorlds = false;

        /// <summary>
        /// The number of standard worlds that should be automatically created by the framework.
        /// </summary>
        [Tooltip("The number of standard worlds that should be automatically created by the framework.")]
        public int NumberOfAutoCreatedWorlds = 0;

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("Whether to try and load data from a resources file.")]
        public bool LoadWorldDatafromResources = false;

        /// <summary>
        /// How we plan on letting users unlock worlds.
        /// </summary>
        [Tooltip("How we plan on letting users unlock worlds.")]
        public GameItem.UnlockModeType WorldUnlockMode;

        /// <summary>
        /// The default number of coins to unlock worlds (can be overriden by json configuration files or code).
        /// </summary>
        [Tooltip("The default number of coins to unlock worlds (can be overriden by json configuration files or code).")]
        public int CoinsToUnlockWorlds = 10;

        /// <summary>
        /// What levels numbers are assigned to each world.
        /// </summary>
        /// These are used for internal lookup and references for e.g. in-app purchase.
        /// Enter a range but keep the numbers unique. You might also want to leave room to expand these later. e.g. (1-10), (101,110)
        [Tooltip("What levels numbers are assigned to each world.\nThese are used for internal lookup and references for e.g. in-app purchase.\n\nEnter a range but keep the numbers unique. You might also want to leave room to expand these later. e.g. (1-10), (101,110)")]
        public MinMax[] WorldLevelNumbers;


        /// <summary>
        /// Whether to automatically setup levels using default values.
        /// </summary>
        [Header("Levels")]
        [Tooltip("Whether to automatically setup levels using default values.\n\nThis option is hidden if automatically creating worlds as we then need per world configuration.")]
        public bool AutoCreateLevels = false;

        /// <summary>
        /// The number of standard levels that should be automatically created by the framework.
        /// </summary>
        [Tooltip("The number of standard levels that should be automatically created by the framework.")]
        [FormerlySerializedAs("NumberOfStandardLevels")]
        public int NumberOfAutoCreatedLevels = 10;

        /// <summary>
        /// Whether to try and load data from a resources file.
        /// </summary>
        [Tooltip("Whether to try and load data from a resources file.")]
        public bool LoadLevelDatafromResources = false;

        /// <summary>
        /// How we plan on letting users unlock levels.
        /// </summary>
        [Tooltip("How we plan on letting users unlock levels.")]
        public GameItem.UnlockModeType LevelUnlockMode;

        /// <summary>
        /// The default number of coins to unlock levels (can be overriden by json configuration files or code).
        /// </summary>
        [Tooltip("The default number of coins to unlock levels (can be overriden by json configuration files or code).")]
        public int CoinsToUnlockLevels = 10;


        /// <summary>
        /// Whether to automatically setup characters using default values.
        /// </summary>
        [Header("Characters")]
        [Tooltip("Whether to automatically setup characters using default values.")]
        public bool AutoCreateCharacters = false;

        /// <summary>
        /// The number of standard characters that should be automatically created by the framework.
        /// </summary>
        [Tooltip("The number of standard characters that should be automatically created by the framework.")]
        public int NumberOfAutoCreatedCharacters = 10;

        /// <summary>
        /// Whether to try and load data from a resources file.
        /// </summary>
        [Tooltip("Whether to try and load data from a resources file.")]
        public bool LoadCharacterDatafromResources = false;

        /// <summary>
        /// How we plan on letting users unlock characters.
        /// </summary>
        [Tooltip("How we plan on letting users unlock characters.")]
        public GameItem.UnlockModeType CharacterUnlockMode;

        /// <summary>
        /// The default number of coins to unlock characters (can be overriden by json configuration files or code).
        /// </summary>
        [Tooltip("The default number of coins to unlock characters (can be overriden by json configuration files or code).")]
        public int CoinsToUnlockCharacters = 10;

        #endregion Game Structure setup
        #region Localisation Inspector Values
        /// <summary>
        /// A list of localisation languages that we support
        /// </summary>
        [Tooltip("A list of localisation languages that we support")]
        public string[] SupportedLanguages;

        #endregion Localisation Inspector Values

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

        [Obsolete("Use functions in LevelManager instead")]
        public bool IsPaused { get; set; }
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
                if (IsInitialised && oldValue != _backGroundAudioVolume)
                    GameManager.Messenger.QueueMessage(new BackgroundVolumeChangedMessage(oldValue, _backGroundAudioVolume));
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
                if (IsInitialised && oldValue != _effectAudioVolume)
                    GameManager.Messenger.QueueMessage(new EffectVolumeChangedMessage(oldValue, _effectAudioVolume));

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
        /// The world bottom left position as seen from the main camera
        /// </summary>
        public Vector3 WorldBottomLeftPosition { get; private set; }

        /// <summary>
        /// The world bottom top right position as seen from the main camera
        /// </summary>
        public Vector3 WorldTopRightPosition { get; private set; }

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
        #region Player properties 

        /// <summary>
        /// The current Player. 
        /// </summary>
        /// PlayerChangedMessage is sent whenever this value changes outside of initialisation.
        public Player Player
        {
            get { return _player; }
            set
            {
                var oldPlayer = Player;
                _player = value;
                if (IsInitialised && oldPlayer != null && oldPlayer.Number != Player.Number)
                    GameManager.SafeQueueMessage(new PlayerChangedMessage(oldPlayer, Player));
            }
        }
        Player _player;
        protected Player[] Players;

        #endregion Player properties 
        #region GameManager properties

        /// <summary>
        /// GameItemManager containing the current Characters
        /// </summary>
        public GameItemsManager<Character, GameItem> Characters { get; set; }

        /// <summary>
        /// GameItemManager containing the current Worlds
        /// </summary>
        public GameItemsManager<World, GameItem> Worlds { get; set; }

        /// <summary>
        /// GameItemManager containing the current Levels
        /// </summary>
        public GameItemsManager<Level, GameItem> Levels { get; set; }

        #endregion GameManager properties
        #region Messaging properties 

        /// <summary>
        /// A reference to the global Messaging system
        /// </summary>
        Messenger _messenger = new Messenger();
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
#pragma warning disable 618
            IsUserInteractionEnabled = true;
#pragma warning restore 618
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

            // audio related properties
            BackGroundAudioVolume = 1;              // default if nothing else is set.
            EffectAudioVolume = 1;                  // default if nothing else is set.
            var audioSources = GetComponents<AudioSource>();
            if (audioSources.Length == 0)
            {
                MyDebug.LogWarning(
                    "To make use of the Game Manager audio functions you should add 2 AudioSource components to the same gameobject as the GameManager. The first for background audio and the second for effects.");
            }
            else
            {
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
            }

            BackGroundAudioVolume = PreferencesFactory.GetFloat("BackGroundAudioVolume", BackGroundAudioVolume, false);
            EffectAudioVolume = PreferencesFactory.GetFloat("EffectAudioVolume", EffectAudioVolume, false);

			Assert.IsNotNull(Camera.main, "You need a main camera in your scene!");
            // display related properties
            SetDisplayProperties();

            // Localisation setup. If nothing stored then use system Language if it exists. Otherwise we will default to English.
            LocaliseText.AllowedLanguages = SupportedLanguages;

            // setup players.
            Players = new Player[Instance.PlayerCount];
            for (var i = 0; i < Instance.PlayerCount; i++)
            {
                Players[i] = CreatePlayer(i);
            }
            SetPlayerByNumber(0);

            // setup worlds if auto setup
            if (AutoCreateWorlds)
            {
                Worlds = new GameItemsManager<World, GameItem>();
                if (WorldUnlockMode == GameItem.UnlockModeType.Coins)
                    Worlds.LoadDefaultItems(1, NumberOfAutoCreatedWorlds, CoinsToUnlockLevels, LoadWorldDatafromResources);
                else
                    Worlds.LoadDefaultItems(1, NumberOfAutoCreatedWorlds, loadFromResources: LoadWorldDatafromResources);
            }

            // setup levels if auto setup
            if (AutoCreateLevels)
            {
                int startLevel = AutoCreateWorlds ? WorldLevelNumbers[Worlds.Selected.Number - 1].Min : 1;
                int endLevel = AutoCreateWorlds ? WorldLevelNumbers[Worlds.Selected.Number - 1].Max : NumberOfAutoCreatedLevels;
                Levels = new GameItemsManager<Level, GameItem>();
                if (LevelUnlockMode == GameItem.UnlockModeType.Coins)
                    Levels.LoadDefaultItems(startLevel, endLevel, CoinsToUnlockLevels, LoadLevelDatafromResources);
                else
                    Levels.LoadDefaultItems(startLevel, endLevel, loadFromResources: LoadLevelDatafromResources);
            }

            // setup levels if auto setup
            if (AutoCreateCharacters)
            {
                Characters = new GameItemsManager<Character, GameItem>();
                if (CharacterUnlockMode == GameItem.UnlockModeType.Coins)
                    Characters.LoadDefaultItems(1, NumberOfAutoCreatedCharacters, CoinsToUnlockCharacters, LoadCharacterDatafromResources);
                else
                    Characters.LoadDefaultItems(1, NumberOfAutoCreatedCharacters, loadFromResources: LoadCharacterDatafromResources);
            }

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
            // process messages.
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
            WorldSize = WorldTopRightPosition - WorldBottomLeftPosition;
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
            Assert.IsNotNull(EffectAudioSources, "Ensure that you have added AudioSources if you are playying effects.");
            Assert.AreNotEqual(0, EffectAudioSources.Length, "Ensure that you have added AudioSources if you are playying effects.");

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
            MyDebug.LogWarningF("Not enough free effect AudioSources for playing {0}, ({1}). Adding a new one - consider adding more AudioSources to the GameManager gameobject for performance.", clip.name, newPitch);
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
            if (GameManager.IsActive)
                return string.IsNullOrEmpty(GameManager.Instance.IdentifierBase) ? null : GameManager.Instance.IdentifierBase;
            else
            {
                return null;
            }
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
            string newSceneName = string.IsNullOrEmpty(GameManager.GetIdentifierBase())
                    ? sceneName
                    : GameManager.GetIdentifierBase() + "-" + sceneName;
            return newSceneName;
        }

        #endregion BaseIdentifier related

        #region Messaging

        /// <summary>
        /// Shortcut and safe method for adding a listener without needing to test whether a gamemanager is setup.
        /// </summary>
        /// As GameManager can be destroyed before other components when you shut down your game, it is important to 
        /// <param name="msg"></param>
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
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool SafeRemoveListener<T>(Messenger.MessageListenerDelegate handler) where T : BaseMessage
        {
            if (!IsActive || Messenger == null) return false;
            Messenger.RemoveListener<T>(handler);
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
            sceneName = GameManager.GetIdentifierScene(sceneName);
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
        /// Create the given player number (note: probably soon to be made obsolete)
        /// </summary>
        /// Can be overridden if you have a custom player class.
        /// <param name="playerNumber"></param>
        /// <returns></returns>
        protected virtual Player CreatePlayer(int playerNumber)
        {
            MyDebug.Log("GameManager: CreatePlayer");

            var player = new Player();
            player.Initialise(playerNumber, localiseDescription: false);
            return player;
        }

        /// <summary>
        /// Get the current player
        /// </summary>
        /// <returns></returns>
        public Player GetPlayer()
        {
            return Player;
        }

        /// <summary>
        /// Get the specified player
        /// </summary>
        /// <param name="playerNumber">player number 0 being the first player</param>
        /// <returns></returns>
        public Player GetPlayer(int playerNumber)
        {
            return Players[playerNumber];
        }

        /// <summary>
        /// Set the current player by player number
        /// </summary>
        /// <param name="playerNumber"></param>
        public void SetPlayerByNumber(int playerNumber)
        {
            Player = Players[playerNumber];
        }
        #endregion Player Related
    }
}
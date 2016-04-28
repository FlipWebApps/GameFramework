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
    public class GameManager : SingletonPersistantSavedState<GameManager>
    {
        #region Inspector Values

        // General related
        [Tooltip("The name of this game")]
        public string GameName = "";
        [Tooltip("If targeting Andriod, a Google Play url for the game")]
        public string PlayWebUrl = "";
        [Tooltip("If targeting Andriod, a Google Play direct link for the game")]
        public string PlayMarketUrl = "";
        [Tooltip("If targeting iOS, a link to the App Store page game")]
        public string iOSWebUrl = "";
        [Tooltip("Set the base identifier to allow for multiple games in a single project")]
        public string IdentifierBase;
        [Tooltip("The amount of debug logging that should be shown (only applies in editor mode and debug builds.")]
        public MyDebug.DebugLevelType DebugLevel = MyDebug.DebugLevelType.None;

        // Display related
        [Header("Display")]
        [Tooltip("Physical size the game is designed against. Can be used later with PhysicalScreenHeightMultiplier for screen scaling")]
        public float ReferencePhysicalScreenHeightInInches = 4f;
        [Tooltip("How often to check for orientation and resolution changes")]
        public float DisplayChangeCheckDelay = 0.5f;                      // How long to wait until we check orientation & resolution changes.

        /// <summary>
        /// Game Structure setup
        /// </summary>
        [Header("Players")]
        [Tooltip("The default number of lives players will have (optional if not using lives).")]
        public int DefaultLives = 3;
        [Tooltip("The number of local players to setup")]
        public int PlayerCount = 1;

        [Header("Worlds")]
        [Tooltip("Whether to automatically setup wolds using default values.")]
        public bool AutoCreateWorlds = false;
        [Tooltip("The number of standard worlds that should be automatically created by the framework.")]
        public int NumberOfAutoCreatedWorlds = 0;
        [Tooltip("Whether to try and load data from a resources file.")]
        public bool LoadWorldDatafromResources = false;
        [Tooltip("How we plan on letting users unlock worlds.")]
        public GameItem.UnlockModeType WorldUnlockMode;
        [Tooltip("The default number of coins to unlock worlds (can be overriden by json configuration files or code).")]
        public int CoinsToUnlockWorlds = 10;
        [Tooltip("What levels numbers are assigned to each world.\nThese are used for internal lookup and references for e.g. in-app purchase.\n\nEnter a range but keep the numbers unique. You might also want to leave room to expand these later. e.g. (1-10), (101,110)")]
        public MinMax[] WorldLevelNumbers;

        [Header("Levels")]
        [Tooltip("Whether to automatically setup levels using default values.\n\nThis option is hidden if automatically creating worlds as we then need per world configuration.")]
        public bool AutoCreateLevels = false;
        [Tooltip("The number of standard levels that should be automatically created by the framework.")]
        [FormerlySerializedAs("NumberOfStandardLevels")]
        public int NumberOfAutoCreatedLevels = 10;
        [Tooltip("Whether to try and load data from a resources file.")]
        public bool LoadLevelDatafromResources = false;
        [Tooltip("How we plan on letting users unlock levels.")]
        public GameItem.UnlockModeType LevelUnlockMode;
        [Tooltip("The default number of coins to unlock levels (can be overriden by json configuration files or code).")]
        public int CoinsToUnlockLevels = 10;

        [Header("Characters")]
        [Tooltip("Whether to automatically setup characters using default values.")]
        public bool AutoCreateCharacters = false;
        [Tooltip("The number of standard characters that should be automatically created by the framework.")]
        public int NumberOfAutoCreatedCharacters = 10;
        [Tooltip("Whether to try and load data from a resources file.")]
        public bool LoadCharacterDatafromResources = false;
        [Tooltip("How we plan on letting users unlock characters.")]
        public GameItem.UnlockModeType CharacterUnlockMode;
        [Tooltip("The default number of coins to unlock characters (can be overriden by json configuration files or code).")]
        public int CoinsToUnlockCharacters = 10;

        [Tooltip("A list of localisation languages that we support")]
        public string[] SupportedLanguages;

        #endregion Inspector Values

        /// <summary>
        /// Gameplay related properties
        /// </summary>

        // set once GameSetup function is complete.
        public bool IsInitialised { get; set; }                             

        // Whether the game is unlocked. Can be used to only enable certain features and can be linked to in app purchase.
        public bool IsUnlocked
        {
            get { return _isUnlocked; }
            set
            {
                var oldValue = IsUnlocked;
                _isUnlocked = value;
                if (IsInitialised && oldValue != IsUnlocked)
                    SafeQueueMessage(new GameUnlockedMessage(IsUnlocked));
            }
        }
        bool _isUnlocked;

        [Obsolete("Use functions in LevelManager instead")]
        public bool IsPaused { get; set; }
        [Obsolete("Use functions in LevelManager instead?")]
        public bool IsUserInteractionEnabled { get; set; }
        public bool IsSplashScreenShown { get; set; }
        public int TimesGamePlayed { get; set; }
        public int TimesLevelsPlayed { get; set; }
        public int TimesPlayedForRatingPrompt { get; set; }                 // seperate from the above as if they click remind then this will be reset to 0.

        /// <summary>
        /// Audio related properties
        /// </summary>
        public AudioSource BackGroundAudioSource { get; set; }
        public AudioSource[] EffectAudioSources { get; set; }

        public float BackGroundAudioVolume
        {
            get { return _backGroundAudioVolume; }
            set
            {
                _backGroundAudioVolume = value;
                if (BackGroundAudioSource != null) BackGroundAudioSource.volume = value;
            }
        }
        float _backGroundAudioVolume;

        public float EffectAudioVolume {
            get { return _effectAudioVolume; }
            set {
                _effectAudioVolume = value;
                if (EffectAudioSources != null)
                {
                    foreach (var audioSource in EffectAudioSources)
                        audioSource.volume = value;
                }

            }
        }
        float _effectAudioVolume;

        /// <summary>
        /// Display related properties
        /// </summary>
        Vector2 _resolution;                                                 // Current Resolution - for detecting changes only
        DeviceOrientation _orientation;                                      // Current Device Orientation - for detecting changes only
        public event Action<Vector2> OnResolutionChange;                    // raised on resolution change (event as only we can invoke this)
        public event Action<DeviceOrientation> OnOrientationChange;         // raised on orientation change (event as only we can invoke this)
        public Vector3 WorldBottomLeftPosition { get; private set; }
        public Vector3 WorldTopRightPosition { get; private set; }
        public Vector3 WorldSize { get; private set; }
        public float AspectRatioMultiplier { get; private set; }            // Ratio above and beyong 4:3 ratio
        public float PhysicalScreenHeightMultiplier { get; private set; }   // Ratio above and beyong 4:3 ratio

        /// <summary>
        /// player related properties 
        /// </summary>
        public Player Player { get; private set; }
        protected Player[] Players;
        public Action<Player> PlayerChanged;                                // NOT YET IMPLEMENTED

        /// <summary>
        /// GameItemManagers
        /// </summary>
        public GameItemsManager<Character, GameItem> Characters { get; set; }
        public GameItemsManager<World, GameItem> Worlds { get; set; }
        public GameItemsManager<Level, GameItem> Levels { get; set; }

        /// <summary>
        /// Messaging
        /// </summary>
        Messenger _messenger = new Messenger();
        public static Messenger Messenger {
            get {
                return IsActive ? Instance._messenger : null;
            }
        }


        #region Setup

        protected override void GameSetup()
        {
            base.GameSetup();

            var sb = new System.Text.StringBuilder();

            MyDebug.DebugLevel = DebugLevel;

            sb.Append("GameManager: GameSetup()");
            sb.Append("\nApplication.systemLanguage: ").Append(Application.systemLanguage);


            // Gameplay related properties
            IsUnlocked = PlayerPrefs.GetInt("IsUnlocked", 0) != 0;
#pragma warning disable 618
            IsUserInteractionEnabled = true;
#pragma warning restore 618
            IsSplashScreenShown = false;
            TimesGamePlayed = PlayerPrefs.GetInt("TimesGamePlayed", 0);
            TimesGamePlayed++;
            TimesLevelsPlayed = PlayerPrefs.GetInt("TimesLevelsPlayed", 0);
            TimesPlayedForRatingPrompt = PlayerPrefs.GetInt("TimesPlayedForRatingPrompt", 0);
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

            BackGroundAudioVolume = PlayerPrefs.GetFloat("BackGroundAudioVolume", BackGroundAudioVolume);
            EffectAudioVolume = PlayerPrefs.GetFloat("EffectAudioVolume", EffectAudioVolume);


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

        void Update()
        {
            // process messages.
            _messenger.ProcessQueue();
        }

        #endregion Update Loop

        #region Display

        /// <summary>
        /// Setup some properties about the current state of the display
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
                    _resolution = new Vector2(Screen.width, Screen.height);
                    SetDisplayProperties();
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
                            _orientation = UnityEngine.Input.deviceOrientation;
                            SetDisplayProperties();
                            if (OnOrientationChange != null) OnOrientationChange(_orientation);
                        }
                        break;
                }

                yield return new WaitForSeconds(DisplayChangeCheckDelay);
            }
        }

        #endregion Display

        public override void SaveState()
        {
            MyDebug.Log("GameManager: SaveState");

            PlayerPrefs.SetInt("TimesPlayedForRatingPrompt", TimesPlayedForRatingPrompt);
            PlayerPrefs.SetInt("TimesGamePlayed", TimesGamePlayed);
            PlayerPrefs.SetInt("TimesLevelsPlayed", TimesLevelsPlayed);

            PlayerPrefs.SetFloat("BackGroundAudioVolume", BackGroundAudioVolume);
            PlayerPrefs.SetFloat("EffectAudioVolume", EffectAudioVolume);

            PlayerPrefs.Save();
        }

        #region Audio
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
        /// Get the identifier that is defined
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
        /// Safe method for adding a listener without needing to test whether a gamemanager is setup.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static void SafeAddListener<T>(Messenger.MessageListenerDelegate handler) where T : BaseMessage
        {
            if (!IsActive || Messenger == null) return;
            Messenger.AddListener<T>(handler);
        }

        /// <summary>
        /// Safe method for removing a listener without needing to test whether a gamemanager is setup.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static void SafeRemoveListener<T>(Messenger.MessageListenerDelegate handler) where T : BaseMessage
        {
            if (!IsActive || Messenger == null) return;
            Messenger.RemoveListener<T>(handler);
        }

        /// <summary>
        /// Safe method for queueing messages without needing to test whether a gamemanager is setup.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool SafeQueueMessage(BaseMessage msg)
        {
            if (!IsActive || Messenger == null) return false;
            return Messenger.QueueMessage(msg);
        }

        /// <summary>
        /// Safe method for triggering messages without needing to test whether a gamemanager is setup.
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
        /// 
        /// Player related - override if you have a custom player class.
        ///
        protected virtual Player CreatePlayer(int playerNumber)
        {
            MyDebug.Log("GameManager: CreatePlayer");

            var player = new Player();
            player.Initialise(playerNumber, localiseDescription: false);
            return player;
        }

        public Player GetPlayer()
        {
            return Player;
        }

        public Player GetPlayer(int playerNumber)
        {
            return Players[playerNumber];
        }

        public void SetPlayerByNumber(int playerNumber)
        {
            Player = Players[playerNumber];
        }
        #endregion Player Related
    }
}
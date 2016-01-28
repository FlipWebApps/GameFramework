//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using System;
using System.Collections;
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

namespace FlipWebApps.GameFramework.Scripts.GameStructure
{
    /// <summary>
    /// Contains details about the game and is the base class for managing other aspects such as levels etc.
    /// </summary>
    public class GameManager : SingletonPersistantSavedState<GameManager>
    {
        [Header("Game Details")]
        public string GameName = "";
        public string PlayWebUrl = "";
        public string PlayMarketUrl = "";
        public string iOSWebUrl = "";

        [Header("Game Setup")]
        public int PlayerCount = 1;
        public bool IsUnlocked;
        public string IdentifierBase;

        [Header("Basic Level Setup")]
        public LevelGameItemsManager.LevelUnlockModeType LevelUnlockMode;
        public int NumberOfStandardLevels = 10;
        public int CoinsToUnlockLevels = 10;

        [Header("Display")]
        public float ReferencePhysicalScreenHeightInInches = 4f;
        public float DisplayChangeCheckDelay = 0.5f;                      // How long to wait until we check orientation & resolution changes.

        /// <summary>
        /// Gameplay related properties
        /// </summary>
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
        public AudioSource EffectAudioSource { get; set; }
        float _backGroundAudioVolume;
        public float BackGroundAudioVolume { get { return _backGroundAudioVolume; } set { _backGroundAudioVolume = value; if (BackGroundAudioSource != null) BackGroundAudioSource.volume = value; } }
        float _effectAudioVolume;
        public float EffectAudioVolume { get { return _effectAudioVolume; } set { _effectAudioVolume = value; if (EffectAudioSource != null) EffectAudioSource.volume = value; } }

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

        protected override void GameSetup()
        {
            base.GameSetup();

            Debug.Log("GameManager: GameSetup");
            Debug.Log("Application.systemLanguage: " + Application.systemLanguage);

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
            Debug.Log("TimesGamePlayed : " + TimesGamePlayed);
            Debug.Log("TimesLevelsPlayed : " + TimesLevelsPlayed);
            Debug.Log("TimesPlayedForRatingPrompt : " + TimesPlayedForRatingPrompt);
            Debug.Log("Application.PersistantDataPath : " + Application.persistentDataPath);

            // audio related properties
            var audioSources = GetComponents<AudioSource>();
            if (audioSources.Length == 0)
            {
                Debug.LogWarning(
                    "To make use of the Game Manager audio functions you should add 2 AudioSource components to the same gameobject as the GameManager. The first for background audio and the second for effects.");
            }
            else
            {
                if (audioSources.Length > 0)
                    BackGroundAudioSource = GetComponents<AudioSource>()[0];
                if (audioSources.Length > 1)
                    EffectAudioSource = GetComponents<AudioSource>()[1];
            }

            BackGroundAudioVolume = PlayerPrefs.GetFloat("BackGroundAudioVolume", BackGroundAudioSource != null ? BackGroundAudioSource.volume : 1);
            EffectAudioVolume = PlayerPrefs.GetFloat("EffectAudioVolume", EffectAudioSource != null ? EffectAudioSource.volume : 1);

            // display related properties
            SetDisplayProperties();

            // Localisation setup. If nothing stored then use system Language if it exists. Otherwise we will default to English.
            if (PlayerPrefs.GetString("Language") == null)
            {
                if (Array.Exists(LocaliseText.Languages, s => s.Equals(Application.systemLanguage.ToString())))
                {
                    LocaliseText.Language = Application.systemLanguage.ToString();
                }
            }

            // setup players.
            Players = new Player[Instance.PlayerCount];
            for (var i = 0; i < Instance.PlayerCount; i++)
                Players[i] = CreatePlayer(i);
            SetPlayerByNumber(0);

#if UNITY_EDITOR
#elif UNITY_ANDROID
#elif UNITY_IPHONE
#endif

            // setup levels if we aren't doing this ourselves
            if (LevelUnlockMode != LevelGameItemsManager.LevelUnlockModeType.Custom)
            {
                Levels = new LevelGameItemsManager(NumberOfStandardLevels, LevelUnlockMode, CoinsToUnlockLevels);
                Levels.Load();
            }

            // coroutine to check for display changes (don't need to do this every frame)
            if (!Mathf.Approximately(DisplayChangeCheckDelay, 0))
                StartCoroutine(CheckForDisplayChanges());
        }


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

            Debug.Log("WorldBottomLeftPosition: " + WorldBottomLeftPosition);
            Debug.Log("WorldTopRightPosition: " + WorldTopRightPosition);
            Debug.Log("WorldSize: " + WorldSize);
            Debug.Log("AspectRatioMultiplier: " + AspectRatioMultiplier);
            Debug.Log("Screen size is : " + DisplayMetrics.GetPhysicalWidth() + "x" + +DisplayMetrics.GetPhysicalHeight() + "\"");
            Debug.Log("PhysicalScreenHeightMultiplier: " + PhysicalScreenHeightMultiplier);
        }


        public override void SaveState()
        {
            Debug.Log("GameManager: SaveState");

            PlayerPrefs.SetInt("TimesPlayedForRatingPrompt", TimesPlayedForRatingPrompt);
            PlayerPrefs.SetInt("TimesGamePlayed", TimesGamePlayed);
            PlayerPrefs.SetInt("TimesLevelsPlayed", TimesLevelsPlayed);

            PlayerPrefs.SetFloat("BackGroundAudioVolume", BackGroundAudioVolume);
            PlayerPrefs.SetFloat("EffectAudioVolume", EffectAudioVolume);

            PlayerPrefs.Save();
        }


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


        public void PlayEffect(AudioClip clip)
        {
            PlayEffect(clip, 1, 1);
        }

        public void PlayEffect(AudioClip clip, float pitchLow, float pitchHigh)
        {
            EffectAudioSource.clip = clip;
            EffectAudioSource.pitch = UnityEngine.Random.Range(pitchLow, pitchHigh);
            EffectAudioSource.Play();
        }

        /// <summary>
        /// Get the theme that is defined
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
                string newSceneName = GameManager.GetIdentifierBase() == null
                    ? sceneName
                    : GameManager.GetIdentifierBase() + "-" + sceneName;
            return newSceneName;
        }


        /// 
        /// Player related - override if you have a custom player class.
        ///
        protected virtual Player CreatePlayer(int playerNumber)
        {
            Debug.Log("GameManager: CreatePlayer");

            return new Player(playerNumber);
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
    }
}
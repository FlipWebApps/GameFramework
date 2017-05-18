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
using System.Linq;
using GameFramework.GameObjects.Components;
using GameFramework.Localisation.ObjectModel;
using GameFramework.GameStructure;
using UnityEngine;
using UnityEngine.Assertions;
using GameFramework.Preferences;
using GameFramework.Localisation.Messages;

namespace GameFramework.Localisation.Components
{
    /// <summary>
    /// Provides dialog creation, display and management functionality.
    /// </summary>
    [AddComponentMenu("Game Framework/Localisation/Localisation Manager")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/localisation/")]
    public class LocalisationManager : Singleton<LocalisationManager>
    {
        /// <summary>
        /// Different modes for setting up the localisation
        /// </summary>
        /// Auto = Try loading from a resources folder with the names Default/Localisation & Localisation
        /// Specified = Specified LocalisationData files are passed in.
        public enum SetupModeType { Auto, Specified }

        #region editor fields

        /// <summary>
        /// How to setup the localisation either by default or specified resource files.
        /// </summary>
        public SetupModeType SetupMode
        {
            get
            {
                return _setupMode;
            }
            set
            {
                _setupMode = value;
            }
        }
        [SerializeField]
        [Tooltip("How to setup the localisation either by default or specified resource files.")]
        SetupModeType _setupMode = SetupModeType.Auto;

        /// <summary>
        /// A list of specified localisation data files for loading
        /// </summary>
        /// Values in files towards the bottom of the list will override any values present in earlier files.
        public LocalisationData[] SpecifiedLocalisationData
        {
            get
            {
                return _specifiedLocalisationData;
            }
            set
            {
                _specifiedLocalisationData = value;
            }
        }
        [SerializeField]
        [Tooltip("A list of specified localisation data files. Values in files towards the bottom of the list will override any values present in earlier files.")]
        LocalisationData[] _specifiedLocalisationData = new LocalisationData[0];

        /// <summary>
        /// A list of localisation languages that we support
        /// </summary>
        public string[] SupportedLanguages
        {
            get
            {
                return _supportedLanguages;
            }
            set
            {
                _supportedLanguages = value;
            }
        }
        [SerializeField]
        [Tooltip("A list of localisation languages that we support")]
        string[] _supportedLanguages = new string[0];

        #endregion editor fields

        /// <summary>
        /// A combined LocalisationData from all added sources
        /// </summary>
        public LocalisationData LocalisationData { get; private set; }

        /// <summary>
        /// The currently active Language. 
        /// </summary>
        /// This will allow you to set the Language to something that isn't in the AllowedLanguages array.
        public string Language
        {
            get
            {
                return _language;
            }
            set
            {
                // make sure we change something
                var oldLanguage = _language;
                if (oldLanguage == value) return;

                // check we can use this language
                if (!CanUseLanguage(value)) return;

                // set and notify of changes
                _language = value;
                _languageIndex = LocalisationData.GetLanguageIndex(_language);
                PreferencesFactory.SetString("Language", Language, false);
                GameManager.SafeQueueMessage(new LocalisationChangedMessage(_language, oldLanguage));
            }
        }
        string _language;
        int _languageIndex;

        void Awake()
        {
            LoadLocalisationData();
            foreach (var language in SupportedLanguages)
                if (!LocalisationData.ContainsLanguage(language))
                    Debug.Log("Localisation files do not contain definitions for the specified supported language '" + language + "'");
        }

        #region Load Dictionary

        /// <summary>
        /// Load LocalisationData files
        /// </summary>
        public void LoadLocalisationData()
        {
            LocalisationData = null;

            if (SetupMode == SetupModeType.Auto)
            {
                // Try to load the default Localisation file directly - don't use GameMangager method as we load in 'reverse'
                var asset = Resources.Load<LocalisationData>("Default/Localisation");
                if (asset != null)
                    LocalisationData = Instantiate(asset);  // create a copy so we don't overwrite values.

                // try and load identifier localisation
                if (GameManager.IsActive && GameManager.GetIdentifierBase() != null)
                {
                    asset = Resources.Load<LocalisationData>(GameManager.GetIdentifierBase() + "/Localisation");
                    if (asset != null)
                    {
                        if (LocalisationData == null)
                            LocalisationData = asset;
                        else
                            LocalisationData.Merge(asset);
                    }
                }

                // try and load user localisation
                if (GameManager.IsActive && GameManager.GetIdentifierBase() != null)
                {
                    asset = Resources.Load<LocalisationData>("Localisation");
                    if (asset != null)
                    {
                        if (LocalisationData == null)
                            LocalisationData = asset;
                        else
                            LocalisationData.Merge(asset);
                    }
                }
            }
            else if (SetupMode == SetupModeType.Specified)
            {
                foreach (var localisationData in SpecifiedLocalisationData)
                {
                    if (LocalisationData == null)
                        LocalisationData = Instantiate(localisationData);  // create a copy so we don't overwrite values.
                    else
                        LocalisationData.Merge(localisationData);
                }
            }

            Assert.IsNotNull(LocalisationData, "LocalisationManager: No localisation data was loaded. Please check that localisation files exist and are in the correct location!");

            // if no usable language is already set then set to the default language.
            if (!CanUseLanguage(Language))
                SetLanguageToDefault();
        }
        #endregion Load Dictionary

        #region Language
        /// <summary>
        /// Returns whether the passed language can be used (is in the loaded definition and in supported languages).
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public bool CanUseLanguage(string language)
        {
            if (LocalisationData == null) return false;

            // make sure it is loaded
            if (!LocalisationData.ContainsLanguage(language))
                return false;

            // make sure it is supported
            if (!SupportedLanguages.Contains(language))
                return false;
            return true;
        }


        /// <summary>
        /// Try and set the specified language, returning true if it was set or false otherwise.
        /// </summary>
        /// <param name="newDefaultLanguage"></param>
        /// <returns></returns>
        public bool TrySetAllowedLanguage(string newDefaultLanguage)
        {
            if (CanUseLanguage(newDefaultLanguage))
            {
                Language = newDefaultLanguage;
                return Language == newDefaultLanguage;
            }
            return false;
        }


        void SetLanguageToDefault(bool keepIfAlreadySet = false)
        {
            if (keepIfAlreadySet && !string.IsNullOrEmpty(Language))
                return;

            // 1. try and set from prefs
            var prefsLanguage = PreferencesFactory.GetString("Language", useSecurePrefs: false);
            if (TrySetAllowedLanguage(prefsLanguage)) return;

            //// 2. if not then try and set system Language.
            if (TrySetAllowedLanguage(Application.systemLanguage.ToString())) return;

            // 3. use the first language from any user localisation file
            //Language = _defaultUserLanguage;
            if (SupportedLanguages.Length > 0 && TrySetAllowedLanguage(SupportedLanguages[0])) return;

            //// 2. if not set then fall back to first Language from first (default) file
            if (LocalisationData.Languages.Count > 0 && TrySetAllowedLanguage(LocalisationData.Languages[0].Name)) return;
        }

        #endregion Language

        #region Access

        /// <summary>
        /// Returns whether the specified key is present.
        /// </summary>
        public bool Exists(string key)
        {
            if (LocalisationData == null) return false;
            return LocalisationData.ContainsEntry(key);
        }


        /// <summary>
        /// Localise the specified value based on the currently set language.
        /// </summary>
        /// If language is specific then this method will try and get the key for that particular value, returning null if not found.
        public string GetText(string key, string language = null, bool missingReturnsNull = false)
        {
            Assert.IsNotNull(LocalisationData, "Localisation data has not been loaded. Ensure that you have a Localisation Manager added to your scene and if needed increase the script execution of that component.");

            if (language == null)
            {
                return LocalisationData.GetText(key, _languageIndex);
            }
            else
            {
                return (LocalisationData.GetText(key, language));
            }
            //// try and get the key
            //string[] vals;
            //if (_localisations.TryGetValue(key, out vals))
            //{
            //    if (language == null)
            //    {
            //        if (_languageIndex < vals.Length)
            //            return vals[_languageIndex];
            //    }
            //    else
            //    {
            //        // get value for a specific language
            //        var index = Array.IndexOf(Languages, language);
            //        if (index == -1) return null;
            //        if (index < vals.Length)
            //            return vals[index];
            //        else
            //            return null;
            //    }
            //}

            //if (missingReturnsNull) return null;

            //MyDebug.LogWarningF("Localisation key not found: '{0}' for Language {1}", key, Language);
            //return key;
            return null;
        }


        /// <summary>
        /// Get the localised value and format it.
        /// </summary>
        public string FormatText(string key, params object[] parameters) { return string.Format(GetText(key), parameters); }

        #endregion Access
    }
}
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

using System.Linq;
using GameFramework.Localisation.ObjectModel;
using GameFramework.GameStructure;
using UnityEngine;
using GameFramework.Preferences;
using GameFramework.Localisation.Messages;
using GameFramework.Localisation.Components;
using System.Collections.Generic;
using GameFramework.Debugging;

namespace GameFramework.Localisation
{
    /// <summary>
    /// Support for a shared global localisation including retrieval and displaying of text and notifications.
    /// </summary>
    /// LocalisationData files are loaded based upon the configuration in a LocalisationConfiguration component if one is found, otherwise using 
    /// an automatic detection mode....
    /// Supported languages are loaded from a LocalisationConfiguration component if one is added to the scene. Otherwise this is taken from the 
    /// last loaded LocalisationData file.
    /// For further information please see: http://www.flipwebapps.com/unity-assets/game-framework/localisation/
    public static class GlobalLocalisation
    {
        /// <summary>
        /// A list of localisation languages that we support. This is either loaded from a LocalisationConfiguration file if
        /// present of based upon the loaded localisations.
        /// </summary>
        public static string[] SupportedLanguages
        {
            get
            {
                Load();
                return _supportedLanguages;
            }
            set
            {
                _supportedLanguages = value;
            }
        }
        static string[] _supportedLanguages = new string[0];

        /// <summary>
        /// A combined LocalisationData from all added sources
        /// </summary>
        public static LocalisationData LocalisationData { get; private set; }

        /// <summary>
        /// The currently active Language. 
        /// </summary>
        /// This will allow you to set the Language to something that isn't in the AllowedLanguages array.
        public static string Language
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
        static string _language;
        static int _languageIndex;

        #region Load

        /// <summary>
        /// Load LocalisationData files
        /// </summary>
        public static void Load(LocalisationConfiguration localisationConfiguration = null)
        {
            if (LocalisationData != null) return;
            Reload(localisationConfiguration);
        }

        /// <summary>
        /// Load LocalisationData files
        /// </summary>
        public static void Reload(LocalisationConfiguration localisationConfiguration = null)
        {
            Clear();

            if (localisationConfiguration == null)
                localisationConfiguration = GameManager.LoadResource<LocalisationConfiguration>("LocalisationConfiguration");
                //localisationConfiguration = GameObject.FindObjectOfType<LocalisationConfiguration>();
            var setupMode = localisationConfiguration == null ? LocalisationConfiguration.SetupModeType.Auto : localisationConfiguration.SetupMode;
            string[] loadedSupportedLanguages = new string[0];

            // set localisation data
            if (setupMode == LocalisationConfiguration.SetupModeType.Auto)
            {
                // Try to load the default Localisation file directly - don't use GameMangager method as we load in 'reverse' as user items
                // should overwrite system ones.
                var asset = Resources.Load<LocalisationData>("Default/Localisation");
                if (asset != null)
                {
                    LocalisationData = ScriptableObject.Instantiate(asset);  // create a copy so we don't overwrite values.
                    loadedSupportedLanguages = asset.GetLanguageNames();
                }

                // try and load identifier localisation if specified and present, or if not user localisation
                var identifierLocalisationLoaded = false;
                if (GameManager.IsActive && GameManager.GetIdentifierBase() != null)
                {
                    asset = Resources.Load<LocalisationData>(GameManager.GetIdentifierBase() + "/Localisation");
                    if (asset != null)
                    {
                        identifierLocalisationLoaded = true;
                        if (LocalisationData == null)
                            LocalisationData = asset;
                        else
                            LocalisationData.Merge(asset);
                        loadedSupportedLanguages = asset.GetLanguageNames(); // override any previous
                    }
                }
                if (!identifierLocalisationLoaded)
                {
                    asset = Resources.Load<LocalisationData>("Localisation");
                    if (asset != null)
                    {
                        if (LocalisationData == null)
                            LocalisationData = asset;
                        else
                            LocalisationData.Merge(asset);
                        loadedSupportedLanguages = asset.GetLanguageNames(); // override any previous
                    }
                }
                if (LocalisationData == null)
                    MyDebug.LogWarning("GlobalLocalisation: No localisation data was found so creating an empty one. Please check that a localisation files exist at /Resources/Localisation or /Resources/Default/Localisation!");
            }
            else if (setupMode == LocalisationConfiguration.SetupModeType.Specified)
            {
                foreach (var localisationData in localisationConfiguration.SpecifiedLocalisationData)
                {
                    // first item gets loaded / copied, subsequent get merged into this.
                    if (LocalisationData == null)
                        LocalisationData = ScriptableObject.Instantiate(localisationData);  // create a copy so we don't overwrite values.
                    else
                    {
                        LocalisationData.Merge(localisationData);
                    }
                    loadedSupportedLanguages = localisationData.GetLanguageNames(); // if exists override
                }
                if (LocalisationData == null)
                    MyDebug.LogWarning("GlobalLocalisation: No localisation data was found so creating an empty one. Please check that localisation files exist and are in the correct location!");
            }

            // if nothing loaded then create an empty localisation to avoid errors.
            if (LocalisationData == null)
            {
                LocalisationData = ScriptableObject.CreateInstance<LocalisationData>();
                LocalisationData.AddLanguage("English", "en");
                loadedSupportedLanguages = LocalisationData.GetLanguageNames();
            }

            // set Supported Languages - either from config if present or based upon loaded files.
            if (localisationConfiguration != null && localisationConfiguration.SupportedLanguages.Length > 0)
            {
                List<string> validSupportedLanguages = new List<string>();
                foreach (var language in localisationConfiguration.SupportedLanguages) {
                    if (LocalisationData.ContainsLanguage(language))
                            validSupportedLanguages.Add(language);
                    else
                        Debug.Log("GlobalLocalisation: Localisation files do not contain definitions for the specified supported language '" + language + "'");
                }
                    SupportedLanguages = validSupportedLanguages.ToArray();
            }
            else
            {
                SupportedLanguages = loadedSupportedLanguages;
            }


            // if no usable language is already set then set to the default language.
            if (!CanUseLanguage(Language))
                SetLanguageToDefault();
        }

        /// <summary>
        /// Clear any loaded dictionaries
        /// </summary>
        public static void Clear()
        {
            LocalisationData = null;
            SupportedLanguages = new string[0];
            _language = null;
            _languageIndex = -1;
        }
        #endregion Load

        #region Language
        /// <summary>
        /// Returns whether the passed language can be used (is in the loaded definition and in supported languages).
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static bool CanUseLanguage(string language)
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
        public static bool TrySetAllowedLanguage(string newDefaultLanguage)
        {
            if (CanUseLanguage(newDefaultLanguage))
            {
                Language = newDefaultLanguage;
                return Language == newDefaultLanguage;
            }
            return false;
        }


        static void SetLanguageToDefault(bool keepIfAlreadySet = false)
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
            if (LocalisationData != null && LocalisationData.Languages.Count > 0 && TrySetAllowedLanguage(LocalisationData.Languages[0].Name)) return;
        }

        #endregion Language

        #region Access

        /// <summary>
        /// Returns whether the specified key is present.
        /// </summary>
        public static bool Exists(string key)
        {
            Load();
            return LocalisationData.ContainsEntry(key);
        }


        /// <summary>
        /// Localise the specified value based on the currently set language.
        /// </summary>
        /// If language is specific then this method will try and get the key for that particular value, returning null if not found unless
        /// missingReturnsKey is set in which case the key will be returned.
        public static string GetText(string key, string language = null, bool missingReturnsKey = false)
        {
            string text;
            Load();
            if (language == null)
                text = LocalisationData.GetText(key, _languageIndex);
            else
                text = LocalisationData.GetText(key, language);

            return (text != null || !missingReturnsKey) ? text : key;
        }


        /// <summary>
        /// Get the localised value and format it.
        /// </summary>
        /// This will return null if the key is not found.
        public static string FormatText(string key, params object[] parameters) {
            var text = GetText(key);
            if (text != null)
                return string.Format(text, parameters);
            else
                return null;
        }

        #endregion Access
    }
}
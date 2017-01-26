
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
using System.IO;
using System.Linq;
using System.Text;
using GameFramework.Debugging;
using GameFramework.GameStructure;
using UnityEngine;
using GameFramework.Localisation.Messages;
using GameFramework.Preferences;


namespace GameFramework.Localisation
{
/// <summary>
/// Support for localisation including retrieval and displaying of text, helper components and notifications.
/// </summary>
/// For further information please see: http://www.flipwebapps.com/unity-assets/game-framework/localisation/
    public static class LocaliseText
    {
        /// <summary>
        /// Whether the localisation text is loaded.
        /// </summary>
        public static bool IsLocalisationLoaded { get; set; }

        /// <summary>
        /// Dictionary key is the Localisation key, values are array of languages from csv file.
        /// </summary>
        public static Dictionary<string, string[]> Localisations
        {
            get
            {
                return _localisations;
            }
        }
        static readonly Dictionary<string, string[]> _localisations = new Dictionary<string, string[]>();


        /// <summary>
        /// List of allowed languages. As the default file can contain additional languages that the game doesn't localise for
        /// then we specify what the user can actually choose from. If this is empty then we default to the first language in the 
        /// user / main localisation file.
        /// </summary>
        public static string[] AllowedLanguages { get; set; }


        /// <summary>
        /// List of loaded languages.
        /// </summary>
        public static string[] Languages
        {
            get
            {
                return _languages;
            }
        }
        static string[] _languages = new string[0];


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

                // make sure it is loaded
                var index = Array.IndexOf(Languages, value);
                if (index == -1) return;

                _language = value;
                _languageIndex = index;
                PreferencesFactory.SetString("Language", Language, false);

                // notify of change here
                GameManager.SafeQueueMessage(new LocalisationChangedMessage(_language, oldLanguage));
            }
        }
        static string _language;                        // selected Language
        static int _languageIndex = -1;                 // selected Language index
        static string _defaultUserLanguage;             // the first language from the user localisation file.


        /// <summary>
        /// Static constructor 
        /// </summary>
        static LocaliseText()
        {
            AllowedLanguages = new string[0];
        }

        #region Load Dictionary

        /// <summary>
        /// Load the default and user Localisation files if not already loaded
        /// </summary>
        public static void LoadDictionary()
        {
            if (!IsLocalisationLoaded)
            {
                ReloadDictionary();
            }
        }

        /// <summary>
        /// Clear any loaded dictionaries
        /// </summary>
        public static void ClearDictionaries()
        {
            _localisations.Clear();
            _languages = new string[0];
            IsLocalisationLoaded = false;
            _language = null;
            _languageIndex = -1;
            _defaultUserLanguage = null;
        }

        /// <summary>
        /// (re)load the default and user Localisation files
        /// </summary>
        public static void ReloadDictionary()
        {
            ClearDictionaries();

            // Try to load the default Localisation file directly
            var asset = Resources.Load<TextAsset>("Default/Localisation");
            if (asset != null)
                IsLocalisationLoaded = LoadCSV(asset.bytes, true);

            // Try and additionally load user specific localisations and overrides
            asset = GameManager.LoadResource<TextAsset>("Localisation");
            if (asset != null)
                LoadCSV(asset.bytes);

            // if loaded then set default language
            if (IsLocalisationLoaded)
            {
                LogState();
                SetDefaultLanguage();
            }
            else
            {
                Debug.LogError("Could not load localisation data");
            }
        }

        #endregion Load Dictionary

        #region Load CSV
        static bool LoadCSV(byte[] bytes, bool isMasterFile = false)
        {
            try
            {
                using (var rdr = new StreamReader(new MemoryStream(bytes)))
                {
                    bool isHeader = true;
                    int[] columnLanguageMapping = new int[0];
                    foreach (IList<string> columns in FromReader(rdr))
                    {
                        // first row is the header
                        if (isHeader)
                        {
                            // sanity check
                            if (columns.Count < 2)
                            {
                                Debug.Log("File should contain at least 2 columns (KEY,<Language>");
                                return false;
                            }

                            // find what languages are new and update references for existing ones
                            columnLanguageMapping = new int[columns.Count];                 // mapping from new columns to languages array
                            var newLanguageCount = columns.Count - 1;               // the number of languages in the file (1 less for 'KEY')
                            for (var column = 1; column < columns.Count; ++column)
                            {
                                columnLanguageMapping[column] = -1;  // default to not found

                                // check all languages for a match and update if found.
                                for (var j = 0; j < Languages.Length; ++j)
                                {
                                    if (Languages[j] == columns[column])
                                    {
                                        columnLanguageMapping[column] = j;
                                        newLanguageCount--;

                                        if (!isMasterFile && _defaultUserLanguage == null)
                                            _defaultUserLanguage = Languages[j];

                                        break;
                                    }
                                }
                            }

                            // resize the languages array
                            Array.Resize(ref _languages, Languages.Length + newLanguageCount);

                            // copy new languages to the end and update mapping references
                            int maxLanguageColumn = Languages.Length - newLanguageCount;
                            for (int i = 1; i < columns.Count; ++i)
                            {
                                if (columnLanguageMapping[i] == -1)
                                {
                                    columnLanguageMapping[i] = maxLanguageColumn;
                                    Languages[maxLanguageColumn++] = columns[i];
                                }
                            }

                            isHeader = false;
                        }

                        else
                        {
                            bool isNewValue = true;
                            string key = columns[0];
                            string[] oldValues;
                            string[] values = new string[Languages.Length];

                            // get reference to existing list if any and copy old values over
                            if (Localisations.TryGetValue(key, out oldValues))
                            {
                                isNewValue = false;
                                Array.Copy(oldValues, values, oldValues.Length);
                            }

                            // copy in new values
                            for (int i = 1; i < columns.Count; ++i)
                            {
                                values[columnLanguageMapping[i]] = columns[i];
                            }

                            if (isNewValue)
                                Localisations.Add(key, values);
                            else
                                Localisations[key] = values;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }


        static IEnumerable<IList<string>> FromReader(TextReader csv)
        {
            IList<string> result = new List<string>();

            StringBuilder curValue = new StringBuilder();
            var c = (char)csv.Read();
            while (csv.Peek() != -1)
            {
                // when we are here we are at the start of a new column and c contains the first character
                switch (c)
                {
                    case ',': //empty field
                        result.Add("");
                        c = (char)csv.Read();
                        break;
                    case '"': //qualified text
                    case '\'':
                        char q = c;
                        c = (char)csv.Read();
                        bool inQuotes = true;
                        while (inQuotes && csv.Peek() != -1)
                        {
                            if (c == q)
                            {
                                c = (char)csv.Read();
                                if (c != q)
                                    inQuotes = false;
                            }

                            if (inQuotes)
                            {
                                curValue.Append(c);
                                c = (char)csv.Read();
                            }
                        }
                        result.Add(curValue.ToString());
                        curValue = new StringBuilder();
                        if (c == ',') c = (char)csv.Read(); // either ',', newline, or endofstream
                        break;
                    case '\n': //end of the record
                    case '\r':
                        //potential bug here depending on what your line breaks look like
                        if (result.Count > 0) // don't return empty records
                        {
                            yield return result;
                            result = new List<string>();
                        }
                        c = (char)csv.Read();
                        break;
                    default: //normal unqualified text
                        while (c != ',' && c != '\r' && c != '\n' && csv.Peek() != -1)
                        {
                            curValue.Append(c);
                            c = (char)csv.Read();
                        }
                        // if end of file then make sure that we add the last read character
                        if (csv.Peek() == -1)
                            curValue.Append(c);

                        result.Add(curValue.ToString());
                        curValue = new StringBuilder();
                        if (c == ',') c = (char)csv.Read(); //either ',', newline, or endofstream
                        break;
                }

            }
            if (curValue.Length > 0) //potential bug: I don't want to skip on a empty column in the last record if a caller really expects it to be there
                result.Add(curValue.ToString());
            if (result.Count > 0)
                yield return result;

        }

        #endregion Load CSV

        static void SetDefaultLanguage()
        {
            // 1. try and set from prefs
            if (TrySetAllowedLanguage(PreferencesFactory.GetString("Language", useSecurePrefs: false))) return;

            // 2. if not then try and set system Language.
            if (TrySetAllowedLanguage(Application.systemLanguage.ToString())) return;

            // 3. use the first language from any user localisation file
            Language = _defaultUserLanguage;
            if (_languageIndex != -1) return;

            // 2. if not set then fall back to first Language from first (default) file
            Language = Languages[0];
        }


        /// <summary>
        /// Try and set the specified language, first verifying that we have it loaded and it is allowed to select it.
        /// </summary>
        /// <param name="newDefaultLanguage"></param>
        /// <returns></returns>
        public static bool TrySetAllowedLanguage(string newDefaultLanguage)
        {
            if (AllowedLanguages.Contains(newDefaultLanguage) && Languages.Contains(newDefaultLanguage))
            {
                Language = newDefaultLanguage;
                return true;
            }
            return false;
        }


        /// <summary>
        /// Localise the specified value based on the currently set language.
        /// </summary>
        /// If language is specific then this method will try and get the key for that particular value, returning null if not found.
        public static string Get(string key, string language = null, bool missingReturnsNull = false)
        {
            // Ensure we have a Language to work with
            LoadDictionary();
            if (!IsLocalisationLoaded) return null;

            // try and get the key
            string[] vals;
            if (_localisations.TryGetValue(key, out vals))
            {
                if (language == null)
                {
                    if (_languageIndex < vals.Length)
                        return vals[_languageIndex];
                }
                else
                {
                    // get value for a specific language
                    var index = Array.IndexOf(Languages, language);
                    if (index == -1) return null;
                    if (index < vals.Length)
                        return vals[index];
                    else
                        return null;
                }
            }

            if (missingReturnsNull) return null;

            MyDebug.LogWarningF("Localisation key not found: '{0}' for Language {1}", key, Language);
            return key;
        }


        /// <summary>
        /// Get the localised value and format it.
        /// </summary>
        public static string Format(string key, params object[] parameters) { return string.Format(Get(key), parameters); }


        /// <summary>
        /// Returns whether the specified key is present.
        /// </summary>
        public static bool Exists(string key)
        {
            // Ensure we have a Language to work with
            LoadDictionary();
            if (!IsLocalisationLoaded) return false;

            return _localisations.ContainsKey(key);
        }


        public static void LogState()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var language in Languages)
            {
                sb.Append(language + ", ");
            }
            MyDebug.Log(sb);

            sb = new StringBuilder();
            foreach (var key in Localisations.Keys)
            {
                sb.Append(key + ": ");
                foreach (var value in Localisations[key])
                    sb.Append(value + ",");
                sb.AppendLine();
            }
            MyDebug.Log(sb);
        }
    }
}
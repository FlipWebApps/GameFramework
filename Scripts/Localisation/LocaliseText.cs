
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

using System.Collections.Generic;
using UnityEngine;

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
        public static string[] AllowedLanguages {
            get {
                ShowDeprecationWarning();
                return GlobalLocalisation.SupportedLanguages;
            }
        }


        /// <summary>
        /// List of loaded languages.
        /// </summary>
        public static string[] Languages
        {
            get
            {
                ShowDeprecationWarning();
                var languages = new List<string>();
                foreach (var language in GlobalLocalisation.LocalisationData.Languages)
                    languages.Add(language.Name);
                return languages.ToArray();
            }
        }


        /// <summary>
        /// The currently active Language. 
        /// </summary>
        /// This will allow you to set the Language to something that isn't in the AllowedLanguages array.
        public static string Language
        {
            get
            {
                return GlobalLocalisation.Language;
            }
            set
            {
                GlobalLocalisation.Language = value;
            }
        }


        static bool _hasShownWarning = false;
        public static void ShowDeprecationWarning()
        {
            if (!_hasShownWarning)
            {
                Debug.LogError("LocaliseText and the use of csv files as a way of doing localisation has been replaced. Please convert csv files to Localisation files and code calls from LocaliseText to GlobalLocalisation. See the documentation for further upgrade details.");
            }
            _hasShownWarning = true;
        }

        #region Load Dictionary

        /// <summary>
        /// Load the default and user Localisation files if not already loaded
        /// </summary>
        public static void LoadDictionary()
        {
            ShowDeprecationWarning();
        }

        /// <summary>
        /// Clear any loaded dictionaries
        /// </summary>
        public static void ClearDictionaries()
        {
            ShowDeprecationWarning();
        }

        /// <summary>
        /// (re)load the default and user Localisation files
        /// </summary>
        public static void ReloadDictionary()
        {
            ShowDeprecationWarning();
        }

        #endregion Load Dictionary

        /// <summary>
        /// Try and set the specified language, first verifying that we have it loaded and it is allowed to select it.
        /// </summary>
        /// <param name="newDefaultLanguage"></param>
        /// <returns></returns>
        public static bool TrySetAllowedLanguage(string newDefaultLanguage)
        {
            ShowDeprecationWarning();
            return GlobalLocalisation.TrySetAllowedLanguage(newDefaultLanguage);
        }


        /// <summary>
        /// Localise the specified value based on the currently set language.
        /// </summary>
        /// If language is specific then this method will try and get the key for that particular value, returning null if not found.
        public static string Get(string key, string language = null, bool missingReturnsNull = false)
        {
            ShowDeprecationWarning();
            return GlobalLocalisation.GetText(key, language) ?? key;
        }


        /// <summary>
        /// Get the localised value and format it.
        /// </summary>
        public static string Format(string key, params object[] parameters) { return string.Format(Get(key, missingReturnsNull: true), parameters); }


        /// <summary>
        /// Returns whether the specified key is present.
        /// </summary>
        public static bool Exists(string key)
        {
            ShowDeprecationWarning();
            return GlobalLocalisation.Exists(key);
        }


        //public static void LogState()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (var language in Languages)
        //    {
        //        sb.Append(language + ", ");
        //    }
        //    MyDebug.Log(sb);

        //    sb = new StringBuilder();
        //    foreach (var key in Localisations.Keys)
        //    {
        //        sb.Append(key + ": ");
        //        foreach (var value in Localisations[key])
        //            sb.Append(value + ",");
        //        sb.AppendLine();
        //    }
        //    MyDebug.Log(sb);
        //}
    }
}
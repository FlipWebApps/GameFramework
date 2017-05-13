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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework.Localisation.ObjectModel
{

    [CreateAssetMenu(fileName = "Localisation", menuName = "Game Framework/Localisation")]
    [System.Serializable]
    public class LocalisationData : ScriptableObject //, ISerializationCallbackReceiver
    {
        /// <summary>
        /// List of loaded languages.
        /// </summary>
        List<Language> Languages
        {
            get
            {
                return _languages;
            }
        }
        [SerializeField]
        List<Language> _languages = new List<Language>();

        /// <summary>
        /// List of loaded localisation entries.
        /// </summary>
        List<LocalisationEntry> LocalisationEntries
        {
            get
            {
                return _localisationEntries;
            }
        }
        [SerializeField]
        List<LocalisationEntry> _localisationEntries = new List<LocalisationEntry>();

        /// <summary>
        /// Dictionary key is the Localisation key, values are array of languages from csv file.
        /// </summary>
        static Dictionary<string, LocalisationEntry> Localisations
        {
            get
            {
                return _localisations;
            }
        }
        static readonly Dictionary<string, LocalisationEntry> _localisations = new Dictionary<string, LocalisationEntry>(System.StringComparer.Ordinal);

        void PopulateDictionary()
        {
            _localisations.Clear();
            foreach (var localisationEntry in LocalisationEntries)
            {
                _localisations.Add(localisationEntry.Key, localisationEntry);
            }
        }

        //public void OnBeforeSerialize()
        //{
        //    LocalisationEntries.Clear();
        //    foreach (var localisation in Localisations)
        //    {
        //        LocalisationEntries.Add(localisation.Value);
        //    }
        //}

        //public void OnAfterDeserialize()
        //{
        //    PopulateDictionary();
        //}

        /// <summary>
        /// Internal method for verifying that the dictionary and list are synchronised and that entries have the same number of languages as localisations.
        /// This setup is needed for ease of working within the Unity Editor
        /// </summary>
        /// <returns></returns>
        public string InternalVerifyState()
        {
            foreach (var entry in LocalisationEntries)
            {
                if (!Localisations.ContainsKey(entry.Key)) return string.Format("Missing {0} from dictionary", entry.Key);
                if (entry.Languages.Length != Languages.Count) return string.Format("Missing languages from {0}", entry.Key);
            }
            if (LocalisationEntries.Count != Localisations.Count) return string.Format("Counts different - {0} different from {1}", LocalisationEntries.Count, Localisations.Count);
            return null;
        }

        #region Language

        /// <summary>
        /// Add a new localisation language
        /// </summary>
        /// <param name="language"></param>
        public void AddLanguage(string language)
        {
            Languages.Add(new Language(language));
        }

        /// <summary>
        /// Remove a localisation language
        /// </summary>
        /// <param name="language"></param>
        public void RemoveLanguage(string language)
        {
            for (var i = 0; i < Languages.Count; i++)
            {
                if (Languages[i].Name == language)
                {
                    Languages.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Get a localisation language
        /// </summary>
        /// <param name="language"></param>
        public bool ContainsLanguage(string language)
        {
            for (var i = 0; i < Languages.Count; i++)
            {
                if (Languages[i].Name == language)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion Language

        #region LocalisationEntries

        /// <summary>
        /// Add a new localisation entry
        /// </summary>
        /// <param name="key"></param>
        public void AddEntry(string key)
        {
            var localisationEntry = new LocalisationEntry(key);
            LocalisationEntries.Add(localisationEntry);
            Localisations.Add(key, localisationEntry);
        }

        ///// <summary>
        ///// Remove a localisation language
        ///// </summary>
        ///// <param name="language"></param>
        //public void RemoveLanguage(string language)
        //{
        //    for (var i = 0; i < Languages.Count; i++)
        //    {
        //        if (Languages[i].Name == language)
        //        {
        //            Languages.RemoveAt(i);
        //            return;
        //        }
        //    }
        //}

        /// <summary>
        /// Returns whether a localisation entry exists
        /// </summary>
        /// <param name="key"></param>
        public bool ContainsEntry(string key)
        {
            foreach (var keyEntry in Localisations.Keys)
            {
                if (keyEntry == key)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion LocalisationEntries

        // Merge(LocalistionData) - useful at runtime to create a single object
    }



        [System.Serializable]
    public class LocalisationEntry
    {
        public string Key = string.Empty;
        public string Description = string.Empty;
        public string[] Languages = new string[0];

        public LocalisationEntry(string key)
        {
            Key = key;
        }
    }

    [System.Serializable]
    public class Language
    {
        public string Name = string.Empty;

        public Language(string name)
        {
            Name = name;
        }
    }
}
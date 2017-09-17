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
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using GameFramework.Debugging;
using UnityEngine.Assertions;

namespace GameFramework.Localisation.ObjectModel
{
    /// <summary>
    /// A class for holding information and working with a collection of localisation entries.
    /// </summary>
    /// Notes: We could have used a 2d array / matrix to hold values for each language, but would still need a dictionary to reference 
    /// metadata such as the key (and index into the 2d array) pro's and con's of both, but we just put an entry straight into the dictionary for now.
    [CreateAssetMenu(fileName = "Localisation", menuName = "Game Framework/Localisation", order = 20)]
    [System.Serializable]
    public class LocalisationData : ScriptableObject , ISerializationCallbackReceiver
    {
        /// <summary>
        /// List of loaded languages. You can read from this, but should not manipulate this - use the other methods.
        /// </summary>
        public List<Language> Languages
        {
            get
            {
                return _languages;
            }
        }
        [SerializeField]
        List<Language> _languages = new List<Language>();

        /// <summary>
        /// List of loaded localisation entries. You can read from this, but should not manipulate this - use the other methods.
        /// </summary>
        public List<LocalisationEntry> Entries
        {
            get
            {
                return _entries;
            }
        }
        [SerializeField]
        List<LocalisationEntry> _entries = new List<LocalisationEntry>();

        /// <summary>
        /// Dictionary key is the Localisation key, values are LocalisationEntries. You can read from this, but should not manipulate this - use the other methods.
        /// </summary>
        public Dictionary<string, LocalisationEntry> EntriesDictionary
        {
            get
            {
                return _entriesDictionary;
            }
        }
        readonly Dictionary<string, LocalisationEntry> _entriesDictionary = new Dictionary<string, LocalisationEntry>(System.StringComparer.Ordinal);

        #region Setup

        /// <summary>
        /// As dictionaries can't be serialised we need to populate from the serialised lists.
        /// </summary>
        void PopulateDictionary()
        {
            _entriesDictionary.Clear();
            foreach (var entry in Entries)
            {
                _entriesDictionary.Add(entry.Key, entry);
            }
        }

        public void OnBeforeSerialize()
        {
            // Don't need to do anything as code should ensure both dictionaries and lists are updated
        }

        public void OnAfterDeserialize()
        {
            // As dictionaries can't be serialised we need to populate from the serialised lists.
            PopulateDictionary();
        }

        /// <summary>
        /// Internal method for verifying that the dictionary and list are synchronised and that entries have the same number of languages as localisations.
        /// This setup with both list and dictionaries is needed for ease of working within the Unity Editor
        /// </summary>
        /// <returns></returns>
        public string InternalVerifyState()
        {
            foreach (var entry in Entries)
            {
                if (!EntriesDictionary.ContainsKey(entry.Key)) return string.Format("Missing {0} from dictionary", entry.Key);
                if (entry.Languages.Length != Languages.Count) return string.Format("Missing languages from {0}", entry.Key);
            }
            if (Entries.Count != EntriesDictionary.Count) return string.Format("Counts different - {0} different from {1}", Entries.Count, EntriesDictionary.Count);
            return null;
        }

        #endregion Setup

        #region Language

        /// <summary>
        /// Add a new localisation language
        /// </summary>
        /// <param name="language"></param>
        /// <param name="code"></param>
        public Language AddLanguage(string language, string code = "")
        {
            // don't allow duplicates
            var existingLanguage = GetLanguage(language);
            if (existingLanguage != null) return existingLanguage;

            // add language
            var newLanguage = new Language(language, code);
            Languages.Add(newLanguage);

            // add language to entries
            foreach (var entry in Entries)
            {
                entry.AddLanguage();
            }
            return newLanguage;
        }

        /// <summary>
        /// Remove a localisation language
        /// </summary>
        /// <param name="language"></param>
        public void RemoveLanguage(string language)
        {
            var index = GetLanguageIndex(language);
            if (index == -1) return;

            Languages.RemoveAt(index);

            // remove language from entries
            foreach (var entry in Entries)
            {
                entry.RemoveLanguage(index);
            }
        }

        /// <summary>
        /// Gets a localisation language
        /// </summary>
        /// <param name="language"></param>
        public Language GetLanguage(string language)
        {
            return Languages.Find(x => x.Name == language);
        }

        /// <summary>
        /// Gets the index for the specified localisation language
        /// </summary>
        /// <param name="language"></param>
        public int GetLanguageIndex(string language)
        {
            for (var i = 0; i < Languages.Count; i++)
                if (Languages[i].Name == language)
                    return i;
            return -1;
        }

        /// <summary>
        /// Gets an array of the localisation language names
        /// </summary>
        public string[] GetLanguageNames()
        {
            var languageNames = new string[Languages.Count];
            for (int i = 0; i < Languages.Count; i++)
                languageNames[i] = Languages[i].Name;
            return languageNames;
        }

        /// <summary>
        /// Whether this contains the specified language
        /// </summary>
        /// <param name="language"></param>
        public bool ContainsLanguage(string language)
        {
            return GetLanguage(language) != null;
        }

        #endregion Language

        #region LocalisationEntries

        /// <summary>
        /// Add a new localisation entry
        /// </summary>
        /// <param name="key"></param>
        public LocalisationEntry AddEntry(string key)
        {
            // don't allow duplicates
            var existingEntry = GetEntry(key);
            if (existingEntry != null) return existingEntry;

            // add a default language if there isn't one already
            if (Languages.Count == 0)
                AddLanguage("English", "en");

            var entry = new LocalisationEntry(key)
            {
                Languages = new string[Languages.Count]
            };

            Entries.Add(entry);
            EntriesDictionary.Add(key, entry);

            return entry;
        }

        /// <summary>
        /// Remove a localisation entry
        /// </summary>
        /// <param name="key"></param>
        public void RemoveEntry(string key)
        {
            for (var i = 0; i < Entries.Count; i++)
            {
                if (Entries[i].Key == key)
                {
                    Entries.RemoveAt(i);
                    EntriesDictionary.Remove(key);
                    return;
                }
            }
        }

        /// <summary>
        /// Remove all localisation entries
        /// </summary>
        public void ClearEntries()
        {
            Entries.Clear();
            EntriesDictionary.Clear();
        }

        /// <summary>
        /// Gets a localisation entry
        /// </summary>
        /// <param name="key"></param>
        public LocalisationEntry GetEntry(string key)
        {
            LocalisationEntry value;
            return EntriesDictionary.TryGetValue(key, out value) ? value : null;
        }

        /// <summary>
        /// Returns whether a localisation entry exists
        /// </summary>
        /// <param name="key"></param>
        public bool ContainsEntry(string key)
        {
            return GetEntry(key) != null;
        }


        /// <summary>
        /// Gets a text translation from a LocalisationEntry
        /// </summary>
        /// <param name="key"></param>
        /// <param name="language"></param>
        public string GetText(string key, string language)
        {
            var languageIndex = GetLanguageIndex(language);
            if (languageIndex == -1)
            {
                MyDebug.LogWarningF("Localisation key {0} not found for language {1}.", key, language);
                return null;
            }
            return GetText(key, languageIndex);
        }


        /// <summary>
        /// Gets a text translation from a LocalisationEntry
        /// </summary>
        /// <param name="key"></param>
        /// <param name="languageIndex"></param>
        public string GetText(string key, int languageIndex)
        {
            Assert.IsTrue(languageIndex >= 0 && languageIndex < Languages.Count, string.Format("language index {0} is out of bounds when getting key '{1}'", languageIndex, key));
            var entry = GetEntry(key);
            if (entry == null)
            {
                MyDebug.LogWarningF("Localisation key {0} not found for language {1} (index {2}).", key, Languages[languageIndex].Name, languageIndex);
                return null;
            }
            return entry.Languages[languageIndex];
        }
        #endregion LocalisationEntries

        #region IO
        /// <summary>
        /// Merge languages and entries from a second LocalisationData into the current one overwriting existing entries with the same name.
        /// </summary>
        /// <param name="localisationData"></param>
        public void Merge(LocalisationData localisationData) {
            // TODO: TRACK THE INSTANCE ID SO WE CAN LATER UNLOAD");
            // TODO: Merge(LocalistionData, bool unloadable) - allow for a localisation to be later unloadable

            // Merge languages
            foreach (var language in localisationData.Languages)
            {
                if (!ContainsLanguage(language.Name))
                {
                    AddLanguage(language.Name, language.Code);
                }
            }
            // Merge keys
            foreach (var entry in localisationData.Entries)
            {
                if (!ContainsEntry(entry.Key))
                {
                    AddEntry(entry.Key);
                }
            }
            // Merge values for each key and language from the second file. 
            foreach (var entry in localisationData.Entries)
            {
                var masterEntry = GetEntry(entry.Key);
                foreach (var language in localisationData.Languages)
                {
                    var index = localisationData.GetLanguageIndex(language.Name);
                    var masterindex = GetLanguageIndex(language.Name);
                    masterEntry.Languages[masterindex] = entry.Languages[index];
                }
            }
        }

        /// <summary>
        /// Write localisation data out to a csv file 
        /// </summary>
        /// <param name="filename"></param>
        public bool WriteCsv(string filename)
        {
            try
            {
                var buffer = new StringBuilder();
                buffer.Append("KEY,");
                for (var i = 0; i < Languages.Count; i++)
                {
                    buffer.Append("\"");
                    buffer.Append(Languages[i].Name);
                    buffer.Append("\"");
                    buffer.Append(i == Languages.Count - 1 ? "\n" : ",");
                }

                for (var i = 0; i < Entries.Count; i++)
                {
                    buffer.Append("\"");
                    buffer.Append(Entries[i].Key.Replace("\"", "\"\""));
                    buffer.Append("\",");
                    for (var il = 0; il < Entries[i].Languages.Length; il++)
                    {
                        var text = Entries[i].Languages[il];
                        if (text.Contains("\"") || text.Contains(",") || text.Contains("\n"))
                        {
                            buffer.Append("\"");
                            buffer.Append(Entries[i].Languages[il].Replace("\"", "\"\""));
                            buffer.Append("\"");
                        }
                        else
                        {
                            buffer.Append(text);
                        }
                        buffer.Append(il == Entries[i].Languages.Length - 1 ? "\n" : ",");
                    }
                }
                System.IO.File.WriteAllText(filename, buffer.ToString(), System.Text.Encoding.UTF8);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning("An error occurred writing the csv file: " + e.Message);
            }
            return false;
        }


        #region Load CSV
        public static LocalisationData LoadCsv(string filename)
        {
            try
            {
                using (var rdr = new System.IO.StreamReader(filename, System.Text.Encoding.UTF8, true))
                {
                    return LoadCsv(rdr);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("An error occurred loading the csv file: " + e.Message);
                return null;
            }
        }

        public static LocalisationData LoadCsv(System.IO.TextReader rdr)
        {
            var localisationData = CreateInstance<LocalisationData>();
            var isHeader = true;
            foreach (IList<string> columns in FromReader(rdr))
            {
                // first row is the header
                if (isHeader)
                {
                    // sanity check
                    if (columns.Count < 2)
                    {
                        Debug.LogWarning("File should contain at least 2 columns (KEY,<Language>");
                        return null;
                    }

                    // add missing languages and get reference to language index
                    for (var column = 1; column < columns.Count; ++column)
                    {
                        var language = columns[column];
                        if (!localisationData.ContainsLanguage(language))
                        {
                            MyDebug.Log("Adding new language " + language);
                            localisationData.AddLanguage(language);
                        }
                    }
                    isHeader = false;
                }

                else
                {
                    var key = columns[0];
                    var entry = localisationData.AddEntry(key); // will return existing if found.

                    // copy in new values
                    for (int i = 1; i < columns.Count; ++i)
                    {
                        entry.Languages[i - 1] = columns[i];
                    }
                }
            }
            return localisationData;
        }

        // parses a row from a csv file and yields the result back as a list of strings - one for each column.
        static IEnumerable<IList<string>> FromReader(System.IO.TextReader csv)
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
                        if (csv.Peek() == -1 && c != '\r' && c != '\n')
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
        #endregion IO
    }


    /// <summary>
    /// Holds information about a single localisation entry including the key that identifies it and per language translations.
    /// </summary>
    [System.Serializable]
    public class LocalisationEntry
    {
        public string Key;
        public string Description = string.Empty;
        public string[] Languages = new string[0];

        public LocalisationEntry(string key)
        {
            Key = key;
        }

        /// <summary>
        /// Extend the languages array to add a new language.
        /// </summary>
        public void AddLanguage()
        {
            Array.Resize(ref Languages, Languages.Length + 1);
        }

        /// <summary>
        /// Remove a language from the languages array.
        /// </summary>
        public void RemoveLanguage(int index)
        {
            for (var i = index; i < Languages.Length - 1; i++)
            {
                Languages[i] = Languages[i + 1];
            }
            Array.Resize(ref Languages, Languages.Length - 1);
        }

    }

    /// <summary>
    /// Holds information about a localisation language including it's name and abbreviated code.
    /// </summary>
    [System.Serializable]
    public class Language
    {
        /// <summary>
        /// English name of this language
        /// </summary>
        public string Name;

        /// <summary>
        /// ISO-639-1 Code for the language
        /// </summary>
        public string Code;

        public Language(string name = "", string code = "")
        {
            Name = name;
            Code = code;
        }
    }
}
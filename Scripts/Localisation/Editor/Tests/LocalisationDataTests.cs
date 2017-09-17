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

#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
#else
using GameFramework.Localisation.ObjectModel;
using NUnit.Framework;
using UnityEngine;

namespace GameFramework.Localisation
{
    /// <summary>
    /// Test cases for localisation. You can also view these to see how you might use the API.
    /// </summary>
    public class LocalisationDataTests
    {
        #region Helper Functions

        /// <summary>
        /// Create an empty LocalisationData
        /// </summary>
        internal static LocalisationData CreateLocalisationData()
        {
            return ScriptableObject.CreateInstance<LocalisationData>();
        }

        /// <summary>
        /// Create and fill a LocalisationData with the specified languages and keys and values in the format key-language
        /// </summary>
        /// <param name="languages"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        internal static LocalisationData CreateLocalisationData(string[] languages, string[] keys)
        {
            var localisationData = ScriptableObject.CreateInstance<LocalisationData>();
            FillLocalisationData(localisationData, languages, keys);
            return localisationData;
        }

        /// <summary>
        /// Fills a LocalisationData with the specified languages and keys and values in the format key-language
        /// </summary>
        /// <param name="localisationData"></param>
        /// <param name="languages"></param>
        /// <param name="keys"></param>
        internal static void FillLocalisationData(LocalisationData localisationData, string[] languages, string[] keys)
        {
            foreach (var language in languages)
                localisationData.AddLanguage(language);

            foreach (var key in keys)
            {
                var entry = localisationData.AddEntry(key);
                for (var i = 0; i < languages.Length; i++)
                    entry.Languages[i] = key + "-" + languages[i];
            }
        }
        #endregion Helper Functions

        #region Setup Tests

        [Test]
        public void CreateLocalisation()
        {
            //// Act
            var localisationData = CreateLocalisationData();

            //// Assert
            Assert.IsNotNull(localisationData, "No LocalisationData was created");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }

        [TestCase(new[] { "English" }, new[] { "Key1", "Key2" })]
        [TestCase(new[] { "English", "French" }, new[] { "Key1", "Key2", "Key3" })]
        [TestCase(new[] { "English", "French", "Spanish" }, new[] { "Key1", "Key2", "Key3" })]
        public void Instantiate(string[] languages, string[] keys)
        {
            // Arrange
            var localisationData = CreateLocalisationData(languages, keys);

            //// Act
            var localisationDataCopy = ScriptableObject.Instantiate(localisationData);

            //// Assert
            Assert.IsNull(localisationDataCopy.InternalVerifyState(), localisationDataCopy.InternalVerifyState());
        }

        #endregion Setup Tests

        #region Language Tests

        [TestCase("English")]
        [TestCase("French")]
        public void AddLanguage(string language)
        {
            // Arrange
            var localisationData = CreateLocalisationData();

            //// Act
            localisationData.AddLanguage(language);

            //// Assert
            Assert.IsTrue(localisationData.ContainsLanguage(language), "Language not added");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }

        [TestCase("English", "en")]
        [TestCase("French", "fr")]
        public void AddLanguage(string language, string code)
        {
            // Arrange
            var localisationData = CreateLocalisationData();

            //// Act
            localisationData.AddLanguage(language, code);

            //// Assert
            Assert.IsTrue(localisationData.ContainsLanguage(language), "Language not added");
            Assert.AreEqual(code, localisationData.GetLanguage(language).Code, "Language not added");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }

        [TestCase("English", "French", "Spanish")]
        [TestCase("French", "German", "Spanish")]
        public void AddLanguageMultiple(string language, string language2, string language3)
        {
            // Arrange
            var localisationData = CreateLocalisationData();

            //// Act
            localisationData.AddLanguage(language);
            localisationData.AddLanguage(language2);
            localisationData.AddLanguage(language3);

            //// Assert
            Assert.IsTrue(localisationData.ContainsLanguage(language), "Language not added");
            Assert.IsTrue(localisationData.ContainsLanguage(language2), "Language2 not added");
            Assert.IsTrue(localisationData.ContainsLanguage(language3), "Language3 not added");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }

        [TestCase("English")]
        [TestCase("French")]
        public void AddlanguageNoDuplicates(string language)
        {
            // Arrange
            var localisationData = CreateLocalisationData();

            //// Act
            var entry1 = localisationData.AddLanguage(language);
            var entry2 = localisationData.AddLanguage(language);

            //// Assert
            Assert.AreSame(entry1, entry2,
                "Adding a duplicate should return the existing object rather than a new one.");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }


        [TestCase(new[] { "English" }, new[] { "Key1", "Key2" }, "Spanish")]
        [TestCase(new[] { "English", "French" }, new[] { "Key1", "Key2", "Key3" }, "Spanish")]
        public void AddLanguageAdjustsEntries(string[] languages, string[] keys, string newLanguage)
        {
            // Arrange
            var localisationData = CreateLocalisationData();

            foreach (var language in languages)
                localisationData.AddLanguage(language);

            foreach (var key in keys)
            {
                var entry = localisationData.AddEntry(key);
                for (var i = 0; i < languages.Length; i++)
                    entry.Languages[i] = key + "-" + languages[i];
            }

            //// Act
            localisationData.AddLanguage(newLanguage);

            //// Assert
            foreach (var key in keys)
            {
                var entry = localisationData.GetEntry(key);
                for (var i = 0; i < languages.Length; i++)
                    Assert.AreEqual(entry.Languages[i], key + "-" + languages[i], "Key is corrupted");
            }
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }

        [TestCase("English")]
        [TestCase("French")]
        public void GetLanguage(string language)
        {
            // Arrange
            var localisationData = CreateLocalisationData();
            localisationData.AddLanguage(language);

            //// Act
            var entry = localisationData.GetLanguage(language);

            //// Assert
            Assert.AreEqual(language, entry.Name, "Language was not found");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }


        [TestCase(new[] { "English" }, "French")]
        [TestCase(new[] { "English", "French" }, "English2")]
        [TestCase(new[] { "English", "French", "Spanish" }, "German")]
        public void GetLanguageNotFound(string[] languages, string checkLanguage)
        {
            //// Arrange
            var localisationData = CreateLocalisationData();
            foreach (var language in languages)
                localisationData.AddLanguage(language);

            //// Act
            var isLanguageFound = localisationData.ContainsLanguage(checkLanguage);

            //// Assert
            Assert.IsFalse(isLanguageFound, "Language " + checkLanguage + " was found but should not be present");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }


        [TestCase("English")]
        [TestCase("English", "French")]
        [TestCase("English", "French", "Spanish")]
        public void GetLanguages(params string[] languages)
        {
            //// Arrange
            var localisationData = CreateLocalisationData();
            foreach (var language in languages)
                localisationData.AddLanguage(language);

            //// Act
            var returnedLanguages = localisationData.Languages;

            //// Assert
            Assert.AreEqual(languages.Length, returnedLanguages.Count, "The number of languages is different");
            foreach (var language in languages)
                Assert.IsTrue(localisationData.ContainsLanguage(language), "Language " + language + " was not found at the correct index");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }


        [TestCase("English")]
        [TestCase("English", "French")]
        [TestCase("English", "French", "Spanish")]
        public void GetLanguageNames(params string[] languages)
        {
            //// Arrange
            var localisationData = CreateLocalisationData();
            foreach (var language in languages)
                localisationData.AddLanguage(language);

            //// Act
            var returnedLanguages = localisationData.GetLanguageNames();

            //// Assert
            Assert.AreEqual(languages.Length, returnedLanguages.Length, "The number of languages is different");
            for (var i = 0; i < languages.Length; i++)
                Assert.AreEqual(languages[i], returnedLanguages[i], "Language " + languages[i] + " was not found");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }


        [TestCase("English")]
        [TestCase("English", "French")]
        [TestCase("English", "French", "Spanish")]
        public void GetLanguageIndex(params string[] languages)
        {
            //// Arrange
            var localisationData = CreateLocalisationData();
            foreach (var language in languages)
                localisationData.AddLanguage(language);

            //// Assert
            for (int i = 0; i < languages.Length; i++)
                Assert.AreEqual(i, localisationData.GetLanguageIndex(languages[i]), "Language was not found at the correct index");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }

        [TestCase("English")]
        [TestCase("French")]
        public void RemoveLanguage(string language)
        {
            // Arrange
            var localisationData = CreateLocalisationData();
            localisationData.AddLanguage(language);

            //// Act
            localisationData.RemoveLanguage(language);

            //// Assert
            Assert.IsFalse(localisationData.ContainsLanguage(language), "Language was not removed");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }

        [TestCase("English", "English", "French", "Spanish")]
        [TestCase("German", "French", "German", "Spanish")]
        [TestCase("German", "French", "German", "English")]
        public void RemoveLanguageWhenMultiple(string languageToRemove, string language, string language2, string language3)
        {
            // Arrange
            var localisationData = CreateLocalisationData();
            localisationData.AddLanguage(language);
            localisationData.AddLanguage(language2);
            localisationData.AddLanguage(language3);

            //// Act
            localisationData.RemoveLanguage(languageToRemove);

            //// Assert
            Assert.IsFalse(localisationData.ContainsLanguage(languageToRemove), "Language was not removed");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }


        [TestCase(new[] { "English" }, new[] { "Key1", "Key2" }, "English")]
        [TestCase(new[] { "English2", "French" }, new[] { "Key1", "Key2", "Key3" }, "English2")]
        [TestCase(new[] { "English", "French" }, new[] { "Key1", "Key2", "Key3" }, "French")]
        [TestCase(new[] { "English3", "French", "Spanish" }, new[] { "Key1", "Key2", "Key3" }, "English3")]
        [TestCase(new[] { "English", "French3", "Spanish" }, new[] { "Key1", "Key2", "Key3" }, "French3")]
        [TestCase(new[] { "English", "French", "Spanish" }, new[] { "Key1", "Key2", "Key3" }, "Spanish")]
        public void RemoveLanguageAdjustsEntries(string[] languages, string[] keys, string removeLanguage)
        {
            // Arrange
            var localisationData = CreateLocalisationData(languages, keys);

            //// Act
            localisationData.RemoveLanguage(removeLanguage);

            //// Assert
            foreach (var key in keys)
            {
                var entry = localisationData.GetEntry(key);
                for (int i = 0, newIndex = 0; i < languages.Length - 1; i++, newIndex++)
                {
                    if (languages[i] == removeLanguage) newIndex++; // skip one
                    Assert.AreEqual(entry.Languages[i], key + "-" + languages[newIndex],
                        "Key is corrupted when removing " + removeLanguage);
                }
            }
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }

        [TestCase("English")]
        [TestCase("German")]
        public void ContainsLanguage(string language)
        {
            // Arrange
            var localisationData = CreateLocalisationData();
            localisationData.AddLanguage(language);

            //// Act
            var containslanguage = localisationData.ContainsLanguage(language);

            //// Assert
            Assert.IsTrue(containslanguage, "Language Was Not Found");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }

        [TestCase("English", "German")]
        [TestCase("German", "French")]
        public void ContainsLanguageNotFound(string language, string languageToAdd)
        {
            // Arrange
            var localisationData = CreateLocalisationData();
            localisationData.AddLanguage(languageToAdd);

            //// Act
            var containslanguage = localisationData.ContainsLanguage(language);

            //// Assert
            Assert.IsFalse(containslanguage, "Missing language was returned as found");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }

        #endregion Language Tests

        #region LocalisationEntry Tests

        [TestCase("Key1")]
        [TestCase("Key2")]
        public void AddLocalisationEntry(string key)
        {
            // Arrange
            var localisationData = CreateLocalisationData();

            //// Act
            localisationData.AddEntry(key);

            //// Assert
            Assert.IsTrue(localisationData.ContainsEntry(key), "LocalisationEntry not added");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }

        [TestCase("Key1", "Key2", "Key3")]
        [TestCase("Key4", "Key5", "Key6")]
        public void AddLocalisationEntryMultiple(string key, string key2, string key3)
        {
            // Arrange
            var localisationData = CreateLocalisationData();

            //// Act
            localisationData.AddEntry(key);
            localisationData.AddEntry(key2);
            localisationData.AddEntry(key3);

            //// Assert
            Assert.IsTrue(localisationData.ContainsEntry(key), "LocalisationEntry not added");
            Assert.IsTrue(localisationData.ContainsEntry(key2), "LocalisationEntry not added");
            Assert.IsTrue(localisationData.ContainsEntry(key3), "LocalisationEntry not added");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }

        [TestCase("Key1")]
        [TestCase("Key2")]
        public void AddLocalisationEntryNoLanguagesCreatesDefault(string key)
        {
            // Arrange
            var localisationData = CreateLocalisationData();

            //// Act
            localisationData.AddEntry(key);

            //// Assert
            Assert.IsTrue(localisationData.ContainsLanguage("English"), "Language not created");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }

        [TestCase("Key1")]
        [TestCase("Key2")]
        public void AddLocalisationEntryNoDuplicates(string key)
        {
            // Arrange
            var localisationData = CreateLocalisationData();

            //// Act
            var entry1 = localisationData.AddEntry(key);
            var entry2 = localisationData.AddEntry(key);

            //// Assert
            Assert.AreSame(entry1, entry2, "Adding a duplicate should return the existing object rather than a new one.");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }


        [TestCase("Key1")]
        [TestCase("Key2")]
        public void GetLocalisationEntry(string key)
        {
            // Arrange
            var localisationData = CreateLocalisationData();
            localisationData.AddEntry(key);

            //// Act
            var entry = localisationData.GetEntry(key);

            //// Assert
            Assert.AreEqual(key, entry.Key, "LocalisationEntry was not found");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }

        [TestCase("Key1")]
        [TestCase("Key2")]
        public void RemoveLocalisationEntry(string key)
        {
            // Arrange
            var localisationData = CreateLocalisationData();
            localisationData.AddEntry(key);

            //// Act
            localisationData.RemoveEntry(key);

            //// Assert
            Assert.IsFalse(localisationData.ContainsEntry(key), "LocalisationEntry was not removed");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }

        [TestCase("English", "English", "French", "Spanish")]
        [TestCase("German", "French", "German", "Spanish")]
        [TestCase("German", "French", "German", "English")]
        public void RemoveLocalisationEntryWhenMultiple(string languageToRemove, string language, string language2, string language3)
        {
            // Arrange
            var localisationData = CreateLocalisationData();
            localisationData.AddLanguage(language);
            localisationData.AddLanguage(language2);
            localisationData.AddLanguage(language3);

            //// Act
            localisationData.RemoveLanguage(languageToRemove);

            //// Assert
            Assert.IsFalse(localisationData.ContainsLanguage(languageToRemove), "Language was not removed");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }

        [TestCase(new[] { "Key1" }, "")]
        [TestCase(new[] { "Key1", "Key2" }, "")]
        [TestCase(new[] { "Key1", "Key2", "Key3" }, "")]
        public void ClearLocalisationEntries(string[] keys, string dummy)
        {
            // Arrange
            var localisationData = CreateLocalisationData();
            foreach(var key in keys)
                localisationData.AddEntry(key);

            //// Act
            localisationData.ClearEntries();

            //// Assert
            foreach (var key in keys)
                Assert.IsFalse(localisationData.ContainsEntry(key), "LocalisationEntry was not removed");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }

        [TestCase("Key1")]
        [TestCase("Key2")]
        public void ContainsLocalisationEntry(string key)
        {
            // Arrange
            var localisationData = CreateLocalisationData();
            localisationData.AddEntry(key);

            //// Act
            var containsEntry = localisationData.ContainsEntry(key);

            //// Assert
            Assert.IsTrue(containsEntry, "LocalisationEntry Was Not Found");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }

        [TestCase("Key1", "Key2")]
        [TestCase("Key2", "Key1")]
        public void ContainsLocalisationEntryNotFound(string key, string keyToAdd)
        {
            // Arrange
            var localisationData = CreateLocalisationData();
            localisationData.AddEntry(keyToAdd);

            //// Act
            var containsEntry = localisationData.ContainsEntry(key);

            //// Assert
            Assert.IsFalse(containsEntry, "Missing LocalisationEntry was returned as found");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }

        [TestCase(new[] { "English" }, new[] { "Key1", "Key2" }, "Key1", "English")]
        [TestCase(new[] { "English2", "French" }, new[] { "Key1", "Key2", "Key3" }, "Key1", "English2")]
        [TestCase(new[] { "English", "French" }, new[] { "Key1", "Key2", "Key3" }, "Key1", "French")]
        [TestCase(new[] { "English3", "French", "Spanish" }, new[] { "Key1", "Key2", "Key3" }, "Key1", "English3")]
        [TestCase(new[] { "English", "French3", "Spanish" }, new[] { "Key1", "Key2", "Key3" }, "Key2", "French3")]
        [TestCase(new[] { "English", "French", "Spanish" }, new[] { "Key1", "Key2", "Key3" }, "Key3", "Spanish")]
        public void GetText(string[] languages, string[] keys, string getKey, string getLanguage)
        {
            // Arrange
            var localisationData = CreateLocalisationData(languages, keys);

            //// Act
            var text = localisationData.GetText(getKey, getLanguage);

            //// Assert
            Assert.AreEqual(getKey + "-" + getLanguage, text, "Got the wrong value");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }

        [TestCase(new[] { "English" }, new[] { "Key1", "Key2" }, "Key3", "English")]
        [TestCase(new[] { "English2", "French" }, new[] { "Key1", "Key2", "Key3" }, "Key4", "English2")]
        public void GetTextKeyNotFound(string[] languages, string[] keys, string getKey, string getLanguage)
        {
            // Arrange
            var localisationData = CreateLocalisationData(languages, keys);

            //// Act
            var text = localisationData.GetText(getKey, getLanguage);

            //// Assert
            Assert.IsNull(text, "Should not have found a value");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }

        [TestCase(new[] { "English" }, new[] { "Key1", "Key2" }, "Key1", "French")]
        [TestCase(new[] { "English2", "French" }, new[] { "Key1", "Key2", "Key3" }, "Key2", "Spanish")]
        public void GetTextLanguageNotFound(string[] languages, string[] keys, string getKey, string getLanguage)
        {
            // Arrange
            var localisationData = CreateLocalisationData(languages, keys);

            //// Act
            var text = localisationData.GetText(getKey, getLanguage);

            //// Assert
            Assert.IsNull(text, "Should not have found a value");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }

        [TestCase(new[] { "English" }, new[] { "Key1", "Key2" }, "Key1", 0)]
        [TestCase(new[] { "English2", "French" }, new[] { "Key1", "Key2", "Key3" }, "Key1", 1)]
        [TestCase(new[] { "English", "French" }, new[] { "Key1", "Key2", "Key3" }, "Key1", 1)]
        [TestCase(new[] { "English3", "French", "Spanish" }, new[] { "Key1", "Key2", "Key3" }, "Key1", 0)]
        [TestCase(new[] { "English", "French3", "Spanish" }, new[] { "Key1", "Key2", "Key3" }, "Key2", 1)]
        [TestCase(new[] { "English", "French", "Spanish" }, new[] { "Key1", "Key2", "Key3" }, "Key3", 2)]
        public void GetTextByIndex(string[] languages, string[] keys, string getKey, int language)
        {
            // Arrange
            var localisationData = CreateLocalisationData(languages, keys);

            //// Act
            var text = localisationData.GetText(getKey, language);

            //// Assert
            Assert.AreEqual(getKey + "-" + localisationData.Languages[language].Name, text, "Got the wrong value");
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }

        #endregion LocalisationEntry Tests		

        #region IO


        public static System.Collections.Generic.IEnumerable<TestCaseData> LoadCsvTests()
        {
            // Last row doesn't end in new line
            yield return new TestCaseData(
                "Key,English\nKey1,Value1\nKey2,Value2", new[] { "English" }, new[] { "Key1", "Key2" },
                new[,] { { "Value1" }, { "Value2" } }
                );
            // Last row ends in new line
            yield return new TestCaseData(
                "Key,English\nKey1,Value1\nKey2,Value2\n", new[] { "English" }, new[] { "Key1", "Key2" },
                new[,] { { "Value1" }, { "Value2" } }
                );
            // Quoted value
            yield return new TestCaseData(
                "Key,English\nKey1,\"Value1\"\nKey2,Value2", new[] { "English" }, new[] { "Key1", "Key2" },
                new[,] { { "Value1" }, { "Value2" } }
                );
            // Quoted value with escaped quote
            yield return new TestCaseData(
                "Key,English\nKey1,\"Value\"\"1\"\nKey2,Value2", new[] { "English" }, new[] { "Key1", "Key2" },
                new[,] { { "Value\"1" }, { "Value2" } }
                );
            // Multiple languages value
            yield return new TestCaseData(
                "Key,English,Spanish\nKey1,Value1,Value2\nKey2,Value3,Value4", new[] { "English", "Spanish" }, new[] { "Key1", "Key2" },
                new[,] { { "Value1", "Value2" }, { "Value3", "Value4" } }
                );
        }

        [Test]
        [TestCaseSource("LoadCsvTests")]
        public void LoadCsv(string csvData, string[] languages, string[] keys, string[,] values)
        {
            // Arrange
            using (var rdr = new System.IO.StringReader(csvData))
            {

                //// Act
                var localisationData = LocalisationData.LoadCsv(rdr);

                //// Assert
                Assert.AreEqual(languages.Length, localisationData.Languages.Count, "Languages not setup as expected");
                Assert.AreEqual(keys.Length, localisationData.Entries.Count, "Entries not setup as expected");
                foreach(var language in languages)
                {
                    Assert.IsTrue(localisationData.GetLanguage(language) != null, "Missing language " + language);
                }
                for (var i = 0; i < keys.Length; i++)
                {
                    var key = keys[i];
                    Assert.IsTrue(localisationData.GetEntry(key) != null, "Missing entry " + key);
                    for (var l = 0; l < languages.Length; l++)
                    {
                        var language = languages[l];
                        Assert.AreEqual(values[i,l], localisationData.GetText(key, language), "Values are not the same.");
                    }
                }
            }
        }


        [TestCase(new[] { "English" }, new[] { "English" })]
        [TestCase(new[] { "English" }, new[] { "French" })]
        [TestCase(new[] { "English" }, new[] { "English", "French" })]
        public void MergeLanguage(string[] languages, string[] languages2)
        {
            // Arrange
            var keys = new[] { "Key1", "Key2" };
            var localisationData = CreateLocalisationData(languages, keys);
            var localisationData2 = CreateLocalisationData(languages2, keys);

            //// Act
            localisationData.Merge(localisationData2);

            //// Assert
            foreach (var language in languages)
            {
                Assert.IsNotNull(localisationData.GetLanguage(language), "Missing language " + language + " from the initial localisation file");
            }
            foreach (var language in languages2)
            {
                Assert.IsNotNull(localisationData.GetLanguage(language), "Missing language " + language + " from the localisation file being merged in.");
            }
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }

        [TestCase(new[] { "Key1", "Key2" }, new[] { "Key1", "Key2" })]
        [TestCase(new[] { "Key1", "Key2" }, new[] { "Key3", "Key4" })]
        [TestCase(new[] { "Key1", "Key2" }, new[] { "Key2", "Key3" })]
        public void MergeKeys(string[] keys, string[] keys2)
        {
            // Arrange
            var languages = new[] { "English" };
            var localisationData = CreateLocalisationData(languages, keys);
            var localisationData2 = CreateLocalisationData(languages, keys2);

            //// Act
            localisationData.Merge(localisationData2);

            //// Assert
            foreach (var key in keys)
            {
                Assert.IsNotNull(localisationData.GetEntry(key), "Missing key " + key + " from the initial localisation file.");
            }
            foreach (var key in keys2)
            {
                Assert.IsNotNull(localisationData.GetEntry(key), "Missing key " + key + " from the localisation file being merged in.");
            }
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }

        [TestCase(new[] { "English" }, new[] { "Key1", "Key2" }, new[] { "English" }, new[] { "Key1", "Key2" })]
        [TestCase(new[] { "English" }, new[] { "Key1", "Key2" }, new[] { "English", "French" }, new[] { "Key1", "Key2" })]
        [TestCase(new[] { "English" }, new[] { "Key1", "Key2" }, new[] { "English" }, new[] { "Key3", "Key4" })]
        [TestCase(new[] { "English" }, new[] { "Key1", "Key2" }, new[] { "English", "French" }, new[] { "Key3", "Key4" })]
        public void MergeValues(string[] languages, string[] keys, string[] languages2, string[] keys2)
        {
            // Arrange
            var localisationData = CreateLocalisationData(languages, keys);
            var localisationDataOriginal = CreateLocalisationData(languages, keys);
            var localisationData2 = CreateLocalisationData(languages2, keys2);
            // adjust auto keys in second localisation data file so we can identify them after merging.
            foreach (var entry in localisationData2.Entries)
            {
                for (int i = 0; i < entry.Languages.Length; i++)
                {
                    entry.Languages[i] = "2-" + entry.Languages[i];
                }
            }

            //// Act
            localisationData.Merge(localisationData2);

            //// Assert
            foreach (var entry in localisationData.Entries)
            {
                foreach (var language in localisationData.Languages)
                {
                    if (localisationData2.ContainsEntry(entry.Key) && localisationData2.ContainsLanguage(language.Name))
                        Assert.AreEqual("2-" + entry.Key + "-" + language.Name, localisationData.GetText(entry.Key, language.Name), "Value is not changed " + entry.Key + " - " + language.Name);
                    else if (!localisationData2.ContainsEntry(entry.Key) && !localisationDataOriginal.ContainsLanguage(language.Name))
                        Assert.IsNull(localisationData.GetText(entry.Key, language.Name), "Value should be null " + entry.Key + " - " + language.Name);
                    else
                        Assert.AreEqual(entry.Key + "-" + language.Name, localisationData.GetText(entry.Key, language.Name), "Value is incorrectly changed " + entry.Key + " - " + language.Name);
                }
            }
            Assert.IsNull(localisationData.InternalVerifyState(), localisationData.InternalVerifyState());
        }
        #endregion IO

        [Test]
        public void PlaceHolder()
        {
            // Arrange
            //var messenger = new Messenger();
            //_testHandlerCalled = false;
            //messenger.AddListener<BaseMessage>(TestHandler);

            //// Act
            //messenger.TriggerMessage(new BaseMessage());

            //// Assert
            //Assert.IsTrue(_testHandlerCalled, "The test handler was not called!");

            //// Cleanup
            //messenger.RemoveListener<BaseMessage>(TestHandler);
        }

    }
}
#endif
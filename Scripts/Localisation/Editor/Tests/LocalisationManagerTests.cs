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
using NUnit.Framework;
using UnityEngine;
using GameFramework.Localisation.Components;
using GameFramework.Localisation.ObjectModel;

namespace GameFramework.Localisation
{
    /// <summary>
    /// Test cases for localisation manager. You can also view these to see how you might use the API.
    /// </summary>
    public class LocalisationManagerTests
    {
        #region Helper Functions

        LocalisationManager CreateLocalisationManagerAuto(string[] supportedLanguages)
        {
            return CreateLocalisationManager(LocalisationManager.SetupModeType.Auto, new LocalisationData[0], supportedLanguages);
        }

        LocalisationManager CreateLocalisationManagerSpecified(LocalisationData[] specifiedLocalisationData, string[] supportedLanguages)
        {
            return CreateLocalisationManager(LocalisationManager.SetupModeType.Specified, specifiedLocalisationData, supportedLanguages);
        }

        LocalisationManager CreateLocalisationManager(LocalisationManager.SetupModeType setupMode, LocalisationData[] specifiedLocalisationData, string[] supportedLanguages)
        {
            // Create and then disable so we can set component properties before Awake
            var localisationManagerGameObject = new GameObject("LocalisationManager");
            var localisationManager = localisationManagerGameObject.AddComponent<LocalisationManager>();
            localisationManager.SetupMode = setupMode;
            localisationManager.SpecifiedLocalisationData = specifiedLocalisationData;
            localisationManager.SupportedLanguages = supportedLanguages;
            localisationManager.LoadLocalisationData();
            return localisationManager;
        }

        void DestroyLocalisationManager(LocalisationManager localisationManager)
        {
            UnityEngine.Object.DestroyImmediate(localisationManager);
        }

        LocalisationData CreateLocalisationData(string[] languages, string[]keys)
        {
            var localisationData = ScriptableObject.CreateInstance<LocalisationData>();
            foreach (var language in languages)
                localisationData.AddLanguage(language);
            foreach (var key in keys)
            {
                var entry = localisationData.AddEntry(key);
                for (var i = 0; i < languages.Length; i++)
                    entry.Languages[i] = key + "-" + languages[i];
            }
            return localisationData;
        }
        #endregion Helper Functions

        #region Setup Tests

        [Test]
        public void CreateLocalisationSetup()
        {
            //// Act
            var localisationManager = CreateLocalisationManagerAuto(new string[0]);

            //// Assert
            Assert.IsNotNull(localisationManager, "LocalisationManager was not created");

            //// Cleanup
            DestroyLocalisationManager(localisationManager);
        }

        [TestCase(new[] { "English" }, new[] { "Key1", "Key2" })]
        [TestCase(new[] { "English", "French" }, new[] { "Key1", "Key2", "Key3" })]
        [TestCase(new[] { "English", "French" }, new[] { "Key1", "Key2", "Key3" })]
        public void CreateLocalisationSpecifiedSingle(string[] languages, string[] keys)
        {
            // Arrange
            var localisationData = CreateLocalisationData(languages, keys);

            //// Act
            var localisationManager = CreateLocalisationManagerSpecified(new LocalisationData[] { localisationData }, new string[0]);

            //// Assert
            Assert.IsNotNull(localisationManager.LocalisationData, "LocalisationData is not set!");
            Assert.AreEqual(keys.Length, localisationManager.LocalisationData.LocalisationEntries.Count, "The number of localisation entries differs!");
            Assert.AreEqual(languages.Length, localisationManager.LocalisationData.Languages.Count, "The number of languages differs!");

            //// Cleanup
            DestroyLocalisationManager(localisationManager);
        }

        [TestCase(new[] { "English" }, new[] { "Key1", "Key2" })]
        [TestCase(new[] { "English", "French" }, new[] { "Key1", "Key2", "Key3" })]
        [TestCase(new[] { "English", "French" }, new[] { "Key1", "Key2", "Key3" })]
        public void CreateLocalisationSpecifiedMultiple(string[] languages, string[] keys)
        {
            // Arrange
            var localisationData = CreateLocalisationData(languages, keys);

            //// Act
            var localisationManager = CreateLocalisationManagerSpecified(new LocalisationData[] { localisationData }, new string[0]);

            //// Assert
            Assert.IsNotNull(localisationManager.LocalisationData, "LocalisationData is not set!");
            Assert.AreEqual(keys.Length, localisationManager.LocalisationData.LocalisationEntries.Count, "The number of localisation entries differs!");
            Assert.AreEqual(languages.Length, localisationManager.LocalisationData.Languages.Count, "The number of languages differs!");

            //// Cleanup
            DestroyLocalisationManager(localisationManager);
        }
        #endregion Setup Tests

        #region Language Tests
        [TestCase(new[] { "English" }, new[] { "Key1", "Key2" }, new[] { "English" }, "English")]
        [TestCase(new[] { "English", "French" }, new[] { "Key1", "Key2", "Key3" }, new[] { "English", "French" }, "English")]
        [TestCase(new[] { "English", "French" }, new[] { "Key1", "Key2", "Key3" }, new[] { "English", "French" }, "French")]
        public void CanUseLanguage(string[] languages, string[] keys, string[] supportedLanguages, string language)
        {
            // Arrange
            var localisationData = CreateLocalisationData(languages, keys);
            var localisationManager = CreateLocalisationManagerSpecified( new LocalisationData[] { localisationData }, supportedLanguages);

            //// Act
            var canUseLanguage = localisationManager.CanUseLanguage(language);

            //// Assert
            Assert.IsTrue(canUseLanguage, "The language can not be used!");

            //// Cleanup
            DestroyLocalisationManager(localisationManager);
        }

        [TestCase(new[] { "English" }, new[] { "Key1", "Key2" }, new[] { "English" }, "Spanish")]
        [TestCase(new[] { "English", "French" }, new[] { "Key1", "Key2", "Key3" }, new[] { "English", "French" }, "Spanish")]
        [TestCase(new[] { "English", "French" }, new[] { "Key1", "Key2", "Key3" }, new string[0], "Spanish")]
        public void CanUseLanguageNotValid(string[] languages, string[] keys, string[] supportedLanguages, string language)
        {
            // Arrange
            var localisationData = CreateLocalisationData(languages, keys);
            var localisationManager = CreateLocalisationManagerSpecified(new LocalisationData[] { localisationData }, supportedLanguages);

            //// Act
            var canUseLanguage = localisationManager.CanUseLanguage(language);

            //// Assert
            Assert.IsFalse(canUseLanguage, "The language can not be used!");

            //// Cleanup
            DestroyLocalisationManager(localisationManager);
        }
        #endregion Language Tests

        #region Get

        [TestCase(new[] { "English" }, new[] { "Key1", "Key2" }, new[] { "English" }, "Spanish")]
        [TestCase(new[] { "English", "French" }, new[] { "Key1", "Key2", "Key3" }, new[] { "English", "French" }, "Spanish")]
        [TestCase(new[] { "English", "French" }, new[] { "Key1", "Key2", "Key3" }, new string[0], "Spanish")]
        public void GetText(string[] languages, string[] keys, string[] supportedLanguages, string language)
        {
            Assert.IsTrue(false, "TO IMPLEMENT");
            //// Arrange
            //var localisationData = CreateLocalisationData(languages, keys);
            //var localisationManager = CreateLocalisationManagerSpecified(new LocalisationData[] { localisationData }, supportedLanguages);

            ////// Act
            //var canUseLanguage = localisationManager.CanUseLanguage(language);

            ////// Assert
            //Assert.IsFalse(canUseLanguage, "The language can not be used!");

            ////// Cleanup
            //DestroyLocalisationManager(localisationManager);
        }
        #endregion Get

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
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
using GameFramework.GameStructure.GameItems.ObjectModel;
using NUnit.Framework;
using UnityEngine;
using GameFramework.GameStructure.Game.ObjectModel;

namespace GameFramework.GameStructure.GameItems
{
    /// <summary>
    /// Test cases for Counters. You can also view these to see how you might use the API.
    /// </summary>
    public class CounterTests
    {
        #region Helper
        public class CounterCallbackTestClass : ICounterChangedCallback
        {
            public int IntAmountChangedCounter, IntAmountBestChangedCounter, FloatAmountChangedCounter, FloatAmountBestChangedCounter;
            public int NewIntAmount, OldIntAmount;
            public int NewIntAmountBest, OldIntAmountBest;
            public float NewFloatAmount, OldFloatAmount;
            public float NewFloatAmountBest, OldFloatAmountBest;

            public void CounterIntAmountChanged(Counter counter, int oldAmount)
            {
                IntAmountChangedCounter++;
                NewIntAmount = counter.IntAmount;
                OldIntAmount = oldAmount;
            }

            public void CounterIntAmountBestChanged(Counter counter, int oldAmount)
            {
                IntAmountBestChangedCounter++;
                NewIntAmountBest = counter.IntAmountBest;
                OldIntAmountBest = oldAmount;
            }

            public void CounterFloatAmountChanged(Counter counter, float oldAmount)
            {
                FloatAmountChangedCounter++;
                NewFloatAmount = counter.FloatAmount;
                OldFloatAmount = oldAmount;
            }

            public void CounterFloatAmountBestChanged(Counter counter, float oldAmount)
            {
                FloatAmountBestChangedCounter++;
                NewFloatAmountBest = counter.FloatAmountBest;
                OldFloatAmountBest = oldAmount;
            }
        }
        #endregion Helper

        #region Initialisation

        /// <summary>
        /// Seperate test from creating, saving and then loading a GameItem to verify the consistency of saved preferences
        /// across different versions of the framework (that we use the same preferences keys).
        /// </summary>
        /// expectedAmount should be set based upon persistCHanges parameter
        [Test]
        public void CounterInitialisationDefault()
        {
            //// Arrange
            PlayerPrefs.DeleteAll();

            //// Act
            var counterConfiguration = new CounterConfiguration()
            {
                SaveBest = CounterConfiguration.SaveType.None
            };
            var counter = new Counter(counterConfiguration);

            //// Assert
            Assert.AreEqual(counterConfiguration, counter.Configuration, "Counter configuration not initialised correctly");
            Assert.AreEqual(-1, counter.Identifier, "Identifier configuration not initialised correctly");
            Assert.AreEqual(0, counter.IntAmount, "IntAmount not initialised correctly");
            Assert.AreEqual(0, counter.IntAmountSaved, "IntAmountLastSaved not initialised correctly");
            Assert.AreEqual(0, counter.IntAmountBest, "IntAmountBest not initialised correctly");
            Assert.AreEqual(0, counter.IntAmountBestSaved, "IntAmountBestLastSaved not initialised correctly");
            Assert.AreEqual(0, counter.FloatAmount, "FloatAmount not initialised correctly");
            Assert.AreEqual(0, counter.FloatAmountSaved, "FloatAmountLastSaved not initialised correctly");
            Assert.AreEqual(0, counter.FloatAmountBest, "FloatAmountBest not initialised correctly");
            Assert.AreEqual(0, counter.FloatAmountBestSaved, "FloatAmountBestLastSaved not initialised correctly");
            Assert.AreEqual(CounterConfiguration.CounterTypeEnum.Int, counter.Configuration.CounterType, "Configuration.CounterType not initialised correctly");
            Assert.AreEqual(0, counter.Configuration.IntMinimum, "Configuration.IntMinimum not initialised correctly");
            Assert.AreEqual(int.MaxValue, counter.Configuration.IntMaximum, "Configuration.IntMaximum not initialised correctly");
            Assert.AreEqual(0, counter.Configuration.FloatMinimum, "Configuration.FloatMinimum not initialised correctly");
            Assert.AreEqual(float.MaxValue, counter.Configuration.FloatMaximum, "Configuration.FloatMaximum not initialised correctly");
            Assert.AreEqual(CounterConfiguration.SaveType.None, counter.Configuration.Save, "Configuration.PersistChanges not initialised correctly");
        }


        /// <summary>
        /// Seperate test from creating, saving and then loading a GameItem to verify the consistency of saved preferences
        /// across different versions of the framework (that we use the same preferences keys).
        /// </summary>
        /// expectedAmount should be set based upon persistCHanges parameter
        [TestCase("T", "Counter", 0)]
        [TestCase("T", "Counter", 0)]
        [TestCase("A", "Counter", 0)]
        [TestCase("T", "AnotherCounter", 0)]
        [TestCase("T", "Counter", 0)]
        [TestCase("T", "Counter", 0)]
        public void CounterInitialisationDefault2(string prefsPrefix, string counterName, int identifier)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();

            //// Act
            var counterConfiguration = new CounterConfiguration();
            var counter = new Counter(counterConfiguration, prefsPrefix, identifier);

            //// Assert
            Assert.AreEqual(counterConfiguration, counter.Configuration, "Counter configuration not initialised correctly");
            Assert.AreEqual(identifier, counter.Identifier, "Identifier configuration not initialised correctly");
            Assert.AreEqual(0, counter.IntAmount, "IntAmount not initialised correctly");
            Assert.AreEqual(0, counter.IntAmountSaved, "IntAmountLastSaved not initialised correctly");
            Assert.AreEqual(0, counter.IntAmountBest, "IntAmountBest not initialised correctly");
            Assert.AreEqual(0, counter.IntAmountBestSaved, "IntAmountBestLastSaved not initialised correctly");
            Assert.AreEqual(0, counter.FloatAmount, "FloatAmount not initialised correctly");
            Assert.AreEqual(0, counter.FloatAmountSaved, "FloatAmountLastSaved not initialised correctly");
            Assert.AreEqual(0, counter.FloatAmountBest, "FloatAmountBest not initialised correctly");
            Assert.AreEqual(0, counter.FloatAmountBestSaved, "FloatAmountBestLastSaved not initialised correctly");
            Assert.AreEqual(CounterConfiguration.CounterTypeEnum.Int, counter.Configuration.CounterType, "Configuration.CounterType not initialised correctly");
            Assert.AreEqual(0, counter.Configuration.IntMinimum, "Configuration.IntMinimum not initialised correctly");
            Assert.AreEqual(int.MaxValue, counter.Configuration.IntMaximum, "Configuration.IntMaximum not initialised correctly");
            Assert.AreEqual(0, counter.Configuration.FloatMinimum, "Configuration.FloatMinimum not initialised correctly");
            Assert.AreEqual(float.MaxValue, counter.Configuration.FloatMaximum, "Configuration.FloatMaximum not initialised correctly");
            Assert.AreEqual(CounterConfiguration.SaveType.None, counter.Configuration.Save, "Configuration.PersistChanges not initialised correctly");
        }


        /// <summary>
        /// Seperate test from creating, saving and then loading a GameItem to verify the consistency of saved preferences
        /// across different versions of the framework (that we use the same preferences keys).
        /// </summary>
        /// expectedAmount should be set based upon persistCHanges parameter
        [TestCase("T", "Counter", 10, 10)]
        [TestCase("T", "Counter", 10, 10)]
        [TestCase("A", "Counter", 10, 10)]
        [TestCase("T", "AnotherCounter", 10, 10)]
        [TestCase("T", "Counter", 10, 0)]
        [TestCase("T", "Counter", 20, 20)]
        public void CounterInitialisationPersistentIntValuesLoaded(string prefsPrefix, string counterName, int amount, int bestAmount)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt(string.Format("{0}CI.{1}", prefsPrefix, counterName), amount);
            PlayerPrefs.SetInt(string.Format("{0}CIH.{1}", prefsPrefix, counterName), bestAmount);

            //// Act
            var counterConfiguration = new CounterConfiguration() {
                Name = counterName,
                Save = CounterConfiguration.SaveType.Always,    //SaveBest defaults to Always
                CounterType = CounterConfiguration.CounterTypeEnum.Int
            };
            var counter = new Counter(counterConfiguration, prefsPrefix);
            counter.LoadFromPrefs();

            //// Assert
            Assert.AreEqual(amount, counter.IntAmount, "IntAmount not set correctly");
            Assert.AreEqual(amount, counter.IntAmountSaved, "IntAmountLastSaved not set correctly");
            Assert.AreEqual(bestAmount, counter.IntAmountBest, "IntAmountBest not set correctly");
            Assert.AreEqual(bestAmount, counter.IntAmountBestSaved, "IntAmountBestLastSaved not set correctly");
        }


        /// <summary>
        /// Seperate test from creating, saving and then loading a GameItem to verify the consistency of saved preferences
        /// across different versions of the framework (that we use the same preferences keys).
        /// </summary>
        /// expectedAmount should be set based upon persistCHanges parameter
        [TestCase("T", "Counter", 10, 10)]
        [TestCase("T", "Counter", 10, 10)]
        [TestCase("A", "Counter", 10, 10)]
        [TestCase("T", "AnotherCounter", 10, 10)]
        [TestCase("T", "Counter", 10, 0)]
        [TestCase("T", "Counter", 20, 20)]
        public void CounterInitialisationPersistentIntValuesNotLoaded(string prefsPrefix, string counterName, int amount, int bestAmount)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt(string.Format("{0}CI.{1}", prefsPrefix, counterName), amount);
            PlayerPrefs.SetInt(string.Format("{0}CIH.{1}", prefsPrefix, counterName), bestAmount);

            //// Act
            var counterConfiguration = new CounterConfiguration()
            {
                Name = counterName,
                SaveBest = CounterConfiguration.SaveType.None,    //Save defaults to None
                CounterType = CounterConfiguration.CounterTypeEnum.Int
            };
            var counter = new Counter(counterConfiguration, prefsPrefix);
            counter.LoadFromPrefs();

            //// Assert
            Assert.AreEqual(0, counter.IntAmount, "IntAmount not set correctly");
            Assert.AreEqual(0, counter.IntAmountSaved, "IntAmountLastSaved not set correctly");
            Assert.AreEqual(0, counter.IntAmountBest, "IntAmountBest not set correctly");
            Assert.AreEqual(0, counter.IntAmountBestSaved, "IntAmountBestLastSaved not set correctly");
        }


        /// <summary>
        /// Seperate test from creating, saving and then loading a GameItem to verify the consistency of saved preferences
        /// across different versions of the framework (that we use the same preferences keys).
        /// </summary>
        /// expectedAmount should be set based upon persistCHanges parameter
        [TestCase("T", "Counter", 10, 10)]
        [TestCase("T", "Counter", 10, 10)]
        [TestCase("A", "Counter", 10, 10)]
        [TestCase("T", "AnotherCounter", 10, 10)]
        [TestCase("T", "Counter", 10, 0)]
        [TestCase("T", "Counter", 20, 20)]
        public void CounterInitialisationPersistentFloatValuesLoaded(string prefsPrefix, string counterName, float amount, float bestAmount)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetFloat(string.Format("{0}CF.{1}", prefsPrefix, counterName), amount);
            PlayerPrefs.SetFloat(string.Format("{0}CFH.{1}", prefsPrefix, counterName), bestAmount);

            //// Act
            var counterConfiguration = new CounterConfiguration()
            {
                Name = counterName,
                Save = CounterConfiguration.SaveType.Always,
                CounterType = CounterConfiguration.CounterTypeEnum.Float
            };
            var counter = new Counter(counterConfiguration, prefsPrefix);
            counter.LoadFromPrefs();

            //// Assert
            Assert.AreEqual(amount, counter.FloatAmount, "FloatAmount not set correctly");
            Assert.AreEqual(bestAmount, counter.FloatAmountBest, "HighestFloatAmount not set correctly");
        }


        /// <summary>
        /// Seperate test from creating, saving and then loading a GameItem to verify the consistency of saved preferences
        /// across different versions of the framework (that we use the same preferences keys).
        /// </summary>
        /// expectedAmount should be set based upon persistCHanges parameter
        [TestCase("T", "Counter", 10, 10)]
        [TestCase("T", "Counter", 10, 10)]
        [TestCase("A", "Counter", 10, 10)]
        [TestCase("T", "AnotherCounter", 10, 10)]
        [TestCase("T", "Counter", 10, 0)]
        [TestCase("T", "Counter", 20, 20)]
        public void CounterInitialisationPersistentFloatValuesNotLoaded(string prefsPrefix, string counterName, float amount, float bestAmount)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetFloat(string.Format("{0}CI.{1}", prefsPrefix, counterName), amount);
            PlayerPrefs.SetFloat(string.Format("{0}CIH.{1}", prefsPrefix, counterName), bestAmount);

            //// Act
            var counterConfiguration = new CounterConfiguration()
            {
                Name = counterName,
                SaveBest = CounterConfiguration.SaveType.None,    //Save defaults to None
                CounterType = CounterConfiguration.CounterTypeEnum.Float
            };
            var counter = new Counter(counterConfiguration, prefsPrefix);
            counter.LoadFromPrefs();

            //// Assert
            Assert.AreEqual(0, counter.FloatAmount, "FloatAmount not set correctly");
            Assert.AreEqual(0, counter.FloatAmountSaved, "FloatAmountSaved not set correctly");
            Assert.AreEqual(0, counter.FloatAmountBest, "FloatAmountBest not set correctly");
            Assert.AreEqual(0, counter.FloatAmountBestSaved, "FloatAmountBestSaved not set correctly");
        }


        /// <summary>
        /// Seperate test from creating, saving and then loadina GameItem to verify the consistency of saved preferences
        /// across different versions of the framework (that we use the same preferences keys).
        /// </summary>
        [TestCase("T", "Counter", 10, 10)]
        [TestCase("T", "Counter", 10, 10)]
        [TestCase("A", "Counter", 10, 10)]
        [TestCase("T", "AnotherCounter", 10, 10)]
        [TestCase("T", "Counter", 10, 20)]
        [TestCase("T", "Counter", 20, 20)]
        public void CounterPersistentValuesSavedInt(string prefsPrefix,
            string counterName, int amount, int bestAmount)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();

            var counterConfiguration = new CounterConfiguration()
            {
                Name = counterName,
                Save = CounterConfiguration.SaveType.Always,
                CounterType = CounterConfiguration.CounterTypeEnum.Int
            };
            var counter = new Counter(counterConfiguration, prefsPrefix);
            counter.IntAmount = bestAmount; // to set best
            counter.IntAmount = amount;

            //// Act
            counter.UpdatePlayerPrefs();
            PlayerPrefs.Save();

            //// Assert
            Assert.AreEqual(amount, PlayerPrefs.GetInt(string.Format("{0}CI.{1}", prefsPrefix, counterName), 0), "IntAmount not saved correctly");
            Assert.AreEqual(bestAmount, PlayerPrefs.GetInt(string.Format("{0}CIH.{1}", prefsPrefix, counterName), 0), "HighestIntAmount not saved correctly");
        }


        /// <summary>
        /// Seperate test from creating, saving and then loadina GameItem to verify the consistency of saved preferences
        /// across different versions of the framework (that we use the same preferences keys).
        /// </summary>
        [TestCase("T", "Counter", 10, 10)]
        [TestCase("T", "Counter", 10, 10)]
        [TestCase("A", "Counter", 10, 10)]
        [TestCase("T", "AnotherCounter", 10, 10)]
        [TestCase("T", "Counter", 10, 0)]
        [TestCase("T", "Counter", 10, 20)]
        [TestCase("T", "Counter", 20, 20)]
        public void CounterNonPersistentValuesNotSavedInt(string prefsPrefix,
            string counterName, int amount, int bestAmount)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();

            var counterConfiguration = new CounterConfiguration()
            {
                Name = counterName,
                SaveBest = CounterConfiguration.SaveType.None,  // Save defaults to none
                CounterType = CounterConfiguration.CounterTypeEnum.Int
            };
            var counter = new Counter(counterConfiguration, prefsPrefix);
            counter.IntAmount = amount;
            //counter.IntAmountBest = bestAmount;

            //// Act
            counter.UpdatePlayerPrefs();
            PlayerPrefs.Save();

            //// Assert
            Assert.IsFalse(PlayerPrefs.HasKey(string.Format("{0}CI.{1}", prefsPrefix, counterName)), "IntAmount should not have been saved");
            Assert.IsFalse(PlayerPrefs.HasKey(string.Format("{0}CIH.{1}", prefsPrefix, counterName)), "HighestIntAmount should not have been saved");
        }


        /// <summary>
        /// Seperate test from creating, saving and then loadina GameItem to verify the consistency of saved preferences
        /// across different versions of the framework (that we use the same preferences keys).
        /// </summary>
        [TestCase("T", "Counter", 10, 10)]
        [TestCase("T", "Counter", 10, 10)]
        [TestCase("A", "Counter", 10, 10)]
        [TestCase("T", "AnotherCounter", 10, 10)]
        [TestCase("T", "Counter", 10, 20)]
        [TestCase("T", "Counter", 20, 20)]
        public void CounterPersistentValuesSavedFloat(string prefsPrefix, string counterName, float amount, float bestAmount)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();

            var counterConfiguration = new CounterConfiguration()
            {
                Name = counterName,
                Save = CounterConfiguration.SaveType.Always,
                CounterType = CounterConfiguration.CounterTypeEnum.Float
            };
            var counter = new Counter(counterConfiguration, prefsPrefix);
            counter.FloatAmount = bestAmount;   // to set best
            counter.FloatAmount = amount;

            //// Act
            counter.UpdatePlayerPrefs();
            PlayerPrefs.Save();

            //// Assert
            Assert.AreEqual(amount, PlayerPrefs.GetFloat(string.Format("{0}CF.{1}", prefsPrefix, counterName), 0), "FloatAmount not saved correctly");
            Assert.AreEqual(bestAmount, PlayerPrefs.GetFloat(string.Format("{0}CFH.{1}", prefsPrefix, counterName), 0), "HighestFloatAmount not saved correctly");
        }


        /// <summary>
        /// Seperate test from creating, saving and then loadina GameItem to verify the consistency of saved preferences
        /// across different versions of the framework (that we use the same preferences keys).
        /// </summary>
        [TestCase("T", "Counter", 10, 10)]
        [TestCase("T", "Counter", 10, 10)]
        [TestCase("A", "Counter", 10, 10)]
        [TestCase("T", "AnotherCounter", 10, 10)]
        [TestCase("T", "Counter", 10, 0)]
        [TestCase("T", "Counter", 10, 20)]
        [TestCase("T", "Counter", 20, 20)]
        public void CounterNonPersistentValuesNotSavedFloat(string prefsPrefix,
            string counterName, float amount, float bestAmount)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();

            var counterConfiguration = new CounterConfiguration()
            {
                Name = counterName,
                SaveBest = CounterConfiguration.SaveType.None,  // Save defaults to none
                CounterType = CounterConfiguration.CounterTypeEnum.Float
            };
            var counter = new Counter(counterConfiguration, prefsPrefix);
            counter.FloatAmount = amount;
            //counter.FloatAmountBest = bestAmount;

            //// Act
            counter.UpdatePlayerPrefs();
            PlayerPrefs.Save();

            //// Assert
            Assert.IsFalse(PlayerPrefs.HasKey(string.Format("{0}CI.{1}", prefsPrefix, counterName)), "FloatAmount should not have been saved");
            Assert.IsFalse(PlayerPrefs.HasKey(string.Format("{0}CIH.{1}", prefsPrefix, counterName)), "HighestFloatAmount should not have been saved");
        }
        #endregion Initialisation

        [TestCase( 0, int.MaxValue, 10, 10)]
        [TestCase(0, int.MaxValue, 20000, 20000)]
        [TestCase(0, int.MaxValue, -1, 0)]
        [TestCase(0, 100, 10, 10)]
        [TestCase(0, 100, 101, 100)]
        [TestCase(int.MinValue, 100, 10, 10)]
        [TestCase(int.MinValue, 100, -10, -10)]
        [TestCase(int.MinValue, 110, 110, 110)]
        public void CounterSetIntWithinBounds(int min, int max, int amount, int expected)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();

            var counterConfiguration = new CounterConfiguration()
            {
                Name = "Counter",
                CounterType = CounterConfiguration.CounterTypeEnum.Int,
                SaveBest = CounterConfiguration.SaveType.None,
                IntMinimum = min,
                IntMaximum = max
            };
            var counter = new Counter(counterConfiguration);

            //// Act
            counter.Set(amount);

            //// Assert
            Assert.AreEqual(expected, counter.IntAmount, "Amount not set correctly.");
        }


        [TestCase(0, float.MaxValue, 10, 10)]
        [TestCase(0, float.MaxValue, 20000, 20000)]
        [TestCase(0, float.MaxValue, -1, 0)]
        [TestCase(0, 100, 10, 10)]
        [TestCase(0, 100, 101, 100)]
        [TestCase(float.MinValue, 100, 10, 10)]
        [TestCase(float.MinValue, 100, -10, -10)]
        [TestCase(float.MinValue, 110, 110, 110)]
        public void CounterSetFloatWithinBounds(float min, float max, float amount, float expected)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();

            var counterConfiguration = new CounterConfiguration()
            {
                Name = "Counter",
                CounterType = CounterConfiguration.CounterTypeEnum.Float,
                SaveBest = CounterConfiguration.SaveType.None,
                FloatMinimum = min,
                FloatMaximum = max
            };
            var counter = new Counter(counterConfiguration);

            //// Act
            counter.Set(amount);

            //// Assert
            Assert.AreEqual(expected, counter.FloatAmount, "Amount not set correctly.");
        }


        [TestCase(0, 10)]
        [TestCase(0, 0)]
        [TestCase(10, 100)]
        public void CounterResetFloat(float defaultAmount, float amount)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();

            var counterConfiguration = new CounterConfiguration()
            {
                Name = "Counter",
                CounterType = CounterConfiguration.CounterTypeEnum.Float,
                FloatDefault = defaultAmount,
                SaveBest = CounterConfiguration.SaveType.None,
            };
            var counter = new Counter(counterConfiguration);
            counter.Set(amount);

            //// Act
            counter.Reset();

            //// Assert
            Assert.AreEqual(defaultAmount, counter.FloatAmount, "Reset did not set default amount correctly.");
        }


        [TestCase(0, 10)]
        [TestCase(0, 0)]
        [TestCase(10, 100)]
        public void CounterResetInt(int defaultAmount, int amount)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();

            var counterConfiguration = new CounterConfiguration()
            {
                Name = "Counter",
                CounterType = CounterConfiguration.CounterTypeEnum.Int,
                IntDefault = defaultAmount,
                SaveBest = CounterConfiguration.SaveType.None,
            };
            var counter = new Counter(counterConfiguration);
            counter.Set(amount);

            //// Act
            counter.Reset();

            //// Assert
            Assert.AreEqual(defaultAmount, counter.IntAmount, "Reset did not set default amount correctly.");
        }


        [TestCase(10, 10, CounterConfiguration.SaveType.Always, CounterConfiguration.SaveType.Always)]
        [TestCase(20000, 10, CounterConfiguration.SaveType.Always, CounterConfiguration.SaveType.Always)]
        [TestCase(0, 10, CounterConfiguration.SaveType.Always, CounterConfiguration.SaveType.Always)]
        [TestCase(10, 100, CounterConfiguration.SaveType.Always, CounterConfiguration.SaveType.Always)]
        [TestCase(10, 100, CounterConfiguration.SaveType.None, CounterConfiguration.SaveType.Always)]
        [TestCase(10, 100, CounterConfiguration.SaveType.Always, CounterConfiguration.SaveType.None)]
        public void PushToSavedInt(int amount, int amountBest, CounterConfiguration.SaveType save, CounterConfiguration.SaveType saveBest)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();

            var counterConfiguration = new CounterConfiguration()
            {
                Save = save,
                SaveBest = saveBest
            };
            var counter = new Counter(counterConfiguration, "Prefix.");
            counter.IntAmount = amount;
            //counter.IntAmountBest = amountBest;

            //// Act
            counter.PushToSaved();

            //// Assert
            if (save == CounterConfiguration.SaveType.None)
                Assert.AreEqual(0, counter.IntAmountSaved, "IntAmount wrongly pushed to save.");
            else
                Assert.AreEqual(counter.IntAmount, counter.IntAmountSaved, "Amount not pushed to save.");

            if (saveBest == CounterConfiguration.SaveType.None)
                Assert.AreEqual(0, counter.IntAmountBestSaved, "IntAmountBest wrongly pushed to save.");
            else
                Assert.AreEqual(counter.IntAmountBest, counter.IntAmountBestSaved, "IntAmountBest not pushed to save.");
        }


        [TestCase(10, 10, CounterConfiguration.SaveType.Always, CounterConfiguration.SaveType.Always)]
        [TestCase(20000, 10, CounterConfiguration.SaveType.Always, CounterConfiguration.SaveType.Always)]
        [TestCase(0, 10, CounterConfiguration.SaveType.Always, CounterConfiguration.SaveType.Always)]
        [TestCase(10, 100, CounterConfiguration.SaveType.Always, CounterConfiguration.SaveType.Always)]
        [TestCase(10, 100, CounterConfiguration.SaveType.None, CounterConfiguration.SaveType.Always)]
        [TestCase(10, 100, CounterConfiguration.SaveType.Always, CounterConfiguration.SaveType.None)]
        public void PushToSavedFloat(float amount, float amountBest, CounterConfiguration.SaveType save, CounterConfiguration.SaveType saveBest)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();

            var counterConfiguration = new CounterConfiguration()
            {
                Save = save,
                SaveBest = saveBest
            };
            var counter = new Counter(counterConfiguration, "Prefix.");
            counter.FloatAmount = amount;
            //counter.FloatAmountBest = amountBest;

            //// Act
            counter.PushToSaved();

            //// Assert
            if (save == CounterConfiguration.SaveType.None)
                Assert.AreEqual(0, counter.FloatAmountSaved, "FloatAmount wrongly pushed to save.");
            else
                Assert.AreEqual(counter.FloatAmount, counter.FloatAmountSaved, "Amount not pushed to save.");

            if (saveBest == CounterConfiguration.SaveType.None)
                Assert.AreEqual(0, counter.FloatAmountBestSaved, "FloatAmountBest wrongly pushed to save.");
            else
                Assert.AreEqual(counter.FloatAmountBest, counter.FloatAmountBestSaved, "IntAmountBest not pushed to save.");
        }

        [TestCase(1, 1, 0, 10)]
        [TestCase(2, 2, 20, 30)]
        [TestCase(2, 2, 30, 40)]
        public void IntAmountChangeHandlerCalled(int expectedMessages, int expectedBestMessages, int amount1, int amount2)
        {
            //// Arrange
            var counterConfiguration = new CounterConfiguration()
            {
                SaveBest = CounterConfiguration.SaveType.None
            };
            var counterCallbackTestClass = new CounterCallbackTestClass();
            var counter = new Counter(counterConfiguration, counterChangedCallbacks: counterCallbackTestClass);
            counter.Set(amount1);

            //// Act
            counter.Set(amount2);

            //// Assert
            Assert.AreEqual(expectedMessages, counterCallbackTestClass.IntAmountChangedCounter, "Incorrect number of messages sent.");
            Assert.AreEqual(expectedBestMessages, counterCallbackTestClass.IntAmountBestChangedCounter, "Incorrect number of messages sent.");
            Assert.AreEqual(amount1, counterCallbackTestClass.OldIntAmount, "Incorrect old amount in message.");
            Assert.AreEqual(amount2, counterCallbackTestClass.NewIntAmount, "Incorrect new amount in message.");
        }

        [TestCase(1, 1, 0, 10)]
        [TestCase(2, 2, 20, 30)]
        [TestCase(2, 2, 30, 40)]
        public void FloatAmountChangeHandlerCalled(int expectedMessages, int expectedBestMessages, float amount1, float amount2)
        {
            //// Arrange
            var counterConfiguration = new CounterConfiguration()
            {
                SaveBest = CounterConfiguration.SaveType.None
            };
            var counterCallbackTestClass = new CounterCallbackTestClass();
            var counter = new Counter(counterConfiguration, counterChangedCallbacks: counterCallbackTestClass);
            counter.Set(amount1);

            //// Act
            counter.Set(amount2);

            //// Assert
            Assert.AreEqual(expectedMessages, counterCallbackTestClass.FloatAmountChangedCounter, "Incorrect number of messages sent.");
            Assert.AreEqual(expectedBestMessages, counterCallbackTestClass.FloatAmountBestChangedCounter, "Incorrect number of messages sent.");
            Assert.AreEqual(amount1, counterCallbackTestClass.OldFloatAmount, "Incorrect old amount in message.");
            Assert.AreEqual(amount2, counterCallbackTestClass.NewFloatAmount, "Incorrect new amount in message.");
        }
    }
}
#endif
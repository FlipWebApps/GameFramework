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
using GameFramework.GameStructure.Levels.ObjectModel;
using GameFramework.Localisation.ObjectModel;
using GameFramework.Messaging;
using NUnit.Framework;
using UnityEngine;
using GameFramework.GameStructure.Game.ObjectModel;
using GameFramework.GameStructure.Players.ObjectModel;
using GameFramework.GameStructure.GameItems;
using System.Collections.Generic;
using GameFramework.GameStructure.Levels.Messages;

namespace GameFramework.GameStructure.Levels
{
    /// <summary>
    /// Test cases for Player GameItems. You can also view these to see how you might use the API.
    /// </summary>
    public class LevelsTests
    {

        #region Helper functions for verifying testing

        #endregion Helper functions for verifying testing

        #region Initialisation

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void BasicInitialisationDefaults(int number)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);

            //// Act
            var gameItem = ScriptableObject.CreateInstance<Level>();
            gameItem.Initialise(gameConfiguration, player, messenger, number);

            //// Assert
            Assert.IsNotNull(gameItem, "GameItem not setup.");
            Assert.AreEqual(number, gameItem.Number, "Number not set correctly");
            //TODO: Verify if we can test the below, or if localisation setup will interfere?
            //Assert.AreEqual("Name", gameItem.Name, "Name not set correctly");
            //Assert.AreEqual("Desc", gameItem.Description, "Description not set correctly");
            Assert.AreEqual("Level", gameItem.IdentifierBase, "IdentifierBase not set correctly");
            Assert.AreEqual("L", gameItem.IdentifierBasePrefs, "IdentifierBasePrefs not set correctly");
            Assert.AreEqual(0, gameItem.Score, "Score not set correctly");
            Assert.AreEqual(0, gameItem.Coins, "Coins not set correctly");
            Assert.AreEqual(0, gameItem.HighScore, "HighScore not set correctly");
            Assert.AreEqual(false, player.IsBought, "IsBought not set correctly");
            Assert.AreEqual(false, gameItem.IsUnlocked, "IsUnlocked not set correctly");
            Assert.AreEqual(false, gameItem.IsUnlockedAnimationShown, "IsUnlockedAnimationShown not set correctly");

            Assert.AreEqual(3, gameItem.StarsTotalCount, "StarTotalCount not set correctly");
            Assert.AreEqual(10, gameItem.Star1Target, "Star1Target not set correctly");
            Assert.AreEqual(15, gameItem.Star2Target, "Star2Target not set correctly");
            Assert.AreEqual(20, gameItem.Star3Target, "Star3Target not set correctly");
            Assert.AreEqual(25, gameItem.Star4Target, "Star4Target not set correctly");
            Assert.AreEqual(0, gameItem.TimeTarget, "TimeTarget not set correctly");
            Assert.AreEqual(0, gameItem.ScoreTarget, "ScoreTarget not set correctly");
            Assert.AreEqual(0, gameItem.CoinTarget, "CoinTarget not set correctly");

            Assert.AreEqual(0, gameItem.StarsWon, "StarsWon not set correctly");
            Assert.AreEqual(0, gameItem.TimeBest, "TimeBest not set correctly");
            Assert.AreEqual(0, gameItem.ProgressBest, "ProgressBest not set correctly");
        }


        [TestCase(1, "Name", "Desc")]
        [TestCase(2, "Name2", "Desc2")]
        public void BasicInitialisationSpecifiedValues(int number, string name, string desc)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);

            //// Act
            var gameItem = ScriptableObject.CreateInstance<Level>();
            gameItem.Initialise(gameConfiguration, player, messenger,
                number, LocalisableText.CreateNonLocalised(name), LocalisableText.CreateNonLocalised(desc),
                identifierBase: "Test_Should_Not_Override", identifierBasePrefs: "T");

            //// Assert
            Assert.IsNotNull(gameItem, "GameItem not setup.");
            Assert.AreEqual(number, gameItem.Number, "Number not set correctly");
            Assert.AreEqual(name, gameItem.Name, "Name not set correctly");
            Assert.AreEqual(desc, gameItem.Description, "Description not set correctly");
            Assert.AreEqual("Level", gameItem.IdentifierBase, "IdentifierBase not set correctly");
            Assert.AreEqual("L", gameItem.IdentifierBasePrefs, "IdentifierBasePrefs not set correctly");
            Assert.AreEqual(0, gameItem.Score, "Score not set correctly");
            Assert.AreEqual(0, gameItem.Coins, "Coins not set correctly");
            Assert.AreEqual(0, gameItem.HighScore, "HighScore not set correctly");
            Assert.AreEqual(false, gameItem.IsBought, "IsBought not set correctly");
            Assert.AreEqual(false, gameItem.IsUnlocked, "IsUnlocked not set correctly");
            Assert.AreEqual(false, gameItem.IsUnlockedAnimationShown, "IsUnlockedAnimationShown not set correctly");

            Assert.AreEqual(3, gameItem.StarsTotalCount, "StarTotalCount not set correctly");
            Assert.AreEqual(10, gameItem.Star1Target, "Star1Target not set correctly");
            Assert.AreEqual(15, gameItem.Star2Target, "Star2Target not set correctly");
            Assert.AreEqual(20, gameItem.Star3Target, "Star3Target not set correctly");
            Assert.AreEqual(25, gameItem.Star4Target, "Star4Target not set correctly");
            Assert.AreEqual(0, gameItem.TimeTarget, "TimeTarget not set correctly");
            Assert.AreEqual(0, gameItem.ScoreTarget, "ScoreTarget not set correctly");
            Assert.AreEqual(0, gameItem.CoinTarget, "CoinTarget not set correctly");

            Assert.AreEqual(0, gameItem.StarsWon, "StarsWon not set correctly");
            Assert.AreEqual(0, gameItem.TimeBest, "TimeBest not set correctly");
            Assert.AreEqual(0, gameItem.ProgressBest, "ProgressBest not set correctly");
        }


        /// <summary>
        /// Seperate test from creating, saving and then loading a GameItem to verify the consistency of saved preferences
        /// across different versions of the framework (that we use the same preferences keys).
        /// </summary>
        [TestCase(0, 1, 10, false, false, false, 0, 0, 0)]
        [TestCase(1, 2, 10, false, false, false, 0, 0, 0)]
        [TestCase(0, 1, 20, false, false, false, 0, 0, 0)]
        [TestCase(0, 1, 20, true, false, false, 0, 0, 0)]
        [TestCase(0, 1, 20, false, true, false, 0, 0, 0)]
        [TestCase(0, 1, 20, false, false, true, 0, 0, 0)]
        [TestCase(0, 1, 20, false, false, true, 3, 0, 0)]
        [TestCase(0, 1, 20, false, false, true, 0, 20, 0)]
        [TestCase(0, 1, 20, false, false, true, 0, 0, 30)]
        public void PersistentValuesLoaded(int playerNumber, int number,
            int highScore, bool isUnlocked, bool isUnlockedAnimationShown, bool isBought, 
            int starsWon, float timeBest, float progressBest)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt(string.Format("P{0}.L{1}.SW", playerNumber, number), starsWon);
            PlayerPrefs.SetFloat(string.Format("P{0}.L{1}.TimeBest", playerNumber, number), timeBest);
            PlayerPrefs.SetFloat(string.Format("P{0}.L{1}.ProgressBest", playerNumber, number), progressBest);
            GameItemTests.SetCommonPreferences(playerNumber, number, "L", highScore, isUnlocked, isUnlockedAnimationShown, isBought);
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, playerNumber);

            //// Act
            var gameItem = ScriptableObject.CreateInstance<Level>();
            gameItem.Initialise(gameConfiguration, player, messenger, number);

            //// Assert
            Assert.IsNotNull(player, "GameItem not setup.");
            Assert.AreEqual(highScore, gameItem.HighScore, "HighScore not set correctly");
            Assert.AreEqual(isBought, gameItem.IsBought, "IsBought not set correctly");
            if (isBought)
                Assert.AreEqual(true, gameItem.IsUnlocked, "IsUnlocked not set correctly when bought");
            else
                Assert.AreEqual(isUnlocked, gameItem.IsUnlocked, "IsUnlocked not set correctly when not bought");
            Assert.AreEqual(isUnlockedAnimationShown, gameItem.IsUnlockedAnimationShown, "IsUnlockedAnimationShown not set correctly");

            Assert.AreEqual(starsWon, gameItem.StarsWon, "StarsWon not set correctly");
            Assert.AreEqual(timeBest, gameItem.TimeBest, "TimeBest not set correctly");
            Assert.AreEqual(progressBest, gameItem.ProgressBest, "ProgressBest not set correctly");
        }


        /// <summary>
        /// Seperate test from creating, saving and then loading a GameItem to verify the consistency of saved preferences
        /// across different versions of the framework (that we use the same preferences keys).
        /// </summary>
        [TestCase(0, 1, 10, false, false, false, 0, 0, 0)]
        [TestCase(1, 2, 10, false, false, false, 0, 0, 0)]
        [TestCase(0, 1, 20, false, false, false, 0, 0, 0)]
        [TestCase(0, 1, 20, true, false, false, 0, 0, 0)]
        [TestCase(0, 1, 20, false, true, false, 0, 0, 0)]
        [TestCase(0, 1, 20, false, false, true, 0, 0, 0)]
        [TestCase(0, 1, 20, false, false, true, 3, 0, 0)]
        [TestCase(0, 1, 20, false, false, true, 0, 20, 0)]
        [TestCase(0, 1, 20, false, false, true, 0, 0, 0.7f)]
        public void PersistentValuesSaved(int playerNumber, int number,
            int highScore, bool isUnlocked, bool isUnlockedAnimationShown, bool isBought,
            int starsWon, float timeBest, float progressBest)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, playerNumber);

            //// Act
            var gameItem = ScriptableObject.CreateInstance<Level>();
            gameItem.Initialise(gameConfiguration, player, messenger, number);
            gameItem.Score = highScore; // score should set high score automatically which is saved
            //gameItem.HighScore = highScore;
            gameItem.IsUnlocked = isUnlocked;
            gameItem.IsUnlockedAnimationShown = isUnlockedAnimationShown;
            gameItem.IsBought = isBought;
            gameItem.StarsWon = starsWon;
            gameItem.TimeBest = timeBest;
            gameItem.Progress = progressBest; // progress should set ProgressBest automatically which is saved
            gameItem.UpdatePlayerPrefs();
            PlayerPrefs.Save();

            //// Assert
            Assert.IsNotNull(gameItem, "GameItem not setup.");
            GameItemTests.AssertCommonPreferences(playerNumber, number, "L", gameItem);
            Assert.AreEqual(starsWon, PlayerPrefs.GetInt(string.Format("P{0}.L{1}.SW", playerNumber, number), 0), "StarsWon not set correctly");
            Assert.AreEqual(timeBest, PlayerPrefs.GetFloat(string.Format("P{0}.L{1}.TimeBest", playerNumber, number), 0), "TimeBest not set correctly");
            Assert.AreEqual(progressBest, PlayerPrefs.GetFloat(string.Format("P{0}.L{1}.ProgressBest", playerNumber, number), 0), "ProgressBest not set correctly");
        }

        #endregion Initialisation

        #region Score

        [TestCase(0, 10)]
        [TestCase(20, 30)]
        [TestCase(30, 40)]
        public void ScoreMessageSent(int score1, int score2)
        {
            //// Arrange
            List<LevelScoreChangedMessage> messages = new List<LevelScoreChangedMessage>();
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);
            var gameItem = ScriptableObject.CreateInstance<Level>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);
            gameItem.Score = score1;
            messenger.ProcessQueue();               // clear queue incase initialisation generated a message.
            messenger.AddListener(typeof(LevelScoreChangedMessage), (x) => {
                messages.Add(x as LevelScoreChangedMessage);
                return true;
            });

            //// Act
            gameItem.Score = score2;

            messenger.ProcessQueue();   // force processing of messages.

            //// Assert
            Assert.AreEqual(1, messages.Count, "Incorrect number of messages sent.");
            Assert.AreEqual(score1, messages[0].OldScore, "Incorrect old score in message2.");
            Assert.AreEqual(score2, messages[0].NewScore, "Incorrect new score in message2.");
        }


        [TestCase(0, 10)]
        [TestCase(20, 30)]
        [TestCase(30, 40)]
        public void HighScoreMessageSent(int score1, int score2)
        {
            //// Arrange
            List<LevelHighScoreChangedMessage> messages = new List<LevelHighScoreChangedMessage>();
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);
            var gameItem = ScriptableObject.CreateInstance<Level>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);
            gameItem.Score = score1;
            messenger.ProcessQueue();               // clear queue incase initialisation generated a message.
            messenger.AddListener(typeof(LevelHighScoreChangedMessage), (x) => {
                messages.Add(x as LevelHighScoreChangedMessage);
                return true;
            });

            //// Act
            gameItem.Score = score2;
            messenger.ProcessQueue();   // force processing of messages.

            //// Assert
            Assert.AreEqual(1, messages.Count, "Incorrect number of messages sent.");
            Assert.AreEqual(score1, messages[0].OldHighScore, "Incorrect old score in message2.");
            Assert.AreEqual(score2, messages[0].NewHighScore, "Incorrect new score in message2.");
        }
        #endregion Score

        #region Coins

        [TestCase(0, 10)]
        [TestCase(20, 30)]
        [TestCase(30, 40)]
        public void CoinsMessageSent(int coins1, int coins2)
        {
            //// Arrange
            List<LevelCoinsChangedMessage> messages = new List<LevelCoinsChangedMessage>();
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);
            var gameItem = ScriptableObject.CreateInstance<Level>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);
            gameItem.Coins = coins1;
            messenger.ProcessQueue();               // clear queue incase initialisation generated a message.
            messenger.AddListener(typeof(LevelCoinsChangedMessage), (x) => {
                messages.Add(x as LevelCoinsChangedMessage);
                return true;
            });

            //// Act
            gameItem.Coins = coins2;

            messenger.ProcessQueue();   // force processing of messages.

            //// Assert
            Assert.AreEqual(1, messages.Count, "Incorrect number of messages sent.");
            Assert.AreEqual(coins1, messages[0].OldCoins, "Incorrect old coins in message2.");
            Assert.AreEqual(coins2, messages[0].NewCoins, "Incorrect new coins in message2.");
        }

        #endregion Coins    

        #region Counters

        [Test]
        public void CounterInitialisationDefaults()
        {
            //// Arrange
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);

            //// Act
            var gameItem = ScriptableObject.CreateInstance<Level>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);

            //// Assert
            Assert.AreNotEqual(-1, gameItem.GetCounterIndex("Score"), "Score counter not setup.");
            Assert.AreNotEqual(-1, gameItem.GetCounterIndex("Coins"), "Coins counter not setup.");
        }

        [TestCase("Test")]
        [TestCase("Test2")]
        public void CounterInitialisationSpecifiedCounters(string counterKey)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            gameConfiguration.LevelCounterConfiguration.Add(new CounterConfiguration() { Name = counterKey });
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);

            //// Act
            var gameItem = ScriptableObject.CreateInstance<Level>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);

            //// Assert
            Assert.AreNotEqual(-1, gameItem.GetCounterIndex("Score"), "Score counter not setup.");
            Assert.AreNotEqual(-1, gameItem.GetCounterIndex("Coins"), "Coins counter not setup.");
            Assert.AreNotEqual(-1, gameItem.GetCounterIndex(counterKey), "Test counter not setup.");
        }
        #endregion Counters    
    }
}
#endif
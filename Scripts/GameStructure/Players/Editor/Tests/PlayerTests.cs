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
using GameFramework.GameStructure.Players.ObjectModel;
using GameFramework.Localisation.ObjectModel;
using GameFramework.Messaging;
using NUnit.Framework;
using UnityEngine;
using GameFramework.GameStructure.Game.ObjectModel;
using GameFramework.GameStructure.Players.Messages;
using System.Collections.Generic;

namespace GameFramework.GameStructure.Players
{
    /// <summary>
    /// Test cases for Player GameItems. You can also view these to see how you might use the API.
    /// </summary>
    public class PlayerTests
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

            //// Act
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, number);

            //// Assert
            Assert.IsNotNull(player, "GameItem not setup.");
            Assert.AreEqual(number, player.Number, "Number not set correctly");
            //TODO: Verify if we can test the below, or if localisation setup will interfere?
            //Assert.AreEqual("Name", gameItem.Name, "Name not set correctly");
            //Assert.AreEqual("Desc", gameItem.Description, "Description not set correctly");
            Assert.AreEqual("Player", player.IdentifierBase, "IdentifierBase not set correctly");
            Assert.AreEqual("P", player.IdentifierBasePrefs, "IdentifierBasePrefs not set correctly");
            Assert.AreEqual(0, player.Score, "Score not set correctly");
            Assert.AreEqual(0, player.Coins, "Coins not set correctly");
            Assert.AreEqual(false, player.IsGameWon, "IsGameWon not set correctly");
            Assert.AreEqual(false, player.IsBought, "IsBought not set correctly");
            Assert.AreEqual(false, player.IsUnlocked, "IsUnlocked not set correctly");
            Assert.AreEqual(false, player.IsUnlockedAnimationShown, "IsUnlockedAnimationShown not set correctly");

            Assert.AreEqual(0, player.HighScore, "HighScore not set correctly");
            Assert.AreEqual(3, player.Lives, "Lives not set correctly");
            Assert.AreEqual(1, player.Health, "Health not set correctly");
        }


        [TestCase(1, "Name", "Desc")]
        [TestCase(2, "Name2", "Desc2")]
        public void BasicInitialisationSpecifiedValues(int number, string name, string desc)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();


            //// Act
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 
                number, LocalisableText.CreateNonLocalised(name), LocalisableText.CreateNonLocalised(desc),
                identifierBase: "Test_Should_Not_Override", identifierBasePrefs: "T");

            //// Assert
            Assert.IsNotNull(player, "GameItem not setup.");
            Assert.AreEqual(number, player.Number, "Number not set correctly");
            Assert.AreEqual(name, player.Name, "Name not set correctly");
            Assert.AreEqual(desc, player.Description, "Description not set correctly");
            Assert.AreEqual("Player", player.IdentifierBase, "IdentifierBase not set correctly");
            Assert.AreEqual("P", player.IdentifierBasePrefs, "IdentifierBasePrefs not set correctly");
            Assert.AreEqual(0, player.Score, "Score not set correctly");
            Assert.AreEqual(0, player.Coins, "Coins not set correctly");
            Assert.AreEqual(0, player.HighScore, "HighScore not set correctly");
            Assert.AreEqual(false, player.IsBought, "IsBought not set correctly");
            Assert.AreEqual(false, player.IsUnlocked, "IsUnlocked not set correctly");
            Assert.AreEqual(false, player.IsUnlockedAnimationShown, "IsUnlockedAnimationShown not set correctly");

            Assert.AreEqual(3, player.Lives, "Lives not set correctly");
            Assert.AreEqual(1, player.Health, "Health not set correctly");
            Assert.AreEqual(false, player.IsGameWon, "IsGameWon not set correctly");
        }


        /// <summary>
        /// Seperate test from creating, saving and then loadina GameItem to verify the consistency of saved preferences
        /// across different versions of the framework (that we use the same preferences keys).
        /// </summary>
        [TestCase(0, 1, 10, false, false, false, 0, 0, 0, 0, false)]
        [TestCase(1, 2, 10, false, false, false, 0, 0, 0, 0, false)]
        [TestCase(0, 1, 20, false, false, false, 0, 0, 0, 0, false)]
        [TestCase(0, 1, 20, true, false, false, 0, 0, 0, 0, false)]
        [TestCase(0, 1, 20, false, true, false, 0, 0, 0, 0, false)]
        [TestCase(0, 1, 20, false, false, true, 0, 0, 0, 0, false)]
        [TestCase(0, 1, 20, false, false, false, 10, 0, 0, 0, false)]
        [TestCase(0, 1, 20, false, false, false, 0, 20, 0, 0, false)]
        [TestCase(0, 1, 20, false, false, false, 0, 0, 3, 0, false)]
        [TestCase(0, 1, 20, false, false, false, 0, 0, 0, 0.5f, false)]
        [TestCase(0, 1, 20, false, false, false, 0, 0, 0, 0, true)]
        public void PersistentValuesLoaded(int playerNumber, int number,  
            int highScore, bool isUnlocked, bool isUnlockedAnimationShown, bool isBought, int score,
            int coins, int lives, float health, bool isGameWon)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt(string.Format("P{0}.HS", playerNumber), highScore);
            PlayerPrefs.SetInt(string.Format("P{0}.IsU", playerNumber), isUnlocked ? 1 : 0);
            PlayerPrefs.SetInt(string.Format("P{0}.IsUAS", playerNumber), isUnlockedAnimationShown ? 1 : 0);
            PlayerPrefs.SetInt(string.Format("P{0}.IsB", playerNumber), isBought ? 1 : 0);
            PlayerPrefs.SetInt(string.Format("P{0}.TotalScore", playerNumber), score);
            PlayerPrefs.SetInt(string.Format("P{0}.TotalCoins", playerNumber), coins);
            PlayerPrefs.SetInt(string.Format("P{0}.Lives", playerNumber), lives);
            PlayerPrefs.SetFloat(string.Format("P{0}.Health", playerNumber), health);
            PlayerPrefs.SetInt(string.Format("P{0}.IsGameWon", playerNumber), isGameWon ? 1 : 0);
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();

            //// Act
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, playerNumber);

            //// Assert
            Assert.IsNotNull(player, "GameItem not setup.");
            Assert.AreEqual(highScore, player.HighScore, "HighScore not set correctly");
            Assert.AreEqual(isBought, player.IsBought, "IsBought not set correctly");
            if (isBought)
                Assert.AreEqual(true, player.IsUnlocked, "IsUnlocked not set correctly when bought");
            else
                Assert.AreEqual(isUnlocked, player.IsUnlocked, "IsUnlocked not set correctly when not bought");
            Assert.AreEqual(isUnlockedAnimationShown, player.IsUnlockedAnimationShown, "IsUnlockedAnimationShown not set correctly");
            Assert.AreEqual(score, player.Score, "Score not set correctly");
            Assert.AreEqual(coins, player.Coins, "Coins not set correctly");
            Assert.AreEqual(lives, player.Lives, "Lives not set correctly");
            Assert.AreEqual(health, player.Health, "Health not set correctly");
            Assert.AreEqual(isGameWon, player.IsGameWon, "IsGameWon not set correctly");
        }


        /// <summary>
        /// Seperate test from creating, saving and then loadina GameItem to verify the consistency of saved preferences
        /// across different versions of the framework (that we use the same preferences keys).
        /// </summary>
        [TestCase(0, 1, 10, false, false, false, 0, 0, 0, 0, false)]
        [TestCase(1, 2, 10, false, false, false, 0, 0, 0, 0, false)]
        [TestCase(0, 1, 20, false, false, false, 0, 0, 0, 0, false)]
        [TestCase(0, 1, 20, true, false, false, 0, 0, 0, 0, false)]
        [TestCase(0, 1, 20, false, true, false, 0, 0, 0, 0, false)]
        [TestCase(0, 1, 20, false, false, true, 0, 0, 0, 0, false)]
        [TestCase(0, 1, 20, false, false, false, 10, 0, 0, 0, false)]
        [TestCase(0, 1, 20, false, false, false, 0, 20, 0, 0, false)]
        [TestCase(0, 1, 20, false, false, false, 0, 0, 3, 0, false)]
        [TestCase(0, 1, 20, false, false, false, 0, 0, 0, 0.5f, false)]
        [TestCase(0, 1, 20, false, false, false, 0, 0, 0, 0, true)]
        public void PersistentValuesSaved(int playerNumber, int number, 
            int highScore, bool isUnlocked, bool isUnlockedAnimationShown, bool isBought, int score,
            int coins, int lives, float health, bool isGameWon)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();

            //// Act
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, playerNumber);
            player.Score = highScore; // score should set high score automatically which is saved
            //gameItem.HighScore = highScore;
            player.IsUnlocked = isUnlocked;
            player.IsUnlockedAnimationShown = isUnlockedAnimationShown;
            player.IsBought = isBought;
            player.Lives = lives;
            player.Health = health;
            player.IsGameWon = isGameWon;
            player.UpdatePlayerPrefs();
            PlayerPrefs.Save();

            //// Assert
            Assert.IsNotNull(player, "GameItem not setup.");
            Assert.AreEqual(PlayerPrefs.GetInt(string.Format("P{0}.HS", playerNumber), 0), player.HighScore, "HighScore not set correctly");
            // note for the below, unlocked is changed by setting bought - we should have a test for this somewhere... 
            Assert.AreEqual(PlayerPrefs.GetInt(string.Format("P{0}.IsU", playerNumber), 0) == 1, player.IsUnlocked, "IsUnlocked not set correctly when not bought");
            Assert.AreEqual(PlayerPrefs.GetInt(string.Format("P{0}.IsUAS", playerNumber), 0) == 1, player.IsUnlockedAnimationShown, "IsUnlockedAnimationShown not set correctly");
            Assert.AreEqual(PlayerPrefs.GetInt(string.Format("P{0}.IsB", playerNumber), 0) == 1, player.IsBought, "IsBought not set correctly");
            Assert.AreEqual(PlayerPrefs.GetInt(string.Format("P{0}.TotalScore", playerNumber), 0), player.Score, "Score not set correctly");
            Assert.AreEqual(PlayerPrefs.GetInt(string.Format("P{0}.TotalCoins", playerNumber), 0), player.Coins, "Coins not set correctly");
            Assert.AreEqual(PlayerPrefs.GetInt(string.Format("P{0}.Lives", playerNumber), 0), player.Lives, "Lives not set correctly");
            Assert.AreEqual(PlayerPrefs.GetFloat(string.Format("P{0}.Health", playerNumber), 0), player.Health, "Health not set correctly");
            Assert.AreEqual(PlayerPrefs.GetInt(string.Format("P{0}.IsGameWon", playerNumber), 0) == 1, player.IsGameWon, "IsGameWon not set correctly");
        }

        #endregion Initialisation

        #region Score

        [TestCase(0, 10)]
        [TestCase(20, 30)]
        [TestCase(30, 40)]
        public void ScoreMessageSent(int score1, int score2)
        {
            //// Arrange
            List<PlayerScoreChangedMessage> messages = new List<PlayerScoreChangedMessage>();
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);
            player.Score = score1;
            messenger.ProcessQueue();               // clear queue incase initialisation generated a message.
            messenger.AddListener(typeof(PlayerScoreChangedMessage), (x) => {
                messages.Add(x as PlayerScoreChangedMessage);
                return true;
            });

            //// Act
            player.Score = score2;
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
            List<PlayerHighScoreChangedMessage> messages = new List<PlayerHighScoreChangedMessage>();
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);
            player.Score = score1;
            messenger.ProcessQueue();               // clear queue incase initialisation generated a message.
            messenger.AddListener(typeof(PlayerHighScoreChangedMessage), (x) => {
                messages.Add(x as PlayerHighScoreChangedMessage);
                return true;
            });

            //// Act
            player.Score = score2;
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
            List<PlayerCoinsChangedMessage> messages = new List<PlayerCoinsChangedMessage>();
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);
            player.Coins = coins1;
            messenger.ProcessQueue();               // clear queue incase initialisation generated a message.
            messenger.AddListener(typeof(PlayerCoinsChangedMessage), (x) => {
                messages.Add(x as PlayerCoinsChangedMessage);
                return true;
            });

            //// Act
            player.Coins = coins2;
            messenger.ProcessQueue();   // force processing of messages.

            //// Assert
            Assert.AreEqual(1, messages.Count, "Incorrect number of messages sent.");
            Assert.AreEqual(coins1, messages[0].OldCoins, "Incorrect old coins in message2.");
            Assert.AreEqual(coins2, messages[0].NewCoins, "Incorrect new coins in message2.");
        }

        #endregion Coins

        #region Lives

        [TestCase(0, 10)]
        [TestCase(20, 30)]
        [TestCase(30, 40)]
        public void LivesMessageSent(int lives1, int lives2)
        {
            //// Arrange
            List<LivesChangedMessage> messages = new List<LivesChangedMessage>();
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);
            player.Lives = lives1;
            messenger.ProcessQueue();               // clear queue incase initialisation generated a message.
            messenger.AddListener(typeof(LivesChangedMessage), (x) => {
                messages.Add(x as LivesChangedMessage);
                return true;
            });

            //// Act
            player.Lives = lives2;
            messenger.ProcessQueue();   // force processing of messages.

            //// Assert
            Assert.AreEqual(1, messages.Count, "Incorrect number of messages sent.");
            Assert.AreEqual(lives1, messages[0].OldLives, "Incorrect old lives in message2.");
            Assert.AreEqual(lives2, messages[0].NewLives, "Incorrect new lives in message2.");
        }

        #endregion Lives

        #region Health

        [TestCase(0f, 0.1f)]
        [TestCase(0.2f, 0.3f)]
        [TestCase(0f, 1f)]
        public void HealthMessageSent(float health1, float health2)
        {
            //// Arrange
            List<HealthChangedMessage> messages = new List<HealthChangedMessage>();
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);
            player.Health = health1;
            messenger.ProcessQueue();               // clear queue incase initialisation generated a message.
            messenger.AddListener(typeof(HealthChangedMessage), (x) => {
                messages.Add(x as HealthChangedMessage);
                return true;
            });

            //// Act
            player.Health = health2;
            messenger.ProcessQueue();   // force processing of messages.

            //// Assert
            Assert.AreEqual(1, messages.Count, "Incorrect number of messages sent.");
            Assert.AreEqual(health1, messages[0].OldHealth, "Incorrect old health in message2.");
            Assert.AreEqual(health2, messages[0].NewHealth, "Incorrect new health in message2.");
        }

        #endregion Health

        #region Counters

        [Test]
        public void CounterInitialisationDefaults()
        {
            //// Arrange
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();

            //// Act
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);

            //// Assert
            Assert.AreNotEqual(-1, player.GetCounterIndex("Score"), "Score counter not setup.");
            Assert.AreNotEqual(-1, player.GetCounterIndex("Coins"), "Coins counter not setup.");
        }

        [TestCase("Test")]
        [TestCase("Test2")]
        public void CounterInitialisationSpecifiedCounters(string counterKey)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            gameConfiguration.PlayerCounterConfiguration.Add(new CounterConfiguration() { Name = counterKey });
            var messenger = new Messenger();

            //// Act
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);

            //// Assert
            Assert.AreNotEqual(-1, player.GetCounterIndex("Score"), "Score counter not setup.");
            Assert.AreNotEqual(-1, player.GetCounterIndex("Coins"), "Coins counter not setup.");
            Assert.AreNotEqual(-1, player.GetCounterIndex(counterKey), "Test counter not setup.");
        }
        #endregion Counters
    }
}
#endif
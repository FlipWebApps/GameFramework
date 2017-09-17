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
using GameFramework.GameStructure.Players.ObjectModel;
using GameFramework.Localisation.ObjectModel;
using GameFramework.Messaging;
using NUnit.Framework;
using UnityEngine;
using GameFramework.GameStructure.Game.ObjectModel;
using GameFramework.GameStructure.GameItems.Messages;
using System.Collections.Generic;

namespace GameFramework.GameStructure.GameItems
{
    /// <summary>
    /// Test cases for GameItems. You can also view these to see how you might use the API.
    /// TODO:
    /// Test persisted values loaded
    /// </summary>
    public class GameItemTests
    {

        #region Helper functions for verifying testing

        internal static void SetCommonPreferences(int playerNumber, int number, string identifierBasePrefs, int highScore, bool isUnlocked, bool isUnlockedAnimationShown, bool isBought)
        {
            PlayerPrefs.SetInt(string.Format("P{0}.{1}{2}.HS", playerNumber, identifierBasePrefs, number), highScore);
            PlayerPrefs.SetInt(string.Format("P{0}.{1}{2}.IsU", playerNumber, identifierBasePrefs, number), isUnlocked ? 1 : 0);
            PlayerPrefs.SetInt(string.Format("P{0}.{1}{2}.IsUAS", playerNumber, identifierBasePrefs, number), isUnlockedAnimationShown ? 1 : 0);
            PlayerPrefs.SetInt(string.Format("{0}{1}.IsB", identifierBasePrefs, number), isBought ? 1 : 0);
        }

        internal static void AssertCommonPreferences(int playerNumber, int number, string identifierBasePrefs, GameItem gameItem)
        {
            Assert.AreEqual(PlayerPrefs.GetInt(string.Format("P{0}.{1}{2}.HS", playerNumber, identifierBasePrefs, number), 0), gameItem.HighScore, "HighScore not set correctly");
            // note for the below, unlocked is changed by setting bought - we should have a test for this somewhere... 
            Assert.AreEqual(PlayerPrefs.GetInt(string.Format("P{0}.{1}{2}.IsU", playerNumber, identifierBasePrefs, number), 0) == 1, gameItem.IsUnlocked, "IsUnlocked not set correctly when not bought");
            Assert.AreEqual(PlayerPrefs.GetInt(string.Format("P{0}.{1}{2}.IsUAS", playerNumber, identifierBasePrefs, number), 0) == 1, gameItem.IsUnlockedAnimationShown, "IsUnlockedAnimationShown not set correctly");
            Assert.AreEqual(PlayerPrefs.GetInt(string.Format("{0}{1}.IsB", identifierBasePrefs, number), 0) == 1, gameItem.IsBought, "IsBought not set correctly");
        }

        #endregion Helper functions for verifying testing

        #region Initialisation

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void InitialisationDefaults(int number)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);

            //// Act
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, number);

            //// Assert
            Assert.IsNotNull(gameItem, "GameItem not setup.");
            Assert.AreEqual(number, gameItem.Number, "Number not set correctly");
            //TODO: Verify if we can test the below, or if localisation setup will interfere?
            //Assert.AreEqual("Name", gameItem.Name, "Name not set correctly");
            //Assert.AreEqual("Desc", gameItem.Description, "Description not set correctly");
            Assert.AreEqual("", gameItem.IdentifierBase, "IdentifierBase not set correctly");
            Assert.AreEqual("", gameItem.IdentifierBasePrefs, "IdentifierBasePrefs not set correctly");
            Assert.AreEqual(0, gameItem.Score, "Score not set correctly");
            Assert.AreEqual(0, gameItem.Coins, "Coins not set correctly");
            Assert.AreEqual(0, gameItem.HighScore, "HighScore not set correctly");
            Assert.AreEqual(false, gameItem.IsBought, "IsBought not set correctly");
            Assert.AreEqual(false, gameItem.IsUnlocked, "IsUnlocked not set correctly");
            Assert.AreEqual(false, gameItem.IsUnlockedAnimationShown, "IsUnlockedAnimationShown not set correctly");
        }


        [TestCase(1, "Name", "Desc", "Test", "T")]
        [TestCase(2, "Name2", "Desc2", "Another", "A")]
        public void InitialisationBasicSpecifiedValues(int number, string name, string desc, string identifierBase, string identifierBasePrefs)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);

            //// Act
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger,
                number, LocalisableText.CreateNonLocalised(name), LocalisableText.CreateNonLocalised(desc), 
                identifierBase: identifierBase, identifierBasePrefs: identifierBasePrefs);

            //// Assert
            Assert.IsNotNull(gameItem, "GameItem not setup.");
            Assert.AreEqual(number, gameItem.Number, "Number not set correctly");
            Assert.AreEqual(name, gameItem.Name, "Name not set correctly");
            Assert.AreEqual(desc, gameItem.Description, "Description not set correctly");
            Assert.AreEqual(identifierBase, gameItem.IdentifierBase, "IdentifierBase not set correctly");
            Assert.AreEqual(identifierBasePrefs, gameItem.IdentifierBasePrefs, "IdentifierBasePrefs not set correctly");
            Assert.AreEqual(0, gameItem.Score, "Score not set correctly");
            Assert.AreEqual(0, gameItem.Coins, "Coins not set correctly");
            Assert.AreEqual(0, gameItem.HighScore, "HighScore not set correctly");
            Assert.AreEqual(false, gameItem.IsBought, "IsBought not set correctly");
            Assert.AreEqual(false, gameItem.IsUnlocked, "IsUnlocked not set correctly");
            Assert.AreEqual(false, gameItem.IsUnlockedAnimationShown, "IsUnlockedAnimationShown not set correctly");
        }


        [TestCase(1)]
        [TestCase(2)]
        public void InitialisationStartUnlocked(int number)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);

            //// Act
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.StartUnlocked = true;
            gameItem.Initialise(gameConfiguration, player, messenger, number);

            //// Assert
            Assert.AreEqual(true, gameItem.IsUnlocked, "IsUnlocked not set correctly");
            Assert.AreEqual(true, gameItem.IsUnlockedAnimationShown, "IsUnlockedAnimationShown not set correctly");
        }


        /// <summary>
        /// Seperate test from creating, saving and then loadina GameItem to verify the consistency of saved preferences
        /// across different versions of the framework (that we use the same preferences keys).
        /// </summary>
        [TestCase(0, 1, "Test", "T", 10, false, false, false)]
        [TestCase(1, 2, "Another", "A", 10, false, false, false)]
        [TestCase(0, 1, "Test", "T", 20, false, false, false)]
        [TestCase(0, 1, "Test", "T", 20, true, false, false)]
        [TestCase(0, 1, "Test", "T", 20, false, true, false)]
        [TestCase(0, 1, "Test", "T", 20, false, false, true)]
        public void PersistentValuesLoaded(int playerNumber, int number, string identifierBase, string identifierBasePrefs, 
            int highScore, bool isUnlocked, bool isUnlockedAnimationShown, bool isBought)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();
            SetCommonPreferences(playerNumber, number, identifierBasePrefs, highScore, isUnlocked, isUnlockedAnimationShown, isBought);
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, playerNumber);

            //// Act
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger,
                number, identifierBase: identifierBase, identifierBasePrefs: identifierBasePrefs);

            //// Assert
            Assert.IsNotNull(gameItem, "GameItem not setup.");
            Assert.AreEqual(highScore, gameItem.HighScore, "HighScore not set correctly");
            Assert.AreEqual(isBought, gameItem.IsBought, "IsBought not set correctly");
            if (isBought)
                Assert.AreEqual(true, gameItem.IsUnlocked, "IsUnlocked not set correctly when bought");
            else
                Assert.AreEqual(isUnlocked, gameItem.IsUnlocked, "IsUnlocked not set correctly when not bought");
            Assert.AreEqual(isUnlockedAnimationShown, gameItem.IsUnlockedAnimationShown, "IsUnlockedAnimationShown not set correctly");
        }


        /// <summary>
        /// Seperate test from creating, saving and then loadina GameItem to verify the consistency of saved preferences
        /// across different versions of the framework (that we use the same preferences keys).
        /// </summary>
        [TestCase(0, 1, "Test", "T", 10, false, false, false)]
        [TestCase(1, 2, "Another", "A", 10, false, false, false)]
        [TestCase(0, 1, "Test", "T", 20, false, false, false)]
        [TestCase(0, 1, "Test", "T", 20, true, false, false)]
        [TestCase(0, 1, "Test", "T", 20, false, true, false)]
        [TestCase(0, 1, "Test", "T", 20, false, false, true)]
        public void PersistentValuesSaved(int playerNumber, int number, string identifierBase, string identifierBasePrefs,
            int highScore, bool isUnlocked, bool isUnlockedAnimationShown, bool isBought)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, playerNumber);

            //// Act
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger,
                number, identifierBase: identifierBase, identifierBasePrefs: identifierBasePrefs);
            gameItem.Score = highScore; // score should set high score automatically which is saved
            //gameItem.HighScore = highScore;
            gameItem.IsUnlocked = isUnlocked;
            gameItem.IsUnlockedAnimationShown = isUnlockedAnimationShown;
            gameItem.IsBought = isBought;
            gameItem.UpdatePlayerPrefs();
            PlayerPrefs.Save();

            //// Assert
            Assert.IsNotNull(gameItem, "GameItem not setup.");
            AssertCommonPreferences(playerNumber, number, identifierBasePrefs, gameItem);
        }

        #endregion Initialisation

        #region Score

        [TestCase(0, 10)]
        [TestCase(20, 30)]
        [TestCase(30, 40)]
        public void ScoreMessageSent(int score1, int score2)
        {
            //// Arrange
            List<ScoreChangedMessage> messages = new List<ScoreChangedMessage>();
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);
            gameItem.Score = score1;
            messenger.ProcessQueue();               // clear queue incase initialisation generated a message.
            messenger.AddListener(typeof(ScoreChangedMessage), (x) => {
                messages.Add(x as ScoreChangedMessage);
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


        [TestCase(0)]
        [TestCase(20)]
        [TestCase(30)]
        public void ScoreUnchangedNoMessageSent(int score)
        {
            //// Arrange
            List<ScoreChangedMessage> messages = new List<ScoreChangedMessage>();
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);
            gameItem.Score = score;
            messenger.ProcessQueue();               // clear queue incase initialisation generated a message.
            messenger.AddListener(typeof(ScoreChangedMessage), (x) => {
                messages.Add(x as ScoreChangedMessage);
                return true;
            });

            //// Act
            gameItem.Score = score;
            messenger.ProcessQueue();   // force processing of messages.

            //// Assert
            Assert.AreEqual(0, messages.Count, "Incorrect number of messages sent.");
        }


        /// <summary>
        /// Expected score to allow for testing of any limits.
        /// </summary>
        [TestCase(20)]
        [TestCase(30)]
        public void ScoreAddPoint(int score)
        {
            //// Arrange
            int expectedScore = score + 1;
            List<ScoreChangedMessage> messages = new List<ScoreChangedMessage>();
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);
            gameItem.Score = score;                 // initialise
            messenger.ProcessQueue();               // clear queue incase initialisation generated a message.
            messenger.AddListener(typeof(ScoreChangedMessage), (x) => {
                messages.Add(x as ScoreChangedMessage);
                return true;
            });

            //// Act
            gameItem.AddPoint();
            messenger.ProcessQueue();   // force processing of messages.

            //// Assert
            Assert.AreEqual(expectedScore, gameItem.Score, "Score is incorrect.");
            Assert.AreEqual(1, messages.Count, "Incorrect number of messages sent.");
            Assert.AreEqual(score, messages[0].OldScore, "Incorrect old score in message2.");
            Assert.AreEqual(expectedScore, messages[0].NewScore, "Incorrect new score in message2.");
        }


        /// <summary>
        /// Expected score to allow for testing of any limits.
        /// </summary>
        [TestCase(20, 10)]
        [TestCase(30, 5)]
        [TestCase(100, -10)]
        public void ScoreAddPoints(int score, int pointsToAdd)
        {
            //// Arrange
            int expectedScore = score + pointsToAdd;
            List<ScoreChangedMessage> messages = new List<ScoreChangedMessage>();
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);
            gameItem.Score = score;                 // initialise
            messenger.ProcessQueue();               // clear queue incase initialisation generated a message.
            messenger.AddListener(typeof(ScoreChangedMessage), (x) => {
                messages.Add(x as ScoreChangedMessage);
                return true;
            });

            //// Act
            gameItem.AddPoints(pointsToAdd);
            messenger.ProcessQueue();   // force processing of messages.

            //// Assert
            Assert.AreEqual(expectedScore, gameItem.Score, "Score is incorrect.");
            Assert.AreEqual(1, messages.Count, "Incorrect number of messages sent.");
            Assert.AreEqual(score, messages[0].OldScore, "Incorrect old score in message2.");
            Assert.AreEqual(expectedScore, messages[0].NewScore, "Incorrect new score in message2.");
        }


        /// <summary>
        /// Expected score to allow for testing of any limits.
        /// </summary>
        [TestCase(20)]
        [TestCase(30)]
        public void ScoreRemovePoint(int score)
        {
            //// Arrange
            int expectedScore = score - 1;
            List<ScoreChangedMessage> messages = new List<ScoreChangedMessage>();
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);
            gameItem.Score = score;                 // initialise
            messenger.ProcessQueue();               // clear queue incase initialisation generated a message.
            messenger.AddListener(typeof(ScoreChangedMessage), (x) => {
                messages.Add(x as ScoreChangedMessage);
                return true;
            });

            //// Act
            gameItem.RemovePoint();
            messenger.ProcessQueue();   // force processing of messages.

            //// Assert
            Assert.AreEqual(expectedScore, gameItem.Score, "Score is incorrect.");
            Assert.AreEqual(1, messages.Count, "Incorrect number of messages sent.");
            Assert.AreEqual(score, messages[0].OldScore, "Incorrect old score in message2.");
            Assert.AreEqual(expectedScore, messages[0].NewScore, "Incorrect new score in message2.");
        }


        /// <summary>
        /// Expected score to allow for testing of any limits.
        /// </summary>
        [TestCase(20, 10)]
        [TestCase(30, 5)]
        [TestCase(100, -10)]
        public void ScoreRemovePoints(int score, int pointsToRemove)
        {
            //// Arrange
            int expectedScore = score - pointsToRemove;
            List<ScoreChangedMessage> messages = new List<ScoreChangedMessage>();
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);
            gameItem.Score = score;                 // initialise
            messenger.ProcessQueue();               // clear queue incase initialisation generated a message.
            messenger.AddListener(typeof(ScoreChangedMessage), (x) => {
                messages.Add(x as ScoreChangedMessage);
                return true;
            });

            //// Act
            gameItem.RemovePoints(pointsToRemove);
            messenger.ProcessQueue();   // force processing of messages.

            //// Assert
            Assert.AreEqual(expectedScore, gameItem.Score, "Score is incorrect.");
            Assert.AreEqual(1, messages.Count, "Incorrect number of messages sent.");
            Assert.AreEqual(score, messages[0].OldScore, "Incorrect old score in message2.");
            Assert.AreEqual(expectedScore, messages[0].NewScore, "Incorrect new score in message2.");
        }


        /// <summary>
        /// Expected score to allow for testing of any limits.
        /// </summary>
        [TestCase(-1)]
        [TestCase(-10)]
        public void ScoreCantBeNegative(int score)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var player = ScriptableObject.CreateInstance<Player>();
            var messenger = new Messenger();
            player.Initialise(gameConfiguration, null, messenger, 1);
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);

            //// Act
            gameItem.Score = score;

            //// Assert
            Assert.AreEqual(0, gameItem.Score, "Score is incorrect.");
        }

        [TestCase(0, 10)]
        [TestCase(20, 30)]
        [TestCase(30, 40)]
        public void HighScoreMessageSent(int score1, int score2)
        {
            //// Arrange
            List<HighScoreChangedMessage> messages = new List<HighScoreChangedMessage>();
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);
            gameItem.Score = score1;
            messenger.ProcessQueue();               // clear queue incase initialisation generated a message.
            messenger.AddListener(typeof(HighScoreChangedMessage), (x) => {
                messages.Add(x as HighScoreChangedMessage);
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


        [TestCase(0, 0)]
        [TestCase(20, 10)]
        [TestCase(30, 0)]
        [TestCase(40, 40)]
        public void HighScoreUnchangedNoMessageSent(int score1, int score2)
        {
            //// Arrange
            List<HighScoreChangedMessage> messages = new List<HighScoreChangedMessage>();
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);
            gameItem.Score = score1;
            messenger.ProcessQueue();               // clear queue incase initialisation generated a message.
            messenger.AddListener(typeof(HighScoreChangedMessage), (x) => {
                messages.Add(x as HighScoreChangedMessage);
                return true;
            });

            //// Act
            gameItem.Score = score2;
            messenger.ProcessQueue();   // force processing of messages.

            //// Assert
            Assert.AreEqual(0, messages.Count, "Incorrect number of messages sent.");
        }
        #endregion

        #region Coins

        [TestCase(0, 10)]
        [TestCase(20, 30)]
        [TestCase(30, 40)]
        public void CoinsMessageSent(int coins1, int coins2)
        {
            //// Arrange
            List<CoinsChangedMessage> messages = new List<CoinsChangedMessage>();
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);
            gameItem.Coins = coins1;
            messenger.ProcessQueue();               // clear queue incase initialisation generated a message.
            messenger.AddListener(typeof(CoinsChangedMessage), (x) => {
                messages.Add(x as CoinsChangedMessage);
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


        [TestCase(0)]
        [TestCase(20)]
        [TestCase(30)]
        public void CoinsUnchangedNoMessageSent(int coins)
        {
            //// Arrange
            List<CoinsChangedMessage> messages = new List<CoinsChangedMessage>();
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);
            gameItem.Coins = coins;
            messenger.ProcessQueue();               // clear queue incase initialisation generated a message.
            messenger.AddListener(typeof(CoinsChangedMessage), (x) => {
                messages.Add(x as CoinsChangedMessage);
                return true;
            });

            //// Act
            gameItem.Coins = coins;
            messenger.ProcessQueue();   // force processing of messages.

            //// Assert
            Assert.AreEqual(0, messages.Count, "Incorrect number of messages sent.");
        }


        /// <summary>
        /// Expected coins to allow for testing of any limits.
        /// </summary>
        [TestCase(20)]
        [TestCase(30)]
        public void CoinsAddCoin(int coins)
        {
            //// Arrange
            int expectedCoins = coins + 1;
            List<CoinsChangedMessage> messages = new List<CoinsChangedMessage>();
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);
            gameItem.Coins = coins;                 // initialise
            messenger.ProcessQueue();               // clear queue incase initialisation generated a message.
            messenger.AddListener(typeof(CoinsChangedMessage), (x) => {
                messages.Add(x as CoinsChangedMessage);
                return true;
            });

            //// Act
            gameItem.AddCoin();
            messenger.ProcessQueue();   // force processing of messages.

            //// Assert
            Assert.AreEqual(expectedCoins, gameItem.Coins, "Coins is incorrect.");
            Assert.AreEqual(1, messages.Count, "Incorrect number of messages sent.");
            Assert.AreEqual(coins, messages[0].OldCoins, "Incorrect old coins in message2.");
            Assert.AreEqual(expectedCoins, messages[0].NewCoins, "Incorrect new coins in message2.");
        }


        /// <summary>
        /// Expected coins to allow for testing of any limits.
        /// </summary>
        [TestCase(20, 10)]
        [TestCase(30, 5)]
        [TestCase(100, -10)]
        public void CoinsAddCoins(int coins, int pointsToAdd)
        {
            //// Arrange
            int expectedCoins = coins + pointsToAdd;
            List<CoinsChangedMessage> messages = new List<CoinsChangedMessage>();
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);
            gameItem.Coins = coins;                 // initialise
            messenger.ProcessQueue();               // clear queue incase initialisation generated a message.
            messenger.AddListener(typeof(CoinsChangedMessage), (x) => {
                messages.Add(x as CoinsChangedMessage);
                return true;
            });

            //// Act
            gameItem.AddCoins(pointsToAdd);
            messenger.ProcessQueue();   // force processing of messages.

            //// Assert
            Assert.AreEqual(expectedCoins, gameItem.Coins, "Coins is incorrect.");
            Assert.AreEqual(1, messages.Count, "Incorrect number of messages sent.");
            Assert.AreEqual(coins, messages[0].OldCoins, "Incorrect old coins in message2.");
            Assert.AreEqual(expectedCoins, messages[0].NewCoins, "Incorrect new coins in message2.");
        }


        /// <summary>
        /// Expected coins to allow for testing of any limits.
        /// </summary>
        [TestCase(20)]
        [TestCase(30)]
        public void CoinsRemoveCoin(int coins)
        {
            //// Arrange
            int expectedCoins = coins - 1;
            List<CoinsChangedMessage> messages = new List<CoinsChangedMessage>();
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);
            gameItem.Coins = coins;                 // initialise
            messenger.ProcessQueue();               // clear queue incase initialisation generated a message.
            messenger.AddListener(typeof(CoinsChangedMessage), (x) => {
                messages.Add(x as CoinsChangedMessage);
                return true;
            });

            //// Act
            gameItem.RemoveCoin();
            messenger.ProcessQueue();   // force processing of messages.

            //// Assert
            Assert.AreEqual(expectedCoins, gameItem.Coins, "Coins is incorrect.");
            Assert.AreEqual(1, messages.Count, "Incorrect number of messages sent.");
            Assert.AreEqual(coins, messages[0].OldCoins, "Incorrect old coins in message2.");
            Assert.AreEqual(expectedCoins, messages[0].NewCoins, "Incorrect new coins in message2.");
        }


        /// <summary>
        /// Expected coins to allow for testing of any limits.
        /// </summary>
        [TestCase(20, 10)]
        [TestCase(30, 5)]
        [TestCase(100, -10)]
        public void CoinsRemoveCoins(int coins, int pointsToRemove)
        {
            //// Arrange
            int expectedCoins = coins - pointsToRemove;
            List<CoinsChangedMessage> messages = new List<CoinsChangedMessage>();
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);
            gameItem.Coins = coins;                 // initialise
            messenger.ProcessQueue();               // clear queue incase initialisation generated a message.
            messenger.AddListener(typeof(CoinsChangedMessage), (x) => {
                messages.Add(x as CoinsChangedMessage);
                return true;
            });

            //// Act
            gameItem.RemoveCoins(pointsToRemove);
            messenger.ProcessQueue();   // force processing of messages.

            //// Assert
            Assert.AreEqual(expectedCoins, gameItem.Coins, "Coins is incorrect.");
            Assert.AreEqual(1, messages.Count, "Incorrect number of messages sent.");
            Assert.AreEqual(coins, messages[0].OldCoins, "Incorrect old coins in message2.");
            Assert.AreEqual(expectedCoins, messages[0].NewCoins, "Incorrect new coins in message2.");
        }


        /// <summary>
        /// Expected coins to allow for testing of any limits.
        /// </summary>
        [TestCase(-1)]
        [TestCase(-10)]
        public void CoinsCantBeNegative(int coins)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var player = ScriptableObject.CreateInstance<Player>();
            var messenger = new Messenger();
            player.Initialise(gameConfiguration, null, messenger, 1);
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);

            //// Act
            gameItem.Coins = coins;

            //// Assert
            Assert.AreEqual(0, gameItem.Coins, "Coins is incorrect.");
        }

        #endregion

        #region Unlocking

        [TestCase(1)]
        [TestCase(2)]
        public void IsBoughtSetsUnlocked(int number)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, number);

            //// Act
            gameItem.IsBought = true;

            //// Assert
            Assert.AreEqual(true, gameItem.IsUnlocked, "IsUnlocked not set correctly");
            Assert.AreEqual(false, gameItem.IsUnlockedAnimationShown, "IsUnlockedAnimationShown not set correctly");
        }


        #endregion Unlocking

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
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);

            //// Assert
            Assert.AreEqual("Score", gameItem.GetCounter("Score").Configuration.Name, "Score counter not setup.");
            Assert.AreEqual("Coins", gameItem.GetCounter("Coins").Configuration.Name, "Coins counter not setup.");
        }

        [TestCase("NewCounter")]
        [TestCase("AnotherCounter")]
        public void CounterInitialisationCustom(string counterKey)
        {
            //// Arrange
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            gameConfiguration.DefaultGameItemCounterConfiguration.Add(new CounterConfiguration()
            {
                Name = counterKey
            });
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);

            //// Act
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);

            //// Assert
            Assert.AreNotEqual(-1, gameItem.GetCounterIndex(counterKey), "Counter index not found.");
            Assert.IsNotNull(gameItem.GetCounter(gameItem.GetCounterIndex(counterKey)), "Counter not found from index.");
            Assert.IsNotNull(gameItem.GetCounter(counterKey), "Counter not found from name.");
            Assert.AreEqual(counterKey, gameItem.GetCounter(counterKey).Configuration.Name, "Counter not setup.");
        }


        [TestCase("NewCounter", 0, 10)]
        [TestCase("NewCounter", 20, 30)]
        [TestCase("AnotherCounter", 30, 40)]
        public void CounterIntAmountChangedMessageSent(string counterKey, int amount1, int amount2)
        {
            //// Arrange
            List<CounterIntAmountChangedMessage> messages = new List<CounterIntAmountChangedMessage>();
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            gameConfiguration.DefaultGameItemCounterConfiguration.Add(new CounterConfiguration()
            {
                Name = counterKey
            });
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);
            gameItem.GetCounter(counterKey).Set(amount1);
            messenger.ProcessQueue();               // clear queue incase initialisation generated a message.
            messenger.AddListener(typeof(CounterIntAmountChangedMessage), (x) => {
                messages.Add(x as CounterIntAmountChangedMessage);
                return true;
            });

            //// Act
            gameItem.GetCounter(counterKey).Set(amount2);
            messenger.ProcessQueue();   // force processing of messages.

            //// Assert
            Assert.AreEqual(1, messages.Count, "Incorrect number of messages sent.");
            Assert.AreEqual(amount1, messages[0].OldAmount, "Incorrect old amount in message.");
            Assert.AreEqual(amount2, messages[0].NewAmount, "Incorrect new amount in message.");
        }


        [TestCase("NewCounter", 0, 10)]
        [TestCase("NewCounter", 20, 30)]
        [TestCase("AnotherCounter", 30, 40)]
        public void CounterIntAmountBestChangedMessageSent(string counterKey, int amount1, int amount2)
        {
            //// Arrange
            List<CounterIntAmountBestChangedMessage> messages = new List<CounterIntAmountBestChangedMessage>();
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            gameConfiguration.DefaultGameItemCounterConfiguration.Add(new CounterConfiguration()
            {
                Name = counterKey
            });
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);
            gameItem.GetCounter(counterKey).Set(amount1);
            messenger.ProcessQueue();               // clear queue incase initialisation generated a message.
            messenger.AddListener(typeof(CounterIntAmountBestChangedMessage), (x) => {
                messages.Add(x as CounterIntAmountBestChangedMessage);
                return true;
            });

            //// Act
            gameItem.GetCounter(counterKey).Set(amount2);
            messenger.ProcessQueue();   // force processing of messages.

            //// Assert
            Assert.AreEqual(1, messages.Count, "Incorrect number of messages sent.");
            Assert.AreEqual(amount1, messages[0].OldAmount, "Incorrect old amount in message.");
            Assert.AreEqual(amount2, messages[0].NewAmount, "Incorrect new amount in message.");
        }


        [TestCase("NewCounter", 0, 10)]
        [TestCase("NewCounter", 20, 30)]
        [TestCase("AnotherCounter", 30, 40)]
        public void CounterFloatAmountChangedMessageSent(string counterKey, float amount1, float amount2)
        {
            //// Arrange
            List<CounterFloatAmountChangedMessage> messages = new List<CounterFloatAmountChangedMessage>();
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            gameConfiguration.DefaultGameItemCounterConfiguration.Add(new CounterConfiguration()
            {
                Name = counterKey,
                CounterType = CounterConfiguration.CounterTypeEnum.Float
            });
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);
            gameItem.GetCounter(counterKey).Set(amount1);
            messenger.ProcessQueue();               // clear queue incase initialisation generated a message.
            messenger.AddListener(typeof(CounterFloatAmountChangedMessage), (x) => {
                messages.Add(x as CounterFloatAmountChangedMessage);
                return true;
            });

            //// Act
            gameItem.GetCounter(counterKey).Set(amount2);
            messenger.ProcessQueue();   // force processing of messages.

            //// Assert
            Assert.AreEqual(1, messages.Count, "Incorrect number of messages sent.");
            Assert.AreEqual(amount1, messages[0].OldAmount, "Incorrect old amount in message.");
            Assert.AreEqual(amount2, messages[0].NewAmount, "Incorrect new amount in message.");
        }


        [TestCase("NewCounter", 0, 10)]
        [TestCase("NewCounter", 20, 30)]
        [TestCase("AnotherCounter", 30, 40)]
        public void CounterFloatAmountBestChangedMessageSent(string counterKey, float amount1, float amount2)
        {
            //// Arrange
            List<CounterFloatAmountBestChangedMessage> messages = new List<CounterFloatAmountBestChangedMessage>();
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            gameConfiguration.DefaultGameItemCounterConfiguration.Add(new CounterConfiguration()
            {
                Name = counterKey,
                CounterType = CounterConfiguration.CounterTypeEnum.Float
            });
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);
            gameItem.GetCounter(counterKey).Set(amount1);
            messenger.ProcessQueue();               // clear queue incase initialisation generated a message.
            messenger.AddListener(typeof(CounterFloatAmountBestChangedMessage), (x) => {
                messages.Add(x as CounterFloatAmountBestChangedMessage);
                return true;
            });

            //// Act
            gameItem.GetCounter(counterKey).Set(amount2);
            messenger.ProcessQueue();   // force processing of messages.

            //// Assert
            Assert.AreEqual(1, messages.Count, "Incorrect number of messages sent.");
            Assert.AreEqual(amount1, messages[0].OldAmount, "Incorrect old amount in message.");
            Assert.AreEqual(amount2, messages[0].NewAmount, "Incorrect new amount in message.");
        }


        [TestCase("NewCounter", 0)]
        [TestCase("NewCounter", 20)]
        [TestCase("AnotherCounter", 30)]
        public void CounterIntAmountChangedMessageNotSentBeforeInitialised(string counterKey, int amount)
        {
            //// Arrange - setup and create some prefs data for later loading
            int messageCount = 0;
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            gameConfiguration.DefaultGameItemCounterConfiguration.Add(new CounterConfiguration()
            {
                Name = counterKey,
                Save = CounterConfiguration.SaveType.Always       // persist to true so we load / set defaults in initialise.
            });
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);

            // save some data to prefs.
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);
            gameItem.GetCounter(counterKey).Set(amount);
            gameItem.UpdatePlayerPrefs();
            PlayerPrefs.Save();

            messenger.ProcessQueue();   // force processing of messages.

            //// Act
            messenger.AddListener(typeof(CounterIntAmountChangedMessage), (x) => {
                messageCount++;
                return true;
            });

            gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);

            messenger.ProcessQueue();   // force processing of messages.

            //// Assert
            Assert.AreEqual(0, messageCount, "No messages should have been sent.");
        }


        [TestCase("NewCounter", 0)]
        [TestCase("NewCounter", 20)]
        [TestCase("AnotherCounter", 30)]
        public void CounterIntAmountBestChangedMessageNotSentBeforeInitialised(string counterKey, int amount)
        {
            //// Arrange - setup and create some prefs data for later loading
            int messageCount = 0;
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            gameConfiguration.DefaultGameItemCounterConfiguration.Add(new CounterConfiguration()
            {
                Name = counterKey,
                Save = CounterConfiguration.SaveType.Always       // persist to true so we load / set defaults in initialise.
            });
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);

            // save some data to prefs.
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);
            gameItem.GetCounter(counterKey).Set(amount);
            gameItem.UpdatePlayerPrefs();
            PlayerPrefs.Save();

            messenger.ProcessQueue();   // force processing of messages.

            //// Act
            messenger.AddListener(typeof(CounterIntAmountBestChangedMessage), (x) => {
                messageCount++;
                return true;
            });

            gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);

            messenger.ProcessQueue();   // force processing of messages.

            //// Assert
            Assert.AreEqual(0, messageCount, "No messages should have been sent.");
        }


        [TestCase("NewCounter", 0)]
        [TestCase("NewCounter", 20)]
        [TestCase("AnotherCounter", 30)]
        public void CounterFloatAmountChangedMessageNotSentBeforeInitialised(string counterKey, float amount)
        {
            //// Arrange - setup and create some prefs data for later loading
            int messageCount = 0;
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            gameConfiguration.DefaultGameItemCounterConfiguration.Add(new CounterConfiguration()
            {
                Name = counterKey,
                Save = CounterConfiguration.SaveType.Always,       // persist to true so we load / set defaults in initialise.
                CounterType = CounterConfiguration.CounterTypeEnum.Float
            });
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);

            // save some data to prefs.
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);
            gameItem.GetCounter(counterKey).Set(amount);
            gameItem.UpdatePlayerPrefs();
            PlayerPrefs.Save();

            messenger.ProcessQueue();   // force processing of messages.

            //// Act - add listener, create new game item that will try and load prefs
            messenger.AddListener(typeof(CounterFloatAmountChangedMessage), (x) => {
                messageCount++;
                return true;
            });

            gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);

            messenger.ProcessQueue();   // force processing of messages.

            //// Assert
            Assert.AreEqual(0, messageCount, "No messages should have been sent.");
        }


        [TestCase("NewCounter", 0)]
        [TestCase("NewCounter", 20)]
        [TestCase("AnotherCounter", 30)]
        public void CounterFloatAmountBestChangedMessageNotSentBeforeInitialised(string counterKey, float amount)
        {
            //// Arrange - setup and create some prefs data for later loading
            int messageCount = 0;
            PlayerPrefs.DeleteAll();
            var gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();
            gameConfiguration.DefaultGameItemCounterConfiguration.Add(new CounterConfiguration()
            {
                Name = counterKey,
                Save = CounterConfiguration.SaveType.Always,       // persist to true so we load / set defaults in initialise.
                CounterType = CounterConfiguration.CounterTypeEnum.Float
            });
            var messenger = new Messenger();
            var player = ScriptableObject.CreateInstance<Player>();
            player.Initialise(gameConfiguration, null, messenger, 1);

            // save some data to prefs.
            var gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);
            gameItem.GetCounter(counterKey).Set(amount);
            gameItem.UpdatePlayerPrefs();
            PlayerPrefs.Save();

            messenger.ProcessQueue();   // force processing of messages.

            //// Act - add listener, create new game item that will try and load prefs
            messenger.AddListener(typeof(CounterFloatAmountBestChangedMessage), (x) => {
                messageCount++;
                return true;
            });

            gameItem = ScriptableObject.CreateInstance<GameItem>();
            gameItem.Initialise(gameConfiguration, player, messenger, 1);

            messenger.ProcessQueue();   // force processing of messages.

            //// Assert
            Assert.AreEqual(0, messageCount, "No messages should have been sent.");
        }

        #endregion Counters
    }
}
#endif
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

namespace GameFramework.GameStructure.GameItems
{
    /// <summary>
    /// Test cases for messaging. You can also view these to see how you might use the API.
    /// </summary>
    public class GameItemManagerTests
    {

        #region Helper functions for verifying testing

        #endregion Helper functions for verifying testing

        #region Initialisation

        [Test]
        public void BasicInitialisation()
        {
            //// Arrange
            var gameItemmanager = new GameItemManager<GameItem,GameItem>();

            //// Assert
            Assert.IsNotNull(gameItemmanager.Items, "The items array should be initialised.");
            Assert.AreEqual(gameItemmanager.Items.Length, 0, "The items array should be empty on initialisation.");
        }

        [Test]
        public void Load()
        {
            //    //// Arrange
            //    var gameItemmanager = new GameItemManager<GameItem, GameItem>();
            //    gameItemmanager.Items = new GameItem[5];
            //    for (var i = 0; i < 5; i++)
            //    {
            //        // preference is to load from resources.
            //        gameItemmanager.Items[i] = ScriptableObject.CreateInstance<GameItem>();
            //        gameItemmanager.Items[i].Initialise(i, LocalisableText.CreateLocalised(), LocalisableText.CreateLocalised(), valueToUnlock: 10, player: ScriptableObject.CreateInstance<Player>());
            //    }

            //    //// Assert
            //    Assert.IsNotNull(gameItemmanager.Items, "The items array should be initialised");
            //    Assert.AreEqual(gameItemmanager.Items.Length, 5, "The items array should hold 5 items.");
        }
        #endregion Initialisation

        #region Unlocking

        [Test]
        public void Unlocking()
        {
            ////// Arrange
            //var gameItemManager = new GameItemManager<GameItem, GameItem>();
            ////var messenger = new Messenger();
            ////_testHandlerCalled = false;
            ////messenger.AddListener<BaseMessage>(TestHandler);

            ////// Act
            ////messenger.TriggerMessage(new BaseMessage());

            ////// Assert
            //Assert.IsNotNull(gameItemmanager.Items, "The items array should be initialised");
            //Assert.AreEqual(gameItemmanager.Items.Length, 0, "The items array should be be empty on initialisation");

            ////// Cleanup
            ////messenger.RemoveListener<BaseMessage>(TestHandler);
        }

        #endregion Unlocking
    }
}
#endif
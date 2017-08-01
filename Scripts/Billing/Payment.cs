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

using GameFramework.Billing.Messages;
using GameFramework.Debugging;
using GameFramework.GameStructure;
using GameFramework.GameStructure.Characters.ObjectModel;
using GameFramework.GameStructure.Levels.ObjectModel;
using GameFramework.GameStructure.Worlds.ObjectModel;
using GameFramework.UI.Dialogs.Components;
using System;
using GameFramework.Preferences;
using GameFramework.GameStructure.GenericGameItems.ObjectModel;
using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.GameStructure.GenericGameItems.Components;
using GameFramework.Messaging;
using UnityEngine.Assertions;
using GameFramework.GameStructure.Game.ObjectModel;

/// <summary>
/// Extended support and integration of In App Purchasing.
/// </summary>
/// For further information please see: http://www.flipwebapps.com/unity-assets/game-framework/billing/
namespace GameFramework.Billing
{

    /// <summary>
    /// Helper functions for in app billing. See also the PaymentManager component that you should add to your scene.
    /// </summary>
    public class Payment
    {
        /// <summary>
        /// Process purchase of a given productId.
        /// </summary>
        /// This automatically handles certain types of purchase and notifications. It is called by PaymentManager but can also be
        /// called from code if needed.
        public static void ProcessPurchase(string productId)
        {
            MyDebug.Log(string.Format("ProcessPurchase: Product: '{0}'", productId));

            if (string.Equals(productId, "android.test.purchased", StringComparison.Ordinal))
            {
                DialogManager.Instance.ShowInfo("Test payment android.test.purchased purchased ok");
            }

            else if (productId.Equals("unlockgame"))
            {
                // update on GameManager
                if (GameManager.IsActive)
                    GameManager.Instance.IsUnlocked = true;

                // ensure it is saved
                PreferencesFactory.SetInt("IsUnlocked", 1);
                PreferencesFactory.Save();

                // notify all subscribers of the purchase
                GameManager.SafeQueueMessage(new UnlockGamePurchasedMessage());
            }

            else if (productId.StartsWith("unlock.world."))
                PurchaseGameItem<World>(productId, "unlock.world.", () => GameManager.Instance.Worlds, number => new WorldPurchasedMessage(number));

            else if (productId.StartsWith("unlock.level."))
                PurchaseGameItem<Level>(productId, "unlock.level.", () => GameManager.Instance.Levels, number => new LevelPurchasedMessage(number));

            else if (productId.StartsWith("unlock.character."))
                PurchaseGameItem<Character>(productId, "unlock.character.", () => GameManager.Instance.Characters, number => new CharacterPurchasedMessage(number));

            else if (productId.StartsWith("unlock.genericgameitem."))
                PurchaseGameItem<GenericGameItem>(productId, "unlock.genericgameitem.", () => GenericGameItemManager.Instance.GenericGameItems, number => new GenericGameItemPurchasedMessage(number));

            // finally send the generic påurchased message.
            GameManager.SafeQueueMessage(new ItemPurchasedMessage(productId));
        }


        /// <summary>
        /// Handle purchasing of a GameItem
        /// </summary>
        /// <typeparam name="TGameItem"></typeparam>
        /// <param name="productId"></param>
        /// <param name="key"></param>
        /// <param name="getGameItemManager"></param>
        /// <param name="createMessage"></param>
        static void PurchaseGameItem<TGameItem>(
            string productId,
            string key,
            Func<GameItemManager<TGameItem, GameItem>> getGameItemManager,
            Func<int, BaseMessage> createMessage) where TGameItem : GameItem, new()
        {
            Assert.IsTrue(productId.StartsWith(key), "Invalid product id found");

            int number = int.Parse(productId.Substring(key.Length));
            TGameItem multiPurposeGameItem = null;

            // first try and get from game manager
            if (GameManager.IsActive && getGameItemManager() != null)
                multiPurposeGameItem = getGameItemManager().GetItem(number);

            // if not found on game manager then create a new copy to ensure this purchase is recorded
            if (multiPurposeGameItem == null)
            {
                multiPurposeGameItem = new TGameItem();
                multiPurposeGameItem.Initialise(GameConfiguration.Instance, GameManager.Instance.Player, GameManager.Messenger, number);
            }

            // mark the item as bought and unlocked
            multiPurposeGameItem.MarkAndSaveAsBought();

            // notify all subscribers of the purchase
            GameManager.SafeQueueMessage(createMessage(number));
        }
    }
}

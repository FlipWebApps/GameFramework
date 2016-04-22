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

using FlipWebApps.GameFramework.Scripts.Billing.Messages;
using FlipWebApps.GameFramework.Scripts.Debugging;
using FlipWebApps.GameFramework.Scripts.GameStructure;
using FlipWebApps.GameFramework.Scripts.GameStructure.Characters.ObjectModel;
using FlipWebApps.GameFramework.Scripts.GameStructure.Levels.ObjectModel;
using FlipWebApps.GameFramework.Scripts.GameStructure.Worlds.ObjectModel;
using FlipWebApps.GameFramework.Scripts.UI.Dialogs.Components;
using System;
using FlipWebApps.GameFramework.Scripts.Billing.Components;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Billing
{

    /// <summary>
    /// Helpoer functions for in app billing. See also the PaymentManager component that you should add to your scene.
    /// </summary>
    public class Payment
    {
        /// <summary>
        /// Process purchase of a given productId. This automatically handles certain types of purchase and notifications
        /// </summary>
        public static void ProcessPurchase(string productId)
        {
            MyDebug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", productId));

            if (string.Equals(productId, "android.test.purchased", StringComparison.Ordinal))
            {
                DialogManager.Instance.ShowInfo("Test payment android.test.purchased purchased ok");
            }

            else if (productId.Equals("unlockgame"))
            {
                // update on GameManager
                if (GameManager.IsActive)
                    GameManager.Instance.IsUnlocked = true;
                PlayerPrefs.SetInt("IsUnlocked", 1);
                PlayerPrefs.Save();

                // notify all subscribers of the purchase
                GameManager.SafeQueueMessage(new UnlockGamePurchasedMessage());
#if UNITY_PURCHASING
                if (PaymentManager.IsActive && PaymentManager.Instance.UnlockGamePurchased != null)
                    PaymentManager.Instance.UnlockGamePurchased();          // deprecated.
#endif
            }

            else if (productId.StartsWith("unlock.world."))
            {
                int number = int.Parse(productId.Substring("unlock.world.".Length));
                World world = null;

                // first try and get from game manager
                if (GameManager.IsActive && GameManager.Instance.Worlds != null)
                    world = GameManager.Instance.Worlds.GetItem(number);

                // if not found on game manager then create a new copy to ensure this purchase is recorded
                if (world == null)
                {
                    world = new World();
                    world.Initialise(number);
                }

                // mark the item as bought and unlocked
                world.MarkAsBought();

                // notify all subscribers of the purchase
                GameManager.SafeQueueMessage(new WorldPurchasedMessage(number));
#if UNITY_PURCHASING
                if (PaymentManager.IsActive && PaymentManager.Instance.WorldPurchased != null)
                    PaymentManager.Instance.WorldPurchased(number);          // deprecated.
#endif
            }

            else if (productId.StartsWith("unlock.level."))
            {
                int number = int.Parse(productId.Substring("unlock.level.".Length));
                Level level = null;

                // first try and get from game manager
                if (GameManager.IsActive && GameManager.Instance.Levels != null)
                    level = GameManager.Instance.Levels.GetItem(number);

                // if not found on game manager then create a new copy to ensure this purchase is recorded
                if (level == null)
                {
                    level = new Level();
                    level.Initialise(number);
                }

                // mark the item as bought and unlocked
                level.MarkAsBought();

                // notify all subscribers of the purchase
                GameManager.SafeQueueMessage(new LevelPurchasedMessage(number));
#if UNITY_PURCHASING
                if (PaymentManager.IsActive && PaymentManager.Instance.LevelPurchased != null)
                    PaymentManager.Instance.LevelPurchased(number);          // deprecated.
#endif
            }

            else if (productId.StartsWith("unlock.character."))
            {
                int number = int.Parse(productId.Substring("unlock.character.".Length));
                Character character = null;

                // first try and get from game manager
                if (GameManager.IsActive && GameManager.Instance.Characters != null)
                    character = GameManager.Instance.Characters.GetItem(number);

                // if not found on game manager then create a new copy to ensure this purchase is recorded
                if (character == null)
                {
                    character = new Character();
                    character.Initialise(number);
                }

                // mark the item as bought and unlocked
                character.MarkAsBought();

                // notify all subscribers of the purchase
                GameManager.SafeQueueMessage(new CharacterPurchasedMessage(number));
#if UNITY_PURCHASING
                if (PaymentManager.IsActive && PaymentManager.Instance.CharacterPurchased != null)
                    PaymentManager.Instance.CharacterPurchased(number);          // deprecated.
#endif
            }

            // finally send the generic påurchased message.
            GameManager.SafeQueueMessage(new ItemPurchasedMessage(productId));
        }
    }
}

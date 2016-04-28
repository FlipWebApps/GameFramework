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

#if UNITY_PURCHASING
using FlipWebApps.GameFramework.Scripts.Billing.Components;
#endif
using System.Runtime.Remoting.Messaging;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.Components;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;
using FlipWebApps.GameFramework.Scripts.GameStructure.Worlds.ObjectModel;
using FlipWebApps.GameFramework.Scripts.UI.Other.Components;
using UnityEngine;
using FlipWebApps.GameFramework.Scripts.GameStructure.Levels.ObjectModel;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Worlds.Components
{
    /// <summary>
    /// World Details Button
    /// </summary>
    [AddComponentMenu("Game Framework/GameStructure/Worlds/WorldButton")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class WorldButton : GameItemButton<World>
    {
        public new void Awake()
        {
            base.Awake();

#if UNITY_PURCHASING
            if (PaymentManager.Instance != null)
                PaymentManager.Instance.WorldPurchased += UnlockIfNumberMatches;
#endif
        }

        protected new void OnDestroy()
        {
#if UNITY_PURCHASING
            if (PaymentManager.Instance != null)
                PaymentManager.Instance.WorldPurchased -= UnlockIfNumberMatches;
#endif

            base.OnDestroy();
        }

        protected override GameItemsManager<World, GameItem> GetGameItemsManager()
        {
            return GameManager.Instance.Worlds;
        }


        public override void ClickUnlocked()
        {
            if (GameManager.Instance.AutoCreateLevels)
            {
                int startLevel = GameManager.Instance.WorldLevelNumbers[CurrentItem.Number - 1].Min;
                int endLevel = GameManager.Instance.WorldLevelNumbers[CurrentItem.Number - 1].Max;
                GameManager.Instance.Levels = new GameItemsManager<Level, GameItem>();
                if (GameManager.Instance.LevelUnlockMode == GameItem.UnlockModeType.Coins)
                    GameManager.Instance.Levels.LoadDefaultItems(startLevel, endLevel, GameManager.Instance.CoinsToUnlockLevels);
                else
                    GameManager.Instance.Levels.LoadDefaultItems(startLevel, endLevel);
            }
            base.ClickUnlocked();
        }

    }
}
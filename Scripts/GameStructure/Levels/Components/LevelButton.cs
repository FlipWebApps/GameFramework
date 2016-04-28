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
using FlipWebApps.GameFramework.Scripts.GameObjects;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.Components;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;
using FlipWebApps.GameFramework.Scripts.GameStructure.Levels.ObjectModel;
using FlipWebApps.GameFramework.Scripts.UI.Other.Components;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Levels.Components
{
    /// <summary>
    /// Level Details Button
    /// </summary>
    [AddComponentMenu("Game Framework/GameStructure/Levels/LevelButton")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class LevelButton : GameItemButton<Level>
    {
        public new void Awake()
        {
            base.Awake();

#if UNITY_PURCHASING
            if (PaymentManager.Instance != null)
                PaymentManager.Instance.LevelPurchased += UnlockIfNumberMatches;
#endif
        }

        protected new void OnDestroy()
        {
#if UNITY_PURCHASING
            if (PaymentManager.Instance != null)
                PaymentManager.Instance.LevelPurchased -= UnlockIfNumberMatches;
#endif

            base.OnDestroy();
        }


        public override void SetupDisplay()
        {
            base.SetupDisplay();

            GameObjectHelper.SafeSetActive(StarsWonGameObject, CurrentItem.IsUnlocked);
            GameObjectHelper.SafeSetActive(Star1WonGameObject, CurrentItem.IsStarWon(1));
            GameObjectHelper.SafeSetActive(Star1NotWonGameObject, !CurrentItem.IsStarWon(1));
            GameObjectHelper.SafeSetActive(Star2WonGameObject, CurrentItem.IsStarWon(2));
            GameObjectHelper.SafeSetActive(Star2NotWonGameObject, !CurrentItem.IsStarWon(2));
            GameObjectHelper.SafeSetActive(Star3WonGameObject, CurrentItem.IsStarWon(3));
            GameObjectHelper.SafeSetActive(Star3NotWonGameObject, !CurrentItem.IsStarWon(3));
            GameObjectHelper.SafeSetActive(Star4WonGameObject, CurrentItem.IsStarWon(4));
            GameObjectHelper.SafeSetActive(Star4NotWonGameObject, !CurrentItem.IsStarWon(4));
        }


        protected override GameItemsManager<Level, GameItem> GetGameItemsManager()
        {
            return GameManager.Instance.Levels;
        }
    }
}
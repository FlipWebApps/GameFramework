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
using GameFramework.Billing.Components;
#endif
using GameFramework.Billing.Messages;
using GameFramework.GameObjects;
using GameFramework.GameStructure.GameItems.Components.AbstractClasses;
using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.GameStructure.Levels.ObjectModel;
using GameFramework.Messaging;
using UnityEngine;

namespace GameFramework.GameStructure.Levels.Components
{
    /// <summary>
    /// Level details button
    /// </summary>
    /// Provides support for a details button including selection, unlocking, IAP and more.
    [AddComponentMenu("Game Framework/GameStructure/Levels/LevelButton")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/game-structure/levels/")]
    public class LevelButton : GameItemButton<Level>
    {
        protected GameObject StarsWonGameObject;
        protected GameObject Star1WonGameObject;
        protected GameObject Star1NotWonGameObject;
        protected GameObject Star2WonGameObject;
        protected GameObject Star2NotWonGameObject;
        protected GameObject Star3WonGameObject;
        protected GameObject Star3NotWonGameObject;
        protected GameObject Star4WonGameObject;
        protected GameObject Star4NotWonGameObject;

        /// <summary>
        /// Pass static parametres to base class.
        /// </summary>
        public LevelButton() : base("Level") { }

        public new void Awake()
        {
            StarsWonGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "StarsWon", true);
            Star1WonGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "Star1Won", true);
            Star1NotWonGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "Star1NotWon", true);
            Star2WonGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "Star2Won", true);
            Star2NotWonGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "Star2NotWon", true);
            Star3WonGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "Star3Won", true);
            Star3NotWonGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "Star3NotWon", true);
            Star4WonGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "Star4Won", true);
            Star4NotWonGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "Star4NotWon", true);

            base.Awake();
            GameManager.SafeAddListener<LevelPurchasedMessage>(LevelPurchasedHandler);
        }

        protected new void OnDestroy()
        {
            GameManager.SafeRemoveListener<LevelPurchasedMessage>(LevelPurchasedHandler);
            base.OnDestroy();
        }


        /// <summary>
        /// Handler for Level purchase messages
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool LevelPurchasedHandler(BaseMessage message)
        {
            var levelPurchasedMessage = message as LevelPurchasedMessage;
            UnlockIfNumberMatches(levelPurchasedMessage.LevelNumber);
            return true;
        }


        /// <summary>
        /// Additional display setup functionality
        /// </summary>
        public override void SetupDisplay()
        {
            base.SetupDisplay();

            var level = GetGameItem<Level>();
            GameObjectHelper.SafeSetActive(StarsWonGameObject, level.IsUnlocked);
            GameObjectHelper.SafeSetActive(Star1WonGameObject, level.IsStarWon(1));
            GameObjectHelper.SafeSetActive(Star1NotWonGameObject, !level.IsStarWon(1));
            GameObjectHelper.SafeSetActive(Star2WonGameObject, level.IsStarWon(2));
            GameObjectHelper.SafeSetActive(Star2NotWonGameObject, !level.IsStarWon(2));
            GameObjectHelper.SafeSetActive(Star3WonGameObject, level.IsStarWon(3));
            GameObjectHelper.SafeSetActive(Star3NotWonGameObject, !level.IsStarWon(3));
            GameObjectHelper.SafeSetActive(Star4WonGameObject, level.IsStarWon(4));
            GameObjectHelper.SafeSetActive(Star4NotWonGameObject, !level.IsStarWon(4));
        }


        /// <summary>
        /// Returns the GameItemManager that holds Levels
        /// </summary>
        /// <returns></returns>
        protected override GameItemManager<Level, GameItem> GetGameItemManager()
        {
            return GameManager.Instance.Levels;
        }
    }
}
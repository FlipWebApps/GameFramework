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

using System.Collections;
#if UNITY_PURCHASING
using FlipWebApps.GameFramework.Scripts.Billing.Components;
#endif
using FlipWebApps.GameFramework.Scripts.GameObjects;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;
using FlipWebApps.GameFramework.Scripts.GameStructure.Players.ObjectModel;
using FlipWebApps.GameFramework.Scripts.UI.Dialogs.Components;
using FlipWebApps.GameFramework.Scripts.UI.Other;
using FlipWebApps.GameFramework.Scripts.UI.Other.Components;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using FlipWebApps.GameFramework.Scripts.GameStructure.Players.Messages;
using FlipWebApps.GameFramework.Scripts.Messaging;
using FlipWebApps.GameFramework.Scripts.Localisation;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.Components
{
    /// <summary>
    /// Base Game Item button that displays information about the linked Game Item
    /// </summary>
    /// <typeparam name="T">The type of the GameItem that we are creating a button for</typeparam>
    public abstract class GameItemButton<T> : MonoBehaviour where T : GameItem, new()
    {
        public enum SelectionModeType { ClickThrough, Select }

        [Header("Unique Identifier")]
        public int Number;

        [Header("Selection Mode")]
        public SelectionModeType SelectionMode;

        [Header("Unlocking")]
        public float CoinColorCheckInterval = 1f;
        public Color CoinColorCanUnlock = Color.green;
        public Color CoinColorCantUnlock = Color.white;

        [Header("Default Handling")]
        public string ClickUnlockedSceneToLoad;

        protected Player CurrentPlayer;

        public T CurrentItem { get; set; }

        public Image DisplayImage { get; set; }
        protected Image HighlightImage;
        protected Text ValueToUnlockAmount;
        protected GameObject LockGameObject;
        protected GameObject HighScoreGameObject;
        protected GameObject ValueToUnlockGameObject;
        protected GameObject StarsWonGameObject;
        protected GameObject Star1WonGameObject;
        protected GameObject Star1NotWonGameObject;
        protected GameObject Star2WonGameObject;
        protected GameObject Star2NotWonGameObject;
        protected GameObject Star3WonGameObject;
        protected GameObject Star3NotWonGameObject;
        protected GameObject Star4WonGameObject;
        protected GameObject Star4NotWonGameObject;

        protected bool LoadSpriteFromResources;

        public void Awake()
        {
            CurrentPlayer = GameManager.Instance.Player;

            HighlightImage = GameObjectHelper.GetChildComponentOnNamedGameObject<Image>(gameObject, "Highlight", true);
            DisplayImage = GameObjectHelper.GetChildComponentOnNamedGameObject<Image>(gameObject, "Sprite", true);
            ValueToUnlockAmount = GameObjectHelper.GetChildComponentOnNamedGameObject<Text>(gameObject, "ValueToUnlockAmount", true);
            LockGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "Lock", true);
            HighScoreGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "HighScore", true);
            ValueToUnlockGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "ValueToUnlock", true);
            StarsWonGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "StarsWon", true);
            Star1WonGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "Star1Won", true);
            Star1NotWonGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "Star1NotWon", true);
            Star2WonGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "Star2Won", true);
            Star2NotWonGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "Star2NotWon", true);
            Star3WonGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "Star3Won", true);
            Star3NotWonGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "Star3NotWon", true);
            Star4WonGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "Star4Won", true);
            Star4NotWonGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "Star4NotWon", true);

            Button b = gameObject.GetComponent<Button>();
            b.onClick.AddListener(OnClick);

            CurrentItem = GetGameItemsManager().GetItem(Number);
            Assert.IsNotNull(CurrentItem, "Could not find the specified GameItem for GameItemButton with Number " + Number);
        }

        public void Start()
        {
            SetupDisplay();

            // show unlock animation if it isn't already shown.
            if (CurrentItem.IsUnlocked == true && CurrentItem.IsUnlockedAnimationShown == false)
                Unlock();

            // add event and message listeners.
            GameManager.SafeAddListener<PlayerCoinsChangedMessage>(OnPlayerCoinsChanged);
            GetGameItemsManager().Unlocked += UnlockIfGameItemMatches;
            GetGameItemsManager().SelectedChanged += SelectedChanged;
        }

        protected void OnDestroy()
        {
            GetGameItemsManager().SelectedChanged -= SelectedChanged;
            GetGameItemsManager().Unlocked -= UnlockIfGameItemMatches;
            GameManager.SafeRemoveListener<PlayerCoinsChangedMessage>(OnPlayerCoinsChanged);
        }

        public virtual void SetupDisplay()
        {
            var isUnlockedAndAnimationShown = CurrentItem.IsUnlocked && CurrentItem.IsUnlockedAnimationShown;

            UIHelper.SetTextOnChildGameObject(gameObject, "Name", CurrentItem.Name, true);

            if (DisplayImage != null)
                DisplayImage.sprite = CurrentItem.Sprite;

            if (LockGameObject != null)
                LockGameObject.SetActive(!isUnlockedAndAnimationShown);

            if (HighScoreGameObject != null)
            {
                HighScoreGameObject.SetActive(CurrentItem.IsUnlocked);
                UIHelper.SetTextOnChildGameObject(HighScoreGameObject, "HighScoreText", CurrentItem.HighScore.ToString(), true);
            }

            if (ValueToUnlockGameObject != null)
            {
                ValueToUnlockGameObject.SetActive(!isUnlockedAndAnimationShown && CurrentItem.ValueToUnlock != -1);
                UIHelper.SetTextOnChildGameObject(ValueToUnlockGameObject, "ValueToUnlockAmount", "x" + CurrentItem.ValueToUnlock.ToString(), true);
            }

            if (SelectionMode == SelectionModeType.Select && HighlightImage != null)
            {
                HighlightImage.enabled = GetGameItemsManager().Selected.Number == CurrentItem.Number;
            }
        }

        protected abstract GameItemsManager<T, GameItem> GetGameItemsManager();

        void SelectedChanged(T oldItem, T item)
        {
            if ((oldItem != null && oldItem.Number == Number) ||
                (item != null && item.Number == Number))
                SetupDisplay();
        }

        void OnClick()
        {
            if (CurrentItem.IsUnlocked)
            {
                ClickUnlocked();
            }
            else
            {
                ClickLocked();
            }
        }


        public virtual void ClickUnlocked()
        {
            GetGameItemsManager().Selected = CurrentItem;
            PlayerPrefs.Save();

            if (!string.IsNullOrEmpty(ClickUnlockedSceneToLoad))
            {
                GameManager.LoadSceneWithTransitions(string.Format(ClickUnlockedSceneToLoad, CurrentItem.Number));
            }
        }


        public virtual void ClickLocked()
        {
#if UNITY_PURCHASING
            DialogManager.Instance.Show(title: LocaliseText.Get(CurrentItem.IdentifierBase + ".Buy.Title"),
                                        text: LocaliseText.Get(CurrentItem.IdentifierBase + ".Buy.Text1"),
                                        text2: LocaliseText.Get(CurrentItem.IdentifierBase + ".Buy.Text2"),
                                        sprite: DisplayImage.sprite,
                                        doneCallback: BuyDialogCallback,
                                        dialogButtons: DialogInstance.DialogButtonsType.OkCancel);
#else
            DialogManager.Instance.ShowInfo(textKey: CurrentItem.IdentifierBase + ".Buy.NotEnabled");
#endif
        }

        
        public void BuyDialogCallback(DialogInstance dialogInstance)
        {
            if (dialogInstance.DialogResult == DialogInstance.DialogResultType.Ok)
            {
#if UNITY_PURCHASING
                PaymentManager.Instance.BuyProductId("unlock." + CurrentItem.IdentifierBase.ToLower() + "." + CurrentItem.Number);
#endif
            }
        }


        protected void UnlockIfNumberMatches(int number)
        {
            if (number == Number)
                Unlock();
        }

   
        protected void UnlockIfGameItemMatches(T gameItem)
        {
            if (gameItem.Number == Number)
                Unlock();
        }

        public virtual void Unlock()
        {
            CurrentItem.IsUnlocked = true;
            CurrentItem.IsUnlockedAnimationShown = true;
            CurrentItem.UpdatePlayerPrefs();
            PlayerPrefs.Save();

            Animator animator = GetComponent<Animator>();
            if (animator != null)
                animator.SetTrigger("Unlock");
            else 
                SetupDisplay();
        }


        /// <summary>
        /// Receives a PlayerCoinsChangedMessage and updates the coin to unlock color.
        /// 
        /// Override to provide your own custom handling.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual bool OnPlayerCoinsChanged(BaseMessage message)
        {
            var playerCoinsChangedMessage = message as PlayerCoinsChangedMessage;
            if (ValueToUnlockAmount != null)
            {
                ValueToUnlockAmount.color = playerCoinsChangedMessage.NewCoins >= CurrentItem.ValueToUnlock ? CoinColorCanUnlock : CoinColorCantUnlock;
                return true;
            }
            return false;
        }
    }
}
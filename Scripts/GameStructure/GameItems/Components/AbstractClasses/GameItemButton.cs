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
using GameFramework.Localisation;
using GameFramework.GameObjects;
using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.GameStructure.Players.ObjectModel;
using GameFramework.UI.Dialogs.Components;
using GameFramework.UI.Other;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using GameFramework.GameStructure.Players.Messages;
using GameFramework.Messaging;
using GameFramework.Localisation.Messages;
using GameFramework.Preferences;

namespace GameFramework.GameStructure.GameItems.Components.AbstractClasses
{
    /// <summary>
    /// Abstract Game Item button class that displays information about the linked Game Item
    /// </summary>
    /// <typeparam name="T">The type of the GameItem that we are creating a button for</typeparam>
    public abstract class GameItemButton<T> : MonoBehaviour where T : GameItem, new()
    {
        public enum SelectionModeType { ClickThrough, Select }

        /// <summary>
        /// Identifier that represents the GameItem this button corresponds to.
        /// </summary>
        [Header("General")]
        [Tooltip("Identifier that represents the GameItem this button corresponds to.")]
        public int Number;

        /// <summary>
        /// This items selection mode
        /// </summary>
        /// How this is handled depends a bit on the exact implementation, however typically ClickThrough = go to next scene, Select = select item and remain
        [Tooltip("This items selection mode.")]
        public SelectionModeType SelectionMode;

        /// <summary>
        /// A color to use when this item is available for unlock
        /// </summary>
        [Header("Unlocking")]
        [Tooltip("A color to use when this item is available for unlock")]
        public Color CoinColorCanUnlock = Color.green;

        /// <summary>
        /// A color to use when this item is not available for unlock
        /// </summary>
        [Tooltip("A color to use when this item is not available for unlock")]
        public Color CoinColorCantUnlock = Color.white;

        /// <summary>
        /// The name of the scene that should be loaded when this button is clicked.
        /// </summary>
        /// You can add the format parameter{0} to substitute in the current gameitems number to allow for different scenes for each gameitem.
        [Header("Default Handling")]
        [Tooltip("The name of the scene that should be loaded when this button is clicked.\n\nYou can add the format parameter{0} to substitute in the current gameitems number to allow for different scenes for each gameitem.")]
        public string ClickUnlockedSceneToLoad;

        /// <summary>
        /// The current item that this button corresponds to.
        /// </summary>
        public T CurrentItem { get; set; }

        protected Player CurrentPlayer;

        protected Image DisplayImage { get; set; }
        protected Text ValueToUnlockAmount;
        protected GameObject HighlightGameObject;
        protected GameObject LockGameObject;
        protected GameObject HighScoreGameObject;
        protected GameObject ValueToUnlockGameObject;

        protected bool LoadSpriteFromResources;

        /// <summary>
        /// Setup and get various references for later use
        /// </summary>
        public void Awake()
        {
            CurrentPlayer = GameManager.Instance.Player;

            CurrentItem = GetGameItemManager().GetItem(Number);
            Assert.IsNotNull(CurrentItem, "Could not find the specified GameItem for GameItemButton with Number " + Number);

            // Get some references for UI button type buttons
            HighlightGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "Highlight", true);
            DisplayImage = GameObjectHelper.GetChildComponentOnNamedGameObject<Image>(gameObject, "Sprite", true);
            LockGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "Lock", true);
            HighScoreGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "HighScore", true);
            ValueToUnlockGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "ValueToUnlock", true);
            if (ValueToUnlockGameObject != null)
                ValueToUnlockAmount = GameObjectHelper.GetChildComponentOnNamedGameObject<Text>(ValueToUnlockGameObject, "ValueToUnlockAmount", true);
            var button = gameObject.GetComponent<Button>();
            if (button != null)
                button.onClick.AddListener(OnClick);
        }

        public void Start()
        {
            SetupDisplay();

            // show unlock animation if it isn't already shown.
            if (CurrentItem.IsUnlocked && CurrentItem.IsUnlockedAnimationShown == false)
                Unlock();

            // add event and message listeners.
            GetGameItemManager().Unlocked += UnlockIfGameItemMatches;
            GetGameItemManager().SelectedChanged += SelectedChanged;

            GameManager.SafeAddListener<LocalisationChangedMessage>(OnLocalisationChanged);
            GameManager.SafeAddListener<PlayerCoinsChangedMessage>(OnPlayerCoinsChanged);
        }

        protected void OnDestroy()
        {
            GameManager.SafeRemoveListener<LocalisationChangedMessage>(OnLocalisationChanged);
            GameManager.SafeRemoveListener<PlayerCoinsChangedMessage>(OnPlayerCoinsChanged);

            GetGameItemManager().SelectedChanged -= SelectedChanged;
            GetGameItemManager().Unlocked -= UnlockIfGameItemMatches;
        }

        /// <summary>
        /// Setup the display based upon configuration and availability of child gameobjects
        /// </summary>
        /// You may override this in a child class, however in most cases you will need to call this base instance also.
        public virtual void SetupDisplay()
        {
            var isUnlockedAndAnimationShown = CurrentItem.IsUnlocked && CurrentItem.IsUnlockedAnimationShown;

            UIHelper.SetTextOnChildGameObject(gameObject, "Name", CurrentItem.Name, true);

            if (DisplayImage != null)
            {
                var selectionMenuSprite = CurrentItem.GetSpriteSelectionMenu();
                DisplayImage.sprite = selectionMenuSprite ?? CurrentItem.Sprite;
            }

            if (LockGameObject != null)
                LockGameObject.SetActive(!isUnlockedAndAnimationShown);

            if (HighScoreGameObject != null)
            {
                HighScoreGameObject.SetActive(CurrentItem.IsUnlocked);
                UIHelper.SetTextOnChildGameObject(HighScoreGameObject, "HighScoreText", CurrentItem.HighScore.ToString(), true);
            }

            if (ValueToUnlockGameObject != null)
            {
                ValueToUnlockGameObject.SetActive(CurrentItem.UnlockWithCoins && !isUnlockedAndAnimationShown);
                if (ValueToUnlockAmount != null)
                    ValueToUnlockAmount.text = "x" + CurrentItem.ValueToUnlock.ToString();
            }

            if (SelectionMode == SelectionModeType.Select && HighlightGameObject != null)
            {
                HighlightGameObject.SetActive(GetGameItemManager().Selected.Number == CurrentItem.Number);
            }
        }

        /// <summary>
        /// Return a GameItemsMaanger that holds the GameItem that we are displaying.
        /// </summary>
        /// <returns></returns>
        protected abstract GameItemManager<T, GameItem> GetGameItemManager();

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


        /// <summary>
        /// Called when an unlocked button is clicked
        /// </summary>
        /// The default implementation sets the GameItemManager's selected item and then if specified loads the scene specified by ClickUnlockedSceneToLoad.
        /// You may override this in a derived class.
        public virtual void ClickUnlocked()
        {
            GetGameItemManager().Selected = CurrentItem;
            PreferencesFactory.Save();

            if (!string.IsNullOrEmpty(ClickUnlockedSceneToLoad))
            {
                GameManager.LoadSceneWithTransitions(string.Format(ClickUnlockedSceneToLoad, CurrentItem.Number));
            }
        }


        /// <summary>
        /// Called when a locked button is clicked
        /// </summary>
        /// The default implementation initiates IAP if enabled or shows a dialog otherwise.
        /// You may override this in a derived class.
        public virtual void ClickLocked()
        {
            if (CurrentItem.UnlockWithPayment)
            {
                DialogManager.Instance.Show(title: LocaliseText.Get(CurrentItem.IdentifierBase + ".Buy.Title"),
                    text: LocaliseText.Get(CurrentItem.IdentifierBase + ".Buy.Text1"),
                    text2: CurrentItem.UnlockWithCoins ? LocaliseText.Get(CurrentItem.IdentifierBase + ".Buy.Text2") : null,
                    sprite: DisplayImage.sprite,
                    doneCallback: BuyDialogCallback,
                    dialogButtons: DialogInstance.DialogButtonsType.OkCancel);
            }
            else if (CurrentItem.UnlockWithCoins)
            {
                DialogManager.Instance.ShowInfo(textKey: CurrentItem.IdentifierBase + ".Buy.NotEnabled");
            }
        }


        /// <summary>
        /// Callback from the purchase dialog. Default implementation submits the IAP request if IAP is enabled.
        /// </summary>
        /// <param name="dialogInstance"></param>
        public void BuyDialogCallback(DialogInstance dialogInstance)
        {
            if (dialogInstance.DialogResult == DialogInstance.DialogResultType.Ok)
            {
#if UNITY_PURCHASING
                PaymentManager.Instance.BuyProductId("unlock." + CurrentItem.IdentifierBase.ToLower() + "." + CurrentItem.Number);
#else
                Debug.LogWarning("You need to enable the Unity IAP Service to use payments");
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

        /// <summary>
        /// Called when the GameItem that this button represents is unlocked.
        /// </summary>
        /// The default behaviour will set the Unlock trigger on any attached Animator so you can animation the unlocking.
        /// You may override this method to provide your own custom handling.
        public virtual void Unlock()
        {
            CurrentItem.IsUnlocked = true;
            CurrentItem.IsUnlockedAnimationShown = true;
            CurrentItem.UpdatePlayerPrefs();
            PreferencesFactory.Save();

            Animator animator = GetComponent<Animator>();
            if (animator != null)
                animator.SetTrigger("Unlock");
            else 
                SetupDisplay();
        }


        /// <summary>
        /// This method is triggered when a the players coins are changed and updates the coin to unlock color.
        /// </summary>
        /// Override to provide your own custom handling.
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


        /// <summary>
        /// This method is triggered when a localisation change is detected, and triggers an update of the display.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool OnLocalisationChanged(BaseMessage message)
        {
            SetupDisplay();
            return true;
        }

    }
}
//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using System.Collections;
#if UNITY_PURCHASING
using FlipWebApps.GameFramework.Scripts.Billing.Components;
#endif
using FlipWebApps.GameFramework.Scripts.GameObjects;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;
using FlipWebApps.GameFramework.Scripts.GameStructure.Players.ObjectModel;
using FlipWebApps.GameFramework.Scripts.Localisation;
using FlipWebApps.GameFramework.Scripts.UI.Dialogs.Components;
using FlipWebApps.GameFramework.Scripts.UI.Other;
using FlipWebApps.GameFramework.Scripts.UI.Other.Components;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.Components
{
    /// <summary>
    /// Base Game Item button that displays information about the linked Game Item
    /// </summary>
    /// <typeparam name="T">The type of the GameItem that we are creating a button for</typeparam>
    public abstract class GameItemButton<T> : MonoBehaviour where T : GameItem
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

            GetGameItemsManager().Unlocked += UnlockIfGameItemMatches;
            GetGameItemsManager().SelectedChanged += SelectedChanged;
        }

        protected void OnDestroy()
        {
            GetGameItemsManager().SelectedChanged -= SelectedChanged;
            GetGameItemsManager().Unlocked -= UnlockIfGameItemMatches;
        }

        public void Start()
        {
            SetupDisplay();

            if (CoinColorCheckInterval > 0 && ValueToUnlockAmount != null)
                StartCoroutine(CoinColorCheck());
        }

        public virtual void SetupDisplay()
        {
            UIHelper.SetTextOnChildGameObject(gameObject, "Name", CurrentItem.Name, true);

            if (DisplayImage != null)
                DisplayImage.sprite = CurrentItem.Sprite;

            if (LockGameObject != null)
                LockGameObject.SetActive(!CurrentItem.IsUnlocked);

            GameObjectHelper.SafeSetActive(StarsWonGameObject, CurrentItem.IsUnlocked);
            GameObjectHelper.SafeSetActive(Star1WonGameObject, (CurrentItem.StarsWon & 1) == 1);
            GameObjectHelper.SafeSetActive(Star1NotWonGameObject, (CurrentItem.StarsWon & 1) != 1);
            GameObjectHelper.SafeSetActive(Star2WonGameObject, (CurrentItem.StarsWon & 2) == 2);
            GameObjectHelper.SafeSetActive(Star2NotWonGameObject, (CurrentItem.StarsWon & 2) != 2);
            GameObjectHelper.SafeSetActive(Star3WonGameObject, (CurrentItem.StarsWon & 4) == 4);
            GameObjectHelper.SafeSetActive(Star3NotWonGameObject, (CurrentItem.StarsWon & 4) != 4);
            GameObjectHelper.SafeSetActive(Star4WonGameObject, (CurrentItem.StarsWon & 8) == 8);
            GameObjectHelper.SafeSetActive(Star4NotWonGameObject, (CurrentItem.StarsWon & 8) != 8);

            if (HighScoreGameObject != null)
            {
                HighScoreGameObject.SetActive(CurrentItem.IsUnlocked);
                UIHelper.SetTextOnChildGameObject(HighScoreGameObject, "HighScoreText", CurrentItem.HighScore.ToString(), true);
            }

            if (ValueToUnlockGameObject != null)
            {
                ValueToUnlockGameObject.SetActive(!CurrentItem.IsUnlocked);
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
                string sceneName = GameManager.GetIdentifierScene(ClickUnlockedSceneToLoad);
                if (FadeLevelManager.IsActive)
                {
                    FadeLevelManager.Instance.LoadScene(sceneName);
                }
                else
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
                }
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

            Animator animator = GetComponent<Animator>();
            if (animator != null)
                animator.SetTrigger("Unlock");
            else 
                SetupDisplay();
        }


        protected virtual IEnumerator CoinColorCheck()
        {
            while (true)
            {
                if (GameManager.Instance.GetPlayer().Coins >= CurrentItem.ValueToUnlock)
                {
                    GameObjectHelper.GetChildComponentOnNamedGameObject<Text>(gameObject, "ValueToUnlockAmount", true).color = CoinColorCanUnlock;
                }
                else
                {
                    GameObjectHelper.GetChildComponentOnNamedGameObject<Text>(gameObject, "ValueToUnlockAmount", true).color = CoinColorCantUnlock;
                }
                yield return new WaitForSeconds(CoinColorCheckInterval);
            }

        }
    }
}
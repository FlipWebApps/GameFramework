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
using System;
using GameFramework.Debugging;
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
using GameFramework.Localisation.ObjectModel;
using GameFramework.Preferences;

namespace GameFramework.GameStructure.GameItems.Components.AbstractClasses
{
    public abstract class GameItemButton
    {
        public enum SelectionModeType { ClickThrough, Select }
    }

    /// <summary>
    /// Abstract Game Item button class that displays information about the linked Game Item
    /// </summary>
    /// <typeparam name="T">The type of the GameItem that we are creating a button for</typeparam>
    public abstract class GameItemButton<T> : GameItemContextBaseRunnable<T> where T : GameItem, new()
    {
        [Obsolete("Use GameItemButton.SelectionModeType instead. This enum will be removed in a future version.")]
        public enum SelectionModeType { ClickThrough, Select }

        /// <summary>
        /// DEPRECATED: Use GameItem Context instead.
        /// </summary>
        [Header("General")]
        [Tooltip("Identifier that represents the GameItem this button corresponds to.")]
        [Obsolete("DEPRECATED: Use GameItem Context instead")]
        public int Number;

        /// <summary>
        /// Whether to use the legacy display mode where values are set based upon hardcoded gameobject names (in the future this more will be deprecated).
        /// </summary>
        [Tooltip("Whether to use the legacy display mode where values are set based upon hardcoded gameobject names (in the future this more will be deprecated).")]
        public bool LegacyDisplayMode = true;

        /// <summary>
        /// This items selection mode
        /// </summary>
        /// How this is handled depends a bit on the exact implementation, however typically ClickThrough = go to next scene, Select = select item and remain
        [Tooltip("This items selection mode.")]
        public GameItemButton.SelectionModeType SelectionMode;

        /// <summary>
        /// The name of the scene that should be loaded when this button is clicked.
        /// </summary>
        /// You can add the format parameter{0} to substitute in the current gameitems number to allow for different scenes for each gameitem.
        [Tooltip("The name of the scene that should be loaded when this button is clicked.\n\nYou can add the format parameter{0} to substitute in the current gameitems number to allow for different scenes for each gameitem.")]
        public string ClickUnlockedSceneToLoad;

        /// <summary>
        /// A color to use when this item is available for unlock
        /// </summary>
        [Tooltip("A color to use when this item is available for unlock")]
        public Color CoinColorCanUnlock = Color.green;

        /// <summary>
        /// A color to use when this item is not available for unlock
        /// </summary>
        [Tooltip("A color to use when this item is not available for unlock")]
        public Color CoinColorCantUnlock = Color.white;

        /// <summary>
        /// Whether the user can unlock this button directly by clicking (only applicable if the GameItem has coin unlock enabled and they have enough coins).
        /// </summary>
        public bool ClickToUnlock
        {
            get { return _clickToUnlock; }
            set { _clickToUnlock = value; }
        }
        [Tooltip("Whether the user can unlock this button directly by clicking (only applicable if the GameItem has coin unlock enabled and they have enough coins).")]
        [SerializeField]
        bool _clickToUnlock;

        /// <summary>
        /// Whether the user can buy this button directly by clicking (only applicable if the GameItem has Unlock With Payment enabled and they have enough coins).
        /// </summary>
        public bool ClickToBuy
        {
            get { return _clickToBuy; }
            set { _clickToBuy = value; }
        }
        [Tooltip("Whether the user can buy this button directly by clicking (only applicable if the GameItem has Unlock With Payment and they have enough coins).")]
        [SerializeField]
        bool _clickToBuy = true;


        /// <summary>
        /// The localisation / text to use for the title of the BuyUnlock window. You can include the values: {0} - Name, {1} - Description, {2} - Value to Unlock
        /// </summary>
        [Tooltip("The localisation / text to use for the title of the BuyUnlock window. You can include the values:\n{0} - Name\n{1} - Description\n{2} - Value to Unlock")]
        public LocalisableText BuyOrUnlockTitleText;

        /// <summary>
        /// The localisation / text to use for the main text of the BuyUnlock window. You can include the values: {0} - Name, {1} - Description, {2} - Value to Unlock
        /// </summary>
        [Tooltip("The localisation / text to use for the main text of the BuyUnlock window. You can include the values:\n{0} - Name\n{1} - Description\n{2} - Value to Unlock")]
        public LocalisableText BuyOrUnlockText1;

        /// <summary>
        /// The localisation / text to use for the secondary text of the BuyUnlock window. You can include the values: {0} - Name, {1} - Description, {2} - Value to Unlock
        /// </summary>
        [Tooltip("The localisation / text to use for the secondary  text of the BuyUnlock window. You can include the values:\n{0} - Name\n{1} - Description\n{2} - Value to Unlock")]
        public LocalisableText BuyOrUnlockText2;

        /// <summary>
        /// The type of sprite that should be shown in the window. For a setting of GameItem this will use the UnlockWindow sprite type from the GameItem configuration. For more advanced customisation options use the content Prefab option below
        /// </summary>
        [Tooltip("The type of sprite that should be shown in the window. For a setting of GameItem this will use the UnlockWindow sprite type from the GameItem configuration. For more advanced customisation options use the content Prefab option below")]
        public UnlockGameItemButton.DialogSpriteType BuyOrUnlockDialogSpriteType;

        /// <summary>
        /// A custom sprite that should be used for this dialog
        /// </summary>
        [Tooltip("A custom sprite that should be used for this dialog")]
        public LocalisableSprite BuyOrUnlockDialogSprite;

        /// <summary>
        /// A optional prefab that will be inserted into the created dialog for a customised display
        /// </summary>
        [Tooltip("A optional prefab that will be inserted into the created dialog for a customised display")]
        public GameObject BuyOrUnlockContentPrefab;

        /// <summary>
        /// An optional animation controller that can be used for animating the dialog
        /// </summary>
        [Tooltip("An optional animation controller that can be used for animating the dialog")]
        public RuntimeAnimatorController BuyOrUnlockContentAnimatorController;

        /// <summary>
        /// If animating the dialog you may not want the action buttons displayed straight away. Check this it you will enable them through the animator or manually
        /// </summary>
        [Tooltip("If animating the dialog you may not want the action buttons displayed straight away. Check this it you will enable them through the animator or manually")]
        public bool BuyOrUnlockContentShowsButtons;


        /// <summary>
        /// The localisation / text to use for the title of the BuyCantUnlock window. You can include the values: {0} - Name, {1} - Description, {2} - Value to Unlock
        /// </summary>
        [Tooltip("The localisation / text to use for the title of the BuyCantUnlock window. You can include the values:\n{0} - Name\n{1} - Description\n{2} - Value to Unlock")]
        public LocalisableText BuyCantUnlockTitleText;

        /// <summary>
        /// The localisation / text to use for the main text of the BuyCantUnlock window. You can include the values: {0} - Name, {1} - Description, {2} - Value to Unlock
        /// </summary>
        [Tooltip("The localisation / text to use for the main text of the BuyCantUnlock window. You can include the values:\n{0} - Name\n{1} - Description\n{2} - Value to Unlock")]
        public LocalisableText BuyCantUnlockText1;

        /// <summary>
        /// The localisation / text to use for the secondary text of the BuyCantUnlock window. You can include the values: {0} - Name, {1} - Description, {2} - Value to Unlock
        /// </summary>
        [Tooltip("The localisation / text to use for the secondary  text of the BuyCantUnlock window. You can include the values:\n{0} - Name\n{1} - Description\n{2} - Value to Unlock")]
        public LocalisableText BuyCantUnlockText2;

        /// <summary>
        /// The type of sprite that should be shown in the window. For a setting of GameItem this will use the UnlockWindow sprite type from the GameItem configuration. For more advanced customisation options use the content Prefab option below
        /// </summary>
        [Tooltip("The type of sprite that should be shown in the window. For a setting of GameItem this will use the UnlockWindow sprite type from the GameItem configuration. For more advanced customisation options use the content Prefab option below")]
        public UnlockGameItemButton.DialogSpriteType BuyCantUnlockDialogSpriteType;

        /// <summary>
        /// A custom sprite that should be used for this dialog
        /// </summary>
        [Tooltip("A custom sprite that should be used for this dialog")]
        public LocalisableSprite BuyCantUnlockDialogSprite;

        /// <summary>
        /// A optional prefab that will be inserted into the created dialog for a customised display
        /// </summary>
        [Tooltip("A optional prefab that will be inserted into the created dialog for a customised display")]
        public GameObject BuyCantUnlockContentPrefab;

        /// <summary>
        /// An optional animation controller that can be used for animating the dialog
        /// </summary>
        [Tooltip("An optional animation controller that can be used for animating the dialog")]
        public RuntimeAnimatorController BuyCantUnlockContentAnimatorController;

        /// <summary>
        /// If animating the dialog you may not want the action buttons displayed straight away. Check this it you will enable them through the animator or manually
        /// </summary>
        [Tooltip("If animating the dialog you may not want the action buttons displayed straight away. Check this it you will enable them through the animator or manually")]
        public bool BuyCantUnlockContentShowsButtons;

        /// <summary>
        /// Whether the Buy window should be shown first to confirm they want to buy.
        /// </summary>
        [Tooltip("Whether the Buy window should be shown first to confirm they want to buy.")]
        public bool ShowBuyWindow;

        /// <summary>
        /// The localisation / text to use for the title of the buy window. You can include the values: {0} - Name, {1} - Description, {2} - Value to Unlock
        /// </summary>
        [Tooltip("The localisation / text to use for the title of the Buy window. You can include the values:\n{0} - Name\n{1} - Description\n{2} - Value to Unlock")]
        public LocalisableText BuyTitleText;

        /// <summary>
        /// The localisation / text to use for the main text of the buy window. You can include the values: {0} - Name, {1} - Description, {2} - Value to Unlock
        /// </summary>
        [Tooltip("The localisation / text to use for the main text of the Buy window. You can include the values:\n{0} - Name\n{1} - Description\n{2} - Value to Unlock")]
        public LocalisableText BuyText1;

        /// <summary>
        /// The localisation / text to use for the secondary text of the buy window. You can include the values: {0} - Name, {1} - Description, {2} - Value to Unlock
        /// </summary>
        [Tooltip("The localisation / text to use for the secondary  text of the Buy window. You can include the values:\n{0} - Name\n{1} - Description\n{2} - Value to Unlock")]
        public LocalisableText BuyText2;

        /// <summary>
        /// The type of sprite that should be shown in the window. For a setting of GameItem this will use the UnlockWindow sprite type from the GameItem configuration. For more advanced customisation options use the content Prefab option below
        /// </summary>
        [Tooltip("The type of sprite that should be shown in the window. For a setting of GameItem this will use the UnlockWindow sprite type from the GameItem configuration. For more advanced customisation options use the content Prefab option below")]
        public UnlockGameItemButton.DialogSpriteType BuyDialogSpriteType;

        /// <summary>
        /// A custom sprite that should be used for this dialog
        /// </summary>
        [Tooltip("A custom sprite that should be used for this dialog")]
        public LocalisableSprite BuyDialogSprite;

        /// <summary>
        /// A optional prefab that will be inserted into the created dialog for a customised display
        /// </summary>
        [Tooltip("A optional prefab that will be inserted into the created dialog for a customised display")]
        public GameObject BuyContentPrefab;

        /// <summary>
        /// An optional animation controller that can be used for animating the dialog
        /// </summary>
        [Tooltip("An optional animation controller that can be used for animating the dialog")]
        public RuntimeAnimatorController BuyContentAnimatorController;

        /// <summary>
        /// If animating the dialog you may not want the action buttons displayed straight away. Check this it you will enable them through the animator or manually
        /// </summary>
        [Tooltip("If animating the dialog you may not want the action buttons displayed straight away. Check this it you will enable them through the animator or manually")]
        public bool BuyContentShowsButtons;

        /// <summary>
        /// Whether the confirmation window should be shown first to confirm they want to unlock.
        /// </summary>
        [Tooltip("Whether the confirmation window should be shown first to confirm they want to unlock.")]
        public bool ShowConfirmWindow;

        /// <summary>
        /// The localisation / text to use for the title of the confirmation window. You can include the values: {0} - Name, {1} - Description, {2} - Value to Unlock
        /// </summary>
        [Tooltip("The localisation / text to use for the title of the confirmation window. You can include the values:\n{0} - Name\n{1} - Description\n{2} - Value to Unlock")]
        public LocalisableText ConfirmTitleText;

        /// <summary>
        /// The localisation / text to use for the main text of the confirmation window. You can include the values: {0} - Name, {1} - Description, {2} - Value to Unlock
        /// </summary>
        [Tooltip("The localisation / text to use for the main text of the confirmation window. You can include the values:\n{0} - Name\n{1} - Description\n{2} - Value to Unlock")]
        public LocalisableText ConfirmText1;

        /// <summary>
        /// The localisation / text to use for the secondary text of the confirmation window. You can include the values: {0} - Name, {1} - Description, {2} - Value to Unlock
        /// </summary>
        [Tooltip("The localisation / text to use for the secondary  text of the confirmation window. You can include the values:\n{0} - Name\n{1} - Description\n{2} - Value to Unlock")]
        public LocalisableText ConfirmText2;

        /// <summary>
        /// The type of sprite that should be shown in the window. For a setting of GameItem this will use the UnlockWindow sprite type from the GameItem configuration. For more advanced customisation options use the content Prefab option below
        /// </summary>
        [Tooltip("The type of sprite that should be shown in the window. For a setting of GameItem this will use the UnlockWindow sprite type from the GameItem configuration. For more advanced customisation options use the content Prefab option below")]
        public UnlockGameItemButton.DialogSpriteType ConfirmDialogSpriteType;

        /// <summary>
        /// A custom sprite that should be used for this dialog
        /// </summary>
        [Tooltip("A custom sprite that should be used for this dialog")]
        public LocalisableSprite ConfirmDialogSprite;

        /// <summary>
        /// A optional prefab that will be inserted into the created dialog for a customised display
        /// </summary>
        [Tooltip("A optional prefab that will be inserted into the created dialog for a customised display")]
        public GameObject ConfirmContentPrefab;

        /// <summary>
        /// An optional animation controller that can be used for animating the dialog
        /// </summary>
        [Tooltip("An optional animation controller that can be used for animating the dialog")]
        public RuntimeAnimatorController ConfirmContentAnimatorController;

        /// <summary>
        /// If animating the dialog you may not want the action buttons displayed straight away. Check this it you will enable them through the animator or manually
        /// </summary>
        [Tooltip("If animating the dialog you may not want the action buttons displayed straight away. Check this it you will enable them through the animator or manually")]
        public bool ConfirmContentShowsButtons;


        /// <summary>
        /// Whether the unlock window should be shown.
        /// </summary>
        [Tooltip("The type of unlock window that we should.")]
        public bool ShowUnlockWindow = true;

        /// <summary>
        /// The localisation / text to use for the title of the unlock window. You can include the values: {0} - Name, {1} - Description, {2} - Value to Unlock
        /// </summary>
        [Tooltip("The localisation / text to use for the title of the unlock window. You can include the values:\n{0} - Name\n{1} - Description\n{2} - Value to Unlock")]
        public LocalisableText UnlockTitleText;

        /// <summary>
        /// The localisation / text to use for the main text of the unlock window when a new item is unlocked. You can include the values: {0} - Name, {1} - Description, {2} - Value to Unlock
        /// </summary>
        [Tooltip("The localisation / text to use for the main text of the unlock window when a new item is unlocked. You can include the values:\n{0} - Name\n{1} - Description\n{2} - Value to Unlock")]
        public LocalisableText UnlockText1;

        /// <summary>
        /// The localisation / text to use for the secondary text of the unlock window when a new item is unlocked. You can include the values: {0} - Name, {1} - Description, {2} - Value to Unlock
        /// </summary>
        [Tooltip("The localisation / text to use for the secondary text of the unlock window when a new item is unlocked. You can include the values:\n{0} - Name\n{1} - Description\n{2} - Value to Unlock")]
        public LocalisableText UnlockText2;

        /// <summary>
        /// A optional prefab that will be inserted into the created dialog for a customised display
        /// </summary>
        [Tooltip("A optional prefab that will be inserted into the created dialog for a customised display")]
        public GameObject UnlockContentPrefab;

        /// <summary>
        /// An optional animation controller that can be used for animating the dialog
        /// </summary>
        [Tooltip("An optional animation controller that can be used for animating the dialog")]
        public RuntimeAnimatorController UnlockContentAnimatorController;

        /// <summary>
        /// If animating the dialog you may not want the action buttons displayed straight away. Check this it you will enable them through the animator or manually
        /// </summary>
        [Tooltip("If animating the dialog you may not want the action buttons displayed straight away. Check this it you will enable them through the animator or manually")]
        public bool UnlockContentShowsButtons;

        /// <summary>
        /// Whether the not enough coins window should be shown if they don't have enough coins to unlock this item.
        /// </summary>
        [Tooltip("Whether the not enough coins window should be shown first if they don't have enough coins to unlock this item.")]
        public bool ShowNotEnoughCoinsWindow = true;

        /// <summary>
        /// The localisation / text to use for the title of the not enough coins window. You can include the values: {0} - Name, {1} - Description, {2} - Value to Unlock
        /// </summary>
        [Tooltip("The localisation / text to use for the title of the not enough coins window. You can include the values:\n{0} - Name\n{1} - Description\n{2} - Value to Unlock")]
        public LocalisableText NotEnoughCoinsTitleText;

        /// <summary>
        /// The localisation / text to use for the main text of the not enough coins window. You can include the values: {0} - Name, {1} - Description, {2} - Value to Unlock
        /// </summary>
        [Tooltip("The localisation / text to use for the main text of the not enough coins window. You can include the values:\n{0} - Name\n{1} - Description\n{2} - Value to Unlock")]
        public LocalisableText NotEnoughCoinsText1;

        /// <summary>
        /// The localisation / text to use for the secondary text of the not enough coins window. You can include the values: {0} - Name, {1} - Description, {2} - Value to Unlock
        /// </summary>
        [Tooltip("The localisation / text to use for the secondary  text of the not enough coins window. You can include the values:\n{0} - Name\n{1} - Description\n{2} - Value to Unlock")]
        public LocalisableText NotEnoughCoinsText2;

        /// <summary>
        /// The type of sprite that should be shown in the window. For a setting of GameItem this will use the UnlockWindow sprite type from the GameItem configuration. For more advanced customisation options use the content Prefab option below
        /// </summary>
        [Tooltip("The type of sprite that should be shown in the window. For a setting of GameItem this will use the UnlockWindow sprite type from the GameItem configuration. For more advanced customisation options use the content Prefab option below")]
        public UnlockGameItemButton.DialogSpriteType NotEnoughCoinsDialogSpriteType;

        /// <summary>
        /// A custom sprite that should be used for this dialog
        /// </summary>
        [Tooltip("A custom sprite that should be used for this dialog")]
        public LocalisableSprite NotEnoughCoinsDialogSprite;

        /// <summary>
        /// A optional prefab that will be inserted into the created dialog for a customised display
        /// </summary>
        [Tooltip("A optional prefab that will be inserted into the created dialog for a customised display")]
        public GameObject NotEnoughCoinsContentPrefab;

        /// <summary>
        /// An optional animation controller that can be used for animating the dialog
        /// </summary>
        [Tooltip("An optional animation controller that can be used for animating the dialog")]
        public RuntimeAnimatorController NotEnoughCoinsContentAnimatorController;

        /// <summary>
        /// If animating the dialog you may not want the action buttons displayed straight away. Check this it you will enable them through the animator or manually
        /// </summary>
        [Tooltip("If animating the dialog you may not want the action buttons displayed straight away. Check this it you will enable them through the animator or manually")]
        public bool NotEnoughCoinsContentShowsButtons;



        /// <summary>
        /// The current item that this button corresponds to.
        /// </summary>
        [Obsolete("GameItemButton CurrentItem property is DEPRECATED. Change to use GameItemButton GameItem or GetGameItem<T>() instead", true)]
        public T CurrentItem {
            get
            {
                Debug.LogError("GameItemButton CurrentItem property is DEPRECATED and no longer used. Change references to GameItemButton GameItem or GetGameItem<T>() instead");
                return _currentItem;
            } set { _currentItem = value; } }

        T _currentItem;

        protected Player CurrentPlayer;

        protected Image DisplayImage { get; set; } 
        protected Text ValueToUnlockAmount;
        protected GameObject HighlightGameObject;
        protected GameObject LockGameObject;
        protected GameObject HighScoreGameObject;
        protected GameObject ValueToUnlockGameObject;

        protected bool LoadSpriteFromResources;

        string _localisationBase;

        /// <summary>
        /// Setup default values
        /// </summary>
        protected GameItemButton(string localisationBase)
        {
            _localisationBase = localisationBase;
            BuyOrUnlockTitleText = new LocalisableText { IsLocalised = true, Data = _localisationBase + ".Unlock.Title" };
            BuyOrUnlockText1 = new LocalisableText { IsLocalised = true, Data = _localisationBase + ".BuyOrUnlock.Text1" };
            BuyOrUnlockDialogSpriteType = UnlockGameItemButton.DialogSpriteType.FromGameItem;

            BuyCantUnlockTitleText = new LocalisableText { IsLocalised = true, Data = _localisationBase + ".Buy.Title" };
            BuyCantUnlockText1 = new LocalisableText { IsLocalised = true, Data = _localisationBase + ".Buy.Text1" };
            BuyCantUnlockText2 = new LocalisableText { IsLocalised = true, Data = _localisationBase + ".Buy.Text2" };
            BuyCantUnlockDialogSpriteType = UnlockGameItemButton.DialogSpriteType.FromGameItem;

            BuyTitleText = new LocalisableText { IsLocalised = true, Data = _localisationBase + ".Buy.Title" };
            BuyText1 = new LocalisableText { IsLocalised = true, Data = _localisationBase + ".Buy.Text1" };
            BuyDialogSpriteType = UnlockGameItemButton.DialogSpriteType.FromGameItem;

            ConfirmTitleText = new LocalisableText { IsLocalised = true, Data = _localisationBase + ".Unlock.Title" };
            ConfirmText1 = new LocalisableText { IsLocalised = true, Data = _localisationBase + ".Unlock.Confirm.Text1" };
            ConfirmText2 = LocalisableText.CreateNonLocalised();
            ConfirmDialogSpriteType = UnlockGameItemButton.DialogSpriteType.FromGameItem;

            UnlockTitleText = new LocalisableText { IsLocalised = true, Data = _localisationBase + ".Unlock.Title" };
            UnlockText1 = new LocalisableText { IsLocalised = true, Data = _localisationBase + ".Unlock.New.Text1" };
            UnlockText2 = new LocalisableText { IsLocalised = true, Data = _localisationBase + ".Unlock.New.Text2" };

            NotEnoughCoinsTitleText = new LocalisableText { IsLocalised = true, Data = _localisationBase + ".Unlock.Title" };
            NotEnoughCoinsText1 = new LocalisableText { IsLocalised = true, Data = _localisationBase + ".Unlock.NotEnoughCoins" };
            NotEnoughCoinsDialogSpriteType = UnlockGameItemButton.DialogSpriteType.FromGameItem;
        }

        /// <summary>
        /// Setup and get various references for later use
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            CurrentPlayer = GameManager.Instance.Player;

            // Get some references for UI button type buttons
            if (LegacyDisplayMode)
            {
                HighlightGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "Highlight", true);
                DisplayImage = GameObjectHelper.GetChildComponentOnNamedGameObject<Image>(gameObject, "Sprite", true);
                LockGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "Lock", true);
                HighScoreGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "HighScore", true);
                ValueToUnlockGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "ValueToUnlock", true);
                if (ValueToUnlockGameObject != null)
                    ValueToUnlockAmount =
                        GameObjectHelper.GetChildComponentOnNamedGameObject<Text>(ValueToUnlockGameObject,
                            "ValueToUnlockAmount", true);
            }
            var button = gameObject.GetComponent<Button>();
            if (button != null)
                button.onClick.AddListener(OnClick);
        }

        protected override void Start()
        {
            base.Start();

#pragma warning disable 618
            if (Number != 0 && Context.ContextMode != GameItemContext.ContextModeType.FromLoop)
#pragma warning restore 618
                MyDebug.LogWarning("<GameItem>Button Number field is replaced by GameItem Context and will be removed. On any <GameItem>Button component, please set General->Number to 0 on any gameobjects / prefabs and set GameItem context accordingly to remove this warning. Note: Create<GameItem>Buttons will automatically use a mode of FromLoop. GameObject: " + gameObject.name);

            Assert.IsNotNull(GameItem, "Could not find the specified GameItem for GameItemButton " + gameObject.name);

            SetupDisplay();

            // show unlock animation if it isn't already shown.
            if (GameItem.IsUnlocked && GameItem.IsUnlockedAnimationShown == false)
                Unlock();

            // add event and message listeners.
            GetGameItemManager().Unlocked += UnlockIfGameItemMatches;
            GetGameItemManager().SelectedChanged += SelectedChanged;

            GameManager.SafeAddListener<LocalisationChangedMessage>(OnLocalisationChanged);
            if (LegacyDisplayMode)
                GameManager.SafeAddListener<PlayerCoinsChangedMessage>(OnPlayerCoinsChanged);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            GameManager.SafeRemoveListener<LocalisationChangedMessage>(OnLocalisationChanged);
            if (LegacyDisplayMode)
                GameManager.SafeRemoveListener<PlayerCoinsChangedMessage>(OnPlayerCoinsChanged);

            GetGameItemManager().SelectedChanged -= SelectedChanged;
            GetGameItemManager().Unlocked -= UnlockIfGameItemMatches;
        }


        public override void RunMethod(bool isStart = true)
        {
            SetupDisplay();
        }

        /// <summary>
        /// Setup the display based upon configuration and availability of child gameobjects
        /// </summary>
        /// You may override this in a child class, however in most cases you will need to call this base instance also.
        public virtual void SetupDisplay()
        {
            var isUnlockedAndAnimationShown = GameItem.IsUnlocked && GameItem.IsUnlockedAnimationShown;

            if (LegacyDisplayMode)
            {
                UIHelper.SetTextOnChildGameObject(gameObject, "Name", GameItem.Name, true);

                if (DisplayImage != null)
                {
                    var selectionMenuSprite = GameItem.GetSpriteByType(GameItem.LocalisableSpriteType.SelectionMenu);
                    DisplayImage.sprite = selectionMenuSprite ?? GameItem.Sprite;
                }

                if (LockGameObject != null)
                    LockGameObject.SetActive(!isUnlockedAndAnimationShown);

                if (HighScoreGameObject != null)
                {
                    HighScoreGameObject.SetActive(GameItem.IsUnlocked);
                    UIHelper.SetTextOnChildGameObject(HighScoreGameObject, "HighScoreText",
                        GameItem.HighScore.ToString(), true);
                }

                if (ValueToUnlockGameObject != null)
                {
                    ValueToUnlockGameObject.SetActive(GameItem.UnlockWithCoins && !isUnlockedAndAnimationShown);
                    if (ValueToUnlockAmount != null)
                        ValueToUnlockAmount.text = "x" + GameItem.ValueToUnlock.ToString();
                }

                if (SelectionMode == GameItemButton.SelectionModeType.Select && HighlightGameObject != null)
                {
                    HighlightGameObject.SetActive(GetGameItemManager().Selected.Number == GameItem.Number);
                }
            }
        }


        //void SelectedChanged(T oldItem, T item)
        //{
        //    if ((oldItem != null && oldItem.Number == Number) ||
        //        (item != null && item.Number == Number))
        //        SetupDisplay();
        //}


        void OnClick()
        {
            if (GameItem.IsUnlocked)
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
            GetGameItemManager().Selected = GetGameItem<T>();
            PreferencesFactory.Save();

            if (SelectionMode == GameItemButton.SelectionModeType.ClickThrough && !string.IsNullOrEmpty(ClickUnlockedSceneToLoad))
            {
                GameManager.LoadSceneWithTransitions(string.Format(ClickUnlockedSceneToLoad, GameItem.Number));
            }
        }

        #region Unlocking

        /// <summary>
        /// Called when a locked button is clicked and handles display of any dialogs plus unlocking / IAP
        /// </summary>
        public virtual void ClickLocked()
        {
            if (SelectionMode == GameItemButton.SelectionModeType.Select)
            {
                GetGameItemManager().Selected = GetGameItem<T>();
                PreferencesFactory.Save();
            }

            if (ClickToUnlock && GameItem.UnlockWithCoins && ClickToBuy && GameItem.UnlockWithPayment)
            {
                if (GameManager.Instance.Player.Coins >= GameItem.ValueToUnlock)
                    DisplayBuyOrUnlockDialog();
                else
                    DisplayBuyCantUnlockDialog();
            }
            else if (ClickToBuy && GameItem.UnlockWithPayment)
            {
                if (ShowBuyWindow)
                    DisplayBuyDialog();
                else
                    BuyGameItemButton<T>.ProcessBuy(GameItem);
            }
            else if (ClickToUnlock && GameItem.UnlockWithCoins)
            {
                if (GameManager.Instance.Player.Coins >= GameItem.ValueToUnlock)
                {
                    if (ShowConfirmWindow)
                        DisplayConfirmUnlockDialog();
                    else if (ShowUnlockWindow)
                        DisplayUnlockDialog();
                    else
                        ProcessUnlock();
                }
                else
                {
                    if (ShowNotEnoughCoinsWindow)
                    {
                        DisplayNotEnoughCoinsDialog();
                    }
                }
            }
        }


        /// <summary>
        /// Display the buy or unlock dialog window
        /// </summary>
        void DisplayBuyOrUnlockDialog()
        {
            Assert.IsTrue(DialogManager.IsActive, "Ensure that you have added a DialogManager component to your scene before showing a dialog!");
            var dialogInstance = DialogManager.Instance.Create(null, null, BuyOrUnlockContentPrefab, null, runtimeAnimatorController: BuyOrUnlockContentAnimatorController, contentSiblingIndex: 1);
            dialogInstance.Show(title: BuyOrUnlockTitleText.FormatValue(GameItem.Name, GameItem.Description, GameItem.ValueToUnlock),
                text: ValueOrNull(BuyOrUnlockText1.FormatValue(GameItem.Name, GameItem.Description, GameItem.ValueToUnlock)),
                text2: ValueOrNull(BuyOrUnlockText2.FormatValue(GameItem.Name, GameItem.Description, GameItem.ValueToUnlock)),
                sprite: DialogSpriteToUse(BuyOrUnlockDialogSpriteType, BuyOrUnlockDialogSprite),
                doneCallback: BuyOrUnlockDialogCallback,
                dialogButtons:
                    BuyOrUnlockContentShowsButtons
                        ? DialogInstance.DialogButtonsType.Custom
                        : DialogInstance.DialogButtonsType.Text,
                buttonText: new[] { LocalisableText.CreateLocalised("Button.Unlock"), LocalisableText.CreateLocalised("Button.Buy"), LocalisableText.CreateLocalised("Button.Cancel")});
        }


        /// <summary>
        /// Display the buy not enough coins dialog window
        /// </summary>
        void DisplayBuyCantUnlockDialog()
        {
            Assert.IsTrue(DialogManager.IsActive, "Ensure that you have added a DialogManager component to your scene before showing a dialog!");
            var dialogInstance = DialogManager.Instance.Create(null, null, BuyCantUnlockContentPrefab, null, runtimeAnimatorController: BuyCantUnlockContentAnimatorController, contentSiblingIndex: 1);
            dialogInstance.Show(title: BuyCantUnlockTitleText.FormatValue(GameItem.Name, GameItem.Description, GameItem.ValueToUnlock),
                text: ValueOrNull(BuyCantUnlockText1.FormatValue(GameItem.Name, GameItem.Description, GameItem.ValueToUnlock)),
                text2: ValueOrNull(BuyCantUnlockText2.FormatValue(GameItem.Name, GameItem.Description, GameItem.ValueToUnlock)),
                sprite: DialogSpriteToUse(BuyCantUnlockDialogSpriteType, BuyCantUnlockDialogSprite),
                doneCallback: BuyDialogCallback,
                dialogButtons:
                    BuyCantUnlockContentShowsButtons
                        ? DialogInstance.DialogButtonsType.Custom
                        : DialogInstance.DialogButtonsType.OkCancel);
        }


        /// <summary>
        /// Display the buy dialog window
        /// </summary>
        void DisplayBuyDialog()
        {
            Assert.IsTrue(DialogManager.IsActive, "Ensure that you have added a DialogManager component to your scene before showing a dialog!");
            DialogManager.Instance.Show(title: BuyTitleText.FormatValue(GameItem.Name, GameItem.Description, GameItem.ValueToUnlock),
               text: ValueOrNull(BuyText1.FormatValue(GameItem.Name, GameItem.Description, GameItem.ValueToUnlock)),
               text2: ValueOrNull(BuyText2.FormatValue(GameItem.Name, GameItem.Description, GameItem.ValueToUnlock)),
               sprite: GameItem.GetSpriteByType(GameItem.LocalisableSpriteType.UnlockWindow) ?? GameItem.Sprite,
               doneCallback: BuyDialogCallback,
               dialogButtons: DialogInstance.DialogButtonsType.OkCancel);
        }


        /// <summary>
        /// Display the confirm unlock window
        /// </summary>
        void DisplayConfirmUnlockDialog()
        {
            Assert.IsTrue(DialogManager.IsActive, "Ensure that you have added a DialogManager component to your scene before showing a dialog!");
            var dialogInstance = DialogManager.Instance.Create(null, null, ConfirmContentPrefab, null, runtimeAnimatorController: ConfirmContentAnimatorController, contentSiblingIndex: 1);
            dialogInstance.Show(title: ConfirmTitleText.FormatValue(GameItem.Name, GameItem.Description, GameItem.ValueToUnlock),
                text: ValueOrNull(ConfirmText1.FormatValue(GameItem.Name, GameItem.Description, GameItem.ValueToUnlock)),
                text2: ValueOrNull(ConfirmText2.FormatValue(GameItem.Name, GameItem.Description, GameItem.ValueToUnlock)),
                sprite: DialogSpriteToUse(ConfirmDialogSpriteType, ConfirmDialogSprite),
                doneCallback: ConfirmDialogCallback,
                dialogButtons:
                    ConfirmContentShowsButtons
                        ? DialogInstance.DialogButtonsType.Custom
                        : DialogInstance.DialogButtonsType.OkCancel);
        }


        /// <summary>
        /// Display the unlock dialog window
        /// </summary>
        void DisplayUnlockDialog()
        {
            Assert.IsTrue(DialogManager.IsActive, "Ensure that you have added a DialogManager component to your scene before showing a dialog!");
            var dialogInstance = DialogManager.Instance.Create(null, null, UnlockContentPrefab, null, runtimeAnimatorController: UnlockContentAnimatorController, contentSiblingIndex: 1);
            var unlockWindowSprite = GameItem.GetSpriteByType(GameItem.LocalisableSpriteType.UnlockWindow);
            dialogInstance.Show(title: UnlockTitleText.FormatValue(GameItem.Name, GameItem.Description, GameItem.ValueToUnlock),
                text: ValueOrNull(UnlockText1.FormatValue(GameItem.Name, GameItem.Description, GameItem.ValueToUnlock)),
                text2: ValueOrNull(UnlockText2.FormatValue(GameItem.Name, GameItem.Description, GameItem.ValueToUnlock)),
                sprite: unlockWindowSprite ?? GameItem.Sprite,
                doneCallback: UnlockedDialogCallback,
                dialogButtons: UnlockContentShowsButtons ? DialogInstance.DialogButtonsType.Custom : DialogInstance.DialogButtonsType.Ok);
        }

        /// <summary>
        /// Display the not enough coins to unlock window
        /// </summary>
        void DisplayNotEnoughCoinsDialog()
        {
            Assert.IsTrue(DialogManager.IsActive, "Ensure that you have added a DialogManager component to your scene before showing a dialog!");
            var dialogInstance = DialogManager.Instance.Create(null, null, NotEnoughCoinsContentPrefab, null, runtimeAnimatorController: NotEnoughCoinsContentAnimatorController, contentSiblingIndex: 1);
            dialogInstance.Show(title: NotEnoughCoinsTitleText.FormatValue(GameItem.Name, GameItem.Description, GameItem.ValueToUnlock),
                text: ValueOrNull(NotEnoughCoinsText1.FormatValue(GameItem.Name, GameItem.Description, GameItem.ValueToUnlock)),
                text2: ValueOrNull(NotEnoughCoinsText2.FormatValue(GameItem.Name, GameItem.Description, GameItem.ValueToUnlock)),
                sprite: DialogSpriteToUse(NotEnoughCoinsDialogSpriteType, NotEnoughCoinsDialogSprite),
                dialogButtons: NotEnoughCoinsContentShowsButtons ? DialogInstance.DialogButtonsType.Custom : DialogInstance.DialogButtonsType.Ok);
        }


        /// <summary>
        /// Callback when the buy or unlock dialog completes
        /// </summary>
        /// <param name="dialogInstance"></param>
        void BuyOrUnlockDialogCallback(DialogInstance dialogInstance)
        {
            if (dialogInstance.DialogResultCustom == 0)
            {
                if (ShowUnlockWindow)
                    DisplayUnlockDialog();
                else
                    ProcessUnlock();
            }
            else if (dialogInstance.DialogResultCustom == 1)
                BuyGameItemButton<T>.ProcessBuy(GameItem);
        }


        /// <summary>
        /// Callback from the purchase dialog. Default implementation submits the IAP request if IAP is enabled.
        /// </summary>
        /// <param name="dialogInstance"></param>
        public void BuyDialogCallback(DialogInstance dialogInstance)
        {
            if (dialogInstance.DialogResult == DialogInstance.DialogResultType.Ok)
            {
                BuyGameItemButton<T>.ProcessBuy(GameItem);
            }
        }


        /// <summary>
        /// Callback when the confirm dialog completes
        /// </summary>
        /// <param name="dialogInstance"></param>
        void ConfirmDialogCallback(DialogInstance dialogInstance)
        {
            if (dialogInstance.DialogResult == DialogInstance.DialogResultType.Ok)
            {
                if (ShowUnlockWindow)
                    DisplayUnlockDialog();
                else
                    ProcessUnlock();
            }
        }


        /// <summary>
        /// Callback when the dialog completes
        /// </summary>
        /// <param name="dialogInstance"></param>
        void UnlockedDialogCallback(DialogInstance dialogInstance)
        {
            ProcessUnlock();
        }


        /// <summary>
        /// Process the actual unlocking of an item.
        /// </summary>
        void ProcessUnlock()
        {
            GetGameItemManager().Unlocked(GetGameItem<T>());

#if UNITY_ANALYTICS
    // record some analytics on the item unlocked
                //Analytics.CustomEvent(_localisationBase + ".Unlock", new Dictionary<string, object>
                //{
                //    { "number", _gameItemToUnlock.Number },
                //    { "timesplayed", GameManager.Instance.TimesGamePlayed }
                //});
#endif
            //update new coin count.
            GameManager.Instance.Player.Coins -= GameItem.ValueToUnlock;
            GameManager.Instance.Player.UpdatePlayerPrefs();
        }


        protected void UnlockIfNumberMatches(int number)
        {
            if (number == GameItem.Number)
                Unlock();
        }

   
        protected void UnlockIfGameItemMatches(T gameItem)
        {
            if (gameItem.Number == GameItem.Number)
                Unlock();
        }

        /// <summary>
        /// Called when the GameItem that this button represents is unlocked.
        /// </summary>
        /// The default behaviour will set the Unlock trigger on any attached Animator so you can animation the unlocking.
        /// You may override this method to provide your own custom handling.
        public virtual void Unlock()
        {
            GameItem.IsUnlocked = true;
            GameItem.IsUnlockedAnimationShown = true;
            GameItem.UpdatePlayerPrefs();
            PreferencesFactory.Save();

            Animator animator = GetComponent<Animator>();
            if (animator != null)
                animator.SetTrigger("Unlock");
            else 
                SetupDisplay();
        }
        #endregion Unlocking

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
                ValueToUnlockAmount.color = playerCoinsChangedMessage.NewCoins >= GameItem.ValueToUnlock ? CoinColorCanUnlock : CoinColorCantUnlock;
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

        #region Helper Methods
        static string ValueOrNull(string value)
        {
            return string.IsNullOrEmpty(value) ? null : value;
        }


        Sprite DialogSpriteToUse(UnlockGameItemButton.DialogSpriteType spriteType, LocalisableSprite localisableSprite)
        {
            Sprite sprite = null;
            if (spriteType == UnlockGameItemButton.DialogSpriteType.FromGameItem)
                sprite = GameItem.GetSpriteByType(GameItem.LocalisableSpriteType.UnlockWindow) ??
                    GameItem.Sprite;
            else if (spriteType == UnlockGameItemButton.DialogSpriteType.Custom)
                sprite = localisableSprite.GetSprite();
            return sprite;
        }
        #endregion
    }
}
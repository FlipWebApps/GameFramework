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

using GameFramework.Debugging;
using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.Localisation.ObjectModel;
using GameFramework.UI.Dialogs.Components;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityEngine.UI;

#if UNITY_ANALYTICS
using System.Collections.Generic;
using UnityEngine.Analytics;
#endif

namespace GameFramework.GameStructure.GameItems.Components.AbstractClasses
{
    public class UnlockGameItemButton
    {
        /// <summary>
        /// The different modes for unlocking items.
        /// </summary>
        /// Placed in non generic class to avoid display issues in Unity Editor
        public enum UnlockModeType
        {
            RandomAll,
            RandomLocked,
            NextLocked,
            Selected,
            ByNumber,
            FromLoop,
            Reference
        }

        /// <summary>
        /// A simple option for configuring the dialog sprite types.
        /// </summary>
        /// Placed in non generic class to avoid display issues in Unity Editor
        public enum DialogSpriteType
        {
            None,
            FromGameItem,
            Custom
        }
    }

    /// <summary>
    /// abstract base Unlock GameItem button that handles the ability to unlock GameItems 
    /// </summary>
    /// <typeparam name="T">The type of the GameItem that we are creating a button for</typeparam>
    [RequireComponent(typeof(Button))]
    public abstract class UnlockGameItemButton<T> : GameItemContextBaseRunnable<T> where T : GameItem, new()
    {
        #region Inspector variables
        /// <summary>
        /// The mode to use for unlocking items.
        /// </summary>
        [Tooltip("The mode to use for unlocking items.")]
        public UnlockGameItemButton.UnlockModeType UnlockMode;

        /// <summary>
        /// A maximum number of failed unlock attmepts before we make sure to unlock something
        /// </summary>
        [Tooltip("A maximum number of failed unlock attmepts before we make sure to unlock something")]
        public int MaxFailedUnlocks = 999;

        /// <summary>
        /// Whether the button should be disabled when we can't unlock anything.
        /// </summary>
        [Header("Input")]
        [Tooltip("Whether the button should be disabled when we can't unlock anything.")]
        public bool DisableIfCanNotUnlock = true;

        /// <summary>
        /// The localisation / text to use for the title of the can't unlock window. You can include the values: {0} - Name, {1} - Description, {2} - Value to Unlock
        /// </summary>
        [Tooltip("The localisation / text to use for the title of the can't unlock window. You can include the values:\n{0} - Name\n{1} - Description\n{2} - Value to Unlock")]
        public LocalisableText CanNotUnlockTitleText;

        /// <summary>
        /// The localisation / text to use for the main text of the can't unlock window when trying to unlock an item. You can include the values: {0} - Name, {1} - Description, {2} - Value to Unlock
        /// </summary>
        [Tooltip("The localisation / text to use for the main text of the can't unlock window when trying to unlock an item. You can include the values:\n{0} - Name\n{1} - Description\n{2} - Value to Unlock")]
        public LocalisableText CanNotUnlockText1;

        /// <summary>
        /// The localisation / text to use for the secondary text of the can't unlock window when trying to unlock an item. You can include the values: {0} - Name, {1} - Description, {2} - Value to Unlock
        /// </summary>
        [Tooltip("The localisation / text to use for the secondary  text of the can't unlock window when trying to unlock an item. You can include the values:\n{0} - Name\n{1} - Description\n{2} - Value to Unlock")]
        public LocalisableText CanNotUnlockText2;

        /// <summary>
        /// The type of sprite that should be shown in the window. For a setting of GameItem this will use the UnlockWindow sprite type from the GameItem configuration. For more advanced customisation options use the content Prefab option below
        /// </summary>
        [Tooltip("The type of sprite that should be shown in the window. For a setting of GameItem this will use the UnlockWindow sprite type from the GameItem configuration. For more advanced customisation options use the content Prefab option below")]
        public UnlockGameItemButton.DialogSpriteType CanNotUnlockDialogSpriteType;

        /// <summary>
        /// A custom sprite that should be used for this dialog
        /// </summary>
        [Tooltip("A custom sprite that should be used for this dialog")]
        public LocalisableSprite CanNotUnlockDialogSprite;

        /// <summary>
        /// A optional prefab that will be inserted into the created dialog for a customised display
        /// </summary>
        [Tooltip("A optional prefab that will be inserted into the created dialog for a customised display")]
        public GameObject CanNotUnlockContentPrefab;

        /// <summary>
        /// An optional animation controller that can be used for animating the dialog
        /// </summary>
        [Tooltip("An optional animation controller that can be used for animating the dialog")]
        public RuntimeAnimatorController CanNotUnlockContentAnimatorController;

        /// <summary>
        /// If animating the dialog you may not want the action buttons displayed straight away. Check this it you will enable them through the animator or manually
        /// </summary>
        [Tooltip("If animating the dialog you may not want the action buttons displayed straight away. Check this it you will enable them through the animator or manually")]
        public bool CanNotUnlockContentShowsButtons;

        /// <summary>
        /// Whether the confirmation window should be shown first to confirm they want to unlock.
        /// </summary>
        [Header("Feedback")]
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
        public LocalisableText UnlockedText1;

        /// <summary>
        /// The localisation / text to use for the secondary text of the unlock window when a new item is unlocked. You can include the values: {0} - Name, {1} - Description, {2} - Value to Unlock
        /// </summary>
        [Tooltip("The localisation / text to use for the secondary text of the unlock window when a new item is unlocked. You can include the values:\n{0} - Name\n{1} - Description\n{2} - Value to Unlock")]
        public LocalisableText UnlockedText2;

        /// <summary>
        /// The localisation / text to use for the main text of the unlock window when the item is already unlocked. You can include the values: {0} - Name, {1} - Description, {2} - Value to Unlock
        /// </summary>
        [Tooltip("The localisation / text to use for the main text of the unlock window when the item is already unlocked. You can include the values:\n{0} - Name\n{1} - Description\n{2} - Value to Unlock")]
        public LocalisableText AlreadyUnlockedText1;

        /// <summary>
        /// The localisation / text to use for the secondary text of the unlock window when the item is already unlocked. You can include the values: {0} - Name, {1} - Description, {2} - Value to Unlock
        /// </summary>
        [Tooltip("The localisation / text to use for the secondary text of the unlock window when the item is already unlocked. You can include the values:\n{0} - Name\n{1} - Description\n{2} - Value to Unlock")]
        public LocalisableText AlreadyUnlockedText2;

        /// <summary>
        /// A optional prefab that will be inserted into the created dialog for a customised display
        /// </summary>
        [Tooltip("A optional prefab that will be inserted into the created dialog for a customised display")]
        [FormerlySerializedAs("ContentPrefab")]
        public GameObject UnlockContentPrefab;

        /// <summary>
        /// An optional animation controller that can be used for animating the dialog
        /// </summary>
        [Tooltip("An optional animation controller that can be used for animating the dialog")]
        [FormerlySerializedAs("ContentAnimatorController")]
        public RuntimeAnimatorController UnlockContentAnimatorController;

        /// <summary>
        /// If animating the dialog you may not want the action buttons displayed straight away. Check this it you will enable them through the animator or manually
        /// </summary>
        [Tooltip("If animating the dialog you may not want the action buttons displayed straight away. Check this it you will enable them through the animator or manually")]
        [FormerlySerializedAs("ContentShowsButtons")]
        public bool UnlockContentShowsButtons;

        #endregion Inspector variables

        Button _button;
        UnityEngine.Animation _animation;

        string _localisationBase;
        int _failedUnlockAttempts;

        T _gameItemToUnlock;
        bool _alreadyUnlocked;

        /// <summary>
        /// Setup default values
        /// </summary>
        protected UnlockGameItemButton(string localisationBase)
        {
            _localisationBase = localisationBase;

            CanNotUnlockTitleText = new LocalisableText { IsLocalised = true, Data = _localisationBase + ".Unlock.Title" };
            CanNotUnlockText1 = new LocalisableText { IsLocalised = true, Data = _localisationBase + ".Unlock.NotEnoughCoins" };
            CanNotUnlockText2 = new LocalisableText { IsLocalised = true, Data = _localisationBase + ".Unlock.Existing.Text2"};
            CanNotUnlockDialogSpriteType = UnlockGameItemButton.DialogSpriteType.None;
            
            ConfirmTitleText = new LocalisableText { IsLocalised = true, Data = _localisationBase + ".Unlock.Title" };
            ConfirmText1 = new LocalisableText { IsLocalised = true, Data = _localisationBase + ".Unlock.Confirm.Text1" };
            ConfirmText2 = LocalisableText.CreateNonLocalised();
            ConfirmDialogSpriteType = UnlockGameItemButton.DialogSpriteType.FromGameItem;

            UnlockTitleText = new LocalisableText {IsLocalised = true, Data = _localisationBase + ".Unlock.Title"};
            UnlockedText1 = new LocalisableText { IsLocalised = true, Data = _localisationBase + ".Unlock.New.Text1" };
            UnlockedText2 = new LocalisableText { IsLocalised = true, Data = _localisationBase + ".Unlock.New.Text2" };
            AlreadyUnlockedText1 = new LocalisableText { IsLocalised = true, Data = _localisationBase + ".Unlock.Existing.Text1" };
            AlreadyUnlockedText2 = new LocalisableText { IsLocalised = true, Data = _localisationBase + ".Unlock.Existing.Text2" };
        }

        /// <summary>
        /// Setup
        /// </summary>
        protected override void Awake()
        {
            // sync mode over to Context
            if (UnlockMode == UnlockGameItemButton.UnlockModeType.ByNumber)
                Context.ContextMode = GameItemContext.ContextModeType.ByNumber;
            else if (UnlockMode == UnlockGameItemButton.UnlockModeType.Selected)
                Context.ContextMode = GameItemContext.ContextModeType.Selected;
            else if (UnlockMode == UnlockGameItemButton.UnlockModeType.FromLoop)
                Context.ContextMode = GameItemContext.ContextModeType.FromLoop;
            else if (UnlockMode == UnlockGameItemButton.UnlockModeType.Reference)
                Context.ContextMode = GameItemContext.ContextModeType.Reference;
            
            base.Awake();
            _button = GetComponent<Button>();
            _button.onClick.AddListener(Unlock);

            _animation = GetComponent<UnityEngine.Animation>();

            _failedUnlockAttempts = GameManager.Instance.Player.GetSettingInt(_localisationBase + ".FailedUnlockAttempts", 0);

            // Sanity checking
            if (UnlockMode == UnlockGameItemButton.UnlockModeType.RandomAll && !ShowUnlockWindow)
                MyDebug.LogWarning("If you use an UnlockMode of RandomAll then you should show the Unlock Window so the user gets feedback if nothing is unlocked.");
        }


        /// <summary>
        /// Continually check whether changes mean we can now unlock a new item.
        /// </summary>
        void Update()
        {
            var canUnlock = false;
            if (!DisableIfCanNotUnlock)
            {
                // if there are no more items available to unlock then still disable anyway, otherwise enabled for trying.
                canUnlock = GetGameItemManager().HasMoreLockedCoinUnlockableItems();
            }
            else
            {
                // otherwise base enable state on whether we can try unlocking
                canUnlock = ConvertToGameItemManagerUnlockMode(UnlockMode) ==
                                GameItemManager.UnlockModeType.GameItem
                    ? GameManager.Instance.Player.Coins >= GameItem.ValueToUnlock && !GameItem.IsUnlocked
                    : GetGameItemManager()
                        .CanTryCoinUnlocking(ConvertToGameItemManagerUnlockMode(UnlockMode),
                            GameManager.Instance.Player.Coins);
            }
            _button.interactable = canUnlock;
            if (_animation != null)
                _animation.enabled = _button.interactable;
        }


        /// <summary>
        /// Convert the unlock modes defined by this component to ones that can be used with a GameItemManager.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected GameItemManager.UnlockModeType ConvertToGameItemManagerUnlockMode(UnlockGameItemButton.UnlockModeType type)
        {
            switch (type)
            {
                case UnlockGameItemButton.UnlockModeType.RandomAll:
                    return GameItemManager.UnlockModeType.RandomAll;
                case UnlockGameItemButton.UnlockModeType.RandomLocked:
                    return GameItemManager.UnlockModeType.RandomLocked;
                case UnlockGameItemButton.UnlockModeType.NextLocked:
                    return GameItemManager.UnlockModeType.NextLocked;
                default:
                    return GameItemManager.UnlockModeType.GameItem;
            }
        }


        /// <summary>
        /// Called by the base class from start and optionally if the selection chages.
        /// </summary>
        /// <param name="isStart"></param>
        public override void RunMethod(bool isStart = true)
        {      
            // we don't use this for now due to the Update() loop!
        }


        /// <summary>
        /// Added as a listener to the attached button and is called to trigger the unlock process and show the unlock dialog 
        /// </summary>
        public void Unlock()
        {
            // Check if we have enough coins to actually unlock an item to try unlocking.
            var gameItem = ConvertToGameItemManagerUnlockMode(UnlockMode) == GameItemManager.UnlockModeType.GameItem ?
                GetGameItem<T>() :
                GetGameItemManager().GetItemToTryCoinUnlocking(ConvertToGameItemManagerUnlockMode(UnlockMode), GameManager.Instance.Player.Coins, _failedUnlockAttempts, MaxFailedUnlocks);
            if (gameItem != null)
            {
                _gameItemToUnlock = gameItem;
                _alreadyUnlocked = _gameItemToUnlock.IsUnlocked;

                if (gameItem.ValueToUnlock > GameManager.Instance.Player.Coins)
                {
                    DisplayCanNotUnlockWindow();
                }
                else
                {
                    if (ShowConfirmWindow)
                    {
                        DisplayConfirmWindow();
                    }
                    else if (ShowUnlockWindow)
                    {
                        DisplayUnlockWindow();
                    }
                    else
                    {
                        ProcessUnlock();
                    }
                }
            }
            else
            {
                DisplayCanNotUnlockWindow();
            }
        }


        /// <summary>
        /// Display the can't unlock window
        /// </summary>
        void DisplayCanNotUnlockWindow()
        {
            Sprite sprite = null;
            if (CanNotUnlockDialogSpriteType == UnlockGameItemButton.DialogSpriteType.FromGameItem)
                sprite = _gameItemToUnlock.GetSpriteByType(GameItem.LocalisableSpriteType.UnlockWindow) ??
                    _gameItemToUnlock.Sprite;
            else if (CanNotUnlockDialogSpriteType == UnlockGameItemButton.DialogSpriteType.Custom)
                sprite = CanNotUnlockDialogSprite.GetSprite();

            Assert.IsTrue(DialogManager.IsActive, "Ensure that you have added a DialogManager component to your scene before showing a dialog!");
            var dialogInstance = DialogManager.Instance.Create(null, null, CanNotUnlockContentPrefab, null, runtimeAnimatorController: CanNotUnlockContentAnimatorController, contentSiblingIndex: 1);
            dialogInstance.Show(title: CanNotUnlockTitleText.GetValue(),
                text: ValueOrNull(CanNotUnlockText1.GetValue()),
                text2: ValueOrNull(CanNotUnlockText2.GetValue()),
                sprite: sprite,
                dialogButtons:
                    CanNotUnlockContentShowsButtons
                        ? DialogInstance.DialogButtonsType.Custom
                        : DialogInstance.DialogButtonsType.Ok);
        }


        /// <summary>
        /// Display the confirm window
        /// </summary>
        void DisplayConfirmWindow()
        {
            Sprite sprite = null;
            if (ConfirmDialogSpriteType == UnlockGameItemButton.DialogSpriteType.FromGameItem)
                sprite = _gameItemToUnlock.GetSpriteByType(GameItem.LocalisableSpriteType.UnlockWindow) ??
                    _gameItemToUnlock.Sprite;
            else if (ConfirmDialogSpriteType == UnlockGameItemButton.DialogSpriteType.Custom)
                sprite = ConfirmDialogSprite.GetSprite();

            Assert.IsTrue(DialogManager.IsActive, "Ensure that you have added a DialogManager component to your scene before showing a dialog!");
            var dialogInstance = DialogManager.Instance.Create(null, null, ConfirmContentPrefab, null, runtimeAnimatorController: ConfirmContentAnimatorController, contentSiblingIndex: 1);
            dialogInstance.Show(title: ConfirmTitleText.FormatValue(_gameItemToUnlock.Name, _gameItemToUnlock.Description, _gameItemToUnlock.ValueToUnlock),
                text: ValueOrNull(ConfirmText1.FormatValue(_gameItemToUnlock.Name, _gameItemToUnlock.Description, _gameItemToUnlock.ValueToUnlock)),
                text2: ValueOrNull(ConfirmText2.FormatValue(_gameItemToUnlock.Name, _gameItemToUnlock.Description, _gameItemToUnlock.ValueToUnlock)),
                sprite: sprite,
                doneCallback: ConfirmWindowCallback,
                dialogButtons:
                    ConfirmContentShowsButtons
                        ? DialogInstance.DialogButtonsType.Custom
                        : DialogInstance.DialogButtonsType.OkCancel);
        }


        /// <summary>
        /// Callback when the confirm dialog completes
        /// </summary>
        /// <param name="dialogInstance"></param>
        void ConfirmWindowCallback(DialogInstance dialogInstance)
        {
            if (dialogInstance.DialogResult == DialogInstance.DialogResultType.Ok)
            {
                if (ShowUnlockWindow)
                {
                    DisplayUnlockWindow();
                }
                else
                {
                    ProcessUnlock();
                }
            }
        }


        /// <summary>
        /// Display the unlock window
        /// </summary>
        void DisplayUnlockWindow()
        {
            LocalisableText textKey, text2Key;
            if (!_alreadyUnlocked)
            {
                _failedUnlockAttempts = 0;       // reset counter
                textKey = UnlockedText1;
                text2Key = UnlockedText2;
            }
            else
            {
                _failedUnlockAttempts++;         // increase counter

                textKey = AlreadyUnlockedText1;
                text2Key = AlreadyUnlockedText2;
            }

            // save updated counter for later.
            GameManager.Instance.Player.SetSetting(_localisationBase + ".FailedUnlockAttempts", _failedUnlockAttempts);
            GameManager.Instance.Player.UpdatePlayerPrefs();
            var unlockWindowSprite = _gameItemToUnlock.GetSpriteByType(GameItem.LocalisableSpriteType.UnlockWindow);

            Assert.IsTrue(DialogManager.IsActive, "Ensure that you have added a DialogManager component to your scene before showing a dialog!");
            var dialogInstance = DialogManager.Instance.Create(null, null, UnlockContentPrefab, null, runtimeAnimatorController: UnlockContentAnimatorController, contentSiblingIndex: 1);
            dialogInstance.Show(title: UnlockTitleText.FormatValue(_gameItemToUnlock.Name, _gameItemToUnlock.Description, _gameItemToUnlock.ValueToUnlock),
                text: ValueOrNull(textKey.FormatValue(_gameItemToUnlock.Name, _gameItemToUnlock.Description, _gameItemToUnlock.ValueToUnlock)),
                text2: ValueOrNull(text2Key.FormatValue(_gameItemToUnlock.Name, _gameItemToUnlock.Description, _gameItemToUnlock.ValueToUnlock)),
                sprite: unlockWindowSprite ?? _gameItemToUnlock.Sprite,
                doneCallback: UnlockedWindowCallback,
                dialogButtons: UnlockContentShowsButtons ? DialogInstance.DialogButtonsType.Custom : DialogInstance.DialogButtonsType.Ok);
        }


        /// <summary>
        /// Callback when the dialog completes
        /// </summary>
        /// <param name="dialogInstance"></param>
        void UnlockedWindowCallback(DialogInstance dialogInstance)
        {
            ProcessUnlock();
        }


        /// <summary>
        /// Process the actual unlocking of an item.
        /// </summary>
        protected virtual void ProcessUnlock()
        {
            if (!_alreadyUnlocked)
            {
                GetGameItemManager().Unlocked(_gameItemToUnlock);

#if UNITY_ANALYTICS
                // record some analytics on the item unlocked
                Analytics.CustomEvent(_localisationBase + ".Unlock", new Dictionary<string, object>
                {
                    { "number", _gameItemToUnlock.Number },
                    { "timesplayed", GameManager.Instance.TimesGamePlayed }
                });
#endif
            }

            //update new coin count.
            GameManager.Instance.Player.Coins -= _gameItemToUnlock.ValueToUnlock;
            GameManager.Instance.Player.UpdatePlayerPrefs();
        }

        #region Helper Methods
        static string ValueOrNull(string value)
        {
            return string.IsNullOrEmpty(value) ? null : value;
        }
        #endregion
    }
}
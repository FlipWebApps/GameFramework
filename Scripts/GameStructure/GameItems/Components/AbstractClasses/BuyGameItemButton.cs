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
    /// <summary>
    /// abstract base BuyGameItemButton button that handles the ability to buy GameItems 
    /// </summary>
    /// <typeparam name="T">The type of the GameItem that we are creating a button for</typeparam>
    [RequireComponent(typeof(Button))]
    public abstract class BuyGameItemButton<T> : GameItemContextBaseRunnable<T> where T : GameItem, new()
    {
        #region Inspector variables

        /// <summary>
        /// Whether the confirmation window should be shown first to confirm they want to unlock.
        /// </summary>
        [Header("Feedback")]
        [Tooltip("Whether the confirmation window should be shown first to confirm they want to unlock.")]
        public bool ShowConfirmWindow = true;

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
        [Tooltip("The localisation / text to use for the secondary text of the confirmation window. You can include the values:\n{0} - Name\n{1} - Description\n{2} - Value to Unlock")]
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

        #endregion Inspector variables

        Button _button;
        UnityEngine.Animation _animation;

        readonly string _localisationBase;

        T _gameItemToBuy;


        /// <summary>
        /// Setup default values
        /// </summary>
        protected BuyGameItemButton(string localisationBase)
        {
            _localisationBase = localisationBase;

            ConfirmTitleText = new LocalisableText { IsLocalised = true, Data = _localisationBase + ".Buy.Title" };
            ConfirmText1 = new LocalisableText { IsLocalised = true, Data = _localisationBase + ".Buy.Text1" };
            ConfirmText2 = LocalisableText.CreateNonLocalised();
            ConfirmDialogSpriteType = UnlockGameItemButton.DialogSpriteType.FromGameItem;
        }


        /// <summary>
        /// Setup
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            _button = GetComponent<Button>();
            _button.onClick.AddListener(Unlock);

            _animation = GetComponent<UnityEngine.Animation>();
        }


        /// <summary>
        /// Setup
        /// </summary>
        protected override void Start()
        {
            base.Start();
            GetGameItemManager().Unlocked += Unlocked;
        }


        /// <summary>
        /// Destroy
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            GetGameItemManager().Unlocked -= Unlocked;
        }


        /// <summary>
        /// Called when a GameItem is unlocked.
        /// </summary>
        /// <param name="gameItem"></param>
        void Unlocked(T gameItem)
        {
            if (gameItem.Number == GameItem.Number)
                RunMethod(false);
        }


        /// <summary>
        /// Called by the base class from start and optionally if the selection chages.
        /// </summary>
        /// <param name="isStart"></param>
        public override void RunMethod(bool isStart = true)
        {
            var canBuy = !GameItem.IsUnlocked && GameItem.UnlockWithPayment;

            _button.interactable = canBuy;
            if (_animation != null)
                _animation.enabled = canBuy;
        }


        /// <summary>
        /// Added as a listener to the attached button and is called to trigger the unlock process and show the unlock dialog 
        /// </summary>
        public void Unlock()
        {
            // There should always be an item - we should not let them unlock if there is nothing to unlock - that is bad practice!
            var gameItem = GetGameItem<T>();
            if (gameItem != null)
            {
                _gameItemToBuy = gameItem;

                if (ShowConfirmWindow)
                {
                    DisplayConfirmWindow();
                }
                else
                {
                    ProcessBuy(GameItem);
                }

            }
            else
            {
                DialogManager.Instance.Show(text: "All items are already bought");
            }
        }


        /// <summary>
        /// Display the confirm window
        /// </summary>
        void DisplayConfirmWindow()
        {
            Sprite sprite = null;
            if (ConfirmDialogSpriteType == UnlockGameItemButton.DialogSpriteType.FromGameItem)
                sprite = _gameItemToBuy.GetSpriteByType(GameItem.LocalisableSpriteType.UnlockWindow) ??
                    _gameItemToBuy.Sprite;
            else if (ConfirmDialogSpriteType == UnlockGameItemButton.DialogSpriteType.Custom)
                sprite = ConfirmDialogSprite.GetSprite();

            Assert.IsTrue(DialogManager.IsActive, "Ensure that you have added a DialogManager component to your scene before showing a dialog!");
            var dialogInstance = DialogManager.Instance.Create(null, null, ConfirmContentPrefab, null, runtimeAnimatorController: ConfirmContentAnimatorController, contentSiblingIndex: 1);
            dialogInstance.Show(title: ConfirmTitleText.FormatValue(_gameItemToBuy.Name, _gameItemToBuy.Description, _gameItemToBuy.ValueToUnlock),
                text: ValueOrNull(ConfirmText1.FormatValue(_gameItemToBuy.Name, _gameItemToBuy.Description, _gameItemToBuy.ValueToUnlock)),
                text2: ValueOrNull(ConfirmText2.FormatValue(_gameItemToBuy.Name, _gameItemToBuy.Description, _gameItemToBuy.ValueToUnlock)),
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
                ProcessBuy(GameItem);
            }
        }


        /// <summary>
        /// Process the actual purchase of an item.
        /// </summary>
        public static void ProcessBuy(GameItem gameItem)
        {
#if UNITY_PURCHASING
            Assert.IsTrue(PaymentManager.IsActive, "Please add a PaymentManager component to your scene if using In App Purchases.");
            PaymentManager.Instance.BuyProductId("unlock." + gameItem.IdentifierBase.ToLower() + "." + gameItem.Number);
#else
            Debug.LogWarning("You need to enable the Unity IAP Service to use payments");
#endif
        }


        #region Helper Methods
        static string ValueOrNull(string value)
        {
            return string.IsNullOrEmpty(value) ? null : value;
        }
        #endregion
    }
}
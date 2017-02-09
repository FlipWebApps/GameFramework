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

using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.UI.Dialogs.Components;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_ANALYTICS
using System.Collections.Generic;
using UnityEngine.Analytics;
#endif

namespace GameFramework.GameStructure.GameItems.Components.AbstractClasses
{
    /// <summary>
    /// abstract base Unlock GameItem button that handles the ability to unlock GameItems 
    /// </summary>
    /// <typeparam name="T">The type of the GameItem that we are creating a button for</typeparam>
    [RequireComponent(typeof(Button))]
    public abstract class UnlockGameItemButton<T> : MonoBehaviour where T : GameItem, new()
    {

        /// <summary>
        /// The mode to use for unlocking items.
        /// </summary>
        [Header("Settings")]
        [Tooltip("The mode to use for unlocking items.")]
        public GameItemManager.UnlockModeType UnlockMode;

        /// <summary>
        /// A maximum number of failed unlock attmepts before we make sure to unlock something
        /// </summary>
        [Tooltip("A maximum number of failed unlock attmepts before we make sure to unlock something")]
        public int MaxFailedUnlocks = 999;

        /// <summary>
        /// A optional prefab that will be inserted into the created dialog for a customised display
        /// </summary>
        [Header("Display")]
        [Tooltip("A optional prefab that will be inserted into the created dialog for a customised display")]
        public GameObject ContentPrefab;

        /// <summary>
        /// An optional animation controller that can be used for animating the dialog
        /// </summary>
        [Tooltip("An optional animation controller that can be used for animating the dialog")]
        public RuntimeAnimatorController ContentAnimatorController;

        /// <summary>
        /// If animating the dialog you may not want the action buttons displayed straight away. Check this it you will enable them through the animator or manually
        /// </summary>
        [Tooltip("If animating the dialog you may not want the action buttons displayed straight away. Check this it you will enable them through the animator or manually")]
        public bool ContentShowsButtons;

        Button _button;
        UnityEngine.Animation _animation;

        string _localisationBase;
        int _failedUnlockAttempts;

        T _gameItemToUnlock;
        bool _alreadyUnlocked;


        /// <summary>
        /// Setup
        /// </summary>
        void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(Unlock);

            _animation = GetComponent<UnityEngine.Animation>();

            _localisationBase = GetGameItemManager().TypeName;
            _failedUnlockAttempts = GameManager.Instance.Player.GetSettingInt(_localisationBase + ".FailedUnlockAttempts", 0);
        }


        /// <summary>
        /// Continually check whether changes mean we can now unlock a new item.
        /// </summary>
        void Update()
        {
            var canUnlock = GetGameItemManager().CanCoinUnlockNewItem(UnlockMode, GameManager.Instance.Player.Coins);

            _button.interactable = canUnlock;
            if (_animation != null)
                _animation.enabled = canUnlock;
        }



        /// <summary>
        /// Implement this method to return a GameItemManager that contains the GameItems that this button works upon.
        /// </summary>
        /// <returns></returns>
        protected abstract GameItemManager<T, GameItem> GetGameItemManager();


        /// <summary>
        /// Added as a listener to the attached button and is called to trigger the unlock process and show the unlock dialog 
        /// </summary>
        public void Unlock()
        {
            var dialogInstance = DialogManager.Instance.Create(null, null, ContentPrefab, null, runtimeAnimatorController: ContentAnimatorController, contentSiblingIndex: 1);

            // There should always be an item - we should not let them unlock if there is nothing to unlock!
            var gameItem = GetGameItemManager().GetItemToCoinUnlock(UnlockMode, GameManager.Instance.Player.Coins, _failedUnlockAttempts, MaxFailedUnlocks);
            if (gameItem != null)
            {
                _gameItemToUnlock = gameItem;
                _alreadyUnlocked = _gameItemToUnlock.IsUnlocked;

                string textKey, text2Key;
                if (!_alreadyUnlocked)
                {
                    _failedUnlockAttempts = 0;       // reset counter
                    textKey = _localisationBase + ".Unlock.New.Text1";
                    text2Key = _localisationBase + ".Unlock.New.Text2";
                }
                else
                {
                    _failedUnlockAttempts++;         // increase counter

                    textKey = _localisationBase + ".Unlock.Existing.Text1";
                    text2Key = _localisationBase + ".Unlock.Existing.Text2";
                }

                // save updated counter for later.
                GameManager.Instance.Player.SetSetting(_localisationBase + ".FailedUnlockAttempts", _failedUnlockAttempts);
                GameManager.Instance.Player.UpdatePlayerPrefs();
                var unlockWindowSprite = _gameItemToUnlock.GetSpriteUnlockWindow();
                dialogInstance.Show(titleKey: _localisationBase + ".Unlock.Title",
                    textKey: textKey,
                    text2Key: text2Key,
                    sprite: unlockWindowSprite ?? _gameItemToUnlock.Sprite,
                    doneCallback: UnlockedCallback,
                    dialogButtons: ContentShowsButtons ? DialogInstance.DialogButtonsType.Custom : DialogInstance.DialogButtonsType.Ok);
            }
            else
            {
                // Note: this should never happen in a well designed solution hence we don't localise!
                DialogManager.Instance.Show(text: "All items are already unlocked");
            }
        }



        /// <summary>
        /// Callback when the dialog completes
        /// </summary>
        /// <param name="dialogInstance"></param>
        void UnlockedCallback(DialogInstance dialogInstance)
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
    }
}
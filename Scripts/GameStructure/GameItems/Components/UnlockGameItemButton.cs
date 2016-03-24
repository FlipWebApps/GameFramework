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
using FlipWebApps.GameFramework.Scripts.GameObjects;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;
using FlipWebApps.GameFramework.Scripts.Localisation;
using FlipWebApps.GameFramework.Scripts.UI.Dialogs.Components;
using FlipWebApps.GameFramework.Scripts.UI.Other;
using FlipWebApps.GameFramework.Scripts.UI.Other.Components;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_ANALYTICS
using System.Collections.Generic;
using UnityEngine.Analytics;
#endif

namespace FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.Components
{
    /// <summary>
    /// abstract base Unlock GameItem button that handles the ability to unlock GameItems 
    /// </summary>
    /// <typeparam name="T">The type of the GameItem that we are creating a button for</typeparam>
    [RequireComponent(typeof(Button))]
    public abstract class UnlockGameItemButton<T> : MonoBehaviour where T : GameItem, new()
    {
        [Header("Settings")]
        public int MaxFailedUnlocks = 999;      // number of failed unlock attmepts before we actually unlock something for them

        [Header("Display")]
        public GameObject ContentPrefab;
        public RuntimeAnimatorController ContentAnimatorController;
        public bool ContentShowsButtons;

        Button _button;
        UnityEngine.Animation _animation;

        string _localisationBase;
        int _failedUnlockAttempts;

        T _gameItemToUnlock;
        bool _alreadyUnlocked;

        void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(Unlock);

            _animation = GetComponent<UnityEngine.Animation>();

            _localisationBase = GetGameItemsManager().TypeName;
            _failedUnlockAttempts = GameManager.Instance.Player.GetSettingInt(_localisationBase + ".FailedUnlockAttempts", 0);
        }

        void Update()
        {
            int minimumCoinsToOpenLevel = GetGameItemsManager().ExtraValueNeededToUnlock(GameManager.Instance.Player.Coins);
            bool canUnlock = minimumCoinsToOpenLevel != -1 && minimumCoinsToOpenLevel == 0;
            _button.interactable = canUnlock;
            if (_animation != null)
                _animation.enabled = canUnlock;
        }

        protected abstract GameItemsManager<T, GameItem> GetGameItemsManager();

        public void Unlock()
        {
            var dialogInstance = DialogManager.Instance.Create(null, null, ContentPrefab, null, runtimeAnimatorController: ContentAnimatorController, contentSiblingIndex: 1);

            // If failed unlock attempts is greater then max then unlock one of the locked items so they don't get fed up.
            T[] gameItems;
            if (_failedUnlockAttempts >= MaxFailedUnlocks)
            {
                gameItems = GetGameItemsManager().UnlockableItems(GameManager.Instance.Player.Coins, true);
            }
            else {
                gameItems = GetGameItemsManager().UnlockableItems(GameManager.Instance.Player.Coins);
            }

            // There should always be an item - we should not let them unlock if there is nothing to unlock!
            if (gameItems.Length >= 0)
            {
                _gameItemToUnlock = gameItems[Random.Range(0, gameItems.Length)];
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

                //UIHelper.SetTextOnChildGameObject(dialogInstance.Content, "ulph_Text", text, true);
                //UIHelper.SetTextOnChildGameObject(dialogInstance.Content, "ulph_Text2", text2, true);
                //UIHelper.SetSpriteOnChildGameObject(dialogInstance.Content, "ulph_Image", _gameItemToUnlock.Sprite, true);// SceneManager.Instance.ChoosePantsButtons[LevelToUnlock.Number-1].DisplayImage.sprite, true);

                dialogInstance.Show(titleKey: _localisationBase + ".Unlock.Title",
                    textKey: textKey,
                    text2Key: text2Key,
                    sprite: _gameItemToUnlock.Sprite,
                    doneCallback: UnlockedCallback,
                    dialogButtons: ContentShowsButtons ? DialogInstance.DialogButtonsType.Custom : DialogInstance.DialogButtonsType.Ok);

                //StartCoroutine(UnlockCoRoutine(dialogInstance));
            }
            else
            {
                // Note: this should never happen in a well designed solution hence we don't localise!
                DialogManager.Instance.Show(text: "All items are already unlocked");
            }
        }


        void UnlockedCallback(DialogInstance dialogInstance)
        {
            if (!_alreadyUnlocked)
            {
                GetGameItemsManager().Unlocked(_gameItemToUnlock);

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
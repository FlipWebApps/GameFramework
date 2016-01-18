//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
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
    public abstract class UnlockGameItemButton<T> : MonoBehaviour where T : GameItem
    {
        public int MaxFailedUnlocks = 999;      // number of failed unlock attmepts before we actually unlock something for them
        public float DialogShowButtonDelay;

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
            StartCoroutine(UnlockCoRoutine());
        }

        IEnumerator UnlockCoRoutine()
        {
            DialogInstance dialogInstance = DialogManager.Instance.Create(null, "UnlockLevelPlaceHolder");

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

                string text, text2;
                if (!_alreadyUnlocked)
                {
                    _failedUnlockAttempts = 0;       // reset counter
                    text = LocaliseText.Get(_localisationBase + ".Unlock.New.Text1");
                    text2 = LocaliseText.Get(_localisationBase + ".Unlock.New.Text2");
                }
                else
                {
                    _failedUnlockAttempts++;         // increase counter

                    text = LocaliseText.Get(_localisationBase + ".Unlock.Existing.Text1");
                    text2 = LocaliseText.Get(_localisationBase + ".Unlock.Existing.Text2");
                }

                // save updated counter for later.
                GameManager.Instance.Player.SetSetting(_localisationBase + ".FailedUnlockAttempts", _failedUnlockAttempts);
                GameManager.Instance.Player.UpdatePlayerPrefs();

                UIHelper.SetTextOnChildGameObject(dialogInstance.Content, "ulph_Text", text, true);
                UIHelper.SetTextOnChildGameObject(dialogInstance.Content, "ulph_Text2", text2, true);
                UIHelper.SetSpriteOnChildGameObject(dialogInstance.Content, "ulph_Image", _gameItemToUnlock.Sprite, true);// SceneManager.Instance.ChoosePantsButtons[LevelToUnlock.Number-1].DisplayImage.sprite, true);

                dialogInstance.Show(titleKey: _localisationBase + ".Unlock.Title", doneCallback: UnlockedCallback);

                yield return new WaitForSeconds(DialogShowButtonDelay);

                GameObjectHelper.GetChildNamedGameObject(dialogInstance.gameObject, "OkButton", true).SetActive(true);
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
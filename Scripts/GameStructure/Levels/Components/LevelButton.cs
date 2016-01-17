//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

#if UNITY_PURCHASING
using FlipWebApps.GameFramework.Scripts.Billing.Components;
#endif
using System.Runtime.Remoting.Messaging;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.Components;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;
using FlipWebApps.GameFramework.Scripts.GameStructure.Levels.ObjectModel;
using FlipWebApps.GameFramework.Scripts.UI.Other.Components;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Levels.Components
{
    /// <summary>
    /// Level Details Button
    /// </summary>
    public class LevelButton : GameItemButton<Level>
    {
        [Header("Default Handling")]
        public string ClickUnlockedSceneToLoad;

        public new void Awake()
        {
            base.Awake();

#if UNITY_PURCHASING
            if (PaymentManager.Instance != null)
                PaymentManager.Instance.LevelPurchased += UnlockIfNumberMatches;
#endif
        }

        protected new void OnDestroy()
        {
#if UNITY_PURCHASING
            if (PaymentManager.Instance != null)
                PaymentManager.Instance.LevelPurchased -= UnlockIfNumberMatches;
#endif

            base.OnDestroy();
        }

        protected override GameItemsManager<Level, GameItem> GetGameItemsManager()
        {
            return GameManager.Instance.Levels;
        }

        public override void ClickUnlocked()
        {
            base.ClickUnlocked();

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
    }
}
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
using FlipWebApps.GameFramework.Scripts.GameStructure.Worlds.ObjectModel;
using FlipWebApps.GameFramework.Scripts.UI.Other.Components;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Worlds.Components
{
    /// <summary>
    /// World Details Button
    /// </summary>
    public class WorldButton : GameItemButton<World>
    {
        public new void Awake()
        {
            base.Awake();

#if UNITY_PURCHASING
            if (PaymentManager.Instance != null)
                PaymentManager.Instance.WorldPurchased += UnlockIfNumberMatches;
#endif
        }

        protected new void OnDestroy()
        {
#if UNITY_PURCHASING
            if (PaymentManager.Instance != null)
                PaymentManager.Instance.WorldPurchased -= UnlockIfNumberMatches;
#endif

            base.OnDestroy();
        }

        protected override GameItemsManager<World, GameItem> GetGameItemsManager()
        {
            return GameManager.Instance.Worlds;
        }
    }
}
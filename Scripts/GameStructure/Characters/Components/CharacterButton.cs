//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

#if UNITY_PURCHASING
using FlipWebApps.GameFramework.Scripts.Billing.Components;
#endif
using FlipWebApps.GameFramework.Scripts.GameStructure.Characters.ObjectModel;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.Components;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Characters.Components
{
    /// <summary>
    /// Character Details Button
    /// </summary>
    public class CharacterButton : GameItemButton<Character>
    {

        public new void Awake()
        {
            base.Awake();

#if UNITY_PURCHASING
            if (PaymentManager.Instance != null)
                PaymentManager.Instance.CharacterPurchased += UnlockIfNumberMatches;
#endif
        }

        protected new void OnDestroy()
        {
#if UNITY_PURCHASING
            if (PaymentManager.Instance != null)
                PaymentManager.Instance.CharacterPurchased -= UnlockIfNumberMatches;
#endif

            base.OnDestroy();
        }

        protected override GameItemsManager<Character, GameItem> GetGameItemsManager()
        {
            return GameManager.Instance.Characters;
        }
    }
}
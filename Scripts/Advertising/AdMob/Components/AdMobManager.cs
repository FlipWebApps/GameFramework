//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameObjects.Components;
using FlipWebApps.GameFramework.Scripts.GameStructure;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Advertising.AdMob.Components
{
    /// <summary>
    /// manager class for setting up and accessing AdMob functionality.
    /// 
    /// NOTE: This class is beta and subject to changebreaking change without warning.
    /// </summary>
    public class AdMobManager : SingletonPersistant<GameManager>
    {
        [Header("Advertising")]
        public string AdmobUnitIdAndroid = "";
        public string AdmobUnitIdIos = "";

        /// <summary>
        /// Advertising related properties
        /// </summary>
        public AdMob Adverts;

        protected override void GameSetup()
        {
            Debug.Log("AdMobManager: GameSetup");

            // advertising
#if GOOGLE_ADS
            if (!GameManager.Instance.IsUnlocked && !string.IsNullOrEmpty(AdmobUnitIdAndroid) && !string.IsNullOrEmpty(AdmobUnitIdIos))
            {
                Adverts = new AdMob();
                Adverts.HideBanner();
                Adverts.RequestBanner();
            }
#endif
        }
    }
}
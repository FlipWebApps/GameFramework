//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using System.Security.Cryptography;
using FlipWebApps.GameFramework.Scripts.Social.Components;
using FlipWebApps.GameFramework.Scripts.UI.Dialogs.Components;

#if UNITY_ADS
using FlipWebApps.GameFramework.Scripts.GameStructure;
using FlipWebApps.GameFramework.Scripts.Localisation;
using FlipWebApps.GameFramework.Scripts.UI.Other.Components;
using UnityEngine;
using UnityEngine.Advertisements;
#endif

namespace FlipWebApps.GameFramework.Scripts.Advertising.UnityAds
{
    /// <summary>
    /// Helper functions for Unity Adverts
    /// </summary>
    public class UnityAds
    {
        /// <summary>
        /// Show an advert and award the player coins if they complete watching it.
        /// </summary>
        /// <param name="coins"></param>
        public static void ShowWatchAdvertForCoins(int coins)
        {
#if UNITY_ADS
            //TODO only show advert button if actually ready to avoid errors.
            if (Advertisement.IsReady())
            {
                Advertisement.Show(null, new ShowOptions
                {
                    //pause = true,
                    resultCallback = result =>
                    {
                        switch (result)
                        {
                            case (ShowResult.Finished):
                                GameManager.Instance.GetPlayer().Coins += coins;
                                GameManager.Instance.GetPlayer().UpdatePlayerPrefs();
                                PlayerPrefs.Save();
                                DialogManager.Instance.Show(title: "Coins", text: LocaliseText.Format("Advertising.UnityAds.WatchForCoins.Finished", coins));
                                break;
                            case (ShowResult.Skipped):
                                DialogManager.Instance.Show(title: "Coins", text: LocaliseText.Get("Advertising.UnityAds.WatchForCoins.Skipped"));
                                break;
                            case (ShowResult.Failed):
                                DialogManager.Instance.Show(title: "Coins", text: LocaliseText.Get("Advertising.UnityAds.WatchForCoins.UnableToShow"));
                                break;

                        }
                        Debug.Log(result.ToString());
                    }
                });
            }
            else
            {
                DialogManager.Instance.Show(title: "Error", text: LocaliseText.Get("Advertising.UnityAds.UnableToShow"));
            }
#else
            DialogManager.Instance.ShowInfo("This functionality requires that you enable the standard Unity Ads service.\n\nPlease check our website if you need further help.");
#endif
        }
    }
}
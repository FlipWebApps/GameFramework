//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace FlipWebApps.GameFramework.Scripts.Advertising.UnityAds.Components
{
    /// <summary>
    /// Component for showing the watch advert for coins dialog on button click. 
    /// 
    /// This automatically hooks up the button onClick listener
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class OnButtonClickWatchAdvertForCoins : MonoBehaviour
    {
        /// <summary>
        /// Number of coins they will win
        /// </summary>
        public int Coins;

        void Start()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
        }

        public void OnClick()
        {
            UnityAds.ShowWatchAdvertForCoins(Coins);
        }
    }
}
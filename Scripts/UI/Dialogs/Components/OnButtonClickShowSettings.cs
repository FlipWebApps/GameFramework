//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace FlipWebApps.GameFramework.Scripts.UI.Dialogs.Components
{
    /// <summary>
    /// Show the settings dialog
    /// 
    /// This automatically hooks up the button onClick listener
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class OnButtonClickShowSettings : MonoBehaviour
    {

        void Start()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
        }

        void OnClick()
        {
            Assert.IsTrue(Settings.IsActive, "You need to add a settings dialog instance before using this component.");

            Settings.Instance.Show();
        }
    }
}
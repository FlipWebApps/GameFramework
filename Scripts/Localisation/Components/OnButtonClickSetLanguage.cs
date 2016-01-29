//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameObjects.Components;
using FlipWebApps.GameFramework.Scripts.UI.Dialogs.Components;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace FlipWebApps.GameFramework.Scripts.Localisation.Components
{
    /// <summary>
    /// Show the settings dialog
    /// 
    /// This automatically hooks up the button onClick listener
    /// </summary>
    public class OnButtonClickSetLanguage : OnButtonClick
    {
        /// <summary>
        /// The langauge that we want to set
        /// </summary>
        public string Language;

        /// <summary>
        /// Override that is called when the attached button is clicked.
        /// Sets the language to that specified by Language.
        /// </summary>
        public  override void OnClick()
        {
            Localisation.LocaliseText.Language = Language;
        }
    }
}
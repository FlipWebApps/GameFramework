//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameObjects.Components;

namespace FlipWebApps.GameFramework.Scripts.Localisation.Components
{
    /// <summary>
    /// Enabled or a disabled a gameobject based upon whether the facebook SDK is installed
    /// </summary>
    public class EnableIfMultipleLanguages : EnableDisableGameObject
    {
        public override bool IsConditionMet()
        {
            return Localisation.LocaliseText.AllowedLanguages.Length > 1;
        }
    }
}
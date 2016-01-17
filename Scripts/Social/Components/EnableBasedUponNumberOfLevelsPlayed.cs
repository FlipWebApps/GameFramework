//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameObjects.Components;
using FlipWebApps.GameFramework.Scripts.GameStructure;

namespace FlipWebApps.GameFramework.Scripts.Social.Components
{
    /// <summary>
    /// Shows an enabled or a disabled gameobject based upon the number of levels played
    /// </summary>
    public class EnableBasedUponNumberOfLevelsPlayed : EnableDisableGameObject
    {
        public int NumberOfLevels;

        public override bool IsConditionMet()
        {
            return GameManager.Instance.TimesLevelsPlayed >= NumberOfLevels;
        }
    }
}
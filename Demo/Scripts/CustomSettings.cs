using System.Collections;
using FlipWebApps.GameFramework.Scripts.GameObjects;
using FlipWebApps.GameFramework.Scripts.GameStructure;
using FlipWebApps.GameFramework.Scripts.UI.Dialogs.Components;
using UnityEngine;
using UnityEngine.UI;

namespace FlipWebApps.GameFramework.Demo.Scripts
{
    public class CustomSettings : Settings
    {
        public Demo Demo;

        Slider _playBackSpeedSlider;

        /// <summary>
        /// Override default settings to inject our custom value
        /// </summary>
        public override void Show()
        {
            _playBackSpeedSlider = GameObjectHelper.GetChildComponentOnNamedGameObject<Slider>(DialogInstance.DialogGameObject, "PlaybackSpeedSlider", true);
            _playBackSpeedSlider.value = 35 - Demo.PlaybackSpeed;

            base.Show();
        }

        /// <summary>
        /// Override DoneCallback to save our custom value.
        /// </summary>
        /// <param name="dialogInstance"></param>
        public override void DoneCallback(DialogInstance dialogInstance)
        {
            if (DialogInstance.DialogResult == DialogInstance.DialogResultType.Ok)
            {
                Demo.PlaybackSpeed = 35 - _playBackSpeedSlider.value;
                PlayerPrefs.SetFloat("GameFramework.Demo.PlaybackSpeed", Demo.PlaybackSpeed);
                PlayerPrefs.Save();
            }

            base.DoneCallback(dialogInstance);
        }

    }
}
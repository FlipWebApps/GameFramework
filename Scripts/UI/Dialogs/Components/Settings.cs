//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

#if UNITY_PURCHASING
using FlipWebApps.GameFramework.Scripts.Billing.Components;
#endif
using FlipWebApps.GameFramework.Scripts.GameObjects;
using FlipWebApps.GameFramework.Scripts.GameObjects.Components;
using FlipWebApps.GameFramework.Scripts.GameStructure;
using UnityEngine;
using UnityEngine.Assertions;

namespace FlipWebApps.GameFramework.Scripts.UI.Dialogs.Components
{
    /// <summary>
    /// Base class for a settings dialog that contains built in support for settings audio and effect volumes and restoring 
    /// in app purchases.
    /// 
    /// You can override this class to add additional functionality.
    /// </summary>
    [RequireComponent(typeof(DialogInstance))]
    public class Settings : Singleton<Settings>
    {
        GameObject _canvas;
        UnityEngine.UI.Slider _musicVolume, _sfxVolume;
        protected DialogInstance DialogInstance;
        float _oldBackGroundAudioVolume;

        protected override void GameSetup()
        {
            DialogInstance = GetComponent<DialogInstance>();

            Assert.IsNotNull(DialogInstance.DialogGameObject, "Ensure that you have set the script execution order of dialog instance in settings (see help for details.");

            _musicVolume = GameObjectHelper.GetChildComponentOnNamedGameObject<UnityEngine.UI.Slider>(DialogInstance.DialogGameObject, "MusicSlider", true);
            _sfxVolume = GameObjectHelper.GetChildComponentOnNamedGameObject<UnityEngine.UI.Slider>(DialogInstance.DialogGameObject, "SfxSlider", true);
        }

        public virtual void Show()
        {
            // set values
            _oldBackGroundAudioVolume = GameManager.Instance.BackGroundAudioVolume;
            _musicVolume.value = GameManager.Instance.BackGroundAudioVolume;
            _sfxVolume.value = GameManager.Instance.EffectAudioVolume;

            DialogInstance.Show(doneCallback: DoneCallback, destroyOnClose: false);
        }

        public virtual void DoneCallback(DialogInstance dialogInstance)
        {
            if (DialogInstance.DialogResult == DialogInstance.DialogResultType.Ok)
            {
                // set values
                GameManager.Instance.BackGroundAudioVolume = _musicVolume.value;
                GameManager.Instance.EffectAudioVolume = _sfxVolume.value;
                PlayerPrefs.Save();
            }
            else
            {
                GameManager.Instance.BackGroundAudioVolume = _oldBackGroundAudioVolume;
            }
        }

        public void MusicVolumeChanged()
        {
            GameManager.Instance.BackGroundAudioVolume = _musicVolume.value;
        }

        public void RestorePurchases()
        {
#if UNITY_PURCHASING
            PaymentManager.Instance.RestorePurchases();
#endif
        }
    }
}
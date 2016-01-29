//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

#if UNITY_PURCHASING
using FlipWebApps.GameFramework.Scripts.Billing.Components;
#endif
using System.Linq;
using FlipWebApps.GameFramework.Scripts.GameObjects;
using FlipWebApps.GameFramework.Scripts.GameObjects.Components;
using FlipWebApps.GameFramework.Scripts.GameStructure;
using FlipWebApps.GameFramework.Scripts.Localisation;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

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
        UnityEngine.UI.Dropdown _language;

        protected DialogInstance DialogInstance;
        float _oldBackGroundAudioVolume;

        protected override void GameSetup()
        {
            DialogInstance = GetComponent<DialogInstance>();

            Assert.IsNotNull(DialogInstance.DialogGameObject, "Ensure that you have set the script execution order of dialog instance in settings (see help for details.");

            _musicVolume = GameObjectHelper.GetChildComponentOnNamedGameObject<UnityEngine.UI.Slider>(DialogInstance.DialogGameObject, "MusicSlider", true);
            _sfxVolume = GameObjectHelper.GetChildComponentOnNamedGameObject<UnityEngine.UI.Slider>(DialogInstance.DialogGameObject, "SfxSlider", true);
            _language = GameObjectHelper.GetChildComponentOnNamedGameObject<UnityEngine.UI.Dropdown>(DialogInstance.DialogGameObject, "LanguageDropdown", true);
        }

        public virtual void Show()
        {
            // set values
            _oldBackGroundAudioVolume = GameManager.Instance.BackGroundAudioVolume;
            _musicVolume.value = GameManager.Instance.BackGroundAudioVolume;
            _sfxVolume.value = GameManager.Instance.EffectAudioVolume;
            _language.options = (from item in LocaliseText.AllowedLanguages select new Dropdown.OptionData(item)).ToList();
            for (int i = 0; i < _language.options.Count; i++)
                if (_language.options[i].text == LocaliseText.Language)
                    _language.value = i;

            DialogInstance.Show(doneCallback: DoneCallback, destroyOnClose: false);
        }

        public virtual void DoneCallback(DialogInstance dialogInstance)
        {
            if (DialogInstance.DialogResult == DialogInstance.DialogResultType.Ok)
            {
                // set values not updated automatically
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

        public void LanguageChanged(int index)
        {
            LocaliseText.Language = _language.options[index].text;
        }

        public void RestorePurchases()
        {
#if UNITY_PURCHASING
            PaymentManager.Instance.RestorePurchases();
#endif
        }
    }
}
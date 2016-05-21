//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//
// Please direct any bugs/comments/suggestions to http://www.flipwebapps.com
// 
// The copyright owner grants to the end user a non-exclusive, worldwide, and perpetual license to this Asset
// to integrate only as incorporated and embedded components of electronic games and interactive media and 
// distribute such electronic game and interactive media. End user may modify Assets. End user may otherwise 
// not reproduce, distribute, sublicense, rent, lease or lend the Assets. It is emphasized that the end 
// user shall not be entitled to distribute or transfer in any way (including, without, limitation by way of 
// sublicense) the Assets in any other way than as integrated components of electronic games and interactive media. 

// The above copyright notice and this permission notice must not be removed from any files.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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
    [AddComponentMenu("Game Framework/UI/Dialogs/Settings")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
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
            _language.options = (from item in LocaliseText.AllowedLanguages select new Dropdown.OptionData(LocaliseText.Get("Language.LocalisedName", item))).ToList();
            for (var i = 0; i < LocaliseText.AllowedLanguages.Length; i++)
                if (LocaliseText.AllowedLanguages[i] == LocaliseText.Language)
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
            LocaliseText.Language = LocaliseText.AllowedLanguages[index];
        }

        public void RestorePurchases()
        {
#if UNITY_PURCHASING
            PaymentManager.Instance.RestorePurchases();
#endif
        }
    }
}
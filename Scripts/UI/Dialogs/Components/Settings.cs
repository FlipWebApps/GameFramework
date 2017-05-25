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
using GameFramework.Billing.Components;
#endif
using System.Linq;
using GameFramework.GameObjects;
using GameFramework.GameObjects.Components;
using GameFramework.GameStructure;
using GameFramework.Localisation;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using GameFramework.Preferences;

namespace GameFramework.UI.Dialogs.Components
{
    /// <summary>
    /// Provides functionality for displaying and managing a settings dialog that contains built in support for settings audio and effect volumes and restoring 
    /// in app purchases.
    /// </summary>
    /// You can override this class to add additional functionality.
    [RequireComponent(typeof(DialogInstance))]
    [AddComponentMenu("Game Framework/UI/Dialogs/Settings")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/ui/dialogs/")]
    public class Settings : Singleton<Settings>
    {
        /// <summary>
        /// The DialogInstance associated with the settings dialog.
        /// </summary>
        protected DialogInstance DialogInstance;

        Slider _musicVolume, _sfxVolume;
        Dropdown _language;
        float _oldBackGroundAudioVolume;


        /// <summary>
        /// Called when this instance is created for one time initialisation.
        /// </summary>
        /// Override this in your own base class if you want to customise the settings window. Be sure to call this base instance first.
        protected override void GameSetup()
        {
            DialogInstance = GetComponent<DialogInstance>();
            Assert.IsNotNull(DialogInstance.Target, "Ensure that you have set the script execution order of dialog instance in project settings (see help for details.");

            _musicVolume = GameObjectHelper.GetChildComponentOnNamedGameObject<Slider>(DialogInstance.Target, "MusicSlider", true);
            _sfxVolume = GameObjectHelper.GetChildComponentOnNamedGameObject<Slider>(DialogInstance.Target, "SfxSlider", true);
            _language = GameObjectHelper.GetChildComponentOnNamedGameObject<Dropdown>(DialogInstance.Target, "LanguageDropdown", true);
        }


        /// <summary>
        /// Shows the settings dialog.
        /// </summary>
        /// Override this in your own base class if you want to customise the settings window. Be sure to call this base instance when done.
        public virtual void Show()
        {
            // save any values in case of cancel
            _oldBackGroundAudioVolume = GameManager.Instance.BackGroundAudioVolume;

            // set values in UI
            _musicVolume.value = GameManager.Instance.BackGroundAudioVolume;
            _sfxVolume.value = GameManager.Instance.EffectAudioVolume;
            _language.options = (from item in GlobalLocalisation.SupportedLanguages select new Dropdown.OptionData(GlobalLocalisation.GetText("Language.LocalisedName", item, missingReturnsKey: true))).ToList();
            for (var i = 0; i < GlobalLocalisation.SupportedLanguages.Length; i++)
                if (GlobalLocalisation.SupportedLanguages[i] == GlobalLocalisation.Language)
                    _language.value = i;

            // show the dialog
            DialogInstance.Show(doneCallback: DoneCallback, destroyOnClose: false);
        }


        /// <summary>
        /// Called when the dialog completes so you can save values or do other adjustments.
        /// </summary>
        /// Check DialogInstance.DialogResult to find out the action that caused the dialog to complete.
        /// Override this in your own base class if you want to customise the settings window. Be sure to call this base instance when done.
        public virtual void DoneCallback(DialogInstance dialogInstance)
        {
            if (DialogInstance.DialogResult == DialogInstance.DialogResultType.Ok)
            {
                // set values not updated automatically
                GameManager.Instance.EffectAudioVolume = _sfxVolume.value;
                PreferencesFactory.Save();
            }
            else
            {
                GameManager.Instance.BackGroundAudioVolume = _oldBackGroundAudioVolume;
            }
        }


        /// <summary>
        /// Callback for when the music volume is changed to preview the difference immediately.
        /// </summary>
        public void MusicVolumeChanged()
        {
            GameManager.Instance.BackGroundAudioVolume = _musicVolume.value;
        }


        /// <summary>
        /// Callback for when the language is changed to preview the difference immediately.
        /// </summary>
        public void LanguageChanged(int index)
        {
            GlobalLocalisation.Language = GlobalLocalisation.SupportedLanguages[index];
        }


        /// <summary>
        /// Callback for restoring purchases.
        /// </summary>
        public void RestorePurchases()
        {
#if UNITY_PURCHASING
            PaymentManager.Instance.RestorePurchases();
#endif
        }
    }
}
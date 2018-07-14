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

using System;
using System.Linq;
using GameFramework.GameObjects.Components;
using GameFramework.GameStructure;
using UnityEngine;
using UnityEngine.Assertions;
using GameFramework.Preferences;
using GameFramework.Localisation.ObjectModel;

namespace GameFramework.UI.Dialogs.Components
{
    /// <summary>
    /// Provides dialog creation, display and management functionality.
    /// </summary>
    [AddComponentMenu("Game Framework/UI/Dialogs/DialogManager")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/ui/dialogs/")]
    public class DialogManager : Singleton<DialogManager>
    {
        #region editor fields

        /// <summary>
        /// An optional UI camera that should be substituted into created dialogs
        /// </summary>
        [Tooltip("An optional UI camera that should be substituted into created dialogs")]
        public Camera UiCamera;

        /// <summary>
        /// Use the dialog overrides for setting what prefabs should be used. We do this rather than loading from resources so that we don't
        /// unclude unnecessary assets.
        /// </summary>
        [Tooltip("for setting what prefabs should be used for the different dialogs.")]
        public DialogPrefabOverride[] DialogPrefabOverrides =
        {
            new DialogPrefabOverride { Path = "GeneralMessage"},
            new DialogPrefabOverride { Path = "GameFeedbackDialog"}
        };

        #endregion editor fields

        /// <summary>
        /// A counter of the number of dialogs that DialogManager is currently displaying
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Show a dialog one time only using the specified dialog as a key for identifying this instance.
        /// </summary>
        /// <param name="dialogKey"></param>
        /// <param name="prefab"></param>
        /// <param name="title"></param>
        /// <param name="titleKey"></param>
        /// <param name="text"></param>
        /// <param name="textKey"></param>
        /// <param name="text2"></param>
        /// <param name="text2Key"></param>
        /// <param name="sprite"></param>
        /// <param name="doneCallback"></param>
        /// <param name="dialogButtons"></param>
        /// <returns></returns>
        public DialogInstance ShowOnce(string dialogKey, string prefab = null, string title = null, string titleKey = null, string text = null, string textKey = null, string text2 = null, string text2Key = null, Sprite sprite = null, Action<DialogInstance> doneCallback = null, DialogInstance.DialogButtonsType dialogButtons = DialogInstance.DialogButtonsType.Ok)
        {
            // show hint panel first time only
            if (PreferencesFactory.GetInt("GeneralMessage." + dialogKey, 0) == 0)
            {
                PreferencesFactory.SetInt("GeneralMessage." + dialogKey, 1);
                PreferencesFactory.Save();

                return Show(prefab, title, titleKey, text, textKey, text2, text2Key, sprite, doneCallback, dialogButtons);
            }
            return null;
        }

        /// <summary>
        /// Show a general error dialog with passed content
        /// </summary>
        /// <param name="text"></param>
        /// <param name="textKey"></param>
        /// <param name="text2"></param>
        /// <param name="text2Key"></param>
        /// <param name="sprite"></param>
        /// <param name="doneCallback"></param>
        /// <returns></returns>
        public DialogInstance ShowError(string text = null, string textKey = null, string text2 = null, string text2Key = null, Sprite sprite = null, Action<DialogInstance> doneCallback = null)
        {
            return Show(null, null, "GeneralMessage.Error.Title", text, textKey, text2, text2Key, sprite, doneCallback);
        }

        /// <summary>
        /// Show a general info dialog with passed content
        /// </summary>
        /// <param name="text"></param>
        /// <param name="textKey"></param>
        /// <param name="text2"></param>
        /// <param name="text2Key"></param>
        /// <param name="sprite"></param>
        /// <param name="doneCallback"></param>
        /// <returns></returns>
        public DialogInstance ShowInfo(string text = null, string textKey = null, string text2 = null, string text2Key = null, Sprite sprite = null, Action<DialogInstance> doneCallback = null)
        {
            return Show(null, null, "GeneralMessage.Info.Title", text, textKey, text2, text2Key, sprite, doneCallback);
        }

        /// <summary>
        /// Show a dialog with passed content, defaulting to the general message dialog if no prefabName is passed.
        /// </summary>
        /// <param name="prefabName"></param>
        /// <param name="title"></param>
        /// <param name="titleKey"></param>
        /// <param name="text"></param>
        /// <param name="textKey"></param>
        /// <param name="text2"></param>
        /// <param name="text2Key"></param>
        /// <param name="sprite"></param>
        /// <param name="doneCallback"></param>
        /// <param name="dialogButtons"></param>
        /// <param name="buttonText"></param>
        /// <returns></returns>
        public DialogInstance Show(string prefabName = null, string title = null, string titleKey = null, string text = null, string textKey = null, string text2 = null, string text2Key = null, Sprite sprite = null, Action<DialogInstance> doneCallback = null, DialogInstance.DialogButtonsType dialogButtons = DialogInstance.DialogButtonsType.Ok, LocalisableText[] buttonText = null)
        {
            var dialogInstance = Create(prefabName);
            dialogInstance.Show(title, titleKey, text, textKey, text2, text2Key, sprite, doneCallback, dialogButtons: dialogButtons, buttonText: buttonText);
            return dialogInstance;
        }


        /// <summary>
        /// Create an instance of the specified named prefab, or use the default GeneralMessage prefab if not found.
        /// </summary>
        /// <param name="prefabName"></param>
        /// <returns></returns>
        public DialogInstance Create(string prefabName)
        {
            return string.IsNullOrEmpty(prefabName) ?
                Create() :
                Create(LoadPrefab(prefabName));
        }


        /// <summary>
        /// Create an instance of the given dialog prefab, or use the default GeneralMessage prefab if nothing is specified.
        /// </summary>
        /// <param name="dialogPrefab"></param>
        /// <returns></returns>
        public DialogInstance Create(GameObject dialogPrefab = null)
        {
            if (dialogPrefab == null) dialogPrefab = LoadPrefab("GeneralMessage");

            // create prefab instance
            var dialogPrefabInstance = Instantiate(dialogPrefab);
            dialogPrefabInstance.transform.SetParent(transform);
            dialogPrefabInstance.transform.localPosition = Vector3.zero;

            // set camera
            if (UiCamera != null)
            {
                var canvas = dialogPrefabInstance.GetComponentInChildren<Canvas>(true);
                if (canvas != null)
                    canvas.worldCamera = UiCamera;
            }

            return dialogPrefabInstance.GetComponent<DialogInstance>();
        }


        /// <summary>
        /// Create an instance of the specified dialog
        /// </summary>
        /// <param name="prefabName"></param>
        /// <param name="contentPrefabName"></param>
        /// <param name="contentPrefab"></param>
        /// <param name="contentSiblingIndex"></param>
        /// <param name="runtimeAnimatorController"></param>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public DialogInstance Create(GameObject prefab, string prefabName, GameObject contentPrefab, string contentPrefabName, int contentSiblingIndex = 2, RuntimeAnimatorController runtimeAnimatorController = null)
        {
            // create and get dialog Instance
            var dialogInstance = prefab != null ? Create(prefab) : Create(prefabName);

            // if no content prefab then get from name if specified.
            if (contentPrefab == null && contentPrefabName != null)
            {
                contentPrefab = LoadPrefab("Content/" + contentPrefabName);
                Assert.IsNotNull(contentPrefab, "Unable to find named content prefab 'Content/" + contentPrefabName + "'");
            }

            // add custom content
            if (contentPrefab != null)
            {
                dialogInstance.CustomContentItem = Instantiate(contentPrefab);
                dialogInstance.CustomContentItem.name = contentPrefab.name;                           // copy name so animation work.
                dialogInstance.CustomContentItem.transform.SetParent(dialogInstance.Content.transform, false);
                dialogInstance.CustomContentItem.transform.localPosition = Vector3.zero;
                if (contentSiblingIndex != -1)
                    dialogInstance.CustomContentItem.transform.SetSiblingIndex(contentSiblingIndex);
            }

            // add custom content animator
            if (runtimeAnimatorController != null)
            {
                dialogInstance.ContentAnimator.runtimeAnimatorController = runtimeAnimatorController;
            }

            return dialogInstance;
        }


        /// <summary>
        /// Load the prefab for the given prefab name
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        GameObject LoadPrefab(string prefab)
        {
            foreach (var dialogOverride in DialogPrefabOverrides.Where(dialogOverride => dialogOverride.Path == prefab && dialogOverride.Prefab != null))
                return dialogOverride.Prefab;

            return GameManager.LoadResource<GameObject>("Dialog/" + prefab);
        }
    }


    [Serializable]
    public class DialogPrefabOverride
    {
        public string Path;
        public GameObject Prefab;
    }
}
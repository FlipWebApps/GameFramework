//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using System;
using System.Linq;
using FlipWebApps.GameFramework.Scripts.GameObjects;
using FlipWebApps.GameFramework.Scripts.GameObjects.Components;
using FlipWebApps.GameFramework.Scripts.GameStructure;
using UnityEngine;
using UnityEngine.Assertions;

namespace FlipWebApps.GameFramework.Scripts.UI.Dialogs.Components
{
    public class DialogManager : Singleton<DialogManager>
    {
        public int Count;
        public Camera UiCamera;

        /// <summary>
        /// Use the dialog overrides for setting what prefabs should be used. We do this rather than loading from resources so that we don't
        /// unclude unnecessary assets.
        /// </summary>
        public DialogPrefabOverride[] DialogPrefabOverrides =
        {
            new DialogPrefabOverride { Path = "GeneralMessage"},
            new DialogPrefabOverride { Path = "GameFeedbackDialog"}
        };

        public DialogPrefabOverride[] DialogContentPrefabOverrides =
        {
            new DialogPrefabOverride { Path = "UnlockLevelPlaceHolder"}
        };

        public DialogInstance ShowOnce(string dialogKey, string prefab = null, string title = null, string titleKey = null, string text = null, string textKey = null, string text2 = null, string text2Key = null, Sprite sprite = null, System.Action<DialogInstance> doneCallback = null, DialogInstance.DialogButtonsType dialogButtons = DialogInstance.DialogButtonsType.Ok)
        {
            // show hint panel first time only
            if (PlayerPrefs.GetInt("GeneralMessage." + dialogKey, 0) == 0)
            {
                PlayerPrefs.SetInt("GeneralMessage." + dialogKey, 1);
                PlayerPrefs.Save();

                return Show(prefab, title, titleKey, text, textKey, text2, text2Key, sprite, doneCallback, dialogButtons);
            }
            return null;
        }

        public DialogInstance ShowError(string text = null, string textKey = null, string text2 = null, string text2Key = null, Sprite sprite = null, System.Action<DialogInstance> doneCallback = null)
        {
            return Show(null, null, "GeneralMessage.Error.Title", text, textKey, text2, text2Key, sprite, doneCallback);
        }

        public DialogInstance ShowInfo(string text = null, string textKey = null, string text2 = null, string text2Key = null, Sprite sprite = null, System.Action<DialogInstance> doneCallback = null)
        {
            return Show(null, null, "GeneralMessage.Info.Title", text, textKey, text2, text2Key, sprite, doneCallback);
        }

        public DialogInstance Show(string prefab = null, string title = null, string titleKey = null, string text = null, string textKey = null, string text2 = null, string text2Key = null, Sprite sprite = null, System.Action<DialogInstance> doneCallback = null, DialogInstance.DialogButtonsType dialogButtons = DialogInstance.DialogButtonsType.Ok)
        {
            DialogInstance generalMessage = Create(prefab);
            generalMessage.Show(title, titleKey, text, textKey, text2, text2Key, sprite, doneCallback, dialogButtons: dialogButtons);
            return generalMessage;
        }


        /// <summary>
        /// Create an instance of the given dialog prefab
        /// </summary>
        /// <param name="dialogPrefab"></param>
        /// <returns></returns>
        public DialogInstance Create(GameObject dialogPrefab = null)
        {
            if (dialogPrefab == null) dialogPrefab = GetPrefab("GeneralMessage");

            var dialogPrefabInstance = Instantiate(dialogPrefab);
            dialogPrefabInstance.transform.SetParent(transform);
            dialogPrefabInstance.transform.localPosition = Vector3.zero;

            var canvas = dialogPrefabInstance.GetComponentInChildren<Canvas>(true);
            if (canvas != null)
                canvas.worldCamera = UiCamera;

            return dialogPrefabInstance.GetComponent<DialogInstance>();
        }


        /// <summary>
        /// Create an instance of the specified named prefab, or use the default GeneralMessage prefab if nothing is specified.
        /// </summary>
        /// <param name="prefabName"></param>
        /// <returns></returns>
        public DialogInstance Create(string prefabName)
        {
            return Create(GetPrefab(prefabName));
        }


        /// <summary>
        /// 
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
                contentPrefab = GetPrefab("Content/" + contentPrefabName);
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

        GameObject GetPrefab(string prefab)
        {
            foreach (var dialogOverride in DialogPrefabOverrides.Where(dialogOverride => dialogOverride.Path == prefab && dialogOverride.Prefab != null))
                return dialogOverride.Prefab;
            foreach (var dialogOverride in DialogContentPrefabOverrides.Where(dialogOverride => ("Content/" + dialogOverride.Path) == prefab && dialogOverride.Prefab != null))
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
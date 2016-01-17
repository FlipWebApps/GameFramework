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
            new DialogPrefabOverride { Path = "FreePrizePlaceHolder"},
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


        public DialogInstance Create(string prefab = null)
        {
            if (string.IsNullOrEmpty(prefab))
                prefab = "GeneralMessage";
            GameObject messageWindow = GetPrefab(prefab);
            messageWindow.transform.SetParent(transform);
            messageWindow.transform.localPosition = Vector3.zero;

            Canvas canvas = messageWindow.GetComponent<Canvas>();
            if (canvas != null)
                canvas.worldCamera = UiCamera;

            DialogInstance generalMessage = messageWindow.GetComponent<DialogInstance>();
            return generalMessage;
        }

        public DialogInstance Create(string prefab, string contentPrefab, int siblingIndex = -1)
        {
            DialogInstance generalMessage = Create(prefab);
            generalMessage.ContentItem = GetPrefab("Content/" + contentPrefab);
                //Instantiate(Resources.Load("Dialog/Content/" + contentPrefab, typeof(GameObject))) as GameObject ??
                //                       Instantiate(Resources.Load("Dialog/Content/" + contentPrefab + "-Default", typeof(GameObject))) as GameObject;
            generalMessage.ContentItem.transform.SetParent(generalMessage.Content.transform, false);
            generalMessage.ContentItem.transform.localPosition = Vector3.zero;

            if (siblingIndex != -1)
                generalMessage.ContentItem.transform.SetSiblingIndex(siblingIndex);
            
            return generalMessage;
        }

        GameObject GetPrefab(string prefab)
        {
            foreach (var dialogOverride in DialogPrefabOverrides.Where(dialogOverride => dialogOverride.Path == prefab && dialogOverride.Prefab != null))
                return Instantiate(dialogOverride.Prefab);
            foreach (var dialogOverride in DialogPrefabOverrides.Where(dialogOverride => ("Content/" + dialogOverride.Path) == prefab && dialogOverride.Prefab != null))
                return Instantiate(dialogOverride.Prefab);

            var prefabGameObject = GameManager.LoadResource<GameObject>("Dialog/" + prefab);
            return prefabGameObject == null ? null : Instantiate(prefabGameObject);
        }
    }


    [Serializable]
    public class DialogPrefabOverride
    {
        public string Path;
        public GameObject Prefab;
    }
}
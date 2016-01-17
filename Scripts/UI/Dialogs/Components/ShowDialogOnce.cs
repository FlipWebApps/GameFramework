//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;
using UnityEngine.Assertions;

namespace FlipWebApps.GameFramework.Scripts.UI.Dialogs.Components
{
    /// <summary>
    /// Shows the given dialog a single time
    /// </summary>
    public class ShowDialogOnce : MonoBehaviour
    {
        public string DialogKey;
        public string Prefab;
        public string TitleKey;
        public string TextKey;
        public string Text2Key;
        public Sprite Sprite;

        void Start()
        {
            Assert.IsTrue(DialogManager.IsActive, "To use the ShowDialogOnce component, you must have a DialogManager added to your scene.");

            DialogManager.Instance.ShowOnce(DialogKey, Prefab, titleKey: TitleKey, textKey: TextKey, text2Key: Text2Key,
                sprite: Sprite);
        }
    }
}
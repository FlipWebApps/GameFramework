//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameObjects;
using UnityEngine;
using UnityEngine.Assertions;

namespace FlipWebApps.GameFramework.Scripts.UI.Dialogs.Components
{
    /// <summary>
    /// Call back that will show the specified dialog buttons. 
    /// 
    /// This might typically be triggered from an animation to only show buttons dialog after an animation is
    /// shown. This can be used to stop the user clicking and exiting a dialog before we have shown what we
    /// want to show.
    /// </summary>
    public class DialogCallbackShowButtons : MonoBehaviour
    {
        public DialogInstance.DialogButtonsType Buttons = DialogInstance.DialogButtonsType.Ok;

        void ShowDialogButtons()
        {
            Assert.AreEqual(DialogInstance.DialogButtonsType.Ok, Buttons, "Currently only Ok button is supported.");

            var panelGameobject = GameObjectHelper.GetParentNamedGameObject(gameObject, "Panel");
            Assert.IsNotNull(panelGameobject, "A Dialog must follow certain naming conventions. Missing Panel.");
            GameObjectHelper.GetChildNamedGameObject(panelGameobject, "OkButton", true).SetActive(true);
        }
    }
}
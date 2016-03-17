//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.Input.Components;

namespace FlipWebApps.GameFramework.Scripts.UI.Dialogs.Components
{
    /// <summary>
    /// Loads the given level when a mouse button is pressed or the screen is tapped anywhere on teh screen
    /// 
    /// By setting a list of UI game objects you can set areas that wonwill block the change.
    /// </summary>
    public class OnMouseClickOrTapSwapDialogInstance : OnMouseClickOrTap
    {
        public DialogInstance Source;
        public DialogInstance Target;

        public override void RunMethod() {
            if (Source.IsShown)
            {
                Source.SwapTo(Target);
            }
        }
    }
}
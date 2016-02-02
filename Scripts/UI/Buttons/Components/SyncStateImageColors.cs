//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FlipWebApps.GameFramework.Scripts.UI.Buttons.Components
{
    public class SyncStateImageColors : SyncState
    {
        public Color DisabledColor;
        public Color HighlightedColor;
        public Color PressedColor;

        Color _normalColor;

        public Image[] Images;

        new void Awake()
        {
            Assert.AreNotEqual(0, Images.Length, "Please specify the images that you would like to sync the button state with.");

            _normalColor = Images[0].color;

            base.Awake();
        }

        public override void StateChanged()
        {
            Color imageColor;

            if (IsInteractable)
            {
                if (IsPointerOver || IsSelected)
                {
                    if (IsPointerOver && IsPointerDown)
                    {
                        imageColor = PressedColor;
                    }
                    else
                    {
                        imageColor = HighlightedColor;
                    }
                }
                else
                {
                    imageColor = _normalColor;
                }
            }
            else
            {
                imageColor = DisabledColor;
            }

            // update colours
            foreach (var image in Images)
            {
                image.color = imageColor;
            }
        }
    }
}
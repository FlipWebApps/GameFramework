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
    [RequireComponent(typeof(Button))]
    public class SyncStateTextColors : SyncState
    {
        [Header("Text Color")]
        public Color DisabledTextColor;
        public Color HighlightedTextColor;
        public Color PressedTextColor;

        [Header("Shadow Color")]
        public Color DisabledShadowColor;
        public Color HighlightedShadowColor;
        public Color PressedShadowColor;

        [Header("Outline Color")]
        public Color DisabledOutlineColor;
        public Color HighlightedOutlineColor;
        public Color PressedOutlineColor;

        Color _normalTextColor;
        Color _normalShadowColor;
        Color _normalOutlineColor;

        Text[] _texts;
        Shadow[] _shadows;
        Outline[] _outlines;

        new void Awake()
        {
            _texts = GetComponentsInChildren<Text>(true);
            Assert.AreNotEqual(0, _texts.Length, "No child Text components found.");
            _normalTextColor = _texts[0].color;

            _shadows = GetComponentsInChildren<Shadow>(true);
            if (_shadows.Length > 0)
                _normalShadowColor = _shadows[0].effectColor;

            _outlines = GetComponentsInChildren<Outline>(true);
            if (_shadows.Length > 0)
                _normalOutlineColor = _shadows[0].effectColor;

            base.Awake();
        }

        public override void StateChanged()
        {
            // determin colour based upon state.
            Color textColor;
            Color shadowColor;
            Color outlineColor;
            if (IsInteractable)
            {
                if (IsPointerOver || IsSelected)
                {
                    if (IsPointerOver &&IsPointerDown)
                    {
                        textColor = PressedTextColor;
                        shadowColor = PressedShadowColor;
                        outlineColor = PressedOutlineColor;
                    }
                    else
                    {
                        textColor = HighlightedTextColor;
                        shadowColor = HighlightedShadowColor;
                        outlineColor = HighlightedOutlineColor;
                    }
                }
                else
                {
                    textColor = _normalTextColor;
                    shadowColor = _normalShadowColor;
                    outlineColor = _normalOutlineColor;
                }
            }
            else
            {
                textColor = DisabledTextColor;
                shadowColor = DisabledShadowColor;
                outlineColor = DisabledOutlineColor;
            }

            // update colours
            foreach (var text in _texts)
            {
                text.color = textColor;
            }
            foreach (var shadow in _shadows)
            {
                shadow.effectColor = shadowColor;
            }
            foreach (var outline in _outlines)
            {
                outline.effectColor = outlineColor;
            }
        }
    }
}
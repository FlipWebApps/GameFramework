//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FlipWebApps.GameFramework.Scripts.UI.Other.Components
{
    public class NestedTextButtonColours : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Color DisabledTextColor, HoverTextColor;
        public bool ApplyToText;    // needed for backwards compatibility where we haven't necessarily set the colour.
        public Color DisabledEffectColor, HoverEffectColor;

        Button _button;
        bool _isInteractable;
        bool _isPointerOver;

        Color _normalTextColor;
        Color _normalEffectColor;

        Text[] _texts;
        Shadow[] _shadows;
        Outline[] _outlines;

        void Awake()
        {
            _button = GetComponent<Button>();
            _isInteractable = _button.interactable;
            _isPointerOver = false;

            _texts = GetComponentsInChildren<Text>(true);
            _normalTextColor = _texts[0].color;

            _shadows = GetComponentsInChildren<Shadow>(true);
            _outlines = GetComponentsInChildren<Outline>(true);
            if (_shadows.Length > 0)
                _normalEffectColor = _shadows[0].effectColor;

            SetColours();
        }

        void Update()
        {
            if (_button.interactable != _isInteractable)
            {
                _isInteractable = _button.interactable;
                SetColours();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isPointerOver = true;
            SetColours();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isPointerOver = false;
            SetColours();
        }

        void SetColours()
        {
            Color textColor;
            Color effectColor;
            if (_isInteractable)
            {
                if (_isPointerOver)
                {
                    textColor = HoverTextColor;
                    effectColor = HoverEffectColor;
                }
                else
                {
                    textColor = _normalTextColor;
                    effectColor = _normalEffectColor;
                }
            }
            else
            {
                textColor = DisabledTextColor;
                effectColor = DisabledEffectColor;
            }

            if (ApplyToText)
            {
                foreach (Text text in _texts)
                {
                    text.color = textColor;
                }
            }

            foreach (Shadow shadow in _shadows)
            {
                shadow.effectColor = effectColor;
            }
            foreach (Outline outline in _outlines)
            {
                outline.effectColor = effectColor;
            }
        }
    }
}
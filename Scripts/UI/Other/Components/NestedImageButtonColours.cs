//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FlipWebApps.GameFramework.Scripts.UI.Other.Components
{
    public class NestedImageButtonColours : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Button Button;
        public Color DisabledColor, HoverColor;

        bool _isButtonInteractable;
        bool _isPointerOverButton;

        Image _image;
        Color _normalColor;

        void Awake()
        {
            _isButtonInteractable = Button.interactable;
            _isPointerOverButton = false;

            _image = GetComponent<Image>();
            _normalColor = _image.color;

            SetColours();
        }

        void Update()
        {
            if (Button.interactable != _isButtonInteractable)
            {
                _isButtonInteractable = Button.interactable;
                SetColours();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isPointerOverButton = true;
            SetColours();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isPointerOverButton = false;
            SetColours();
        }

        void SetColours()
        {
            Color imageColor;
            if (_isButtonInteractable)
            {
                imageColor = _isPointerOverButton ? HoverColor : _normalColor;
            }
            else
            {
                imageColor = DisabledColor;
            }

            _image.color = imageColor;
        }
    }
}
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
    public abstract class SyncState : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IPointerDownHandler, IPointerUpHandler
    {
        Button _button;
        protected bool IsInteractable;
        protected bool IsPointerOver;
        protected bool IsPointerDown;
        protected bool IsSelected;

        public void Awake()
        {
            _button = GetComponent<Button>();
            IsInteractable = _button.interactable;

            StateChanged();
        }

        public void Update()
        {
            if (_button.interactable == IsInteractable) return;

            IsInteractable = _button.interactable;
            StateChanged();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            IsPointerOver = true;
            StateChanged();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsPointerOver = false;
            StateChanged();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            IsPointerDown = true;
            StateChanged();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsPointerDown = false;
            StateChanged();
        }

        public void OnSelect(BaseEventData eventData)
        {
            IsSelected = true;
            StateChanged();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            IsSelected = false;
            StateChanged();
        }

        public abstract void StateChanged();
    }
}
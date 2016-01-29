//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Localisation.Components
{
    /// <summary>
    /// Localises a Text field based upon the given Key
    /// </summary>
    [RequireComponent(typeof(UnityEngine.UI.Text))]
    public class LocaliseText : MonoBehaviour
    {
        /// <summary>
        /// Localization key.
        /// </summary>
        public string Key;

        /// <summary>
        /// Manually change the value of whatever the localization component is attached to.
        /// </summary>
        public string Value
        {
            set
            {
                if (string.IsNullOrEmpty(value)) return;

                _textComponent.text = value;
            }
        }

        UnityEngine.UI.Text _textComponent;

        void Awake()
        {
            _textComponent = GetComponent<UnityEngine.UI.Text>();

            // If no localization key has been specified, use the text value as the key
            if (string.IsNullOrEmpty(Key))
            {
                Key = _textComponent.text;
            }
        }

        /// <summary>
        /// Localize the widget in OnEnable so we don't miss notifications (onStart is only called once).
        /// </summary>
        void OnEnable()
        {
            OnLocalise();
            Localisation.LocaliseText.OnLocalise += OnLocalise;
        }

        void OnDisable()
        {
            Localisation.LocaliseText.OnLocalise -= OnLocalise;
        }

        /// <summary>
        /// Update the display with the localise text
        /// </summary>
        void OnLocalise()
        {
            // If we don't have a key then don't change the value
            if (!string.IsNullOrEmpty(Key)) Value = Localisation.LocaliseText.Get(Key);
        }
    }
}
//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Localisation.Components
{
    /// <summary>
    /// Localises a Text input based upon the given Key
    /// TODO Add automatic notifications when localisation changes e.g. new Language selected.
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
                if (!string.IsNullOrEmpty(value))
                {
                    UnityEngine.UI.Text text = GetComponent<UnityEngine.UI.Text>();

                    if (text != null)
                    {
                        text.text = value;
                    }
                }
            }
        }

        /// <summary>
        /// Localize the widget on start.
        /// </summary>
        void Start()
        {
            // If no localization key has been specified, use the text value as the key
            if (string.IsNullOrEmpty(Key))
            {
                UnityEngine.UI.Text text = GetComponent<UnityEngine.UI.Text>();
                if (text != null) Key = text.text;
            }

            // If we still don't have a key, leave the value as blank
            if (!string.IsNullOrEmpty(Key)) Value = Localisation.LocaliseText.Get(Key);
        }
    }
}
//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.Debugging;
using UnityEngine;
using UnityEngine.UI;

namespace FlipWebApps.GameFramework.Scripts.UI.Other.Components
{
    /// <summary>
    /// NOTE: Is this of any use?
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class ScaleTextBasedOnScreenSize : MonoBehaviour
    {
        public enum RunType { OnStart, OnEnable, OnUpdate };

        public RunType Run;
        public int MinSize, MaxSize;
        public float ReferenceSizeInInches;

        //public float ReferenceDpi, ReferenceScreenHeight;
        public float ScreenDpiEditForTesting;

        Text _text;
        float _startTextSize;

        void Awake()
        {
            _text = GetComponent<Text>();
            _startTextSize = _text.fontSize;

            ScreenDpiEditForTesting = Screen.dpi;
            if (Mathf.Approximately(ScreenDpiEditForTesting, 0))
                Debug.LogWarning("Screen.dpi is 0. Not performing text scaling");
//#if UNITY_EDITOR
//            if (ScreenDpi != ReferenceDpi)
//                Debug.LogWarning("Screen.dpi is not the same as reference dpi. this may give distorted results");
//#endif
        }

        void Start()
        {

            if (Run == RunType.OnStart)
                SetTextSize();
        }

        void OnEnable()
        {
            if (Run == RunType.OnEnable)
                SetTextSize();
        }

        void Update()
        {
            if (Run == RunType.OnUpdate)
                SetTextSize();
        }

        void SetTextSize()
        {
            if (!Mathf.Approximately(Screen.dpi, 0))
            {
                // we assume that the canvas scaler already scales the text up based upon screen size and considering dpi
                // therefor we just adjust by the physical size, so if the reference height is 5" and the new size is 10" then we half the font size.
                float screenHeightInches = Screen.height / ScreenDpiEditForTesting;
                MyDebug.Log("Estimated screen height in inches is : " + screenHeightInches);

                float scaleFactor = ReferenceSizeInInches / screenHeightInches;
                _text.fontSize = Mathf.CeilToInt(Mathf.Clamp(_startTextSize * scaleFactor, MinSize, MaxSize));

                // OLD CODE WHERE WE TRIED WORKING IN DPI etc... Not sure 100% how this should all work best!!!
                //// adjust font size by height scale and then adjust by dpi scale 
                //int size = (int)(ReferenceTextSize * (ReferenceScreenHeight / Screen.height) * (ScreenDpi / ReferenceDpi));
                //Debug.LogError("size" + ReferenceScreenHeight);
                //Debug.LogError("size" + Screen.height);
                //Debug.LogError("size" + ReferenceScreenHeight / Screen.height);
                //Debug.LogError("size" + ScreenDpi / ReferenceDpi);
                //Debug.LogError("size" + size);
                //Text.fontSize = Mathf.Clamp(size, MinSize, MaxSize);
            }
        }
    }
}
//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Display.Placement
{
    /// <summary>
    /// Size and placement conversion functions
    /// </summary>
    public static class DisplayMetrics
    {
        const float DefaultDpi = 160.0f;

        public static float GetDpi()
        {
            return Mathf.Approximately(Screen.dpi, 0) ? DefaultDpi : Screen.dpi;
        }

        public static float GetScale()
        {
            return GetDpi() / DefaultDpi;
        }

        public static float GetPhysicalHeight()
        {
            return Screen.height / GetDpi();
        }

        public static float GetPhysicalWidth()
        {
            return Screen.width / GetDpi();
        }
    }
}
//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using System;
using System.Globalization;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Display.Other { 
    /// <summary>
    /// Functions to help with color conversion
    /// </summary>
    public class ColorHelper {
        public static Color ParseColorString(string colorString)
        {
            var clr = new Color(0, 0, 0);
            if (string.IsNullOrEmpty(colorString)) return clr;

            try
            {
                var str = colorString.Substring(1, colorString.Length - 1);
                clr.r = int.Parse(str.Substring(0, 2),
                    NumberStyles.AllowHexSpecifier) / 255.0f;
                clr.g = int.Parse(str.Substring(2, 2),
                    NumberStyles.AllowHexSpecifier) / 255.0f;
                clr.b = int.Parse(str.Substring(4, 2),
                    NumberStyles.AllowHexSpecifier) / 255.0f;
                if (str.Length == 8) clr.a = int.Parse(str.Substring(6, 2),
                    NumberStyles.AllowHexSpecifier) / 255.0f;
                else clr.a = 1.0f;
            }
            catch (Exception e)
            {
                Debug.Log("Unable to convert " + colorString + " to Color. " + e);
                return new Color(0, 0, 0, 0);
            }
            return clr;
        }

        public static string HexString(Color color,
                bool includeAlpha = false)
        {
            return HexString((Color32)color, includeAlpha);
        }

        public static string HexString(Color32 color,
                bool includeAlpha = false)
        {
            var red = Convert.ToString(color.r, 16).ToUpper();
            var green = Convert.ToString(color.g, 16).ToUpper();
            var blue = Convert.ToString(color.b, 16).ToUpper();
            var alpha = Convert.ToString(color.a, 16).ToUpper();
            while (red.Length < 2) red = "0" + red;
            while (green.Length < 2) green = "0" + green;
            while (blue.Length < 2) blue = "0" + blue;
            while (alpha.Length < 2) alpha = "0" + alpha;

            if (includeAlpha) return "#" + red + green + blue + alpha;
            return "#" + red + green + blue;
        }
    }
}

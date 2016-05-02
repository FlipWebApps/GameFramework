//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//
// Please direct any bugs/comments/suggestions to http://www.flipwebapps.com
// 
// The copyright owner grants to the end user a non-exclusive, worldwide, and perpetual license to this Asset
// to integrate only as incorporated and embedded components of electronic games and interactive media and 
// distribute such electronic game and interactive media. End user may modify Assets. End user may otherwise 
// not reproduce, distribute, sublicense, rent, lease or lend the Assets. It is emphasized that the end 
// user shall not be entitled to distribute or transfer in any way (including, without, limitation by way of 
// sublicense) the Assets in any other way than as integrated components of electronic games and interactive media. 

// The above copyright notice and this permission notice must not be removed from any files.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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

        public static Color LerpHSV(ColorHSV a, ColorHSV b, float t)
        {
            // Hue interpolation
            float h;
            float d = b.h - a.h;
            if (a.h > b.h)
            {
                // Swap (a.h, b.h)
                var htemp = b.h;
                b.h = a.h;
                a.h = htemp;

                d = -d;
                t = 1 - t;
            }

            if (d > 0.5) // 180deg
            {
                a.h = a.h + 1; // 360deg
                h = (a.h + t * (b.h - a.h)) % 1; // 360deg
            }
            else //if (d <= 0.5) // 180deg
            {
                h = a.h + t*d;

            }

            // Interpolates the rest
            return new ColorHSV
            (
                h,          // H
                a.s + t * (b.s - a.s),  // S
                a.v + t * (b.v - a.v),  // V
                a.v + t * (b.v - a.v)   // A
            );
        }
    }
}

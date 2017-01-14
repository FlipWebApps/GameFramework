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

using UnityEngine;

/// <summary>
/// Contains components and libraries to help with positioning and movement of gameobjects and other items
/// </summary>
/// For further information please see: http://www.flipwebapps.com/unity-assets/game-framework/display/
namespace GameFramework.Display.Placement
{
    /// <summary>
    /// Size and placement conversion functions
    /// </summary>
    public static class DisplayMetrics
    {
        /// <summary>
        /// A default Dpi that is used as a reference value incase the system is not able to provide this information.
        /// </summary>
        public static float DefaultDpi = 160.0f;

        /// <summary>
        /// Whether the system Dpi is available.
        /// </summary>
        /// <returns></returns>
        public static bool IsSystemDpiAvailable()
        {
            return !Mathf.Approximately(Screen.dpi, 0);
        }

        /// <summary>
        /// Return the current Dpi, falling back to DefaultDpi if the system is not able to provide this information.
        /// </summary>
        /// <returns></returns>
        public static float GetDpi()
        {
            return IsSystemDpiAvailable() ? Screen.dpi : DefaultDpi;
        }

        /// <summary>
        /// Returns a scale factor of the current DPP against the DefaultDPI
        /// </summary>
        /// <returns></returns>
        public static float GetScale()
        {
            return GetDpi() / DefaultDpi;
        }

        /// <summary>
        /// Get the physical height of the display
        /// </summary>
        /// If the system is not able to report the Dpi then this will fall back to the DefaultDpi which may give incorrect results.
        /// Check IsSystemDpiAvailable() if necessary to verify the returned value.
        /// <returns></returns>
        public static float GetPhysicalHeight()
        {
            return Screen.height / GetDpi();
        }


        /// <summary>
        /// Get the physical width of the display
        /// </summary>
        /// If the system is not able to report the Dpi then this will fall back to the DefaultDpi which may give incorrect results.
        /// Check IsSystemDpiAvailable() if necessary to verify the returned value.
        /// <returns></returns>
        public static float GetPhysicalWidth()
        {
            return Screen.width / GetDpi();
        }
    }
}
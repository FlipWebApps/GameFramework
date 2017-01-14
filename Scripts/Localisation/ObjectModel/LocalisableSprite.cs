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
using GameFramework.Localisation.ObjectModel.AbstractClasses;
using UnityEngine;

namespace GameFramework.Localisation.ObjectModel
{
    /// <summary>
    /// Class to hold information about localisable sprites.
    /// </summary>
    [Serializable]
    public class LocalisableSprite : LocalisableObject
    {
        /// <summary>
        /// The default sprite that should be used.
        /// </summary>
        public new Sprite Default
        {
            get { return base.Default as Sprite; }
            set { base.Default = value; }
        }


        /// <summary>
        /// Get a sprite that corresponds to the currently set language
        /// </summary>
        /// <param name="fallbackToDefault">Whether to fall back to the default object if no language specific entry is found</param>
        /// <returns></returns>
        public Sprite GetSprite(bool fallbackToDefault = true)
        {
            return GetObject(fallbackToDefault) as Sprite;
        }


        /// <summary>
        /// Get a sprite that corresponds to the specified language
        /// </summary>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault">Whether to fall back to the default object if no language specific entry is found</param>
        /// <returns></returns>
        public Sprite GetSprite(SystemLanguage language, bool fallbackToDefault = true)
        {
            return GetObject(language, fallbackToDefault) as Sprite;
        }


        /// <summary>
        /// Get a sprite that corresponds to the specified language
        /// </summary>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault">Whether to fall back to the default object if no language specific entry is found</param>
        /// <returns></returns>
        public Sprite GetSprite(string language, bool fallbackToDefault = true)
        {
            return GetObject(language, fallbackToDefault) as Sprite;
        }
    }
}

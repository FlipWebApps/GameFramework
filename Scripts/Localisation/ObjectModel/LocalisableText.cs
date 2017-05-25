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
using UnityEngine;

namespace GameFramework.Localisation.ObjectModel
{
    /// <summary>
    /// Simple struture to hold information about localisable text.
    /// </summary>
    [Serializable]
    public class LocalisableText {

        /// <summary>
        /// Indicates whether this is a localisation key or an actual value.
        /// </summary>
        public bool IsLocalised
        {
            get
            {
                return _isLocalised;
            }
            set
            {
                _isLocalised = value;
            }
        }
        [Tooltip("Indicates whether this is a localisation key or an actual value.")]
        [SerializeField]
        bool _isLocalised;


        /// <summary>
        /// Data associated with this item. Use IsLocalised to determine whether this is a localisation key or an actual value.
        /// </summary>
        public string Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }
        [Tooltip("Data associated with this item. Use IsLocalised to determine whether this is a localisation key or an actual value.")]
        [SerializeField]
        string _data;


        /// <summary>
        /// Create LocalisableText
        /// </summary>
        /// <param name="isLocalised"></param>
        /// <param name="data"></param>
        public LocalisableText(bool isLocalised = false, string data = null)
        {
            IsLocalised = isLocalised;
            Data = data;
        }
        

        /// <summary>
        /// Create a non localised version of LocalisableText
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static LocalisableText CreateNonLocalised(string text = null)
        {
            return new LocalisableText(false, text);
        }


        /// <summary>
        /// Create a non localised version of LocalisableText
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static LocalisableText CreateLocalised(string text = null)
        {
            return new LocalisableText(true, text);
        }


        /// <summary>
        /// Get the actual value that this LocalisableText represents, either a physical value or the localised string.
        /// </summary>
        public string GetValue(string language = null)
        {
            return IsLocalised ? GlobalLocalisation.GetText(Data, language, true) : Data;
        }


        /// <summary>
        /// Get the actual value that this LocalisableText represents, either a physical value or the localised string formatting in passed parameters
        /// </summary>
        public string FormatValue(params object[] parameters)
        {
            return IsLocalised ? (GlobalLocalisation.FormatText(Data, parameters) ?? Data) : string.Format(Data, parameters);
        }


        /// <summary>
        /// Whether this is localised but doesn't have any key set
        /// </summary>
        /// <returns></returns>
        public bool IsLocalisedWithNoKey()
        {
            return IsLocalised && string.IsNullOrEmpty(Data);
        }
    }
}

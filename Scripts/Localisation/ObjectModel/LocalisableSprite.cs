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
using System.Collections.Generic;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Localisation.ObjectModel
{
    /// <summary>
    /// Simple struture to hold information about localisable sprites.
    /// </summary>
    [Serializable]
    public class LocalisableSprite
    {

        /// <summary>
        /// The default sprite that should be used.
        /// </summary>
        /// See GameItem for more information.
        public Sprite Default
        {
            get
            {
                return _default;
            }
            set
            {
                _default = value;
            }
        }
        [Tooltip("The default sprite that should be used.")]
        [SerializeField]
        Sprite _default;

        /// <summary>
        /// A list of sprites for different localisations.
        /// </summary>
        /// See GameItem for more information.
        public List<LocalisedSprite> LocalisedSprites
        {
            get
            {
                return _localisedItems;
            }
            set
            {
                _localisedItems = value;
            }
        }
        [Tooltip("A list of sprites for different localisations.")]
        [SerializeField]
        List<LocalisedSprite> _localisedItems;

        [Serializable]
        public class LocalisedSprite
        {
            /// <summary>
            /// The language that this sprite override is for.
            /// </summary>
            public SystemLanguage Language
            {
                get
                {
                    return _language;
                }
                set
                {
                    _language = value;
                }
            }
            [Tooltip("The language that this sprite override is for.")]
            [SerializeField]
            SystemLanguage _language;


            /// <summary>
            /// The sprite for this language
            /// </summary>
            public Sprite Sprite
            {
                get
                {
                    return _item;
                }
                set
                {
                    _item = value;
                }
            }
            [Tooltip("The sprite for this language")]
            [SerializeField]
            Sprite _item;
        }
    }
}

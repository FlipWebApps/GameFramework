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
    /// Simple struture to hold information about localisable prefabs.
    /// </summary>
    [Serializable]
    public class LocalisablePrefab {

        /// <summary>
        /// The default prefab that should be used.
        /// </summary>
        /// See GameItem for more information.
        public GameObject Default
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
        [Tooltip("The default prefab that should be used.")]
        [SerializeField]
        GameObject _default;

        /// <summary>
        /// A list of prefabs for different localisations.
        /// </summary>
        /// See GameItem for more information.
        public List<LocalisedPrefab> LocalisedPrefabs
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
        [Tooltip("A list of prefabs for different localisations.")]
        [SerializeField]
        List<LocalisedPrefab> _localisedItems;

        [Serializable]
        public class LocalisedPrefab
        {
            /// <summary>
            /// The language that this prefab override is for.
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
            [Tooltip("The language that this prefab override is for.")]
            [SerializeField]
            SystemLanguage _language;


            /// <summary>
            /// The prefab for this language
            /// </summary>
            public GameObject Prefab
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
            [Tooltip("The prefab for this language")]
            [SerializeField]
            GameObject _item;
        }
    }
}

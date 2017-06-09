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

namespace GameFramework.Localisation.ObjectModel.AbstractClasses
{
    /// <summary>
    /// Class that holds information about localisable Object's.
    /// </summary>
    /// Note: Due to Unity localisation constraints and an inability to serialise generic types properly we use Object's as the types 
    /// here and restrict the internals to inherited classes
    [Serializable]
    public class LocalisableObject
    {
        #region Variables
        /// <summary>
        /// The default object that should be used.
        /// </summary>
        public UnityEngine.Object Default
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
        [Tooltip("The default object that should be used.")]
        [SerializeField]
        UnityEngine.Object _default;

        /// <summary>
        /// A list of objects for different localisations.
        /// </summary>
        List<LocalisedObject> LocalisedObjects
        {
            get
            {
                return _localisedObjects;
            }
            set
            {
                _localisedObjects = value;
            }
        }
        [Tooltip("A list of objects for different localisations.")]
        [SerializeField]
        List<LocalisedObject> _localisedObjects;

        [Serializable]
        internal class LocalisedObject
        {
            /// <summary>
            /// The language that this object override is for.
            /// </summary>
            internal SystemLanguage Language
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
            [Tooltip("The language that this object override is for.")]
            [SerializeField]
            SystemLanguage _language;


            /// <summary>
            /// The Object for this language
            /// </summary>
            internal UnityEngine.Object Object
            {
                get
                {
                    return _object;
                }
                set
                {
                    _object = value;
                }
            }
            [Tooltip("The object for this language")]
            [SerializeField]
            UnityEngine.Object _object;
        }
        #endregion Variables


        /// <summary>
        /// Get an object that corresponds to the currently set language
        /// </summary>
        /// <param name="fallbackToDefault">Whether to fall back to the default object if no language specific entry is found</param>
        /// <returns></returns>
        public UnityEngine.Object GetObject(bool fallbackToDefault = true)
        {
            return GetObject(GlobalLocalisation.Language, fallbackToDefault);
        }


        /// <summary>
        /// Get an object that corresponds to the specified language
        /// </summary>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault">Whether to fall back to the default object if no language specific entry is found</param>
        /// <returns></returns>
        public UnityEngine.Object GetObject(SystemLanguage language, bool fallbackToDefault = true)
        {
            foreach (var localisedObject in LocalisedObjects)
            {
                if (localisedObject.Language == language)
                    return localisedObject.Object;
            }
            return fallbackToDefault ? Default : null;
        }


        /// <summary>
        /// Get an object that corresponds to the specified language
        /// </summary>
        /// <param name="language"></param>
        /// <param name="fallbackToDefault">Whether to fall back to the default object if no language specific entry is found</param>
        /// <returns></returns>
        public UnityEngine.Object GetObject(string language, bool fallbackToDefault = true)
        {
            foreach (var localisedObject in LocalisedObjects)
            {
                if (localisedObject.Language.ToString() == language)
                    return localisedObject.Object;
            }
            return fallbackToDefault ? Default : null;
        }
    }
}

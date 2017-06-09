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

using GameFramework.Localisation.ObjectModel;
using UnityEngine;

namespace GameFramework.Localisation.Components
{
    /// <summary>
    /// Allows for setting of configuration used by the global localisation.
    /// </summary>
    [CreateAssetMenu(fileName = "LocalisationConfiguration", menuName = "Game Framework/Localisation Configuration", order = 21)]
    [System.Serializable]
    public class LocalisationConfiguration : ScriptableObject
    {
        /// <summary>
        /// Different modes for setting up the localisation
        /// </summary>
        /// Auto = Try loading from a resources folder with the names Default/Localisation & Localisation
        /// Specified = Specified LocalisationData files are passed in.
        public enum SetupModeType { Auto, Specified }

        #region editor fields

        /// <summary>
        /// How to setup the localisation either by default or specified resource files.
        /// </summary>
        public SetupModeType SetupMode
        {
            get
            {
                return _setupMode;
            }
            set
            {
                _setupMode = value;
            }
        }
        [SerializeField]
        [Tooltip("How to setup the localisation either by automatically using files from default locations or using specified resource files.")]
        SetupModeType _setupMode = SetupModeType.Auto;

        /// <summary>
        /// A list of specified localisation data files for loading
        /// </summary>
        /// Values in files towards the bottom of the list will override any values present in earlier files.
        public LocalisationData[] SpecifiedLocalisationData
        {
            get
            {
                return _specifiedLocalisationData;
            }
            set
            {
                _specifiedLocalisationData = value;
            }
        }
        [SerializeField]
        [Tooltip("A list of specified localisation data files. Values in files towards the bottom of the list will override any values present in earlier files.")]
        LocalisationData[] _specifiedLocalisationData = new LocalisationData[0];

        /// <summary>
        /// A list of localisation languages that we support
        /// </summary>
        public string[] SupportedLanguages
        {
            get
            {
                return _supportedLanguages;
            }
            set
            {
                _supportedLanguages = value;
            }
        }
        [SerializeField]
        [Tooltip("A list of localisation languages that we support.")]
        string[] _supportedLanguages = new string[0];

        #endregion editor fields
    }
}
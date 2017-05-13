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
using System.Linq;
using GameFramework.GameObjects.Components;
using GameFramework.Localisation.ObjectModel;
using GameFramework.GameStructure;
using UnityEngine;
using UnityEngine.Assertions;
using GameFramework.Preferences;

namespace GameFramework.Localisation.Components
{
    /// <summary>
    /// Provides dialog creation, display and management functionality.
    /// </summary>
    [AddComponentMenu("Game Framework/Localisation/LocalisationManager")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/localisation/")]
    public class LocalisationManager : Singleton<LocalisationManager>
    {
        enum SetupModeType { Auto, Specified }

        #region editor fields

        /// <summary>
        /// How to setup the localisation either by default or specified resource files.
        /// </summary>
        SetupModeType SetupMode
        {
            get
            {
                return _setupMode;
            }
        }
        [SerializeField]
        [Tooltip("How to setup the localisation either by default or specified resource files.")]
        SetupModeType _setupMode = SetupModeType.Auto;

        /// <summary>
        /// A list of localisation data files
        /// </summary>
        /// Values in files towards the bottom of the list will override any values present in earlier files.
        LocalisationData[] LocalisationData
        {
            get
            {
                return _localisationData;
            }
        }
        [SerializeField]
        [Tooltip("A list of localisation data files. Values in files towards the bottom of the list will override any values present in earlier files.")]
        LocalisationData[] _localisationData = new LocalisationData[0];

        /// <summary>
        /// A list of localisation languages that we support
        /// </summary>
        string[] SupportedLanguages
        {
            get
            {
                return _supportedLanguages;
            }
        }
        [SerializeField]
        [Tooltip("A list of localisation languages that we support")]
        string[] _supportedLanguages;

        #endregion editor fields
    }
}
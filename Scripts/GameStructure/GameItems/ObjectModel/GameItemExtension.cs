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

namespace FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel
{
    /// <summary>
    /// Allows for adding your own custom data and functionality to a GameItem
    /// </summary>
    [CreateAssetMenu(fileName = "type_x", menuName="GameFramework/GameItem Extension")]
    public class GameItemExtension : ScriptableObject
    {
        /// <summary>
        /// An override for the default name. For automatically created GameItems this will be a localisation key rather than a literal value.
        /// </summary>
        /// See GameItem for more information.
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        [Tooltip("An override for the default name. For automatically created GameItems this will be a localisation key rather than a literal value.")]
        [SerializeField]
        string _name;

        /// <summary>
        /// An override for the default description. For automatically created GameItems this will be a localisation key rather than a literal value.
        /// </summary>
        /// See GameItem for more information.
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }
        [Tooltip("An override for the default description. For automatically created GameItems this will be a localisation key rather than a literal value.")]
        [SerializeField]
        string _description;

        /// <summary>
        /// Whether to override the value to unlock field (we need this as we can't otherwise tell if the integer value is set or the default 0)
        /// </summary>
        /// See GameItem for more information.
        public bool OverrideValueToUnlock
        {
            get
            {
                return _overrideValueToUnlock;
            }
            set
            {
                _overrideValueToUnlock = value;
            }
        }
        [Tooltip("Whether to override the value to unlock field (we need this as we can't otherwise tell if the integer value is set or the default 0)")]
        [SerializeField]
        public bool _overrideValueToUnlock;

        /// <summary>
        /// An override for the default value needed to unlock this item.
        /// </summary>
        /// See GameItem for more information.
        public int ValueToUnlock {
            get
            {
                return _valueToUnlock;
            }
            set
            {
                _valueToUnlock = value;
            }
        }
        [Tooltip("An override for the default value needed to unlock this item.")]
        [SerializeField]
        int _valueToUnlock;
    }

}
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

using GameFramework.GameObjects.Components.AbstractClasses;
using GameFramework.Preferences;
using UnityEngine;

namespace GameFramework.GameObjects.Components
{
    /// <summary>
    /// Enable one of two gameobjects based upon a given flag (preferences key)
    /// </summary>
    /// This component also contains helper methods for setting and clearing the specified flag
    [AddComponentMenu("Game Framework/GameObjects/EnableBasedUponFlag")]
    [HelpURL("http://www.flipwebapps.com/game-framework/gameobjects/")]
    public class EnableBasedUponFlag : EnableDisableGameObject
    {
        /// <summary>
        /// A unique PlayerPrefs key that acts as the flag.
        /// </summary>
        [Tooltip("A unique PlayerPrefs key that acts as the flag.")]
        public string Key;


        /// <summary>
        /// Whether to show the condition met gameobject (true) or the condition not met one (false)
        /// </summary>
        /// <returns></returns>
        public override bool IsConditionMet()
        {
            return PreferencesFactory.IsFlagSet(Key);
        }


        /// <summary>
        /// Sets the flag
        /// </summary>
        /// <returns></returns>
        public void SetFlag()
        {
            PreferencesFactory.SetFlag(Key);
        }


        /// <summary>
        /// Clear the flag
        /// </summary>
        /// <returns></returns>
        public void ClearFlag()
        {
            PreferencesFactory.ClearFlag(Key);
        }
    }
}
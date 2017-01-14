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

using GameFramework.Preferences;
using UnityEngine;

namespace GameFramework.GameObjects.Components.AbstractClasses
{
    /// <summary>
    /// An abstract class to run something one time only.
    /// </summary>
    /// Override and implement the condition as you best see fit.
    public abstract class RunOnce : RunOnState
    {
        /// <summary>
        /// A unique PlayerPrefs key that identifies this instance.
        /// </summary>
        [Tooltip("A unique PlayerPrefs key that identifies this instance.")]
        public string Key;

        /// <summary>
        /// A key for a seperate instance that we should run after.
        /// </summary>
        [Tooltip("A key for a seperate instance that we should run after.")]
        public string EnableAfterKey;

        /// <summary>
        /// Calculates whether we need to run once or have already done so. In derived classes override RunOnce instead.
        /// </summary>
        public override void RunMethod()
        {
            if (PreferencesFactory.CheckAndSetFlag(Key, EnableAfterKey))
                RunOnceMethod();
        }


        /// <summary>
        /// Implement this as the method that should be run one time only
        /// </summary>
        public abstract void RunOnceMethod();
    }
}
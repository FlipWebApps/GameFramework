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

using GameFramework.GameStructure.Levels;
using GameFramework.UI.Buttons.Components.AbstractClasses;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace GameFramework.UI.Dialogs.Components
{
    /// <summary>
    /// Pauses a level when an attached button is clicked.
    /// </summary>
    /// This automatically hooks up the button onClick listener
    [RequireComponent(typeof(Button))]
    [AddComponentMenu("Game Framework/UI/Dialogs/On Button Click Pause Level")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/ui/")]
    public class OnButtonClickPauseLevel : OnButtonClick
    {
        /// <summary>
        /// The time scale that should be set when paused. Use this to stop physics and other effects.
        /// </summary>
        [Tooltip("The time scale that should be set when paused. Use this to stop physics and other effects.")]
        [Range(0, 1)]
        public float TimeScale;

        /// <summary>
        /// Pause the level when the button is clicked.
        /// </summary>
        public override void OnClick()
        {
            Assert.IsTrue(LevelManager.IsActive, "Ensure that you have added a LevelManager component to your scene!");
            LevelManager.Instance.PauseLevel(true, TimeScale);
        }
    }
}
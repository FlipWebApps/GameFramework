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

using GameFramework.GameObjects.Components;
using GameFramework.GameStructure;
using GameFramework.GameStructure.Levels;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameFramework.UI.Dialogs.Components
{
    /// <summary>
    /// Base class for a pause dialog
    /// </summary>
    /// You can override this class to add additional functionality.
    [RequireComponent(typeof(DialogInstance))]
    [AddComponentMenu("Game Framework/UI/Dialogs/Pause Window")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/ui/dialogs/")]
    public class PauseWindow : Singleton<PauseWindow>
    {
        /// <summary>
        /// The DialogInstance associated with the settings dialog.
        /// </summary>
        protected DialogInstance DialogInstance;


        /// <summary>
        /// Called when this instance is created for one time initialisation.
        /// </summary>
        /// Override this in your own base class if you want to customise the settings window. Be sure to call this base instance first.
        protected override void GameSetup()
        {
            DialogInstance = GetComponent<DialogInstance>();
            Assert.IsNotNull(DialogInstance.Target, "Ensure that you have set the script execution order of dialog instance in project settings (see help for details.");
        }


        /// <summary>
        /// Shows the pause window dialog.
        /// </summary>
        /// Override this in your own base class if you want to customise the settings window. Be sure to call this base instance when done.
        public virtual void Show()
        {
            Debug.LogWarning("The Pause Window is experimental and should not be used yet!");
            Debug.Log("TODO: Add options for what buttons to display");
            // show the dialog
            DialogInstance.Show(destroyOnClose: false);
        }


        /// <summary>
        /// Callback for resuming.
        /// </summary>
        public void Resume()
        {
            LevelManager.Instance.ResumeLevel();
            DialogInstance.Done();
        }


        /// <summary>
        /// Callback for resuming.
        /// </summary>
        public void Restart()
        {
            Debug.Log("TODO");
        }


        /// <summary>
        /// Callback for exiting the level back to ....
        /// </summary>
        public void ExitLevel()
        {
            Debug.Log("TODO");
        }

        /// <summary>
        /// Callback for quitting the game.
        /// </summary>
        public void Quit()
        {
            GameManager.Instance.SaveState();
            Application.Quit();
        }
    }
}
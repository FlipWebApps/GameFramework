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

using GameFramework.EditorExtras;
using GameFramework.GameObjects;
using GameFramework.GameObjects.Components;
using GameFramework.GameStructure;
using GameFramework.GameStructure.Levels;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace GameFramework.UI.Dialogs.Components
{
    /// <summary>
    /// Provides functionality for displaying and managing a pause window
    /// </summary>
    /// You can override this class to add additional functionality.
    [RequireComponent(typeof(DialogInstance))]
    [AddComponentMenu("Game Framework/UI/Dialogs/Pause Window")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/ui/dialogs/")]
    public class PauseWindow : Singleton<PauseWindow>
    {
        #region Editor Parameters

        /// <summary>
        /// Whether to reset the time scale on things like exit level or restart. If you don't set this and the pause window
        /// was set to freeze time then you will need to manually reset this yourself otherwise your game might still appear paused.
        /// </summary>
        bool AlwaysResetTimeScale
        {
            get
            {
                return _alwaysResetTimeScale;
            }
            set
            {
                _alwaysResetTimeScale = value;
            }
        }
        [Tooltip("Whether to reset the time scale on things like exit level or restart.\nIf you don't set this and the pause window was set to freeze time then you will need to manually reset this yourself otherwise your game might still appear paused.")]
        [SerializeField]
        bool _alwaysResetTimeScale = true;

        /// <summary>
        /// Whether to show a restart button
        /// </summary>
        bool ShowRestart
        {
            get
            {
                return _showRestart;
            }
            set
            {
                _showRestart = value;
            }
        }
        [Header("Display Options")]
        [Tooltip("Whether to show a restart button")]
        [SerializeField]
        bool _showRestart;

        /// <summary>
        /// Whether to show an options button
        /// </summary>
        bool ShowOptions
        {
            get
            {
                return _showOptions;
            }
            set
            {
                _showOptions = value;
            }
        }
        [Tooltip("Whether to show an options button")]
        [SerializeField]
        bool _showOptions;

        /// <summary>
        /// Whether to show an exit level button
        /// </summary>
        bool ShowExitLevel
        {
            get
            {
                return _showExitLevel;
            }
            set
            {
                _showExitLevel = value;
            }
        }
        [Tooltip("Whether to show an exit level button")]
        [SerializeField]
        bool _showExitLevel;

        /// <summary>
        /// (private) Localisable name for this GameItem. See also Name for easier access.
        /// </summary>
        string ExitLevelScene
        {
            get
            {
                return _exitLevelScene;
            }
            set
            {
                _exitLevelScene = value;
            }
        }
        [Tooltip("The scene that should be loaded if they chose exit level.")]
        [SerializeField]
        [ConditionalHide("_showExitLevel")]
        string _exitLevelScene;

        /// <summary>
        /// Whether to show a quit button
        /// </summary>
        bool ShowQuit
        {
            get
            {
                return _showQuit;
            }
            set
            {
                _showQuit = value;
            }
        }
        [Tooltip("Whether to show a quit button")]
        [SerializeField]
        bool _showQuit;
        #endregion Editor Parameters

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
            GameObjectHelper.SafeSetActive(GameObjectHelper.GetChildNamedGameObject(DialogInstance.gameObject, "Restart", true), ShowRestart);
            GameObjectHelper.SafeSetActive(GameObjectHelper.GetChildNamedGameObject(DialogInstance.gameObject, "Options", true), ShowOptions);
            GameObjectHelper.SafeSetActive(GameObjectHelper.GetChildNamedGameObject(DialogInstance.gameObject, "ExitLevel", true), ShowExitLevel);
            GameObjectHelper.SafeSetActive(GameObjectHelper.GetChildNamedGameObject(DialogInstance.gameObject, "Quit", true), ShowQuit);


            // show the dialog
            DialogInstance.Show(destroyOnClose: false);
        }


        /// <summary>
        /// Callback for resuming.
        /// </summary>
        public virtual void Resume()
        {
            LevelManager.Instance.ResumeLevel();
            DialogInstance.Done();
        }


        /// <summary>
        /// Callback for resuming.
        /// </summary>
        public virtual void Restart()
        {
            if (AlwaysResetTimeScale) Time.timeScale = 1;
            var sceneName = !string.IsNullOrEmpty(GameManager.Instance.IdentifierBase) && SceneManager.GetActiveScene().name.StartsWith(GameManager.Instance.IdentifierBase + "-") ? SceneManager.GetActiveScene().name.Substring((GameManager.Instance.IdentifierBase + "-").Length) : SceneManager.GetActiveScene().name;
            GameManager.LoadSceneWithTransitions(sceneName);
        }


        /// <summary>
        /// Callback for resuming.
        /// </summary>
        public virtual void Options()
        {
            Assert.IsTrue(Settings.IsActive, "You must add a settings prefab to your scene!");
            Settings.Instance.Show();
        }


        /// <summary>
        /// Callback for exiting the level back to ....
        /// </summary>
        public virtual void ExitLevel()
        {
            if (AlwaysResetTimeScale) Time.timeScale = 1;
            GameManager.LoadSceneWithTransitions(ExitLevelScene);
        }

        
        /// <summary>
        /// Callback for quitting the game.
        /// </summary>
        public virtual void Quit()
        {
            GameManager.Instance.SaveState();
            Application.Quit();
        }
    }
}
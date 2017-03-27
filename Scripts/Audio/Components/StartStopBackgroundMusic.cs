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

using GameFramework.GameStructure;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameFramework.Audio.Components
{
    /// <summary>
    /// Automatically start or stop the global background music.
    /// </summary>
    /// You can use this component to automatically start or stop background music that is configured on
    /// the GameManager. By using this component on different scenes you can control things such as cross scene 
    /// audio.
    [AddComponentMenu("Game Framework/Audio/StartStopBackgroundMusic")]
    [HelpURL("http://www.flipwebapps.com/game-framework/audio/")]
    public class StartStopBackgroundMusic : MonoBehaviour
    {
        /// <summary>
        /// The type of action to take
        /// </summary>
        public enum ModeType { None, Play, Stop };

        /// <summary>
        /// The type of action to take when this component is enabled
        /// </summary>
        [Tooltip("The type of action to take when this component is enabled")]
        public ModeType Enable = ModeType.Play;

        /// <summary>
        /// The type of action to take when this component is disabled
        /// </summary>
        [Tooltip("The type of action to take when this component is disabled")]
        public ModeType Disable = ModeType.None;

        void OnEnable()
        {
            Assert.IsTrue(GameManager.IsActive, "Please ensure that GameFramework.GameStructure.GameManager is added to Edit->ProjectSettings->ScriptExecution before 'Default Time'.\n" +
                                                "GameFramework.GameStructure.Audio.Components.StartStopBackgroundMusic does not necessarily need to appear in this list, but if it does ensure GameManager comes first");
            Assert.IsNotNull(GameManager.Instance.BackGroundAudioSource, "To make use of the StartStopBackgroundMusic component you should add an AudioSource component to the same gameobject as the GameManager that will be used for playing background music.");

            if (Enable == ModeType.Play && !GameManager.Instance.BackGroundAudioSource.isPlaying)
                GameManager.Instance.BackGroundAudioSource.Play();
            else if (Enable == ModeType.Stop && GameManager.Instance.BackGroundAudioSource.isPlaying)
                GameManager.Instance.BackGroundAudioSource.Stop();
        }

        public void OnDisable()
        {
            if (Disable == ModeType.Play && !GameManager.Instance.BackGroundAudioSource.isPlaying)
                GameManager.Instance.BackGroundAudioSource.Play();
            else if (Disable == ModeType.Stop && GameManager.Instance.BackGroundAudioSource.isPlaying)
                GameManager.Instance.BackGroundAudioSource.Stop();
        }
    }
}
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
using FlipWebApps.GameFramework.Scripts.GameStructure;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Support components for working with audio.
/// 
/// For additional information see http://www.flipwebapps.com/unity-assets/game-framework/audio/
/// </summary>
namespace FlipWebApps.GameFramework.Scripts.Audio
{
    // For doxygen documentation purposes only 
}

namespace FlipWebApps.GameFramework.Scripts.Audio.Components
{
    /// <summary>
    /// Copy the global effect volume to the attached Audio Source
    /// </summary>
    /// The global effect volume is stored on the GameManager component and can be adjusted through the provided
    /// in game settings window. This component will copy the users preference and automatically set the attached
    /// AudioSources volume to be as specified by the global setting.
    /// 
    /// Must be attached to a gameobject that contains an AudioSource component.
    /// 
    /// TODO: We should have notification when the global effect volume changes and update this.
    [RequireComponent(typeof(AudioSource))]
    [AddComponentMenu("Game Framework/Audio/CopyGlobalEffectVolume")]
    [HelpURL("http://www.flipwebapps.com/game-framework/audio/")]
    public class CopyGlobalEffectVolume : MonoBehaviour
    {
        void Awake()
        {
            Assert.IsTrue(GameManager.IsActive, "Please ensure that FlipWebApps.GameFramework.Scripts.GameStructure.GameManager is added to Edit->ProjectSettings->ScriptExecution before 'Default Time'.\n" +
                                                "FlipWebApps.GameFramework.Scripts.GameStructure.Audio.Components.StartStopBackgroundMusic does not necessarily need to appear in this list, but if it does ensure GameManager comes first");

            GetComponent<AudioSource>().volume = GameManager.Instance.EffectAudioVolume;
        }
    }
}
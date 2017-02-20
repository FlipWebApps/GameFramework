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

using GameFramework.Audio.Messages;
using GameFramework.GameStructure;
using GameFramework.Messaging.Components.AbstractClasses;
using UnityEngine;
using UnityEngine.Assertions;


namespace GameFramework.Audio.Components
{
    /// <summary>
    /// Copy the global effect volume to an attached Audio Source
    /// </summary>
    /// The global effect volume is stored on the GameManager component and can be adjusted through the provided
    /// in game settings window. This component will copy the users preference and automatically set the attached
    /// AudioSources volume to be as specified by the global setting.
    /// 
    /// Must be attached to a gameobject that contains an AudioSource component.
    /// 
    [RequireComponent(typeof(AudioSource))]
    [AddComponentMenu("Game Framework/Audio/CopyGlobalEffectVolume")]
    [HelpURL("http://www.flipwebapps.com/game-framework/audio/")]
    public class CopyGlobalEffectVolume : RunOnMessage<EffectVolumeChangedMessage>
    {
        /// <summary>
        /// A multiplier to apply to the global volume. 1 = use the global volume directly.
        /// </summary>
        [Range(0.0f, 2.0f)]
        [Tooltip("A multiplier to apply to the global volume. 1 = use the global volume directly.")]
        public float Multiplier = 1.0f;

        AudioSource _audioSource;

        public override void OnEnable()
        {
            Assert.IsTrue(GameManager.IsActive, "Please ensure that you have added a 'GameFramework | GameStructure | GameManager' component to your scene");

            _audioSource = GetComponent<AudioSource>();
            RunMethod(new EffectVolumeChangedMessage(0, GameManager.Instance.EffectAudioVolume));

            base.OnEnable();
        }


        /// <summary>
        /// Update the volume.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool RunMethod(EffectVolumeChangedMessage message)
        {
            _audioSource.volume = message.NewVolume * Multiplier;
            return true;
        }
    }
}
//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameStructure;
using UnityEngine;
using UnityEngine.Assertions;

namespace FlipWebApps.GameFramework.Scripts.Audio.Components
{
    /// <summary>
    /// Start or stop the global background music when enabled / disabled
    /// </summary>
    public class StartStopBackgroundMusic : MonoBehaviour
    {
        public enum ModeType { None, Play, Stop };
        public ModeType Enable = ModeType.Play;
        public ModeType Disable = ModeType.None;

        void OnEnable()
        {
            Assert.IsTrue(GameManager.IsActive, "Please ensure that FlipWebApps.GameFramework.Scripts.GameStructure.GameManager is added to Edit->ProjectSettings->ScriptExecution before 'Default Time'.\n" +
                                                "FlipWebApps.GameFramework.Scripts.GameStructure.Audio.Components.StartStopBackgroundMusic does not necessarily need to appear in this list, but if it does ensure GameManager comes first");
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
//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------
using FlipWebApps.GameFramework.Scripts.GameStructure;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Audio.Components
{
    /// <summary>
    /// Copy the global effect volume to the attached Audio Source
    /// TODO: We should have notification when teh global effect volume changes and update this.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class CopyGlobalEffectVolume : MonoBehaviour
    {
        void Awake()
        {
            if (GetComponent<AudioSource>() != null)
            {
                GetComponent<AudioSource>().volume = GameManager.Instance.EffectAudioVolume;
            }
        }
    }
}
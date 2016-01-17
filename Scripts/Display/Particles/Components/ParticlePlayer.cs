//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Display.Particles.Components
{
    /// <summary>
    /// Provides a callback for creating a new particle system.
    /// </summary>
    public class ParticlePlayer : MonoBehaviour
    {
        public ParticleSystem ParticleSystem;

        public void CreateParticleSystem()
        {
            ParticleSystem newParticleSystem = Instantiate(
                ParticleSystem,
                transform.position,
                Quaternion.identity
                ) as ParticleSystem;

            // Make sure it will be destroyed
            if (newParticleSystem != null)
            {
                Destroy(
                    newParticleSystem.gameObject,
                    newParticleSystem.startLifetime
                    );
            }
        }
    }
}
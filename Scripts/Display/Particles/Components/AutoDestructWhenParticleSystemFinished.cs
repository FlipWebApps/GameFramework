//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using System.Collections;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Display.Particles.Components
{
    /// <summary>
    /// Destroy or disable the specified gameobject once the particle system is finished playing.
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    public class AutoDestructWhenParticleSystemFinished : MonoBehaviour
    {
        public bool OnlyDeactivate;
        public GameObject GameObjectToDestruct;

        ParticleSystem _particleSystem;

        void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        void OnEnable()
        {
            if (GameObjectToDestruct == null) GameObjectToDestruct = gameObject;
            StartCoroutine("CheckIfAlive");
        }

        IEnumerator CheckIfAlive()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.5f);
                if (!_particleSystem.IsAlive(true))
                {
                    if (OnlyDeactivate)
                        GameObjectToDestruct.SetActive(false);
                    else
                        Destroy(GameObjectToDestruct);
                    break;
                }
            }
        }
    }
}
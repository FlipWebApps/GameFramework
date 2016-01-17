//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Collectables
{
    /// <summary>
    /// Coin collectable item
    /// 
    /// NOTE: This class is beta and subject to changebreaking change without warning.
    /// </summary>
    public class Coin : MonoBehaviour
    {
        public ParticleSystem FireworksEffect;
        public AudioClip PowerupAudioClip;
        public int Value;

        void OnTriggerEnter2D(Collider2D otherCollider)
        {
            // Is this an obsticle?
            if (otherCollider.gameObject.tag == "Player")
            {
                if (FireworksEffect != null)
                    Instantiate(FireworksEffect, transform.position, Quaternion.identity);

                if (PowerupAudioClip != null)
                    GameManager.Instance.PlayEffect(PowerupAudioClip);

                GameManager.Instance.Player.AddCoins(Value);
                GameManager.Instance.Levels.Selected.AddCoins(Value);

                Destroy(gameObject);
            }

        }
    }
}
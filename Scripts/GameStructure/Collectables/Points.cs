//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Collectables
{
    /// <summary>
    /// Points collectable item
    /// 
    /// NOTE: This class is beta and subject to changebreaking change without warning.
    /// </summary>
    public class Points : MonoBehaviour
    {
        public ParticleSystem Effect;
        public AudioClip AudioClip;
        public int Value;

        void OnTriggerEnter2D(Collider2D otherCollider)
        {
            // Is this an obsticle?
            if (otherCollider.gameObject.tag == "Player")
            {
                Instantiate(Effect, transform.position, Quaternion.identity);
                GameManager.Instance.PlayEffect(AudioClip);

                GameManager.Instance.Player.AddPoints(Value);
                GameManager.Instance.Levels.Selected.AddPoints(Value);

                Destroy(gameObject);
            }

        }
    }
}
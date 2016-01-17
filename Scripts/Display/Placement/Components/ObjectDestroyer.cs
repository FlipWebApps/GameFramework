//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Display.Placement.Components
{

    public class ObjectDestroyer : MonoBehaviour
    {
        public enum DestroyModeType { DestroyCollidingGameObject, DestroyCollidingParentGameObject }
        public DestroyModeType DestroyMode;

        public string DestroyTag = "Obstacle";

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision == null)
                return;

            // Is this an obsticle?
            if (collision.gameObject.tag == DestroyTag)
            {
                DestroyCollidingGameObject(collision.gameObject);
            }
        }

        void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (otherCollider == null)
                return;

            // Is this an obsticle?
            if (otherCollider.gameObject.tag == DestroyTag)
            {
                Destroy(otherCollider.gameObject); // Remember to always target the game object, otherwise you will just remove the script
            }
        }

        void DestroyCollidingGameObject(GameObject collidingGameObject)
        {
            switch (DestroyMode)
            {
                case DestroyModeType.DestroyCollidingGameObject:
                    Destroy(collidingGameObject);
                    break;
                case DestroyModeType.DestroyCollidingParentGameObject:
                    Destroy(collidingGameObject.transform.parent.gameObject);
                    break;
            }
        }
    }
}
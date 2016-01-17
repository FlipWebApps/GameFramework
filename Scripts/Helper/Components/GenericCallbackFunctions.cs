//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameStructure.Levels;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Helper.Components
{
    /// <summary>
    /// A collection of generic callback functions that can be used from animators or otherwise.
    /// 
    /// You can specify a reference to have these operate on a gameobject other than the one where this is currently attached.
    /// </summary>
    public class GenericCallbackFunctions : MonoBehaviour
    {
        public GameObject GameObject;

        public void Awake()
        {
            if (GameObject == null)
                GameObject = gameObject;
        }

        public void DestroyGameObject()
        {
            Destroy(GameObject);
        }

        public void DestroyParentGameObject()
        {
            Destroy(GameObject.transform.parent.gameObject);
        }

        public void DisableGameObject()
        {
            GameObject.SetActive(false);
        }

        public void EnableGameObject()
        {
            GameObject.SetActive(true);
        }

        public void LevelStarted()
        {
            LevelManager.Instance.LevelStarted();
        }

    }
}
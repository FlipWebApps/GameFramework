//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Display.Other.Components
{
    /// <summary>
    /// Set the renderers sorting layer
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class SetSortingLayer : MonoBehaviour
    {
        public string SortingLayerName = "Foreground";
        public int SortingOrder;

        void Start()
        {
            // Set the sorting layer of the particle system.
            GetComponent<Renderer>().sortingLayerName = SortingLayerName;
            GetComponent<Renderer>().sortingOrder = SortingOrder;
        }
    }
}
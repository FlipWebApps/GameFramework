//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Display.Other
{
    /// <summary>
    /// Some renderer extensions
    /// </summary>
    public static class RendererExtensions
    {
        public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
            return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
        }
    }
}
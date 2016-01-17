//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.Display.Placement;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Input
{
    /// <summary>
    /// Input helper functions
    /// </summary>
    public class InputHelper
    {
        public static Vector3 GetMousePositionOnXzPlane()
        {
            return PositionHelper.GetPositionOnXzPlane(UnityEngine.Input.mousePosition);
        }

        public static Vector3 GetMousePositionOnXzPlane(float y)
        {
            return PositionHelper.GetPositionOnXzPlane(UnityEngine.Input.mousePosition, y);
        }

        public static Vector3 GetMousePositionOnPlane(Plane plane)
        {
            return PositionHelper.GetPositionOnPlane(UnityEngine.Input.mousePosition, plane);
        }
    }
}
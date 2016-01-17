//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Display.Placement.Components
{
    /// <summary>
    /// Aligh this game object with the screen bounds. Useful when catering for dynamic screen sizes.
    /// </summary>
    public class AlignScreenBounds : MonoBehaviour
    {
        public enum BorderType { Top, Bottom, Left, Right }
        public BorderType Border;
        public float Offset;

        void Start()
        {
            Vector3 worldBottomLeftPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
            Vector3 worldTopRightPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

            switch (Border)
            {
                case BorderType.Top:
                    transform.position = new Vector3(transform.position.x, worldTopRightPosition.y + Offset, transform.position.z);
                    break;
                case BorderType.Bottom:
                    transform.position = new Vector3(transform.position.x, worldBottomLeftPosition.y + Offset, transform.position.z);
                    break;
                case BorderType.Left:
                    transform.position = new Vector3(worldBottomLeftPosition.x + Offset, transform.position.y, transform.position.z);
                    break;
                case BorderType.Right:
                    transform.position = new Vector3(worldTopRightPosition.x + Offset, transform.position.y, transform.position.z);
                    break;
            }
        }
    }
}
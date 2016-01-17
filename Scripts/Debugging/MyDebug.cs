//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

//#if UNITY_EDITOR
//#define DEBUG
//#endif

namespace FlipWebApps.GameFramework.Scripts.Debugging
{
    /// <summary>
    /// My debugging functions that supplement those provided by unity.
    /// Logging done through these is only output if run via the editor or as part of a debug build
    /// </summary>
    public class MyDebug
    {

        public static bool IsDebugBuildOrEditor
        {
#if UNITY_EDITOR
            get { return true; }
#else
        get { return UnityEngine.Debug.isDebugBuild; }
#endif
        }

        // show text to a text component if it exists
        public static void DebugText(string text)
        {
            if (!IsDebugBuildOrEditor)
                return;

            GameObject go = GameObject.Find("DebugText");
            if (go != null)
            {
                UnityEngine.UI.Text label = go.GetComponent<UnityEngine.UI.Text>();
                if (label != null)
                {
                    label.text = text;
                }
            }
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void Log(object message)
        {
            Debug.Log(message);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogF(string format, params object[] args)
        {
            Debug.Log(string.Format(format, args));
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void Log(object message, Object context)
        {
            Debug.Log(message, context);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogError(object message)
        {
            Debug.LogError(message);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogError(object message, Object context)
        {
            Debug.LogError(message, context);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogWarning(object message)
        {
            Debug.LogWarning(message.ToString());
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogWarningF(string format, params object[] args)
        {
            Debug.LogWarning(string.Format(format, args));
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogWarning(object message, Object context)
        {
            Debug.LogWarning(message.ToString(), context);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void DrawLine(Vector3 start, Vector3 end, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
        {
            Debug.DrawLine(start, end, color, duration, depthTest);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void DrawRay(Vector3 start, Vector3 dir, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
        {
            Debug.DrawRay(start, dir, color, duration, depthTest);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void Throw(string message)
        {
            Debug.LogWarning(message);
        }

        public static void DrawCube(Vector3 pos, Color col, Vector3 size, float duration = 0.0f)
        {
            Vector3 halfScale = size * 0.5f;

            Vector3[] points =
            {
                pos + new Vector3(halfScale.x,      halfScale.y,    halfScale.z),
                pos + new Vector3(-halfScale.x,     halfScale.y,    halfScale.z),
                pos + new Vector3(-halfScale.x,     -halfScale.y,   halfScale.z),
                pos + new Vector3(halfScale.x,      -halfScale.y,   halfScale.z),
                pos + new Vector3(halfScale.x,      halfScale.y,    -halfScale.z),
                pos + new Vector3(-halfScale.x,     halfScale.y,    -halfScale.z),
                pos + new Vector3(-halfScale.x,     -halfScale.y,   -halfScale.z),
                pos + new Vector3(halfScale.x,      -halfScale.y,   -halfScale.z),
            };

            Debug.DrawLine(points[0], points[1], col, duration);
            Debug.DrawLine(points[1], points[2], col, duration);
            Debug.DrawLine(points[2], points[3], col, duration);
            Debug.DrawLine(points[3], points[0], col, duration);
        }

        public static void DrawRect(Rect rect, Color color, float duration = 0.0f)
        {
            Vector3 pos = new Vector3(rect.x + rect.width / 2, rect.y + rect.height / 2, 0.0f);
            Vector3 scale = new Vector3(rect.width, rect.height, 0.0f);

            DrawRect(pos, color, scale, duration);
        }

        public static void DrawRect(Vector3 pos, Color color, Vector3 size, float duration = 0.0f)
        {
            Vector3 halfSize = size * 0.5f;

            Vector3[] points = 
            {
                pos + new Vector3(halfSize.x,      halfSize.y,    halfSize.z),
                pos + new Vector3(-halfSize.x,     halfSize.y,    halfSize.z),
                pos + new Vector3(-halfSize.x,     -halfSize.y,   halfSize.z),
                pos + new Vector3(halfSize.x,      -halfSize.y,   halfSize.z),
            };

            Debug.DrawLine(points[0], points[1], color, duration);
            Debug.DrawLine(points[1], points[2], color, duration);
            Debug.DrawLine(points[2], points[3], color, duration);
            Debug.DrawLine(points[3], points[0], color, duration);
        }

        public static void DrawPoint(Vector3 pos, Color color, float size, float duration = 0.0f)
        {
            Vector3[] points = 
            {
                pos + Vector3.up * size,
                pos - Vector3.up * size,
                pos + Vector3.right * size,
                pos - Vector3.right * size,
                pos + Vector3.forward * size,
                pos - Vector3.forward * size
            };

            Debug.DrawLine(points[0], points[1], color, duration);
            Debug.DrawLine(points[2], points[3], color, duration);
            Debug.DrawLine(points[4], points[5], color, duration);

            Debug.DrawLine(points[0], points[2], color, duration);
            Debug.DrawLine(points[0], points[3], color, duration);
            Debug.DrawLine(points[0], points[4], color, duration);
            Debug.DrawLine(points[0], points[5], color, duration);

            Debug.DrawLine(points[1], points[2], color, duration);
            Debug.DrawLine(points[1], points[3], color, duration);
            Debug.DrawLine(points[1], points[4], color, duration);
            Debug.DrawLine(points[1], points[5], color, duration);

            Debug.DrawLine(points[4], points[2], color, duration);
            Debug.DrawLine(points[4], points[3], color, duration);
            Debug.DrawLine(points[5], points[2], color, duration);
            Debug.DrawLine(points[5], points[3], color, duration);

        }
    }
}
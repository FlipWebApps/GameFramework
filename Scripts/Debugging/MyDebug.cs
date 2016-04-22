//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//
// Please direct any bugs/comments/suggestions to http://www.flipwebapps.com
// 
// The copyright owner grants to the end user a non-exclusive, worldwide, and perpetual license to this Asset
// to integrate only as incorporated and embedded components of electronic games and interactive media and 
// distribute such electronic game and interactive media. End user may modify Assets. End user may otherwise 
// not reproduce, distribute, sublicense, rent, lease or lend the Assets. It is emphasized that the end 
// user shall not be entitled to distribute or transfer in any way (including, without, limitation by way of 
// sublicense) the Assets in any other way than as integrated components of electronic games and interactive media. 

// The above copyright notice and this permission notice must not be removed from any files.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.Display.Other;
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
        public enum DebugLevelType { None, Information, Warning, Error }

        /// <summary>
        /// What level of debugging we actually want to show.
        /// </summary>
        public static DebugLevelType DebugLevel { get; set; }

        /// <summary>
        /// Static Constructuro
        /// </summary>
        static MyDebug() {
            DebugLevel = DebugLevelType.None;
        }

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
            if (DebugLevel >= DebugLevelType.Information)
                Debug.Log(message);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogF(string format, params object[] args)
        {
            if (DebugLevel >= DebugLevelType.Information)
                Debug.Log(string.Format(format, args));
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void Log(object message, Object context)
        {
            if (DebugLevel >= DebugLevelType.Information)
                Debug.Log(message, context);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogError(object message)
        {
            if (DebugLevel >= DebugLevelType.Error)
                Debug.LogError(message);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogErrorF(string format, params object[] args)
        {
            if (DebugLevel >= DebugLevelType.Error)
                Debug.LogError(string.Format(format, args));
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogError(object message, Object context)
        {
            if (DebugLevel >= DebugLevelType.Error)
                Debug.LogError(message, context);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogWarning(object message)
        {
            if (DebugLevel >= DebugLevelType.Warning)
                Debug.LogWarning(message.ToString());
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogWarningF(string format, params object[] args)
        {
            if (DebugLevel >= DebugLevelType.Warning)
                Debug.LogWarning(string.Format(format, args));
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogWarning(object message, Object context)
        {
            if (DebugLevel >= DebugLevelType.Warning)
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


        public static void DrawGizmoRect(Rect rect, Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawLine(new Vector3(rect.xMin, rect.yMin, 0), new Vector3(rect.xMax, rect.yMin, 0));
            Gizmos.DrawLine(new Vector3(rect.xMax, rect.yMin, 0), new Vector3(rect.xMax, rect.yMax, 0));
            Gizmos.DrawLine(new Vector3(rect.xMax, rect.yMax, 0), new Vector3(rect.xMin, rect.yMax, 0));
            Gizmos.DrawLine(new Vector3(rect.xMin, rect.yMax, 0), new Vector3(rect.xMin, rect.yMin, 0));
        }
    }
}
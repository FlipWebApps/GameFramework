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

using UnityEditor;
using UnityEngine;
using FlipWebApps.GameFramework.Scripts.Messaging;

namespace FlipWebApps.GameFramework.Scripts.Debugging.Components.Editor
{
    /// <summary>
    /// Editor window that shows the messaging activity.
    /// </summary>
    class MessagesWindow : EditorWindow
    {
        Vector2 scrollPosition = Vector2.zero;
        Color LineColour1;
        Color LineColour2;
        //Texture2D SmallErrorIcon;
        //Texture2D SmallWarningIcon;
        //Texture2D SmallMessageIcon;

        // Add menu item
        [MenuItem("Window/Flip Web Apps/Message Activity Windows")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            var window = GetWindow(typeof(MessagesWindow));
            window.titleContent.text = "Messages";
        }

        void OnEnable()
        {
            LineColour1 = GUI.backgroundColor;
            LineColour2 = new Color(GUI.backgroundColor.r * 0.9f, GUI.backgroundColor.g * 0.9f, GUI.backgroundColor.b * 0.9f);
            //SmallErrorIcon = EditorGUIUtility.FindTexture("d_console.erroricon.sml");
            //SmallWarningIcon = EditorGUIUtility.FindTexture("d_console.warnicon.sml");
            //SmallMessageIcon = EditorGUIUtility.FindTexture("d_console.infoicon.sml");
        }

        void OnGUI()
        {
            var drawnLines = 0;
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            for (var i = 0; i < Messenger._messageLog.Count; i++)
            {
                drawnLines++;
                GUIStyle s = new GUIStyle();
                s.normal.background = MakeColoredTexture(1, 1, new Color(1.0f, 1.0f, 1.0f, 0.1f));
                GUILayout.BeginHorizontal(s);
                GUI.backgroundColor = (drawnLines % 2 == 0) ? LineColour1 : LineColour2;
                GUILayout.Label(Messenger._messageLog[i].LogEntryType.ToString(), GUILayout.Width(100));
                GUILayout.Label(Messenger._messageLog[i].Time.ToString(), GUILayout.Width(100));
                GUILayout.BeginVertical();
                GUILayout.Label(Messenger._messageLog[i].MessageType);
                if (!string.IsNullOrEmpty(Messenger._messageLog[i].Contents))
                    GUILayout.Label(Messenger._messageLog[i].Contents);
                if (!string.IsNullOrEmpty(Messenger._messageLog[i].Message))
                    GUILayout.Label(Messenger._messageLog[i].Message);
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                //Rect guiRect = GUILayoutUtility.GetLastRect();
                //if (Event.current != null && Event.current.isMouse)
                //{
                //    bool overGUI = guiRect.Contains(Event.current.mousePosition);
                //    if (overGUI)
                //        Debug.Log("CLicked row" + Messenger._messageLog[i].Time.ToString());
                //}
            }
            GUILayout.EndScrollView();
        }


        private Texture2D MakeColoredTexture(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }
    }
}
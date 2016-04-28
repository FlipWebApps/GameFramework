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
using FlipWebApps.GameFramework.Scripts.EditorExtras.Editor;
using System.Linq;

namespace FlipWebApps.GameFramework.Scripts.Messaging.Editor
{
    /// <summary>
    /// Editor window that shows the messaging activity.
    /// </summary>
    public class MessagesWindow : EditorWindow
    {
        string[] _tabNames = { "Activity Log", "Statistics" };
        int _tabSelected;

        Vector2 scrollPosition = Vector2.zero;
        Color LineColour1;
        Color LineColour2;
        //Texture2D SmallErrorIcon;
        //Texture2D SmallWarningIcon;
        //Texture2D SmallMessageIcon;

        //Serialise the logger field so that Unity doesn't forget about the logger when you hit Play
        [UnityEngine.SerializeField]
        MessageLog _messageLog;


        // Add menu item for showing the window
        [MenuItem("Window/Flip Web Apps/Message Activity Windows")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            //var window = 
                GetWindow(typeof(MessagesWindow));
        }


        void OnEnable()
        {
            titleContent.text = "Messages";

            LineColour1 = GUI.backgroundColor;
            LineColour2 = new Color(GUI.backgroundColor.r * 0.9f, GUI.backgroundColor.g * 0.9f, GUI.backgroundColor.b * 0.9f);
            //SmallErrorIcon = EditorGUIUtility.FindTexture("d_console.erroricon.sml");
            //SmallWarningIcon = EditorGUIUtility.FindTexture("d_console.warnicon.sml");
            //SmallMessageIcon = EditorGUIUtility.FindTexture("d_console.infoicon.sml");

            // Get or create the backend
            if (_messageLog == null)
            {
                _messageLog = MessageLogHandler.MessageLog;
                if (!_messageLog)
                {
                    _messageLog = MessageLog.Create();
                }
            }
            MessageLogHandler.MessageLog = _messageLog;

            _messageLog.LogEntryAdded += OnLogEntryAdded;
            EditorApplication.playmodeStateChanged += OnPlaymodeStateChanged;
        }


        void OnDisable()
        {
            EditorApplication.playmodeStateChanged -= OnPlaymodeStateChanged;
            _messageLog.LogEntryAdded -= OnLogEntryAdded;
        }


        /// <summary>
        /// WHen a log entry is added then repaint the window.
        /// </summary>
        void OnLogEntryAdded()
        {
            Repaint();
        }



        /// <summary>
        /// When the playmode changes, clear the log if necessary
        /// </summary>
        void OnPlaymodeStateChanged()
        {
            if (_messageLog.ClearOnPlay && EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
                _messageLog.Clear();
        }


        /// <summary>
        /// Draw the GUI
        /// </summary>
        void OnGUI()
        {
            DrawToolbar();
            DrawTabs();
            switch (_tabSelected)
            {
                case 0:
                    DrawLogEntries();
                    break;
                case 1:
                    DrawStatistics();
                    break;
            }
        }


        /// <summary>
        /// Draw the tabs that we have available
        /// </summary>
        void DrawTabs()
        {
            _tabSelected = GUILayout.Toolbar(_tabSelected, _tabNames);
        }


        /// <summary>
        /// Draws the toolbar.
        /// </summary>
        void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal();
            if(EditorHelper.ButtonTrimmed("Clear", EditorStyles.toolbarButton))
            {
                _messageLog.Clear();
            }
            _messageLog.ClearOnPlay = EditorHelper.ToggleTrimmed(_messageLog.ClearOnPlay, "Clear On Play", EditorStyles.toolbarButton);
            EditorGUILayout.EndHorizontal();
        }


        /// <summary>
        /// Draw the log entries
        /// </summary>
        private void DrawLogEntries()
        {
            var drawnLines = 0;
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);


            for (var i = 0; i < _messageLog.LogEntries.Count; i++)
            {
                drawnLines++;
                GUIStyle s = new GUIStyle();
                s.normal.background = MakeColoredTexture(1, 1, new Color(1.0f, 1.0f, 1.0f, 0.1f));
                GUILayout.BeginHorizontal(s);
                GUI.backgroundColor = (drawnLines % 2 == 0) ? LineColour1 : LineColour2;
                GUILayout.Label(_messageLog.LogEntries[i].LogEntryType.ToString(), GUILayout.Width(100));
                GUILayout.Label(_messageLog.LogEntries[i].Time.ToString(), GUILayout.Width(100));
                GUILayout.BeginVertical();
                GUILayout.Label(_messageLog.LogEntries[i].MessageType);
                if (!string.IsNullOrEmpty(_messageLog.LogEntries[i].Contents))
                    GUILayout.Label(_messageLog.LogEntries[i].Contents);
                if (!string.IsNullOrEmpty(_messageLog.LogEntries[i].Message))
                    GUILayout.Label(_messageLog.LogEntries[i].Message);
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


        /// <summary>
        /// Draw the statistics
        /// </summary>
        private void DrawStatistics()
        {
            if (_messageLog.LogEntries.Count == 0)
            {
                GUILayout.Label("No Activity Yet", EditorHelper.ItalicLabelStyle(TextAnchor.MiddleCenter), GUILayout.ExpandWidth(true));
            }
            else
            {
                var boldLabelStyle = EditorHelper.BoldLabelStyle();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Message Type", boldLabelStyle, GUILayout.Width(250));
                GUILayout.Label("Total Messages", boldLabelStyle, GUILayout.Width(150));
                GUILayout.Label("Active Handlers", boldLabelStyle, GUILayout.Width(150));
                GUILayout.EndHorizontal();


                var drawnLines = 0;
                scrollPosition = GUILayout.BeginScrollView(scrollPosition);

                // Acquire keys and sort them.
                var list = _messageLog.Statistics.Keys.ToList();
                list.Sort();

                // Loop through keys.
                foreach (var key in list)
                {
                    var statisticsEntry = _messageLog.Statistics[key];
                    drawnLines++;
                    GUIStyle s = new GUIStyle();
                    s.normal.background = MakeColoredTexture(1, 1, new Color(1.0f, 1.0f, 1.0f, 0.1f));
                    GUILayout.BeginHorizontal(s);
                    GUI.backgroundColor = (drawnLines % 2 == 0) ? LineColour1 : LineColour2;
                    GUILayout.Label(key, GUILayout.Width(250));
                    GUILayout.Label(statisticsEntry.MessageCount.ToString(), GUILayout.Width(150));
                    GUILayout.Label(statisticsEntry.HandlerCount.ToString(), GUILayout.Width(150));
                    GUILayout.EndHorizontal();
                }

                GUILayout.EndScrollView();
            }
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

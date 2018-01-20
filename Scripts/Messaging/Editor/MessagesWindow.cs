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
using GameFramework.EditorExtras.Editor;
using System.Linq;

namespace GameFramework.Messaging.Editor
{
    /// <summary>
    /// Editor window that shows the messaging activity.
    /// </summary>
    public class MessagesWindow : EditorWindow
    {
        readonly string[] _tabNames = { "Activity Log", "Statistics" };
        int _tabSelected;

        float _topPanelHeight;
        Vector2 _messageLogScrollPosition;
        Vector2 _messageDetailsScrollPosition;
        int _newSelectedRow = -1;
        int _selectedRow = -1;
        Color _lineColour1;
        Color _lineColour2;
        Color _resizeAreaColour;
        bool _isResizing;

        Vector2 _statisticsScrollPosition;

        //Serialise the logger field so that Unity doesn't forget about the logger when you hit Play
        [SerializeField]
        MessageLog _messageLog;


        // Add menu item for showing the window
        [MenuItem("Window/Game Framework/Message Activity Window", priority = 2)]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            var window = GetWindow(typeof(MessagesWindow)) as MessagesWindow;
            window._topPanelHeight = window.position.height / 2;
        }


        void OnEnable()
        {
            titleContent.text = "Messages";

            _lineColour1 = GUI.backgroundColor;
            _lineColour2 = new Color(GUI.backgroundColor.r * 0.8f, GUI.backgroundColor.g * 0.8f, GUI.backgroundColor.b * 0.8f);
            _resizeAreaColour = new Color(_lineColour1.r * 0.5f, _lineColour1.g * 0.5f, _lineColour1.b * 0.5f);

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
#if UNITY_2017_2_OR_NEWER
            EditorApplication.playModeStateChanged += OnPlaymodeStateChanged;
#else
            EditorApplication.playmodeStateChanged += OnPlaymodeStateChanged;
#endif
        }


        void OnDisable()
        {
#if UNITY_2017_2_OR_NEWER
            EditorApplication.playModeStateChanged -= OnPlaymodeStateChanged;
#else
            EditorApplication.playmodeStateChanged -= OnPlaymodeStateChanged;
#endif
            _messageLog.LogEntryAdded -= OnLogEntryAdded;
        }


        /// <summary>
        /// When a log entry is added then repaint the window.
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


#if UNITY_2017_2_OR_NEWER
        /// <summary>
        /// When the playmode changes, clear the log if necessary (2017.2+ version)
        /// </summary>
        void OnPlaymodeStateChanged(PlayModeStateChange state)
        {
            if (_messageLog.ClearOnPlay && state == PlayModeStateChange.ExitingEditMode)
                _messageLog.Clear();
        }
#endif

        /// <summary>
        /// Draw the GUI
        /// </summary>
        void OnGUI()
        {

            // actions to take at the start, so the elements aren't affected between layout and repaint steps.
            if (Event.current.type == EventType.Layout)
            {
                _selectedRow = _newSelectedRow;
            }

            switch (_tabSelected)
            {
                case 0:
                    GUILayout.BeginVertical(GUILayout.Height(_topPanelHeight), GUILayout.MinHeight(100));
                    DrawToolbar();
                    DrawTabs();
                    DrawMessageEntries();
                    GUILayout.EndVertical();
                    HandleResize();
                    GUILayout.Space(10);
                    GUILayout.BeginVertical(GUILayout.Height(position.height - _topPanelHeight));
                    DrawMessageDetails();
                    GUILayout.EndVertical();
                    break;
                case 1:
                    DrawToolbar();
                    DrawTabs();
                    DrawStatistics();
                    break;
            }

            // actions to take at the end, so the elements aren't affected between layout and repaint events.
            //if (Event.current.type == EventType.Repaint)
            //{
            //}
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
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            if (EditorHelper.ButtonTrimmed("Clear", EditorStyles.toolbarButton))
            {
                _messageLog.Clear();
            }
            _messageLog.ClearOnPlay = EditorHelper.ToggleTrimmed(_messageLog.ClearOnPlay, "Clear On Play", EditorStyles.toolbarButton);

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }


        /// <summary>
        /// Draw the message entries
        /// </summary>
        void DrawMessageEntries()
        {
            var drawnLines = 0;
            _messageLogScrollPosition = GUILayout.BeginScrollView(_messageLogScrollPosition);

            // don't optimise at this time due to issues with multiple calls to OnGUI andchanging the number of drawn items.
            //int start = (int)(_messageLogScrollPosition.y / EditorGUIUtility.singleLineHeight);
            //int end = (int)((_messageLogScrollPosition.y + _topPanelHeight) / EditorGUIUtility.singleLineHeight);
            //Debug.Log(start + ", " + end);
            //// don't draw anything outside the visible display for performance - just add space as a dummy container
            //GUILayout.Space(start * EditorGUIUtility.singleLineHeight);

            // draw all visible items
            //for (var i = start; i < end && i < _messageLog.LogEntries.Count; i++)
            for (var i = 0; i < _messageLog.LogEntries.Count; i++)
            {
                //if (i >= start && i <= end)
                //{
                drawnLines++;
                var oldBackgrounColor = GUI.backgroundColor;

                var s = new GUIStyle();
                s.normal.background = MakeColoredTexture(1, 1, new Color(1.0f, 1.0f, 1.0f, 0.1f));
                GUI.backgroundColor = _selectedRow == i ? Color.blue : (drawnLines % 2 == 0) ? _lineColour1 : _lineColour2;
                GUILayout.BeginHorizontal(s);
                GUILayout.Label(_messageLog.LogEntries[i].LogEntryType.ToString(), GUILayout.Width(100));
                GUILayout.Label(_messageLog.LogEntries[i].Time.ToShortTimeString(), GUILayout.Width(100));
                var text = string.IsNullOrEmpty(_messageLog.LogEntries[i].Message) ?
                    _messageLog.LogEntries[i].MessageType :
                    _messageLog.LogEntries[i].MessageType + " - " + _messageLog.LogEntries[i].Message;
                GUILayout.Label(text);
                GUILayout.EndHorizontal();

                if (Event.current.button == 0 && Event.current.type == EventType.MouseUp)
                {
                    if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                    {
                        _newSelectedRow = i;
                        Repaint();
                    }
                }

                GUI.backgroundColor = oldBackgrounColor;
                //}   
                //else
                //{
                //    GUILayout.Space(EditorGUIUtility.singleLineHeight);
                //}
            }

            // don't draw anything outside the visible display for performance - just add space as a dummy container
            //var afterEndCount = _messageLog.LogEntries.Count - end;
            //if (afterEndCount > 0)
            //GUILayout.Space(afterEndCount * EditorGUIUtility.singleLineHeight);
            //else
            //    GUILayout.Space(0);

            GUILayout.EndScrollView();
        }


        /// <summary>
        /// Draw the currently selected message
        /// </summary>
        void DrawMessageDetails()
        {
            if (_selectedRow >= 0 && _selectedRow < _messageLog.LogEntries.Count)
            {
                var boldLabelStyle = EditorHelper.BoldLabelStyle();

                _messageDetailsScrollPosition = EditorGUILayout.BeginScrollView(_messageDetailsScrollPosition);

                var currentLogEntry = _messageLog.LogEntries[_selectedRow];
                GUILayout.BeginHorizontal();
                GUILayout.Label("Log Entry Type", boldLabelStyle, GUILayout.Width(100));
                GUILayout.Label(currentLogEntry.LogEntryType.ToString(), GUILayout.Width(position.width - 100));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("Message Type", boldLabelStyle, GUILayout.Width(100));
                GUILayout.Label(currentLogEntry.MessageType, GUILayout.Width(position.width - 100));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("Time", boldLabelStyle, GUILayout.Width(100));
                GUILayout.Label(currentLogEntry.Time.ToShortTimeString(), GUILayout.Width(position.width - 100));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("Message", boldLabelStyle, GUILayout.Width(100));
                GUILayout.Label(currentLogEntry.Message, GUILayout.Width(position.width - 100));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("Contents:", boldLabelStyle, GUILayout.Width(100));
                GUILayout.Label(currentLogEntry.Contents, GUILayout.Width(position.width - 100));
                GUILayout.EndHorizontal();
                GUILayout.Label("Call Stack", boldLabelStyle, GUILayout.Width(100));
                if (currentLogEntry.StackTrace != null && currentLogEntry.StackTrace.GetFrames() != null)
                {
                    foreach (var frame in currentLogEntry.StackTrace.GetFrames())
                    {
                        GUILayout.Label(frame.ToString());
                    }
                    GUILayout.Label(currentLogEntry.Contents);
                }

                EditorGUILayout.EndScrollView();
            }
        }


        /// <summary>
        /// Handle resize of the split messages window
        /// </summary>
        void HandleResize()
        {
            var dragRect = new Rect(0, _topPanelHeight, position.width, 3f);

            GUI.DrawTexture(dragRect, MakeColoredTexture(1, 1, _resizeAreaColour));
            EditorGUIUtility.AddCursorRect(dragRect, MouseCursor.ResizeVertical);

            if (Event.current.type == EventType.MouseDown && dragRect.Contains(Event.current.mousePosition))
            {
                _isResizing = true;
            }

            if (_isResizing)
            {
                _topPanelHeight = Mathf.Clamp(Event.current.mousePosition.y, 100, position.height - 100);
                Repaint();
            }

            if (Event.current.type == EventType.MouseUp)
                _isResizing = false;

        }

        /// <summary>
        /// Draw the statistics
        /// </summary>
        void DrawStatistics()
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
                _statisticsScrollPosition = GUILayout.BeginScrollView(_statisticsScrollPosition);

                // Acquire keys and sort them.
                var list = _messageLog.Statistics.Keys.ToList();
                list.Sort();

                // Loop through keys.
                foreach (var key in list)
                {
                    var statisticsEntry = _messageLog.Statistics[key];
                    drawnLines++;
                    var s = new GUIStyle();
                    s.normal.background = MakeColoredTexture(1, 1, new Color(1.0f, 1.0f, 1.0f, 0.1f));
                    GUILayout.BeginHorizontal(s);
                    GUI.backgroundColor = (drawnLines % 2 == 0) ? _lineColour1 : _lineColour2;
                    GUILayout.Label(key, GUILayout.Width(250));
                    GUILayout.Label(statisticsEntry.MessageCount.ToString(), GUILayout.Width(150));
                    GUILayout.Label(statisticsEntry.HandlerCount.ToString(), GUILayout.Width(150));
                    GUILayout.EndHorizontal();
                }

                GUILayout.EndScrollView();
            }
        }


        Texture2D MakeColoredTexture(int width, int height, Color col)
        {
            var pixels = new Color[width * height];

            for (var i = 0; i < pixels.Length; i++)
                pixels[i] = col;

            var texture = new Texture2D(width, height);
            texture.SetPixels(pixels);
            texture.Apply();

            return texture;
        }
    }
}

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

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GameFramework.EditorExtras.Editor
{
    /// <summary>
    /// Helper functions for dealing with editor windows, inspectors etc...
    /// </summary>
    public class EditorHelper
    {
        #region Drawing of GUI Elements
        /// <summary>
        /// Show a button trimmed to the length of the text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public static bool ButtonTrimmed(string text, GUIStyle style)
        {
            return GUILayout.Button(text, style, GUILayout.MaxWidth(style.CalcSize(new GUIContent(text)).x));
        }


        /// <summary>
        /// Show a button trimmed to the length of the text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public static bool ButtonTrimmed(string text, Texture2D texture, GUIStyle style, string tooltip = null)
        {
            if (texture != null)
                return GUILayout.Button(new GUIContent(text, texture, tooltip), style, GUILayout.MaxWidth(style.CalcSize(new GUIContent(text)).x + texture.width));
            else
                return ButtonTrimmed(text, style);
        }


        /// <summary>
        /// Show a button that is styled to look like a link
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="url"></param>
        public static bool LinkButton(string text, bool restrictWidth = false, params GUILayoutOption[] options)
        {
            var style = GUI.skin.label;
            style.richText = true;
            text = string.Format("<color=#0000FF>{0}</color>", text);

            bool wasClicked;
            if (restrictWidth)
                wasClicked = GUILayout.Button(text, style, GUILayout.MaxWidth(style.CalcSize(new GUIContent(text)).x));
            else
                wasClicked = GUILayout.Button(text, style, options);

            var rect = GUILayoutUtility.GetLastRect();
            rect.width = style.CalcSize(new GUIContent(text)).x;
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);

            return wasClicked;
        }

        /// <summary>
        /// Show a toggle trimmed to the length of the text
        /// </summary>
        /// <param name="value"></param>
        /// <param name="text"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public static bool ToggleTrimmed(bool value, string text, GUIStyle style)
        {
            return GUILayout.Toggle(value, text, style, GUILayout.MaxWidth(style.CalcSize(new GUIContent(text)).x));
        }


        /// <summary>
        /// Show a label trimmed to the length of the text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="style"></param>
        public static void LabelTrimmed(string text, GUIStyle style)
        {
            GUILayout.Label(text, style, GUILayout.MaxWidth(style.CalcSize(new GUIContent(text)).x));
        }

        #endregion Drawing of GUI Elements

        /// <summary>
        /// Try parsing a range string in the format 1,2,5-10,8 etc. and return a list of the expanded range.
        /// </summary>
        /// <param name="rangeString"></param>
        /// <param name="expandedRange"></param>
        /// <returns></returns>
        public static bool TryParseRangeString(string rangeString, out List<int> expandedRange)
        {
            var hasError = false;
            var tempRange = new List<int>();
            foreach (var s in rangeString.Split(','))
            {
                // try and get the number
                int num;
                if (int.TryParse(s, out num))
                {
                    tempRange.Add(num);
                }

                // otherwise we might have a range so split on the range delimiter
                else
                {
                    var parts = s.Split('-');
                    int start, end;

                    // now see if we can parse a start and end
                    if (parts.Length == 2 &&
                        int.TryParse(parts[0], out start) &&
                        int.TryParse(parts[1], out end) &&
                        end >= start)
                    {
                        for (var i = start; i <= end; i++)
                        {
                            tempRange.Add(i);
                        }
                    }
                    else
                        hasError = true;
                }
            }

            expandedRange = hasError ? null : tempRange;
            return hasError;
        }

        #region GUIStyle
        /// <summary>
        /// Get an italic style for a label
        /// </summary>
        /// <returns></returns>
        public static GUIStyle ItalicLabelStyle(TextAnchor alignment = TextAnchor.MiddleLeft) {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontStyle = FontStyle.Italic;
            labelStyle.alignment = alignment;
            return labelStyle;
        }

        /// <summary>
        /// Get a bold style for a label
        /// </summary>
        /// <returns></returns>
        public static GUIStyle BoldLabelStyle(TextAnchor alignment = TextAnchor.MiddleLeft)
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.alignment = alignment;
            return labelStyle;
        }
        #endregion GUIStyle
    }
}
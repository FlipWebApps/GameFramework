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

using GameFramework.Helper;
using System;
using System.Collections.Generic;
using System.Text;
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
        /// <param name="texture"></param>
        /// <param name="style"></param>
        /// <param name="tooltip"></param>
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
        /// <param name="text"></param>
        /// <param name="restrictWidth"></param>
        /// <param name="options"></param>
        public static bool LinkButton(string text, bool restrictWidth = false, params GUILayoutOption[] options)
        {
            var style = EditorStyles.whiteLabel;
            style.normal.textColor = new Color(0.25f, 0.5f, 0.9f, 1f);

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

        #region string processing
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


        /// <summary>
        /// Helper method to split a camel case string with spaces
        /// </summary>
        /// <param name="s"></param>
        /// <param name="prefixToStrip"></param>
        /// <param name="postfixToStrip"></param>
        /// <returns></returns>
        public static string PrettyPrintCamelCase(string s, string prefixToStrip = null, string postfixToStrip = null)
        {
            if (prefixToStrip != null && s.StartsWith(prefixToStrip))
                s = s.Substring(prefixToStrip.Length);
            if (postfixToStrip != null && s.EndsWith(postfixToStrip))
                s = s.Remove(s.Length - postfixToStrip.Length);

            var sb = new StringBuilder();
            foreach (var c in s)
            {
                if (char.IsUpper(c) && sb.Length > 0)
                    sb.Append(' ');
                sb.Append(c);
            }
            return sb.ToString();
        }


        /// <summary>
        /// This method will use reflection for finding all actions and add them to the list dynamically.
        /// </summary>
        public static List<Type> FindTypes(Type type)
        {
            // Go through all the types in the Assembly and find non abstract subclasses
            var actionSubTypeList = new List<Type>();
            var allTypes = type.Assembly.GetTypes();
            foreach (var typeInstance in allTypes)
            {
                if (typeInstance.IsSubclassOf(type) && !typeInstance.IsAbstract)
                {
                    actionSubTypeList.Add(typeInstance);
                }
            }
            return actionSubTypeList;
        }


        /// <summary>
        /// Get a list of ClassDetailsAttribute for a list of types.
        /// </summary>
        public static List<ClassDetailsAttribute> FindTypesClassDetails(Type type)
        {
            var types = FindTypes(type);
            return EditorHelper.TypeListClassDetails(types);
        }


        /// <summary>
        /// Convert a list of types to pretty printed names using a ClassDetails attribute if found.
        /// </summary>
        public static List<string> TypeListToNames(IEnumerable<Type> types)
        {
            var typeNameList = new List<string>();
            foreach (var type in types)
            {
                var attributes = type.GetCustomAttributes(typeof(ClassDetailsAttribute), true);
                if (attributes.Length > 0)
                    typeNameList.Add(((ClassDetailsAttribute)attributes[0]).Name);
                else
                    typeNameList.Add(PrettyPrintCamelCase(type.Name));
            }
            return typeNameList;
        }


        /// <summary>
        /// Get a list of ClassDetailsAttribute for a list of types.
        /// </summary>
        public static List<ClassDetailsAttribute> TypeListClassDetails(IEnumerable<Type> types)
        {
            var typeClassDetailsList = new List<ClassDetailsAttribute>();
            foreach (var type in types)
            {
                var attributes = type.GetCustomAttributes(typeof(ClassDetailsAttribute), true);
                ClassDetailsAttribute classDetailsAttribute;
                if (attributes.Length > 0)
                {
                    classDetailsAttribute = (ClassDetailsAttribute)attributes[0];
                }
                else
                {
                    Debug.LogError(string.Format("Missing ClassDetailsAttribute for type {0}.", type.Name));
                    classDetailsAttribute = new ClassDetailsAttribute(type.Name + " <Missing>", "Missing/" + type.Name);
                }
                classDetailsAttribute.ClassType = type;
                typeClassDetailsList.Add(classDetailsAttribute);
            }
            return typeClassDetailsList;
        }


        #endregion string processing

        #region Hideable Help Box
        /// <summary>
        /// Shows an editor help box that is hideable through a small button to the top right. 
        /// </summary>
        /// Pass back in teh returned rect to subsequent calls for the same helpbox
        /// <param name="key"></param>
        /// <param name="text"></param>
        /// <param name="lastRect"></param>
        /// <returns></returns>
        public static Rect ShowHideableHelpBox(string key, string text, Rect lastRect)
        {
            var hidden = EditorPrefs.GetBool(key, false);
            if (!hidden)
            {
                EditorGUILayout.HelpBox(text,
                    MessageType.Info);
                if (Event.current.type == EventType.Repaint)
                {
                    lastRect = GUILayoutUtility.GetLastRect();
                }
                var newRect = new Rect(lastRect.xMax - 15, lastRect.yMin, 15, 15);
                if (GUI.Button(newRect, "x")) EditorPrefs.SetBool(key, true);
            }
            return lastRect;
        }
        #endregion Hideable Help Box

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

        #region Editors
        
        /// <summary>
        /// Destroy all subeditors
        /// </summary>
        internal static void CleanupSubEditors(UnityEditor.Editor[] subEditors)
        {
            if (subEditors == null) return;
            for (var i = 0; i < subEditors.Length; i++)
            {
                if (subEditors[i] != null)
                {
                    UnityEngine.Object.DestroyImmediate(subEditors[i]);
                    subEditors[i] = null;
                }
            }
        }


        /// <summary>
        /// Draw the default inspector excluding listed objects
        /// </summary>
        internal static bool DrawDefaultInspector(SerializedObject obj, List<string> excludePaths)
        {
            obj.Update();
            var changed = DrawPropertiesWithExclusions(obj, excludePaths);
            obj.ApplyModifiedProperties();
            return changed;
        }


        /// <summary>
        /// Draw the default inspector excluding listed objects without doing update / applyModifiedProperties on the SerializedObject
        /// </summary>
        /// Use this when not drwaing a complete inspector
        internal static bool DrawPropertiesWithExclusions(SerializedObject obj, List<string> excludePaths)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty iterator = obj.GetIterator();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                if (excludePaths == null || !excludePaths.Contains(iterator.propertyPath))
                    EditorGUILayout.PropertyField(iterator, true);
                enterChildren = false;
            }
            return EditorGUI.EndChangeCheck();
        }

        /// <summary>
        /// Draw the default inspector excluding listed objects
        /// </summary>
        internal static bool DrawProperties(SerializedObject obj, List<string> paths)
        {
            EditorGUI.BeginChangeCheck();
            foreach (var path in paths)
            {
                SerializedProperty property = obj.FindProperty(path);
                if (property != null)
                    EditorGUILayout.PropertyField(property, true);
            }
            return EditorGUI.EndChangeCheck();
        }

        #endregion Editors
    }
}
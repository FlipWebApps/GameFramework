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

namespace GameFramework.EditorExtras.Editor
{
    /// <summary>
    /// Helper functions for dealing with editor windows, inspectors etc...
    /// </summary>
    public static class GuiStyles
    {
        static readonly Texture2D _earthIcon = AssetDatabase.LoadAssetAtPath(@"Assets\FlipWebApps\GameFramework\Sprites\Editor\Earth.png", typeof(Texture2D)) as Texture2D;
        static readonly Texture2D _earthIconBW = AssetDatabase.LoadAssetAtPath(@"Assets\FlipWebApps\GameFramework\Sprites\Editor\EarthBW.png", typeof(Texture2D)) as Texture2D;

        public static readonly GUIStyle ToolbarSearchField = "ToolbarSeachTextField";
        public static readonly GUIStyle ToolbarSearchFieldCancel = "ToolbarSeachCancelButton";
        public static readonly GUIStyle ToolbarSearchFieldCancelEmpty = "ToolbarSeachCancelButtonEmpty";

        public const float RemoveButtonWidth = 30f;

        #region GUI Styles

        public static Texture2D MakeColoredTexture(Color color)
        {
            var texture = new Texture2D(1, 1) {hideFlags = HideFlags.HideAndDontSave};
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }

        public static GUIStyle LocalisationToggleStyle
        {
            get
            {
                if (_localisationToggleStyle != null) return _localisationToggleStyle;

                _localisationToggleStyle = new GUIStyle
                {
                    normal = { background = _earthIconBW },
                    onNormal = { background = _earthIcon }
                };
                return _localisationToggleStyle;
            }
        }
        static GUIStyle _localisationToggleStyle;

        public static GUIStyle DropAreaStyle
        {
            get
            {
                if (_dropAreaStyle != null) return _dropAreaStyle;

                _dropAreaStyle = new GUIStyle(GUI.skin.box)
                {
                    normal = { background = EditorGUIUtility.isProSkin ? MakeColoredTexture(new Color(1f, 1f, 1f, 0.2f)) : MakeColoredTexture(new Color(1f, 1f, 1f, 0.6f)) },
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 14
                };
                return _dropAreaStyle;
            }
        }
        static GUIStyle _dropAreaStyle;

        public static GUIStyle BoxLightStyle
        {
            get
            {
                if (_boxLightStyle != null) return _boxLightStyle;

                _boxLightStyle = new GUIStyle(GUI.skin.box)
                {
                    normal = { background = EditorGUIUtility.isProSkin ? MakeColoredTexture(new Color(0.5f, 0.5f, 0.5f, 0.2f)) : MakeColoredTexture(new Color(1f, 1f, 1f, 0.4f)) },
                };
                return _boxLightStyle;
            }
        }
        static GUIStyle _boxLightStyle;

        public static GUIStyle BorderlessButtonStyle
        {
            get
            {
                if (_borderlessButtonStyle != null) return _borderlessButtonStyle;

                _borderlessButtonStyle = new GUIStyle(GUI.skin.button)
                {
                    normal = { background = EditorGUIUtility.isProSkin ? MakeColoredTexture(new Color(1f, 1f, 1f, 0.2f)) : MakeColoredTexture(new Color(.8f, .8f, .8f, 0.4f)) },
                    padding = new RectOffset(0, 0, 0, 0),
                    fontSize = 10
                };
                return _borderlessButtonStyle;
            }
        }
        static GUIStyle _borderlessButtonStyle;


        public static GUIStyle WordWrapStyle
        {
            get
            {
                if (_wordWrapStyle != null) return _wordWrapStyle;

                _wordWrapStyle = new GUIStyle(GUI.skin.textField)
                {
                    wordWrap = true
                };
                return _wordWrapStyle;
            }
        }
        static GUIStyle _wordWrapStyle;


        public static GUIStyle CenteredLabelStyle
        {
            get
            {
                if (_centeredLabelStyle != null) return _centeredLabelStyle;

                _centeredLabelStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter
                };
                return _centeredLabelStyle;
            }
        }
        static GUIStyle _centeredLabelStyle;
        #endregion GUI Styles

    }
}
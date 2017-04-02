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

using UnityEngine;
using UnityEditor;

namespace GameFramework.EditorExtras.Editor
{
    /// <summary>
    /// Decorator drawer for showing a help box with text and type specified by HelpBoxAttribute
    /// </summary>
    [CustomPropertyDrawer(typeof(HelpBoxAttribute))]
    public class HelpBoxDecoratorDrawer : DecoratorDrawer
    {
        public override void OnGUI(Rect position)
        {

            var helpBoxAttribute = (HelpBoxAttribute)attribute;
            MessageType type;
            switch (helpBoxAttribute.Type)
            {
                case HelpBoxAttribute.MessageType.Info:
                    type = MessageType.Info;
                    break;
                case HelpBoxAttribute.MessageType.Warning:
                    type = MessageType.Warning;
                    break;
                case HelpBoxAttribute.MessageType.Error:
                    type = MessageType.Error;
                    break;
                case HelpBoxAttribute.MessageType.None:
                    type = MessageType.None;
                    break;
                default:
                    type = MessageType.None;
                    break;
            }
            EditorGUI.HelpBox(position, helpBoxAttribute.Text, type);
        }

        public override float GetHeight()
        {
            var helpBoxAttribute = (HelpBoxAttribute)attribute;
            return EditorStyles.helpBox.CalcHeight(new GUIContent(helpBoxAttribute.Text), EditorGUIUtility.currentViewWidth);
        }
    }
}
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

namespace FlipWebApps.GameFramework.Scripts.Display.Placement.Components
{
    /// <summary>
    /// Aligh this game object with the screen bounds. Useful when catering for dynamic screen sizes.
    /// </summary>
    [AddComponentMenu("Game Framework/Display/Placement/AlignScreenBounds")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
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
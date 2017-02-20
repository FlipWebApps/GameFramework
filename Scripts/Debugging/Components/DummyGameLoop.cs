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

using GameFramework.GameStructure.Levels;
using UnityEngine;

namespace GameFramework.Debugging.Components
{
    /// <summary>
    /// A simple dummy game loop with win and lose buttons that will allow you to test the structure and interfaces in your game.
    /// </summary>
    /// This component works with some of the GameStructure functionality and gives an on screen meny 
    /// where you can set whether the game is won or lost. It requires that you have a LevelManager added 
    /// to your scene
    /// 
    /// It doesn't make any sense to use this in an actual game!!
    [AddComponentMenu("Game Framework/Debugging/DummyGameLoop")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class DummyGameLoop : MonoBehaviour
    {
        /// <summary>
        /// Placement types
        /// </summary>
        public enum PlacementType { TopRight, TopLeft, BottomRight, BottomLeft}

        /// <summary>
        /// Where on screen the buttons should be placed.
        /// </summary>
        [Tooltip("Where on screen the buttons should be placed.")]
        public PlacementType Placement = PlacementType.BottomRight;

        const int ButtonWidth = 75;
        const int ButtonHeight = 25;

        void OnGUI()
        {
            float x = 0, y = 0;
            switch (Placement)
            {
                case PlacementType.TopRight:
                    x = Screen.width - ButtonWidth - 10;
                    y = ButtonHeight + 20;
                    break;
                case PlacementType.TopLeft:
                    x = 10;
                    y = ButtonHeight + 20;
                    break;
                case PlacementType.BottomRight:
                    x = Screen.width - ButtonWidth - 10;
                    y = Screen.height - ButtonHeight - 10;
                    break;
                case PlacementType.BottomLeft:
                    x = 10;
                    y = Screen.height - ButtonHeight - 10;
                    break;
            }

            if (GUI.Button(new Rect(x, y, ButtonWidth, ButtonHeight), "Win"))
            {
                LevelManager.Instance.GameOver(true);
            }
            if (GUI.Button(new Rect(x, y - ButtonHeight - 10, ButtonWidth, ButtonHeight), "Lose"))
            {
                LevelManager.Instance.GameOver(false);
            }
        }
    }
}

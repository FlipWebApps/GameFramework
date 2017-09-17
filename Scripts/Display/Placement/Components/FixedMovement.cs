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

namespace GameFramework.Display.Placement.Components
{
    /// <summary>
    /// Move this gameobject at a given rate.
    /// </summary>
    [AddComponentMenu("Game Framework/Display/Placement/FixedMovement")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/display/")]
    public class FixedMovement : MonoBehaviour
    {
        /// <summary>
        /// Movement speed
        /// </summary>
        [Tooltip("Movement speed")]
        public Vector3 Speed = new Vector3(0, 0, 1);

        /// <summary>
        /// Specify whether to only move when a level is actually running, otherwise this gameobject will always move
        /// </summary>
        [Tooltip("Specify whether to only move when a level is actually running, otherwise this gameobject will always move")]
        public bool OnlyWhenLevelRunning = true;

        void Update()
        {
#pragma warning disable 618
            if (OnlyWhenLevelRunning && !LevelManager.Instance.IsLevelRunning)
#pragma warning restore 618
                return;

            transform.Translate(Speed * Time.deltaTime);
        }
    }
}
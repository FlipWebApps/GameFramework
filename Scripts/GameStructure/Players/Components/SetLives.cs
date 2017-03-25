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

using GameFramework.EditorExtras;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameFramework.GameStructure.Players.Components
{
    /// <summary>
    /// Sets the current players number of lives.
    /// </summary>
    /// Used for initialisation purposes.
    [AddComponentMenu("Game Framework/GameStructure/Players/SetLives")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/game-structure/players/")]
    public class SetLives : MonoBehaviour
    {
        /// <summary>
        /// Whether to use the number of lives set in GameManager
        /// </summary>
        [Tooltip("Whether to use the number of lives set in GameManager.")]
        public bool UseGlobalLifeCount = true;

        /// <summary>
        /// If not using global life cound then the number of lives to set.
        /// </summary>
        [ConditionalHide("UseGlobalLifeCount", true, true)]
        [Tooltip("If not using global life count then the number of lives to set.")]
        public int Lives;

        void Start()
        {
            Assert.IsTrue(GameManager.IsActive, "You need to add a GameManager to your scene to be able to use SetLives.");

            GameManager.Instance.Player.Lives = UseGlobalLifeCount ? GameManager.Instance.DefaultLives : Lives;
        }
    }
}
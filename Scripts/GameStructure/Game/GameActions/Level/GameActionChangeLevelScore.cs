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

using GameFramework.GameStructure.Game.ObjectModel.Abstract;
using GameFramework.GameStructure.Levels;
using GameFramework.Helper;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameFramework.GameStructure.Game.GameActions.Level
{
    /// <summary>
    /// GameAction class that changes the level score
    /// </summary>
    [System.Serializable]
    [ClassDetails("Level: Change Score", "Level/Change Score", "Increase of decrease the currently running level's score.")]
    public class GameActionChangeLevelScore : GameAction
    {
        /// <summary>
        /// An amount that specifies how much the score should change by. Put a minus value to decrease.
        /// </summary>
        public int Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                _amount = value;
            }
        }
        [Tooltip("An amount that specifies how much the score should change by. Put a minus value to decrease.")]
        [SerializeField]
        int _amount = 1;

        /// <summary>
        /// Perform the action
        /// </summary>
        /// <returns></returns>
        protected override void Execute(bool isStart)
        {
            Assert.IsTrue(LevelManager.IsActive, "To use the Change Level Score Action, ensure that you have a LevelManager added to your scene.");
            LevelManager.Instance.Level.AddPoints(Amount);
        }
    }
}

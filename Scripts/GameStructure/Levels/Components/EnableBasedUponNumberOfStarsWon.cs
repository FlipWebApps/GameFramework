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

using GameFramework.GameObjects.Components.AbstractClasses;
using GameFramework.GameStructure.Levels.Messages;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameFramework.GameStructure.Levels.Components
{
    /// <summary>
    /// Shows an enabled or a disabled gameobject based upon the number of stars the player has for the current level.
    /// </summary>
    [AddComponentMenu("Game Framework/GameStructure/Levels/EnableBasedUponNumberOfStarsWon")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/game-structure/levels/")]
    public class EnableBasedUponNumberOfStarsWon : EnableDisableGameObjectMessaging<StarsWonMessage>
    {
        /// <summary>
        /// The number of stars this icon represents. If this star is won then the met gameobject is shown, otherwise the not met gameobject is shown.
        /// </summary>
        [Tooltip("The number of stars this icon represents. If this star is won the the met gameobject is shown, otherwise the not met gameobject is shown.")]
        public int Star;


        /// <summary>
        /// Custom initialisation
        /// </summary>
        public override void Start()
        {
            Assert.IsTrue(LevelManager.IsActive, "You need to add a LevelManager to your scene to be able to use EnableBasedUponNumberOfStarsWon.");

            var level = LevelManager.Instance.Level;
            RunMethod(new StarsWonMessage(level, level.StarsWon, level.StarsWon));
            base.Start();
        }


        /// <summary>
        /// Returns whether to show the condition met gameobject (true) or the condition not met one (false)
        /// </summary>
        /// <returns></returns>
        public override bool IsConditionMet(StarsWonMessage message)
        {
            return message.Level.IsStarWon(Star);
        }
    }
}
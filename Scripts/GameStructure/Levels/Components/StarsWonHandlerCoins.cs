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
using GameFramework.GameStructure.Levels.Messages;
using GameFramework.Messaging.Components.AbstractClasses;
using UnityEngine;

namespace GameFramework.GameStructure.Levels.Components
{
    /// <summary>
    /// A handler to set the number of stars won based upon the amount of coins obtained for the referenced Level.
    /// </summary>
    [AddComponentMenu("Game Framework/GameStructure/Levels/StarsWonHandlerCoins")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/game-structure/levels/")]
    public class StarsWonHandlerCoins : RunOnMessage<LevelCoinsChangedMessage>
    {
        /// <summary>
        /// Whether any targets should come from level configuration as set by json or API calls.
        /// </summary>
        [Tooltip("Whether any targets should come from level configuration as set by json or API calls.")]
        public bool TargetsFromLevelConfig;

        /// <summary>
        /// The number of coins the player must get to achieve 1 star.
        /// </summary>
        [Tooltip("The number of coins the player must get to achieve 1 star.")]
        [ConditionalHide("TargetsFromLevelConfig", true, true)]
        public int Target1Star = 10;

        /// <summary>
        /// The number of coins the player must get to achieve 2 stars.
        /// </summary>
        [Tooltip("The number of coins the player must get to achieve 2 stars.")]
        [ConditionalHide("TargetsFromLevelConfig", true, true)]
        public int Target2Stars = 20;

        /// <summary>
        /// The number of coins the player must get to achieve 3 stars.
        /// </summary>
        [Tooltip("The number of coins the player must get to achieve 3 stars.")]
        [ConditionalHide("TargetsFromLevelConfig", true, true)]
        public int Target3Stars = 30;

        /// <summary>
        /// The number of coins the player must get to achieve 4 stars.
        /// </summary>
        [Tooltip("The number of coins the player must get to achieve 4 stars.")]
        [ConditionalHide("TargetsFromLevelConfig", true, true)]
        public int Target4Stars = 40;

        /// <summary>
        /// Set the number of stars won based upon the number of collected coins.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool RunMethod(LevelCoinsChangedMessage message)
        {
            var level = LevelManager.Instance.Level;

            if (TargetsFromLevelConfig)
            {
                if (message.NewCoins >= level.Star1Target)
                    level.StarWon(1, true);
                if (message.NewCoins >= level.Star2Target)
                    level.StarWon(2, true);
                if (message.NewCoins >= level.Star3Target)
                    level.StarWon(3, true);
                if (message.NewCoins >= level.Star4Target)
                    level.StarWon(4, true);
            }
            else
            {
                if (message.NewCoins >= Target1Star)
                    level.StarWon(1, true);
                if (message.NewCoins >= Target2Stars)
                    level.StarWon(2, true);
                if (message.NewCoins >= Target3Stars)
                    level.StarWon(3, true);
                if (message.NewCoins >= Target4Stars)
                    level.StarWon(4, true);
            }

            return true;
        }
    }
}
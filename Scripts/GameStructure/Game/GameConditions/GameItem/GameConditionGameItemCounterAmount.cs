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
using GameFramework.Helper;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameFramework.GameStructure.Game.GameConditions.GameItem
{
    /// <summary>
    /// GameCondition for testing the coins of a GameItem.
    /// </summary>
    [System.Serializable]
    [ClassDetails("GameItem: Counter Amount", "GameItem/Counter Amount", "Testing a GameItem counters amount.")]
    public class GameConditionGameItemCounterAmount : GameConditionGameItemContextSelectableTypeCounter
    {
        /// <summary>
        /// Evaluate the current condition
        /// </summary>
        /// <returns></returns>
        public override bool Evaluate()
        {
            var gameItem = GameItem;
            if (gameItem)
            {
                var counterReference = GameItem.GetCounter(Counter);
                Assert.IsNotNull(counterReference, string.Format("The specified Counter '{0}' was not found. Check that is exists in the game configuration.", Counter));
                if (counterReference.Configuration.CounterType == ObjectModel.CounterConfiguration.CounterTypeEnum.Int)
                    return GameConditionHelper.CompareNumbers(counterReference.IntAmount, Comparison, IntAmount);
                else 
                    return GameConditionHelper.CompareNumbers(counterReference.FloatAmount, Comparison, FloatAmount);
            }
            return false;
        }
    }
}

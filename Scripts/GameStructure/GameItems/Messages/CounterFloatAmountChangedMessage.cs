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

using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.Messaging;

namespace GameFramework.GameStructure.GameItems.Messages
{
    /// <summary>
    /// A message that is generated when GameItem counter FloatAmount changes (assuming no overrides are in place).
    /// </summary>
    public class CounterFloatAmountChangedMessage : BaseMessage
    {
        public readonly GameItem GameItem;
        public readonly Counter Counter;
        public readonly float NewAmount;
        public readonly float OldAmount;


        public CounterFloatAmountChangedMessage(GameItem gameItem, Counter counter, float oldAmount)
        {
            GameItem = gameItem;
            Counter = counter;
            NewAmount = counter.FloatAmount;
            OldAmount = oldAmount;
        }

        /// <summary>
        /// Return a representation of the message
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("GameItem Type {0}, GameItem Number {1}, Counter {2}, New Amount {3}, Old Amount {4}", GameItem.IdentifierBase, GameItem.Number, Counter.Configuration.Name, NewAmount, OldAmount);
        }
    }
}

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
using GameFramework.GameStructure.Worlds.Messages;

namespace GameFramework.GameStructure.Worlds.ObjectModel
{
    /// <summary>
    /// For managing an array of worlds inlcuding selection, unlocking
    /// </summary>
    public class WorldGameItemManager : GameItemManager<World, GameItem>
    {
        /// <summary>
        /// Called when the current selection changes. Override this in any base class to provide further handling such as sending out messaging.
        /// </summary>
        /// <param name="newSelection"></param>
        /// <param name="oldSelection"></param>
        /// You may want to override this in your derived classes to send custom messages.
        public override void OnSelectedChanged(World newSelection, World oldSelection)
        {
                GameManager.SafeQueueMessage(new WorldChangedMessage(newSelection, oldSelection));
        }


        /// <summary>
        /// Return the total star won count for all worlds contained within this GameItemManager.
        /// </summary>
        public int StarsWon
        {
            get
            {
                var starsWonCount = 0;
                foreach (var world in Items)
                {
                    starsWonCount += world.StarsWon;
                }
                return starsWonCount;
            }
        }


        /// <summary>
        /// Return the total star count for all worlds contained within this GameItemManager.
        /// </summary>
        public int StarsTotal
        {
            get
            {
                var starsTotalCount = 0;
                foreach (var world in Items)
                {
                    starsTotalCount += world.StarsTotal;
                }

                return starsTotalCount;
            }
        }
    }
}
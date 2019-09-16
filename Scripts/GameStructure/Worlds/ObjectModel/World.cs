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

using GameFramework.GameStructure.Game.ObjectModel;
using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.GameStructure.Levels.ObjectModel;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameFramework.GameStructure.Worlds.ObjectModel
{
    /// <summary>
    /// World Game Item
    /// </summary>
    [CreateAssetMenu(fileName = "World_x", menuName = "Game Framework/World")]
    public class World : GameItem
    {
        /// <summary>
        /// Levels for this world
        /// </summary>
        public LevelGameItemManager Levels { get; set; }

        //public int SelectedLevel;

        /// <summary>
        /// A unique identifier for this type of GameItem
        /// </summary>
        public override string IdentifierBase { get { return "World"; } }

        /// <summary>
        /// A unique shortened version of IdentifierBase to save memory.
        /// </summary>
        public override string IdentifierBasePrefs { get { return "W"; } }

        /// <summary>
        /// Override in subclasses to return a list of custom counter configuration entries that should also
        /// be added to this GameItem.
        /// </summary>
        /// <returns></returns>
        public override List<CounterConfiguration> GetCounterConfiguration()
        {
            return GameConfiguration.WorldCounterConfiguration;
        }

        /// <summary>
        /// Return the total star won count for all levels contained within this world.
        /// </summary>
        public int StarsWon
        {
            get
            {
                Assert.IsNotNull(Levels,
                    string.Format(
                        "Error trying to get the star won count when no levels are set for world {0}", Number));

                return Levels.StarsWon;
            }
        }

        /// <summary>
        /// Return the total star count for all levels contained within this world.
        /// </summary>
        public int StarsTotal
        {
            get
            {
                Assert.IsNotNull(Levels,
                    string.Format(
                        "Error trying to get the stars total count when no levels are set for world {0}", Number));

                return Levels.StarsTotal;
            }
        }

        /// <summary>
        /// Update PlayerPrefs with setting or preferences for this item.
        /// Note: This does not call PlayerPrefs.Save()
        /// 
        /// If overriding from a base class be sure to call base.ParseGameData()
        /// </summary>
        //public override void UpdatePlayerPrefs()
        //{
        //    GameManager.Instance.Player.SetSetting(FullKey("SelectedLevel"), SelectedLevel);

        //    base.UpdatePlayerPrefs();
        //}
    }
}

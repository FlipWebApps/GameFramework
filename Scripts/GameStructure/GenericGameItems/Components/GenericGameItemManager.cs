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

using GameFramework.GameObjects.Components;
using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.GameStructure.GenericGameItems.ObjectModel;
using UnityEngine;

namespace GameFramework.GameStructure.GenericGameItems.Components
{
    /// <summary>
    /// Allows for automatic setup and referencing of a set of generic game items for your own usage.
    /// </summary>
    [AddComponentMenu("Game Framework/GameStructure/GenericGameItem/GenericGameItemManager")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class GenericGameItemManager : SingletonPersistant<GenericGameItemManager>
    {
        #region Editor Properties

        /// <summary>
        /// Whether to automatically setup GenericGameItems using default values. You can use GenericGameItems for your own purpose.
        /// </summary>
        [Tooltip("Whether to automatically setup GenericGameItems using default values. You can use GenericGameItems for your own purpose.")]
        public bool AutoCreateItems = false;

        /// <summary>
        /// The number of standard GenericGameItems that should be automatically created by the framework.
        /// </summary>
        [Tooltip("The number of standard GenericGameItems that should be automatically created by the framework.")]
        public int NumberOfItems = 10;

        /// <summary>
        /// Whether to try and load data from a resources file.
        /// </summary>
        [Tooltip("Whether to try and load data from a resources file.")]
        public bool LoadFromResources = false;

        /// <summary>
        /// How we plan on letting users unlock GenericGameItems.
        /// </summary>
        [Tooltip("How we plan on letting users unlock GenericGameItems.")]
        public GameItem.UnlockModeType UnlockMode;

        /// <summary>
        /// The default number of coins to unlock GenericGameItems (can be overriden by json configuration files or code).
        /// </summary>
        [Tooltip("The default number of coins to unlock GenericGameItems (can be overriden by json configuration files or code).")]
        public int CoinsToUnlock = 10;

        #endregion Editor Properties

        #region Properties

        /// <summary>
        /// GameItemManager containing the current GenericGameItems
        /// </summary>
        public GameItemManager<GenericGameItem, GameItem> GenericGameItems { get; set; }

        #endregion Properties

        #region Setup

        /// <summary>
        /// Main setup routine
        /// </summary>
        protected override void GameSetup()
        {
            base.GameSetup();

            if (!AutoCreateItems) return;

            var coinsToUnlock = UnlockMode == GameItem.UnlockModeType.Coins ? CoinsToUnlock : -1;
            GenericGameItems = new GameItemManager<GenericGameItem, GameItem>();
            GenericGameItems.Load(1, NumberOfItems, coinsToUnlock); //, LoadFromResources);
        }

        #endregion Setup
    }
}
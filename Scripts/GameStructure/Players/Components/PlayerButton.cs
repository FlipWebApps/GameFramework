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

using GameFramework.GameStructure.Players.ObjectModel;
using GameFramework.GameStructure.GameItems.Components.AbstractClasses;
using GameFramework.GameStructure.GameItems.ObjectModel;
using UnityEngine;

namespace GameFramework.GameStructure.Players.Components
{
    /// <summary>
    /// Player details button
    /// </summary>
    /// Provides support for a details button including selection, unlocking, IAP and more.
    [AddComponentMenu("Game Framework/GameStructure/Players/PlayerButton")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/game-structure/Players/")]
    public class PlayerButton : GameItemButton<Player>
    {
        /// <summary>
        /// Pass static parametres to base class.
        /// </summary>
        public PlayerButton() : base("Player") { }


        /// <summary>
        /// Returns the GameItemManager that holds Players
        /// </summary>
        /// <returns></returns>
        protected override GameItemManager<Player, GameItem> GetGameItemManager()
        {
            return GameManager.Instance.Players;
        }
    }
}
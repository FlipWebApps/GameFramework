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

using GameFramework.GameStructure.GameItems.Components.AbstractClasses;
using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.GameStructure.Worlds.ObjectModel;
using GameFramework.Localisation.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace GameFramework.GameStructure.Worlds.Components
{
    /// <summary>
    /// Show the number of stars won within the referenced World in a UI Text component
    /// </summary>
    /// Information that can be displayed includes the name, number, description and value to unlock.

    [AddComponentMenu("Game Framework/GameStructure/Worlds/ShowWorldStars")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/game-structure/worlds/")]
    public class ShowWorldStars : GameItemContextBaseRunnable<World>
    {

        /// <summary>
        /// A localisation key or text string to use to display. You can include the values: {0} - Stars Total, {1} - Stars Won
        /// </summary>
        [Tooltip("A localisation key or text string to use to display. You can include the values:\n{0} - Stars Total\n{1} - Stars Won\n{2}")]
        public LocalisableText Text;

        Text _textComponent;

        protected override void Awake()
        {
            base.Awake();

            _textComponent = GetComponent<Text>();

            Assert.IsNotNull(_textComponent, "ShowWorldStars must be placed on a GameObject with a UI Text copmponent");
        }

        /// <summary>
        /// You should implement this method which is called from start and optionally if the selection chages.
        /// </summary>
        /// <param name="isStart"></param>
        public override void RunMethod(bool isStart = true)
        {
            Assert.IsNotNull(GameManager.Instance.Worlds, "Worlds are not setup when referenced from ShowWorldStars");

            if (GameItem != null)
            {
                var world = GameItem as World;
                _textComponent.text = Text.FormatValue(world.StarsTotal, world.StarsWon);
            }
        }

        /// <summary>
        /// Returns the current World GameItem
        /// </summary>
        /// <returns></returns>
        protected override GameItemManager<World, GameItem> GetGameItemManager()
        {
            Assert.IsNotNull(GameManager.Instance.Worlds, "Worlds are not setup when referenced from ShowWorldInfo");
            return GameManager.Instance.Worlds;
        }
    }
}
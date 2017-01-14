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

using GameFramework.Localisation;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace GameFramework.GameStructure.GenericGameItems.Components
{
    /// <summary>
    /// Show information about the currently selected GenericGameItem in a UI Text component
    /// </summary>
    [RequireComponent(typeof(Text))]
    [AddComponentMenu("Game Framework/GameStructure/Characters/Show GenericGameItem Info")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class ShowGenericGameItemInfo : MonoBehaviour
    {
        /// <summary>
        /// A localisation key or text string to use to dissplay. You can include the values: {0} - Number, {1} - Name, {2} - Description
        /// </summary>
        [Tooltip("A localisation key or text string to use to dissplay. You can include the values:\n{0} - Number\n{1} - Name\n{2} - Description")]
        public string Key;

        void Awake()
        {
            Assert.IsNotNull(GenericGameItemManager.Instance.GenericGameItems, "GenericGameItems are not setup when referenced from ShowGenericGameItemInfo");

            var genericGameItem = GenericGameItemManager.Instance.GenericGameItems.Selected;
            if (genericGameItem != null)
            {
                var textComponent = GetComponent<Text>();
                var text = LocaliseText.Exists(Key) ? LocaliseText.Get(Key) : Key;
                textComponent.text = string.Format(text, genericGameItem.Number, genericGameItem.Name, genericGameItem.Description);
            }
        }
    }
}
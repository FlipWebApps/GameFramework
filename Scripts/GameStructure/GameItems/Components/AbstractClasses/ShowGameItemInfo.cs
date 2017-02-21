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
using GameFramework.Localisation.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace GameFramework.GameStructure.GameItems.Components.AbstractClasses
{
    /// <summary>
    /// Show information about the currently selected level
    /// </summary>
    [RequireComponent(typeof(Text))]
    public abstract class ShowGameItemInfo<T> : GameItemContextBaseRunnable<T> where T : GameItem
    {
        /// <summary>
        /// DEPRECATED: Use the localisable text field below
        /// </summary>
        [Tooltip("DEPRECATED: Use the localisable text field below")]
        public string Key;

        /// <summary>
        /// A localisation key or text string to use to display. You can include the values: {0} - Number, {1} - Name, {2} - Description, {3} - Value to Unlock
        /// </summary>
        [Tooltip("A localisation key or text string to use to display. You can include the values:\n{0} - Number\n{1} - Name\n{2} - Description\n{3} - Value to Unlock")]
        public LocalisableText Text;

        Text _textComponent;

        protected override void Awake()
        {
            base.Awake();
            if (!string.IsNullOrEmpty(Key))
            {
                Debug.LogWarning("The Key property on the ShowXxxInfo component on " + gameObject.name + " is deprecated and will be removed. Copy the value over to the Text property instead and set Key to empty.");
                Text.Data = Key;
            }

            _textComponent = GetComponent<Text>();
        }

        /// <summary>
        /// You should implement this method which is called from start and optionally if the selection chages.
        /// </summary>
        /// <param name="isStart"></param>
        public override void RunMethod(bool isStart = true)
        {
            Assert.IsNotNull(GameManager.Instance.Levels, "Levels are not setup when referenced from ShowLevelInfo");

            if (GameItem != null)
            {
                _textComponent.text = Text.FormatValue(GameItem.Number, GameItem.Name, GameItem.Description, GameItem.ValueToUnlock);
            }
        }
    }
}
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

using GameFramework.Debugging;
using GameFramework.GameStructure.GameItems.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameFramework.GameStructure.GameItems.Components.AbstractClasses
{
    /// <summary>
    /// Set a sprite to the specified target
    /// </summary>
    /// <typeparam name="T">The type of the GameItem that we are getting the sprite from</typeparam>
    /// <typeparam name="TC">Component type to target</typeparam>
    public abstract class SetSprite<TC, T> : GameItemContextBaseRunnable<T> where TC : Component where T : GameItem
    {
        /// <summary>
        ///  The type of sprite to set
        /// </summary>
        public GameItem.LocalisableSpriteType SpriteType
        {
            get { return _spriteType; }
            set { _spriteType = value; }
        }
        [Tooltip("The type of sprite to set.")]
        [SerializeField]
        GameItem.LocalisableSpriteType _spriteType;

        /// <summary>
        /// For custom sprite types the name of the sprite to set.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name  = value; }
        }
        [Tooltip("For custom sprite types the name of the sprite to set.")]
        [SerializeField]
        string _name;

        TC _component;

        /// <summary>
        /// Setup
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            _component = GetComponent<TC>();
        }


        /// <summary>
        /// Called by the base class from start and optionally if the selection chages.
        /// </summary>
        /// <param name="isStart"></param>
        public override void RunMethod(bool isStart = true)
        {
            var sprite = GameItem.GetSprite(SpriteType, Name);
            // if not set then for legacy reasons we fallback to the default sprite loaded from resources.
            if (sprite == null)
            {
                sprite = GameItem.Sprite;
                MyDebug.Log(string.Format("The Sprite you are trying to instantiate is not setup. Please add it to the target GameItem {0}_{1} or put a default sprint in the resources folder.", GameItem.IdentifierBase, GameItem.Number));
            }
            Assert.IsNotNull(sprite, string.Format("The Sprite you are trying to instantiate is not setup. Please add it to the target GameItem {0}_{1} or put a default sprint in the resources folder.", GameItem.IdentifierBase, GameItem.Number));
            AssignSprite(_component, sprite);
        }


        /// <summary>
        /// Assigns the sprite to the target component.
        /// </summary>
        /// <returns></returns>
        protected abstract void AssignSprite(TC component, Sprite sprite);
    }
}
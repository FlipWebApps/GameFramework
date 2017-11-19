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
using GameFramework.GameStructure.Game.ObjectModel.Abstract;
using GameFramework.Helper;
using UnityEngine;

namespace GameFramework.GameStructure.Game.GameConditions.GameItem
{
    /// <summary>
    /// GameCondition for testing whether two GameItems match. Can e.g. be used for testing whether the current GameItem
    /// matches the selected one.
    /// </summary>
    [System.Serializable]
    [ClassDetails("GameItem: Matches GameItem", "GameItem/Matches GameItem", "Testing whether two GameItems match.")]
    public class GameConditionGameItemMatchesGameItem : GameConditionGameItemContextSelectableType
    {
        /// <summary>
        /// GameItem Context to operate within.
        /// </summary>
        public GameItemContext SecondContext
        {
            get { return _secondContext; }
            set { _secondContext = value; }
        }
        [Tooltip("The second context that we are working within for determining what GameItem to use.")]
        [SerializeField]
        GameItemContext _secondContext = new GameItemContext();



        /// <summary>
        /// Returns a reference to the GameItem that this context represents.
        /// </summary>
        public GameStructure.GameItems.ObjectModel.GameItem SecondGameItem
        {
            get
            {
                // refresh if needed
                if (SecondContext.ContextMode == GameItemContext.ContextModeType.Selected || SecondContext.ContextMode == GameItemContext.ContextModeType.Reference || _secondGameItem == null)
                    SecondGameItem = GameItemContext.GetGameItemFromContextReference(SecondContext, GetIBaseGameItemManager(), GetType().Name);
                return _secondGameItem;
            }
            private set
            {
                _secondGameItem = value;
            }
        }
        GameStructure.GameItems.ObjectModel.GameItem _secondGameItem;

        /// <summary>
        /// Evaluate the current condition
        /// </summary>
        /// <returns></returns>
        public override bool Evaluate()
        {
            var gameItem = GameItem;
            var secondGameItem = SecondGameItem;
            if (gameItem && secondGameItem)
            {
                return gameItem.Number == secondGameItem.Number;
            }
            return false;
        }


        #region IScriptableObjectContainerSyncReferences

        /// <summary>
        /// Workaround for ObjectReference issues with ScriptableObjects (See ScriptableObjectContainer for details)
        /// </summary>
        /// <param name="objectReferences"></param>
        public override void SetReferencesFromContainer(UnityEngine.Object[] objectReferences)
        {
            if (objectReferences != null && objectReferences.Length == 2)
            {
                base.SetReferencesFromContainer(objectReferences);
                SecondContext.ReferencedGameItemContextBase = objectReferences[1] as GameItems.Components.AbstractClasses.GameItemContextBase;
            }
        }

        /// <summary>
        /// Workaround for ObjectReference issues with ScriptableObjects (See ScriptableObjectContainer for details)
        /// </summary>
        public override UnityEngine.Object[] GetReferencesForContainer()
        {
            var baseReferences = base.GetReferencesForContainer();
            var objectReferences = new Object[2];
            objectReferences[0] = baseReferences[0];
            objectReferences[1] = SecondContext.ReferencedGameItemContextBase;
            return objectReferences;
        }

        #endregion IScriptableObjectContainerSyncReferences

    }
}

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

using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameFramework.GameStructure.GameItems.ObjectModel
{
    /// <summary>
    /// Used for setting what GameItem should be referenced by various different components.
    /// </summary>
    [Serializable]
    public class GameItemContext
    {
        #region Enums

        /// <summary>
        /// The context that we are working within for determining what GameItem to use
        /// </summary>
        /// Selected - The referenced GameItem is the selected one
        /// ByNumber - The referenced GameItem is selected based upon number.
        /// FromLoop - The referenced GameItem is taken from a parent loop enumerating GameItemManager - note will only be available for reference in the Awake method.
        /// Reference - The referenced GameItem is taken from a seperate referenced GameItemContext component.
        public enum ContextModeType
        {
            Selected,
            ByNumber,
            FromLoop,
            Reference
        }


        #endregion Enums

        #region Editor Parameters

        /// <summary>
        /// What GameItem we are referencing.
        /// </summary>
        public ContextModeType ContextMode
        {
            get
            {
                return _contextMode;
            }
            set
            {
                _contextMode = value;
            }
        }
        [Tooltip("What GameItem we are referencing.")]
        [SerializeField]
        ContextModeType _contextMode;


        /// <summary>
        /// If ContextMode is ByNumber then the number of the GameItem we are referencing
        /// </summary>
        public int Number
        {
            get
            {
                return _number;
            }
            set
            {
                _number = value;
            }
        }
        [Tooltip("The number of the GameItem we are referencing.")]
        [SerializeField]
        int _number;


        /// <summary>
        /// If ContextMode is Selected then whether to listen for changes and update the display when the selection changes.
        /// </summary>
        [Obsolete("The assumption is that if using selected then you will always want to react to changes!")]
        public bool ReactToChanges
        {
            get { return _reactToChanges; }
            set { _reactToChanges = value; }
        }
        [Tooltip("If ContextMode is Selected then whether to listen for changes and update the display when the selection changes.")]
        [SerializeField]
        bool _reactToChanges = true;


        /// <summary>
        /// If ContextMode is Referenced then the referenced GameItemContext component.
        /// </summary>
        public Components.AbstractClasses.GameItemContextBase ReferencedGameItemContextBase
        {
            get { return _referencedGameItemContextBase; }
            set { _referencedGameItemContextBase = value; }
        }
        [Tooltip("If ContextMode is Referenced then the referenced GameItemContext component.")]
        [SerializeField]
        Components.AbstractClasses.GameItemContextBase _referencedGameItemContextBase;

        #endregion Editor Parameters

        /// <summary>
        /// Method for getting the context mode taking into consideration following of references
        /// </summary>
        /// <returns></returns>
        public ContextModeType GetReferencedContextMode()
        {
            if (ContextMode == ContextModeType.Reference)
            {
                Assert.IsNotNull(ReferencedGameItemContextBase, "If you are using a GameItemContext of Reference then ensure that the ReferencedGameItem is setup.");
                return ReferencedGameItemContextBase.Context.GetReferencedContextMode();
            }
            else
            {
                return ContextMode;
            }
        }
    }
}
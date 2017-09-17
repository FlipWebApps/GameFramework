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
using UnityEngine;
using UnityEngine.Assertions;

namespace GameFramework.GameStructure.GameItems.Components.AbstractClasses
{
    public abstract class SetPositionFromGameItemVariable
    {
        /// <summary>
        /// Enum of position modes that can be used.
        /// </summary>
        public enum PositionModeType
        {
            Global,
            Local
        };
    }

    /// <summary>
    /// Set a transform position from a GameItem variables
    /// </summary>
    public abstract class SetPositionFromGameItemVariable<T> : GameItemContextBaseRunnable<T> where T : GameItem
    {

        /// <summary>
        /// The tag of a Vector3 variable to set the position from.
        /// </summary>
        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
        [Tooltip("The tag of a Vector3 variable to set the position from.")]
        [SerializeField]
        string _tag;

        /// <summary>
        /// The position mode that is to be used.
        /// </summary>
        public SetPositionFromGameItemVariable.PositionModeType PositionMode
        {
            get { return _positionMode; }
            set { _positionMode = value; }
        }
        [Tooltip("The position mode that is to be used.")]
        [SerializeField]
        SetPositionFromGameItemVariable.PositionModeType _positionMode;

        /// <summary>
        /// Called by the base class from start and optionally if the selection chages.
        /// </summary>
        /// <param name="isStart"></param>
        public override void RunMethod(bool isStart = true)
        {
            var vector3Variable = GameItem.Variables.GetVector3(Tag);
            Assert.IsNotNull(vector3Variable, string.Format("A variable with tag '{0}' was not found. Please check that it exists.", Tag));
            if (PositionMode == SetPositionFromGameItemVariable.PositionModeType.Global)
                transform.position = vector3Variable.Value;
            else
                transform.localPosition = vector3Variable.Value;
        }
    }
}
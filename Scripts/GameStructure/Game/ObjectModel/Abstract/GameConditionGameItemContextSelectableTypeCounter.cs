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

using UnityEngine;

namespace GameFramework.GameStructure.Game.ObjectModel.Abstract
{
    /// <summary>
    /// Base GameCondition class that that allows for specifying the GameItem context and counter
    /// </summary>

    [System.Serializable]
    public abstract class GameConditionGameItemContextSelectableTypeCounter : GameConditionGameItemContextSelectableType
    {
        /// <summary>
        /// The counter that we want to use.
        /// </summary>
        public string Counter
        {
            get { return _counter; }
            set { _counter = value; }
        }
        [Tooltip("The counter that we want to use.")]
        [SerializeField]
        string _counter;

        /// <summary>
        /// The type of the condition for number types.
        /// </summary>
        public GameConditionHelper.ComparisonType Comparison
        {
            get
            {
                return _comparison;
            }
            set
            {
                _comparison = value;
            }
        }
        [Tooltip("The type of the condition for number types.")]
        [SerializeField]
        GameConditionHelper.ComparisonType _comparison;

        /// <summary>
        /// A reference amount to compare the current amount against.
        /// </summary>
        public int IntAmount

        {
            get
            {
                return _intAmount;
            }
            set
            {
                _intAmount = value;
            }
        }
        [Tooltip("A reference amount to compare the current amount against.")]
        [SerializeField]
        int _intAmount;

        /// <summary>
        /// A reference amount to compare the current amount against.
        /// </summary>
        public float FloatAmount

        {
            get
            {
                return _floatAmount;
            }
            set
            {
                _floatAmount = value;
            }
        }
        [Tooltip("A reference amount to compare the current amount against.")]
        [SerializeField]
        float _floatAmount;
    }
}

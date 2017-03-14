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

namespace GameFramework.GameStructure.GameItems.ObjectModel.Conditions
{
    public class ConditionNumber
    {
        public enum ComparisonTypeNumber
        {
            LessThan,
            LessThanOrEqual,
            Equal,
            GreaterThanOrEqual,
            GreaterThan,
            NotEqual
        }
    }

    /// <summary>
    /// Class that holds information about a gameitem condition.
    /// </summary>
    [Serializable]
    public abstract class ConditionNumber<T> : Condition
    {

        #region Variables

        /// <summary>
        /// The type of the condition for number types.
        /// </summary>
        public ConditionNumber.ComparisonTypeNumber Comparison
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
        ConditionNumber.ComparisonTypeNumber _comparison;

        /// <summary>
        /// An value used to compare against a reference
        /// </summary>
        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
        [Tooltip("An value used to compare against a reference.")]
        [SerializeField]
        T _value;

        #endregion Variables


        /// <summary>
        /// Evaluate the passed number against this condition
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool EvaluateNumber(int referenceNumber)
        {
            var value = (int)(object)Value;
            switch (Comparison)
            {
                case ConditionNumber.ComparisonTypeNumber.LessThan:
                    return referenceNumber < value;
                case ConditionNumber.ComparisonTypeNumber.LessThanOrEqual:
                    return referenceNumber <= value;
                case ConditionNumber.ComparisonTypeNumber.Equal:
                    return referenceNumber == value;
                case ConditionNumber.ComparisonTypeNumber.GreaterThanOrEqual:
                    return referenceNumber >= value;
                case ConditionNumber.ComparisonTypeNumber.GreaterThan:
                    return referenceNumber > value;
                case ConditionNumber.ComparisonTypeNumber.NotEqual:
                    return referenceNumber != value;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }



        /// <summary>
        /// Evaluate the passed number against this condition
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool EvaluateNumber(float referenceNumber)
        {
            var value = (float)(object)Value;
            switch (Comparison)
            {
                case ConditionNumber.ComparisonTypeNumber.LessThan:
                    return referenceNumber < value;
                case ConditionNumber.ComparisonTypeNumber.LessThanOrEqual:
                    return referenceNumber <= value;
                case ConditionNumber.ComparisonTypeNumber.Equal:
                    return Mathf.Approximately(value, referenceNumber);
                case ConditionNumber.ComparisonTypeNumber.GreaterThanOrEqual:
                    return referenceNumber >= value;
                case ConditionNumber.ComparisonTypeNumber.GreaterThan:
                    return referenceNumber > value;
                case ConditionNumber.ComparisonTypeNumber.NotEqual:
                    return !Mathf.Approximately(value, referenceNumber);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

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
using GameFramework.Helper;
using UnityEngine;

namespace GameFramework.GameStructure.GameItems.ObjectModel.Conditions
{
    /// <summary>
    /// Class to allow for arrays of Condition based subclasses.
    /// </summary>
    [Serializable]
    public class ConditionReference : ScriptableObjectContainer<Condition>
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


        #region Built in variables

        /// <summary>
        /// An identifier that can be used for e.g. referencing a built in type or category.
        /// </summary>
        public int Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }
        [SerializeField]
        int _identifier;


        /// <summary>
        /// The type of the condition for boolean types.
        /// </summary>
        public bool BoolValue
        {
            get
            {
                return _boolValue;
            }
            set
            {
                _boolValue = value;
            }
        }
        [Tooltip("The type of the condition for boolean types.")]
        [SerializeField]
        bool _boolValue;

        /// <summary>
        /// The type of the condition for boolean types.
        /// </summary>
        public int IntValue
        {
            get
            {
                return _intValue;
            }
            set
            {
                _intValue = value;
            }
        }
        [Tooltip("The type of the condition for int types.")]
        [SerializeField]
        int _intValue;

        /// <summary>
        /// The type of the condition for number types.
        /// </summary>
        public ComparisonTypeNumber Comparison
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
        ComparisonTypeNumber _comparison;

        #endregion Built in variables


        /// <summary>
        /// Evaluate the passed number against this condition
        /// </summary>
        /// <param name="referenceNumber"></param>
        /// <param name="comparison"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool EvaluateNumber(int referenceNumber, ComparisonTypeNumber comparison, int value)
        {
            switch (comparison)
            {
                case ComparisonTypeNumber.LessThan:
                    return referenceNumber < value;
                case ComparisonTypeNumber.LessThanOrEqual:
                    return referenceNumber <= value;
                case ComparisonTypeNumber.Equal:
                    return referenceNumber == value;
                case ComparisonTypeNumber.GreaterThanOrEqual:
                    return referenceNumber >= value;
                case ComparisonTypeNumber.GreaterThan:
                    return referenceNumber > value;
                case ComparisonTypeNumber.NotEqual:
                    return referenceNumber != value;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }



        ///// <summary>
        ///// Evaluate the passed number against this condition
        ///// </summary>
        ///// <param name="number"></param>
        ///// <returns></returns>
        //public bool EvaluateNumber(float referenceNumber)
        //{
        //    switch (Comparison)
        //    {
        //        case ComparisonTypeNumber.LessThan:
        //            return referenceNumber < FloatValue;
        //        case ComparisonTypeNumber.LessThanOrEqual:
        //            return referenceNumber <= FloatValue;
        //        case ComparisonTypeNumber.Equal:
        //            return Mathf.Approximately(FloatValue, referenceNumber);
        //        case ComparisonTypeNumber.GreaterThanOrEqual:
        //            return referenceNumber >= FloatValue;
        //        case ComparisonTypeNumber.GreaterThan:
        //            return referenceNumber > FloatValue;
        //        case ComparisonTypeNumber.NotEqual:
        //            return !Mathf.Approximately(FloatValue, referenceNumber);
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }
        //}
    }

}

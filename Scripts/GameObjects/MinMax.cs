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

namespace GameFramework.GameObjects
{
    /// <summary>
    /// Simple struture to hold a set of minimum and maximum integer values.
    /// </summary>
    [Serializable]
    public struct MinMax
    {
        /// <summary>
        /// Minimum value (inclusive)
        /// </summary>
        public int Min;
        /// <summary>
        /// Maximum value (inclusive)
        /// </summary>
        public int Max;

        /// <summary>
        /// Difference between the two values
        /// </summary>
        public int Difference { get { return Max - Min; } }

        /// <summary>
        /// Whether a passed value is within the minimum / maximum range
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsWithinRange(int other)
        {
            return other >= Min && other <= Max;
        }

        /// <summary>
        /// Whether two MinMax structs overlap.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Overlaps(MinMax other) {
            return IsWithinRange(other.Min) || IsWithinRange(other.Max) || other.IsWithinRange(Min) || other.IsWithinRange(Max);
        }

        /// <summary>
        /// Returns a random integer number between min[inclusive] and max[exclusive] (Read Only).
        /// </summary>
        /// <returns></returns>
        public float RandomValue()
        {
            return UnityEngine.Random.Range(Min, Max + 1);
        }
    }

    /// <summary>
    /// Simple struture to hold a set of minimum and maximum float values.
    /// </summary>
    [Serializable]
    public struct MinMaxf
    {
        /// <summary>
        /// Minimum value (inclusive)
        /// </summary>
        public float Min;

        /// <summary>
        /// Maximum value (inclusive)
        /// </summary>
        public float Max;

        /// <summary>
        /// Difference between the two values
        /// </summary>
        public float Difference { get { return Max - Min; } }

        /// <summary>
        /// Whether a passed value is within the minimum / maximum range
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsWithinRange(float other)
        {
            return other >= Min && other <= Max;
        }

        /// <summary>
        /// Whether two MinMax structs overlap.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Overlaps(MinMaxf other)
        {
            return IsWithinRange(other.Min) || IsWithinRange(other.Max) || other.IsWithinRange(Min) || other.IsWithinRange(Max);
        }

        /// <summary>
        /// Returns a random integer number between min[inclusive] and max[inclusive] (Read Only).
        /// </summary>
        /// <returns></returns>
        public float RandomValue()
        {
            return UnityEngine.Random.Range(Min, Max);
        }
    }
}
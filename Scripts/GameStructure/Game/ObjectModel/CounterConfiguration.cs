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

namespace GameFramework.GameStructure.Game.ObjectModel
{
    /// <summary>
    /// Configuration information about a single counter entry including the key that identifies it.
    /// </summary>
    [System.Serializable]
    public class CounterConfiguration
    {
        public enum CounterTypeEnum { Int, Float }

        public enum SaveType { None, Always }

        #region Configuration Properties
        /// <summary>
        /// A unique key that identifies this counter.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        [SerializeField]
        [Tooltip("A unique name that identifies this counter.")]
        string _name;

        /// <summary>
        /// The type that this counter represents.
        /// </summary>
        public CounterTypeEnum CounterType
        {
            get
            {
                return _counterType;
            }
            set
            {
                _counterType = value;
            }
        }
        [SerializeField]
        [Tooltip("The type that this counter represents.")]
        CounterTypeEnum _counterType;

        /// <summary>
        /// The lowest value that this counter can take (if it is an int type).
        /// </summary>
        public int IntMinimum
        {
            get
            {
                return _intMinimum;
            }
            set
            {
                _intMinimum = value;
            }
        }
        [SerializeField]
        [Tooltip("The lowest value that this counter can take.")]
        int _intMinimum;

        /// <summary>
        /// The lowest value that this counter can take (if it is an float type).
        /// </summary>
        public float FloatMinimum
        {
            get
            {
                return _floatMinimum;
            }
            set
            {
                _floatMinimum = value;
            }
        }
        [SerializeField]
        [Tooltip("The lowest value that this counter can take.")]
        float _floatMinimum;

        /// <summary>
        /// The lowest value that this counter can take (if it is an int type).
        /// </summary>
        public int IntMaximum
        {
            get
            {
                return _intMaximum;
            }
            set
            {
                _intMaximum = value;
            }
        }
        [SerializeField]
        [Tooltip("The highest value that this counter can take.")]
        int _intMaximum = int.MaxValue;

        /// <summary>
        /// The lowest value that this counter can take (if it is an float type).
        /// </summary>
        public float FloatMaximum
        {
            get
            {
                return _floatMaximum;
            }
            set
            {
                _floatMaximum = value;
            }
        }
        [SerializeField]
        [Tooltip("The highest value that this counter can take.")]
        float _floatMaximum = float.MaxValue;

        /// <summary>
        /// The default value that this counter should take (if it is an int type).
        /// </summary>
        public int IntDefault
        {
            get
            {
                return _intDefault;
            }
            set
            {
                _intDefault = value;
            }
        }
        [SerializeField]
        [Tooltip("The default value that this counter should take.")]
        int _intDefault;

        /// <summary>
        /// The default value that this counter should take (if it is an float type).
        /// </summary>
        public float FloatDefault
        {
            get
            {
                return _floatDefault;
            }
            set
            {
                _floatDefault = value;
            }
        }
        [SerializeField]
        [Tooltip("The default value that this counter should take.")]
        float _floatDefault;

        /// <summary>
        /// If and when the counter should be saved for use across game sessions.
        /// </summary>
        public SaveType Save
        {
            get
            {
                return _save;
            }
            set
            {
                _save = value;
            }
        }
        [Tooltip("If and when the counter should be saved for use across game sessions.")]
        [SerializeField]
        SaveType _save;

        /// <summary>
        /// If and when the best value for the counter should be saved for use across game sessions.
        /// </summary>
        public SaveType SaveBest
        {
            get
            {
                return _saveBest;
            }
            set
            {
                _saveBest = value;
            }
        }
        [Tooltip("If and when the counter should be saved for use across game sessions.")]
        [SerializeField]
        SaveType _saveBest = SaveType.Always;

        #endregion Configuration Properties
    }
}
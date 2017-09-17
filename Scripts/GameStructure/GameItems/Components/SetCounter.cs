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

using GameFramework.GameStructure.GameItems.Components.AbstractClasses;
using UnityEngine;

namespace GameFramework.GameStructure.GameItems.Components
{
    /// <summary>
    /// Sets a counter to either the default or a specified value.
    /// </summary>
    /// Used for initialisation purposes.
    [AddComponentMenu("Game Framework/GameStructure/Common/Set Counter")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/game-structure/")]
    public class SetCounter : GameItemContextBaseSelectableTypeRunnableCounter
    {
        /// <summary>
        /// Whether to use the default counter amount.
        /// </summary>
        public bool UseDefaultAmount
        {
            get { return _useDefaultAmount; }
            set { _useDefaultAmount = value; }
        }
        [Tooltip("Whether to use the default counter amount.")]
        [SerializeField]
        bool _useDefaultAmount = true;

        /// <summary>
        /// The amount that the counter should be set to when not using the default value.
        /// </summary>
        public int IntAmount
        {
            get { return _intAmount; }
            set { _intAmount = value; }
        }
        [Tooltip("The amount that the counter should be set to when not using the default value.")]
        [SerializeField]
        int _intAmount;

        /// <summary>
        /// The amount that the counter should be set to when not using the default value.
        /// </summary>
        public float FloatAmount
        {
            get { return _floatAmount; }
            set { _floatAmount = value; }
        }
        [Tooltip("The amount that the counter should be set to when not using the default value.")]
        [SerializeField]
        float _floatAmount;

        /// <summary>
        /// You should implement this method which is called from start and optionally if the selection chages.
        /// </summary>
        /// <param name="isStart"></param>
        public override void RunMethod(bool isStart = true)
        {
            // only set on start.
            if (isStart && GameItem != null)
            {
                if (UseDefaultAmount)
                    CounterReference.Reset();
                else
                    CounterReference.Set(CounterReference.Configuration.CounterType == Game.ObjectModel.CounterConfiguration.CounterTypeEnum.Int ?
                                        IntAmount : FloatAmount);
            }
        }
    }
}
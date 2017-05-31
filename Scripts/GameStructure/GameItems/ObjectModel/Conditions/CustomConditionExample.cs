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
using UnityEngine;

namespace GameFramework.GameStructure.GameItems.ObjectModel.Conditions
{
    /// <summary>
    /// Example class that shows how to add your own custom condition classes.
    /// </summary>
    [CreateAssetMenu(fileName = "CustomConditionExample", menuName = "Game Framework/Custom Condition Example", order = 60)]
    [System.Serializable]
    public class CustomConditionExample : Condition
    {
        bool _errorShown;

        /// <summary>
        /// A string value that can be exposed through the editor and used by your own code as part of the comparison.
        /// </summary>
        public string CustomValue;

        /// <summary>
        /// Evaluate the current condition
        /// </summary>
        /// <returns></returns>
        public override bool EvaluateCondition(GameItem gameItem)
        {
            if (_errorShown) return false;
            _errorShown = true;
            MyDebug.LogWarning("TheCustomExample condition is for demonstration purposes only to show how to add your own custom conditions. Do not use this in custom code.");

            // Here you would have your own custom condition test.

            return false;
        }

        /// <summary>
        /// Returns whether this condition can process the specified GameItem / GameItem derived class
        /// </summary>
        /// <returns></returns>
        public override bool CanProcessGameItem(GameItem gameItem)
        {
            // Here you would optionally test the type of gameItem if this requires GameItems of a specific type.

            return true; // works for all GameItems
        }
    }
}

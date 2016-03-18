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

using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Levels.ObjectModel
{
    /// <summary>
    /// A simple implementation of a Level GameItemsManager for setting up Levels using the standard object model class.
    /// 
    /// Name and Description are collected from the localisation file.
    /// </summary>
    public class LevelGameItemsManager : GameItemsManager<Level, GameItem>
    {
        public enum LevelUnlockModeType { Custom, Completion, Coins }

        readonly int _numberOfLevels;
        readonly LevelUnlockModeType _levelUnlockModeType;
        readonly int _valueToUnlock;

        public LevelGameItemsManager(int numberOfLevels, LevelUnlockModeType levelUnlockModeType, int valueToUnlock)
        {
            _numberOfLevels = numberOfLevels;
            _levelUnlockModeType = levelUnlockModeType;
            _valueToUnlock = valueToUnlock;
        }

        protected override void LoadItems()
        {
            Items = new Level[_numberOfLevels];

            for (var count = 0; count < _numberOfLevels; count++ )
            {
                if (_levelUnlockModeType == LevelUnlockModeType.Completion)
                {
                    Items[count] = new Level(count + 1);
                }
                else
                {
                    Items[count] = new Level(count + 1, valueToUnlock: _valueToUnlock);
                }
            };
        }
    }
}
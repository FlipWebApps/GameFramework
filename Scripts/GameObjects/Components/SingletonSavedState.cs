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

using FlipWebApps.GameFramework.Scripts.Debugging;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.GameObjects.Components
{
    /// <summary>
    ///  A singleton implementation pattern that additionally allows for saving state upon exit or application pause events
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingletonSavedState<T> : Singleton<T> where T : Component
    {
        protected override void GameDestroy()
        {
            MyDebug.Log(TypeName + "(PersistantSingletonSavedState): GameDestroy");
            SaveState();
        }

        public abstract void SaveState();

        //Note that iOS applications are usually suspended and do not quit. You should tick "Exit on Suspend" in Player settings for iOS builds to cause the game to quit and not suspend, otherwise you may not see this call. If "Exit on Suspend" is not ticked then you will see calls to OnApplicationPause instead.
        protected virtual void OnApplicationQuit()
        {
            MyDebug.Log(TypeName + "(PersistantSingletonSavedState): OnApplicationQuit");

            SaveState();
        }

        protected virtual void OnApplicationPause(bool pauseStatus)
        {
            MyDebug.Log(TypeName + "(PersistantSingletonSavedState): OnApplicationPause");

            SaveState();
        }

        //protected virtual void Reset()
        //{
        //    MyDebug.Log(TypeName + "(PersistantSingletonSavedState): Reset");

        //    SaveState();
        //}
    }
}
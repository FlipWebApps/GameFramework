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

namespace GameFramework.GameObjects.Components
{
    /// <summary>
    ///  A singleton implementation pattern
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        /// <summary>
        /// Static singleton access property
        /// </summary>
        /// Use this for accessing the singleton instance.
        public static T Instance { get; private set; }

        /// <summary>
        /// The typename that this singleton is for.
        /// </summary>
        public string TypeName { get; private set; }


        /// <summary>
        /// Whether this singleton is active and setup.
        /// </summary>
        public static bool IsActive
        {
            get
            {
                return Instance != null;
            }
        }


        /// <summary>
        /// Try and setup the singleton
        /// </summary>
        /// This will only let one instance of the component exist. 
        /// If any other instances are attempted created then they will be automatically destroyed.
        void Awake()
        {
            TypeName = typeof(T).FullName;
            MyDebug.Log(TypeName + ": Awake");

            // First we check if there are any other instances conflicting then destroy this and return
            if (Instance != null)
            {
                if (Instance != this)
                    Destroy(gameObject);
                return;             // return is my addition so that the inspector in unity still updates
            }

            // Here we save our singleton instance
            Instance = this as T;

            // setup specifics.
            GameSetup();
        }


        /// <summary>
        /// Destroy the singleton
        /// </summary>
        void OnDestroy()
        {
            MyDebug.Log(TypeName + ": OnDestroy");

            if (Instance == this)
            {
                SaveState();
                GameDestroy();
            }
        }


        /// <summary>
        /// Save any state when the application quits.
        /// </summary>
        /// Note that iOS applications are usually suspended and do not quit. You should tick "Exit on Suspend" in Player settings 
        /// for iOS builds to cause the game to quit and not suspend, otherwise you may not see this call. If "Exit on Suspend" is 
        /// not ticked then you will see calls to OnApplicationPause instead.
        protected virtual void OnApplicationQuit()
        {
            MyDebug.Log(TypeName + ": OnApplicationQuit");

            SaveState();
        }


        /// <summary>
        /// Save any state when the application pauses.
        /// </summary>
        protected virtual void OnApplicationPause(bool pauseStatus)
        {
            MyDebug.Log(TypeName + ": OnApplicationPause");

            SaveState();
        }


        /// <summary>
        /// Called from Awake when the singleton is setup.
        /// </summary>
        protected virtual void GameSetup()
        {
        }


        /// <summary>
        /// Override this to save whatever state you need.
        /// </summary>
        /// This is typically called from OnDestroy and OnApplicationQuit although can be triggered from your own code if needed.
        public virtual void SaveState()
        {

        }

        
        /// <summary>
        /// Called from OnDestroy when the singleton is destroyed
        /// </summary>
        protected virtual void GameDestroy()
        {
        }


        //protected virtual void Reset()
        //{
        //    MyDebug.Log(TypeName + "(PersistantSingletonSavedState): Reset");

        //    SaveState();
        //}
    }
}
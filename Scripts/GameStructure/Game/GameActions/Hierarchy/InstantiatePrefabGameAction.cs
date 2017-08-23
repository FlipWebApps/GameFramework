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

using GameFramework.GameStructure.Game.ObjectModel.Abstract;
using GameFramework.Helper;
using UnityEngine;

namespace GameFramework.GameStructure.Game.GameActions.Hierarchy
{
    /// <summary>
    /// GameAction class that instantiates the specified prefab.
    /// </summary>
    [System.Serializable]
    [ClassDetails("Instantiate Prefab", "Hierarchy/Instantiate Prefab", "Instantiate the given prefab.")]
    public class InstantiatePrefabGameAction : GameAction
    {
        /// <summary>
        /// The prefab to instantiate
        /// </summary>
        public GameObject Prefab
        {
            get
            {
                return _prefab;
            }
            set
            {
                _prefab = value;
            }
        }
        [Tooltip("The prefab to instantiate")]
        [SerializeField]
        GameObject _prefab;

        /// <summary>
        /// A Transform that defines where to instantiate the prefab
        /// </summary>
        public Transform Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }
        [Tooltip("A Transform that defines where to instantiate the prefab")]
        [SerializeField]
        Transform _location;

        /// <summary>
        /// Perform the action
        /// </summary>
        /// <returns></returns>
        protected override void PerformAction()
        {
            if (Prefab != null && Location != null)
                Instantiate(Prefab, Location.position, Location.rotation);
        }


        /// <summary>
        /// Workaround for ObjectReference issues with ScriptableObjects (See ScriptableObjectContainer for details)
        /// </summary>
        /// <param name="References"></param>
        public override void SetReferencesFromContainer(UnityEngine.Object[] objectReferences)
        {
            if (objectReferences != null && objectReferences.Length == 1)
                Prefab = objectReferences[0] as GameObject;
        }

        /// <summary>
        /// Workaround for ObjectReference issues with ScriptableObjects (See ScriptableObjectContainer for details)
        /// </summary>
        /// <param name="References"></param>
        public override UnityEngine.Object[] GetReferencesForContainer()
        {
            var objectReferences = new Object[1];
            objectReferences[0] = Prefab;
            return objectReferences;
        }
    }
}

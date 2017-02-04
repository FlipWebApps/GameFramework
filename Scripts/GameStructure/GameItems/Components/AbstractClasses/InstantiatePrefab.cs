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

using GameFramework.GameObjects.Components.AbstractClasses;
using GameFramework.GameStructure.GameItems.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameFramework.GameStructure.GameItems.Components.AbstractClasses
{
    /// <summary>
    /// Create an instance of the specified prefab
    /// </summary>
    /// <typeparam name="T">The type of the GameItem that we are creating a button for</typeparam>
    public abstract class InstantiatePrefab<T> : RunOnState where T : GameItem
    {
        /// <summary>
        ///  The type of prefab to instantiate.
        /// </summary>
        public GameItem.LocalisablePrefabType PrefabType
        {
            get { return _prefabType; }
            set { _prefabType  = value; }
        }
        [Header("Prefab Settings")]
        [Tooltip("The type of prefab to instantiate.")]
        [SerializeField]
        GameItem.LocalisablePrefabType _prefabType;

        /// <summary>
        /// For custom prefab types the name of the prefab to instantiate.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name  = value; }
        }
        [Tooltip("For custom prefab types the name of the prefab to instantiate.")]
        [SerializeField]
        string _name;

        /// <summary>
        /// Optional parent for the newly instantiated object (if not specified adds as a child of the current gameobject.
        /// </summary>
        public GameObject Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
        [Tooltip("Optional parent for the newly instantiated object (if not specified adds as a child of the current gameobject.")]
        [SerializeField]
        GameObject _parent;

        /// <summary>
        /// If true, the parent-relative position, scale and rotation is modified such that the object keeps the same world space position, rotation and scale as before.
        /// </summary>
        public bool WorldPositionStays
        {
            get { return _worldPositionStays; }
            set { _worldPositionStays = value; }
        }
        [Tooltip("If true, the parent-relative position, scale and rotation is modified such that the object keeps the same world space position, rotation and scale as before.")]
        [SerializeField]
        bool _worldPositionStays;

        /// <summary>
        /// The current item that this button corresponds to.
        /// </summary>
        public T CurrentItem { get; set; }

        /// <summary>
        /// Setup and get various references for later use
        /// </summary>
        public override void Awake()
        {
            CurrentItem = GetCurrentItem();

            base.Awake();
        }

        /// <summary>
        /// Return the current GameItem that should be used for looking up the prefab.
        /// </summary>
        /// <returns></returns>
        protected abstract T GetCurrentItem();

        /// <summary>
        /// Show the correct gameobject based upon the current state.
        /// </summary>
        public override void RunMethod()
        {
            var newInstance = CurrentItem.InstantiatePrefab(PrefabType, Name, Parent == null ? transform : Parent.transform, WorldPositionStays);
            Assert.IsNotNull(newInstance, string.Format("The Prefab you are trying to instantiate is not setup. Please ensure the add it to the target GameItem {0}_{1}.", CurrentItem.IdentifierBase, CurrentItem.Number));
        }
    }
}
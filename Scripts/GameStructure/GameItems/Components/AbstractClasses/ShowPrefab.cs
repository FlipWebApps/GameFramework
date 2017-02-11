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

using System.Collections;
using System.Collections.Generic;
using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.Messaging;
using UnityEngine;
using UnityEngine.Assertions;

#if BEAUTIFUL_TRANSITIONS
using FlipWebApps.BeautifulTransitions.Scripts.Transitions;
#endif

namespace GameFramework.GameStructure.GameItems.Components.AbstractClasses
{
    /// <summary>
    /// abstract base for showing a prefab from the selected item including updating to a new prefab when the selection changes.
    /// </summary>
    /// <typeparam name="T">The type of the GameItem that we are creating a button for</typeparam>
    public abstract class ShowPrefab<T> : MonoBehaviour where T : GameItem
    {
        /// <summary>
        /// What GameItem we should use for showing prefabs from.
        /// </summary>
        public GameItemContext GameItemContext
        {
            get { return _gameItemContext; }
            set { _gameItemContext = value; }
        }
        [Header("GameItem")]
        [Tooltip("What GameItem we should use for showing prefabs from.")]
        [SerializeField]
        GameItemContext _gameItemContext;

        /// <summary>
        ///  The type of prefab to instantiate.
        /// </summary>
        public GameItem.LocalisablePrefabType PrefabType
        {
            get { return _prefabType; }
            set { _prefabType = value; }
        }
        [Header("Show Settings")]
        [Tooltip("The type of prefab to instantiate.")]
        [SerializeField]
        GameItem.LocalisablePrefabType _prefabType;

        /// <summary>
        /// For custom prefab types the name of the prefab to instantiate.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
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

        readonly Dictionary<int, GameObject> _cachedPrefabInstances = new Dictionary<int, GameObject>();
        GameObject _selectedPrefabInstance;

        /// <summary>
        /// Setup
        /// </summary>
        protected virtual void Start()
        {
            GetGameItemManager().SelectedChanged += SelectedChanged;
            var gameItem = GetGameItemManager().GetReferencedGameItem(GameItemContext);
            Assert.IsNotNull(gameItem, "A GameItem was not found. Check the GameItemContext settings refer to a valid GameItem.");
            Show(gameItem);
        }


        /// <summary>
        /// Destroy
        /// </summary>
        protected virtual void OnDestroy()
        {
            GetGameItemManager().SelectedChanged -= SelectedChanged;
        }

        /// <summary>
        /// Implement this method to return a GameItemManager that contains the GameItems that this works upon.
        /// </summary>
        /// <returns></returns>
        protected abstract GameItemManager<T, GameItem> GetGameItemManager();


        /// <summary>
        /// Called when the selection changes.
        /// </summary>
        /// <param name="oldItem"></param>
        /// <param name="item"></param>
        protected virtual void SelectedChanged(T oldItem, T item)
        {
            if (GameItemContext.ReactToSelectionChanges() || oldItem == null)
            {
                Show(item);
            }
        }


        /// <summary>
        /// Show the actual prefab
        /// </summary>
        /// <param name="item"></param>
        void Show(T item)
        {
            GameObject _newPrefabInstance;
            _cachedPrefabInstances.TryGetValue(item.Number, out _newPrefabInstance);
            if (_newPrefabInstance == null)
            {
                _newPrefabInstance = item.InstantiatePrefab(PrefabType, Name,
                    Parent == null ? transform : Parent.transform, WorldPositionStays);
                if (_newPrefabInstance != null)
                {
                    _newPrefabInstance.SetActive(false); // start inactive so we don't run transitions immediately
                    _cachedPrefabInstances.Add(item.Number, _newPrefabInstance);
                }
            }

            Assert.IsNotNull(_newPrefabInstance,
                string.Format(
                    "The Prefab you are trying to instantiate is not setup. Please ensure the add it to the target GameItem {0}_{1}.",
                    item.IdentifierBase, item.Number));

#if BEAUTIFUL_TRANSITIONS
            StartCoroutine(TransitionOutIn(_selectedPrefabInstance, _newPrefabInstance));
#else
                if (_selectedPrefabInstance != null)
                    _selectedPrefabInstance.SetActive(false);
                _newPrefabInstance.SetActive(true);
#endif
            _selectedPrefabInstance = _newPrefabInstance;
        }

#if BEAUTIFUL_TRANSITIONS
        IEnumerator TransitionOutIn(GameObject oldGameObject, GameObject newGameObject)
        {
            if (oldGameObject != null)
            {
                if (TransitionHelper.ContainsTransition(oldGameObject))
                {
                    var transitions = TransitionHelper.TransitionOut(oldGameObject);
                    yield return new WaitForSeconds(TransitionHelper.GetTransitionOutTime(transitions));
                }
                oldGameObject.SetActive(false);
            }
            newGameObject.SetActive(true);
        }
#endif
    }
}
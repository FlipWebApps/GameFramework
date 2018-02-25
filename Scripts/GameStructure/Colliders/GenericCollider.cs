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

using GameFramework.Helper.UnityEvents;
using GameFramework.GameStructure.Levels;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameFramework.GameStructure.Colliders
{
    /// <summary>
    /// Generic collider for acting when a tagged gameobject touches the attached collider or trigger.
    /// </summary>
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/game-structure/colliders/")]
    public class GenericCollider : MonoBehaviour
    {
        public enum DisableAfterUseType { None, ThisComponent, GameObject, Colliders}

        /// <summary>
        /// Tag with which this gameobject can collide.
        /// </summary>
        public string CollidingTag
        {
            get
            {
                return _collidingTag;
            }
            set
            {
                _collidingTag = value;
            }
        }
        [Tooltip("Tag with which this gameobject can collide.")]
        [SerializeField]
        string _collidingTag = "Player";

        /// <summary>
        /// An minimum time interval before a new collision can occur.
        /// </summary>
        public float Interval
        {
            get
            {
                return _interval;
            }
            set
            {
                _interval = value;
            }
        }
        [Tooltip("An minimum time interval before a new collision can occur.")]
        [SerializeField]
        float _interval;

        /// <summary>
        /// How this handling of collisions should be disabled after first use.
        /// </summary>
        public DisableAfterUseType DisableAfterUse
        {
            get
            {
                return _disableAfterUse;
            }
            set
            {
                _disableAfterUse = value;
            }
        }
        [Tooltip("How this component should be disabled after first use.")]
        [SerializeField]
        DisableAfterUseType _disableAfterUse;

        /// <summary>
        /// Specify whether to perform collision checiking only when a level is actually running, otherwise collisions will always be checked.
        /// </summary>
        public bool OnlyWhenLevelRunning
        {
            get
            {
                return _onlyWhenLevelRunning;
            }
            set
            {
                _onlyWhenLevelRunning = value;
            }
        }
        [Tooltip("Specify whether to perform collision checiking only when a level is actually running, otherwise collisions will always be checked.")]
        [SerializeField]
        bool _onlyWhenLevelRunning = true;

        /// <summary>
        /// Trigger Data for when entering a trigger
        /// </summary>
        public TriggerData Enter
        {
            get
            {
                return _enter;
            }
            set
            {
                _enter = value;
            }
        }
        [SerializeField]
        TriggerData _enter;

        /// <summary>
        /// Whether to continuously process when within a trigger (process trigger / collider stay events)
        /// </summary>
        public bool ProcessWithin
        {
            get
            {
                return _processWithin;
            }
            set
            {
                _processWithin = value;
            }
        }
        //[Header("When Within a Trigger")]
        [Tooltip("Whether to continuously process when within a trigger (process trigger / collider stay events)")]
        [SerializeField]
        bool _processWithin;

        /// <summary>
        /// A time interval to wait before and between runs within triggers.
        /// </summary>
        public float RunInterval
        {
            get
            {
                return _runInterval;
            }
            set
            {
                _runInterval = value;
            }
        }
        [Tooltip("A time to wait interval before and between runs within triggers.")]
        [SerializeField]
        float _runInterval;

        /// <summary>
        /// Trigger Data for when within a trigger
        /// </summary>
        public TriggerData Within
        {
            get
            {
                return _within;
            }
            set
            {
                _within = value;
            }
        }
        [SerializeField]
        TriggerData _within;

        /// <summary>
        /// Trigger Data for when leaving a trigger
        /// </summary>
        public TriggerData Exit
        {
            get
            {
                return _exit;
            }
            set
            {
                _exit = value;
            }
        }
        [Header("When Exiting a Trigger")]
        [SerializeField]
        TriggerData _exit;


        Collider[] _siblingColliders;
        Collider2D[] _siblingColliders2D;

        float _lastTriggerTime = -1000;
        float _lastWithinTime;
        bool _processingDisabled;

        public virtual void Awake()
        {
            _siblingColliders = GetComponents<Collider>();
            _siblingColliders2D = GetComponents<Collider2D>();
            Debug.LogWarning(gameObject.name + " : All old colliders are replaced in favour of the new, more powerful, Game Collider (Add Component | Game Framework | Game Structure | Game Collider) and will in the future be removed. Please convert your game to use this new component.");
        }

        #region Trigger / Collision Monobehaviour Methods
        public virtual void OnTriggerEnter2D(Collider2D otherCollider)
        {
            ProcessEnter(otherCollider.gameObject);
        }

        public virtual void OnTriggerStay2D(Collider2D otherCollider)
        {
            if (ProcessWithin)
                ProcessStay(otherCollider.gameObject);
        }

        public virtual void OnTriggerExit2D(Collider2D otherCollider)
        {
            ProcessExit(otherCollider.gameObject);
        }

        public virtual void OnTriggerEnter(Collider otherCollider)
        {
            ProcessEnter(otherCollider.gameObject);
        }

        public virtual void OnTriggerStay(Collider otherCollider)
        {
            if (ProcessWithin)
                ProcessStay(otherCollider.gameObject);
        }

        public virtual void OnTriggerExit(Collider otherCollider)
        {
            ProcessExit(otherCollider.gameObject);
        }

        public virtual void OnCollisionEnter2D(Collision2D collision)
        {
            ProcessEnter(collision.gameObject);
        }

        public virtual void OnCollisionStay2D(Collision2D collision)
        {
            if (ProcessWithin)
                ProcessStay(collision.gameObject);
        }

        public virtual void OnCollisionExit2D(Collision2D collision)
        {
            ProcessExit(collision.gameObject);
        }

        public virtual void OnCollisionEnter(Collision collision)
        {
            ProcessEnter(collision.gameObject);
        }

        public virtual void OnCollisionStay(Collision collision)
        {
            if (ProcessWithin)
                ProcessStay(collision.gameObject);
        }

        public virtual void OnCollisionExit(Collision collision)
        {
            ProcessExit(collision.gameObject);
        }

        #endregion Trigger / Collision Monobehaviour Methods

        /// <summary>
        /// Processes a trigger / collider enter event according to the current settings.
        /// </summary>
        /// <param name="collidingGameObject">The GameObject that we collided with</param>
        public virtual void ProcessEnter(GameObject collidingGameObject)
        {
            if ((!OnlyWhenLevelRunning || LevelManager.Instance.IsLevelRunning) &&
                collidingGameObject.CompareTag(CollidingTag) && Time.time > _lastTriggerTime + Interval && !_processingDisabled)
            {
                _lastTriggerTime = Time.time;
                _lastWithinTime = _lastTriggerTime;

                ProcessTriggerData(Enter, collidingGameObject);
                EnterOccurred(collidingGameObject);

                switch (DisableAfterUse)
                {
                    case DisableAfterUseType.None:
                        break;
                    case DisableAfterUseType.ThisComponent:
                        _processingDisabled = true;
                        enabled = false;
                        break;
                    case DisableAfterUseType.GameObject:
                        gameObject.SetActive(false);
                        break;
                    case DisableAfterUseType.Colliders:
                        foreach (var siblingCollider in _siblingColliders)
                            siblingCollider.enabled = false;
                        foreach (var siblingCollider2D in _siblingColliders2D)
                            siblingCollider2D.enabled = false;
                        break;
                }
            }
        }


        /// <summary>
        /// Called when we have detected and processed a valid trigger / collider enter based upon other settings
        /// </summary>
        /// Override this in you custom base classes that you want to hook into the trigger system.
        /// <param name="collidingGameObject">The GameObject that we collided with</param>
        public virtual void EnterOccurred(GameObject collidingGameObject)
        {
        }


        /// <summary>
        /// Processes a trigger / collider stay event according to the current settings.
        /// </summary>
        /// <param name="collidingGameObject">The GameObject that we collided with</param>
        public virtual void ProcessStay(GameObject collidingGameObject)
        {
            if ((!OnlyWhenLevelRunning || LevelManager.Instance.IsLevelRunning) &&
                collidingGameObject.CompareTag(CollidingTag) && Time.time > _lastWithinTime + RunInterval && !_processingDisabled)
            {
                _lastWithinTime = Time.time;

                ProcessTriggerData(Within, collidingGameObject);
                StayOccurred(collidingGameObject);
            }
        }


        /// <summary>
        /// Called when we have detected and processed a valid trigger / collider stay based upon other settings
        /// </summary>
        /// Override this in you custom base classes that you want to hook into the trigger system.
        /// <param name="collidingGameObject">The GameObject that we collided with</param>
        public virtual void StayOccurred(GameObject collidingGameObject)
        {
        }


        /// <summary>
        /// Processes a trigger / collider exit event according to the current settings.
        /// </summary>
        /// <param name="collidingGameObject">The GameObject that we collided with</param>
        public virtual void ProcessExit(GameObject collidingGameObject)
        {
            if ((!OnlyWhenLevelRunning || LevelManager.Instance.IsLevelRunning) &&
                collidingGameObject.CompareTag(CollidingTag) && !_processingDisabled)
            {
                ProcessTriggerData(Exit, collidingGameObject);
                ExitOccurred(collidingGameObject);
            }
        }


        /// <summary>
        /// Called when we have detected and processed a valid trigger / collider exit based upon other settings
        /// </summary>
        /// Override this in you custom base classes that you want to hook into the trigger system.
        /// <param name="collidingGameObject">The GameObject that we collided with</param>
        public virtual void ExitOccurred(GameObject collidingGameObject)
        {
        }


        /// <summary>
        /// Processing of any trigger data.
        /// </summary>
        /// <param name="triggerData"></param>
        /// <param name="collidingGameObject"></param>
        void ProcessTriggerData(TriggerData triggerData, GameObject collidingGameObject)
        {
            if (triggerData.InstantiatePrefab != null)
                Instantiate(triggerData.InstantiatePrefab, transform.position, Quaternion.identity);

#if PRO_POOLING
            if (!string.IsNullOrEmpty(triggerData.AddPooledItem))
                ProPooling.Components.GlobalPools.Instance.Spawn(triggerData.AddPooledItem, transform.position, Quaternion.identity);
#endif

            if (triggerData.AudioClip != null)
            {
                Assert.IsTrue(GameManager.Instance.IsInitialised, "Add a GameManager component to your scene to play audio effects.");
                GameManager.Instance.PlayEffect(triggerData.AudioClip);
            }

            foreach (var gameobject in triggerData.EnableGameObjects)
                if (gameobject != null)
                    gameobject.SetActive(true);

            foreach (var gameobject in triggerData.DisableGameObjects)
                if (gameobject != null)
                    gameobject.SetActive(false);

            triggerData.Callback.Invoke(collidingGameObject);
        }


        [System.Serializable]
        public class TriggerData
        {
            /// <summary>
            /// An optional prefab to instantiate
            /// </summary>
            public GameObject InstantiatePrefab
            {
                get
                {
                    return _instantiatePrefab;
                }
                set
                {
                    _instantiatePrefab = value;
                }
            }
            [Tooltip("An optional prefab to instantiate")]
            [SerializeField]
            GameObject _instantiatePrefab;

            /// <summary>
            /// Get an item from a Pro Pooling PoolManager pool with the given name (requires Pro Pooling)
            /// </summary>
            public string AddPooledItem
            {
                get
                {
                    return _addPooledItem;
                }
                set
                {
                    _addPooledItem = value;
                }
            }
            [Tooltip("Get an item from a Pro Pooling PoolManager pool with the given name (requires Pro Pooling)")]
            [SerializeField]
            string _addPooledItem;

            /// <summary>
            /// An audio clip to play
            /// </summary>
            public AudioClip AudioClip
            {
                get
                {
                    return _audioClip;
                }
                set
                {
                    _audioClip = value;
                }
            }
            [Tooltip("An audio clip to play")]
            [SerializeField]
            AudioClip _audioClip;

            /// <summary>
            /// An optional list of gameobjects to enable
            /// </summary>
            public GameObject[] EnableGameObjects
            {
                get
                {
                    return _enableGameObjects;
                }
                set
                {
                    _enableGameObjects = value;
                }
            }
            [Tooltip("An optional list of gameobjects to enable")]
            [SerializeField]
            GameObject[] _enableGameObjects;

            /// <summary>
            /// An optional list of gameobjects to disable
            /// </summary>
            public GameObject[] DisableGameObjects
            {
                get
                {
                    return _disableGameObjects;
                }
                set
                {
                    _disableGameObjects = value;
                }
            }
            [Tooltip("An optional list of gameobjects to disable")]
            [SerializeField]
            GameObject[] _disableGameObjects;

            /// <summary>
            /// Methods that should be called.
            /// </summary>
            public UnityGameObjectEvent Callback
            {
                get
                {
                    return _callback;
                }
                set
                {
                    _callback = value;
                }
            }
            [Tooltip("Methods that should be called.")]
            [SerializeField]
            UnityGameObjectEvent _callback;
        }
    }
}
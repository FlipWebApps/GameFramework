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

using System;
using GameFramework.Helper.UnityEvents;
using GameFramework.GameStructure.Levels;
using UnityEngine;
using GameFramework.GameStructure.Game.ObjectModel;
using GameFramework.GameStructure.Game.ObjectModel.Abstract;
using UnityEngine.Assertions;

namespace GameFramework.GameStructure.Game.Components
{
    /// <summary>
    /// Generic collision handler for acting when a tagged gameobject touches the attached collider or trigger.
    /// </summary>
    [AddComponentMenu("Game Framework/GameStructure/Collision Handler")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/game-structure/colliders/")]
    public class CollisionHandler : MonoBehaviour
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
        /// Whether to process when exiting a trigger (process trigger / collider exit events)
        /// </summary>
        public bool ProcessExit
        {
            get
            {
                return _processExit;
            }
            set
            {
                _processExit = value;
            }
        }
        [Tooltip("Whether to continuously process when exiting a trigger (process trigger / collider exit events)")]
        [SerializeField]
        bool _processExit;

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

        void Awake()
        {
            _siblingColliders = GetComponents<Collider>();
            _siblingColliders2D = GetComponents<Collider2D>();
        }

        void Start()
        {
            if (OnlyWhenLevelRunning)
                Assert.IsTrue(LevelManager.IsActive, string.Format("The CollisionHandler on {0} has Only When Level Running set, but no LevelManager was found. Either add one or clear this setting", name));

            GameActionHelper.InitialiseGameActions(Enter.ActionReferences, this);
            GameActionHelper.InitialiseGameActions(Within.ActionReferences, this);
            GameActionHelper.InitialiseGameActions(Exit.ActionReferences, this);
        }

        #region Trigger / Collision Monobehaviour Methods
        void OnTriggerEnter2D(Collider2D otherCollider)
        {
            ProcessEnterEvents(new GameAction.GameActionInvocationContext() { OtherGameObject = otherCollider.gameObject });
        }

        void OnTriggerStay2D(Collider2D otherCollider)
        {
            ProcessStayEvents(new GameAction.GameActionInvocationContext() { OtherGameObject = otherCollider.gameObject });
        }

        void OnTriggerExit2D(Collider2D otherCollider)
        {
            ProcessExitEvents(new GameAction.GameActionInvocationContext() { OtherGameObject = otherCollider.gameObject });
        }

        void OnTriggerEnter(Collider otherCollider)
        {
            ProcessEnterEvents(new GameAction.GameActionInvocationContext() { OtherGameObject = otherCollider.gameObject });
        }

        void OnTriggerStay(Collider otherCollider)
        {
            ProcessStayEvents(new GameAction.GameActionInvocationContext() { OtherGameObject = otherCollider.gameObject });
        }

        void OnTriggerExit(Collider otherCollider)
        {
            ProcessExitEvents(new GameAction.GameActionInvocationContext() { OtherGameObject = otherCollider.gameObject });
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            ProcessEnterEvents(new GameAction.GameActionInvocationContext() { OtherGameObject = collision.gameObject });
        }

        void OnCollisionStay2D(Collision2D collision)
        {
            ProcessStayEvents(new GameAction.GameActionInvocationContext() { OtherGameObject = collision.gameObject });
        }

        void OnCollisionExit2D(Collision2D collision)
        {
            ProcessExitEvents(new GameAction.GameActionInvocationContext() { OtherGameObject = collision.gameObject });
        }

        void OnCollisionEnter(Collision collision)
        {
            ProcessEnterEvents(new GameAction.GameActionInvocationContext() { OtherGameObject = collision.gameObject });
        }

        void OnCollisionStay(Collision collision)
        {
            ProcessStayEvents(new GameAction.GameActionInvocationContext() { OtherGameObject = collision.gameObject });
        }

        void OnCollisionExit(Collision collision)
        {
            ProcessExitEvents(new GameAction.GameActionInvocationContext() { OtherGameObject = collision.gameObject });
        }

        #endregion Trigger / Collision Monobehaviour Methods

        /// <summary>
        /// Processes a trigger / collider enter event according to the current settings.
        /// </summary>
        /// <param name="collidingGameObject">The GameObject that we collided with</param>
        void ProcessEnterEvents(GameAction.GameActionInvocationContext context)
        {
            if ((!OnlyWhenLevelRunning || LevelManager.Instance.IsLevelRunning) &&
                context.OtherGameObject.CompareTag(CollidingTag) && Time.time > _lastTriggerTime + Interval && !_processingDisabled)
            {
                _lastTriggerTime = Time.time;
                _lastWithinTime = _lastTriggerTime;

                ProcessTriggerData(Enter, context);
                EnterOccurred(context);

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
        public virtual void EnterOccurred(GameAction.GameActionInvocationContext context)
        {
        }


        /// <summary>
        /// Processes a trigger / collider stay event according to the current settings.
        /// </summary>
        /// <param name="collidingGameObject">The GameObject that we collided with</param>
        void ProcessStayEvents(GameAction.GameActionInvocationContext context)
        {
            if (ProcessWithin && (!OnlyWhenLevelRunning || LevelManager.Instance.IsLevelRunning) &&
                context.OtherGameObject.CompareTag(CollidingTag) && Time.time > _lastWithinTime + RunInterval && !_processingDisabled)
            {
                _lastWithinTime = Time.time;

                ProcessTriggerData(Within, context);
                StayOccurred(context);
            }
        }


        /// <summary>
        /// Called when we have detected and processed a valid trigger / collider stay based upon other settings
        /// </summary>
        /// Override this in you custom base classes that you want to hook into the trigger system.
        /// <param name="collidingGameObject">The GameObject that we collided with</param>
        public virtual void StayOccurred(GameAction.GameActionInvocationContext context)
        {
        }


        /// <summary>
        /// Processes a trigger / collider exit event according to the current settings.
        /// </summary>
        /// <param name="collidingGameObject">The GameObject that we collided with</param>
        void ProcessExitEvents(GameAction.GameActionInvocationContext context)
        {
            if (ProcessExit && (!OnlyWhenLevelRunning || LevelManager.Instance.IsLevelRunning) &&
                context.OtherGameObject.CompareTag(CollidingTag) && !_processingDisabled)
            {
                ProcessTriggerData(Exit, context);
                ExitOccurred(context);
            }
        }


        /// <summary>
        /// Called when we have detected and processed a valid trigger / collider exit based upon other settings
        /// </summary>
        /// Override this in you custom base classes that you want to hook into the trigger system.
        /// <param name="collidingGameObject">The GameObject that we collided with</param>
        public virtual void ExitOccurred(GameAction.GameActionInvocationContext context)
        {
        }


        /// <summary>
        /// Processing of any trigger data.
        /// </summary>
        /// <param name="triggerData"></param>
        /// <param name="collidingGameObject"></param>
        void ProcessTriggerData(TriggerData triggerData, GameAction.GameActionInvocationContext context)
        {
            GameActionHelper.ExecuteGameActions(triggerData.ActionReferences, false, context);
            triggerData.Callback.Invoke(context.OtherGameObject);
        }


        [Serializable]
        public class TriggerData
        {
            /// <summary>
            /// A list of actions that should be run.
            /// </summary>
            public GameActionReference[] ActionReferences
            {
                get
                {
                    return _actionReferences;
                }
                set
                {
                    _actionReferences = value;
                }
            }
            [Tooltip("A list of actions that should be run.")]
            [SerializeField]
            GameActionReference[] _actionReferences = new GameActionReference[0];

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
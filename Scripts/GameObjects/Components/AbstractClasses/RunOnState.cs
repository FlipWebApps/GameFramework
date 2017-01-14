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
using UnityEngine;

namespace GameFramework.GameObjects.Components.AbstractClasses
{
    /// <summary>
    /// An abstract class that runs a method at the specified perion.
    /// </summary>
    /// Override and implement the condition as you best see fit
    /// TODO: Not best proctice to have a redundant Update method. Consider splitting this component.
    public abstract class RunOnState : MonoBehaviour
    {
        public enum RunType { OnAwake, OnEnable, OnStart, OnUpdate, Periodically = 100 };
        /// <summary>
        /// When we want this to run.
        /// </summary>
        [Header("Run Settings")]
        [Tooltip("When we want this to run.")]
        public RunType Run;

        /// <summary>
        /// If Run is set to Periodically then how often to check
        /// </summary>
        [Tooltip("If Run is set to Periodically then how often to check")]
        public float RunFrequency = 1;

        /// <summary>
        /// Implementation for running on Awake. If you override this then be sure to call base.Awake()
        /// </summary>
        public virtual void Awake()
        {
            if (Run == RunType.OnAwake)
                RunMethod();
        }

        /// <summary>
        /// Implementation for running on OnEnable. If you override this then be sure to call base.OnEnable()
        /// </summary>
        public virtual void OnEnable()
        {
            if (Run == RunType.OnEnable)
                RunMethod();
        }

        /// <summary>
        /// Implementation for running on Start. If you override this then be sure to call base.Start()
        /// </summary>
        public virtual void Start()
        {
            if (Run == RunType.OnStart)
                RunMethod();
            else if (Run == RunType.Periodically)
                StartCoroutine(PeriodicUpdate());
        }

        /// <summary>
        /// Implementation for running on Update. If you override this then be sure to call base.Update()
        /// </summary>
        public virtual void Update()
        {
            if (Run == RunType.OnUpdate)
                RunMethod();
        }


        IEnumerator PeriodicUpdate()
        {
            while(true)
            {
                RunMethod();
                yield return new WaitForSeconds(RunFrequency);

            }
        }

        /// <summary>
        /// You should implement this method which is called based upon the Run property.
        /// </summary>
        public abstract void RunMethod();
    }
}
//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using System.Collections;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.GameObjects.Components
{
    /// <summary>
    /// An abstract class that runs a method at the specified perion.
    /// 
    /// Override and implement the condition as you best see fir
    /// 
    /// TODO Add option for coroutine so we don't run every frame.
    /// </summary>
    public abstract class RunOnState : MonoBehaviour
    {
        public enum RunType { OnAwake, OnEnable, OnStart, OnUpdate, Periodically = 100 };
        public RunType Run;
        public float RunFrequency = 1;

        public void Awake()
        {
            if (Run == RunType.OnAwake)
                RunMethod();
        }

        public void OnEnable()
        {
            if (Run == RunType.OnEnable)
                RunMethod();
        }

        public void Start()
        {
            if (Run == RunType.OnStart)
                RunMethod();
            else if (Run == RunType.Periodically)
                StartCoroutine(PeriodicUpdate());
        }

        public void Update()
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

        public abstract void RunMethod();
    }
}
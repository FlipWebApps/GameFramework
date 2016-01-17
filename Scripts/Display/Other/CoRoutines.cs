//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

//#if UNITY_EDITOR
//#define DEBUG
//#endif

namespace FlipWebApps.GameFramework.Scripts.Display.Other
{
    /// <summary>
    /// Generic coroutines that can be called
    /// </summary>
    public class CoRoutines
    {
        //Our wait function - do we need this or just use  new WaitForSeconds(animationTime)????????
        public static IEnumerator Wait(float duration)
        {
            for (float timer = 0; timer < duration; timer += Time.deltaTime)
                yield return 0;
        }

        //IEnumerator MoveOnPath(Transform transform, Vector3[] path, float moveSpeed, bool loop)
        //{
        //    do
        //    {
        //        foreach (var point in path)
        //            yield return StartCoroutine(MoveToPosition(transform, point, moveSpeed));
        //    }
        //    while (loop);
        //}

        public static IEnumerator DelayedMoveToPosition(MonoBehaviour moneBehaviour, float wait, Transform transform, Vector3 target, float duration)
        {
            yield return new WaitForSeconds(wait);
            yield return moneBehaviour.StartCoroutine(MoveToPosition(transform, target, duration));
        }


        public static IEnumerator MoveToPosition(Transform transform, Vector3 target, float duration)
        {
            float lerpPercent = 0;
            Vector3 startPosition = transform.position;
            while (lerpPercent < 1)
            {
                lerpPercent += ((1 / duration) * Time.deltaTime);
                if (transform == null) break;
                transform.position = Vector3.Lerp(startPosition, target, lerpPercent);
                yield return 0;
            }
            if (transform != null)
                transform.position = target;    // avoid rounding errors.
        }


        public static IEnumerator MoveToPositionAndBack(MonoBehaviour moneBehaviour, Transform transform, Vector3 target, float duration)
        {
            Vector3 startPosition = transform.position;
            yield return moneBehaviour.StartCoroutine(MoveToPosition(transform, target, duration));
            yield return moneBehaviour.StartCoroutine(MoveToPosition(transform, startPosition, duration));
        }


        public static IEnumerator MoveToPositionAndScale(Transform transform, Vector3 targetPosition, Vector3 targetScale, float duration)
        {
            float lerpPercent = 0;
            Vector3 startPosition = transform.position;
            Vector3 startScale = transform.localScale;
            while (lerpPercent < 1)
            {
                lerpPercent += ((1 / duration) * Time.deltaTime);
                if (transform == null) break;
                transform.position = Vector3.Lerp(startPosition, targetPosition, lerpPercent);
                transform.localScale = Vector3.Lerp(startScale, targetScale, lerpPercent);
                yield return 0;
            }
            transform.position = targetPosition;    // avoid rounding errors.
            transform.localScale = targetScale;    // avoid rounding errors.
        }


        public static IEnumerator Scale(Transform transform, Vector3 scaleTarget, float duration)
        {
            float lerpPercent = 0;
            Vector3 startScale = transform.localScale;
            while (lerpPercent < 1)
            {
                lerpPercent += ((1 / duration) * Time.deltaTime);
                if (transform == null) break;
                transform.localScale = Vector3.Lerp(startScale, scaleTarget, lerpPercent);
                yield return 0;
            }
            if (transform != null)
                transform.localScale = scaleTarget;    // avoid rounding errors.
        }


        public static IEnumerator ScaleThenDestroy(MonoBehaviour moneBehaviour, Transform transform, Vector3 vector, float duration)
        {
            yield return moneBehaviour.StartCoroutine(Scale(transform, vector, duration));
            if (transform != null)
                Object.Destroy(transform.gameObject);
        }

        ////
        //// Panels
        ////
        public static IEnumerator ShowPanel(GameObject gameObject, float animationTime = -1f, System.Action callback = null, string animationState = "PanelIn")
        {
            Animator animator = gameObject.GetComponent<Animator>();
            Assert.IsNotNull(animator, "To use transitions, you need to have an animator added.");
            animator.Play(animationState);

            if (Mathf.Approximately(animationTime, -1f))
            {
                // need to wait for ned of next rame before calling GetCurrentAnimatorStateInfo().length for new clip
                yield return new WaitForEndOfFrame();
                animationTime = animator.GetCurrentAnimatorStateInfo(0).length;
            }

            yield return new WaitForSeconds(animationTime);

            if (callback != null)
                callback();
        }

        public static IEnumerator HidePanel(GameObject gameObject, float animationTime = -1f, System.Action callback = null, string animationState = "PanelOut")
        {
            Animator animator = gameObject.GetComponent<Animator>();
            Assert.IsNotNull(animator, "To use transitions, you need to have an animator added.");
            animator.Play(animationState);

            if (Mathf.Approximately(animationTime, -1f))
            {
                // need to wait for ned of next rame before calling GetCurrentAnimatorStateInfo().length for new clip
                yield return new WaitForEndOfFrame();
                animationTime = animator.GetCurrentAnimatorStateInfo(0).length;
            }

            yield return new WaitForSeconds(animationTime);
            //gameObject.SetActive(false);

            if (callback != null)
                callback();
        }

    }
}
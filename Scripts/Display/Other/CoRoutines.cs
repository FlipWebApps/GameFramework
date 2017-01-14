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

//#if UNITY_EDITOR
//#define DEBUG
//#endif

namespace GameFramework.Display.Other
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


        public static IEnumerator DelayedCallback(float delay, System.Action callback)
        {
            if (!Mathf.Approximately(delay, 0))
                yield return  new WaitForSeconds(delay);

            if (callback != null)
                callback();
        }
    }
}
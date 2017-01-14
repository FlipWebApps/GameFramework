using GameFramework.Debugging;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.UI.Other.Components
{
    /// <summary>
    /// Provides support for scrolling a scrollrect to ensure that a specified item is displayed.
    /// </summary>
    [RequireComponent(typeof(ScrollRect))]
    [AddComponentMenu("Game Framework/UI/Other/ScrollRectEnsureVisible")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/ui/")]
    public class ScrollRectEnsureVisible : MonoBehaviour
    {
        /// <summary>
        /// How long to use to scroll any item into view (requires Beautiful Transitions). 0 indicates to display the item immediately.
        /// </summary>
        [Tooltip("How long to use to scroll any item into view (requires Beautiful Transitions). 0 indicates to display the item immediately.")]
        public float Time;

        /// <summary>
        /// Viewport
        /// </summary>
        [Tooltip("Viewport")]
        public RectTransform Viewport;

        ScrollRect _scrollRect;
        RectTransform _scrollTransform;
        RectTransform _content;

        void Awake()
        {
            _scrollRect = GetComponent<ScrollRect>();
            _scrollTransform = _scrollRect.transform as RectTransform;
            _content = _scrollRect.content;
        }

        /// <summary>
        /// Scroll the RectTransform so that it is centered on the specified target
        /// </summary>
        /// <param name="target"></param>
        public void CenterOnItem(RectTransform target)
        {
            // Start and target positions
            var itemCenterPositionInScroll = GetWorldPointInWidget(_scrollTransform, GetWidgetWorldPoint(target));
            var targetPositionInScroll = GetWorldPointInWidget(_scrollTransform, GetWidgetWorldPoint(Viewport));

            // distance to move
            var difference = targetPositionInScroll - itemCenterPositionInScroll;
            difference.z = 0f;

            //clear axis data that is not enabled in the scrollrect
            if (!_scrollRect.horizontal)
            {
                difference.x = 0f;
            }
            if (!_scrollRect.vertical)
            {
                difference.y = 0f;
            }

            var normalizedDifference = new Vector2(
                difference.x / (_content.rect.size.x - _scrollTransform.rect.size.x),
                difference.y / (_content.rect.size.y - _scrollTransform.rect.size.y));
            var newNormalizedPosition = _scrollRect.normalizedPosition - normalizedDifference;
            MyDebug.LogF("Difference ({0}), Normalised({1}), New Normalised ({2})", difference, normalizedDifference, newNormalizedPosition);

            if (_scrollRect.movementType != ScrollRect.MovementType.Unrestricted)
            {
                newNormalizedPosition.x = Mathf.Clamp01(newNormalizedPosition.x);
                newNormalizedPosition.y = Mathf.Clamp01(newNormalizedPosition.y);
                Debug.Log("Clamped normalized position: " + newNormalizedPosition);
            }

            ScrollToPosition(newNormalizedPosition, Time);
        }


        /// <summary>
        /// Get the associated scroll rect's normalised position
        /// </summary>
        /// <returns></returns>
        public Vector2 GetScrollPosition()
        {
            return _scrollRect.normalizedPosition;
        }


        /// <summary>
        /// Scroll to the specified normalised position.
        /// </summary>
        /// <param name="newNormalizedPosition"></param>
        /// <param name="time"></param>
        public void ScrollToPosition(Vector2 newNormalizedPosition, float time = 0.0f)
        {
            if (Mathf.Approximately(time, 0.0f))
            {
                _scrollRect.normalizedPosition = newNormalizedPosition;
            }
            else
            {
//#if //BEAUTIFUL_TRANSITIONS
                //ValueTo(_scrollRect.gameObject, 
                //    "from", _scrollRect.normalizedPosition,
                //    "to", newNormalizedPosition,
                //    "time", time,
                //    "easetype", "easeInOutBack",
                //    "onupdate", "TweenOnUpdateCallBack"
                //    ));
//#else
                _scrollRect.normalizedPosition = newNormalizedPosition;
//#endif
            }
        }


        //public void TweenOnUpdateCallBack(Vector2 pos)
        //{
        //    _scrollRect.normalizedPosition = pos;
        //}

        static Vector3 GetWidgetWorldPoint(RectTransform target)
        {
            // factor in pivot position + item size
            var pivotOffset = new Vector3((0.5f - target.pivot.x) * target.rect.size.x, (0.5f - target.pivot.y) * target.rect.size.y, 0f);
            var localPosition = target.localPosition + pivotOffset;
            return target.parent.TransformPoint(localPosition);
        }

        static Vector3 GetWorldPointInWidget(RectTransform target, Vector3 worldPoint)
        {
            return target.InverseTransformPoint(worldPoint);
        }
    }
}
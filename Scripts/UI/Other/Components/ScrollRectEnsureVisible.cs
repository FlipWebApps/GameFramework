using UnityEngine;
using UnityEngine.UI;

namespace FlipWebApps.GameFramework.Scripts.UI.Other.Components
{
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollRectEnsureVisible : MonoBehaviour
    {
        public float AnimTime = 0.15f;
        public bool Snap;
        public RectTransform MaskTransform;

        ScrollRect _scrollRect;
        RectTransform _scrollTransform;
        RectTransform _content;

        void Awake()
        {
            _scrollRect = GetComponent<ScrollRect>();
            _scrollTransform = _scrollRect.transform as RectTransform;
            _content = _scrollRect.content;
        }

        public void CenterOnItem(RectTransform target)
        {
            // Item is here
            var itemCenterPositionInScroll = GetWorldPointInWidget(_scrollTransform, GetWidgetWorldPoint(target));
            // But must be here
            var targetPositionInScroll = GetWorldPointInWidget(_scrollTransform, GetWidgetWorldPoint(MaskTransform));

            // So it has to move this distance
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

            Debug.Log("Difference: " + difference);

            var normalizedDifference = new Vector2(
                difference.x / (_content.rect.size.x - _scrollTransform.rect.size.x),
                difference.y / (_content.rect.size.y - _scrollTransform.rect.size.y));

            Debug.Log("Normalized Difference: " + normalizedDifference);

            var newNormalizedPosition = _scrollRect.normalizedPosition - normalizedDifference;
            Debug.Log("New normalized position: " + newNormalizedPosition);
            if (_scrollRect.movementType != ScrollRect.MovementType.Unrestricted)
            {
                newNormalizedPosition.x = Mathf.Clamp01(newNormalizedPosition.x);
                newNormalizedPosition.y = Mathf.Clamp01(newNormalizedPosition.y);
                Debug.Log("Clamped normalized position: " + newNormalizedPosition);
            }

            ScrollToPosition(newNormalizedPosition, Snap);
        }

        public Vector2 GetScrollPosition()
        {
            return _scrollRect.normalizedPosition;
        }

        public void ScrollToPosition(Vector2 newNormalizedPosition, bool snap = true)
        {
            if (snap)
            {
                _scrollRect.normalizedPosition = newNormalizedPosition;
            }
            else
            {
#if ITWEEN
                iTween.ValueTo(_scrollRect.gameObject, iTween.Hash(
                    "from", _scrollRect.normalizedPosition,
                    "to", newNormalizedPosition,
                    "time", AnimTime,
                    "easetype", "easeInOutBack",
                    "onupdate", "tweenOnUpdateCallBack"
                    ));
#else
                _scrollRect.normalizedPosition = newNormalizedPosition;
#endif
            }
        }


        public void TweenOnUpdateCallBack(Vector2 pos)
        {
            _scrollRect.normalizedPosition = pos;
        }

        Vector3 GetWidgetWorldPoint(RectTransform target)
        {
            //pivot position + item size has to be included
            var pivotOffset = new Vector3(
                (0.5f - target.pivot.x) * target.rect.size.x,
                (0.5f - target.pivot.y) * target.rect.size.y,
                0f);
            var localPosition = target.localPosition + pivotOffset;
            return target.parent.TransformPoint(localPosition);
        }

        Vector3 GetWorldPointInWidget(RectTransform target, Vector3 worldPoint)
        {
            return target.InverseTransformPoint(worldPoint);
        }
    }
}
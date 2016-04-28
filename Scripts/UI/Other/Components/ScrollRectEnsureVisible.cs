using FlipWebApps.GameFramework.Scripts.Debugging;
using UnityEngine;
using UnityEngine.UI;

namespace FlipWebApps.GameFramework.Scripts.UI.Other.Components
{
    [RequireComponent(typeof(ScrollRect))]
    [AddComponentMenu("Game Framework/UI/Other/ScrollRectEnsureVisible")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class ScrollRectEnsureVisible : MonoBehaviour
    {
        public float Time = 0.1f;
        public bool Immediate;
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

            ScrollToPosition(newNormalizedPosition, Immediate);
        }

        public Vector2 GetScrollPosition()
        {
            return _scrollRect.normalizedPosition;
        }

        public void ScrollToPosition(Vector2 newNormalizedPosition, bool immediate = true)
        {
            if (immediate)
            {
                _scrollRect.normalizedPosition = newNormalizedPosition;
            }
            else
            {
#if ITWEEN
                iTween.ValueTo(_scrollRect.gameObject, iTween.Hash(
                    "from", _scrollRect.normalizedPosition,
                    "to", newNormalizedPosition,
                    "time", Time,
                    "easetype", "easeInOutBack",
                    "onupdate", "TweenOnUpdateCallBack"
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
            // factor in pivot position + item size
            var pivotOffset = new Vector3((0.5f - target.pivot.x) * target.rect.size.x, (0.5f - target.pivot.y) * target.rect.size.y, 0f);
            var localPosition = target.localPosition + pivotOffset;
            return target.parent.TransformPoint(localPosition);
        }

        Vector3 GetWorldPointInWidget(RectTransform target, Vector3 worldPoint)
        {
            return target.InverseTransformPoint(worldPoint);
        }
    }
}
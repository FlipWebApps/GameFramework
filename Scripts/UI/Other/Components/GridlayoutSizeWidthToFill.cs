//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.UI.Other.Components
{
    public class GridlayoutSizeWidthToFill : MonoBehaviour
    {
        void Start()
        {
            UnityEngine.UI.GridLayoutGroup gridLayoutGroup = GetComponent<UnityEngine.UI.GridLayoutGroup>();
            int width = (int)(((RectTransform)gridLayoutGroup.transform).rect.width - ((gridLayoutGroup.constraintCount + 1) * gridLayoutGroup.spacing.x)) / gridLayoutGroup.constraintCount;
            gridLayoutGroup.cellSize = new Vector2(width, gridLayoutGroup.cellSize.y);
        }
    }
}
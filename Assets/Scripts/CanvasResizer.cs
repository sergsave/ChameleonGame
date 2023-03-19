using UnityEngine;

public class CanvasResizer : ResizerToCamera
{
    protected override void OnSizeUpdated(Vector2 size)
    {
        RectTransform rectTransform = transform as RectTransform;
        rectTransform.sizeDelta = size;
    }
}

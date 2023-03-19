using UnityEngine;

public class BackgroundResizer : ResizerToCamera
{
    private SpriteRenderer _spriteRenderer;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void OnSizeUpdated(Vector2 size)
    {
        Debug.Assert(_spriteRenderer.drawMode == SpriteDrawMode.Tiled, "Draw mode must be tiled");
        _spriteRenderer.size = size;
    }
}

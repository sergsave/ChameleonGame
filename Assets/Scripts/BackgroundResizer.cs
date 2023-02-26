using UnityEngine;

[ExecuteInEditMode]
public class BackgroundResizer : MonoBehaviour
{
    public new Camera camera;

    private SpriteRenderer _spriteRenderer;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }
    void Update()
    {
#if UNITY_EDITOR
        UpdateSprite();
#endif
    }

    private void UpdateSprite()
    {
        Debug.Assert(_spriteRenderer.drawMode == SpriteDrawMode.Tiled, "Draw mode must be tiled");
        _spriteRenderer.size = new Vector2(camera.orthographicSize * 2f * camera.aspect, camera.orthographicSize * 2f) / transform.localScale;
    }
}

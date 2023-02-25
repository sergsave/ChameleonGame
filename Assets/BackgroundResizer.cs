using UnityEngine;

[ExecuteInEditMode]
public class BackgroundResizer : MonoBehaviour
{
    public new Camera camera;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        Debug.Assert(spriteRenderer.drawMode == SpriteDrawMode.Tiled, "Draw mode must be tiled");
        spriteRenderer.size = new Vector2(camera.orthographicSize * 2f * camera.aspect / transform.localScale.x, camera.orthographicSize * 2f / transform.localScale.y);
    }
}

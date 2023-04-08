using UnityEngine;

[ExecuteInEditMode]
public abstract class ResizerToCamera : MonoBehaviour
{
    public new Camera camera;

    void Start()
    {
        UpdateSize();
    }

    void Update()
    {
#if UNITY_EDITOR
        UpdateSize();
#endif
    }

    protected abstract void OnSizeUpdated(Vector2 size);

    private void UpdateSize()
    {
        OnSizeUpdated(new Vector2(camera.orthographicSize * 2f * camera.aspect, camera.orthographicSize * 2f) / transform.localScale);
    }
}

using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ViewportHandler : MonoBehaviour
{
    // unitsSize - camera size in units, when game looks good in referenceAspect (width/height)
    public float unitsSize = 0;
    public float referenceAspect = 0f;

    private Camera _camera;

    // Sould call before all objects start
    private void Awake()
    {
        _camera = GetComponent<Camera>();
        ComputeResolution();
    }

    private void ComputeResolution()
    {
        if (_camera.aspect <= referenceAspect)
            _camera.orthographicSize = 1f / _camera.aspect * unitsSize * referenceAspect;
        else
            _camera.orthographicSize = unitsSize;
    }

    private void Update()
    {
#if UNITY_EDITOR
        ComputeResolution();
#endif
    }
}
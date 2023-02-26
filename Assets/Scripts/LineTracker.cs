using UnityEngine;

public class LineTracker : MonoBehaviour
{
    public GameObject trackingObject;

    private LineRenderer _lineRenderer;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        Debug.Assert(_lineRenderer.positionCount == 2, "Need two points for tracking");
    }

    void Update()
    {
        _lineRenderer.SetPosition(1, trackingObject.transform.localPosition);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    LineRenderer lineRenderer;

    public Vector2 direction { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        const float length = 10f;

        Vector3 direction3 = direction;
        direction3.z = transform.position.z;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + direction3 * length);
    }
}

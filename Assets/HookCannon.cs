using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookCannon : MonoBehaviour
{
    enum State
    {
        Stop,
        InProcess
    }

    State state = State.Stop;

    Bullet bullet;
    Aim aim;

    // Start is called before the first frame update
    void Start()
    {
        bullet = gameObject.GetComponentInChildren<Bullet>();
        aim = gameObject.GetComponentInChildren<Aim>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 relativeMousePos = mousePos - transform.position;

        Vector2 direction = relativeMousePos.normalized;

        if (state == State.Stop && Input.GetMouseButtonDown(0))
        {
            state = State.InProcess;
            bullet.Shoot(direction, () => state = State.Stop);
        }

        aim.direction = direction;
    }
}

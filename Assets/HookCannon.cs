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

    // Start is called before the first frame update
    void Start()
    {
        bullet = gameObject.GetComponentInChildren<Bullet>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Stop && Input.GetMouseButtonDown(0))
        {
            state = State.InProcess;
            bullet.Shoot(Vector2.up, () => state = State.Stop);
        }
    }
}

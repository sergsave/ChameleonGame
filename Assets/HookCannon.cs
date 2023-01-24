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
    Animator animator;

    LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        bullet = gameObject.GetComponentInChildren<Bullet>();
        animator = GetComponent<Animator>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Stop && Input.GetMouseButtonDown(0))
        {
            state = State.InProcess;
            animator.Play("OpenMouth", -1, 1f); // Это какой-то подбор таймингов
            bullet.Shoot(Vector2.up, () => {
                state = State.Stop;
                animator.Play("CloseMouth");

            }
            );
        } else
        {
            lineRenderer.SetPosition(1, bullet.transform.localPosition);
        }
    }
}

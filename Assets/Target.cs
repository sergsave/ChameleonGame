using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public enum MoveState 
    {
        Right,
        Left,
        Stop
    }

    public MoveState moveState { get; set; } = MoveState.Right;

    SpriteRenderer spriteRenderer;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        const int speed = 10;

        const float minX = -3.5f;
        const float maxX = 3.5f;

        switch (moveState)
        {
            case MoveState.Right:
            {
                spriteRenderer.flipX = false;

                if (transform.position.x > maxX)
                    moveState = MoveState.Left;
                else
                    transform.Translate(Vector3.right * speed * Time.deltaTime);
                break;
            }
            case MoveState.Left:
            {
                spriteRenderer.flipX = true;
                
                if (transform.position.x < minX)
                    moveState = MoveState.Right;
                else
                    transform.Translate(Vector3.left * speed * Time.deltaTime);
                break;
            }
            case MoveState.Stop:
            {
                animator.enabled = false; // полагаемся на то, что нельзя из stop вернуться в движение. фигня конечно
                break;
            }
            default:
                break;
        }

    }
}

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        const int speed = 10;

        const float minX = -6.0f;
        const float maxX = 6.0f;

        switch (moveState)
        {
            case MoveState.Right:
            {
                if (transform.position.x > maxX)
                    moveState = MoveState.Left;
                else
                    transform.Translate(Vector3.right * speed * Time.deltaTime);
                break;
            }
            case MoveState.Left:
            {
                if (transform.position.x < minX)
                    moveState = MoveState.Right;
                else
                    transform.Translate(Vector3.left * speed * Time.deltaTime);
                break;
            }
            default:
                break;
        }

    }
}

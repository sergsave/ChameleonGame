using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bullet : MonoBehaviour
{
    public const string tagToCheck = "Target";
    enum MoveState 
    {
        Stop,
        Forward,
        Backward
    }

    MoveState moveState = MoveState.Stop;

    Vector2 movingVector;
    System.Action shootCallback;
    GameObject collidedWith = null;
    
    SpriteRenderer spriteRenderer;

    Vector2 startPosition;

    public void Shoot(Vector2 direction, System.Action callback) 
    {
        if (moveState != MoveState.Stop)
            return;

        movingVector = direction;
        shootCallback = callback;
        moveState = MoveState.Forward;
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.localPosition; // Надо придумать способ получше. Нужно нормально позиционирование.
    }

    // Update is called once per frame
    void Update()
    {
        const int speed = 30;

        // При таких тупых проверках шарик будет возрвращаться не точно в изначальное положение
        const float maxLength = 15;
        const float minLenght = 0;

        System.Func<Vector2, float> relativeLength = position => 
            (position - startPosition).magnitude * (Vector2.Dot(movingVector, (position - startPosition)) > 0 ? 1 : -1);

        switch (moveState)
        {
            case MoveState.Stop:
                break;
            case MoveState.Forward:
            {
                if (relativeLength(transform.localPosition) > maxLength)
                    moveState = MoveState.Backward;
                else
                    transform.Translate(movingVector * speed * Time.deltaTime);
                break;
            }
            case MoveState.Backward:
            {
                if (relativeLength(transform.localPosition) < minLenght)
                {
                    moveState = MoveState.Stop;
                    if (collidedWith)
                        Destroy(collidedWith);

                    if (shootCallback != null)
                        shootCallback();

                    movingVector = Vector2.zero;
                    shootCallback = null;
                }
                else
                    transform.Translate(-movingVector * speed * Time.deltaTime);

                break;
            }
            default:
                break;
        }

        if (collidedWith)
        {
            Vector3 newPosition = transform.position;
            Vector3 offset = movingVector * spriteRenderer.bounds.size;
            offset.z = newPosition.z;
            newPosition += offset;

            collidedWith.transform.position = newPosition;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (moveState == MoveState.Forward && !collidedWith && col.gameObject.tag == tagToCheck)
        {
            Debug.Log("Collision");
            collidedWith = col.gameObject;
            collidedWith.GetComponent<Target>().moveState = Target.MoveState.Stop;
        }
    }
}

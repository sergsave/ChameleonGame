using UnityEngine;

public class BackForthMove : MonoBehaviour
{
    public enum MoveState
    {
        Forward,
        Backward,
        Stop
    }

    // Motion animation. Should support "IsMoving" bool parameter.
    public Animator animator;

    public MoveState moveState { get; set; } = MoveState.Forward;

    public int speed = 10;
    public float minX = 0f;
    public float maxX = 0f;

    void Start()
    {
        Debug.Assert(transform.eulerAngles.z == 0 || transform.eulerAngles.z == 180, $"Wrong rotation {transform.eulerAngles.z}");
    }

    void Update()
    {
        System.Action<bool> SetAnimationMoving = (isMoving) =>
        {
            if (animator)
                animator.SetBool("IsMoving", isMoving);
        };

        if (moveState == MoveState.Stop)
        {
            SetAnimationMoving(false);
            return;
        }

        SetAnimationMoving(true);

        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (transform.position.x < minX || transform.position.x > maxX)
        {
            moveState = (moveState == MoveState.Forward) ? MoveState.Backward : MoveState.Forward;
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            transform.Rotate(0, 0, 180);
        }
    }
}

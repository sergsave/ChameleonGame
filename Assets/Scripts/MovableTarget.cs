using UnityEngine;

public class MovableTarget : MonoBehaviour
{
    public enum MoveState
    {
        Right,
        Left,
        Stop
    }

    public MoveState moveState { get; set; } = MoveState.Right;

    public int speed = 10;
    public float minX = 0f;
    public float maxX = 0f;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    void Start()
    {
        // А точно надо здесь??
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        switch (moveState)
        {
            case MoveState.Right:
                {
                    _spriteRenderer.flipX = false;

                    if (transform.position.x > maxX)
                        moveState = MoveState.Left;
                    else
                        transform.Translate(Vector3.right * speed * Time.deltaTime);
                    break;
                }
            case MoveState.Left:
                {
                    _spriteRenderer.flipX = true;

                    if (transform.position.x < minX)
                        moveState = MoveState.Right;
                    else
                        transform.Translate(Vector3.left * speed * Time.deltaTime);
                    break;
                }
            case MoveState.Stop:
                {
                    _animator.enabled = false; // полагаемся на то, что нельзя из stop вернуться в движение. фигня конечно
                    break;
                }
            default:
                break;
        }

    }
}

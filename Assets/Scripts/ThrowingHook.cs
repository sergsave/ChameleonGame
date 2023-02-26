using UnityEngine;

public class ThrowingHook : MonoBehaviour
{
    public int speed = 30;
    public float maxLength = 15;

    [SerializeField] private string targetTag = "";

    private enum MoveState
    {
        Stop,
        Forward,
        Backward
    }

    private Vector2 _startPosition;
    private Vector2 _movingVector;
    private System.Action _throwCallback;
    private MoveState _moveState = MoveState.Stop;
    private GameObject _collidedWith = null;

    // Может как-то можно на корутины переделать??
    public void Throw(Vector2 direction, System.Action callback)
    {
        if (_moveState != MoveState.Stop)
            return;

        _movingVector = direction;
        _throwCallback = callback;
        _moveState = MoveState.Forward;
    }

    void Start()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        _startPosition = transform.localPosition;
    }

    void Update()
    {
        // При таких тупых проверках шарик будет возвращаться не точно в изначальное положение
        System.Func<Vector2, float> relativeLength = position =>
            (position - _startPosition).magnitude * (Vector2.Dot(_movingVector, (position - _startPosition)) > 0 ? 1 : -1);

        switch (_moveState)
        {
            case MoveState.Stop:
                break;
            case MoveState.Forward:
                {
                    if (relativeLength(transform.localPosition) > maxLength)
                        _moveState = MoveState.Backward;
                    else
                        transform.Translate(_movingVector * speed * Time.deltaTime);
                    break;
                }
            case MoveState.Backward:
                {
                    if (relativeLength(transform.localPosition) < 0)
                    {
                        _moveState = MoveState.Stop;

                        // TODO: Может лучше не здесь? Все-таки не связано с притягиванием
                        if (_collidedWith)
                            Destroy(_collidedWith);

                        if (_throwCallback != null)
                            _throwCallback();

                        _movingVector = Vector2.zero;
                        _throwCallback = null;
                    }
                    else
                        transform.Translate(-_movingVector * speed * Time.deltaTime);

                    break;
                }
            default:
                break;
        }

        if (_collidedWith)
        {
            Vector3 newPosition = transform.position;
            Vector3 offset = _movingVector; //  * _spriteRenderer.bounds.size; Как здесь обойтись без spriteRender???
            offset.z = newPosition.z;
            newPosition += offset;

            _collidedWith.transform.position = newPosition;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (_moveState == MoveState.Forward && !_collidedWith && col.gameObject.tag == targetTag)
        {
            _collidedWith = col.gameObject;
            // TODO: меньше конкретики у таргета
            _collidedWith.GetComponent<MovableTarget>().moveState = MovableTarget.MoveState.Stop;
        }
    }
}

using UnityEngine;

public class ThrowingHook : MonoBehaviour
{
    public int speed = 30;
    public float maxLength = 15;

    [SerializeField] private string targetTag = "";

    public enum HookState
    {
        Catched,
        Hooked
    };

    private enum MoveState
    {
        Stop,
        Forward,
        Backward
    }

    private Vector3 _startPosition;
    private Vector3 _endPosition;

    private Vector2 _movingVector;

    private System.Action<GameObject, HookState> _throwCallback;
    private MoveState _moveState = MoveState.Stop;
    private GameObject _collidedWith = null;

    // TODO: use coroutines?
    public void Throw(Vector2 direction, System.Action<GameObject, HookState> callback)
    {
        if (_moveState != MoveState.Stop)
            return;

        _movingVector = direction;
        _throwCallback = callback;
        _moveState = MoveState.Forward;

        _startPosition = transform.localPosition;
        _endPosition = _startPosition + (Vector3)_movingVector * maxLength;
    }

    void Update()
    {
        System.Func<Vector2> MovingDelta = () => _movingVector * speed * Time.deltaTime;

        switch (_moveState)
        {
            case MoveState.Stop:
                break;
            case MoveState.Forward:
                {
                    if (UpdatePosition(MovingDelta()) == UpdateResult.Bounded || _collidedWith)
                    {
                        _moveState = MoveState.Backward;
                    }
                    break;
                }
            case MoveState.Backward:
                {
                    if (UpdatePosition(-MovingDelta()) == UpdateResult.Bounded)
                    {
                        _moveState = MoveState.Stop;

                        if (_throwCallback != null)
                        {
                            _throwCallback(_collidedWith, HookState.Hooked);
                            _collidedWith = null;
                        }

                        _movingVector = Vector2.zero;
                        _throwCallback = null;
                    }
                    break;
                }
            default:
                break;
        }

        if (_collidedWith)
        {
            // TODO: offset?
            Vector3 newPosition = transform.position;
            newPosition.z = _collidedWith.transform.position.z;
            _collidedWith.transform.position = newPosition;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (_moveState == MoveState.Forward && !_collidedWith && col.gameObject.tag == targetTag)
        {
            _collidedWith = col.gameObject;

            if (_throwCallback != null)
                _throwCallback(_collidedWith, HookState.Catched);
        }
    }

    private float SignedLength(Vector3 position)
    {
        Vector2 resultVector = position - _startPosition;
        float sign = Mathf.Sign(Vector2.Dot(_movingVector, resultVector));
        return resultVector.magnitude * sign;
    }

    private enum UpdateResult
    {
        Updated,
        Bounded
    };

    private UpdateResult UpdatePosition(Vector3 delta)
    {
        Vector3 newPosition = transform.localPosition + delta;
        float length = SignedLength(newPosition);

        UpdateResult result = UpdateResult.Bounded;

        if (Mathf.Approximately(length, 0) || length < 0)
            newPosition = _startPosition;
        else if (Mathf.Approximately(length, maxLength) || length > maxLength)
            newPosition = _endPosition;
        else
            result = UpdateResult.Updated;

        transform.localPosition = newPosition;
        return result;
    }
}

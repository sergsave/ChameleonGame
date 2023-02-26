using UnityEngine;

public class ChameleonController : MonoBehaviour
{
    [SerializeField] private ThrowingHook hook;

    private enum State
    {
        Stop,
        InProcess
    }

    private State _state = State.Stop;
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_state == State.Stop && Input.GetMouseButtonDown(0))
        {
            _state = State.InProcess;
            // TODO: Remove hardcode!
            _animator.Play("OpenMouth", -1, 1f);

            hook.Throw(Vector2.up, (hookedObject, hookState) =>
            {
                switch (hookState)
                {
                    case ThrowingHook.HookState.Catched:
                        BackForthMove target = hookedObject.GetComponent<BackForthMove>();
                        if (target)
                            target.moveState = BackForthMove.MoveState.Stop;
                        break;
                    case ThrowingHook.HookState.Hooked:
                        _state = State.Stop;
                        _animator.Play("CloseMouth");
                        Destroy(hookedObject);
                        break;
                    default:
                        break;
                }
            });
        }
    }
}

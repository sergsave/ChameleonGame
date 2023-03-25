using UnityEngine;

public class ChameleonController : MonoBehaviour
{
    public bool isControllable = false;

    // TODO: broadcast?
    public System.Action targetCatchedCallback;
    public System.Action missedCallback;

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
        if (!isControllable || _state != State.Stop || !Input.GetMouseButtonDown(0))
            return;

        _state = State.InProcess;
        // TODO: Remove hardcode!
        _animator.Play("OpenMouth", -1, 1f);

        hook.Throw(Vector2.up, (hookedObject, hookState) =>
        {
            switch (hookState)
            {
                case ThrowingHook.HookEvent.Catched:
                    BackForthMove target = hookedObject.GetComponent<BackForthMove>();
                    if (target)
                        target.moveState = BackForthMove.MoveState.Stop;
                    break;
                case ThrowingHook.HookEvent.Finished:
                    _state = State.Stop;
                    _animator.Play("CloseMouth");

                    Destroy(hookedObject);

                    if (hookedObject != null && targetCatchedCallback != null)
                        targetCatchedCallback();
                    break;
                case ThrowingHook.HookEvent.Missed:
                    if (missedCallback != null)
                        missedCallback();
                    break;
                default:
                    break;
            }
        });

    }
}

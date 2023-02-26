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
        // Может не в этом скрипте??
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_state == State.Stop && Input.GetMouseButtonDown(0))
        {
            _state = State.InProcess;
            _animator.Play("OpenMouth", -1, 1f); // TODO: Это какой-то подбор таймингов, исправить

            hook.Throw(Vector2.up, () =>
            {
                _state = State.Stop;
                _animator.Play("CloseMouth");
            });
        }
    }
}

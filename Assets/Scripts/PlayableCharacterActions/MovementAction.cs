using UnityEngine;
using UnityEngine.InputSystem;

public class MovementAction : PlayableCharacterAction
{
    [SerializeField] private Animator animator;

    private InputAction _dorsoVentralMovementAction;
    private float _inputValue;
    private Transform _transform;
    private CharacterController _characterController;

    private Stat _speedStat;

    private readonly int _speedAnimatorHash = Animator.StringToHash(StringConsts.Speed);
    private float _verticalSpeed;

    protected void Start()
    {
        _speedStat = statsHolder.GetActualStat(StatType.Speed);

        _characterController = GetComponent<CharacterController>();
        _transform = GetComponent<Transform>();

        _dorsoVentralMovementAction = inputActions.Movement.DorsoVentralMovement;
    }

    private void Update()
    {
        _inputValue = _dorsoVentralMovementAction.ReadValue<float>();
    }

    private void FixedUpdate()
    {
        var deltaTime = Time.fixedDeltaTime;

        animator.SetFloat(_speedAnimatorHash, _inputValue);

        if (!_characterController.isGrounded)
            _verticalSpeed += 9.8f * deltaTime;
        else
            _verticalSpeed = 0f;

        var delta = (_transform.forward * (_inputValue * _speedStat.value) + Vector3.down * _verticalSpeed) * deltaTime;

        _characterController.Move(delta);
    }
}
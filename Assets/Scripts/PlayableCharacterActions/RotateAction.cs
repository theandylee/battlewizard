using UnityEngine;
using UnityEngine.InputSystem;

public class RotateAction : PlayableCharacterAction
{
    [SerializeField] private float speed;
    
    private Transform _transform;

    private InputAction _rotationAction;
    private float _inputValue;

    protected void Start()
    {
        _transform = transform;
        
        _rotationAction = inputActions.Movement.Rotation;
    }
    private void Update()
    {
        _inputValue = _rotationAction.ReadValue<float>();
    }

    private void FixedUpdate()
    {
        var rotationDelta = Quaternion.Euler(0f, _inputValue * speed * Time.fixedDeltaTime, 0f);
        _transform.rotation = rotationDelta * _transform.rotation;
    }
}

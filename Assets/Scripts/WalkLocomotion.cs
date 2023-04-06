using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WalkLocomotion : MonoBehaviour
{
    // the walking action
    [SerializeField]
    private InputAction _walkAction;

    // the walking speed
    [SerializeField]
    private float _speed = 0.5f;

    // reference to the rigidbody
    [SerializeField]
    private Rigidbody _rb;

    [SerializeField]
    private Transform _camera;

    private void FixedUpdate()
    {
        // get the joystick input direction
        Vector2 direction = _walkAction.ReadValue<Vector2>();

        // add force in the direction
        _rb.AddForce(
            _camera.forward * direction.y * _speed +
            _camera.right * direction.x * _speed,
            ForceMode.VelocityChange
        );

        // stop the movement if we aren't inputting
        if (direction.magnitude < 0.1f)
            _rb.velocity = Vector3.zero;
    }

    private void OnEnable()
    {
        _walkAction.Enable();
    }

    private void OnDisable()
    {
        _walkAction.Disable();
    }
}

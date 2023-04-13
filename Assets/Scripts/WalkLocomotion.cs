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

        // get the camera direction on the xz plane
        Vector3 cameraDirection = new Vector3(
            _camera.forward.x,
            0,
            _camera.forward.z
        ).normalized;

        // get the camera right on the xz plane
        Vector3 cameraRight = new Vector3(
            _camera.right.x,
            0,
            _camera.right.z
        ).normalized;

        // add force in the direction of input along the camera direction
        _rb.AddForce(
            (cameraDirection * direction.y + cameraRight * direction.x) * _speed,
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

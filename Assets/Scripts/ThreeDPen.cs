using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThreeDPen : MonoBehaviour
{
    [SerializeField] private InputAction _drawAction;

    [SerializeField] private Transform _inkOrigin;

    [SerializeField] private GameObject _inkPrefab;

    [SerializeField] private float _inkSize = 0.01f;

    private Vector3 _lastPosition;

    private LineRenderer _currentInk;

    private void Start()
    {
        _drawAction.performed += ctx => StartDrawing();
        _drawAction.canceled += ctx => EndDrawing();
    }

    private void Update()
    {
        if (_currentInk != null)
        {
            // get the distance we've moved
            float distance = Vector3.Distance(
                _lastPosition, _inkOrigin.position);

            // if we haven't moved enough, don't draw
            if (distance < 0.01f) return;

            // add a new point to the line renderer
            _currentInk.positionCount++;
            _currentInk.SetPosition(
                _currentInk.positionCount - 1,
                _inkOrigin.position
            );

            // update the last position
            _lastPosition = _inkOrigin.position;
        }
    }

    private void StartDrawing()
    {
        if (_currentInk != null) return;
        
        GameObject ink = Instantiate(_inkPrefab, _inkOrigin.position, _inkOrigin.rotation);
        _currentInk = ink.GetComponent<LineRenderer>();
        _currentInk.startWidth = _inkSize;
        _currentInk.endWidth = _inkSize;

        // add the first point to the line renderer
        _currentInk.positionCount++;
        _currentInk.SetPosition(0, _inkOrigin.position);
        _lastPosition = _inkOrigin.position;
    }

    private void EndDrawing()
    {
        _currentInk = null;
    }

    private void OnEnable()
    {
        _drawAction.Enable();
    }

    private void OnDisable()
    {
        _drawAction.Disable();
    }
}

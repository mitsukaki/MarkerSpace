using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grabbing : MonoBehaviour
{
    [SerializeField] private InputAction _grabAction;

    private GameObject _grabbedObject;

    private List<GameObject> _inRange = new List<GameObject>();
    
    private Vector3 _grabbedObjectOffset;

    private void Grab()
    {
        if (_inRange.Count > 0)
        {
            // pick the closest object
            GameObject closest = _inRange[0];
            float closestDistance = Vector3.Distance(transform.position, closest.transform.position);
            for (int i = 1; i < _inRange.Count; i++)
            {
                float distance = Vector3.Distance(transform.position, _inRange[i].transform.position);
                if (distance < closestDistance)
                {
                    closest = _inRange[i];
                    closestDistance = distance;
                }
            }

            // get the target object to move
            _grabbedObject = closest.GetComponent<Grabbable>().target;
            _grabbedObjectOffset = _grabbedObject.transform.position - transform.position;
        }
    }

    private void Release()
    {
        _grabbedObject = null;
    }

    private void Start()
    {
        _grabAction.performed += ctx => Grab();
        _grabAction.canceled += ctx => Release();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Grabbable"))
        {
            _inRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Grabbable"))
        {
            _inRange.Remove(other.gameObject);
        }
    }

    private void Update()
    {
        if (_grabbedObject != null)
            _grabbedObject.transform.position = transform.position + _grabbedObjectOffset;
    }

    private void OnEnable()
    {
        _grabAction.Enable();
    }

    private void OnDisable()
    {
        _grabAction.Disable();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paint : MonoBehaviour
{
    [SerializeField] private Color color;

    private void Start()
    {
        // get the color of the material on this object
        color = GetComponent<Renderer>().material.color;
    }

    // when a pen enters the paint trigger
    private void OnTriggerEnter(Collider other)
    {
        // if the object is a pen
        if (other.tag == "Pen")
        {
            // get the pen script
            Pen pen = other.GetComponent<Pen>();

            pen.SetColor(color);
        }
    }
}

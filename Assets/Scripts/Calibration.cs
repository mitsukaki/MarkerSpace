using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibration : MonoBehaviour
{
    [SerializeField] private GameObject workSpace;

    // things to be enabled based on which hand is used
    [SerializeField] private GameObject[] leftEnables;
    [SerializeField] private GameObject[] rightEnables;

    // On trigger enter only ever called if a hand hits the calibration cube
    private void OnTriggerEnter(Collider other)
    {
        // if the object is the left controller
        if (other.name == "LeftController")
        {
            // enable the left hand objects
            foreach (GameObject obj in leftEnables)
                obj.SetActive(true);
        }
        else if (other.name == "RightController")
        {
            // enable the right hand objects
            foreach (GameObject obj in rightEnables)
                obj.SetActive(true);
        }

        // enable the workspace and disable the calibration object
        workSpace.SetActive(true);
        gameObject.SetActive(false);
    }
}

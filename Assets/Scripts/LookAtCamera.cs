using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{

    // Enumeration defining different modes for the LookAt behavior
    private enum Mode
    {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted,
    }

    // Selected mode for the LookAt behavior
    [SerializeField] private Mode mode;

    // LateUpdate is called once per frame, after Update has finished
    private void LateUpdate()
    {
        // Switch between different LookAt modes based on the selected mode
        switch (mode)
        {
            case Mode.LookAt:
                // Rotate the object to face the camera
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInverted:
                // Rotate the object to face the opposite direction from the camera
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case Mode.CameraForward:
                // Align the object's forward direction with the camera's forward direction
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                // Align the object's forward direction with the opposite of the camera's forward direction
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}

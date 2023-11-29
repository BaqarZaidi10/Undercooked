using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    // Enumeration to define different modes of looking at the camera
    private enum Mode
    {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted,
    }

    [SerializeField] private Mode mode;

    // LateUpdate is called once per frame, after Update
    private void LateUpdate()
    {
        // Switch between different modes of looking at the camera
        switch (mode)
        {
            case Mode.LookAt:
                // Rotate the object to look directly at the camera
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInverted:
                // Rotate the object to look in the opposite direction of the camera
                Vector3 directionFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + directionFromCamera);
                break;
            case Mode.CameraForward:
                // Align the object's forward vector with the camera's forward vector
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                // Align the object's forward vector with the inverted camera's forward vector
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}

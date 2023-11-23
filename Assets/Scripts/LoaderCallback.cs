using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{

    // Flag to track if this is the first Update call
    private bool isFirstUpdate = true;

    // Update is called once per frame
    private void Update()
    {
        // Execute LoaderCallback on the first update frame
        if (isFirstUpdate)
        {
            isFirstUpdate = false;

            // Call LoaderCallback to proceed with loading the target scene
            Loader.LoaderCallback();
        }
    }
}

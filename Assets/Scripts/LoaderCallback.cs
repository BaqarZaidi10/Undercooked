using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A MonoBehaviour for handling the callback to Loader in the first frame of the LoadingScene
public class LoaderCallback : MonoBehaviour
{
    private bool isFirstUpdate = true;

    // Update is called once per frame
    private void Update()
    {
        // Check if this is the first update frame
        if (isFirstUpdate)
        {
            // Set isFirstUpdate to false to ensure LoaderCallback is called only once
            isFirstUpdate = false;
        }

        // Call LoaderCallback to load the target scene
        Loader.LoaderCallback();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    private bool isFirstUpdate = true;

    private void Update() {
        // Check if the first update has occurred
        if (isFirstUpdate) {
            // Load the game scene
            isFirstUpdate = false;
            Loader.LoaderCallback();
        }
    }
}

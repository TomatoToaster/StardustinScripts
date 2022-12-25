using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqualizeOrthographicSize : MonoBehaviour
{
    private Camera cameraRef;

    void Awake()
    {
        cameraRef = gameObject.GetComponent<Camera>();
        float aspectRatio = cameraRef.aspect;

        // Assuming the original orthographicSize is for a landscape view...
        // If the aspectRatio is >=1, we are in a landscape view so do nothing
        if (aspectRatio >= 1) {
            return;
        }

        // Otherwise, we are in a portrait view, so we divide the orthographic
        // size by the aspect ratio (i.e. multiply by its inverse) so that the
        // portrait view's orthographic size (means half the height of camera)
        // is now half the width of what the equivalent landscape orthographic
        // view would cover. This works for all aspect ratios not just 16:9.
        float landscapeOrthoSize = cameraRef.orthographicSize;
        float portraitOrthoSize = landscapeOrthoSize / aspectRatio;
        cameraRef.orthographicSize = portraitOrthoSize;
    }
}

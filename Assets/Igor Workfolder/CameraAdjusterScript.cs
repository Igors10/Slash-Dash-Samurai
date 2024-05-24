using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdjuster : MonoBehaviour
{
    private Camera mainCamera;
    private float originalOrthographicSize;
    private float targetAspect;

    void Start()
    {
        mainCamera = Camera.main;
        originalOrthographicSize = mainCamera.orthographicSize;
        targetAspect = (float)Screen.width / Screen.height;
    }

    void Update()
    {
        float currentAspect = (float)Screen.width / Screen.height;

        if (currentAspect > targetAspect) // Landscape
        {
            mainCamera.orthographicSize = originalOrthographicSize;
        }
        else // Portrait
        {
            float adjustmentFactor = targetAspect / currentAspect;
            mainCamera.orthographicSize = originalOrthographicSize * adjustmentFactor;
        }
    }
}

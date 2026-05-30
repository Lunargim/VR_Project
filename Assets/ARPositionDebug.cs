using System;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPositionsDebug : MonoBehaviour
{
    public ARTrackedImageManager trackedImagesManager;
    
    public Transform xrOrigin;
    public Transform cam;
    private Transform cube;

    public TextMeshProUGUI camPosText;
    public TextMeshProUGUI cubePosText;

    private void Awake()
    {
        trackedImagesManager.trackablesChanged.AddListener(OnChanged);
    }

    private void OnChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        if (args.updated.Count > 0)
        {
            var state = args.updated[0].trackingState;
            if (state == TrackingState.Tracking)
            {
                GameObject go = args.updated[0].gameObject;
                cube = go.transform;
            }   
            else
            {
                cube = null;
            }
        }
    }

    void Update()
    {
        camPosText.text = "Camera position: " + cam.position.ToString("F2");

        if (cube)
        {
            cubePosText.text = "Cube position: " + cube.position.ToString("F2");
        }
        else
        {
            cubePosText.text = "Cube position: N/A";
        }
    }
}
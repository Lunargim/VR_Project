using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AnchorOnPlane : MonoBehaviour
{
    public GameObject anchorPrefab;
    
    private ARRaycastManager _arRaycastManager;
    private ARAnchorManager _anchorManager;
    private ARPlaneManager _planeManager;
    private static List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    private Vector2 _touchPosition;
    private bool _spawnRequested;
    void Start()
    {
        _planeManager = GetComponent<ARPlaneManager>();
        _arRaycastManager = GetComponent<ARRaycastManager>();
        _anchorManager = GetComponent<ARAnchorManager>();
    }

    void Update()
    {
        _spawnRequested = false;

#if UNITY_ANDROID && !UNITY_EDITOR

        if(Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            _touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            _spawnRequested = true;
        }

#else
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                _touchPosition = Mouse.current.position.ReadValue();
                _spawnRequested = true;
            }
        }
#endif

        if (!_spawnRequested)
        {
            return;
        }

        if (_arRaycastManager.Raycast(_touchPosition, _hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = _hits[0].pose;
            ARTrackable trackable = _hits[0].trackable;
            ARPlane hitPlane = trackable.GetComponent<ARPlane>();

            if (hitPlane != null)
            {
                ARAnchor anchor = _anchorManager.AttachAnchor(hitPlane, hitPose);
                if (anchorPrefab != null)
                {
                    Instantiate(anchorPrefab, anchor.transform.position, anchor.transform.rotation, anchor.transform.parent);
                }
            }
            else
            {
                Debug.LogWarning("AnchorOnPlane not found");
            }
            
        }

    }
    
    public void SetPlaneDetectionActive(bool active)
    {
        if (_planeManager != null)
        {
            _planeManager.enabled = active;
        }  
    }
}

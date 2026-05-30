using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARSubsystems;

public class PlaceOnPlane : MonoBehaviour
{
    [Tooltip("Prefab da spawnare sul piano")]
    public GameObject objectToSpawnVertical;
    public GameObject objectToSpawnHorizontal;
    private ARPlaneManager _planeManager;
    
    private ARRaycastManager _arRaycastManager;
    private static List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    private Vector2 _touchPosition;
    private bool _spawnRequested;
    void Start()
    {
        _planeManager = GetComponent<ARPlaneManager>();
        _arRaycastManager = GetComponent<ARRaycastManager>();
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
            var idTrackable = _hits[0].trackableId;
            var plane = _planeManager.GetPlane(idTrackable);
            Pose hitPose = _hits[0].pose;

            if (plane.alignment == PlaneAlignment.Vertical)
            {
                Instantiate(objectToSpawnVertical, hitPose.position, hitPose.rotation);
            }
            else if (plane.alignment == PlaneAlignment.HorizontalDown ||
                     plane.alignment == PlaneAlignment.HorizontalUp)
            {
                Instantiate(objectToSpawnHorizontal, hitPose.position, hitPose.rotation);
            }
           
        }
    }
}

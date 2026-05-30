using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class DoorOpener : MonoBehaviour
{
    [SerializeField] private GameObject[] _doors;
    private float movementDistance = 0.1f;
    [SerializeField] private Button _button;

    public void Awake()
    {
        _button.onClick.AddListener(OpenDoor);
    }

    public void OpenDoor()
    {
        var index = 0;
        foreach (GameObject door in _doors)
        {
            DoorMovement(door, index);
            index++;
        }
    }

    public void DoorMovement(GameObject door, int index)
    {
        if (index == 0)
        {
            door.gameObject.transform.position = new Vector3(0,-movementDistance,0);
        }
        else
        {
            door.gameObject.transform.position = new Vector3(0,movementDistance,0);
        }
    }
    
}

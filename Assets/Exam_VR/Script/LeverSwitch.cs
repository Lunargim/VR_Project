using UnityEngine;
using UnityEngine.Events;

public class LeverSwitch : MonoBehaviour
{
    public float activationAngle = 80f;

    public UnityEvent onActivated;

    private HingeJoint _hinge;
    private bool _activated = false;

    private void Awake()
    {
        _hinge = GetComponent<HingeJoint>();
    }

    private void Update()
    {
        if (_activated) return;

        float angle = Mathf.Abs(_hinge.angle);

        if (angle >= activationAngle)
        {
            _activated = true;
            onActivated?.Invoke();
        }
    }
}
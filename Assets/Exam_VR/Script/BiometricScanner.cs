using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Hands.Gestures;
using Unity.XR.CoreUtils;

public class BiometricScanner : MonoBehaviour
{
    [Header("Hands")]
    public XRHandTrackingEvents leftHand;
    public XRHandTrackingEvents rightHand;

    [Header("XROrigin")]
    public XROrigin xrOrigin;

    [Header("Gesture")]
    public XRHandPose handPose;

    [Header("Prossimita'")]
    public float activationRadius = 0.25f;
    public Transform scannerPoint;

    [Header("Events")]
    public UnityEvent onGestureRecognized;
    
    private bool _ignoreProximity = false;

    private XRHandShape _handShape;
    private bool _alreadyUnlocked = false;

    private void Awake()
    {
        if (handPose != null) _handShape = handPose.handShape;
    }

    private void OnEnable()
    {
        if (leftHand != null) leftHand.jointsUpdated.AddListener(OnJointsUpdated);

        if (rightHand != null) rightHand.jointsUpdated.AddListener(OnJointsUpdated);
    }

    private void OnDisable()
    {
        if (leftHand != null) leftHand.jointsUpdated.RemoveListener(OnJointsUpdated);
        if (rightHand != null) rightHand.jointsUpdated.RemoveListener(OnJointsUpdated);
    }

    private void OnJointsUpdated(XRHandJointsUpdatedEventArgs eventArgs)
    {
        if (_alreadyUnlocked) return;

        bool near = IsHandNear(eventArgs);
        if (!_ignoreProximity && !near) return;

        bool shapeOk = _handShape != null && _handShape.CheckConditions(eventArgs);
        bool poseOk = handPose != null && handPose.CheckConditions(eventArgs);

        if (shapeOk && poseOk)
        {
            Unlock();
        }
    }

    private bool IsHandNear(XRHandJointsUpdatedEventArgs eventArgs)
    {
        var wristJoint = eventArgs.hand.GetJoint(XRHandJointID.Wrist);

        if (wristJoint.TryGetPose(out Pose wristPose))
        {
            Vector3 wristWorld = wristPose.position;
            if (xrOrigin != null)
                wristWorld = xrOrigin.transform.TransformPoint(wristPose.position);

            Vector3 scannerPos = scannerPoint != null ? scannerPoint.position : transform.position;
            float dist = Vector3.Distance(wristWorld, scannerPos);

            return dist <= activationRadius;
        }

        return false;
    }

    private void Unlock()
    {
        _alreadyUnlocked = true;
        onGestureRecognized?.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 c = scannerPoint != null ? scannerPoint.position : transform.position;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(c, activationRadius);
    }
}
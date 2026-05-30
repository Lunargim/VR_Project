using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Hands.Gestures;

public class SimpleGestureDetector : MonoBehaviour
{
    public XRHandTrackingEvents hand;
    public XRHandPose handPose;
    
    private XRHandShape handShape;

    private void Awake()
    {
        handShape = handPose.handShape;
    }

    void OnEnable()
    {
        if (hand != null)
        {
            hand.jointsUpdated.AddListener(OnJointsUpdated);
        }
    }

    void OnDisable()
    {
        if (hand != null)
        {
            hand.jointsUpdated.RemoveListener(OnJointsUpdated);
        }
    }

    private void OnJointsUpdated(XRHandJointsUpdatedEventArgs eventArgs)
    {
        bool gestureDetected = 
            (handShape != null && handShape.CheckConditions(eventArgs) &&
             handPose != null && handPose.CheckConditions(eventArgs));

        if (gestureDetected)
        {
            Debug.Log("Gesture detected: ");
        }
    }
}
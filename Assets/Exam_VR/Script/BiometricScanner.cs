using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Hands.Gestures;
using Unity.XR.CoreUtils; // per XROrigin

/// <summary>
/// Scanner biometrico dell'armadietto (Step 2).
///
/// Riconosce un gesto della mano (es. "pistola") SOLO quando la mano
/// si trova entro un certo raggio dallo scanner.
///
/// IMPORTANTE: le pose dei joint della mano arrivano in coordinate
/// RELATIVE all'XR Origin, non in coordinate mondo. Per questo le
/// trasformiamo nello spazio mondo usando il transform dell'XR Origin
/// prima di misurare la distanza. (Senza questo, la distanza risulta
/// enorme perche' si confronta uno spazio locale con uno mondo.)
/// </summary>
public class BiometricScanner : MonoBehaviour
{
    [Header("Mani da osservare")]
    public XRHandTrackingEvents leftHand;
    public XRHandTrackingEvents rightHand;

    [Header("Riferimento al rig")]
    [Tooltip("L'XR Origin della scena. Serve per convertire le pose " +
             "delle mani in coordinate mondo. Trascina qui l'XR Origin.")]
    public XROrigin xrOrigin;

    [Header("Gesto da riconoscere")]
    public XRHandPose handPose;

    [Header("Prossimita'")]
    public float activationRadius = 0.25f;
    public Transform scannerPoint;

    [Header("Evento")]
    public UnityEvent onGestureRecognized;

    [Header("Debug")]
    public bool verboseLog = true;
    public bool ignoreProximity = false;

    private XRHandShape handShape;
    private bool alreadyUnlocked = false;

    private void Awake()
    {
        if (handPose != null) handShape = handPose.handShape;
        else Debug.LogWarning("[Scanner] handPose non assegnata!");

        if (xrOrigin == null)
        {
            xrOrigin = FindObjectOfType<XROrigin>();
            if (xrOrigin == null)
                Debug.LogWarning("[Scanner] XR Origin non assegnato e non trovato in scena!");
        }
    }

    private void OnEnable()
    {
        if (leftHand != null) leftHand.jointsUpdated.AddListener(OnJointsUpdated);
        else Debug.LogWarning("[Scanner] leftHand non assegnata!");

        if (rightHand != null) rightHand.jointsUpdated.AddListener(OnJointsUpdated);
        else Debug.LogWarning("[Scanner] rightHand non assegnata!");
    }

    private void OnDisable()
    {
        if (leftHand != null) leftHand.jointsUpdated.RemoveListener(OnJointsUpdated);
        if (rightHand != null) rightHand.jointsUpdated.RemoveListener(OnJointsUpdated);
    }

    private void OnJointsUpdated(XRHandJointsUpdatedEventArgs eventArgs)
    {
        if (alreadyUnlocked) return;

        bool near = IsHandNear(eventArgs);
        if (!ignoreProximity && !near) return;

        bool shapeOk = handShape != null && handShape.CheckConditions(eventArgs);
        bool poseOk = handPose != null && handPose.CheckConditions(eventArgs);

        if (verboseLog)
            Debug.Log($"[Scanner] Valuto gesto -> shape:{shapeOk} pose:{poseOk}");

        if (shapeOk && poseOk)
        {
            Debug.Log("[Scanner] Gesto riconosciuto vicino allo scanner!");
            Unlock();
        }
    }

    private bool IsHandNear(XRHandJointsUpdatedEventArgs eventArgs)
    {
        var wristJoint = eventArgs.hand.GetJoint(XRHandJointID.Wrist);

        if (wristJoint.TryGetPose(out Pose wristPose))
        {
            // La posa e' relativa all'XR Origin: convertila in coordinate mondo
            Vector3 wristWorld = wristPose.position;
            if (xrOrigin != null)
                wristWorld = xrOrigin.transform.TransformPoint(wristPose.position);

            Vector3 scannerPos = scannerPoint != null ? scannerPoint.position : transform.position;
            float dist = Vector3.Distance(wristWorld, scannerPos);

            if (verboseLog)
                Debug.Log($"[Scanner] Distanza mano-scanner: {dist:F2} m (raggio {activationRadius})");

            return dist <= activationRadius;
        }

        return false;
    }

    private void Unlock()
    {
        alreadyUnlocked = true;
        Debug.Log("[Scanner] Armadietto sbloccato.");
        onGestureRecognized?.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 c = scannerPoint != null ? scannerPoint.position : transform.position;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(c, activationRadius);
    }
}
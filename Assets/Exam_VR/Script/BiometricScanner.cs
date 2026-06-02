using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Hands.Gestures;

/// <summary>
/// Scanner biometrico dell'armadietto (Step 2).
///
/// Riconosce un gesto della mano (es. "pistola") SOLO quando la mano
/// si trova entro un certo raggio dallo scanner. La prossimita' e'
/// calcolata via codice usando la posizione del polso.
///
/// Riprende il pattern di SimpleGestureDetector: XRHandShape + XRHandPose
/// valutati con CheckConditions su jointsUpdated.
/// </summary>
public class BiometricScanner : MonoBehaviour
{
    [Header("Mani da osservare")]
    [Tooltip("XRHandTrackingEvents della mano sinistra (su Left Hand Tracking).")]
    public XRHandTrackingEvents leftHand;
    [Tooltip("XRHandTrackingEvents della mano destra (su Right Hand Tracking).")]
    public XRHandTrackingEvents rightHand;

    [Header("Gesto da riconoscere")]
    [Tooltip("La Hand Pose da riconoscere (es. PistolPose).")]
    public XRHandPose handPose;

    [Header("Prossimita'")]
    [Tooltip("Raggio entro cui la mano e' considerata 'vicina allo scanner' (in metri).")]
    public float activationRadius = 0.25f;

    [Tooltip("Punto di riferimento dello scanner. Se vuoto, usa la posizione di questo oggetto.")]
    public Transform scannerPoint;

    [Header("Evento")]
    [Tooltip("Invocato quando il gesto e' riconosciuto mentre la mano e' vicina.")]
    public UnityEvent onGestureRecognized;

    [Header("Debug")]
    [Tooltip("Attiva log dettagliati per la messa a punto.")]
    public bool verboseLog = true;

    [Tooltip("Ignora la prossimita': riconosce il gesto a qualsiasi distanza. " +
             "Usalo SOLO per capire se il gesto viene rilevato.")]
    public bool ignoreProximity = false;

    private XRHandShape handShape;
    private bool alreadyUnlocked = false;

    private void Awake()
    {
        if (handPose != null)
            handShape = handPose.handShape;
        else
            Debug.LogWarning("[Scanner] handPose non assegnata!");
    }

    private void OnEnable()
    {
        if (leftHand != null)
            leftHand.jointsUpdated.AddListener(OnJointsUpdated);
        else
            Debug.LogWarning("[Scanner] leftHand non assegnata!");

        if (rightHand != null)
            rightHand.jointsUpdated.AddListener(OnJointsUpdated);
        else
            Debug.LogWarning("[Scanner] rightHand non assegnata!");
    }

    private void OnDisable()
    {
        if (leftHand != null)
            leftHand.jointsUpdated.RemoveListener(OnJointsUpdated);
        if (rightHand != null)
            rightHand.jointsUpdated.RemoveListener(OnJointsUpdated);
    }

    private void OnJointsUpdated(XRHandJointsUpdatedEventArgs eventArgs)
    {
        if (alreadyUnlocked) return;

        // 1) Prossimita'
        bool near = IsHandNear(eventArgs);
        if (!ignoreProximity && !near) return;

        // 2) Gesto
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
            Vector3 scannerPos = scannerPoint != null ? scannerPoint.position : transform.position;
            float dist = Vector3.Distance(wristPose.position, scannerPos);

            if (verboseLog)
                Debug.Log($"[Scanner] jointsUpdated ricevuto. Distanza mano-scanner: {dist:F2} m " +
                          $"(raggio {activationRadius})");

            return dist <= activationRadius;
        }

        if (verboseLog)
            Debug.Log("[Scanner] jointsUpdated ricevuto, ma polso senza posa valida.");

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
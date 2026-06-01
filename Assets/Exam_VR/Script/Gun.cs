using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Sparo")]
    public Transform shootingPoint;         
    public float maxDistance = 100f;
    public LayerMask hitLayers = ~0;
    
    
    public void Fire()
    {
        Transform origin = shootingPoint != null ? shootingPoint : transform;

        Debug.DrawRay(origin.position, origin.forward * maxDistance, Color.red, 0.5f);

        if (Physics.Raycast(origin.position, origin.forward,
                out RaycastHit hit, maxDistance, hitLayers))
        {
            Debug.Log($"[Gun] Colpito: {hit.collider.name}");
            var forceField = hit.collider.GetComponentInParent<ForceField>();
            if (forceField != null) forceField.Hit();
        }
        else
        {
            Debug.Log("[Gun] Sparo a vuoto.");
        }
    }
}
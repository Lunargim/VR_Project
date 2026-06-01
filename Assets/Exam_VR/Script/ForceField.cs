using UnityEngine;
using UnityEngine.Events;

public class ForceField : MonoBehaviour
{
    [Header("Eventi")]
    public UnityEvent onDestroyed;

    private bool alreadyHit = false;
    
    public void Hit()
    {
        if (alreadyHit) return; //evita doppie chiamate
        alreadyHit = true;

        Debug.Log("[ForceField] Colpito! Il campo di forza si disattiva.");
        
        onDestroyed?.Invoke();
        Destroy(gameObject);
    }
}
using UnityEngine;
using UnityEngine.Events;

public class ForceField : MonoBehaviour
{
    [Header("Eventi")]
    public UnityEvent onDestroyed;

    private bool _alreadyHit = false;
    
    public void Hit()
    {
        if (_alreadyHit) return; //evita doppie chiamate
        _alreadyHit = true;

        Debug.Log("[ForceField] Colpito! Il campo di forza si disattiva.");
        
        onDestroyed?.Invoke();
        Destroy(gameObject);
    }
}
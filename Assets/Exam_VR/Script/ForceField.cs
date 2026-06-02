using UnityEngine;
using UnityEngine.Events;

public class ForceField : MonoBehaviour
{
    [Header("Event")]
    public UnityEvent onDestroyed;

    private bool _alreadyHit = false;
    
    public void Hit()
    {
        if (_alreadyHit) return;
        _alreadyHit = true;
        
        onDestroyed?.Invoke();
        Destroy(gameObject);
    }
}
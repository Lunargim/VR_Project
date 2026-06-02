using UnityEngine;
using UnityEngine.Events;

public class RoomEntryTrigger : MonoBehaviour
{
    public Door doorToClose;

    public Behaviour[] movementToDisable;
    public Behaviour[] toEnable;
    
    public UnityEvent onEntered;

    private bool _triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered) return;

        _triggered = true;

        if (doorToClose != null)
            doorToClose.Close();

        if (movementToDisable != null)
        {
            foreach (var behaviour in movementToDisable)
                if (behaviour != null) behaviour.enabled = false;
        }

        if (toEnable != null)
        {
            foreach (var behaviour in toEnable)
                if (behaviour != null) behaviour.enabled = true;
        }

        onEntered?.Invoke();
    }
}

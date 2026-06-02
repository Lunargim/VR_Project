using System.Collections;
using UnityEngine;
public class Door : MonoBehaviour
{
    public float duration = 1.5f;
    
    public Vector3 slideOffset = new Vector3(0f, 3f, 0f);
    private bool _isOpening = false;
    private bool _isOpen = false;
    
    public void Open()
    {
        if (_isOpen || _isOpening) return;
        Debug.Log("[Door] Apertura porta.");
        StartCoroutine(OpenRoutine());
    }

    private IEnumerator OpenRoutine()
    {
        _isOpening = true;

        Vector3 startPos = transform.localPosition;

        Vector3 endPos = startPos + slideOffset;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float smooth = Mathf.SmoothStep(0f, 1f, t);
            
            transform.localPosition = Vector3.Lerp(startPos, endPos, smooth);
            
            yield return null;
        }

        transform.localPosition = endPos;

        _isOpening = false;
        _isOpen = true;
        Debug.Log("[Door] Porta aperta.");
    }
}
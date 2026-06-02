using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Shoot")]
    public Transform muzzle;
    public float maxDistance = 100f;
    public LayerMask hitLayers = 0;
 
    [Header("LineRenderer")]
    public float lineDuration = 0.05f;
 
    private LineRenderer _line;
 
    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _line.enabled = false;          
        _line.positionCount = 2;
        _line.useWorldSpace = true;
    }
    
    public void Fire()
    {
        Transform origin = muzzle != null ? muzzle : transform;
 
        Vector3 start = origin.position;
        Vector3 end;
 
        if (Physics.Raycast(origin.position, origin.forward,
                            out RaycastHit hit, maxDistance, hitLayers))
        {
            end = hit.point;
            var forceField = hit.collider.GetComponentInParent<ForceField>();
            if (forceField != null) forceField.Hit();
        }
        else
        {
            end = origin.position + origin.forward * maxDistance;
        }
        
        StopAllCoroutines();
        StartCoroutine(ShowLine(start, end));
    }
 
    private IEnumerator ShowLine(Vector3 start, Vector3 end)
    {
        _line.SetPosition(0, start);
        _line.SetPosition(1, end);
        _line.enabled = true;
        yield return new WaitForSeconds(lineDuration);
        _line.enabled = false;
    }
}
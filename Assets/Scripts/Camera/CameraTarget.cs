using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothFactor = 0.1f;
    
    void Start()
    {
        offset = target.position - transform.position;

        transform.position = target.localPosition;
    }
    
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, smoothFactor);
    }
}

using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothFactor = 0.1f;
    
    void Start()
    {
        offset = transform.position - target.position;
    }
    
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, smoothFactor);
    }
}

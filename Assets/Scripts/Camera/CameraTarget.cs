using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothFactor = 0.1f;

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + offset, smoothFactor);
    }
}

using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private Vector3 cameraTarget;
    
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothFactor = 0.1f;


    public void SetCameraTarget(Vector3 target)
    {
        cameraTarget = target;
    }
    void LateUpdate()
    {
        Vector3 newPosition = Vector3.Lerp(cameraTarget, cameraTarget + offset, smoothFactor);
        newPosition.z = -10f;

        transform.position = newPosition;
    }
    
}

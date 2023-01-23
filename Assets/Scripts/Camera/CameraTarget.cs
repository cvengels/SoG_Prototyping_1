using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private GameObject cameraTarget;
    
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothFactor = 0.1f;


    public void SetCameraTarget(GameObject target)
    {
        cameraTarget = target;
    }
    void Update()
    {
        Vector3 newPosition = new Vector3(
            cameraTarget.transform.position.x + offset.x,
            cameraTarget.transform.position.y + offset.y,
            cameraTarget.transform.position.z + offset.z
            );
        
        //Vector3.Lerp(cameraTarget.transform.position, cameraTarget.transform.position + offset, smoothFactor);

        transform.position = newPosition;
    }
    
}

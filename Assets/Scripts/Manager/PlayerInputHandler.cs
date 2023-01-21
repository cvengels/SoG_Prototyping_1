using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerMovement movementController;
    private Camera ownCamera;

    private void Awake()
    {
        ownCamera = GetComponent<Camera>();
    }

    
    public void OnGetCharacterToControl(GameObject characterPrefab, SpawnPoint spownPosition)
    {
        GameObject surrogate = Instantiate(characterPrefab, spownPosition.transform.position, Quaternion.identity);
        
        movementController = surrogate.GetComponent<PlayerMovement>();
    }

    
    public void OnMove(InputAction.CallbackContext context)
    {
        movementController?.SetMovement(context.ReadValue<Vector2>());
    }

    
    public void OnAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            movementController?.SetAction(true);
        }

        if (context.canceled)
        {
            movementController?.SetAction(false);
        }
    }

}


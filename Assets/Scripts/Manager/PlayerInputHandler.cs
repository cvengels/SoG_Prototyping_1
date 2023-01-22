using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerMovement movementController;
    private CameraTarget cameraTarget;

    public void OnGetCharacterToControl(GameObject characterPrefab, SpawnPoint spownPosition)
    {
        name = "Surrogate of " + characterPrefab.GetComponent<PlayerIndividualBehavior>().GetPrefabType().ToString();
        GameObject surrogate = Instantiate(characterPrefab, spownPosition.transform.position, Quaternion.identity);
        
        cameraTarget = GetComponent<CameraTarget>();
        cameraTarget.SetCameraTarget(surrogate.transform.Find("CameraTarget").position);
        
        if (GetComponent<PlayerInput>().camera == null)
        {
            GetComponent<PlayerInput>().camera = GetComponent<Camera>();
        }
        
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


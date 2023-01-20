using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerMovement movementController;
    private Camera ownCamera;

    private void Awake()
    {
    GameObject surrogate = Instantiate(
        GameManager.Instance.GetCharacterPrefab(),
        GameManager.Instance.GetSpawnPosition(), 
        Quaternion.identity
        );

        movementController = surrogate.GetComponent<PlayerMovement>();

        ownCamera = GetComponent<Camera>();
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


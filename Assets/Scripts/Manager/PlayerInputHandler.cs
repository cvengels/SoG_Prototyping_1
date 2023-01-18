using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerMovement movementController;

    private void Awake()
    {
    GameObject surrogate = Instantiate(
        GameManager.Instance.GetPrefabFromGM(),
        GameManager.Instance.GetSpawnPosition(), 
        Quaternion.identity
        );

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


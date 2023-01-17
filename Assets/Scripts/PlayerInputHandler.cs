using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{

    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        playerMovement.GetMovement(context.ReadValue<Vector2>());

    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerMovement.GetAction(true);
        }

        if (context.canceled)
        {
            playerMovement.GetAction(false);
        }
    }
}


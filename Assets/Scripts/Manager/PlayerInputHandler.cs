using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private GameObject surrogate;
    private PlayerMovement movementController;

    public void OnGetCharacterToControl(GameObject characterPrefab, SpawnPoint spawnPosition)
    {
        name = "Surrogate of " + characterPrefab.GetComponent<PlayerIndividualBehavior>().GetPrefabType().ToString();
        surrogate = Instantiate(characterPrefab, spawnPosition.transform.position, Quaternion.identity);
        if (surrogate != null)
        {
            print($"Character {name} spawned at {spawnPosition.name}");
        }

        movementController = surrogate.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        transform.position = movementController.transform.position;
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


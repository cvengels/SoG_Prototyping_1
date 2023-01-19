using System;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private CharType characterType;
    private PlayerMovement movementController;
    private CapsuleCollider2D collider;

    private void Awake()
    {
        characterType = GetComponentInParent<PlayerIndividualBehavior>().GetPrefabType();
        movementController = GetComponentInParent<PlayerMovement>();
        print("Interactable Player spawned: " + characterType);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Mouse interactions
        if (characterType == CharType.Mouse)
        {
            // Cheese
            if (col.transform.CompareTag("Cheese"))
            {
                print("Mouse could pick up the cheese!");
                movementController.SetCanPickupCheese();
            }
        }
        
        // Cat interactions
        else if (characterType == CharType.Cat)
        {
            // Cheese
            if (col.transform.CompareTag("Cheese"))
            {
                print("Cat does not want the cheese D:");
            }
            // Mouse
            else if (col.transform.CompareTag("Mouse"))
            {
                print("Cat could catch the mouse! Quick!!!!!!!!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        // Mouse interactions
        if (characterType == CharType.Mouse)
        {
            // Cheese
            if (col.transform.CompareTag("Cheese"))
            {
                print("Bye bye cheese :,)");
                movementController.SetCanNotPickupCheese();
            }
        }
    }
}

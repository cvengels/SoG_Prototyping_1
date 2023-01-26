using System;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private CharType characterType;

    private void Awake()
    {
        characterType = GetComponentInParent<PlayerIndividualBehavior>().GetPrefabType();
        print("Interactable Player: " + characterType);
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        
        // Mouse interactions
        if (characterType == CharType.Mouse)
        {
           
            
            
        }
        
        // Cat interactions
        else if (characterType == CharType.Cat)
        {
            // Touches mouse
            if (col.GetComponentInParent<PlayerIndividualBehavior>().GetPrefabType() == CharType.Mouse)
            {
                // Initiate fight
                print("Jetzt gibt's aufs Maul!");
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        // Mouse interactions
        if (characterType == CharType.Mouse)
        {
           
            
            
        }
        
        // Cat interactions
        else if (characterType == CharType.Cat)
        {
            
            
            
        }
    }
}

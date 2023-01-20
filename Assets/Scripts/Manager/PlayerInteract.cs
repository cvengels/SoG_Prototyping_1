using System;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private CharType characterType;
    private CapsuleCollider2D collider;

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

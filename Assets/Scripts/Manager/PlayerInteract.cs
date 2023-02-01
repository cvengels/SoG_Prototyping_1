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
            PlayerIndividualBehavior playerCollider = col?.GetComponentInParent<PlayerIndividualBehavior>();
            // Touches mouse
            if (playerCollider != null && 
                playerCollider.GetPrefabType() == CharType.Mouse && 
                GameManager.Instance.GetGameState() == GameState.LevelRunning)
            {
                // Initiate fight
                print("Jetzt gibt's aufs Maul!");
                GameEventManager.Instance.GameEvent_OnGameStateChanged(GameState.FightBegin);
                
            }
            
        }
        
        // Everything else (interactive etc ...)
        else
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

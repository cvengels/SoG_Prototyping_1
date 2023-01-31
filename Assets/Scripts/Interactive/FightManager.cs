using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FightManager : MonoBehaviour
{
    public static FightManager Instance;

    [SerializeField] private float timeForMouseToInteract = 10f;
    [SerializeField] private int buttonPressesNeeded = 20;
    
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Fight Manager already spawned!");
            Destroy(this);
        }
        else
        {
            Debug.Log("Fight Manager spawned");
            Instance = this;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Vector2 playerVelocity = GetComponent<Rigidbody2D>().velocity;
        
        playerVelocity = new Vector2(
            Random.Range(-10f, 10f),
            playerVelocity.y
            );

        if (playerVelocity.x < 0.5f)
        {
            playerVelocity = (Vector2.right * Random.Range(-1, 0) + Vector2.up) * 50f ;
        }
    }


    public void SetMovement(Vector2 movementDirection, CharType characterType)
    {
        // Nothing here
    }
    
    public void SetAction(bool contextPerformed, CharType characterType)
    {
        if (contextPerformed)
        {
            print($"{characterType.ToString()} punches really hard!");
        }
    }
}

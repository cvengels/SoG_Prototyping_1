using UnityEngine;

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

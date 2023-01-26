using UnityEngine;

public class FightManager : MonoBehaviour
{
    [SerializeField] private float timeForMouseToInteract = 10f;
    [SerializeField] private int buttonPressesNeeded = 20;
    
    public void SetMovement(Vector2 movementDirection, CharType characterType)
    {
        throw new System.NotImplementedException();
    }
    
    public void SetAction(bool contextPerformed, CharType characterType)
    {
        if (contextPerformed)
        {
            print($"{characterType.ToString()} punches really hard!");
        }
    }
}

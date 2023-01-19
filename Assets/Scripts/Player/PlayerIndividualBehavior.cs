using System;
using UnityEngine;

public class PlayerIndividualBehavior : MonoBehaviour
{
    [SerializeField] private CharType playerPrefabType;
    
    // Mouse interactions / objects
    private bool mouseCanPickSomethingUp;
    private bool mouseHasCheese;
    private bool mouseBringsCheeseHome;

    public Cheese cheeeeeeese;
    
    // Cat interactions / objects
    private bool catCanSetUpTrap;
    private bool catCatchesMouse;

    public CharType GetPrefabType()
    {
        return playerPrefabType;
    }

    public bool HasCheese()
    {
        return mouseHasCheese;
    }

    public void SetCheese()
    {
        if (!mouseHasCheese)
        {
            mouseHasCheese = true;
        }
    }

    public void BringCheeseHome()
    {
        if (mouseHasCheese)
        {
            Destroy(cheeeeeeese);
        }
    }

    public void ThrowCheese()
    {
        if (mouseHasCheese)
        {
            // TODO throw cheese in view direction
            mouseHasCheese = false;
        }
    }
    
}

using System;
using UnityEngine;

public enum FightCondition
{
    Init,
    Begin,
    End,
    MouseWin,
    CatWin
}

public class FightControl : MonoBehaviour
{
    [SerializeField] private float timeForMouseToInteract = 10f;
    [SerializeField] private int buttonPressesNeeded = 20;
    
}

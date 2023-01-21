using System;
using UnityEngine;

public class MouseHole : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        print("Mousehole visited by: " + col.tag);
    }
}

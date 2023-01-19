using System;
using UnityEngine;

public class MouseHole : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        print("Mousehole visited by: " + col.tag);

        if (col.CompareTag("Mouse"))
        {
            if (col.GetComponentInChildren<PlayerIndividualBehavior>().HasCheese())
            {
                col.GetComponentInChildren<PlayerIndividualBehavior>().ThrowCheese();
            }
        }
    }
}

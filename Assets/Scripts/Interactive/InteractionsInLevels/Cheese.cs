using System;
using UnityEngine;

public class Cheese : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //print("Cheese hit " + collision.transform.name);
    }

    public void PickupCheese()
    {
        GetComponent<Rigidbody2D>().simulated = false;
    }
}

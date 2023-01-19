using System;
using UnityEngine;

public class Cheese : MonoBehaviour
{

    private PolygonCollider2D collider;

    private void Awake()
    {
        collider = GetComponent<PolygonCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("Cheese hit " + collision.transform.name);
    }
}

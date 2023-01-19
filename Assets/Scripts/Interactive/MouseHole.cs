using System;
using UnityEngine;

public class MouseHole : MonoBehaviour
{
    private BoxCollider2D collider;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("Mousehole entered by: " + collision.transform.name);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        print("Mousehole was left by: " + other.transform.name);
        other.gameObject.SetActive(false);
    }
}

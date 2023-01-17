using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D playerRB;

    [SerializeField] private bool isOnFloor, isOnWall;
    
    [SerializeField] private int playerIndex;

    [SerializeField] private float horizontal, selectedSpeed;
    [SerializeField][Range(0f, 1f)] private float deadZone = 0.2f;
    [SerializeField][Range(0f, 1f)] private float characterRunThresholdJoystick = 0.75f;
    [SerializeField] private float walkSpeed = 8f;
    [SerializeField] private float runSpeed = 45f;
    [SerializeField][Range(1f, 100f)] private float jumpHeight = 50f;
    [SerializeField][Range(1f, 200f)] private float extraGravity = 100f;
    [SerializeField][Range(0f, 1f)] private float jumpDampening = 0.3f;


    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();

    }
    
    
    private void FixedUpdate()
    {
        playerRB.velocity = new Vector2(selectedSpeed, playerRB.velocity.y);

        if (selectedSpeed > 0f)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else if (selectedSpeed < 0f)
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }

    public void GetContacts(bool isOnFloor, bool isOnWall)
    {
        this.isOnFloor = isOnFloor;
        this.isOnWall = isOnWall;
    }

    public void GetMovement(Vector2 movementVector)
    {
        horizontal = Mathf.Clamp(movementVector.x, -1f, 1f);
        
        if (Mathf.Abs(horizontal) > deadZone)
        {
            selectedSpeed = walkSpeed;
        }
        else
        {
            selectedSpeed = 0f;
        }
        
        if (Mathf.Abs(horizontal) > characterRunThresholdJoystick)
        {
            selectedSpeed = runSpeed;
        }
        selectedSpeed *= horizontal > 0f ? 1 : -1;
    }

    public void GetAction(bool actionTriggered)
    {
        if (actionTriggered && isOnFloor)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpHeight);
        }

        if (!actionTriggered && playerRB.velocity.y > 0f)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, playerRB.velocity.y * jumpDampening);
        }
    }
}

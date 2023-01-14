using System;
using UnityEngine;
using UnityEngine.InputSystem;

// ReSharper disable Unity.PreferAddressByIdToGraphicsParams

public class PlayerMovementTest : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private Animator animator;

    [SerializeField] private Transform[] groundCheck;
    [SerializeField] private Transform[] wallCheck;
    private LayerMask floors, walls;
    
    [SerializeField] private string playerName;

    [SerializeField] private float horizontal, selectedSpeed;
    [SerializeField][Range(0f, 1f)] private float deadZone = 0.2f;
    [SerializeField][Range(0f, 1f)] private float characterRunThresholdJoystick = 0.5f;
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField][Range(0f, 1f)] private float jumpDampening = 0.5f;

    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
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

    private bool IsGrounded()
    {
        foreach (var groundCheckPoint in groundCheck)
        {
            if (Physics2D.OverlapCircle(groundCheckPoint.position, 0.2f, floors))
            {
                return true;
            }
        }
        return false;
    }

    private bool OnWall()
    {
        foreach (var wallCheckPoint in wallCheck)
        {
            if (Physics2D.OverlapCircle(wallCheckPoint.position, 0.2f, walls))
            {
                return true;
            }
        }
        return false;
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = Mathf.Clamp(context.ReadValue<Vector2>().x, -1f, 1f);

        if (Mathf.Abs(horizontal) > deadZone)
        {
            selectedSpeed = walkSpeed;
            animator.SetBool("walk", true);
            animator.SetBool("run", false);
        }
        else
        {
            selectedSpeed = 0f;
            animator.SetBool("walk", false);
            animator.SetBool("run", false);
        }
        
        if (Mathf.Abs(horizontal) > characterRunThresholdJoystick)
        {
            selectedSpeed = runSpeed;
            animator.SetBool("walk", false);
            animator.SetBool("run", true);
        }

        selectedSpeed *= horizontal > 0f ? 1 : -1;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpHeight);
        }

        if (context.canceled && playerRB.velocity.y > 0f)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, playerRB.velocity.y * jumpDampening);
        }
    }
}

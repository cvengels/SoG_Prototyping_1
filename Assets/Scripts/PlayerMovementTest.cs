using System;
using UnityEngine;
using UnityEngine.InputSystem;

// ReSharper disable Unity.PreferAddressByIdToGraphicsParams

public class PlayerMovementTest : MonoBehaviour
{
    private Rigidbody2D playerRB;
    //private Animator animator;

    [SerializeField] private Transform[] groundCheck;
    [SerializeField] private Transform[] wallCheck;
    [SerializeField] private LayerMask floors, walls;
    [SerializeField] private bool isOnFloor, isOnWall;
    
    [SerializeField] private string playerName;

    [SerializeField] private float horizontal, selectedSpeed;
    [SerializeField][Range(0f, 1f)] private float deadZone = 0.2f;
    [SerializeField][Range(0f, 1f)] private float characterRunThresholdJoystick = 0.5f;
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField][Range(1f, 100f)] private float jumpHeight = 5f;
    [SerializeField][Range(1f, 100f)] private float extraGravity = 1f;
    [SerializeField][Range(0f, 1f)] private float jumpDampening = 0.5f;

    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
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

        IsTouchingAnything();
        SetAnimationState();
    }

    private void SetAnimationState()
    {
        /*
        animator.SetBool("walk", false);
        animator.SetBool("run", false);
        animator.SetBool("jump", false);
        animator.SetBool("falling", false);
        animator.SetBool("onWall", false);
        */
        
        if (isOnWall)
        {
            //animator.SetBool("onWall", true);
        }
        
        if (isOnFloor)
        {
            float absSpeed = Mathf.Abs(selectedSpeed);

            if (absSpeed > walkSpeed)
            {
                //animator.SetBool("walk", false);
                //animator.SetBool("run", true);
            }
            else if (absSpeed > 0f && absSpeed < runSpeed)
            {
                //animator.SetBool("walk", true);
                //animator.SetBool("run", false);
            }
            else
            {
                //animator.SetBool("walk", false);
                //animator.SetBool("run", false);
            }

        }

        if (playerRB.velocity.y > 0f)
        {
            //animator.SetBool("jump", true);
            //animator.SetBool("falling", false);
        }
        else if (playerRB.velocity.y < 0f)
        {
            //animator.SetBool("jump", false);
            //animator.SetBool("falling", true);
        }

    }

    private void IsTouchingAnything()
    {
        isOnFloor = false;
        isOnWall = false;
        
        foreach (var groundCheckPoint in groundCheck)
        {
            if (Physics2D.OverlapCircle(groundCheckPoint.position, 0.2f, floors))
            {
                isOnFloor = true;
                break;
            }
            else
            {
                Vector2 vel = playerRB.velocity;
                vel.y -= extraGravity * Time.deltaTime;
                playerRB.velocity = vel;
            }
        }

        foreach (var wallCheckPoint in wallCheck)
        {
            if (Physics2D.OverlapCircle(wallCheckPoint.position, 0.2f, walls))
            {
                isOnWall = true;
                break;
            }
        }
        
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = Mathf.Clamp(context.ReadValue<Vector2>().x, -1f, 1f);

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

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isOnFloor)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpHeight);
        }

        if (context.canceled && playerRB.velocity.y > 0f)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, playerRB.velocity.y * jumpDampening);
        }
    }
}

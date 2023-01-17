using System;
using UnityEngine;
using UnityEngine.InputSystem;

// ReSharper disable Unity.PreferAddressByIdToGraphicsParams

public class PlayerMovementTest : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private Animator animator;
    private AnimationType currentAnimation;

    [SerializeField] private Transform[] groundCheck;
    [SerializeField] private Transform[] wallCheck;
    [SerializeField] private LayerMask floorLayer, wallLayer;
    [SerializeField] private bool isOnFloor, isOnWall;
    
    [SerializeField] private string playerName;
    [SerializeField] private string currentAnimationPlayed;

    [SerializeField] private float horizontal, selectedSpeed;
    [SerializeField][Range(0f, 1f)] private float deadZone = 0.2f;
    [SerializeField][Range(0f, 1f)] private float characterRunThresholdJoystick = 0.75f;
    [SerializeField] private float walkSpeed = 8f;
    [SerializeField] private float runSpeed = 45f;
    [SerializeField][Range(1f, 100f)] private float jumpHeight = 50f;
    [SerializeField][Range(1f, 200f)] private float extraGravity = 100f;
    [SerializeField][Range(0f, 1f)] private float jumpDampening = 0.3f;

    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        ChangeAnimationState(playerName, AnimationType.Idle);
    }
    
    private void Update()
    {
        IsTouchingAnything();
        SetAnimationState();
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

    private void SetAnimationState()
    {
        if (isOnWall && !isOnFloor)
        {
            ChangeAnimationState(playerName, AnimationType.Wall);
            return;
        }
        
        if (isOnFloor)
        {
            float absSpeed = Mathf.Abs(selectedSpeed);

            if (absSpeed > walkSpeed)
            {
                ChangeAnimationState(playerName, AnimationType.Run);
            }
            else if (absSpeed > 0f && absSpeed < runSpeed)
            {
                ChangeAnimationState(playerName, AnimationType.Walk);
            }
            else
            {
                ChangeAnimationState(playerName, AnimationType.Idle);
            }
            return;
        }

        if (playerRB.velocity.y > 0f)
        {
            ChangeAnimationState(playerName, AnimationType.Jump);
        }
        else if (playerRB.velocity.y < 0f)
        {
            ChangeAnimationState(playerName, AnimationType.Fall);
        }

    }

    private void IsTouchingAnything()
    {
        isOnFloor = false;
        isOnWall = false;
        
        foreach (var groundCheckPoint in groundCheck)
        {
            if (Physics2D.OverlapCircle(groundCheckPoint.position, 0.2f, floorLayer))
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
            if (Physics2D.OverlapCircle(wallCheckPoint.position, 0.2f, wallLayer))
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

    private void ChangeAnimationState(CharType pawnName, AnimationType newAnimation)
    {
        if (currentAnimation == newAnimation) return;

        var animationName = AnimationManager.GetAnimationName(pawnName, newAnimation);
        currentAnimationPlayed = animationName;
        
        animator.Play(animationName);

        currentAnimation = newAnimation;
    }
    
    private void ChangeAnimationState(string pawnName, AnimationType newAnimation)
    {
        var getName = (CharType)Enum.Parse(typeof(CharType), pawnName);
        ChangeAnimationState(getName, newAnimation);
    }
}

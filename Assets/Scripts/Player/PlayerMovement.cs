using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Gamepad Settings")]
    [SerializeField] [Range(0f, 1f)] private float deadZone = 0.2f;
    [SerializeField] [Range(0f, 1f)] private float characterRunThresholdJoystick = 0.75f;

    [Header("Horizontal Movement")]
    [SerializeField] private float walkSpeed = 8f;
    [SerializeField] private float runSpeed = 45f;

    [Header("Vertical Movement")] 
    [SerializeField] [Range(1f, 100f)] private float jumpHeight = 50f;
    [SerializeField] [Range(1f, 200f)] private float extraGravity = 100f;
    [SerializeField] [Range(0f, 1f)] private float jumpDampening = 0.3f;

    [Header("Wall Behavior")]
    [SerializeField] [Range(0.1f, 10f)] private float slidingDownWall = 1f;
    [SerializeField] [Range(-90f, 90f)] private float wallJumpAngle = 0f;
    [SerializeField] [Range(1f, 50f)] private float wallJumpForce = 1f;
    
    private Rigidbody2D playerRB;
    private Animator animator;
    private AnimationType currentAnimation;
    private string currentAnimationPlayed;

    [SerializeField] private Transform[] groundCheck;
    [SerializeField] private Transform[] wallCheck;
    [SerializeField] private Transform rotationCenter, wallJumpTarget;
    private LayerMask floorLayer, wallLayer;
    private bool isOnFloor, isOnWall;

    private int playerIndex;
    private PlayerIndividualBehavior individualBehavior;

    private float horizontal, selectedSpeed;



    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        floorLayer = LayerMask.GetMask("Floor");
        wallLayer = LayerMask.GetMask("Wall");
        if (groundCheck.Length == 0 || wallCheck.Length == 0)
        {
            Debug.LogError("Please check floor and wall points on player object.");
        }

        individualBehavior = GetComponent<PlayerIndividualBehavior>();
    }

    private void FixedUpdate()
    {
        IsTouchingAnything();
        RotateWallJumpTarget();

        if (isOnWall && !isOnFloor && playerRB.velocity.y < 0f)
        {
            playerRB.velocity = new Vector2(0f, slidingDownWall * -1);
        }

        playerRB.velocity = new Vector2(selectedSpeed, playerRB.velocity.y);

        if (selectedSpeed > 0f)
        {
            if (transform.localScale.x < 0f)
            {
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            }
        }
        else if (selectedSpeed < 0f)
        {
            if (transform.localScale.x > 0f)
            {
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            }
        }

        SetAnimationState();
    }
    
    
    private void SetAnimationState()
    {
        CharType character = individualBehavior.GetPrefabType();
        
        if (isOnWall && !isOnFloor)
        {
            ChangeAnimationState(character, AnimationType.Wall);
            return;
        }
        
        if (isOnFloor)
        {
            float absSpeed = Mathf.Abs(selectedSpeed);

            if (absSpeed > walkSpeed)
            {
                ChangeAnimationState(character, AnimationType.Run);
            }
            else if (absSpeed > 0f && absSpeed < runSpeed)
            {
                ChangeAnimationState(character, AnimationType.Walk);
            }
            else
            {
                ChangeAnimationState(character, AnimationType.Idle);
            }
            return;
        }

        if (playerRB.velocity.y > 0f)
        {
            ChangeAnimationState(character, AnimationType.Jump);
        }
        else if (playerRB.velocity.y < 0f)
        {
            ChangeAnimationState(character, AnimationType.Fall);
        }
    }

    public void SetPlayerID(int playerID)
    {
        this.playerIndex = playerID;
    }

    public int GetPlayerID()
    {
        return playerIndex;
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

    public void SetMovement(Vector2 movementVector)
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

    public void SetAction(bool actionTriggered)
    {
        // Action button pressed
        if (actionTriggered)
        {
            // on floor
            if (isOnFloor)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpHeight);
            }

            // in air
            else
            {
                // on wall
                if (isOnWall)
                {
                    // Wall jump
                    Vector2 wallJumpDirection = (rotationCenter.transform.position - wallJumpTarget.transform.position).normalized;
                    playerRB.AddForce(wallJumpDirection * wallJumpForce * -1, ForceMode2D.Force);
                }
            }
        }

        // Action button released
        else
        {
            if (playerRB.velocity.y > 0f)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, playerRB.velocity.y * jumpDampening);
            }
        }
    }

    private void RotateWallJumpTarget()
    {
        rotationCenter.eulerAngles = new Vector3(0f, 0f, wallJumpAngle * transform.localScale.x);
    }
    
    private void ChangeAnimationState(CharType characterType, AnimationType newAnimation)
    {
        if (currentAnimation == newAnimation) return;

        var animationName = AnimationManager.GetAnimationName(characterType, newAnimation);
        currentAnimationPlayed = animationName;
        
        animator.Play(animationName);

        currentAnimation = newAnimation;
    }
}

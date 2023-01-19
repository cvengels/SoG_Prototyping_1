using System;
using System.Net;
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
    [SerializeField] [Range(0.5f, 0.9f)] private float moveDragOnFloor = 0.5f;

    [Header("Jumping & V. Movement")] 
    [SerializeField] [Range(1f, 100f)] private float jumpHeight = 50f;
    [SerializeField] [Range(1f, 200f)] private float extraGravity = 100f;
    [SerializeField] [Range(0f, 1f)] private float jumpDampening = 0.3f;
    [SerializeField] [Range(0.5f, 0.9f)] private float moveDragInAir = 0.5f;
    [SerializeField] [Range(0f, 0.2f)] private float coyoteTimer = 0.15f;
    private float coyoteCounter;
    private bool coyoteJumpEnabled;
    [SerializeField] [Range(0f, 1f)] private float jumpBufferTimer = .2f;
    private float jumpBufferCounter;
    private bool jumpBufferEnabled;

    [Header("Wall Behavior")]
    [SerializeField] [Range(0.1f, 10f)] private float slidingDownWall = 2f;
    [SerializeField] [Range(90f, -90f)] private float wallJumpAngle = 20f;
    [SerializeField] [Range(1f, 150f)] private float wallJumpForce = 75f;
    private bool wallJumpEnabled, wallJumpPerforming;

    [Header("Other Options")] 
    [SerializeField] private bool canPickupCheese;

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
        IsTouchingLevelGeometry();
        AllignWallJumpTarget();
        
        // check if coyote jump can be performed
        if (isOnFloor)
        {
            coyoteJumpEnabled = true;
            coyoteCounter = coyoteTimer;
        }
        
        // in air
        else 
        {
            // Coyote jump check
            if (coyoteJumpEnabled)
            {
                coyoteCounter -= Time.deltaTime;
            
                if (coyoteCounter < 0f) 
                {
                    coyoteJumpEnabled = false;
                }
            }
            
            // Jump buffer check
            if (jumpBufferEnabled)
            {
                jumpBufferCounter -= Time.deltaTime;
                
                if (jumpBufferCounter < 0f)
                {
                    jumpBufferEnabled = false;
                }
            }
        }
        
        // If moving towards wall while hanging, slide down slowly
        if (isOnWall && !isOnFloor && playerRB.velocity.y < 0f && (selectedSpeed * transform.localScale.x) > 0f)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, slidingDownWall * -1);
            wallJumpEnabled = true;
        }
        else
        {
            wallJumpEnabled = false;
        }

        // if player moves down after wall jump, deactivate this mode
        if (wallJumpPerforming && playerRB.velocity.y < 1f)
        {
            wallJumpPerforming = false;
        }
        
        // drag while moving
        float dragFactor;
        if (isOnFloor)
        {
            dragFactor = moveDragOnFloor;
        }
        else
        {
            dragFactor = moveDragInAir;
        }
        playerRB.velocity = new Vector2(Mathf.Lerp(selectedSpeed, playerRB.velocity.x, dragFactor), playerRB.velocity.y);

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
        
        if (!isOnFloor && isOnWall && selectedSpeed * transform.localScale.x > 0f)
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
        else if (playerRB.velocity.y < 1f)
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

    private void IsTouchingLevelGeometry()
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

    // Movement
    public void SetMovement(Vector2 movementVector)
    {
        // Horizontal movement
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

    public void SetCanPickupCheese()
    {
        if (!canPickupCheese)
        {
            canPickupCheese = true;
        }
    }
    
    public void SetCanNotPickupCheese()
    {
        if (canPickupCheese)
        {
            canPickupCheese = false;
        }
    }

    public void SetAction(bool actionTriggered)
    {
        // Action button pressed
        if (actionTriggered)
        {

            // PICKUP CHEESE
            if (canPickupCheese)
            {
                canPickupCheese = false;
                print("Cheese is picked up!");
            }
            else
            {

                // +++ JUMPING LOGIC +++
                // On floor
                if (isOnFloor || coyoteJumpEnabled || (jumpBufferEnabled && jumpBufferCounter > 0f))
                {
                    // Disable jump buffs
                    coyoteJumpEnabled = false;
                    jumpBufferEnabled = false;

                    playerRB.velocity = new Vector2(playerRB.velocity.x, jumpHeight);
                }

                // In air
                else
                {
                    // On wall
                    if (isOnWall)
                    {
                        // Wall jump
                        Vector2 wallJumpDirection =
                            (wallJumpTarget.transform.position - rotationCenter.transform.position).normalized;
                        playerRB.velocity = wallJumpDirection * wallJumpForce;
                    }
                    // Not on wall
                    else
                    {
                        // Reset jump timer
                        if (!jumpBufferEnabled)
                        {
                            jumpBufferEnabled = true;
                            jumpBufferCounter = jumpBufferTimer;
                        }

                        playerRB.velocity = new Vector2(Mathf.Lerp(playerRB.velocity.x, selectedSpeed, moveDragInAir),
                            playerRB.velocity.y);
                    }
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

    private void AllignWallJumpTarget()
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

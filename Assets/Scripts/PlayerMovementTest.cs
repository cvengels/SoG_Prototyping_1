using UnityEngine;
using UnityEngine.Serialization;

// ReSharper disable Unity.PreferAddressByIdToGraphicsParams

public class PlayerMovementTest : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private Animator animator;
    
    [SerializeField] private string playerName;
    
    [SerializeField] private Vector2 movementDirection;
    [SerializeField] private float characterRunThresholdJoystick = 0.5f;
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float jumpHeight = 5f;
    
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    
    void FixedUpdate()
    {
        if (playerName == "PlayerOne")
        {
            if (false) //TODO add new input system for action button
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpHeight);
            }
            
            playerRB.velocity = new Vector2(movementDirection.x * walkSpeed, playerRB.velocity.y);
        }
        else
        {
            if (false)//TODO add new input system for action button
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpHeight);
            }
        }
        
        if (movementDirection.x < 0f)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else
        {
            transform.localScale = new Vector2(1, 1);
        }

        /*
        // Animations
        if (Mathf.Abs(playerRB.velocity.x) < 0.01f && Mathf.Abs(playerRB.velocity.y) < 0.01f)
        {
            animator.SetBool("walk", false);
            animator.SetBool("jump", false);
        }
        else
        {
            if (Mathf.Abs(playerRB.velocity.y) > 0.05f)
            {
                animator.SetBool("jump", true);
                animator.SetBool("walk", false);
            }
            else
            {
                animator.SetBool("walk", true);
                animator.SetBool("jump", false);
            }
        }
        */


        
        /*int playerNumber = playerName == "PlayerOne" ? 1 : 2;

        // horizontal movement
        horizontalMovement = Input.GetAxis(InputManager.Instance.GetInputForPlayer(playerNumber, "Horizontal"));
        if (horizontalMovement != 0f)
        {
            playerRB.velocity = new Vector2(horizontalMovement * movementSpeed, playerRB.velocity.y);
            if (horizontalMovement < 0f)
            {
                transform.localScale = new Vector2(-1, 1);
            }
            else
            {
                transform.localScale = new Vector2(1, 1);
            }
        }
        
        // action event
        actionButton = Input.GetAxis(InputManager.Instance.GetInputForPlayer(playerNumber, "Action"));
        if (actionButton > 0f)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpHeight);
        }
        
        if (playerName == "PlayerOne")
        {
           // Animations
            if (Mathf.Abs(playerRB.velocity.x) < 0.01f && Mathf.Abs(playerRB.velocity.y) < 0.01f)
            {
                animator.SetBool("catWalk", false);
                animator.SetBool("catJump", false);
            }
            else
            {
                if (Mathf.Abs(playerRB.velocity.y) > 0.05f)
                {
                    animator.SetBool("catJump", true);
                    animator.SetBool("catWalk", false);
                }
                else
                {
                    animator.SetBool("catWalk", true);
                    animator.SetBool("catJump", false);
                }
            }
            
            
        }
        
        else if (playerName == "PlayerTwo")
        {
            //
        }*/
    }
}

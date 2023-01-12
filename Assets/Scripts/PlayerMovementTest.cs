using UnityEngine;
// ReSharper disable Unity.PreferAddressByIdToGraphicsParams

public class PlayerMovementTest : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRB;

    [SerializeField] private Animator animator;

    private float horizontalMovement, verticalMovement, actionButton;
    
    [SerializeField] private string playerName;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpHeight = 5f;
    
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    
    void FixedUpdate()
    {
        int playerNumber = playerName == "PlayerOne" ? 1 : 2;
        
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
        }
    }
}

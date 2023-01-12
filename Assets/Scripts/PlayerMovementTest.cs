using UnityEngine;

public class PlayerMovementTest : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRB;

    [SerializeField] private Animator animator;
    
    [SerializeField] private string playerName;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpHeight = 5f;
    
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (playerName == "PlayerOne")
        {
            // left
            if (Input.GetKey(KeyCode.A))
            {
                playerRB.velocity = new Vector2(-1 * movementSpeed, playerRB.velocity.y);
                transform.localScale = new Vector2(-1, 1);
            }
            // right
            else if (Input.GetKey(KeyCode.D))
            {
                playerRB.velocity = new Vector2(1 * movementSpeed, playerRB.velocity.y);
                transform.localScale = new Vector2(1, 1);
            }

            // jump (TODO change to multi-purpose button)
            if (Input.GetKey(KeyCode.E))
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpHeight);
            }
            
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
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                playerRB.velocity = new Vector2(-1 * movementSpeed, playerRB.velocity.y);
                transform.localScale = new Vector2(-1, 1);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                playerRB.velocity = new Vector2(1 * movementSpeed, playerRB.velocity.y);
                transform.localScale = new Vector2(1, 1);
            }
            
            if (Input.GetKey(KeyCode.Space))
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpHeight);
            }
            
        }
    }
}

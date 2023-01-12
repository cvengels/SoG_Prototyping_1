using UnityEngine;

public class PlayerMovementTest : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private InputManager inputManager;
    
    [SerializeField] private string playerName;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpHeight = 5f;
    
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (playerName == "PlayerOne")
        {
            if (Input.GetKey(KeyCode.A))
            {
                playerRB.velocity = new Vector2(-1 * movementSpeed, playerRB.velocity.y);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                playerRB.velocity = new Vector2(1 * movementSpeed, playerRB.velocity.y);
            }
            
            if (Input.GetKey(KeyCode.E))
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpHeight);
            }
            
        }
        
        else if (playerName == "PlayerTwo")
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                playerRB.velocity = new Vector2(-1 * movementSpeed, playerRB.velocity.y);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                playerRB.velocity = new Vector2(1 * movementSpeed, playerRB.velocity.y);
            }
            
            if (Input.GetKey(KeyCode.Space))
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpHeight);
            }
            
        }
    }
}

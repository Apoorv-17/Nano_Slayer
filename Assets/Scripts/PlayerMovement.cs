using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10.0f;         // movement speed of player
    public float crouchedMoveSpeed = 5.0f;  // movement speed when crouched
    public float jumpHeight = 10.0f;        // jump force of the player

    public Transform groundCheck;           // to check if player's feet is touching ground
    public LayerMask groundLayer;           // what is ground
    public int extraJumps;                  // number of allowed extra jumps

    public Transform cilingCheck;           // to check if player under a ciling

    public Animator animator;               // player animator 

    private Rigidbody2D _rigidBody;         // player rigidbody
    public BoxCollider2D boxColloider;      // player box colloider
    public Transform firePoint;             // bullet fire-point
    public static bool facingRight = true;
    private bool isGrounded;
    private bool isJumping = false;
    private int landed;
    private bool touchingCiling;
    private bool isCrouching = false;
    private int availableJumps;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // reset jumps if player is grounded
        if(isGrounded)
        {
            availableJumps = extraJumps;

            if(landed == 0) {
                landed = 1;
            }
            else {
                isJumping = false;
                animator.SetBool("IsJumping", isJumping); // ends jumping animation
                //Debug.Log("End");
            }
            
        }
            
        // jump
        if(Input.GetButtonDown("Jump") && availableJumps > 0 && !touchingCiling)
        {
            _rigidBody.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
            availableJumps--;

            isJumping = true;
            landed = 0;
            animator.SetBool("IsJumping", true); // triggers jump animation
        }
        else if(Input.GetButtonDown("Jump") && availableJumps == 0 && isGrounded && !touchingCiling)
        {
            _rigidBody.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);

            isJumping = true;
            landed = 0;
            animator.SetBool("IsJumping", true); // triggers jump animation
        }

        // crouch
        if(Input.GetButtonDown("Crouch"))
        {
            isCrouching = true;

            boxColloider.size = new Vector2(0.14f, 0.24f);
            boxColloider.offset = new Vector2(0.03f, -0.12f);

            firePoint.position = new Vector2(firePoint.position.x, firePoint.position.y - 0.13f);
            
            animator.SetBool("IsCrouching", true);
        }
        else if((Input.GetButtonUp("Crouch") && !touchingCiling) || (!Input.GetButton("Crouch") && isCrouching && !touchingCiling))
        {
            isCrouching = false;

            boxColloider.size = new Vector2(0.14f, 0.37f);
            boxColloider.offset = new Vector2(0.03f, -0.055f);

            firePoint.position = new Vector2(firePoint.position.x, firePoint.position.y + 0.13f);

            animator.SetBool("IsCrouching", false);
        }
    }

    void FixedUpdate()
    {
        // check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // check if the player's head is touching ciling
        touchingCiling = Physics2D.OverlapCircle(cilingCheck.position, 0.1f, groundLayer);

        // movement
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        if(!isCrouching)
        {
            _rigidBody.velocity = new Vector2(moveHorizontal * moveSpeed, _rigidBody.velocity.y);
            animator.SetFloat("Speed", Mathf.Abs(moveHorizontal)); // trigger run animation 
        }
        else
        {
            _rigidBody.velocity = new Vector2(moveHorizontal * crouchedMoveSpeed, _rigidBody.velocity.y);
        }
        
        // flipping when necessary
        if(!facingRight && moveHorizontal > 0 || facingRight && moveHorizontal < 0)
            Flip();
    }
    
    // function to flip character
    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}

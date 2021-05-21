using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10.0f;         // movement speed of player
    public float crouchedMoveSpeed = 5.0f;  // movement speed when crouched
    public float jumpHeight = 5.0f;        // jump force of the player
    public float wallJumpDistance = 50f;

    public int extraJumps;                  // number of allowed extra jumps

    [Range (0.0f, 1.0f)]
    public float wallSlidingSpeed;          // wall sliding speed

    public Transform groundCheck;           // to check if player is touching ground
    public Transform cilingCheck;           // to check if player under a ciling

    public Transform frontCheck;            // to check if player is grabbing a wall
    public Transform rearCheck;

    public LayerMask groundLayer;           // what is ground
    public LayerMask wallLayer;             // what is wall

    public Transform firePoint;             // bullet fire-point
    public Transform shotgunPoint;
    public Transform shotgunFirePoint;

    public static bool facingRight = true;

    private float moveHorizontal;           // horizontal movement input

    private Animator animator;              // player animator 
    private Rigidbody2D _rigidBody;         // player rigidbody
    private BoxCollider2D boxColloider;     // player box colloider
    
    private bool isGrounded;
    private bool touchingCiling;
    private bool isTouchingWall;

    private bool wallSliding = false;
    private bool isCrouching = false;

    private int availableJumps;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxColloider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // to make player slide from wall
        if(wallSliding) {
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, Mathf.Clamp(_rigidBody.velocity.y, wallSlidingSpeed, float.MaxValue));
        }

        // reset jumps if player is grounded
        if(isGrounded || wallSliding) {
            availableJumps = extraJumps;
        }
            
        // jump
        if(Input.GetButtonDown("Jump") && availableJumps > 0 && !touchingCiling && !wallSliding) {
            _rigidBody.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
            availableJumps--;

            animator.SetBool("IsJumping", true); // triggers jump animation
        }
        else if(Input.GetButtonDown("Jump") && availableJumps == 0 && isGrounded && !touchingCiling && !wallSliding) {
            _rigidBody.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);

            animator.SetBool("IsJumping", true); // triggers jump animation
        }
        else if(Input.GetButtonDown("Jump") && wallSliding) {
            _rigidBody.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            _rigidBody.transform.Translate(new Vector2(wallJumpDistance, 0) * Time.deltaTime);

            animator.SetBool("IsJumping", true); // triggers jump animation
        }
        if(_rigidBody.velocity.y <= 0.01 && isGrounded ) 
        {
            animator.SetBool("IsJumping", false); // ends jumping animation
        }

        // crouch
        if(Input.GetButtonDown("Crouch") && !wallSliding)
        {
            isCrouching = true;

            boxColloider.size = new Vector2(0.14f, 0.24f);
            boxColloider.offset = new Vector2(0.03f, -0.12f);

            firePoint.position = new Vector2(firePoint.position.x, firePoint.position.y - 0.13f);
            shotgunFirePoint.position = new Vector2(shotgunFirePoint.position.x, shotgunFirePoint.position.y - 0.13f);
            shotgunPoint.position = new Vector2(shotgunPoint.position.x, shotgunPoint.position.y - 0.13f);

            animator.SetBool("IsCrouching", true);
        }
        else if((Input.GetButtonUp("Crouch") && !touchingCiling && isCrouching) || (!Input.GetButton("Crouch") && isCrouching && !touchingCiling))
        {
            isCrouching = false;

            boxColloider.size = new Vector2(0.14f, 0.37f);
            boxColloider.offset = new Vector2(0.03f, -0.055f);

            firePoint.position = new Vector2(firePoint.position.x, firePoint.position.y + 0.13f);
            shotgunFirePoint.position = new Vector2(shotgunFirePoint.position.x, shotgunFirePoint.position.y + 0.13f);
            shotgunPoint.position = new Vector2(shotgunPoint.position.x, shotgunPoint.position.y + 0.13f);

            animator.SetBool("IsCrouching", false);
        }
    }

    void FixedUpdate()
    {
        // check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // check if the player's head is touching ciling
        touchingCiling = Physics2D.OverlapCircle(cilingCheck.position, 0.1f, groundLayer);

        // check if the player is grabbing a wall
        if(Physics2D.OverlapCircle(frontCheck.position, 0.1f, wallLayer)) {
            Flip();
            isTouchingWall = true;
        }
        else if(Physics2D.OverlapCircle(rearCheck.position, 0.1f, wallLayer)) {
            isTouchingWall = true;
        }  
        else
            isTouchingWall = false;

        // check if wall sliding
        if(isTouchingWall && !isGrounded) {
            if(!wallSliding) {
                if(facingRight)
                {
                    firePoint.position = new Vector2(firePoint.position.x + 0.03f, firePoint.position.y - 0.025f);
                    shotgunFirePoint.position = new Vector2(shotgunFirePoint.position.x + 0.03f, shotgunFirePoint.position.y - 0.025f); ;
                    shotgunPoint.position = new Vector2(shotgunPoint.position.x + 0.03f, shotgunPoint.position.y - 0.025f);
                }

                if(!facingRight)
                {
                    firePoint.position = new Vector2(firePoint.position.x - 0.03f, firePoint.position.y - 0.025f);
                    shotgunFirePoint.position = new Vector2(shotgunFirePoint.position.x - 0.03f, shotgunFirePoint.position.y - 0.025f);
                    shotgunPoint.position = new Vector2(shotgunPoint.position.x - 0.03f, shotgunPoint.position.y - 0.025f);
                }
            }

            wallSliding = true;

            boxColloider.size = new Vector2(0.17f, 0.32f);
            boxColloider.offset = new Vector2(0.015f, -0.05f);

            animator.SetBool("IsWallGrabbing", true);
            animator.SetBool("IsJumping", false);
        }
        else {
            if(wallSliding) {
                if(facingRight)
                {
                    firePoint.position = new Vector2(firePoint.position.x - 0.03f, firePoint.position.y + 0.025f);
                    shotgunFirePoint.position = new Vector2(shotgunFirePoint.position.x - 0.03f, shotgunFirePoint.position.y + 0.025f);
                    shotgunPoint.position = new Vector2(shotgunPoint.position.x - 0.03f, shotgunPoint.position.y + 0.025f);
                }

                if(!facingRight)
                {
                    firePoint.position = new Vector2(firePoint.position.x + 0.03f, firePoint.position.y + 0.025f);
                    shotgunFirePoint.position = new Vector2(shotgunFirePoint.position.x + 0.03f, shotgunFirePoint.position.y + 0.025f);
                    shotgunPoint.position = new Vector2(shotgunPoint.position.x + 0.03f, shotgunPoint.position.y + 0.025f);
                }
            }

            wallSliding = false;

            boxColloider.size = new Vector2(0.14f, 0.37f);
            boxColloider.offset = new Vector2(0.03f, -0.055f);

            animator.SetBool("IsWallGrabbing", false);

            if(!isGrounded) {
                animator.SetBool("IsJumping", true);
            }
        }
            

        // movement
        moveHorizontal = Input.GetAxisRaw("Horizontal");
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
        if(!facingRight && moveHorizontal > 0 && !wallSliding || facingRight && moveHorizontal < 0 && !wallSliding)
            Flip();
    }
    
    // function to flip character
    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}

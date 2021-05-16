using UnityEngine;

public class JumperAI : MonoBehaviour
{
    public float speed = 15.0f;
    public float jumpForce = 10.0f;
    public float detectionRange = 5.0f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Animator animator;
    
    private Rigidbody2D _rigidBody; // jumper rigid body
    private Transform target;       // target transform
    private bool isNear;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(isNear)
            jump();
        
        if(_rigidBody.velocity.y <= 0) {
            animator.SetBool("IsJumping", false);
        }
            
    }

    void FixedUpdate()
    {
        isNear = Mathf.Abs(_rigidBody.position.x - target.position.x) <= detectionRange;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void jump() 
    {
        if(isGrounded) {
            _rigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            animator.SetBool("IsJumping", true);
        }
            
    }
}

using UnityEngine;

public class JumperAI : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float crouchedMoveSpeed = 5.0f;
    public float speed = 5.0f;
    public float jumpForce = 10.0f;
    public float detectionRange = 1.0f;
    public float timeLeft = 10.0f;

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
        timeLeft -= Time.deltaTime * 10;
        Debug.Log(timeLeft);

        if (timeLeft < 0)
        {
            if (_rigidBody.position.x - target.position.x > 0)
            {
                jumpr();
            }
            else
            {
                jumpl();
            }
        }

        if (_rigidBody.velocity.y < 0)
        {
            timeLeft = 10.0f;
        }
        if (_rigidBody.velocity.y <= 0)
        {

            animator.SetBool("IsJumping", false);
        }

    }

    void FixedUpdate()
    {
        isNear = Mathf.Abs(_rigidBody.position.x - target.position.x) <= detectionRange;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void jumpl()
    {
        if (isGrounded)
        {
            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            _rigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            _rigidBody.velocity = new Vector2(moveHorizontal * moveSpeed, _rigidBody.velocity.y);
            animator.SetBool("IsJumping", true);
        }

    }

    void jumpr()
    {
        if (isGrounded)
        {
            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            _rigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            _rigidBody.velocity = new Vector2(-(moveHorizontal * moveSpeed * 2), _rigidBody.velocity.y);
            animator.SetBool("IsJumping", true);
        }

    }
}

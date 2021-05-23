using UnityEngine;

public class JumperAI : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public float jumpForce = 10.0f;
    public float detectionRange = 1.0f;
    public float jumpDelay = 15.0f;
    public int attackDamage = 15;

    public Transform groundCheck;
    public LayerMask groundLayer;

    public Transform hitCheck;
    public LayerMask playerLayer;

    public static float timeLeft;
    private bool isNear;
    private bool isGrounded;
    private bool canAttack;
    private bool isJumping=false;

    private Rigidbody2D _rigidBody; // jumper rigid body
    private Transform target;       // target transform
    private PolygonCollider2D polyColloider;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();

        target = GameObject.FindGameObjectWithTag("Player").transform;

        animator = GetComponent<Animator>();

        timeLeft = jumpDelay;
    }

    // Update is called once per frame
    void Update()
    {
        // reduce delay time
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime * 10;
            isJumping = false;
        }
        if (timeLeft < 0 && isNear) {
            if(!isJumping)
            {
                isJumping = true;
                SoundManagerScript.PlaySound("jump");
            }
            Jump();
        }

        if (_rigidBody.velocity.y < 0) {
            timeLeft = jumpDelay;
            animator.SetBool("IsJumping", false);
        }

        if (!isGrounded && !canAttack) {
            float movedistance = Mathf.Abs(_rigidBody.position.x - target.position.x);

            if (_rigidBody.position.x - target.position.x > 0)
                _rigidBody.transform.Translate(new Vector2(-movedistance * moveSpeed, 0f) * Time.deltaTime);
            else
                _rigidBody.transform.Translate(new Vector2(movedistance * moveSpeed, 0f) * Time.deltaTime);
        }

        if(canAttack) {
            Attack();
        }

    }

    void FixedUpdate()
    {
        isNear = Mathf.Abs(_rigidBody.position.x - target.position.x) <= detectionRange;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        canAttack = Physics2D.OverlapCircle(hitCheck.position, 0.1f, playerLayer);
    }

    void Jump()
    {
        if (isGrounded)
        {
            _rigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

            animator.SetBool("IsJumping", true);
        }
    }

    void Attack()
    {   
        _rigidBody.AddForce(new Vector2(Random.Range(-1, 1), jumpForce), ForceMode2D.Impulse);

        PlayerHealth player = FindObjectOfType<PlayerHealth>();
        //player.TakeDamage(attackDamage);
    }
}

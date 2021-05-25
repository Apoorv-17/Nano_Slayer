using UnityEngine;

public class CrabAI : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float detectionRange = 3.0f;
    public float attackRange = 1.25f;       
    public float escapeRange = 0.75f;

    public float attackDelay = 1.0f;

    public Transform firePoint;
    public GameObject bulletPrefab;

    private bool facingRight = false;
    private static float timeLeft;

    private float targetDistance;
    private bool inRange;
    private bool inEscapeRange;
    private bool inAttackRange;

    private Rigidbody2D _rigidBody;     // crab rigid body
    private Transform target;           // target transform
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();

        target = GameObject.FindGameObjectWithTag("Player").transform;

        animator = GetComponent<Animator>();

        timeLeft = attackDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (inAttackRange) {
            timeLeft -= Time.deltaTime;

            if (timeLeft <= 0) {        //attack after attackDelay
                timeLeft = attackDelay;
                Shoot();                //shoot the player   
            }
        }

        // move close if in detection range
        if (inRange && !inEscapeRange && !inAttackRange) {

            if (_rigidBody.position.x - target.position.x > 0) {
                _rigidBody.velocity = new Vector2(-moveSpeed, 0f);
            }
            else {
                _rigidBody.velocity = new Vector2(moveSpeed, 0f);
            }
                
        }

        // move away if in escape range
        else if (inEscapeRange && !inAttackRange) {

            if (_rigidBody.position.x - target.position.x > 0) {
                _rigidBody.velocity = new Vector2(moveSpeed, 0f);
            }
            else {
                _rigidBody.velocity = new Vector2(-moveSpeed, 0f);
            }

        }

    }

    void FixedUpdate()
    {
        targetDistance = Mathf.Abs(_rigidBody.position.x - target.position.x);

        inRange = (targetDistance <= detectionRange) && (targetDistance > attackRange);

        inEscapeRange = targetDistance <= escapeRange;

        inAttackRange = (targetDistance <= attackRange) && (targetDistance > escapeRange);

        animator.SetFloat("Speed", Mathf.Abs(_rigidBody.velocity.x));

        // flipping when necessary
        if(!facingRight && _rigidBody.velocity.x > 0 || facingRight && _rigidBody.velocity.x < 0)
            Flip();
        else if(facingRight && _rigidBody.velocity.x == 0 && _rigidBody.position.x - target.position.x > 0 || !facingRight && _rigidBody.velocity.x == 0 && _rigidBody.position.x - target.position.x < 0)
            Flip();
    }

    // function to flip character
    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    // function to shoot
    void Shoot() 
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

}
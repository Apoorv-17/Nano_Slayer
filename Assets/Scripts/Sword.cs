using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Animator animator;
    public int damage = 70;
    public float speed =10;
    private Transform target;
    private Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.position = _rigidbody.position + new Vector2(0.3f, 0);
        animator = GetComponent<Animator>();
        animator.SetTrigger("Sword_Cut");
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    void Update()
    {
        Vector3 temp;
        if(PlayerMovement.facingRight)
        {
            temp = new Vector3(0.2f, 0, 0) + target.position;
        }
        else
        {
            temp = new Vector3(-0.2f, 0, 0) + target.position;
        }
        _rigidbody.position = Vector2.MoveTowards(_rigidbody.position, temp, speed * Time.deltaTime);
    }
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if(enemy !=null)
        {
            enemy.TakeDamage(damage);
        }
    }
    // Update is called once per frame
}

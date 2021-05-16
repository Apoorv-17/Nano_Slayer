using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20.0f;             // bullet travel speed
    public float punchMultiplierX = 1.0f;   // bullet punch multiplier in x axis
    public float punchMultiplierY = 1.0f;   // bullet punch multiplier in y axis
    public int damage = 40;                 // bullet damage amount
    public GameObject impactEffect;         // bullet impact effect
    
    private Rigidbody2D _rigidbody;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = transform.right * speed;  // give bullet const velocity
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // if bullet hits an enemy
        Enemy virus = hitInfo.GetComponent<Enemy>();
        if(virus != null)
        {
            virus.TakeDamage(damage);       // damage enemy
        }

        // if bullet hits a prop
        if(hitInfo.tag == "Prop" || hitInfo.tag == "Enemy")
        {
            Rigidbody2D prop = hitInfo.gameObject.GetComponent<Rigidbody2D>();
            prop.AddForce(new Vector2(_rigidbody.velocity.x * punchMultiplierX, 20f * punchMultiplierY), ForceMode2D.Impulse);
        }

        Destroy(gameObject);                // destroy bullet prefab

        GameObject bulletImpact = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(bulletImpact, 0.4f);
    }
}

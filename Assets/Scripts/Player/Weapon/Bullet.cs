using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20.0f;             // bullet travel speed
    public float punchMultiplierX = 1.0f;   // bullet punch multiplier in x axis
    public float punchMultiplierY = 1.0f;   // bullet punch multiplier in y axis
    public int damage = 40;                 // bullet damage amount
    public GameObject impactEffect;         // bullet impact effect
    
    private Rigidbody2D _rigidbody;
    
    public void FireStart(int i)
    {
        SoundManagerScript.PlaySound("shoot");
        if (i == 1)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.velocity = transform.right * speed;  // give bullet const velocity
        }
        else if (i == 2)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.velocity = transform.right * speed + transform.up * speed;
        }
        else if (i == 3)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.velocity = transform.right * speed + -transform.up * speed;
        }
    }
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // if bullet hits an enemy
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if(enemy != null)
        {
            SoundManagerScript.PlaySound("bulletImpact");
            enemy.TakeDamage(damage);       // damage enemy
        }

        BarrelExplode barrel = hitInfo.GetComponent<BarrelExplode>();
        if(barrel != null)
        {
            barrel.TakeDamage();
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

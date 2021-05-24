using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public int enemyMaxDamage = 100;
    public int playerMaxDamage = 10;

    private Transform explosionPosition;

    // Start is called before the first frame update
    void Start()
    {
        explosionPosition = GetComponent<Transform>();
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null) { 
            enemy.TakeDamage(enemyMaxDamage);       // damage enemy
        }

        BarrelExplode barrel = hitInfo.GetComponent<BarrelExplode>();
        if (barrel != null) {
            barrel.TakeDamage(2);
        }

        PlayerHealth player = hitInfo.GetComponent<PlayerHealth>();
        if (player != null) {
            player.TakeDamage(playerMaxDamage);     // damage player 
        }
    }
}

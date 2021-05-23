using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;              // enemy's hit points
    public int weaponPoints = 500;      // points given for powerup when distroyed
    public GameObject deathEffect;   // death effect

    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        SoundManagerScript.PlaySound("enemyDeath");
        if(!Weapon.powerup1 && !Weapon.powerup2 && !Weapon.powerup3)
        {
            Weapon.powerPoints = Weapon.powerPoints + weaponPoints;
        }
        if(Weapon.powerPoints>1000)
        {
            Weapon.powerPoints = 1000;
        }
        Destroy(gameObject);

        GameObject deathEffectObject = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(deathEffectObject, 0.4f);
    }
}

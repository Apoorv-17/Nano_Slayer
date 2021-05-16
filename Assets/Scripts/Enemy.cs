﻿using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;              // enemy's hit points
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
        Destroy(gameObject);

        GameObject deathEffectObject = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(deathEffectObject, 0.4f);
    }
}
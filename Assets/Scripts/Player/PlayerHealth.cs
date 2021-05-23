using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0) {
            Respawn();
        }
    }

    void Respawn()
    {
        FindObjectOfType<GameManager>().EndGame();
    }
}

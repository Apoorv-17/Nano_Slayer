using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerMovement movement;
    public float restartDelay = 1f;
    bool isGameOver = false;

    public void EndGame ()
    {
        if(isGameOver == false)
        {
            isGameOver = true;

            // disable player mavement
            movement.enabled = false;

            // restart the level
            Invoke("Restart", restartDelay);
        }
    }

    void Restart ()
    {
        // reset scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // remove powerups
        Weapon.powerup1 = false;
        Weapon.powerup2 = false;
        Weapon.powerup3 = false;

        // reset player direction
        PlayerMovement.facingRight = true;
    }
}

using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class BarrelExplode : MonoBehaviour
{
    public int hitPoints = 2;           // barrel hit points
    public int weaponPoints = 500;      // points given for powerup when distroyed
    public GameObject explodeEffect;    // death effect

    public void TakeDamage()
    {
        hitPoints -= 1;

        if(hitPoints == 1) {
            GetComponent<Light2D>().enabled = true;
        }

        if(hitPoints <= 0) {
            Explode();
        }
    }

    void Explode()
    {
        //SoundManagerScript.PlaySound("explode");
        if(!Weapon.powerup1 && !Weapon.powerup2 && !Weapon.powerup3) {
            Weapon.powerPoints = Weapon.powerPoints + weaponPoints;
        }

        if(Weapon.powerPoints > 1000) {
            Weapon.powerPoints = 1000;
        }

        Destroy(gameObject);

        GameObject explodeEffectObject = Instantiate(explodeEffect, transform.position, Quaternion.identity);
        Destroy(explodeEffectObject, 0.4f);
    }
}

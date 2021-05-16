using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;         // weapon fire point
    public GameObject bulletPrefab;     // bullet prefab
    public GameObject swordPrefab;      // sword prefab

    public Animator animator;           // for controlling animation


    void Update()
    {
        if(Input.GetButtonUp("Fire1"))
        {
            Shoot();
            animator.SetTrigger("Shoot");
        }
        else if(Input.GetButtonDown("Fire2"))
        {
            Slash();
        }
    }

    void Shoot ()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
    void Slash()
    {
        var swordobj = Instantiate(swordPrefab, firePoint.position, firePoint.rotation);
        Destroy(swordobj, .35f);
    }
}

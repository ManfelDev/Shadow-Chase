using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bullet;

    private bool isShooting;
    private PlayerManager player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isShooting && player.Ammo > 0)
        {
            isShooting = true;
            Shoot();
            isShooting = false;
        }
    }

    // Shoot bullet 
    void Shoot()
    {
        Instantiate(bullet, firePoint.position, firePoint.rotation);
        player.Ammo--;
    }
}

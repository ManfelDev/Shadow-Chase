using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform  firePoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private AudioClip  shootSound;
    [SerializeField] private AudioClip  blankShootSound;

    private AudioSource   audioSource;
    private float         lastShot;
    private PlayerManager player;
    private WeaponsClass  currentWeapon;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerManager>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check player current weapon
        if (currentWeapon != player.CurrentWeapon)
        {
            currentWeapon = player.CurrentWeapon;
        }
        
        // Check if the player has shot
        if (Input.GetButtonDown("Fire1") && Time.time - lastShot >= currentWeapon.FireRate && player.Ammo > 0)
        {
            Shoot();
            lastShot = Time.time;
        }
        else if (Input.GetButtonDown("Fire1") && Time.time - lastShot >= currentWeapon.FireRate)
        {
            audioSource.clip = blankShootSound;
            audioSource.Play();
            lastShot = Time.time;
        }
    }

    // Shoot bullet 
    void Shoot()
    {
        GameObject newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
        Bullets bulletScript = newBullet.GetComponent<Bullets>();
        if (bulletScript != null)
        {
            bulletScript.Shooter = gameObject;
        }
        player.Ammo--;
        audioSource.clip = shootSound;
        audioSource.Play();
    }
}

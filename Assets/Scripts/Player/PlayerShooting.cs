using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform  firePoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private AudioClip  shootSound;
    [SerializeField] private AudioClip  blankShootSound;

    private float         lastShot;
    private PlayerManager player;
    private WeaponsClass  currentWeapon;

    private AudioSource audioSource { get => FindObjectOfType<SoundManager>().AudioSource; }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerManager>();
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
            audioSource.PlayOneShot(blankShootSound, 1f);
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
        audioSource.PlayOneShot(shootSound, 1f);
    }
}

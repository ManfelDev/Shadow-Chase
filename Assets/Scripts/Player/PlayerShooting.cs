using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform  firePoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject throwableWeapon;

    private float           lastShot;
    private PlayerManager   player;
    private WeaponsClass    currentWeapon;
    private EnemyAlarm      alarm;
    private AudioClip       shootSound;
    private AudioClip       blankShootSound;

    private AudioSource audioSource { get => FindObjectOfType<SoundManager>().AudioSource; }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerManager>();
        alarm = FindObjectOfType<EnemyAlarm>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check player current weapon
        if (currentWeapon != player.CurrentWeapon)
        {
            currentWeapon = player.CurrentWeapon;
        }

        shootSound = player.CurrentWeapon.ShootSound;
        blankShootSound = player.CurrentWeapon.BlankShootSound;
        
        // Check if the player has shot
        if (Input.GetButton("Fire1") && 
            Time.time - lastShot >= currentWeapon.FireRate && 
            player.Ammo > 0 &&
            currentWeapon != WeaponsClass.Punch)
        {
            Shoot();
            lastShot = Time.time;
        }
        else if (Input.GetButton("Fire1") && Time.time - lastShot >= currentWeapon.FireRate)
        {
            audioSource.PlayOneShot(blankShootSound, 1f);
            lastShot = Time.time;
        }

        // Throw weapon
        if (Input.GetButtonDown("Fire2") && 
            currentWeapon != WeaponsClass.Punch)
        {
            ThrowWeapon();
        }

        Debug.Log(currentWeapon.Tag);
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

        // If the weapon is not silenced, trigger the enemy alarm
        if (currentWeapon.IsSilenced == false)
        {
            alarm.SlowTriggerAll();
        }
    }

    void ThrowWeapon()
    {
        GameObject newThrowableWeapon = Instantiate(throwableWeapon, firePoint.position, firePoint.rotation);
        ThrowableWeapon throwableWeaponScript = newThrowableWeapon.GetComponent<ThrowableWeapon>();
        if (throwableWeaponScript != null)
        {
            throwableWeaponScript.spriteRenderer.sprite = currentWeapon.WeaponSprite;
        }

        player.ChangeWeapon(WeaponsClass.Punch);
    }
}

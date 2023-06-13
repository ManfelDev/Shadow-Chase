using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform  firePoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject throwableWeapon;
    [SerializeField] private AudioClip  throwWeapon;

    private float           lastShot;
    private PlayerManager   player;
    private WeaponsClass    currentWeapon;
    private EnemyAlarm      alarm;
    private AudioClip       shootSound;
    private AudioClip       blankShootSound;
    private PlayerMovement  playerMovement;
    private PlayerPunch     playerPunch;

    private AudioSource audioSource { get => FindObjectOfType<SoundManager>().AudioSource; }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerManager>();
        alarm = FindObjectOfType<EnemyAlarm>();
        playerMovement = GetComponent<PlayerMovement>();
        playerPunch = GetComponent<PlayerPunch>();
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
            player.CurrentWeapon.Tag != "Punch")
        {
            Shoot();
            lastShot = Time.time;
        }
        else if (Input.GetButton("Fire1") && 
                 Time.time - lastShot >= currentWeapon.FireRate &&
                 player.CurrentWeapon.Tag != "Punch")
        {
            audioSource.PlayOneShot(blankShootSound, 1f);
            lastShot = Time.time;
        }

        // Throw weapon or punch
        if (Input.GetButtonDown("Fire2") &&
            player.CurrentWeapon.Tag != "Punch")
        {
            ThrowWeapon();
        }
        else if (Input.GetButtonDown("Fire2") &&
            Time.time - lastShot >= currentWeapon.FireRate &&
            player.CurrentWeapon.Tag == "Punch")
        {
            playerMovement.IsPunching();
            Invoke("Punch", 0.6f);
            lastShot = Time.time;
        }
    }

    void Punch()
    {
        playerPunch.Punch();
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
        audioSource.PlayOneShot(throwWeapon, 1f);

        if (throwableWeaponScript != null)
        {
            throwableWeaponScript.spriteRenderer.sprite = currentWeapon.WeaponSprite;
        }

        player.ChangeWeapon(WeaponsClass.Punch);
    }
}

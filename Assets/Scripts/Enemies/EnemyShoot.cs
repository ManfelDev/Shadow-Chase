using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private Transform    firePoint;
    [SerializeField] private GameObject   bullet;
    [SerializeField] private float        detectionRadius;
    [SerializeField] private AudioClip    shootSound;
    [SerializeField] private float        fireRate = 0.2f;

    private Vector2       playerPosition;
    private Vector2       selfPosition;
    private float         lastShot;
    private EnemyAlarm    alarm;
    private PlayerManager playerManager;
    private RaycastHit2D  raycast;

    private AudioSource audioSource { get => FindObjectOfType<SoundManager>().AudioSource; }

    void Start()
    {
        alarm = FindObjectOfType<EnemyAlarm>();
        playerManager = FindObjectOfType<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = GameObject.FindWithTag("Player").transform.position;
        selfPosition = transform.position;

        playerPosition.y += 20f;
        selfPosition.y += 20f;
        Vector2 direction = new Vector2 ((playerPosition.x - selfPosition.x),(playerPosition.y - selfPosition.y));

        raycast = Physics2D.Raycast(firePoint.position, direction, 100f);

        if (DetectPlayer())
        {
            if (CheckRaycastCollision(raycast) == "Ground")
                Debug.DrawRay(firePoint.position, direction, Color.blue);

            else if (CheckRaycastCollision(raycast) == "Player" || CheckRaycastCollision(raycast) == "Box")
                {
                    Debug.DrawRay(firePoint.position, direction, Color.green);
                    if (Time.time - lastShot >= fireRate && alarm.IsON && playerManager.CurrentHealth > 0)
                    {
                        Shoot();
                        lastShot = Time.time;
                    }
                }
        }
    }

    public bool DetectPlayer()
    {
        float playerDistance_x = playerPosition.x - selfPosition.x;
        float playerDistance_y = playerPosition.y - selfPosition.y;

        float playerDistance = (float)(Math.Sqrt(Math.Pow(playerDistance_x, 2) + Math.Pow(playerDistance_y, 2)));

        if(playerDistance > -detectionRadius && playerDistance < detectionRadius)
            return true;
        else
            return false;
    }

    private string CheckRaycastCollision(RaycastHit2D raycast)
    {
        if(raycast.collider != null)
            return raycast.collider.gameObject.tag;
        else return "";
    }

    private void Shoot()
    {
        GameObject newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
        Bullets bulletScript = newBullet.GetComponent<Bullets>();
        if (bulletScript != null)
        {
            bulletScript.Shooter = gameObject;
        }
        audioSource.PlayOneShot(shootSound, 1f);
    }
}

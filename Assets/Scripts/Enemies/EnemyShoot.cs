using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField] private Transform  firePoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float      detectionRadius;
    [SerializeField] private AudioClip  shootSound;

    private AudioSource  audioSource;
    private Vector2 playerPosition;
    private Vector2 selfPosition;
    private float lastShot;
    private float fireRate = 0.15f;
    private FollowPlayer followPlayer;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = shootSound;
        followPlayer = GetComponentInChildren<FollowPlayer>();
        Debug.Log(followPlayer.GetEnemyAlarmed());
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = GameObject.FindWithTag("Player").transform.position;
        selfPosition = transform.position;

        if (DetectPlayer() && Time.time - lastShot >= fireRate && followPlayer.GetEnemyAlarmed())
        {
            Shoot();
            lastShot = Time.time;
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

    private void Shoot()
    {
            Instantiate(bullet, firePoint.position, firePoint.rotation);
            audioSource.Play();
    }
}

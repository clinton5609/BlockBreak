using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private PlayerStats stats;
    AudioManager audioManager;

    private float nextShotTime;
    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        if (!stats)
        {
            stats = GetComponent<PlayerStats>();
        }
    }

    void Update()
    {
        // if player clicks and can shoot 
        if (Input.GetMouseButton(0) && Time.time >= nextShotTime)
        {
            // use player stats to find rate of fire 
            float rate = stats != null ? stats.FireRate : 8f;
            nextShotTime = Time.time + (1f / rate);
            Shoot();
        }
    }

    private void Shoot()
    {
        // create the bullet 
        Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
        audioManager.PlaySFX(audioManager.gunShot);
    }
}

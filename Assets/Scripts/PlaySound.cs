using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    
    public AudioSource shoot;
    public AudioSource bouncing;
    public AudioSource targetHit;
    public AudioSource powerUp;
    public AudioSource moving;
    public AudioSource background;

    private PlayerController playerController;
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        background.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.isMoving)
        {
            if (!moving.isPlaying)
            {
                moving.Play(); // Start playing if the sound is not already playing
            }
        }
        else
        {
            if (moving.isPlaying)
            {
                moving.Stop(); // Stop playing only if it's currently playing
            }
        }
    }

    void OnEnable()
    {
        WeaponController.OnPlayerShoot += HandlePlayerShoot;
        ProjectileController.OnProjectileBouncing += HandleProjectileBouncing;
        ProjectileController.OnEnemyHit += HandleEnemyHit;
        ProjectileController.OnProjectileCollected += HandleProjectileCollection;
    }

    void OnDisable()
    {
        WeaponController.OnPlayerShoot -= HandlePlayerShoot;
        ProjectileController.OnProjectileBouncing -= HandleProjectileBouncing;
        ProjectileController.OnEnemyHit -= HandleEnemyHit;
        ProjectileController.OnProjectileCollected -= HandleProjectileCollection;
    }

    void HandlePlayerShoot()
    {
        shoot.Play();
    }

    void HandleProjectileBouncing()
    {
        bouncing.Play();
    }

    void HandleEnemyHit()
    {
        targetHit.Play();
    }

    void HandleProjectileCollection()
    {
        powerUp.Play();
    }
}

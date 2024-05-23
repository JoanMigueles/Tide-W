using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pirate : Troop
{
    public GameObject cannonballPrefab;
    public Transform cannonballSpawn;

    [SerializeField] private AudioSource audioSource;

    public override void Attack() { 

        if (audioSource != null)
        {
            audioSource.Play();
        }

        // Instantiate cannonball prefab
        Projectile cannonball = Instantiate(cannonballPrefab, cannonballSpawn.position, Quaternion.identity).GetComponent<Projectile>();

        if (cannonball != null)
        {
            // Set the target for the cannonball
            cannonball.damage = damage;
            cannonball.SetTarget(target);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pirate : Troop
{
    public GameObject cannonballPrefab;
    public Transform cannonballSpawn;

    public override void Attack()
    {
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

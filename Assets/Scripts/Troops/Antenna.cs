using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antenna : Structure
{
    public override void Attack()
    {
        foreach (Entity entity in EnemyTroopsWithinRange(attackRange))
        {
            if (entity != null)
            {
                entity.TakeDamage(damage);
            }
        }
    }
}

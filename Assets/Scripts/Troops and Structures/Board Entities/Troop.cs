using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Troop : Entity
{
    public float moveSpeed = 5f;
    private NavMeshAgent agent;

    private float sightRange = 20f;
    private bool attacking = false;
    private float cooldownTimer = 0;

    private SailingBoat enemySailingBoat;
    

    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere to visualize the attack range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Draw a wire sphere to visualize the sight range in the editor
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    protected override void Start()
    {
        base.Start();

        //NavMeshAgent behavior start
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        SailingBoat[] sailingBoats = FindObjectsOfType<SailingBoat>();
        foreach (SailingBoat boat in sailingBoats)
        {
            if (boat.team != team)
            {
                enemySailingBoat = boat;
                target = enemySailingBoat;
            }
        }
    }

    protected virtual void Update()
    {
        if (!attacking || target == null)
        {
            //Not attacking -> Move towards the target
            agent.speed = moveSpeed;
            target = SearchTarget();
            if (target != null) {
                agent.SetDestination(target.transform.position);
            }

        } else {
            //Attacking
            agent.speed = 0;
            transform.LookAt(target.transform);
            if (cooldownTimer <= 0)
            {
                Attack();
                cooldownTimer = 1 / attackSpeed;
            } else {
                if (!TargetInRange(attackRange))
                {
                    attacking = false;
                }
            }
        }
        
        if (cooldownTimer > 0 ) cooldownTimer -= Time.deltaTime;
    }

    protected Entity SearchTarget()
    {
        Entity closestEnemy;

        if (target == null && enemySailingBoat != null)
        {
            attacking = false;
            return enemySailingBoat;
        } else {
            closestEnemy = GetClosestEnemyWithinRange(attackRange);
            if (closestEnemy != null)
            {
                attacking = true;
                return closestEnemy;
            }

            closestEnemy = GetClosestEnemyWithinRange(sightRange);
            if (closestEnemy != null)
            {
                attacking = false;
                return closestEnemy;
            }
            return null;
        }
    }
}

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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        SailingBoat[] sailingBoats = FindObjectsOfType<SailingBoat>();
        foreach (SailingBoat boat in sailingBoats)
        {
            if (boat.team != team)
            {
                enemySailingBoat = boat;
            }
        }

        if (healthBar != null)
        {
            healthBar.maxValue = health;
            healthBar.value = health;
        }
    }

    void Update()
    {
        //Not attacking
        if (!attacking || target == null)
        {
            agent.speed = moveSpeed;
            target = SearchTarget();
            if (target != null)
            {
                agent.SetDestination(target.transform.position);
            }
            return;
        }

        //Attacking
        agent.speed = 0;
        if (cooldownTimer <= 0 && target != null)
        {
            
            Attack();

            if (TargetOutsideOfRange(attackRange))
            {
                attacking = false;
            }

            if (target.IsDead()) 
            {
                Destroy(target.gameObject);
                attacking = false;
                return;
            }

            cooldownTimer = 1 / attackSpeed;
        } else {
            cooldownTimer -= Time.deltaTime;
            if (target == null)
            {
                attacking = false;
            }
        }
    }

    private Entity SearchTarget()
    {
        Entity closestEnemy;

        if (target == null && enemySailingBoat != null)
        {
            attacking = false;
            return enemySailingBoat;
        } else {
            closestEnemy = GetClosestEnemy(attackRange);
            if (closestEnemy != null)
            {
                attacking = true;
                return closestEnemy;
            }

            closestEnemy = GetClosestEnemy(sightRange);
            if (closestEnemy != null)
            {
                attacking = false;
                return closestEnemy;
            }
            return null;
        }
    }
}

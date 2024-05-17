using UnityEngine;

public class Structure : Entity
{
    public Hexagon hexagon;
    protected float cooldownTimer = 0;

    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere to visualize the attack range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void Start()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = health;
            healthBar.value = health;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Not attacking
        if (target == null)
        {
            target = SearchTarget();
            return;
        }

        //Attacking
        if (cooldownTimer <= 0)
        {
            Attack();
            if (target.IsDead())
            {
                Destroy(target.gameObject);
                hexagon.SetAvailability(true);
                return;
            }
            if (TargetOutsideOfRange(attackRange))
            {
                target = null;
            }
            cooldownTimer = 1 / attackSpeed;

        }
        else
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    protected Entity SearchTarget()
    {
        Entity closestEnemy = GetClosestEnemy(attackRange);
        if (closestEnemy != null)
        {
            return closestEnemy;
        }

        return null;
    }
}

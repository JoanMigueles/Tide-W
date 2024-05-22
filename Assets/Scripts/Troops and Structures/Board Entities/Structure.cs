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

    protected virtual void Update()
    {
        if (target == null)
        {
            //Not attacking
            target = SearchTarget();
        } else {
            //Attacking
            if (cooldownTimer <= 0)
            {
                Attack();
                cooldownTimer = 1 / attackSpeed;
            } else {
                 if (!TargetInRange(attackRange)) {
                    target = null;
                 }
            }
        }

        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
    }

    public override void Kill()
    {
        hexagon.SetAvailability(true);
        base.Kill();
    }

    protected Entity SearchTarget()
    {
        Entity closestEnemy = GetClosestEnemyWithinRange(attackRange);
        if (closestEnemy != null)
        {
            return closestEnemy;
        }

        return null;
    }
}

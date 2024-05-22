using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Team
{
    Ally,
    Enemy
}

public abstract class Entity : MonoBehaviour
{
    public int type;

    public Team team = Team.Ally;
    public int health = 100;
    public int damage = 10;
    public float attackRange = 5f;
    public float attackSpeed = 1f;
    public int orbCost = 5;
    public Slider healthBar;

    protected Entity target;

    protected virtual void Start()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = health;
            healthBar.value = health;
        }
    }

    public virtual void Attack()
    {
        if (target != null)
        {
            target.TakeDamage(damage);

            if (target.IsDead())
            {
                target.Kill();
                //hexagon.SetAvailability(true);
            }
        } else {
            Debug.Log("target is null!");
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (healthBar != null)
        {
            healthBar.value -= damage;
        }
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    public virtual void Kill()
    {
        Destroy(gameObject);
    }

    public Entity GetClosestEnemyWithinRange(float range)
    {
        Entity closestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Entity enemy in EnemyTroopsWithinRange(range))
        {
            float distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    public Entity GetClosestEnemy()
    {
        Entity closestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        List<Entity> enemies = new List<Entity>(FindObjectsOfType<Entity>());

        foreach (Entity enemy in enemies)
        {
            if (enemy.team != team)
            {
                float distance = Vector3.Distance(enemy.transform.position, transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestEnemy = enemy;
                }
            }
        }

        return closestEnemy;
    }

    protected List<Entity> EnemyTroopsWithinRange(float range)
    {
        List<Entity> enemies = new List<Entity>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, range, LayerMask.GetMask("BoardEntity"));

        foreach (Collider collider in colliders)
        {
            Entity entity = collider.GetComponent<Entity>();
            if (entity != null && entity.team != team && !entity.IsDead())
            {
                enemies.Add(entity);
            }
        }

        return enemies;
    }

    protected bool TargetInRange(float range)
    {
        return EnemyTroopsWithinRange(range).Contains(target);
    }
}

using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;

    private Entity target;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // Rotate towards the target
            transform.LookAt(target.transform);

            // Move towards the direction it's facing
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            // If close enough to the target, destroy the cannonball
            if (Vector3.Distance(transform.position, target.transform.position) < 0.5f)
            {
                Destroy(gameObject);
                target.TakeDamage(10);
            }
        }
        else
        {
            // If target is lost, destroy the cannonball
            Destroy(gameObject);
        }
    }

    // Set the target for the cannonball
    public void SetTarget(Entity newTarget)
    {
        target = newTarget;
    }
}
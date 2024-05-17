using System.Collections;
using UnityEngine;

public class SailingBoat : Structure
{
    public GameObject cannonballPrefab;
    public Transform cannonballSpawn;
    public float moveSpeed = 5f;

    private bool isMoving = false;

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && !isMoving && !GameManager.instance.IsDragging())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // If the ray hits a collider
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Check if the collider is a hexagon
                if (hit.collider.CompareTag("Hex"))
                {
                    Hexagon targetHexagon = hit.collider.gameObject.GetComponent<Hexagon>();
                    // Check if the target hexagon is a neighbor
                    if (targetHexagon != null && hexagon.IsNeighbor(targetHexagon.gameObject) && targetHexagon.IsFree())
                    {
                        hexagon.SetAvailability(true);
                        targetHexagon.SetAvailability(false);
                        // Move the boat to the center of the target hexagon
                        hexagon = targetHexagon;
                        StartCoroutine(MoveToHexagon(hexagon));

                        // Calculate direction vector to the target hexagon
                        Vector3 direction = (targetHexagon.transform.position - transform.position).normalized;

                        // Rotate the boat to face the direction it's moving
                        transform.rotation = Quaternion.LookRotation(direction);
                    }
                }
            }
        }

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
                return;
            }
            cooldownTimer = 1 / attackSpeed;

            if (TargetOutsideOfRange(attackRange))
            {
                target = null;
            }

        }
        else
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public override void Attack()
    {
        // Instantiate cannonball prefab
        Projectile cannonball = Instantiate(cannonballPrefab, cannonballSpawn.position, Quaternion.identity).GetComponent<Projectile>();

        if (cannonball != null)
        {
            // Set the target for the cannonball
            cannonball.SetTarget(target);
        }
    }

    IEnumerator MoveToHexagon(Hexagon hex)
    {
        isMoving = true;

        // Keep moving towards the target position until reached
        while (Vector3.Distance(transform.position, hex.transform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, hex.transform.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
    }
}
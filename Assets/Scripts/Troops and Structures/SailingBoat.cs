using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailingBoat : Structure
{
    public GameObject cannonballPrefab;
    public Transform cannonballSpawn;
    public float moveSpeed = 5f;
    public float moveTimer = 0f;

    private float deployTimer = 0;
    private float nextEnemyTime = 0;

    private bool isMoving = false;
    [SerializeField] private AudioSource audioSource;

    protected override void Update()
    {
        base.Update();

        if (team == Team.Ally)
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
                        Hexagon targetHexagon = hit.collider.GetComponentInParent<Hexagon>();
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
        } else
        {
            //Enemy behavior
            MoveToSafestHexagon();
            //Deploy troops loop
            if (deployTimer > nextEnemyTime || GameManager.instance.GetEnemyOrbs() == 10)
            {
                if (target == null)
                {
                    //If no enemies close, put random troops
                    GameManager.instance.DeployEnemy();
                } else
                {
                    //If there are enemies close, put troops around you to defend yourself
                    List<Hexagon> closeHexagons = GetHexagonsAroundBoat();

                    while (closeHexagons.Count > 0)
                    {
                        int randomIndex = Random.Range(0, closeHexagons.Count);
                        if (closeHexagons[randomIndex].IsFree())
                        {
                            GameManager.instance.DeployEnemy(closeHexagons[randomIndex]);
                            break;
                        } else
                        {
                            closeHexagons.RemoveAt(randomIndex);
                        }
                    }
                }
                
                deployTimer = 0;
                nextEnemyTime = Random.Range(3, 6);
            }
        }
        
    }


    public override void Attack()
    {
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

    public override void Kill()
    {
        StopAllCoroutines();
        if (team == Team.Enemy)
            GameManager.instance.EndGame(true);
        else
            GameManager.instance.EndGame(false);
        base.Kill();
    }

    public Hexagon GetSafestHexagon()
    {
        if (target != null) { 
            Hexagon safestHexagon = null;
            float maxDistance = 0;

            List<Hexagon> closeHexagons = GetHexagonsAroundBoat();
            foreach (Hexagon hex in closeHexagons)
            {
                float distance = Vector3.Distance(target.transform.position, hex.transform.position);
                if (distance > maxDistance && hex.IsFree())
                {
                    maxDistance = distance;
                    safestHexagon = hex;
                }
            }
            return safestHexagon;
        } else
        {
            if(moveTimer > 1f)
            {
                moveTimer = 0f;
                List<Hexagon> closeHexagons = GetHexagonsAroundBoat();
                closeHexagons.Add(hexagon);
                while (closeHexagons.Count > 0)
                {
                    int randomIndex = Random.Range(0, closeHexagons.Count);
                    if (closeHexagons[randomIndex] == hexagon)
                    {
                        return hexagon;
                    }
                    if (closeHexagons[randomIndex].IsFree())
                    {
                        return closeHexagons[randomIndex];
                    }
                    closeHexagons.RemoveAt(randomIndex);
                }
                return hexagon;
            } else
            {
                moveTimer += Time.deltaTime;
                return hexagon;
            }
            
        }
        
    }

    public List<Hexagon> GetHexagonsAroundBoat()
    {
        List<Hexagon> hexagons = new List<Hexagon>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, 8f, LayerMask.GetMask("UI"));
        foreach (Collider collider in colliders)
        {
            Hexagon hex = collider.GetComponentInParent<Hexagon>();
            if (hex != null && hex != hexagon)
            {
                hexagons.Add(hex);
            }
        }

        return hexagons;
    }

    public void MoveToSafestHexagon()
    {
        Hexagon safeHexagon = GetSafestHexagon();
        if (!isMoving)
        {
            if (safeHexagon != hexagon)
            {
                hexagon.SetAvailability(true);
                safeHexagon.SetAvailability(false);
                hexagon = safeHexagon;
                StartCoroutine(MoveToHexagon(safeHexagon));
                // Calculate direction vector to the target hexagon
                Vector3 direction = (safeHexagon.transform.position - transform.position).normalized;
                // Rotate the boat to face the direction it's moving
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }

    public IEnumerator MoveToHexagon(Hexagon hex)
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
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTroopDeployer : MonoBehaviour
{
    public SailingBoat enemySailingBoat;
    // Public list of troop prefabs defined in the editor
    public List<Entity> troopPrefabs;
    private float timer;

    void Start()
    {
        timer = 0;
    }
    
    void Update()
    {
        if (timer > 3f)
        {
            DeployEnemy();
            timer = 0;
        } else
        {
            timer += Time.deltaTime;
        }
    }

    void DeployEnemy()
    {
        // Get all children hexagons
        List<Hexagon> hexagons = new List<Hexagon>(GetComponentsInChildren<Hexagon>());

        // Get a random hexagon that is free
        Hexagon hexagon = null;
        while (hexagons.Count > 0)
        {
            Hexagon randomHexagon = hexagons[Random.Range(0, hexagons.Count)];
            if (randomHexagon.IsFree())
            {
                hexagon = randomHexagon;
                break;
            }
            else
            {
                hexagons.Remove(randomHexagon);
            }
        }

        if (hexagon == null)
        {
            Debug.LogWarning("No free hexagons available.");
            return;
        }

        // Get a random troop prefab from the list

        float currentOrbs = GameManager.instance.GetEnemyOrbs();
        List<Entity> availableTroops = new List<Entity>(troopPrefabs);
        Entity troopPrefab = null;

        while (availableTroops.Count > 0)
        {
            Entity randomTroopPrefab = availableTroops[Random.Range(0, availableTroops.Count)];
            if (currentOrbs >= randomTroopPrefab.orbCost)
            {
                troopPrefab = randomTroopPrefab;
                break;
            }
            else
            {
                availableTroops.Remove(randomTroopPrefab);
            }
        }

        if (troopPrefab == null)
        {
            Debug.LogWarning("No affordable troop prefabs available.");
            return;
        }

        if (hexagon != null && currentOrbs >= troopPrefab.orbCost && hexagon.IsFree())
        {
            // Instantiate the unit prefab onto the hexagon
            GameManager.instance.SetEnemyOrbs(currentOrbs - troopPrefab.orbCost);

            Entity entity = Instantiate(troopPrefab, hexagon.transform.position, Quaternion.identity);
            entity.team = Team.Enemy;
            entity.transform.rotation = Quaternion.Euler(0, 180, 0);

            // Check if the instantiated entity is a Structure
            if (entity is Structure structure)
            {
                // Set the hexagon property for the structure
                structure.hexagon = hexagon;
                hexagon.SetAvailability(false);
            }
        }
    }
}
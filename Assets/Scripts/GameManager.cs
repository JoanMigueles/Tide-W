using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject[] cards;
    public GameObject[] slots;
    public GameObject winScreen;
    public GameObject winText;

    public GameObject enemyBoard;
    public SailingBoat enemySailingBoat;
    public List<Entity> troopPrefabs;

    private float orbs = 1;
    private float enemyOrbs = 1;
    private bool dragging = false;
    private bool gameEnded = false;
    private string endMessage;

    private Queue<int> queue = new Queue<int>();
    private Queue<int> enemyQueue = new Queue<int>();

    private void Awake()
    {
        instance = this;
        
        /*
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);*/
    }

    private void Start()
    {
        SetInitialDecks();

        gameEnded = false;
        StartCoroutine(GameLoop());
    }

    // This is called from start and will run each phase of the game one after another.
    private IEnumerator GameLoop()
    {
        while (!gameEnded)
        {
            UpdateOrbs();
            yield return null;
        }
    }

    private void UpdateOrbs()
    {
        if (orbs < 10) orbs += Time.deltaTime;
        else orbs = 10f;

        if (enemyOrbs < 10) enemyOrbs += Time.deltaTime;
        else enemyOrbs = 10f;
    }

    public void DeployEnemy()
    {
        // Get all children hexagons
        List<Hexagon> hexagons = new List<Hexagon>(enemyBoard.GetComponentsInChildren<Hexagon>());

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
        List<Entity> availableTroops = new List<Entity>(troopPrefabs);
        Entity troopPrefab = null;

        while (availableTroops.Count > 0)
        {
            Entity randomTroopPrefab = availableTroops[Random.Range(0, availableTroops.Count)];
            if (enemyOrbs >= randomTroopPrefab.orbCost)
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

        if (hexagon != null && enemyOrbs >= troopPrefab.orbCost && hexagon.IsFree())
        {
            // Instantiate the unit prefab onto the hexagon
            enemyOrbs -= troopPrefab.orbCost;

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

    public void DeployEnemy(Hexagon hexagon)
    {
        if (hexagon == null)
        {
            Debug.LogWarning("No free hexagons available.");
            return;
        }

        // Get a random troop prefab from the list
        List<Entity> availableTroops = new List<Entity>(troopPrefabs);
        Entity troopPrefab = null;

        while (availableTroops.Count > 0)
        {
            Entity randomTroopPrefab = availableTroops[Random.Range(0, availableTroops.Count)];
            if (enemyOrbs >= randomTroopPrefab.orbCost)
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

        if (hexagon != null && enemyOrbs >= troopPrefab.orbCost && hexagon.IsFree())
        {
            // Instantiate the unit prefab onto the hexagon
            enemyOrbs -= troopPrefab.orbCost;

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

    //Orbs
    public void SetOrbs(float o)
    {
        orbs = o;
    }

    public float GetOrbs()
    {
        return orbs;
    }

    public void SetEnemyOrbs(float o)
    {
        enemyOrbs = o;
    }

    public float GetEnemyOrbs()
    {
        return enemyOrbs;
    }

    //Dragging
    public void SetDragging(bool drag)
    {
        dragging = drag;
    }

    public bool IsDragging()
    {
        return dragging;
    }

    //Deck
    public void SetInitialDecks()
    {
        foreach (GameObject card in cards)
        {
            queue.Enqueue(card.GetComponent<CardDeployer>().troopPrefab.type);
            enemyQueue.Enqueue(card.GetComponent<CardDeployer>().troopPrefab.type);
        }

        foreach (GameObject slot in slots)
        {
            int type = queue.Dequeue();

            GameObject card = Instantiate(cards[type], slot.transform);
            RectTransform rectTransform = card.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                // Set the anchors to stretch the UI element fully
                rectTransform.anchorMin = new Vector2(0, 0); // Bottom-left corner
                rectTransform.anchorMax = new Vector2(1, 1); // Top-right corner

                // Set the offsets to zero
                rectTransform.offsetMin = Vector2.zero; // Left and bottom offsets
                rectTransform.offsetMax = Vector2.zero; // Right and top offsets
            }
            queue.Enqueue(type);
        }
        //PrintQueue();
    }

    public void RestockCard(int type)
    {
        foreach (GameObject slot in slots)
        {
            if (slot.transform.childCount == 0)
            {
                GameObject card = Instantiate(cards[queue.Dequeue()], slot.transform);
                RectTransform rectTransform = card.GetComponent<RectTransform>();

                if (rectTransform != null)
                {
                    // Set the anchors to stretch the UI element fully
                    rectTransform.anchorMin = new Vector2(0, 0); // Bottom-left corner
                    rectTransform.anchorMax = new Vector2(1, 1); // Top-right corner

                    // Set the offsets to zero
                    rectTransform.offsetMin = Vector2.zero; // Left and bottom offsets
                    rectTransform.offsetMax = Vector2.zero; // Right and top offsets
                }
            }
        }
        queue.Enqueue(type);
        //PrintQueue();
    }

    
    
    public Sprite GetNextCardImage()
    {
        if (queue.Count > 0)
        {
            return cards[queue.Peek()].GetComponent<Image>().sprite;
        }
        return null;
    }

    public void EndGame(bool won)
    {
        winScreen.SetActive(true);
        if (won)
        {
            winText.GetComponent<TextMeshProUGUI>().text = "YOU WON";
        }
        else
        {
            winText.GetComponent<TextMeshProUGUI>().text = "YOU LOST";
        }
        gameEnded = true;
    }
    public void Restart()
    {
        // If there is a game winner, restart the level.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    //Debug
    /*
    private void PrintQueue()
    {
        var message = "{ ";
        int count = 0;
        int total = queue.Count;

        foreach (var t in queue)
        {
            message += t.ToString();
            count++;
            if (count < total)
            {
                message += ", ";
            }
        }

        message += " }";
        Debug.Log(message);
    }
    */
    
}

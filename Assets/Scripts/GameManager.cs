using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private float orbs = 1;
    private float enemyOrbs = 1;
    private bool dragging = false;

    public GameObject[] slots;
    public GameObject[] cards;

    private Queue<int> queue;
    private Queue<int> enemyQueue;

    // Comprobar que solo hay un GameManager.
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        queue = new Queue<int>();
        enemyQueue = new Queue<int>();
    }

    void Update()
    {
        if (orbs < 10) orbs += Time.deltaTime / 0.2f;
        else orbs = 10f;

        if (enemyOrbs < 10) enemyOrbs += Time.deltaTime / 0.2f;
        else enemyOrbs = 10f;
    }

    public void SetOrbs(float orbs)
    {
        this.orbs = orbs;
    }

    public float GetOrbs()
    {
        return orbs;
    }

    public void SetEnemyOrbs(float orbs)
    {
        enemyOrbs = orbs;
    }

    public float GetEnemyOrbs()
    {
        return enemyOrbs;
    }

    public void SetDragging(bool drag)
    {
        dragging = drag;
    }

    public bool IsDragging()
    {
        return dragging;
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
        var message = "{ ";
        foreach (var t in queue)
        {
            message += t.ToString() + ", ";
        }
        message += "}";
        Debug.Log(message);
    }

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


        var message = "{ ";
        foreach (var t in queue)
        {
            message += t.ToString() + ", ";
        }
        message += "}";
        Debug.Log(message);
    }
    
    public Sprite GetNextCardImage()
    {
        if (queue.Count > 0)
        {
            return cards[queue.Peek()].GetComponent<Image>().sprite;
        }
        return null;
    }
}

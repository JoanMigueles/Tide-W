using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private float Orbs = 9;
    private bool dragging = false;

    public GameObject[] slots;
    public GameObject[] cards;

    private Queue<int> queue;

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
    }

    public void SetOrbs(float orbs)
    {
        Orbs = orbs;
    }

    public float GetOrbs()
    {
        return Orbs;
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
    }

    public void SetInitialDeck()
    {
        foreach (GameObject card in cards)
        {
            queue.Enqueue(card.GetComponent<CardDeployer>().troopPrefab.type);
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
    }
    
    public Sprite GetNextCardImage()
    {
        if (queue.Count > 0)
        {
            return cards[queue.Peek()].GetComponent<Image>().sprite;
        }
        return null;
    }

    public void ShuffleDeck()
    {
        for (int i = cards.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            GameObject temp = cards[i];
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
    }
}

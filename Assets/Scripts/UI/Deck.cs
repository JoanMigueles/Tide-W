using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.SetInitialDecks();
    }
}

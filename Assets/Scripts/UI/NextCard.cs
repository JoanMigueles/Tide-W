using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextCard : MonoBehaviour
{
    private Sprite sprite;
    // Start is called before the first frame update
    void Start()
    {
        sprite = null;
    }

    // Update is called once per frame
    void Update()
    {
        Sprite nextSprite = GameManager.instance.GetNextCardImage();

        if (nextSprite == null) return;
        if (sprite != nextSprite)
        {
            sprite = nextSprite;
            GetComponent<Image>().sprite = sprite;
        }
    }
}

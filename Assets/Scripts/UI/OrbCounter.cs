using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrbCounter : MonoBehaviour
{
    public Slider slider;  // Reference to the slider
    private TextMeshProUGUI orbCounter;
    private int lastIntegerValue = 0;

    void Start()
    {
        orbCounter = GetComponent<TextMeshProUGUI>();

        if (orbCounter == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on this GameObject.");
            return;
        }

        orbCounter.text = lastIntegerValue.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        int integerValue = (int) GameManager.instance.GetOrbs();

        if (integerValue != lastIntegerValue)
        {
            lastIntegerValue = integerValue;
        }

        orbCounter.text = integerValue.ToString();
    }
}

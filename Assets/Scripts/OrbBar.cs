using System;
using UnityEngine;
using UnityEngine.UI;

public class OrbBar : MonoBehaviour
{
    public Slider slider;  // Reference to the slider

    void Start()
    {
        if (slider == null)
        {
            Debug.LogError("Slider is not assigned!");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        
        slider.value = GameManager.instance.GetOrbs() + Time.deltaTime / 1.5f;
        GameManager.instance.SetOrbs(slider.value);
    }
}

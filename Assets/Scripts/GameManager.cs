using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private bool dragging = false;
    public void setDragging(bool drag)
    {
        dragging = drag;

    }
    public bool isDragging()
    {
        return dragging;
    }

    // Comprobar que solo hay un GameManager.
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

}


using System;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public event Action<KeyCode> OnInputDown = delegate { };

    private readonly List<KeyCode> enableKeys = new List<KeyCode>();

    public bool IsInputEnable = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Update()
    {
        if (!IsInputEnable)
        {
            return;
        }
        else if(enableKeys.Count > 0)
        {
            
        }
        OnKeyDown();
    }

    public void EnableKey(KeyCode keycode)
    {
        enableKeys.Add(keycode);
    }
    
    private void OnKeyDown()
    {
        
    }
}

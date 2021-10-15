using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadUI : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Level_1");
            PlayerManager.Instance.RestartValue();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
        
        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("MainMenu 1");
        }
    }
}

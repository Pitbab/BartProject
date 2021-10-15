using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    [SerializeField] private Text TextFinalTime;
    private void Start()
    {
        TextFinalTime.text = PlayerManager.Instance.GetFinalTime;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void BackTOMain()
    {
        SceneManager.LoadScene("MainMenu 1");
    }
}

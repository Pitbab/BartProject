using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadUI : MonoBehaviour
{

    [SerializeField] private AudioClip SelectSound;

    private AudioSource Source;

    private void Start()
    {
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Source = GetComponent<AudioSource>();
    }

    public void TryAgain()
    {
        Source.PlayOneShot(SelectSound);
        Transition.Instance.ChangeScene("Presentation");
    }

    public void BackToMain()
    {
        Source.PlayOneShot(SelectSound);
        Transition.Instance.ChangeScene("MainMenu 1");
    }
}

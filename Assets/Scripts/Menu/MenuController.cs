using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    [SerializeField] private AudioClip SelectSound;
    [SerializeField] private AudioClip QuitSound;
    [SerializeField] private AudioClip StartSound;
    [SerializeField] private AudioSource Source;
    [SerializeField] private Image Blocker;
    

    //test for highlight
    [SerializeField] private TMP_FontAsset font1;
    [SerializeField] private TMP_FontAsset font2;
    [SerializeField] private Button _button;
    

    private Action LoadLevel;
    private Action QuitApp;
    

    private void Start()
    {
        LoadLevel = PlayGame;
        QuitApp = QuitGame;
    }

    public void PlayGame()
    {
        if (PlayerPrefs.HasKey("x"))
        {
            PlayerPrefs.DeleteAll();
        }
        SceneManager.LoadScene("Presentation");
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    public void Quit()
    {
        Source.PlayOneShot(QuitSound);
        StartCoroutine(Transition(QuitSound.length, QuitApp));
    }

    public void StartGame()
    {
        Source.PlayOneShot(StartSound);
        StartCoroutine(Transition(StartSound.length, LoadLevel));
    }

    public void playSelectSound()
    {
        Source.PlayOneShot(SelectSound);
    }

    private IEnumerator Transition(float transitionTime, Action load)
    {
        Blocker.raycastTarget = true;

        float timer = 0f;
        while(timer < transitionTime)
        {
            timer += Time.deltaTime;
            float lerpValue = timer / transitionTime;

            Blocker.color = Color.Lerp(Color.clear, Color.black, lerpValue);
            yield return null;

        }
        
        load?.Invoke();
        
    }

    private void OnDestroy()
    {
        LoadLevel -= PlayGame;
        QuitApp -= QuitGame;
    }
}

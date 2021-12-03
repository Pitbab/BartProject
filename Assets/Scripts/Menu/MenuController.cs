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

    
    //TO REMOVE (ONLY TO DELETE PLAYER PREFS)
    private void Start()
    {
        //PlayerPrefs.DeleteKey("TUTORIAL");
        //PlayerPrefs.Save();
    }

    public void Quit()
    {
        Source.PlayOneShot(QuitSound);
        StartCoroutine(QuitTransition(QuitSound.length));
    }

    public void StartGame()
    {
        Source.PlayOneShot(StartSound);
        
        if (PlayerPrefs.HasKey("TUTORIAL"))
        {
            if (PlayerPrefs.GetInt("TUTORIAL") == 1)
            {
                Transition.Instance.ChangeScene("Presentation");
            }
            else
            {
                Transition.Instance.ChangeScene("Tutorial");
            }
        }
        else
        {
            PlayerPrefs.SetInt("TUTORIAL", 0);
            PlayerPrefs.Save();
            Transition.Instance.ChangeScene("Tutorial");
        }

        
    }

    //when player want to replay tutorial
    public void ForcePlayTutorial()
    {
        Transition.Instance.ChangeScene("Tutorial");
    }

    public void ToggleSkipTutorial(Toggle toggle)
    {
        if (toggle.isOn)
        {
            PlayerPrefs.SetInt("TUTORIAL", 1);
        }
        else
        {
            PlayerPrefs.SetInt("TUTORIAL", 0);
        }
        
        PlayerPrefs.Save();

    }

    public void playSelectSound()
    {
        Source.PlayOneShot(SelectSound);
    }

    private IEnumerator QuitTransition(float transitionTime)
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
        
        Application.Quit();
        
    }
}

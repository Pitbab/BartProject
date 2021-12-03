using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    [SerializeField] private float TransitionTime = 1f;
    [SerializeField] private Image Blocker;
    [SerializeField] private float ExtraWaitingTime = 2f;
    [SerializeField] private Image LoadingSymbol;

    private AsyncOperation _loadSceneOperation;

    private static Transition instance;
    public static Transition Instance { get { return instance; }}

    public string PreviousScene;
    
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public void ChangeScene(string sceneName, Action onSceneChange = null)
    {
        PreviousScene = SceneManager.GetActiveScene().name;
        _loadSceneOperation = SceneManager.LoadSceneAsync(sceneName);
        _loadSceneOperation.allowSceneActivation = false;

        if (onSceneChange == null)
        {
            onSceneChange = AllowSceneChange;
        }
        else
        {
            onSceneChange += AllowSceneChange; 
        }


        StartCoroutine(FadeRoutine(onSceneChange));
    }

    private void AllowSceneChange()
    {
        _loadSceneOperation.allowSceneActivation = true;
    }

    private IEnumerator FadeRoutine(Action onSceneChange)
    {
        Blocker.raycastTarget = true;

        float timer = 0f;
        while(timer < TransitionTime)
        {
            timer += Time.deltaTime;
            float lerpValue = timer / TransitionTime;

            Blocker.color = Color.Lerp(Color.clear, Color.black, lerpValue);
            yield return null;

        }

        LoadingSymbol.gameObject.SetActive(true);
        yield return new WaitForSeconds(ExtraWaitingTime);
        onSceneChange?.Invoke();
        LoadingSymbol.gameObject.SetActive(false);

        timer = TransitionTime;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            float lerpValue = timer / TransitionTime;
            Blocker.color = Color.Lerp(Color.clear, Color.black, lerpValue);
            yield return null;
        }

        Blocker.raycastTarget = false;

    }
    
    
}

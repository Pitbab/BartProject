using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler
{
    private Button Tab;
    private Image ButtonImage;
    private Vector3 StartingPos;
    
    private Coroutine CurrentRoutine;
    private const float AnimTime = 0.2f;
    private const float Translation = 40f;

    private OptionMenuController OptionController;

    private void Awake()
    {
        Tab = GetComponent<Button>();
        ButtonImage = GetComponent<Image>();
        StartingPos = Tab.transform.position;

        OptionController = GetComponentInParent<OptionMenuController>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CurrentRoutine != null)
        {
            StopCoroutine(CurrentRoutine);
        }

        CurrentRoutine = StartCoroutine(LeftTranslation());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (OptionController.GetCurrentTab() != this)
        {
            if (CurrentRoutine != null)
            {
                StopCoroutine(CurrentRoutine);
            }
        
            CurrentRoutine = StartCoroutine(RightTranslation());
        }

    }

    public void SetColor(Color color)
    {
        ButtonImage.color = color;
    }

    public void OnSelect(BaseEventData eventData)
    {

    }


    public IEnumerator LeftTranslation()
    {
        float timer = 0f;
        Vector3 curPos = Tab.transform.position;
        Vector3 maxPos = StartingPos + new Vector3(-Translation, 0f, 0f);
        
        while(timer < AnimTime)
        {
            timer += Time.deltaTime;
            float lerpValue = timer / AnimTime;
            Tab.transform.position = Vector3.Lerp(curPos, maxPos, lerpValue);

            yield return null;

        }   
    }
    
    public IEnumerator RightTranslation()
    {
        float timer = 0f;
        Vector3 curPos = Tab.transform.position;

        while(timer < AnimTime)
        {
            timer += Time.deltaTime;
            float lerpValue = timer / AnimTime;
            Tab.transform.position = Vector3.Lerp(curPos, StartingPos, lerpValue);

            yield return null;

        }   
    }
    
}

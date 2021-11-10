using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
 
public class ButtonChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_FontAsset Normal;
    [SerializeField] private TMP_FontAsset HighLight;
    [SerializeField] private TMP_Text Text;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Text.font = HighLight;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Text.font = Normal;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DenseEffect : MonoBehaviour
{
    private Image Background;
    [SerializeField] private float Multiplicator;

    private void Start()
    {
        Background = this.GetComponent<Image>();
        
        Background.color = Color.white * Multiplicator;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricGate : MonoBehaviour
{
    private BoxCollider MainCollider;
    private MeshRenderer MainRenderer;
    [SerializeField] private float IntervalTime = 5.0f;

    private void Start()
    {
        MainCollider = GetComponent<BoxCollider>();
        MainRenderer = GetComponent<MeshRenderer>();
        InvokeRepeating("Switching", 1.0f, IntervalTime);
    }

    private void Switching()
    {
        MainCollider.isTrigger = !MainCollider.isTrigger;
        MainRenderer.enabled = !MainRenderer.enabled;
    }

}

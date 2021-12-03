using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    //ref in gameobject
    [SerializeField] private GameObject Bar;
    [SerializeField] private Light Light;
    [SerializeField] private AudioClip PullDownSound;
    private AudioSource Source;

    //RefToGate
    //[SerializeField] private BigGate Gate;
    [SerializeField] private PowerConsumer Obj;

    private bool IsActivated = false;
    private const float RotAngle = 64f;

    private void Start()
    {
        Source = GetComponent<AudioSource>();
    }

    public void Activate()
    {
        if(!IsActivated)
        {
            Light.color = new Color(0, 1, 0);
            Bar.transform.localEulerAngles = new Vector3(0f, 0f, RotAngle);
            IsActivated = true;
            Source.PlayOneShot(PullDownSound);
            Obj.SwitchPower();
        }

    }

    public bool GetState()
    {
        return IsActivated;
    }
}

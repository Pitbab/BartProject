using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Experimental.GlobalIllumination;

public class BigGate : PowerConsumer
{
    [SerializeField] private List<Lever> Levers;
    [SerializeField] private List<GameObject> Bars;
    //public Action CheckLevers;
    [SerializeField] private AudioClip PowerDownSFX;
    [SerializeField] private Light LightState;

    private void Start()
    {
        //CheckLevers = CheckGate;
    }

    public override void SwitchPower()
    {
        CheckGate();
    }

    private void CheckGate()
    {
        int count = 0;

        foreach(Lever lever in Levers)
        {
            if(lever.GetState())
            {
                count++;
            }
        }

        if(count == Levers.Count)
        {
            Debug.Log("Gate opening all levers are open");
            OpenGate();
        }
        else
        {
            Debug.Log("Not all levers are open");
        }
    }

    private void OpenGate()
    {
        LightState.color = Color.green;
        AudioManager.Instance.PlaySfx(PowerDownSFX, transform.position, 1000f);
        foreach(GameObject bar in Bars)
        {
            bar.SetActive(false);
        }
    }

}

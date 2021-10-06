using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BigGate : MonoBehaviour
{
    [SerializeField] private List<Lever> Levers;
    [SerializeField] private List<GameObject> Bars;
    public Action CheckLevers;

    private void Start()
    {
        CheckLevers = CheckGate;
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
        foreach(GameObject bar in Bars)
        {
            bar.SetActive(false);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    //ref in gameobject
    [SerializeField] private GameObject Bar;
    [SerializeField] private Light Light;

    //RefToGate
    //[SerializeField] private BigGate Gate;
    [SerializeField] private PowerConsumer Obj;

    private bool IsActivated = false;
    private const float RotAngle = 64f;
    
    public void Activate()
    {
        if(!IsActivated)
        {
            Light.color = new Color(0, 1, 0);
            Bar.transform.localEulerAngles = new Vector3(0f, 0f, RotAngle);
            IsActivated = true;
            //Gate.CheckLevers?.Invoke();
            Obj.SwitchPower();
        }

    }

    public bool GetState()
    {
        return IsActivated;
    }
}

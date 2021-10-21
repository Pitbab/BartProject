using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    private Vector3 LastStep;
    private Vector3 PlayerPos;

    public static TutorialController Instance;
    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //test
        LastStep = new Vector3(0, 0, 9);
    }

    public void ChangeLastStep(Vector3 Step)
    {
        LastStep = Step;
    }

    public void PlayerRestart(CharacterController controller)
    {
        BasicMovement Player = PlayerManager.Instance.BasicMovement;
        Player.Velocity = Vector3.zero;
        
        //not working because characterController need to disable it
        controller.enabled = false;
        Player.transform.position = LastStep;
        controller.enabled = true;

    }

}

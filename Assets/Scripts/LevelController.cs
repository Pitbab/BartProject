using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private Vector3 PlayerPos;

    [SerializeField] private Vector3 LastStep;

    private void Start()
    {
        if (PlayerManager.Instance.GetMenuState)
        {
            PlayerManager.Instance.ChangeMenuState();
        }

        PlayerManager.Instance.ChangingScene = false;
        PlayerManager.Instance.RestartValue();
    }

    public void ChangeLastStep(Vector3 Step)
    {
        LastStep = Step;
    }

    public void PlayerRestart(CharacterController controller)
    {

        //reset player pos
        BasicMovement Player = controller.gameObject.GetComponent<BasicMovement>();
        Player.Velocity = Vector3.zero;
        
        //need to use this because cannot override characterController set pos
        controller.enabled = false;
        Player.transform.position = LastStep;
        controller.enabled = true;

        AnimationController Anims = controller.gameObject.GetComponent<AnimationController>();
        Anims.StopFallingAnim();
        Anims.SoftLanding();

        PlayerManager.Instance.GetStam(PlayerManager.Instance.GetMaxStam);

    }
    
    public void SkipTutorial()
    {
        
        if (PlayerPrefs.HasKey("TUTORIAL"))
        {
            PlayerPrefs.SetInt("TUTORIAL", 1);

        }
        else
        {
            PlayerPrefs.SetInt("TUTORIAL", 1);
        }
        
        PlayerPrefs.Save();
        
        PlayerManager.Instance.ChangeMenuState();
        Transition.Instance.ChangeScene("Presentation");

    }
}

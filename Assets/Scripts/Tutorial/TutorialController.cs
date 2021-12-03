using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        LastStep = new Vector3(0, 3, 9);
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

    public void GoToLevel1()
    {
        
        PlayerPrefs.SetInt("TUTORIAL", 1);
        PlayerPrefs.Save();
        Transition.Instance.ChangeScene("Presentation");
        
    }

}

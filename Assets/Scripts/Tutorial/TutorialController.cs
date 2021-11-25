using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    private Vector3 LastStep;
    private Vector3 PlayerPos;
    private List<PickableObject> Pickable = new List<PickableObject>();

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

    public void AddObject(PickableObject Obj)
    {
        Pickable.Add(Obj);
    }

    public void ChangeLastStep(Vector3 Step)
    {
        LastStep = Step;
    }

    public void PlayerRestart(CharacterController controller)
    {
        //restart object if they are unreachable
        foreach (var Obj in Pickable)
        {
            Obj.ResetState();
        }
        
        //reset player pos
        BasicMovement Player = controller.gameObject.GetComponent<BasicMovement>();
        Player.Velocity = Vector3.zero;

        AnimationController Anims = controller.gameObject.GetComponent<AnimationController>();
        Anims.SoftLanding();
        //Anims.StopFallingAnim();



        //need to use this because cannot override characterController set pos
        controller.enabled = false;
        Player.transform.position = LastStep;
        controller.enabled = true;

    }

    public void GoToLevel()
    {
        //transition a faire

        SceneManager.LoadScene("Presentation");
    }

}

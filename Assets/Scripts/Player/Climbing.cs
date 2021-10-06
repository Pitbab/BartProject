using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{
    private LayerMask Climable;
    private LayerMask Ledge;
    private BasicMovement moving;
    private AnimationManager AnimManager;

    [SerializeField] private GameObject LeftHand;
    [SerializeField] private GameObject RightHand;

    private const float GrabRange = 0.5f;
    private const float ClimbingGravity = -8.0f;
    private const float ClimbingYVel = 5.0f;
    private const float ClimbingCost = 5.0f;
    private Vector3 WallDetectionOffset = new Vector3(0.0f, 1.0f, 0.0f);

    private void Start()
    {
        PlayerManager.Instance.RegisterClimbing(this);
        Climable = LayerMask.GetMask("Climable");
        Ledge = LayerMask.GetMask("Ledge");
        moving = GetComponent<BasicMovement>();
        AnimManager = GetComponent<AnimationManager>();

    }

    private void Update()
    {
        WallClimb();
    }

    private void WallClimb()
    {
        if (CheckWallFront() && Input.GetKey(KeyCode.Space) && PlayerManager.Instance.PlayerStam > 0)
        {
            Debug.Log("Climbing");
            moving.Gravity = ClimbingGravity;
            moving.Velocity.y = ClimbingYVel;
            moving.Velocity.x = 0.0f;
            moving.Velocity.z = 0.0f;

            PlayerManager.Instance.DepleteStamina(ClimbingCost);
            AnimManager.PlayClimbingAnim();
            DetetectLedge(); ;
        }
        else
        {
            AnimManager.StopClimbingAnim();
        }
    }

    private void DetetectLedge()
    {
        if(Physics.CheckSphere(LeftHand.transform.position, GrabRange, Ledge) || Physics.CheckSphere(RightHand.transform.position, GrabRange, Ledge))
        {
            AnimManager.PlayClimbingUp();
            AnimManager.StopClimbingAnim();
        }
    }

    private bool CheckWallFront()
    {
        return (Physics.Raycast(transform.position + WallDetectionOffset, transform.forward, GrabRange, Climable));
    }

    //if player animation is climbing this return true
    public bool Climb()
    {
        return AnimManager.GetAnimName() == "Climbing";
    }

}

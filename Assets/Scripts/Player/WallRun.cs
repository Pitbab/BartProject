using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{

    [SerializeField] private float WallRunGravity = -10;
    private bool IsGroundClose = true;
    private bool IsWallRunning;
    private const float TiltValue = 15.0f;
    private const float TiltSpeed = 10.0f;
    private const float WallCheckDist = 2.0f;

    private const float WallRunCost = 10.0f;
    private LayerMask Ground;
    private LayerMask RunnableWall;

    //testing
    private BasicMovement basicMoving;


    private void Start()
    {
        PlayerManager.Instance.RegisterWallRun(this);
        Ground = LayerMask.GetMask("Ground");
        RunnableWall = LayerMask.GetMask("WallRun");
        basicMoving = GetComponent<BasicMovement>();
    }

    private void Update()
    {
        DoWallRun();
    }



    private void DoWallRun()
    {
        Quaternion ZRotation = Quaternion.identity;
        
        if (((CheckWallLeft() && Input.GetKey(KeyCode.A) || (CheckWallRight() && Input.GetKey(KeyCode.D))) && !CheckGroundWall() && !basicMoving.JumpFormWall && PlayerManager.Instance.PlayerStam > 0)) //hold a or d to stick to the wall
        {
            /*
             
             lock on feature to stay on the wall when looking away to jump on another wall
             
              
             */
            IsWallRunning = true;

            //wallRun Gravity
            basicMoving.Gravity = WallRunGravity;

            if (CheckWallRight())
            {
                Debug.Log("WallRun on right");
                ZRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, TiltValue);
            }

            if (CheckWallLeft())
            {
                Debug.Log("WallRun on left");
                ZRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, -TiltValue);
            }

            //give little tilt when wall running
            transform.rotation = Quaternion.Lerp(transform.rotation, ZRotation, Time.deltaTime * TiltSpeed);

            if (!CheatManager.Instance.NoRessources)
            {
                StopAllCoroutines();
                PlayerManager.Instance.DepleteStamina(WallRunCost);
            }


        }
        else
        {
            IsWallRunning = false;
            // Normal Gravity
            basicMoving.Gravity = -25.0f;

            //ZRotation set back
            Quaternion InitalRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, 0.0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, InitalRotation, Time.deltaTime * TiltSpeed);

        }
    }

    public bool CheckWallLeft()
    {
        //---------------------------------------------Test-----------------------------------------------//
        //might need a better wall detection than just one raycast

        //Vector3 CheckPos = -transform.right + transform.position + new Vector3(0.5f, 0.8f, -0.1f);
        //return (Physics.CheckSphere(CheckPos, 0.8f, RunnableWall));
        //------------------------------------------------------------------------------------------------//

        return(Physics.Raycast(transform.position, -transform.right, WallCheckDist, RunnableWall));
    }

    public bool CheckWallRight()
    {
        //---------------------------------------------Test-----------------------------------------------//
        //might need a better wall detection than just one raycast

        //Vector3 CheckPos = transform.right + transform.position + new Vector3(0.5f, 0.8f, -0.1f);
        //return (Physics.CheckSphere(CheckPos, 0.8f, RunnableWall));
        //------------------------------------------------------------------------------------------------//

        return (Physics.Raycast(transform.position, transform.right, WallCheckDist, RunnableWall));
    }

    private bool CheckGroundWall()
    {
        IsGroundClose = (Physics.Raycast(transform.position, Vector3.down, WallCheckDist, Ground));
        return IsGroundClose;
    }

    public bool OnWall
    {
        get
        {
            return IsWallRunning;
        }
    }


}

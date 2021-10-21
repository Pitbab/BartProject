using System;
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
    public Action LeaveWall;


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
        
        if (Input.GetKey(KeyCode.LeftShift) && (CheckWallLeft() || (CheckWallRight())) && !basicMoving.JumpFormWall && PlayerManager.Instance.PlayerStam > 0) //hold a or d to stick to the wall
        {
            /*
             lock on feature to stay on the wall when looking away to jump on another wall
             --switch y gravity to 0 but put a timer when on wall
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
#if DEBUG
            if (!CheatManager.Instance.NoRessources)
            {
#endif
                //PlayerManager.Instance.DepleteStamina(WallRunCost);
#if DEBUG
            }
#endif
            
        }
        else
        {

            if (IsWallRunning == true)
            {
                basicMoving.LeaveWall?.Invoke();
            }
            
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

        Vector3 CheckPos = -transform.right + transform.position + new Vector3(0, 0.8f, +0.5f);
        return (Physics.CheckSphere(CheckPos, 0.5f, RunnableWall));
        
        //return(Physics.Raycast(transform.position, -transform.right, WallCheckDist, RunnableWall));
    }

    public bool CheckWallRight()
    {
        //---------------------------------------------Test-----------------------------------------------//
        //might need a better wall detection than just one raycast

        //Vector3 CheckPos = transform.right + transform.position + new Vector3(0.5f, 0.8f, -0.1f);
        //return (Physics.CheckSphere(CheckPos, 0.8f, RunnableWall));
        //------------------------------------------------------------------------------------------------//

        Vector3 CheckPos = transform.right + transform.position + new Vector3(0, 0.8f, +0.5f);
        return (Physics.CheckSphere(CheckPos, 0.5f, RunnableWall));
        //return (Physics.Raycast(transform.position, transform.right, WallCheckDist, RunnableWall));
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + new Vector3(-0.5f, 0.8f, +0.2f), 0.5f);
        Gizmos.DrawSphere(transform.position + new Vector3(0.5f, 0.8f, +0.2f), 0.5f);
    }
}

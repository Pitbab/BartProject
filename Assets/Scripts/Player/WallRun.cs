using System;
using UnityEngine;

public class WallRun : MonoBehaviour
{

    [SerializeField] private float WallRunGravity = -10;
    private bool IsGroundClose = true;
    private bool IsWallRunning;
    private const float TiltValue = 15.0f;
    private const float TiltSpeed = 10.0f;
    private const float WallCheckDist = 2.0f;
    private const float Gravity = -25f;

    private const float WallRunCost = 10.0f;
    private LayerMask Ground;
    private LayerMask RunnableWall;

    //testing
    private BasicMovement basicMoving;

    public GameObject CurrentWall;
    public GameObject LastWall;


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
            IsWallRunning = true;

            //wallRun Gravity
            basicMoving.Gravity = WallRunGravity;

            if (CheckWallRight())
            {
                ZRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, TiltValue);
            }

            if (CheckWallLeft())
            {
                ZRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, -TiltValue);
            }

            LastWall = CurrentWall;

            //give little tilt when wall running
            transform.rotation = Quaternion.Lerp(transform.rotation, ZRotation, Time.deltaTime * TiltSpeed);
#if DEBUG
            if (!CheatManager.Instance.NoRessources)
            {
#endif
                PlayerManager.Instance.DepleteStamina(WallRunCost);
#if DEBUG
            }
#endif
            
        }
        else
        {

            if (IsWallRunning)
            {
                basicMoving.LeaveWall?.Invoke();
            }
            
            IsWallRunning = false;
            // Normal Gravity
            basicMoving.Gravity = Gravity;

            //ZRotation set back
            Quaternion InitalRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, 0.0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, InitalRotation, Time.deltaTime * TiltSpeed);

        }
    }

    public bool CheckWallLeft()
    {

        Vector3 CheckPos = -transform.right + transform.position + new Vector3(0, 0.8f, +0.5f);
        Ray left = new Ray(transform.position, -transform.right);
        
        Physics.Raycast(left, out RaycastHit hit, WallCheckDist, RunnableWall);
        if (hit.collider != null)
        {
            CurrentWall = hit.collider.gameObject;
        }
        
        return (Physics.CheckSphere(CheckPos, 0.5f, RunnableWall));
        
    }

    public bool CheckWallRight()
    {

        Vector3 CheckPos = transform.right + transform.position + new Vector3(0, 0.8f, +0.5f);
        Ray right = new Ray(transform.position, transform.right);
        
        Physics.Raycast(right, out RaycastHit hit, WallCheckDist, RunnableWall);
        if (hit.collider != null)
        {
            CurrentWall = hit.collider.gameObject;
        }
        
        return (Physics.CheckSphere(CheckPos, 0.5f, RunnableWall));
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

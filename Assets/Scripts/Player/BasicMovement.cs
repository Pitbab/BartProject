using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BasicMovement : MonoBehaviour
{
    private CharacterController Controller;
    private AnimationController AnimManager;

    [SerializeField] private GameObject AimTarget;
    [SerializeField] private float JumpYSpeed;
    [SerializeField] private GameObject GroundPlace;

    private float CameraRotationY;
    private float CameraRotationX;
    private const float RotationSpeed = 3f;

    private const float RunningCost = 10.0f;
    private float CurrentSpeed;
    private const float RunningSpeed = 10f;
    private const float WalkingSpeed = 5f;
    private const float Acceleration = 4f;

    private bool UseJump = false;
    private bool IsRunning = false;
    private bool FowardBoost = false;

    private WallRun wallrun;
    private Climbing Climb;

    public bool InLowGrav = false;
    public Vector3 PredictedVel = Vector3.zero;

    private bool IsTrapped = false;

    public Vector3 CurrentPos => transform.position; 


    //jump

    private const float CheckGroundRadius = 0.3f;
    private const float ForwardWallJumpSpeed = 10.0f;
    private const float UpwardWallJumpSpeed = 15.0f;
    private const float GroundResetSpeed = -2.0f;
    private const float JumpDivisor = 3.0f;
    private bool IsOnGround = true;
    public Vector3 Velocity;
    private LayerMask Ground;
    private LayerMask Pickable;
    private LayerMask Car;
    private LayerMask AllLayer;

    public Action LeaveWall;
    
    public float Gravity;

    private const float velocityForRoll = -30f;
    
    //climb
    private const float ClimbOverSpeed = 2.0f;
    private bool EndOfClimb = false;
    
    
    private Vector3 StartingPos;

    //for tools
    private const float CheatSpeed = 20.0f;
        
    //Temporary testing for audio --------------------------------------------------//
    [SerializeField] private AudioSource PlayerMouth;
    [SerializeField] private AudioClip test;
    public List<AudioClip> StepSFX = new List<AudioClip>();
    


    void Start()
    {
#if DEBUG
        CheatManager.ResetLevel = ResetLevel;
#endif
        
        //StartingPos = PlayerManager.Instance.GetCheckpointPos;
        if (PlayerPrefs.HasKey("x"))
        {
            StartingPos.x = PlayerPrefs.GetFloat("x");
            StartingPos.y = PlayerPrefs.GetFloat("y");
            StartingPos.z = PlayerPrefs.GetFloat("z");
        }
        else
        {
            StartingPos = new Vector3(0, 0, -9);
        }

        transform.position = StartingPos;
        LeaveWall = LeavingWall;

        PlayerManager.Instance.RegisterMoving(this);

        Controller = GetComponent<CharacterController>();
        AnimManager = GetComponent<AnimationController>();
        wallrun = GetComponent<WallRun>();
        Climb = GetComponent<Climbing>();

        Ground = LayerMask.GetMask("Ground");
        Pickable = LayerMask.GetMask("Pickable");
        Car = LayerMask.GetMask("Car");
        Gravity = -25f;

        AllLayer = Ground | Pickable | Car;

    }


    void Update()
    {
        //if player is not trapped or in menu
        if(!IsTrapped && PlayerManager.Instance.GetMenuState == false)
        {
            Move();
            CheckForRoll();
            UpdateRotation();
            Jumping();
        }

        //temp code for slide

        if (Input.GetKeyDown(KeyCode.LeftControl) && CheckGround())
        {
            AnimManager.PlaySlidingAnim();
        }
#if DEBUG
        CheatCollision();
#endif
    }



    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        AnimManager.SetMovingBlend(x, z);

        //CurrentSpeed = 0;


        Vector3 move = transform.right * x + transform.forward * z;
        if(!Climb.Climb() &&AnimManager.GetAnimName() != "EndOfWall")
        {
#if DEBUG
            if(!CheatManager.Instance.Flying)
            {
#endif
                //Run faster
                if (Input.GetKey(KeyCode.LeftShift) && IsOnGround && PlayerManager.Instance.PlayerStam > 0)
                {
                    if (CurrentSpeed < RunningSpeed)
                    {
                        CurrentSpeed += Acceleration * Time.deltaTime;
                    }
                    else
                    {
                        CurrentSpeed = RunningSpeed;
                    }
#if DEBUG
                    if (!CheatManager.Instance.NoRessources)
                    {
#endif
                        if (x != 0 || z != 0)
                        {
                            PlayerManager.Instance.DepleteStamina(RunningCost);
                            IsRunning = true;
                        }
#if DEBUG
                    }
#endif
                }
                else
                {
                    IsRunning = false;
                    CurrentSpeed = WalkingSpeed;
                }
#if DEBUG
            }
#endif
            
            Controller.Move(move * CurrentSpeed * Time.deltaTime);
        }


        if(FowardBoost)
        {
            Controller.Move((transform.forward) * RunningSpeed * Time.deltaTime);
            CurrentSpeed = RunningSpeed;
        }

        if (EndOfClimb)
        {
            Controller.Move(transform.forward * ClimbOverSpeed * Time.deltaTime);
        }
        
        PredictedVel = move * CurrentSpeed;
    }

    private void UpdateRotation()
    {
        float MouseX = Input.GetAxis("Mouse X");
        float MouseY = Input.GetAxis("Mouse Y");
        string Slide = "Slide";

        CameraRotationY -= MouseY * RotationSpeed;
        CameraRotationY = Mathf.Clamp(CameraRotationY, -90.0f, 90.0f);
        CameraRotationX = Mathf.Clamp(CameraRotationX, -100, 100);

        //Rotate the player body to the last wall running angle or slide
        if (wallrun.OnWall || AnimManager.GetAnimName() == Slide)
        {
            //Player can look around when wall running or slide
            CameraRotationX -= MouseX * RotationSpeed;
            AimTarget.transform.localEulerAngles = Vector3.down * CameraRotationX + Vector3.right * CameraRotationY;
        }
        else
        {
            if (CameraRotationX != 0)
            {
                
                Quaternion Rotation = new Quaternion(transform.rotation.x, AimTarget.transform.rotation.y, transform.rotation.z, AimTarget.transform.rotation.w);
                transform.rotation = Rotation;
                CameraRotationX = 0.0f;
            }

            //Rotate the body (left right) and the head (up down)
            transform.Rotate(Vector3.up * MouseX * RotationSpeed);
            AimTarget.transform.localEulerAngles = Vector3.right * CameraRotationY;
        }

    }


    private void CheckForRoll()
    {
        if (Velocity.y < velocityForRoll)
        {
            Debug.Log("<color=red>Falling from high, Do a Roll</color>");
            AnimManager.PlayFallingAnim();

        }
        
        float lastvel = Velocity.y;

        if (!InLowGrav)
        {
            if (CheckGround())
            {
                if (lastvel < -60)
                {
                    PlayerManager.Instance.InstantDeath();
                }
                
                AnimManager.StopFallingAnim();
            }
        }
    }

    private void Jumping()
    {
#if DEBUG
        if(CheatManager.Instance.Flying)
        {
            
            ToolNoGravity();
        }
        else
        {
#endif
            //if in no gravity Zone
            if (InLowGrav)
            {
                AnimManager.SoftLanding();
                AnimManager.StopFallingAnim();
                NoGravity();
            }
            else
            {
                //AnimManager.NoSoftLanding();

                if (CheckGround() && Velocity.y < 0)
                {
                    Velocity = PredictedVel;
                    Velocity.y = GroundResetSpeed;
                }

                if (Input.GetKeyDown("space") && CheckGround())
                {
                    Velocity += PredictedVel / JumpDivisor;
                    Velocity.y = Mathf.Sqrt(JumpYSpeed * Gravity);
                    AnimManager.PlayjumpAnim();
                    PlayerMouth.PlayOneShot(test, 0.5f);
                }

                //jump after wallrun (to improve)

                if (Input.GetKeyDown(KeyCode.Space) && wallrun.OnWall && !UseJump)
                {
                    JumpFromWall();
                }

                //Reset the jump use if player change wall
                if ((!wallrun.CheckWallLeft() && !wallrun.CheckWallRight()) || wallrun.CurrentWall != wallrun.LastWall )
                {
                    UseJump = false;
                }

                Velocity.y += Gravity * Time.deltaTime;
            }
#if DEBUG
        }
#endif
        Controller.Move(Velocity * Time.deltaTime);
    }

    private void JumpFromWall()
    {
        Velocity = transform.forward * ForwardWallJumpSpeed;
        Velocity += new Vector3(AimTarget.transform.forward.x, 0, AimTarget.transform.forward.z) * ForwardWallJumpSpeed;
        Velocity.y = UpwardWallJumpSpeed;
        UseJump = true;
        AnimManager.PlayjumpAnim();
    }

    private void LeavingWall()
    {
        if (!UseJump)
        {
            Velocity = transform.forward * ForwardWallJumpSpeed;
            Velocity += new Vector3(AimTarget.transform.forward.x, 0, AimTarget.transform.forward.z) * ForwardWallJumpSpeed;
        }

    }

    private void NoGravity()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Velocity.y += RunningSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            Velocity.y -= RunningSpeed * Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.S))
        {
            Velocity -= transform.forward * RunningSpeed * Time.deltaTime; 
        }
        else if(Input.GetKey(KeyCode.W))
        {
            Velocity += transform.forward * RunningSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            Velocity += transform.right * RunningSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Velocity -= transform.right * RunningSpeed * Time.deltaTime;
        }
    }

    private bool CheckGround()
    {
        //make it a sphere for better detection when ground is small
        IsOnGround = (Physics.CheckSphere(GroundPlace.transform.position, CheckGroundRadius, AllLayer));
        return IsOnGround;
    }
    //----------------------------------------------------------------------------// Called by traps //------------------------------------------------------------------------------------------------//
    public void GetStagger()
    {
        AnimManager.PlayTrapped();
        StartCoroutine("Stagger");
        IsTrapped = true;
    }

    private IEnumerator Stagger()
    {
        yield return new WaitForSeconds(2);
        AnimManager.StopTrapped();
        IsTrapped = false;
    }

    //---------------------------------------------------------------------------// Called by animator //---------------------------------------------------------------------------------------------//

    //animator calling this when changes in controller size is needed to be made
    public void AnimChanges()
    {
        Controller.height = 0.8f;
        Controller.center = new Vector3(0, 0.4f, 0);
        FowardBoost = true;
    }

    //animator calling this to revert changes 
    public void RevertAnimChanges()
    {
        Controller.center = new Vector3(0, 0.9f, 0);
        Controller.height = 1.8f;
        FowardBoost = false;
    }

    //animator calling this to stop boosting when animation done
    public void SetBoostToFalse()
    {
        FowardBoost = false;
    }

    //-------------------------------------------------------------------------//Getter//---------------------------------------------------------------------------------------//

    //to know if player already used his jump form the current wall
    public bool JumpFormWall
    {
        get
        {
            return UseJump;
        }
    }

    //to know if player is running
    public bool Running
    {
        get
        {
            return IsRunning;
        }
    }
    
    //animator calling when climbing over is done
    public void Top()
    {
        EndOfClimb = !EndOfClimb;
    }

    //------------------------------------------------------------------------------------------//for tools//-----------------------------------------------------------------------------//
    //Respawn a startingpoints
    
#if DEBUG
    private void ResetLevel()
    {
        Controller.enabled = false;
        transform.position = StartingPos;
        Controller.enabled = true;
    }

    private void ToolNoGravity()
    {
        CurrentSpeed = CheatSpeed;
        if (Input.GetKey(KeyCode.Space))
        {
            Velocity.y += CheatSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            Velocity.y -= CheatSpeed * Time.deltaTime;
        }
        else
        {
            Velocity.y = 0.0f;
        }
    }

    private void CheatCollision()
    {
        if (CheatManager.Instance.NoClip)
        {
            Physics.IgnoreLayerCollision(14, 8, true);
            Physics.IgnoreLayerCollision(14, 9, true);
            Controller.detectCollisions = false;
        }
        else
        {
            Physics.IgnoreLayerCollision(14, 8, false);
            Physics.IgnoreLayerCollision(14, 9, false);
            Controller.detectCollisions = true;
        }
    }
    
#endif


}

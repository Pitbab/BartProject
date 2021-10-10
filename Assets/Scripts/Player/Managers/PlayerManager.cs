using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public event Action OnBasicMovementRegistered = delegate { };
    public event Action OnPlayerUiRegistered = delegate { };
    public event Action OnWallRunRegistered = delegate { };
    public event Action OnClimbingRegistered = delegate { };


    public event Action<float> OnStaminaChanged = delegate { };
    public event Action<float> OnHealthChanged = delegate { };


    public static PlayerManager Instance;
    private BasicMovement basicMov;
    private PlayerUI playerUI;
    private WallRun wallRun;
    private Climbing climbing;
    public CheckPoints CurrentCheckPoint = null;



    private float Stamina = 100.0f;
    private const float MaxStamina = 100.0f;
    private const float StaminaDepletionRate = 1.0f;

    private float Health = 100.0f;
    private const float HealthDepletionRate = 1.0f;
    private const float RegenRate = 5.0f;
    private float MaxHP = 100.0f;

    private Vector3 CheckpointPos = new Vector3(0.0f, 0.0f, 0.0f);
    private int CheckPointIndex = 0;
    private bool InPauseMenu = false;

    private string FinalTime;

    private List<Coroutine> delays = new List<Coroutine>();

    public float PlayerStam
    {
        get
        {
            return Stamina;
        }
    }

    public float GetMaxStam
    {
        get
        {
            return MaxStamina;
        }
    }

    public float PlayerHealth
    {
        get
        {
            return Health;
        }
    }

    public float GetMaxHealth
    {
        get
        {
            return MaxHP;
        }
    }

    public BasicMovement GetMoving
    {
        get
        {
            return basicMov;
        }
    }

    public Vector3 GetCheckpointPos
    {
        get
        {
            return CheckpointPos;
        }
    }

    public int GetCheckpointindex
    {
        get
        {
            return CheckPointIndex;
        }
    }

    public bool GetMenuState
    {
        get
        {
            return InPauseMenu;
        }
    }


    public BasicMovement BasicMovement => basicMov;

    public PlayerUI PlayerUI => playerUI;

    public WallRun WallRun => wallRun;

    public Climbing Climbing => climbing;



    private void Awake()
    {
       if(Instance == null)
       {
            Instance = this;
            DontDestroyOnLoad(gameObject);
       }
       else
       {
            Destroy(gameObject);
       }
    }

    private void Update()
    {
        if(Health > 0)
        {
            RegenStam();
        }
    }

    public void RegisterClimbing(Climbing climbing)
    {
        this.climbing = climbing;

        OnClimbingRegistered?.Invoke();
    }

    public void RegisterWallRun(WallRun wallRun)
    {
        this.wallRun = wallRun;

        OnWallRunRegistered?.Invoke();
    }

    public void RegisterMoving(BasicMovement moving)
    {
        this.basicMov = moving;

        OnBasicMovementRegistered?.Invoke();
    }

    public void RegisterPlayerUi(PlayerUI playerUI)
    {
        this.playerUI = playerUI;

        OnPlayerUiRegistered?.Invoke();
    }


    public void DepleteStamina(float DepletionRateFactor)
    {
        Stamina -= DepletionRateFactor * StaminaDepletionRate * Time.deltaTime;
        OnStaminaChanged?.Invoke(Stamina);
    }

    public void ChangeCheckPoint(Vector3 pos, int index)
    {
        CheckPointIndex = index;
        CheckpointPos = pos;
    }

    public void DepleteHealth(float DepletionRateFactor)
    {
        if(Health > 0)
        {
            Health -= DepletionRateFactor * HealthDepletionRate * Time.deltaTime;
            OnHealthChanged?.Invoke(Health);
        }
        else
        {
            GoToDeadScreen();
        }
    }

    public void InstantDamage(float Damage)
    {
        if(Health > 0)
        {
            Health -= Damage;
            OnHealthChanged?.Invoke(Health);
        }
        else
        {
            GoToDeadScreen();
        }
    }

    public void RestartValue()
    {
        Health = MaxHP;
        Stamina = MaxHP;
    }

    public void InstantDeath()
    {
        Health = 0;
        OnHealthChanged?.Invoke(Health);
        GoToDeadScreen();
    }

    private void RegenStam()
    {
        //keep delays in list to not use StopAllCoroutine
        if(!wallRun.OnWall && !basicMov.Running && !climbing.Climb())
        {
            Coroutine delay = StartCoroutine(RegenDelay());
            delays.Add(delay);
            OnStaminaChanged?.Invoke(Stamina);
        }
        else
        {
            foreach (var routine in delays)
            {
                StopCoroutine(routine);
            }
        }
    }

    private IEnumerator RegenDelay()
    {
        yield return new WaitForSeconds(2);
        if (Stamina < MaxStamina)
        {
            Stamina += RegenRate * Time.deltaTime;
        }
    }

    public void GoToWinScreen()
    {
        SceneManager.LoadScene("Win");
    }

    private void GoToDeadScreen()
    {
        SceneManager.LoadScene("dead");
    }

    public void ChangeMenuState()
    {
        InPauseMenu = !InPauseMenu;
    }

    public void SavingFinalTime()
    {
        FinalTime = PlayerUI.GetFinalTime();
    }

    public string GetFinalTime
    {
        get
        {
            return FinalTime;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class TurretController : PowerConsumer
{
    [SerializeField] private GameObject BarelEnd;
    [SerializeField] private GameObject TurretHead;
    [SerializeField] private GameObject Bullet;
    [SerializeField] private float MinCooldown;
    [SerializeField] private float MaxCooldown;
    [SerializeField] private float MaxRange;

    [SerializeField] private bool OnDrone = false;
    [SerializeField] private float VisionAngle = 90;
    
    private bool Activated = true;
    
    private BasicMovement PlayerMovement;
    private Transform PlayerPos;
    private Projectile BulletData;
    private float Timer = 5.0f;

    //private float TurretRange = 40.0f;
    private float RotationRate = 100.0f;
    private float timeToShoot;

    private LayerMask Player;

    [SerializeField] private AudioClip ShootSfx;

    private void Start()
    {
        BulletData = Bullet.GetComponent<Projectile>();
        timeToShoot = 0;

        Player = LayerMask.GetMask("Player");
    }

    public override void SwitchPower()
    {
        Activated = !Activated;
    }
    

    private void Update()
    {
        if (Activated)
        {
            CheckForPlayer();
        }
    }

    private float CheckDist()
    {
        return Vector3.Distance(PlayerPos.position, transform.position);
    }

    private void CheckForPlayer()
    {
        bool canShoot = true;
        
        if (PlayerInRange())
        {
            if (!OnDrone)
            {
                Vector3 angle = PlayerPos.position - transform.position;

                //if player is in vision angle
                if (!(Vector3.Angle(transform.forward, angle) <= VisionAngle))
                {
                    canShoot = false;
                }
                
            }

            if (canShoot)
            {
                Vector3 direction = ((PlayerPos.position + (PlayerMovement.PredictedVel * (CheckDist() / BulletData.BulletSpeed))) - transform.position);
                Quaternion rotation = Quaternion.LookRotation(direction);

                if (OnDrone)
                {
                    TurretHead.transform.rotation = Quaternion.Slerp(TurretHead.transform.rotation, rotation, Time.deltaTime * RotationRate);
                }
                else
                {
                    Vector3 correction = new Vector3(direction.x, 0, direction.z);
                    TurretHead.transform.right = -correction;
                }
                

                if (Timer >= timeToShoot)
                {
                    Shoot(rotation);
                }
                else
                {
                    Timer += Time.deltaTime;
                }
            }

        }
    }

    private bool PlayerInRange()
    {
        if (Physics.CheckSphere(transform.position, MaxRange, Player))
        {
            PlayerMovement = PlayerManager.Instance.BasicMovement;
            PlayerPos = PlayerMovement.transform;
            if (CheckDist() < MaxRange)
            {
                return true;
            }
        }

        return false;
    }

    private void Shoot(Quaternion rotation)
    {
        AudioManager.Instance.PlaySfx(ShootSfx, transform.position, 1);
        Instantiate(Bullet, BarelEnd.transform.position, rotation);
        timeToShoot = Random.Range(MinCooldown, MaxCooldown);
        Timer = 0.0f;
    }

    public BasicMovement GetPlayerPos()
    {
        if (PlayerInRange())
        {
            return PlayerMovement;
        }
        else
        {
            return null;
        }
    }
}



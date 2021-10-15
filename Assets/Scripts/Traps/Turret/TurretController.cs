using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class TurretController : MonoBehaviour
{
    [SerializeField] private GameObject BarelEnd;
    [SerializeField] private GameObject TurretHead;
    [SerializeField] private GameObject Bullet;
    [SerializeField] private float MinCooldown;
    [SerializeField] private float MaxCooldown;
    private BasicMovement PlayerMovement;
    private Transform PlayerPos;
    private Projectile BulletData;
    private float Timer = 5.0f;

    private float TurretRange = 40.0f;
    private float RotationRate = 100.0f;
    private float timeToShoot;

    private LayerMask Player;

    [SerializeField] private AudioClip ShootSfx;

    //test for tools
    bool Started = false;

    private void Start()
    {
        BulletData = Bullet.GetComponent<Projectile>();
        timeToShoot = 0;

        Player = LayerMask.GetMask("Player");
    }
    

    private void Update()
    {
        CheckForPlayer();
    }

    private float CheckDist()
    {
        return Vector3.Distance(PlayerPos.position, transform.position);
    }

    private void CheckForPlayer()
    {
        if (Physics.CheckSphere(transform.position, TurretRange, Player))
        {
            PlayerMovement = PlayerManager.Instance.BasicMovement;

            PlayerPos = PlayerMovement.transform;

            if (CheckDist() < TurretRange)
            {
                Vector3 direction = ((PlayerPos.position + (PlayerMovement.PredictedVel * (CheckDist() / BulletData.BulletSpeed))) - transform.position);
                
                Quaternion rotation = Quaternion.LookRotation(direction);
                TurretHead.transform.rotation = Quaternion.Slerp(TurretHead.transform.rotation, rotation, Time.deltaTime * RotationRate);

                if (Timer >= timeToShoot)
                {
                    AudioManager.Instance.PlaySfx(ShootSfx, transform.position, 1);
                    Instantiate(Bullet, BarelEnd.transform.position, rotation);
                    timeToShoot = Random.Range(MinCooldown, MaxCooldown);
                    Timer = 0.0f;
                }
                else
                {
                    Timer += Time.deltaTime;
                }
            }
        }
    }

    //private void OnDrawGizmos()
    //{
        //if(Started)
        //{
            //if (CheckDist() < TurretRange)
            //{
                //Gizmos.color = new Color(1, 0, 0, 0.3f);
            //}
            //else
            //{
                //Gizmos.color = new Color(0, 1, 0, 0.3f);
            //}
        //}
        //else
        //{
            //Gizmos.color = new Color(1, 1, 1, 0.3f);
        //}

        //Gizmos.DrawSphere(transform.position, TurretRange);
    //}
}

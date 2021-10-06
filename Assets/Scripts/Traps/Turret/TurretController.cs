using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] private GameObject BarelEnd;
    [SerializeField] private GameObject TurretHead;
    [SerializeField] private GameObject Bullet;
    private BasicMovement PlayerMovement;
    private Transform PlayerPos;
    private Projectile BulletData;
    private float ShotCooldown = 2.0f;
    private float Timer = 5.0f;

    private float TurretRange = 40.0f;
    private float RotationRate = 100.0f;

    //test for tools
    bool Started = false;

    private void Start()
    {
        BulletData = Bullet.GetComponent<Projectile>();

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
        if (Physics.CheckSphere(transform.position, TurretRange, LayerMask.GetMask("Player")))
        {
            PlayerMovement = PlayerManager.Instance.BasicMovement;

            PlayerPos = PlayerMovement.transform;
            Started = true;

            if (CheckDist() < TurretRange)
            {
                Vector3 direction = ((PlayerPos.position + (PlayerMovement.PredictedVel * (CheckDist() / BulletData.BulletSpeed))) - transform.position);
                Quaternion rotation = Quaternion.LookRotation(direction);
                TurretHead.transform.rotation = Quaternion.Slerp(TurretHead.transform.rotation, rotation, Time.deltaTime * RotationRate);

                if (Timer >= ShotCooldown)
                {
                    Instantiate(Bullet, BarelEnd.transform.position, rotation);
                    Timer = 0.0f;
                }
                else
                {
                    Timer += Time.deltaTime;
                }
            }
        }
    }


    private void OnDrawGizmos()
    {
        if(Started)
        {
            if (CheckDist() < TurretRange)
            {
                Gizmos.color = new Color(1, 0, 0, 0.3f);
            }
            else
            {
                Gizmos.color = new Color(0, 1, 0, 0.3f);
            }
        }
        else
        {
            Gizmos.color = new Color(1, 1, 1, 0.3f);
        }

        Gizmos.DrawSphere(transform.position, TurretRange);
    }
}

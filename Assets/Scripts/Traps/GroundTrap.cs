using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTrap : MonoBehaviour
{
    private LayerMask Player;
    private BasicMovement PlayerMovement;
    private bool IsUsed = false;
    private float TrapRadius = 0.8f;
    private float TrapMaxDist = 1.0f;


    private void Start()
    {
        Player = LayerMask.GetMask("Player");    
    }

    private void Update()
    {
        CheckForPlayer();
    }

    private void CheckForPlayer()
    {
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, TrapRadius, transform.up, out hitInfo, TrapMaxDist, Player))
        {
            if(hitInfo.collider != null)
            {
                PlayerMovement = hitInfo.collider.GetComponent<BasicMovement>();
                if(!IsUsed)
                {
                    PlayerMovement.GetStagger();
                    IsUsed = true;
                    Debug.Log("trapped");
                }
            }
        }
    }

}

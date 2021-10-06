using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestForTurret : MonoBehaviour
{
    private BasicMovement PlayerMovement;
    private Transform PlayerPos;
    [SerializeField] private GameObject Aim;

    private void Update()
    {
        if (PlayerMovement != null)
        {
            PlayerPos = PlayerMovement.transform;
            Aim.transform.position = PlayerPos.position;
        }
        else
        {
            PlayerMovement = PlayerManager.Instance.GetMoving;
        }
    }
}

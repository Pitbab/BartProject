using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    private const string Player = "Player";
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == Player)
        {
            PlayerManager.Instance.SavingFinalTime();
            PlayerManager.Instance.GoToWinScreen();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartZone : MonoBehaviour
{
    private const string Player = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Player))
        {
            CharacterController controller = other.gameObject.GetComponent<CharacterController>();
            TutorialController.Instance.PlayerRestart(controller);
        }
    }
}

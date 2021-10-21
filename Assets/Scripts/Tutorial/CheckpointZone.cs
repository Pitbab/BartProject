using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointZone : MonoBehaviour
{
    private const string Player = "Player";
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Player))
        {
            TutorialController.Instance.ChangeLastStep(this.transform.position);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartZone : MonoBehaviour
{
    private const string Player = "Player";
    private const string Object = "Pickable";

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case Player:
                ResetPlayer(other);
                break;
            case Object:
                ResetObjectState(other);
                break;
        }

    }

    private void ResetPlayer(Collider other)
    {
        CharacterController controller = other.gameObject.GetComponent<CharacterController>();
        TutorialController.Instance.PlayerRestart(controller);
    }

    private void ResetObjectState(Collider other)
    {
        //temp desactivation for presentation
        //PickableObject Object = other.gameObject.GetComponent<PickableObject>();
        //Object.ResetState();
    }
}

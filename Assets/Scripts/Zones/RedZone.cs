using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedZone : MonoBehaviour
{
    [SerializeField] private float ZoneDPS;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("<color=red>in the zone</color>");
            PlayerManager.Instance.DepleteHealth(ZoneDPS);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.hurtSFX[0], other.transform.position, 1.0f);
        }

    }
}

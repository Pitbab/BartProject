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
}

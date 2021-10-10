using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowGravityZone : MonoBehaviour
{

    private BasicMovement PlayerMov;
    
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerMov = other.GetComponent<BasicMovement>();
            PlayerMov.InLowGrav = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerMov.InLowGrav = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    private BasicMovement Movement;
    private float StartingFogDensity;

    private void Awake()
    {
        Movement = GetComponent<BasicMovement>();
        StartingFogDensity = RenderSettings.fogDensity;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Zone")
        {
            Debug.Log("<color=red>in the zone</color>");
            PlayerManager.Instance.DepleteHealth(50);

            //Create Fog When in the Zone
            RenderSettings.fogDensity += 0.01f * Time.deltaTime;
        }
        else if(other.gameObject.tag == "Gravity")
        {
            Movement.InLowGrav = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Zone")
        {
            RenderSettings.fogDensity = StartingFogDensity;
        }
        else if (other.gameObject.tag == "Gravity")
        {
            Movement.InLowGrav = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Death")
        {
            PlayerManager.Instance.InstantDeath();
        }
        else if(other.gameObject.tag == "Finish")
        {
            PlayerManager.Instance.SavingFinalTime();
            PlayerManager.Instance.GoToWinScreen();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Bullet")
        {
            Debug.Log("hit");
            PlayerManager.Instance.InstantDamage(10);
        }
    }
}

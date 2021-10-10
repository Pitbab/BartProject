using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    private BasicMovement Movement;

    private void Awake()
    {
        Movement = GetComponent<BasicMovement>();
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

    //private void OnCollisionEnter(Collision collision)
    //{
       // if (collision.collider.tag == "Bullet")
        //{
         //   Debug.Log("hit");
          //  PlayerManager.Instance.InstantDamage(10);
      //  }
 //   }
}

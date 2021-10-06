using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{

    private LayerMask Interactible;
    private const float DetectionRadius = 3.0f;
    private void Start()
    {
        Interactible = LayerMask.GetMask("Interactible");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            CheckForObjectToInteract();
        }
    }

    private void CheckForObjectToInteract()
    {
        Collider[] powerHitbox = Physics.OverlapSphere(transform.position, DetectionRadius, Interactible);

        foreach (Collider hit in powerHitbox)
        {
            Lever lever = hit.GetComponent<Lever>();
            if(lever != null)
            {
                lever.Activate();
            }

            CheckPoints checkpoint = hit.GetComponent<CheckPoints>();
            if(checkpoint != null)
            {
                checkpoint.SetCheckPoint();
            }


        }
    }
}

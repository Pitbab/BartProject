using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWall : MonoBehaviour
{
    private Rigidbody[] Bodies;
    private BoxCollider PreDestroyCollider;
    private const float TimeToDestroy = 2.0f;
    private void Start()
    {
        PreDestroyCollider = GetComponent<BoxCollider>();
        Bodies = GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody body in Bodies)
        {
            body.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public void Shatter(Vector3 dir)
    {
        Destroy(PreDestroyCollider);
        foreach(Rigidbody body in Bodies)
        {
            body.isKinematic = false;
            body.useGravity = true;
            body.constraints = RigidbodyConstraints.None;
            body.AddForce(dir, ForceMode.VelocityChange);
        }
        Destroy(gameObject, TimeToDestroy);
    }
}

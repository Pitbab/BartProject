using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    private Vector3 StartPos;
    private Rigidbody Body;
    private Quaternion StartRot;

    private void Start()
    {
        StartPos = transform.position;
        Body = GetComponent<Rigidbody>();
    }

    public void ResetState()
    {
        transform.position = StartPos;
        transform.rotation = StartRot;
        
        Body.velocity = Vector3.zero;
        Body.isKinematic = false;
        Body.useGravity = true;
    }
}

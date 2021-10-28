using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    private Vector3 StartPos;
    private Rigidbody Body;

    private void Start()
    {
        StartPos = transform.position;
        Body = GetComponent<Rigidbody>();
        TutorialController.Instance.AddObject(this);
    }

    public void ResetState()
    {
        transform.position = StartPos;

        Body.isKinematic = false;
        Body.useGravity = true;
    }
}

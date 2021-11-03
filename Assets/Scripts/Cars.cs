using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cars : MonoBehaviour
{
    private float CarSpeed = 20.0f;
    private Vector3 Direction = Vector3.zero;
    

    private void Update()
    {
        transform.position += Direction.normalized * CarSpeed * Time.deltaTime;
    }
}

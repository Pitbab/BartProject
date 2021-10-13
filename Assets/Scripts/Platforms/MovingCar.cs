using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class MovingCar : MonoBehaviour
{

    [SerializeField] private float TimeToRotate;
    [SerializeField] private List<Transform> Locations = new List<Transform>();
    [SerializeField] private float Speed;
    [SerializeField] private float RotationSpeed;
    [SerializeField] private float MinDist;
    private int LocationIndex = 0;
    private Vector3 TargetLocation = Vector3.zero;
    private Vector3 LastLocation = Vector3.zero;
    private float Rotator = 90;
    private Transform childMesh;
    private BoxCollider Coll;
    
    //private bool IsGoingLeft;

    private void Start()
    {
        LastLocation = transform.position;
        TargetLocation = Locations[LocationIndex].position;
        StartCoroutine(MoveToLocation());
        childMesh = transform.GetChild(0);
        Coll = GetComponent<BoxCollider>();
        childMesh.Rotate(0,0, 90);
        Coll.size = new Vector3(Coll.size.z, Coll.size.y, Coll.size.x);
        
    }
    
    private void SetLocation()
    {
        if (LocationIndex == Locations.Count - 1) LocationIndex = 0; else LocationIndex++;
        
        TargetLocation = Locations[LocationIndex].position;
    }


    private IEnumerator MoveToLocation()
    {
        float lerpValue = 0;
        Vector3 startingPos = transform.position;
        float timeToDest = Vector3.Distance(transform.position, TargetLocation) / Speed;
        
        while (lerpValue < 1)
        {
            lerpValue += Time.deltaTime / timeToDest;
            transform.position = Vector3.Lerp(LastLocation, TargetLocation, lerpValue);
            yield return null;
        }
        
        SetLocation();
        LastLocation = transform.position;
        
        //doing this for now (assets offset angle messing with turning coroutine)
        childMesh.Rotate(0,0, -90);
        Coll.size = new Vector3(Coll.size.z, Coll.size.y, Coll.size.x);
        StartCoroutine(MoveToLocation());
        //StartCoroutine(Turning());

    }
    
    private IEnumerator Turning()
    {
        float timer = 0;
        
        Vector3 Angle = (childMesh.position - TargetLocation);
        Quaternion lookRot = Quaternion.LookRotation(Angle);

        while (timer < TimeToRotate)
        {
            timer += Time.deltaTime;
            float lerpValue = timer / TimeToRotate;

            
            childMesh.rotation = Quaternion.Lerp(childMesh.rotation, lookRot, lerpValue);

            yield return null;
        }

        StartCoroutine(MoveToLocation());
    }


}

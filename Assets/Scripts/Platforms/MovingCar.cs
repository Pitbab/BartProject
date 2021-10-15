using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class MovingCar : MonoBehaviour
{

    [SerializeField] private float TimeToRotate;
    [SerializeField] private List<GameObject> Locations = new List<GameObject>();
    [SerializeField] private float Speed;
    [SerializeField] private float RotationSpeed;
    [SerializeField] private float MinDist;
    private int LocationIndex = 0;
    private GameObject TargetLocation = null;
    private Vector3 LastLocation = Vector3.zero;
    private float Rotator = 90;
    private Transform childMesh;
    private BoxCollider Coll;

    public bool Interactible;

    private void Start()
    {
        LastLocation = transform.position;
        TargetLocation = Locations[LocationIndex];
        StartCoroutine(MoveToLocation());
        childMesh = transform.GetChild(0);
        Coll = GetComponent<BoxCollider>();
        childMesh.Rotate(0,0, Rotator);
        Coll.size = new Vector3(Coll.size.z, Coll.size.y, Coll.size.x);
        Interactible = true;
    }
    
    private void SetLocation()
    {
        if (LocationIndex == Locations.Count - 1) LocationIndex = 0; else LocationIndex++;
        
        TargetLocation = Locations[LocationIndex];
    }


    private IEnumerator MoveToLocation()
    {
        float lerpValue = 0;
        Vector3 startingPos = transform.position;
        float timeToDest = Vector3.Distance(transform.position, TargetLocation.transform.position) / Speed;
        
        while (lerpValue < 1)
        {
            lerpValue += Time.deltaTime / timeToDest;
            transform.position = Vector3.Lerp(LastLocation, TargetLocation.transform.position, lerpValue);
            yield return null;
        }

        if (TargetLocation.name.Contains("Left"))
        {
            childMesh.Rotate(0,0, -Rotator);
            Coll.size = new Vector3(Coll.size.z, Coll.size.y, Coll.size.x);
        }
        else if(TargetLocation.name.Contains("Right"))
        {
            childMesh.Rotate(0,0, Rotator);
            Coll.size = new Vector3(Coll.size.z, Coll.size.y, Coll.size.x);
        }
        
        SetLocation();
        LastLocation = transform.position;
        
        //doing this for now (assets offset angle messing with turning coroutine)

        StartCoroutine(MoveToLocation());
        //StartCoroutine(Turning());

    }
    
    private IEnumerator Turning()
    {
        float timer = 0;
        
        Vector3 Angle = (childMesh.position - TargetLocation.transform.position);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("endwall"))
        {
            Interactible = false;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("endwall"))
        {
            Interactible = true;
        }
    }
}

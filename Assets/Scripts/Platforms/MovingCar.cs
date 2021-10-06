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
    
    //private bool IsGoingLeft;

    private void Start()
    {
        //StartCoroutine(Moving());
        LastLocation = transform.position;
        TargetLocation = Locations[LocationIndex].position;
        StartCoroutine(MoveToLocation());
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
        StartCoroutine(Turning());
    }
    
    private IEnumerator Turning()
    {
        float timer = 0;

        quaternion rotation = quaternion.Euler(transform.rotation.x, transform.rotation.y - Rotator, transform.rotation.z);
        
        while (timer < TimeToRotate)
        {
            timer += Time.deltaTime;
            float lerpValue = timer / TimeToRotate;

            
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, lerpValue);

            yield return null;
        }

        Rotator += 90;
        
        StartCoroutine(MoveToLocation());
    }
    

    /*
     *
     *     private IEnumerator Moving()
    {
        float timer = 0;

        while (timer < TimeToLap)
        {
            timer += Time.deltaTime;
            float lerpValue = timer / TimeToLap;

            if (IsGoingLeft)
            {
                Mesh.transform.rotation = Quaternion.Euler(-90.0f, -90.0f, 0.0f);
                transform.position = Vector3.Lerp(Starting.position, Ending.position, lerpValue);
            }
            else
            {
                Mesh.transform.rotation = Quaternion.Euler(-90.0f, 90.0f, 0.0f);
                transform.position = Vector3.Lerp(Ending.position, Starting.position, lerpValue);
            }

            yield return null;
        }
        
        StartCoroutine(Turing());

    }


     * 
     */



}

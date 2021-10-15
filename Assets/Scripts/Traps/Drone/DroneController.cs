using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    [SerializeField] private List<GameObject> Locations = new List<GameObject>();
    [SerializeField] private float Speed;
    private int LocationIndex = 0;
    private GameObject TargetLocation = null;
    private Vector3 LastLocation = Vector3.zero;


    private void Start()
    {
        LastLocation = transform.position;
        TargetLocation = Locations[LocationIndex];
        StartCoroutine(MoveToLocation());
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

        SetLocation();
        LastLocation = transform.position;

        StartCoroutine(MoveToLocation());
    }
}

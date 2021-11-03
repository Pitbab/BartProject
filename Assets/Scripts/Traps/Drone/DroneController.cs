using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    [SerializeField] private List<GameObject> Locations = new List<GameObject>();
    [SerializeField] private float Speed;
    [SerializeField] private TurretController turret;
    private int LocationIndex = 0;
    private GameObject Targetobject = null;
    private Vector3 LastLocation = Vector3.zero;
    private Vector3 TargetLocation = Vector3.zero;
    private Coroutine CurrentRoutine;


    private void Start()
    {
        LastLocation = transform.position;
        TargetLocation = Locations[LocationIndex].transform.position;
        CurrentRoutine = StartCoroutine(MoveToLocation());
    }
    

    private void SetLocation()
    {
        if (turret.GetPlayerPos() != null)
        {
            TargetLocation = turret.GetPlayerPos().transform.position + new Vector3(5, 15, 0);
        }
        else
        {
            if (LocationIndex == Locations.Count - 1) LocationIndex = 0; else LocationIndex++;
            TargetLocation = Locations[LocationIndex].transform.position;
        }
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

        CurrentRoutine = StartCoroutine(MoveToLocation());
    }

    public void Destroyed()
    {
        StopCoroutine(CurrentRoutine);
    }
}

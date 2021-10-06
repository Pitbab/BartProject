using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class OnCars : MonoBehaviour
{

    [SerializeField] private Transform GroundPlace;
    private LayerMask CarsLayer;
    private float CheckGroundRadius = 0.3f;
    private GameObject CurrentCar;
    private const string RotCorrector = "RotationCorrector";
    
    void Start()
    {
        CarsLayer = LayerMask.GetMask("Car");
    }
    
    void Update()
    {
        CheckForCars();
    }

    private void CheckForCars()
    {
        Ray GroundCheck = new Ray(GroundPlace.position, Vector3.down * CheckGroundRadius);
        
        RaycastHit hitInfo;
        if (Physics.Raycast(GroundCheck, out hitInfo, CheckGroundRadius, CarsLayer))
        {
            CurrentCar = hitInfo.transform.parent.gameObject;
            this.transform.parent = CurrentCar.transform;
        }

    }
}

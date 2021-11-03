using System;
using UnityEngine;

public class OnCars : MonoBehaviour
{

    [SerializeField] private Transform GroundPlace;
    private LayerMask CarsLayer;
    private float CheckGroundRadius = 0.3f;
    private GameObject CurrentCar;
    
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
            CurrentCar = hitInfo.transform.gameObject;
            MovingCar interactible = CurrentCar.GetComponent<MovingCar>();
            if (interactible.Interactible == true)
            {
                this.transform.parent = CurrentCar.transform; 
            }
            else
            {
                this.transform.parent = null;
            }

            this.transform.parent = CurrentCar.transform;

        }
        else
        {
            this.transform.parent = null;
        }
    }
    
}

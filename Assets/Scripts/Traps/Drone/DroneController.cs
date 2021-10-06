using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    [SerializeField] private GameObject Turret;
    [SerializeField] private List<Transform> PatrolsCheckPoints = new List<Transform>();
    private Transform TargetedCheckpoint;
    private Transform CurrentCheckpoint;
    private int CheckpointIndex;

    private float DroneSpeed = 0.5f;

    private void Start()
    {
        CheckpointIndex = 0;
    }

    private void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        TargetedCheckpoint = PatrolsCheckPoints[CheckpointIndex];

        if (CurrentCheckpoint != TargetedCheckpoint)
        {
            GoToCheckPoint();
        }

        if (transform.position == TargetedCheckpoint.position)
        {
            CurrentCheckpoint = TargetedCheckpoint;
            CheckpointIndex++;
        }
    }

    private void GoToCheckPoint()
    {
        transform.position = Vector3.Lerp(transform.position, TargetedCheckpoint.position, DroneSpeed * Time.deltaTime);
    }
    
}

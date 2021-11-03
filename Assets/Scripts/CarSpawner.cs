using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject CarPrefab;
    [SerializeField] private float SpawnInterval;
    [SerializeField] private Transform StartPos;
    [SerializeField] private Transform EndPos;
    private const float LifeDelay = 1.0f;

    private void Start()
    {
        StartCoroutine(LaunchCycle());
    }

    private IEnumerator LaunchCycle()
    {
        yield return new WaitForSeconds(SpawnInterval);
        Quaternion rotation = Quaternion.LookRotation(EndPos.position - StartPos.position);
        GameObject current = Instantiate(CarPrefab, StartPos.position, Quaternion.identity);
        Cars test = current.GetComponent<Cars>();

        //test.LaunchCar(rotation);
        Destroy(current, SpawnInterval + LifeDelay);
        StartCoroutine(LaunchCycle());
    }
}

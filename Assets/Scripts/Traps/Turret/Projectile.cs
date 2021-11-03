using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float BulletSpeed = 30.0f;
    public bool IsCatched = false;
    private float LifeTime = 1.0f;
    [SerializeField] private Material ColorIndicator;
    [SerializeField] private GameObject Explosion;

    private Vector3 Direction = Vector3.zero;

    //tags
    private const string Breakable = "Breakable";
    private const string Pickable = "Pickable";
    private const string Player = "Player";
    private const string Drone = "Drone";
    
    private const string Cannon = "Cannon";

    private void Start()
    {
        Direction = transform.forward;
    }
    private void Update()
    {
        if(IsCatched == false)
        {
            transform.position += Direction * BulletSpeed * Time.deltaTime;
        }
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag(Breakable))
        {
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag(Pickable))
        {
            Direction = collision.GetContact(0).normal.normalized;
            Destroy(gameObject, LifeTime);
        }

        if (collision.collider.CompareTag(Drone))
        {
            Debug.Log("Drone touched");
            if (this.IsCatched)
            {
                //instantiate explosion
                Instantiate(Explosion, collision.transform);
                //disable the turret
                TurretController turret = collision.gameObject.GetComponentInChildren<TurretController>();
                turret.enabled = false;
                //disable the drone
                DroneController Drone = collision.gameObject.GetComponent<DroneController>();
                Drone.Destroyed();
                //Drone.enabled = false;
                //make it drop
                Rigidbody body = collision.gameObject.AddComponent<Rigidbody>();
                body.useGravity = true;
                body.mass = 100.0f;

                //destroy the drone
                Destroy(collision.gameObject, 2.5f);

            }
        }
        
        if(collision.gameObject.name != Cannon)
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag(Player))
        {
            PlayerAudioCollection audio = collision.gameObject.GetComponent<PlayerAudioCollection>();
            audio.PlayHurt(audio.transform.position, 0.5f);
            Debug.Log("hit");
            PlayerManager.Instance.InstantDamage(10);
            Destroy(gameObject);
        }
    }
}




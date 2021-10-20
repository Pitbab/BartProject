using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float BulletSpeed = 30.0f;
    public bool IsCatched = false;
    private float LifeTime = 1.0f;
    [SerializeField] private Material ColorIndicator;

    private Vector3 Direction = Vector3.zero;

    //tags
    private const string Breakable = "Breakable";
    private const string Pickable = "Pickable";
    private const string Player = "Player";

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




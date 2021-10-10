using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float BulletSpeed = 30.0f;
    public bool IsCatched = false;
    public bool InZone = false;
    private float LifeTime = 5.0f;
    private MeshRenderer Renderer;
    private Material StartingMat;
    [SerializeField] private Material ColorIndicator;

    private void Start()
    {
        Renderer = GetComponent<MeshRenderer>();
        StartingMat = Renderer.material;
    }
    private void Update()
    {
        if(IsCatched == false)
        {
            transform.position += transform.forward * BulletSpeed * Time.deltaTime;
        }
        
        ChangeMats();
    }

    private void ChangeMats()
    {
        if(InZone == true)
        {
            Renderer.material = ColorIndicator;
        }
        else
        {
            Renderer.material = StartingMat;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Breakable")
        {
            Destroy(collision.gameObject);
        }

        if(collision.gameObject.name != "Cannon")
        {
            Destroy(gameObject, LifeTime);
        }

        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("hit");
            PlayerManager.Instance.InstantDamage(10);
        }
    }
}




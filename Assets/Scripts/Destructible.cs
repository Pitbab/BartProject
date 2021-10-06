using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject DestroyedVersion;
    [SerializeField] private MeshCollider[] meshes;

    public void Explode(Vector3 dir)
    {
        DestroyedVersion.gameObject.transform.localScale = transform.localScale;
        Instantiate(DestroyedVersion, transform.position, transform.rotation);
        Destroy(gameObject);

        foreach(MeshCollider mesh in meshes)
        {
            mesh.attachedRigidbody.AddForce(dir, ForceMode.VelocityChange);
        }

        //MeshCollider[] meshes = DestroyedVersion.GetComponentsInChildren<MeshCollider>();
        //foreach(var part in meshes)
        //{
        //    part.attachedRigidbody.AddForce(dir, ForceMode.VelocityChange);
        //}

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionWithRigidBody : MonoBehaviour
{
    
    //not using this class for now but might be useful for later
    
    
    private Vector3 LastFramePosition;
    private float VelocityMagnitude;
    private Vector3 Velocity;
    private float InteractionStrength = 500.0f;

    private void FixedUpdate()
    {
        Velocity.x = transform.position.x - LastFramePosition.x;
        Velocity.y = transform.position.y - LastFramePosition.y;
        Velocity.z = transform.position.z - LastFramePosition.z;

        float vx;
        float vy;
        float vz;
        if (Velocity.x < 0) { vx = Velocity.x * -1; } else { vx = Velocity.x; }
        if (Velocity.y < 0) { vy = Velocity.y * -1; } else { vy = Velocity.y; }
        if (Velocity.z < 0) { vz = Velocity.z * -1; } else { vz = Velocity.z; }

        VelocityMagnitude = vx + vy + vz;

        LastFramePosition = transform.position;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.rigidbody == null) { return; }
        Vector3 Dir = Velocity;

        hit.rigidbody.AddForce(Dir * VelocityMagnitude * InteractionStrength * Time.deltaTime, ForceMode.Impulse);

    }
}

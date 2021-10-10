using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telekinesis : MonoBehaviour
{
    [SerializeField] private GameObject Upper;
    [SerializeField] Transform HoldingPlace;
    [SerializeField] private Vector3 PowerBoxSize;
    [SerializeField] private GameObject PowerBox;

    private const float MaxMana = 100.0f;
    private const float MaxRange = 30.0f;
    private const float Strength = 30.0f;
    private const float ObjectHoldingSpeed = 15.0f;

    private Ray TelekinesisRange;

    private Rigidbody ObjectInUse;
    private List<Rigidbody> ObjectInFront = new List<Rigidbody>();

    public int ModeIndex = 0;

    private Vector3 InitialPos;
    private float MouseScroll;

    public float CurrentMana = 100f;
    private const float ManaRegenRate = 5.0f;
    private const float ManaUseRate = 5.0f;
    private const float BulletStopCost = 10.0f;

    [SerializeField] private GameObject UI;
    private PlayerUI ui;
    private AnimationController AnimController;

    private LayerMask Pickable;


    private void Start()
    {
        ui = UI.GetComponent<PlayerUI>();
        InitialPos = HoldingPlace.localPosition;
        AnimController = GetComponent<AnimationController>();
        Pickable = LayerMask.GetMask("Pickable");
    }


    private void Update()
    {
        TelekinesisRange = new Ray(Upper.transform.position, Upper.transform.forward * MaxRange);

        UpdateModeIndex();

        if(ModeIndex == 0)
        {
            ObjectSelection();
        }

        if(ModeIndex == 1)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && ObjectInUse == null)
            {
                Push();
            }
        }

        if(ObjectInUse == null)
        {
            RegenMana();
        }

    }

    private void ObjectSelection()
    {
        if (ObjectInUse)
        {
            MoveObject();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (ObjectInUse)
            {
                DeSelectObject();
            }
            else
            {
                HoldObject();
            }
        }
    }

    private void RegenMana()
    {
        if(CurrentMana < MaxMana)
        {
            CurrentMana += ManaRegenRate * Time.deltaTime;
        }

    }

    private void UpdateModeIndex()
    {
        //switch between different power index and reset object state if there is one in use.
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(ObjectInUse)
            {
                ObjectInUse.isKinematic = false;
                ObjectInUse.useGravity = true;
                AnimController.StopHold();
            }

            ObjectInUse = null;

            if (ModeIndex < 1)
            {
                ModeIndex++;
            }
            else
            {
                ModeIndex = 0;
            }

        }
    }

    private void HoldObject()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(TelekinesisRange, out hitInfo, MaxRange, Pickable))
        {
            ObjectInUse = hitInfo.collider.gameObject.GetComponent<Rigidbody>();
            if(ObjectInUse)
            {
                ObjectInUse.isKinematic = true;
                AnimController.PlayHold();

            }
        }
    }

    private void MoveObject()
    {

        if(CurrentMana > 0)
        {
            if (!CheatManager.Instance.NoRessources)
            {
                //cost of mana to hold object in the air
                CurrentMana -= ManaUseRate * ObjectInUse.mass * Time.deltaTime;
            }

            //calculate the new position
            Vector3 pos = HoldingPlace.localPosition;
            pos.z += Input.mouseScrollDelta.y;
            HoldingPlace.localPosition = pos;

            //Move object to the holding position
            ObjectInUse.MovePosition(Vector3.Lerp(ObjectInUse.position, HoldingPlace.transform.position, Time.deltaTime * ObjectHoldingSpeed));

            //let the object hold in the air and jump on them
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                ObjectInUse.velocity = Vector3.zero;
                ObjectInUse.useGravity = false;
                ObjectInUse = null;
                HoldingPlace.localPosition = InitialPos;
            }
        }
        else
        {
            DeSelectObject();
            ui.NotEnoughMana();
            Debug.Log("Not enough mana to hold this object");
        }

    }

    private void Push()
    {
        Collider[] powerHitbox = Physics.OverlapBox(PowerBox.transform.position, PowerBoxSize / 2, transform.rotation);

        foreach (Collider hit in powerHitbox)
        {
            string tag = hit.gameObject.tag;
            
            if(tag == "Pickable")
            {
                float cost = hit.attachedRigidbody.mass * ManaUseRate;
                CheckForObjectToPush(hit, cost, null);

            }
            else if (tag == "Bullet")
            {
                Projectile projectile = hit.GetComponent<Projectile>();
                float cost = hit.attachedRigidbody.mass * ManaUseRate * BulletStopCost;
                CheckForObjectToPush(hit, cost, projectile);
            }
            else if(tag == "Breakable")
            {

                float cost = hit.attachedRigidbody.mass * ManaUseRate;
                DestroyWall(hit, cost);
            }
        }

    }



    private void DeSelectObject()
    {
        ObjectInUse.isKinematic = false;
        ObjectInUse.useGravity = true;
        ObjectInUse = null;
        AnimController.StopHold();

        HoldingPlace.localPosition = InitialPos;
    }

    private void DestroyWall(Collider hit, float cost)
    {
        DestroyWall destructible = hit.GetComponent<DestroyWall>();

        if (CurrentMana >= cost)
        {
            if (!CheatManager.Instance.NoRessources)
            {
                CurrentMana -= cost;
            }
            
            AnimController.PlayPush();
            destructible.Shatter(Upper.transform.forward * Strength);
        }
    }

    private void CheckForObjectToPush(Collider hit, float cost, Projectile bullet)
    {

        if (CurrentMana >= cost)
        {
            if(bullet != null)
            {
                bullet.IsCatched = true;
            }

            Rigidbody rb = hit.attachedRigidbody;

            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(Upper.transform.forward * Strength, ForceMode.VelocityChange);

            if (!CheatManager.Instance.NoRessources)
            {
                CurrentMana -= cost;
            }
            
            AnimController.PlayPush();

        }
        else
        {
            ui.NotEnoughMana();
            Debug.Log("not enough mana for all object");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = PowerBox.transform.localToWorldMatrix;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, PowerBoxSize);
    }

    //putting an indicator on the bullet might be better
    public bool CheckForBullets()
    {
        Collider[] powerHitbox = Physics.OverlapBox(PowerBox.transform.position, PowerBoxSize / 2, transform.rotation);

        foreach (Collider hit in powerHitbox)
        {
            if (hit.gameObject.tag == "Bullet")
            {
                return true;
            }
        }
        return false;

    }
}

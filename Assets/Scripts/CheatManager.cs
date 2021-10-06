using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CheatManager : MonoBehaviour
{

    private static CheatManager UniqueInstance;

    public static Action ResetLevel;
    private bool ShowWindow = false;
    private bool NoRessourcesUse = false;
    private bool FlyingMode = false;
    private bool Clipping = false;
    private bool ShowTrigger = false;
    private Rect WinRect = new Rect(100, 100, 200, 300);

    public static CheatManager Instance
    {
        get
        {
            if(UniqueInstance == null)
            {
                GameObject Go = new GameObject("cheatManager");
                Go.AddComponent<CheatManager>();
            }
            return UniqueInstance;
        }
    }

    private void Awake()
    {
        if(UniqueInstance == null)
        {
            UniqueInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public bool NoRessources
    {
        get
        {
            return NoRessourcesUse;
        }
    }

    public bool Flying
    {
        get
        {
            return FlyingMode;
        }
    }

    public bool NoClip
    {
        get
        {
            return Clipping;
        }
    }

#if DEBUG

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ShowWindow = !ShowWindow;
            Cursor.visible = !Cursor.visible;

            if (Cursor.lockState == CursorLockMode.Locked)
            { 
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

        }
    }

    private void WinFunc(int id)
    {
        NoRessourcesUse = GUILayout.Toggle(NoRessourcesUse, "No Mana or Stam use");
        FlyingMode = GUILayout.Toggle(FlyingMode, "FlyMode");
        Clipping = GUILayout.Toggle(Clipping, "No Clip");
        ShowTrigger = GUILayout.Toggle(ShowTrigger, "ShowClimable");

        if(GUILayout.Button("Reset"))
        {
            ResetLevel?.Invoke();
        }
    }

    private void OnGUI()
    {
        if(ShowWindow)
        {
            WinRect = GUI.Window(0, WinRect, WinFunc, "Cheat manager");
        }
    }

    private void OnDrawGizmos()
    {
        if(ShowTrigger)
        {
            GameObject[] interactible = GameObject.FindGameObjectsWithTag("Triggers");
            foreach (GameObject obj in interactible)
            {
                BoxCollider box = obj.GetComponent<BoxCollider>();

                Gizmos.color = new Color(0, 0, 1);
                Gizmos.DrawCube(obj.transform.position, box.bounds.size);
            }

            GameObject[] Climable = GameObject.FindGameObjectsWithTag("Climable");
            foreach (GameObject obj in Climable)
            {
                BoxCollider box = obj.GetComponent<BoxCollider>();

                Gizmos.color = new Color(0, 1, 0);
                Gizmos.DrawCube(obj.transform.position, box.bounds.size);
            }

            var Checkpoints = FindObjectsOfType<CheckPoints>();
            foreach(CheckPoints obj in Checkpoints)
            {
                BoxCollider box = obj.GetComponent<BoxCollider>();
                Gizmos.color = new Color(1, 0, 0);
                Gizmos.matrix = box.transform.localToWorldMatrix;
                Gizmos.DrawCube(box.center, box.size);
            }

            var lever = FindObjectsOfType<Lever>();
            foreach (Lever obj in lever)
            {
                BoxCollider box = obj.GetComponent<BoxCollider>();
                Gizmos.color = new Color(1, 0, 0);
                Gizmos.matrix = box.transform.localToWorldMatrix;
                Gizmos.DrawCube(box.center, box.size);
            }
        }
    }

#endif
}

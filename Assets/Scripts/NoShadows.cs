using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoShadows : MonoBehaviour
{
    private List<MeshRenderer> MeshRenderers = new List<MeshRenderer>();

    private void Start()
    {
        MeshRenderer[] MeshFound = GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer Mesh in MeshFound)
        {
            MeshRenderers.Add(Mesh);
        }

        SwitchShadowMode();
    }

    private void SwitchShadowMode()
    {
        foreach(MeshRenderer Mesh in MeshRenderers)
        {
            Mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
    }
}

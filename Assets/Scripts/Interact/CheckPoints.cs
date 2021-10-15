using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    [SerializeField] private Transform SpawnPlace;
    [SerializeField] private MeshRenderer Indicator;
    [SerializeField] private Light LightColor;
    [SerializeField] private Material IndicatorMat;
    public bool IsActive;
    public int index;

    private void Start()
    {
        if (PlayerManager.Instance.GetCheckpointindex == index)
        {
            IsActive = true;
        }

        if (IsActive)
        {
            ChangeColor();
        }
    }

    public void SetCheckPoint()
    {
        Debug.Log("CheckpointSet");
        //PlayerManager.Instance.CurrentCheckPoint = this;
        PlayerPrefs.SetFloat("x", SpawnPlace.transform.position.x);
        PlayerPrefs.SetFloat("y", SpawnPlace.transform.position.y);
        PlayerPrefs.SetFloat("z", SpawnPlace.transform.position.z);
        PlayerManager.Instance.ChangeCheckPoint(SpawnPlace.transform.position, index);
        ChangeColor();
        IsActive = true;
    }

    private void ChangeColor()
    {
        LightColor.color = new Color(0, 1, 0);
        Material[] mats = Indicator.materials;
        mats[2] = IndicatorMat;
        Indicator.materials = mats;
    }


}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolItem : MonoBehaviour
{
    public Action<PoolItem> OnRemoveCallBack;

    public void OnRemove(Action<PoolItem> callback)
    {
        OnRemoveCallBack = callback;
        Remove();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Remove()
    {
        OnRemoveCallBack?.Invoke(this);
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Pool : MonoBehaviour
{
    [SerializeField] private PoolItem Prefab = default;
    [SerializeField, Range(0, 20)] private int DefaultSize = 0;

    private List<PoolItem> Actives = new List<PoolItem>();
    private List<PoolItem> Inactives = new List<PoolItem>();


    private void Start()
    {
        for(int i = 0; i < DefaultSize; i++)
        {
            AddToPool();
        }
    }

    private void AddToPool()
    {
        PoolItem obj = Instantiate(Prefab, transform);
        obj.OnRemove(OnRemoveCallBack);
    }

    public PoolItem GetAPoolObject()
    {
        int index = Inactives.Count - 1;
        if(index < 0)
        {
            AddToPool();
            index = 0;
        }

        PoolItem obj = Inactives[index];
        Inactives.RemoveAt(index);
        Actives.Add(obj);
        obj.Activate();
        return obj;
    }

    public void OnRemoveCallBack(PoolItem obj)
    {
        Actives.Remove(obj);
        Inactives.Add(obj);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance {  get; private set; }
    private Dictionary<string, object> pools = new Dictionary<string, object>();
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void CreatePool<T>(T prefab, int initCount, int maxCount, Transform parent = null) where T : MonoBehaviour
    {
        string key = prefab.name;

        if(!pools.ContainsKey(key))
        {
            ObjectPool<T> newPool = new ObjectPool<T>(prefab,initCount,maxCount,parent);
            pools.Add(key, newPool);
        }
    }
    public T GetFromPool<T> (T prefab, Transform parent = null) where T : MonoBehaviour
    {
        string key = prefab.name;

        ObjectPool<T> pool = pools[key] as ObjectPool<T>;

        T instance = pool.Dequeue(parent);
        return instance;
    }
    public void ReturnPool<T>(T instance) where T : MonoBehaviour
    {
        string key = instance.gameObject.name.Replace("(Clone)","").Trim();

        if(pools.ContainsKey(key))
        {
            ObjectPool<T> pool = pools[key] as ObjectPool<T>;
            instance.gameObject.SetActive(false);
            instance.transform.SetParent(pool.Root);
            pool.Enqueue(instance);
        }
        else
        {
            Destroy(instance.gameObject);
        }
    }
}

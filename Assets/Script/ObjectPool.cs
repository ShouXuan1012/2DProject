using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private T prefab;
    private Queue<T> poolQueue = new Queue<T>();
    public Transform Root {  get; private set; }
    
    private int maxCount;

    public ObjectPool(T prefab, int initCount, int maxCount, Transform parent = null)
    {
        this.prefab = prefab;
        this.maxCount = maxCount;
        Root = new GameObject(prefab.name + "_pool").transform;

        if(parent != null )
        {
            Root.parent = parent;
            UnityEngine.Object.DontDestroyOnLoad(parent.gameObject);
        }
        else
        {
            UnityEngine.Object.DontDestroyOnLoad(parent.gameObject);   // 자기 자신 유지 (단독일 경우)
        }

        for (int i = 0; i < initCount; i++)
        {
            T instance = CreateInstance();
            Enqueue(instance);
        }
    }

    private T CreateInstance()
    {
        T instance = Object.Instantiate(prefab);
        instance.gameObject.SetActive(false);
        return instance;
        
    }

    public void Enqueue(T instance)
    {
        if (poolQueue.Count >= maxCount)
        {
            Object.Destroy(instance.gameObject);
            return;
        }
        instance.gameObject.SetActive(false);
        instance.transform.SetParent(Root);
        poolQueue.Enqueue(instance);
    }

    public T Dequeue(Transform parent = null)
    {
        if (poolQueue.Count == 0)
        {
            return null;
        }

        T instance = poolQueue.Dequeue();
        instance.gameObject.SetActive(true);

        if (parent != null) 
        {
            instance.transform.SetParent(parent);
        }
        else
        {
            instance.transform.SetParent(null);
        }
        return instance;
    }
}

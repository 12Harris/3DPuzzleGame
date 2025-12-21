// ============================================================================
// OBJECT POOLING SYSTEM
// ============================================================================
using UnityEngine;
using System.Collections.Generic;

public class ObjectPool<T> where T : Component
{
    private readonly T _prefab;
    private readonly Queue<T> _objects = new Queue<T>();
    private readonly Transform _parent;

    public ObjectPool(T prefab, int initialSize = 10, Transform parent = null)
    {
        _prefab = prefab;
        _parent = parent;
        
        for (int i = 0; i < initialSize; i++)
        {
            CreateNewObject();
        }
    }

    private T CreateNewObject()
    {
        T obj = Object.Instantiate(_prefab, _parent);
        obj.gameObject.SetActive(false);
        _objects.Enqueue(obj);
        return obj;
    }

    public T Get()
    {
        T obj = _objects.Count > 0 ? _objects.Dequeue() : CreateNewObject();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        _objects.Enqueue(obj);
    }
}

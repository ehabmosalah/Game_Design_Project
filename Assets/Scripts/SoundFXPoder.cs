using UnityEngine;
using System.Collections.Generic;

public class SoundFXPoder : MonoBehaviour
{
    public static SoundFXPoder current; // Singleton instance

    [Header("Pool Settings")]
    public GameObject pooledObjectPrefab; // The prefab to pool
    public int pooledAmount = 10; // Initial pool size
    public bool willGrow = true; // Allow pool to grow if needed

    private List<GameObject> pooledObjects;

    void Awake()
    {
        current = this; // Set singleton instance
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();

        // Create initial pool
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = Instantiate(pooledObjectPrefab);
            obj.transform.SetParent(transform); // Optional: organize in hierarchy
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    // Get an available pooled object
    public GameObject GetPooledObject()
    {
        // Search for inactive object
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                pooledObjects[i].SetActive(true);
                return pooledObjects[i];
            }
        }

        // If no inactive object found and pool can grow
        if (willGrow)
        {
            GameObject obj = Instantiate(pooledObjectPrefab);
            obj.transform.SetParent(transform); // Optional: organize in hierarchy
            pooledObjects.Add(obj);
            obj.SetActive(true);
            return obj;
        }

        return null; // No object available
    }

    // Return an object to the pool (optional helper method)
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}
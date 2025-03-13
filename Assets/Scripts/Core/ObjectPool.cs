using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static ObjectPool instance;
    public static ObjectPool Instance => instance;

    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public GameObject SpawnFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        string key = prefab.name;

        if (!poolDictionary.ContainsKey(key))
            poolDictionary[key] = new Queue<GameObject>();

        GameObject objectToSpawn;

        if (poolDictionary[key].Count == 0)
        {
            objectToSpawn = Instantiate(prefab, position, rotation);
            objectToSpawn.name = key;
        }
        else
        {
            objectToSpawn = poolDictionary[key].Dequeue();
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
            objectToSpawn.SetActive(true);
        }

        return objectToSpawn;
    }

    public void ReturnToPool(GameObject obj)
    {
        string key = obj.name;
        if (!poolDictionary.ContainsKey(key))
            poolDictionary[key] = new Queue<GameObject>();

        poolDictionary[key].Enqueue(obj);
        obj.SetActive(false);
    }
}

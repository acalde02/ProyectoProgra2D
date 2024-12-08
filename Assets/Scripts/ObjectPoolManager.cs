using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    private Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreatePool(string key, GameObject prefab, int initialSize)
    {
        if (!pools.ContainsKey(key))
        {
            pools[key] = new Queue<GameObject>();
            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                pools[key].Enqueue(obj);
            }
        }
    }

    public GameObject GetFromPool(string key, Vector3 position, Quaternion rotation)
    {
        if (pools.ContainsKey(key) && pools[key].Count > 0)
        {
            GameObject obj = pools[key].Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);
            return obj;
        }

        Debug.LogWarning($"Pool with key {key} is empty or doesn't exist!");
        return null;
    }

    public void ReturnToPool(string key, GameObject obj)
    {
        obj.SetActive(false);
        pools[key].Enqueue(obj);
    }
}
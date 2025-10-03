using System.Collections.Generic;
using UnityEngine;

public class TargetPool : MonoBehaviour
{
    public static TargetPool Instance;

    [System.Serializable]
    public class PoolConfig
    {
        public string key;
        public GameObject prefab;
        public int size;
    }

    public List<PoolConfig> pools;
    private Dictionary<string, List<GameObject>> poolDict;

    void Awake()
    {
        Instance = this;
        poolDict = new Dictionary<string, List<GameObject>>();

        foreach (var config in pools)
        {
            poolDict[config.key] = new List<GameObject>();

            for (int i = 0; i < config.size; i++)
            {
                // Instantiate and parent to this pool GameObject
                var obj = Instantiate(config.prefab, transform);
                obj.SetActive(false);
                poolDict[config.key].Add(obj);
            }
        }
    }

    public GameObject GetTarget(string key)
    {
        if (!poolDict.ContainsKey(key)) return null;

        foreach (var obj in poolDict[key])
        {
            if (!obj.activeInHierarchy)
                return obj;
        }

        var config = pools.Find(p => p.key == key);
        if (config != null)
        {
            var obj = Instantiate(config.prefab, transform);
            obj.SetActive(false);
            poolDict[key].Add(obj);
            return obj;
        }

        return null;
    }
}

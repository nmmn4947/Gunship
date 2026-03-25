using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject Object;
    //[SerializeField] private int InitialCount;
    public List<GameObject> Pool = new List<GameObject>();
    
    protected GameObject AddNewInstance()
    {
        GameObject instance = Instantiate(Object);
        instance.SetActive(false);
        Pool.Add(instance);
    
        return instance;
    }
    
    public GameObject GetAvailable()
    {
        foreach(var obj in Pool)
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        
        GameObject newObj = AddNewInstance();
        newObj.SetActive(true);
        return newObj;
    }

    public GameObject GetRandomActiveObject()
    {
        List<int> randomIndex = new List<int>();
        for (int i = 0; i < Pool.Count; i++)
        {
            randomIndex.Add(i);
        }
        
        while (randomIndex.Count > 0)
        {
            int rand = UnityEngine.Random.Range(0, randomIndex.Count);
            if (Pool[randomIndex[rand]].activeInHierarchy)
            {
                return Pool[randomIndex[rand]];
            }
            else
            {
                randomIndex.RemoveAt(rand);
            }
        }
        
        return null;
    }

    protected void DeletePool()
    {
        foreach (var obj in Pool)
        {
            Destroy(obj);
        }
        Pool.Clear();
        Pool = null;
    }
    
    void OnDestroy()
    {
        //DeletePool();
        Pool.Clear();
        Pool = null;
    }
}

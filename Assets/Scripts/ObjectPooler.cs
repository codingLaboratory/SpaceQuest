using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 5;
    private List<GameObject> pool;

    void Start()
    {
        CreatePool();
    }

    private void CreatePool(){
        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++){
            CreateNewObject();
        }
    }

    private GameObject CreateNewObject(){
        GameObject obj = Instantiate(prefab, transform);
        obj.SetActive(false);
        pool.Add(obj);
        return obj;
    }

    public GameObject GetPooledObject(){
        foreach(GameObject obj in pool){
            if (!obj.activeSelf){
                return obj;
            }
        }
        return CreateNewObject();
    }

}

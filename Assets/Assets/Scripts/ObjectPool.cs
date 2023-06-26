using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    public List<GameObject> pooledObjects; // danh sách đối tượng được lưu trữ
    public GameObject objectToPool; // đối tượng ban đầu được đưa vào Pool
    public int amountToPool; // số lượng đối tượng được lưu trữ

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject() // hàm lấy đối tượng Pool và kiểm tra
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy) // nếu đối tượng không được sử dụng 
            {
                return pooledObjects[i];
            }
        }

        // nếu không có đối tượng nào khả dụng thì tạo thêm mới
        GameObject obj = (GameObject)Instantiate(objectToPool);
        obj.SetActive(false);
        pooledObjects.Add(obj);
        return obj;
    }
}

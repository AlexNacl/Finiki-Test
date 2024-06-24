using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour
{
    public static Pooler Instance;

    [Serializable]
    public struct ObjectInfo
    {
        public enum ObjectType
        {
            COIN,
            BLOCKER,
        }

        public ObjectType Type;
        public GameObject Prefab;
        public int StartCount;
    }
    
    [SerializeField]
    private List<ObjectInfo> objectInfo;

    private Dictionary<ObjectInfo.ObjectType, Pool> pools;

    private void Awake() {
        if (Instance == null) 
            Instance = this;

        InitPooler();
    }

    private void InitPooler() {
        pools = new Dictionary<ObjectInfo.ObjectType, Pool>();
        
        var emptyObject = new GameObject();

        foreach (var obj in objectInfo) {
            var container = Instantiate(emptyObject, transform, false);
            container.name = obj.Type.ToString();

            pools[obj.Type] = new Pool(container.transform);

            for (int i = 0; i < obj.StartCount; i++) 
            {
                var newGameObject = InstantiateObject(obj.Type, container.transform);
                pools[obj.Type].Objects.Enqueue(newGameObject);
            }

            Destroy(emptyObject);
        }
    }

    public GameObject InstantiateObject(ObjectInfo.ObjectType type, Transform parent) 
    {
        var newGameObject = Instantiate(objectInfo.Find(obj => obj.Type == type).Prefab, parent);
        newGameObject.SetActive(false);
        return newGameObject;
    }

    public GameObject GetObject(ObjectInfo.ObjectType type)
    {
        var obj = pools[type].Objects.Count > 0 
            ? pools[type].Objects.Dequeue()
            : InstantiateObject(type, pools[type].Container);

        obj.SetActive(true);

        return obj;
    }

    public void DestroyObject(GameObject obj)
    {
        pools[obj.GetComponent<IPooledObject>().Type].Objects.Enqueue(obj);
        obj.transform.SetParent(pools[obj.GetComponent<IPooledObject>().Type].Container);
        obj.SetActive(false);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem : MonoBehaviour, IPooledObject
{
    public Pooler.ObjectInfo.ObjectType Type => type;

    [SerializeField]
    public Pooler.ObjectInfo.ObjectType type;

    [SerializeField]
    public float rotationSpeed = 20;

    public void OnCreate(Vector3 position, Quaternion rotation) 
    {
        transform.position = position;
        transform.rotation = rotation;
    }

    void Start()
    {
        if (type == Pooler.ObjectInfo.ObjectType.COIN)
            rotationSpeed += Random.Range(0, rotationSpeed / 2f);
    }

    public void Reset()
    {
        Pooler.Instance.DestroyObject(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if (type == Pooler.ObjectInfo.ObjectType.BLOCKER && other.gameObject.tag == "Player") 
        {
            GameEvents.current.EndGame();
        }
        if (type == Pooler.ObjectInfo.ObjectType.COIN && other.gameObject.tag == "Player") 
        {
            GameEvents.current.CoinCollect();
            Reset();
        }
        
    }

    void Update() {
        if (type == Pooler.ObjectInfo.ObjectType.COIN) 
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
    }
}
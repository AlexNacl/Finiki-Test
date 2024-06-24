using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public GameObject RoadPrefab;
    [SerializeField]
    private Pooler.ObjectInfo.ObjectType coinType;
    [SerializeField]
    private Pooler.ObjectInfo.ObjectType blockerType;

    private List<GameObject> roads = new List<GameObject>();
    private float speed = 0;
    public float maxSpeed = 10f;
    public int maxRoadCount = 10;

    /*void Start()
    {
        ResetLevel();
        StartLevel();
    }*/

    public void StartLevel() {
        speed = maxSpeed;
        CreateRoad();
    }

    private void CreateRoad() {
        for (int i = 0; i < maxRoadCount; i++) {
            Vector3 position = new Vector3(0, 0, 10 * (maxRoadCount - i));
            GameObject road = Instantiate(RoadPrefab, position, Quaternion.identity);
            road.transform.SetParent(transform);
            roads.Add(road);
        }
    }

    private void generateItems(GameObject road) {
        var positions = road.GetComponent<Road>().positions; //TODO сохранять позиции

        foreach (Transform position in positions.transform)
        {
            int children = position.childCount;
            for (int i = 0; i < children; i++)
            {
                position.GetChild(i).gameObject.GetComponent<GameItem>().Reset();
            }
        }

        int posIndex = Random.Range(0, positions.transform.childCount);
        var itemPosition = positions.transform.GetChild(posIndex);

        if (itemPosition.transform.childCount == 0) {
            Pooler.ObjectInfo.ObjectType type = Random.Range(0, 10) > 7 ? blockerType : coinType;

            GameObject item = Pooler.Instance.GetObject(type);
            item.transform.position = itemPosition.position;
            item.transform.SetParent(itemPosition);
        }
    }

    public void ResetLevel() {
        foreach (GameObject road in roads) {
            roads.Remove(road);
            Destroy(road);
        }
        speed = 0;
    }


    void Update()
    {
        if (speed == 0 || GameManager.current.gameEnded) return;

        for (int i = roads.Count - 1; i > 0; i--) {
            GameObject road = roads[i];
            if (road.transform.position.z < -10) {
                roads.Remove(road);
                float lastPosition = roads[roads.Count - 1].transform.position.z;
                road.transform.position = new Vector3(0, 0, lastPosition + 10);
                generateItems(road);
                roads.Add(road);
            }
            road.transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
        }
    }
}

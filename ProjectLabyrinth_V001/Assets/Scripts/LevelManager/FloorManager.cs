using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public List<GameObject> floorList = new List<GameObject>();
    public List<Floor> floorTPs = new List<Floor>();        //This is a script with an interface attached, all it does is hold teleport location info, maybe will include other info over time
    public int floorCount = -1;
    private int maxNumOfFloors = 5;
    [SerializeField] private GameObject spawnPoints;

    void Start()
    {
        GetFloorList();
    }

    void GetFloorList()
    {
        for (var count = 0; count < maxNumOfFloors; count++)
        {
            GameObject currentFloor = spawnPoints.transform.GetChild(count).GetChild(0).gameObject;
            Debug.Log("CURRENT FLOOR : " + currentFloor + "=========================================/");
            floorList.Add(currentFloor);
        }
        SetTeleportPoints();
    }

    //Maybe use scriptable objects instead
    void SetTeleportPoints()
    {
        for (var i=0; i < (floorList.Count); i++)
        {
            Floor floorTP = new Floor();
            var point1 = floorList[i].transform.GetChild(0).gameObject;
            var point2 = floorList[i].transform.GetChild(1).gameObject;
            floorTP.GenerateFloorTPs(point1, point2);
            floorTPs.Add(floorTP);
        }

        InstantiateTPPoints();
    }

    
    void InstantiateTPPoints()
    {
        GameObject teleporterPrefab = Resources.Load("Prefabs/MapGenPrefabs/Teleporter") as GameObject;
        GameObject endTeleportPrefab = Resources.Load("Prefabs/MapGenPrefabs/EndTeleporter") as GameObject;
        
        var count = 0;
        foreach(GameObject floor in floorList)
        {
            Instantiate(teleporterPrefab, floorTPs[count]._start.transform.position + new Vector3(0, 0, 0), Quaternion.identity, floor.transform.GetChild(0).transform);
            Instantiate(endTeleportPrefab, floorTPs[count]._tp1.transform.position + new Vector3(0, 0, 0), Quaternion.identity, floor.transform.GetChild(1).transform);
            
            count++;
        }
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public List<GameObject> floorList = new List<GameObject>();
    public List<Floor> floorTPs = new List<Floor>();        //This is a script with an interface attached, all it does is hold teleport location info, maybe will include other info over time
    public int floorCount = -1;
    private int maxNumOfFloors = 5;

    public List<GameObject> floorSpawnPoints = new List<GameObject>();

    void Awake()
    {
        PopulateFields();
    }
    // Start is called before the first frame update

    private int GetFloorCount()
    {
        if (floorCount == -1)
        {
            Object[] floors = Resources.LoadAll("Prefabs/Level_1");
            floorCount = floors.Length;
            Resources.UnloadUnusedAssets();
        }
        return floorCount;
       
    }

    void PopulateFields()
    {
        GameObject spawnPoints = GameObject.Find("SpawnPoints");
        //GetFloorCount();
        for (var count = 0; count < maxNumOfFloors; count++)
        {
            var randomNum = Random.Range(1, GetFloorCount());  //Not in use yet
            floorList.Add(Resources.Load("Prefabs/Level_1/Floor" + (count+1) +"/Level1_" + (count+1)) as GameObject);
            floorSpawnPoints.Add(spawnPoints.transform.GetChild(count).gameObject);
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
            //SetTag(point1, point2, i);
            floorTPs.Add(floorTP);
        }

        InstantiatePrefabs();
    }

    /*
    void SetTag(GameObject point1, GameObject point2, int iterator)
    {
        if(iterator == 0)
        {
            point1.tag = "Start";
            point2.tag = "Tp" + (iterator+1);
        }
        else
        {
            point1.tag = "Tp" + (iterator + 1);
            point2.tag = "Tp" + (iterator + 2);
        }

    }
    */

    void InstantiatePrefabs()
    {
        GameObject teleporterPrefab = Resources.Load("Prefabs/MapGenPrefabs/Teleporter") as GameObject;
        var count = 0;
        var moveTpToFloor = 50;
        foreach(var floor in floorList)
        {
            Instantiate(floor, floorSpawnPoints[count].transform);


            Instantiate(teleporterPrefab, floorTPs[count]._start.transform.position + new Vector3(0, moveTpToFloor, 0), Quaternion.identity, floorSpawnPoints[count].transform.GetChild(0).GetChild(0).transform);
            Instantiate(teleporterPrefab, floorTPs[count]._tp1.transform.position + new Vector3(0, moveTpToFloor, 0), Quaternion.identity, floorSpawnPoints[count].transform.GetChild(0).GetChild(1).transform);

            moveTpToFloor -= 25;
            count++;
        }
    }
}

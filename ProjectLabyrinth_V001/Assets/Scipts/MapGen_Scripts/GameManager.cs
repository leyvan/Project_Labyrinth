using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int numOfRooms = 5;

    [SerializeField]
    private float maxRange = 500;

    [SerializeField]
    private float minRange = 0;

    [SerializeField]
    private float count = 100f;

    [SerializeField]
    private Vector3 randPos = new Vector3(0,0,0);

    [SerializeField]
    private GameObject[] rooms;

    [SerializeField]
    private List<GameObject> roomList = new List<GameObject>();

    [SerializeField]
    List<Vector3> prevPosList = new List<Vector3>();

    private Collider[] neighbours;

    bool spawning = false;
    
    public float radius = 3f; // show penetration into the colliders located inside a sphere of this radius
    public int maxNeighbours = 16; // maximum amount of neighbours visualised

    // Start is called before the first frame update
    void Start()
    {
        neighbours = new Collider[maxNeighbours];
        rooms = Resources.LoadAll("Prefabs/Tiles").Cast<GameObject>().ToArray();        //An array of all my tile prefabs, gets ref to them thru editor path

        foreach (GameObject room in rooms)
        {
            Debug.Log(room);
            roomList.Add(room);
        }

        DungeonGenerator(numOfRooms);
    }

    public void DungeonGenerator(int numberOfRooms)
    {
        for (int i = 0; i < numberOfRooms; i++)
        {
            randPos = new Vector3(Random.Range(minRange, maxRange), 0, Random.Range(minRange, maxRange));
            var rng = Random.Range(0, roomList.Count - 1);


            var count = Physics.OverlapSphere(randPos, radius).Length;

            if(count <= 0)
            {
                Instantiate(roomList[rng], randPos, Quaternion.identity);
            }
            else
            {
                var newPos = randPos * 10;
                Instantiate(roomList[rng], newPos, Quaternion.identity);

                //Instead of changing its position I will probably just not spawn object---
            }
            
            

        }
    }

}

        
    



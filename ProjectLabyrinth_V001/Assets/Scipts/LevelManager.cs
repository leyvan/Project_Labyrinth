using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject floor;

    public int distanceX = 0;
    public int distanceZ = 0;
   

    public Vector3 mapSize = new Vector3(2f, 0f, 2f);

    public NavMeshSurface surface;

    //define a list
    public List<GameObject> tileSets = new List<GameObject>();
    public List<Vector3> spawnPoints = new List<Vector3>();
    private GameObject[] tiles;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;     //Confines the cursor to game screen
        Cursor.visible = false;

        tiles = Resources.LoadAll("Prefabs/Tiles").Cast<GameObject>().ToArray();        //An array of all my tile prefabs, gets ref to them thru editor path

        foreach (GameObject tile in tiles)
        {
            Debug.Log(tile);
            tileSets.Add(tile);
        }


        //GenerateMaze();

        //Adds to the spawn points list, uses ref from spawn locations in editor
        /*
        
        */

        //Adds to the tileSet list, uses the array of prefabs previously made

        //Loop through the number of tiles need to be placed, then instantiates each on to its respective spawn point
        /*
        for (int i = 0; i < (mapSize.x * mapSize.z); i++)
        {

        }
        */

        //Bakes the navmesh
        //surface.BuildNavMesh();

    }

    void GenerateMaze()
    {
        float row = mapSize.x;
        float column = mapSize.z;
        int counter = 0;

        for (int x = 0; x < row; x++)
        {
            for (int z = 0; z < column; z++)
            {
                Instantiate(floor, transform.position + (new Vector3(distanceX, 0, distanceZ)), transform.rotation);
                spawnPoints.Add(floor.transform.position + new Vector3(distanceX, 0, distanceZ));

                /*
            if(x == 1)      //when first row
                Instantiate
            if(z == 1)          //when first column
            if(x == row)            //when last row
            if(z == column)             //when last column

            if(z != 1 && x != 1)    //In the center

            if(lastTag == leftCorner2Doors)
            if(lastTag == rightCorner2Doors)

            */

                Debug.Log("Yo");
                distanceZ += 36;
            }

            distanceX += -36;
            distanceZ = 0;
            counter++;
        }

        for (int i = 0; i < (mapSize.x * mapSize.z); i++)
        {
 
            Instantiate(tileSets[i], spawnPoints[i], Quaternion.identity);
        }
    }

}


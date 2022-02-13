using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class LevelStart : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject map;
    public GameObject player;
    private Transform spawnRoom;

    private Vector3 offset = new Vector3(0f, 8f, 5f);

    void Start()
    {
        if(map.transform.childCount > 0)
        {
            spawnRoom = map.transform.GetChild(0);
        }

        player.transform.position = spawnRoom.position;
        

    }

}
